using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Data.Models.HomePage;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.HomePageDTOS;

namespace BLL.Service
{
    public class HomeVideoSectionService(IHomeVideoSectionRepository _homeVideoSection ,IMapper _mapper) : IHomeVideoSectionService
    {
        public async Task<HomeVideoSectionDTO> GetHomeVideoSectionAsync()
        {
            var homeVideoSection = await _homeVideoSection.GetVideoUrlAsync();
            if (homeVideoSection == null)
                return null;
            var homeVideoSectionDTO = _mapper.Map<HomeVideoSectionDTO>(homeVideoSection);
            return homeVideoSectionDTO;
        }
        public async Task<bool> UpdateHomeVideoSectionAsync(UpdateHomeVideoSectionDTO updateHomeVideoSectionDTO)
        {
            var homeVideoSection = new HomeVideoSection
            {
                Title = updateHomeVideoSectionDTO.Title,
                Description = updateHomeVideoSectionDTO.Description,
                VideoUrl = updateHomeVideoSectionDTO.VideoUrl
            };
            var updatedSection = await _homeVideoSection.UpdateVideoUrlAsync(homeVideoSection);
            return updatedSection != null;
        }
    }
}
