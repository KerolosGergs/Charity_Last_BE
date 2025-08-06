using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using DAL.Data;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class NewsImageRepository : INewsImageRepository
    {
        private readonly ApplicationDbContext _context;

        public NewsImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NewsImage> GetByIdAsync(int id)
        {
            return await _context.NewsImages.FindAsync(id);
        }

        public async Task<IEnumerable<NewsImage>> GetByNewsItemIdAsync(int newsItemId)
        {
            return await _context.NewsImages
                .Where(img => img.NewsItemId == newsItemId)
                .ToListAsync();
        }

        public async Task<NewsImage> AddAsync(NewsImage newsImage)
        {
            _context.NewsImages.Add(newsImage);
            await _context.SaveChangesAsync();
            return newsImage;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var newsImage = await GetByIdAsync(id);
            if (newsImage == null)
                return false;

            _context.NewsImages.Remove(newsImage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByNewsItemIdAsync(int newsItemId)
        {
            var images = await GetByNewsItemIdAsync(newsItemId);
            if (images.Any())
            {
                _context.NewsImages.RemoveRange(images);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<NewsImage> GetByImageUrlAsync(string imageUrl)
        {
            return await _context.NewsImages
                .FirstOrDefaultAsync(img => img.ImageUrl == imageUrl);
        }
    }
}
