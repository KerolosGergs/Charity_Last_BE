using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.HomePageDTOS;

namespace BLL.ServiceAbstraction
{
    public interface ITrendSectionService
    {
        public Task<TrendSectionDTO> GetTrendingAsync();
        public Task<bool> UpdateTrendSection(UpdateTrendSectionDTO trendSectionDTO);
    }
}
