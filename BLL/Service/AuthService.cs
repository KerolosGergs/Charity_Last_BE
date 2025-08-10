using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using Shared.DTOS.AuthDTO;
using BLL.ServiceAbstraction;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories.RepositoryIntrfaces;

namespace BLL.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAdminRepository _adminRepository;
        private readonly IAdvisorRepository _advisorRepository;
        private readonly IEmailService _emailService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IConfiguration configuration,
            IAdminRepository adminRepository,
            IAdvisorRepository advisorRepository,
            IEmailService emailService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
            _adminRepository = adminRepository;
            _advisorRepository = advisorRepository;
            _emailService = emailService;
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        private string TranslateIdentityError(IdentityError error)
        {
            return error.Code switch
            {
                "DuplicateUserName" or "DuplicateEmail" => "البريد الإلكتروني مستخدم بالفعل",
                "InvalidEmail" => "البريد الإلكتروني غير صالح",
                "PasswordTooShort" => "كلمة المرور قصيرة جدًا، يجب أن تكون على الأقل 8 أحرف",
                "PasswordRequiresNonAlphanumeric" => "كلمة المرور يجب أن تحتوي على رمز واحد على الأقل",
                "PasswordRequiresDigit" => "كلمة المرور يجب أن تحتوي على رقم واحد على الأقل",
                "PasswordRequiresUpper" => "كلمة المرور يجب أن تحتوي على حرف كبير واحد على الأقل",
                "PasswordRequiresLower" => "كلمة المرور يجب أن تحتوي على حرف صغير واحد على الأقل",
                _ => error.Description
            };
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var siginCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"]!)),
                signingCredentials: siginCreds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponseDTO> RegisterAdminAsync(RegisterAdminDTO dto)
        {
            if (await IsEmailExistAsync(dto.Email))
                throw new Exception("البريد الإلكتروني مستخدم بالفعل");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join("، ", result.Errors.Select(TranslateIdentityError)));

            await _userManager.AddToRoleAsync(user, "Admin");

            var admin = new Admin
            {
                UserId = user.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true
            };

            await _adminRepository.AddAsync(admin);

            var token = await CreateTokenAsync(user);

            return new AuthResponseDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = "Admin",
                Token = token,
                Success = true,
                Message = "تم إنشاء حساب المدير بنجاح"
            };
        }

        public async Task<AuthResponseDTO> RegisterAdvisorAsync(RegisterAdvisorDTO dto)
        {
            if (await IsEmailExistAsync(dto.Email))
                throw new Exception("البريد الإلكتروني مستخدم بالفعل");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join("، ", result.Errors.Select(TranslateIdentityError)));

            await _userManager.AddToRoleAsync(user, "Advisor");

            var advisor = new Advisor
            {
                UserId = user.Id,
                FullName = dto.FullName,
                Specialty = dto.Specialty,
                Description = dto.Description,
                ZoomRoomUrl = dto.ZoomRoomUrl,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                IsActive = true
            };

            await _advisorRepository.AddAsync(advisor);

            var token = await CreateTokenAsync(user);

            return new AuthResponseDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = "Advisor",
                Token = token,
                Success = true,
                Message = "تم إنشاء حساب المستشار بنجاح"
            };
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                FullName = dto.FullName,
                Address = dto.Address,
                IsActive = true,
                ProfilePictureUrl = dto.ProfilePictureUrl,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join("، ", result.Errors.Select(TranslateIdentityError)));

            await _userManager.AddToRoleAsync(user, "User");

            var token = await CreateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "User",
                Token = token,
                Success = true,
                Message = "تم إنشاء الحساب بنجاح",
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = new CurrentUserDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    Role = roles.ToList(),
                    IsActive = user.IsActive
                }
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var normalizedEmail = dto.Email?.Trim().ToUpperInvariant();
            var user = await _userManager.Users
                .Include(u => u.Advisor)
                .Include(u => u.Admin)
                .Include(u => u.Mediation)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

            if (user == null)
                throw new Exception("البريد الإلكتروني غير مسجل لدينا");

            if (!user.IsActive)
                throw new Exception("الحساب غير مفعل");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordCorrect)
                throw new Exception("كلمة المرور غير صحيحة");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";
            int? roleId = role switch
            {
                "Advisor" => user.Advisor?.Id,
                "Admin" => user.Admin?.Id,
                "Mediation" => user.Mediation?.Id,
                _ => null
            };

            var token = await CreateTokenAsync(user);

            return new AuthResponseDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = role,
                RoleId = roleId,
                Token = token,
                Success = true,
                Message = "تم تسجيل الدخول بنجاح",
                ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"]!)),
                User = new CurrentUserDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = roles.ToList(),
                    IsActive = user.IsActive
                }
            };
        }

        public async Task<CurrentUserDTO> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new CurrentUserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.ToList(),
                IsActive = user.IsActive
            };
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return true;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_configuration["FrontendUrl"]}/reset-password?email={WebUtility.UrlEncode(email)}&token={WebUtility.UrlEncode(token)}";

            string subject = "طلب إعادة تعيين كلمة المرور";
            string body = $@"
                <p>مرحباً {user.FullName},</p>
                <p>لقد تلقينا طلباً لإعادة تعيين كلمة المرور الخاصة بك. اضغط على الرابط أدناه لتعيين كلمة مرور جديدة:</p>
                <p><a href='{resetLink}'>إعادة تعيين كلمة المرور</a></p>
                <p>إذا لم تطلب هذا، يرجى تجاهل هذا البريد.</p>";

            await _emailService.SendEmailAsync(email, subject, body);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO dto, string token)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("البريد الإلكتروني غير مسجل");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
            if (!resetPassResult.Succeeded)
                throw new Exception(string.Join("، ", resetPassResult.Errors.Select(TranslateIdentityError)));

            return true;
        }
    }
}
