using DAL.Data;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.DynamicPage;

namespace DAL.Repositories
{
    public class DynamicPageRepository : IDynamicPageRepository
    {
        private readonly ApplicationDbContext _context;

        public DynamicPageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DynamicPage>> GetAllAsync()
        {
            return await _context.DynamicPages
                .Include(dp => dp.Items.OrderBy(item => item.Order))

                .OrderByDescending(dp => dp.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DynamicPage>> GetAllActiveAsync()
        {
            return await _context.DynamicPages
                .Where(dp => dp.IsActive)
                .Include(dp => dp.Items.OrderBy(item => item.Order))
                .OrderByDescending(dp => dp.CreatedAt)
                .ToListAsync();
        }

        public async Task<DynamicPage?> GetByIdAsync(int id)
        {
            return await _context.DynamicPages
                .Include(dp => dp.Items.OrderBy(item => item.Order))
                .FirstOrDefaultAsync(dp => dp.Id == id);
        }

        public async Task<DynamicPage?> GetBySlugAsync(string slug)
        {
            return await _context.DynamicPages
                .Where(dp => dp.IsActive && dp.Slug == slug)
                .Include(dp => dp.Items.OrderBy(item => item.Order))
                .FirstOrDefaultAsync();
        }

        public async Task<DynamicPage> CreateAsync(DynamicPage dynamicPage)
        {
            _context.DynamicPages.Add(dynamicPage);
            await _context.SaveChangesAsync();
            return dynamicPage;
        }

        public async Task<DynamicPage> UpdateAsync(DynamicPage dynamicPage)
        {
            dynamicPage.UpdatedAt = DateTime.UtcNow;
            _context.DynamicPages.Update(dynamicPage);
            await _context.SaveChangesAsync();
            return dynamicPage;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dynamicPage = await _context.DynamicPages.FindAsync(id);
            if (dynamicPage == null)
                return false;

            _context.DynamicPages.Remove(dynamicPage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DynamicPages.AnyAsync(dp => dp.Id == id);
        }

        public async Task<bool> ExistsBySlugAsync(string slug)
        {
            return await _context.DynamicPages.AnyAsync(dp => dp.Slug == slug);
        }

        public async Task<IEnumerable<DynamicPageListDto>> GetListAsync()
        {
            return await _context.DynamicPages
                .Select(dp => new DynamicPageListDto
                {
                    Id = dp.Id,
                    PageName = dp.PageName,
                    Description = dp.Description,
                    Slug = dp.Slug,
                    IsActive = dp.IsActive,
                    CreatedAt = dp.CreatedAt,
                    UpdatedAt = dp.UpdatedAt,
                    ItemsCount = dp.Items.Count,
                })
                .OrderByDescending(dp => dp.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetItemsCountAsync(int pageId)
        {
            return await _context.DynamicPageItems
                .Where(item => item.DynamicPageId == pageId)
                .CountAsync();
        }
    }
}