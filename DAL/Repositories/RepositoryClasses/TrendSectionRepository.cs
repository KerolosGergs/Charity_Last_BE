using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Data.Models.HomePage;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RepositoryClasses
{
    public class TrendSectionRepository(ApplicationDbContext _dbContext) : ITrendSectionRepository
    {
        public async Task<TrendSection> GetTrendingVideosAsync()
        {
            return await _dbContext.TrendSections.FirstOrDefaultAsync();
        }
        public async Task<TrendSection> UpdateTrendSection(TrendSection trendSection)
        {
            if (trendSection == null)
                throw new ArgumentNullException(nameof(trendSection), "TrendingVideos cannot be null");
            var existing = await _dbContext.TrendSections.FirstOrDefaultAsync();
            if (existing == null)
                throw new InvalidOperationException("TrendingVideos not found");
            existing.Title = trendSection.Title;
            existing.Description = trendSection.Description;
            existing.ButtonText= trendSection.ButtonText;
            existing.ButtonUrl = trendSection.ButtonUrl;
            await _dbContext.SaveChangesAsync();
            return existing;
        }
    }
}
