using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class NewsItemRepository : GenericRepository<NewsItem>, INewsItemRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<NewsItem>> GetActiveNewsAsync()
        {
            return await _context.NewsItems
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();
        }

        public async Task<List<NewsItem>> GetByCategoryAsync(string category)
        {
            return await _context.NewsItems
                .Where(n => n.Category == category && n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();
        }

        public async Task<List<NewsItem>> GetPublishedNewsAsync()
        {
            return await _context.NewsItems
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();
        }

        public async Task<List<NewsItem>> GetByAuthorAsync(string author)
        {
            return await _context.NewsItems
                .Where(n => n.Author == author && n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalViewCountAsync()
        {
            return await _context.NewsItems
                .SumAsync(n => n.ViewCount);
        }

        public async Task<List<NewsItem>> GetMostViewedNewsAsync(int count)
        {
            return await _context.NewsItems
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.ViewCount)
                .Take(count)
                .ToListAsync();
        }
        public async Task<NewsItem> GetByIdWithImagesAsync(int id)
        {
            return await _context.NewsItems
                .Include(n => n.Images)
                .FirstOrDefaultAsync(n => n.Id == id);
        }
        public async Task<List<NewsItem>> GetAllWithImagesAsync()
        {
            return await _context.NewsItems
                .Include(n => n.Images)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
        public async Task<List<NewsItem>> GetActiveNewsWithImagesAsync()
        {
            return await _context.NewsItems
                .Where(n => n.IsPublished)
                .Include(n => n.Images)
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();
        }
        public async Task<List<NewsItem>> GetByCategoryWithImagesAsync(string category)
        {
            return await _context.NewsItems
                .Where(n => n.Category == category && n.IsPublished)
                .Include(n => n.Images)
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();
        }
    }
} 