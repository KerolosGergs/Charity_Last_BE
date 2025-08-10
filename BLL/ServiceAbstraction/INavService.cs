using DAL.Data.Models;
using Shared.DTOS.NavItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface INavItemService
    {
        Task<List<NavItems>> GetAllNavItemsAsync();
        Task<NavItemDto?> GetNavItemByIdAsync(int id);
        Task AddNavItemAsync(NavItems navItem);
        Task<bool> UpdateNavItemAsync(int id, NavItems navItem);
        Task<bool> DeleteNavItemAsync(int id);

        // Pages
        Task<List<Pages>> GetPagesAsync(int navItemId);
        Task<bool> AddPageAsync(int navItemId, PageDto page);
        Task<bool> UpdatePageAsync(int pageId, PageDto page);
        Task<bool> DeletePageAsync(int pageId);
    }
}
