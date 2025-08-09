using DAL.Data;
using DAL.Data.Models;
using DAL.Repositories.GenericRepositries;
using DAL.Repositries.GenericRepositries;
using Shared.DTOS.NavItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface INavItemRepository 
    {
        // NavItems (DTO Reads)
        Task<List<NavItems>> GetAllNavItemsAsync();
        Task<NavItemDto?> GetNavItemByIdAsync(int id);

        // NavItems (Entity Writes)
        Task AddNavItemAsync(NavItems navItem);
        Task UpdateNavItemAsync(NavItems navItem);
        Task DeleteNavItemAsync(int id);

        // Pages (DTO Reads)
        Task<List<Pages>> GetPagesByNavItemIdAsync(int navItemId);

        // Pages (Entity Writes)
        Task AddPageAsync(Pages page);
        Task UpdatePageAsync(Pages page);
        Task DeletePageAsync(int pageId);

        // Helper (Entity lookups when needed)
        Task<NavItems?> FindNavItemEntityAsync(int id);
        Task<Pages?> FindPageEntityAsync(int id);
    }
}