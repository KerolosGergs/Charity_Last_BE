using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.ServiceAbstraction;
using BLL.Services.FileService;
using DAL.Data.Models.HomePage;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.HomePageDTOS;

namespace BLL.Service
{
    public class TrendSectionService(ITrendSectionRepository _sectionRepository,IMapper _mapper) : ITrendSectionService
    {
        public async Task<TrendSectionDTO> GetTrendingAsync()
        {
            var trendingVideos = await _sectionRepository.GetTrendingVideosAsync();
            if (trendingVideos == null)
                return null;
            var trendingVideosDTO = _mapper.Map<TrendSectionDTO>(trendingVideos);
            return trendingVideosDTO;
        }
        public async Task<bool> UpdateTrendSection(UpdateTrendSectionDTO trendSectionDTO)
        {
            var existingSection = await _sectionRepository.GetTrendingVideosAsync();
            if (existingSection == null)
               return false;
            existingSection.Description = trendSectionDTO.Description ?? existingSection.Description;
            existingSection.Title = trendSectionDTO.Title ?? existingSection.Title;
            if (trendSectionDTO.ImageUrl != null)
            {
                var fileService = new FileService();
                if (!string.IsNullOrEmpty(existingSection.ImageUrl))
                {
                    fileService.DeleteFile(existingSection.ImageUrl);
                }
                var newImageUrl = await fileService.UploadFileAsync(trendSectionDTO.ImageUrl, "trend-section");
                existingSection.ImageUrl = newImageUrl;
                existingSection.ButtonUrl = trendSectionDTO.ButtonUrl ?? existingSection.ButtonUrl;
                existingSection.ButtonText = trendSectionDTO.ButtonText ?? existingSection.ButtonText;
                await _sectionRepository.UpdateTrendSection(existingSection);
                return true;
            }
            var trendSection = new TrendSection
            {
                Title = trendSectionDTO.Title,
                Description = trendSectionDTO.Description,
            };
            var updatedSection = await _sectionRepository.UpdateTrendSection(trendSection);
            return updatedSection != null;
        }
    }
}
