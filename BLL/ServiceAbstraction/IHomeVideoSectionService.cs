using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.HomePageDTOS;

namespace BLL.ServiceAbstraction
{
    public interface IHomeVideoSectionService
    {
        public Task<HomeVideoSectionDTO> GetHomeVideoSectionAsync();
        public Task<bool> UpdateHomeVideoSectionAsync(UpdateHomeVideoSectionDTO updateHomeVideoSectionDTO);
    }
}
