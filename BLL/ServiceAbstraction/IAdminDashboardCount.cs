using Shared.DTOS.AdminDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface IAdminDashboardCount
    {
        Task<DashboardStatisticsDTO> Count();

    }
}
