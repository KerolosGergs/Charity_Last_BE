// File: DAL/Data/DataSeed/IdentitySeeder.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DAL.Data.DataSeed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;
            var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");
            var cfg = sp.GetRequiredService<IConfiguration>();
            var db = sp.GetRequiredService<ApplicationDbContext>();
            var users = sp.GetRequiredService<UserManager<ApplicationUser>>();
            var roles = sp.GetRequiredService<RoleManager<IdentityRole>>();

            // Read from appsettings / env (with fallback defaults for local dev)
            var roleName = cfg["Seed:Admin:Role"] ?? "Admin";
            var email = cfg["Seed:Admin:Email"] ?? "Admin@gmail.com";
            var userName = cfg["Seed:Admin:UserName"] ?? email;
            var password = cfg["Seed:Admin:Password"] ?? "Demo@123";
            var fullName = cfg["Seed:Admin:FullName"] ?? "System Administrator";
            var phone = cfg["Seed:Admin:PhoneNumber"] ?? "01000000000";

            logger.LogInformation("Seeding Identity… DB: {conn}", db.Database.GetDbConnection().ConnectionString);

            // If you already call Migrate() in Program.cs, you can remove this line.
            await db.Database.MigrateAsync(); // keep as safety for dev/test

            // 1) Ensure role
            if (!await roles.RoleExistsAsync(roleName))
            {
                var r = await roles.CreateAsync(new IdentityRole(roleName));
                if (!r.Succeeded)
                    throw new InvalidOperationException(string.Join("; ", r.Errors.Select(e => e.Description)));
                logger.LogInformation("Role '{role}' created.", roleName);
            }

            // 2) Ensure user
            var user = await users.FindByEmailAsync(email);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    PhoneNumber = phone,
                    PhoneNumberConfirmed = true,
                    FullName = fullName,
                    LockoutEnabled = false,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var create = await users.CreateAsync(user, password);
                if (!create.Succeeded)
                    throw new InvalidOperationException("Failed to create admin user: " +
                        string.Join("; ", create.Errors.Select(e => e.Description)));

                logger.LogInformation("Admin user created: {email}", email);
            }
            else
            {
                // If the stored hash doesn't match our desired seed password, reset it properly
                var matches = await users.CheckPasswordAsync(user, password);
                if (!matches)
                {
                    var token = await users.GeneratePasswordResetTokenAsync(user);
                    var reset = await users.ResetPasswordAsync(user, token, password);
                    if (!reset.Succeeded)
                        throw new InvalidOperationException("Failed to reset admin password: " +
                            string.Join("; ", reset.Errors.Select(e => e.Description)));
                    logger.LogInformation("Admin password reset for {email}", email);
                }

                // Clear lockout if the account was locked by bad attempts
                if (await users.IsLockedOutAsync(user))
                {
                    await users.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                    await users.ResetAccessFailedCountAsync(user);
                    await users.SetLockoutEnabledAsync(user, false);
                    logger.LogInformation("Cleared lockout for {email}", email);
                }
            }

            // 3) Ensure role assignment
            if (!await users.IsInRoleAsync(user, roleName))
            {
                var addRole = await users.AddToRoleAsync(user, roleName);
                if (!addRole.Succeeded)
                    throw new InvalidOperationException("Failed to add admin role: " +
                        string.Join("; ", addRole.Errors.Select(e => e.Description)));
                logger.LogInformation("User {email} added to role '{role}'", email, roleName);
            }

            // 4) Ensure Admin profile row (1:1 with ApplicationUser)
            var hasAdminRow = await db.Admins.AnyAsync(a => a.UserId == user.Id);
            if (!hasAdminRow)
            {
                db.Admins.Add(new Admin
                {
                    UserId = user.Id,
                    FullName = user.FullName ?? fullName,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber ?? phone,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
                await db.SaveChangesAsync();
                logger.LogInformation("Admins row created for user {email}", email);
            }
            else
            {
                logger.LogInformation("Admins row already exists for {email}", email);
            }

            logger.LogInformation("Identity seeding completed.");
        }
    }
}
