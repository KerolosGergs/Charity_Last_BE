using DAL.Data.Models.IdentityModels;
using Shared.DTOS.DynamicPage;

namespace DAL.Repositories
{
    public interface IDynamicPageRepository
    {
        Task<IEnumerable<DynamicPage>> GetAllAsync();
        Task<IEnumerable<DynamicPage>> GetAllActiveAsync();
        Task<DynamicPage?> GetByIdAsync(int id);
        Task<DynamicPage?> GetBySlugAsync(string slug);
        Task<DynamicPage> CreateAsync(DynamicPage dynamicPage);
        Task<DynamicPage> UpdateAsync(DynamicPage dynamicPage);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsBySlugAsync(string slug);
        Task<IEnumerable<DynamicPageListDto>> GetListAsync();
        Task<int> GetItemsCountAsync(int pageId);
    }
} 