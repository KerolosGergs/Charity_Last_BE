using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.ServiceAbstraction;
using BLL.Services.FileService;
using DAL.Repositories.RepositoryClasses;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.HomePageDTOS;

namespace BLL.Service
{
    public class HeroSectionService(IHeroSectionRepository _heroSection,IMapper _mapper) : IHeroSectionService
    {
        public async Task<HeroSectionDTOs> GetHeroSectionAsync()
        {
            var entity = await _heroSection.GetHeroSectionAsync();
            return _mapper.Map<HeroSectionDTOs>(entity);
        }

        public async Task<bool> UpdateHeroSectionAsync(UpdateHeroSectionDTO dto)
        {
            var entity = await _heroSection.GetHeroSectionAsync();
            if (entity == null) return false;

            entity.MainTitle = dto.MainTitle ?? entity.MainTitle;
            entity.Stats1Label = dto.Stats1Label ?? entity.Stats1Label;
            entity.Stats1Value = dto.Stats1Value ?? entity.Stats1Value;
            entity.Stats2Label = dto.Stats2Label ?? entity.Stats2Label;
            entity.Stats2Value = dto.Stats2Value ?? entity.Stats2Value;
            entity.Stats3Label = dto.Stats3Label ?? entity.Stats3Label;
            entity.Stats3Value = dto.Stats3Value ?? entity.Stats3Value;
            entity.Stats4Label = dto.Stats4Label ?? entity.Stats4Label;
            entity.Stats4Value = dto.Stats4Value ?? entity.Stats4Value;

            if (dto.BackgroundImageUrl != null)
            {
                var fileService = new FileService();

                if (!string.IsNullOrEmpty(entity.BackgroundImageUrl))
                {
                    fileService.DeleteFile(entity.BackgroundImageUrl);
                }

                var newImageUrl = await fileService.UploadFileAsync(dto.BackgroundImageUrl, "hero-section");
                entity.BackgroundImageUrl = newImageUrl;
            }

            await _heroSection.UpdateHeroSectionAsync(entity);
            return true;
        }

    }
}
