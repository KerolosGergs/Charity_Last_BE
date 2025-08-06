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
    public class HeroSectionRepository(ApplicationDbContext _dbContext) : IHeroSectionRepository
    {
        public async Task<HeroSection> UpdateHeroSectionAsync(HeroSection heroSection)
        {
            if (heroSection == null)
                throw new ArgumentNullException(nameof(heroSection), "HeroSection cannot be null");

            var existing = await _dbContext.HeroSections.FirstOrDefaultAsync();

            if (existing == null)
                throw new InvalidOperationException("HeroSection not found");


            existing.MainTitle = heroSection.MainTitle;
            existing.BackgroundImageUrl = heroSection.BackgroundImageUrl;
            existing.Stats1Label = heroSection.Stats1Label;
            existing.Stats1Value = heroSection.Stats1Value;
            existing.Stats2Label = heroSection.Stats2Label;
            existing.Stats2Value = heroSection.Stats2Value;
            existing.Stats3Label = heroSection.Stats3Label;
            existing.Stats3Value = heroSection.Stats3Value;
            existing.Stats4Label = heroSection.Stats4Label;
            existing.Stats4Value = heroSection.Stats4Value;

            await _dbContext.SaveChangesAsync();

            return existing;
        }

        public async Task<HeroSection> GetHeroSectionAsync()
        {
            return await _dbContext.HeroSections.FirstOrDefaultAsync();
        }

    }
}
