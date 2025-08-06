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
    public class HomeVideoSectionRepository(ApplicationDbContext _dbContext) : IHomeVideoSectionRepository
    {
        public async Task<HomeVideoSection> GetVideoUrlAsync()
        {
            return await _dbContext.HomeVideoSections.FirstOrDefaultAsync();
        }
        public async Task<HomeVideoSection> UpdateVideoUrlAsync(HomeVideoSection homeVideoSection)
        {
            if (homeVideoSection == null)
                throw new ArgumentNullException(nameof(homeVideoSection), "HomeVideoSection cannot be null");
            var existing = await _dbContext.HomeVideoSections.FirstOrDefaultAsync();
            if (existing == null)
                throw new InvalidOperationException("HomeVideoSection not found");

            existing.VideoUrl = homeVideoSection.VideoUrl;
            existing.Title = homeVideoSection.Title;
            existing.Description = homeVideoSection.Description;
            await _dbContext.SaveChangesAsync();
            return existing;
        }

    }
}
