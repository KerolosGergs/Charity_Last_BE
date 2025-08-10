using Shared.DTOS.ServiceOfferingDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IServiceOfferingService
    {
        Task<ServiceOfferingDTO> GetServiceOfferingAsync();
        Task<ServiceOfferingDTO> GetServiceOfferingAvaliableAsync();
        Task<bool> UpdateTitleAndDescriptionAsync(string title, string description);
        Task<ServiceOfferingDTOItem> AddServiceItemAsync(CreateServiceOfferingDTOItem dto);
        Task<ServiceOfferingDTOItem> UpdateServiceItemAsync(int itemId, UpdateServiceOfferingDTOItem dto);
        Task<bool> DeleteServiceItemAsync(int itemId);
        Task<List<ServiceOfferingDTOItem>> GetServiceItemsAsync();
        Task<int> GetTotalServicesCountAsync();
    }
} 