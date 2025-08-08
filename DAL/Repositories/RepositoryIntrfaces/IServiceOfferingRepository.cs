using DAL.Data.Models;
using DAL.Repositries.GenericRepositries;
using Shared.DTOS.ServiceOfferingDTOs;

namespace DAL.Repositories.RepositoryIntrfaces
{
    public interface IServiceOfferingRepository : IGenericRepository<ServiceOffering>
    {
        Task<ServiceOffering> GetSingleAsync();
        Task<ServiceOffering> GetSingleAvialblyAsync();
        Task<bool> UpdateTitleAndDescriptionAsync(string title, string description);

        Task<List<ServiceOfferingItem>> GetServiceItemsAsync();
        Task<ServiceOfferingItem> GetItemByIdAsync(int id);
        Task AddServiceItemAsync(ServiceOfferingItem item);
        Task UpdateServiceItemAsync(ServiceOfferingItem item);
        Task DeleteServiceItemAsync(ServiceOfferingItem item);
    }

}