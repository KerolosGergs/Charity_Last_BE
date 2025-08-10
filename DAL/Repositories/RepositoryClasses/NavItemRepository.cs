using DAL.Data.Models;
using DAL.Data;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.NavItemDto;

namespace DAL.Repositories.RepositoryClasses
{
    public class NavItemRepository : INavItemRepository
    {
        private readonly ApplicationDbContext _context;
        public NavItemRepository(ApplicationDbContext context) => _context = context;

        // ===== Reads as DTOs =====
        public async Task<List<NavItems>> GetAllNavItemsAsync()
        {
            return await _context.NavigationItems
                .Select(n => new NavItems
                {
                    Id=n.Id,
                    label = n.label,
                    href = n.href,
                    pages = n.pages.Select(p => new Pages
                    {
                        Id =p.Id,
                        subTilte = p.subTilte,  // (أو subTitle لو معدّل الموديل)
                        subLink = p.subLink
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<NavItemDto?> GetNavItemByIdAsync(int id)
        {
            return await _context.NavigationItems
                .Where(n => n.Id == id)
                .Select(n => new NavItemDto
                {
                    label = n.label,
                    href = n.href,
                    pages = n.pages.Select(p => new PageDto
                    {
                        subTilte = p.subTilte,
                        subLink = p.subLink
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        // ===== Writes on Entities =====
        public async Task AddNavItemAsync(NavItems navItem)
        {
            _context.NavigationItems.Add(navItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNavItemAsync(NavItems navItem)
        {
            _context.NavigationItems.Update(navItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNavItemAsync(int id)
        {
            var entity = await _context.NavigationItems.FindAsync(id);
            if (entity is null) return;
            _context.NavigationItems.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Pages>> GetPagesByNavItemIdAsync(int navItemId)
        {
            return await _context.Pages
                .Where(p => p.NavItemsId == navItemId)
                .Select(p => new Pages
                {
                    Id = p.Id,
                    subTilte = p.subTilte,
                    subLink = p.subLink
                })
                .ToListAsync();
        }

        public async Task AddPageAsync(Pages page)
        {
            _context.Pages.Add(page);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePageAsync(Pages page)
        {
            _context.Pages.Update(page);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePageAsync(int pageId)
        {
            var entity = await _context.Pages.FindAsync(pageId);
            if (entity is null) return;
            _context.Pages.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task<NavItems?> FindNavItemEntityAsync(int id)
            => _context.NavigationItems.FirstOrDefaultAsync(n => n.Id == id);

        public Task<Pages?> FindPageEntityAsync(int id)
            => _context.Pages.FirstOrDefaultAsync(p => p.Id == id);
    }
}