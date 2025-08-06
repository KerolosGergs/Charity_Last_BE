using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.HomePageDTOS;

namespace BLL.ServiceAbstraction
{
    public interface IHeroSectionService
    {
        public Task<HeroSectionDTOs> GetHeroSectionAsync();
        public Task<bool> UpdateHeroSectionAsync(UpdateHeroSectionDTO updateHeroSectionDTO);
    }
}
