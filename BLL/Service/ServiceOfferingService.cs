using AutoMapper;
using BLL.ServiceAbstraction;
using BLL.Services.FileService;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.ServiceOfferingDTOs;

public class ServiceOfferingService : IServiceOfferingService
{
    private readonly IServiceOfferingRepository _serviceOfferingRepository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public ServiceOfferingService(IServiceOfferingRepository serviceOfferingRepository, IMapper mapper,IFileService fileService)
    {
        _serviceOfferingRepository = serviceOfferingRepository;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<ServiceOfferingDTO> GetServiceOfferingAsync()
    {
        var service = await _serviceOfferingRepository.GetSingleAsync(); // Always ID = 1
        return _mapper.Map<ServiceOfferingDTO>(service);
    }
    public async Task<ServiceOfferingDTO> GetServiceOfferingAvaliableAsync()
    {
        var service = await _serviceOfferingRepository.GetSingleAvialblyAsync(); // Always ID = 1
        return _mapper.Map<ServiceOfferingDTO>(service);
    }

    public async Task<bool> UpdateTitleAndDescriptionAsync(string title, string description)
    {
        var service = await _serviceOfferingRepository.GetSingleAsync();
        if (service == null) return false;

        service.Title = title;
        service.Description = description;
        await _serviceOfferingRepository.UpdateAsync(service);
        return true;
    }

    public async Task<ServiceOfferingDTOItem> AddServiceItemAsync(CreateServiceOfferingDTOItem dto)
    {
        var service = await _serviceOfferingRepository.GetSingleAsync();
        if (service == null)
            throw new InvalidOperationException("Default ServiceOffering not found.");

        var entity = _mapper.Map<ServiceOfferingItem>(dto);
        entity.ServiceOfferingId = service.Id;
        entity.CreatedAt = DateTime.UtcNow;

        if (dto.Image != null)
        {
            string imageUrl = await _fileService.UploadFileAsync(dto.Image, "serviceOffering");
            entity.ImageUrl = imageUrl;
        }

        await _serviceOfferingRepository.AddServiceItemAsync(entity);
        return _mapper.Map<ServiceOfferingDTOItem>(entity);
    }

    public async Task<ServiceOfferingDTOItem> UpdateServiceItemAsync(int itemId, UpdateServiceOfferingDTOItem dto)
    {
        var item = await _serviceOfferingRepository.GetItemByIdAsync(itemId);
        if (item == null) return null;

        // Map the updatable fields manually (or use AutoMapper for selected fields)
        item.Name = dto.Name ?? item.Name;
        item.Description = dto.Description ?? item.Description;
        item.Url = dto.Url ?? item.Url;
        if (dto.IsActive.HasValue)
            item.IsActive = dto.IsActive.Value;

        item.UpdatedAt = DateTime.UtcNow;

        // Handle new image upload
        if (dto.Image != null)
        {
            // Delete the old image if exists
            if (!string.IsNullOrEmpty(item.ImageUrl))
            {
                _fileService.DeleteFile(item.ImageUrl);
            }

            // Upload the new image
            var imageUrl = await _fileService.UploadFileAsync(dto.Image, "serviceImages");
            item.ImageUrl = imageUrl;
        }

        await _serviceOfferingRepository.UpdateServiceItemAsync(item);
        return _mapper.Map<ServiceOfferingDTOItem>(item);
    }


    public async Task<bool> DeleteServiceItemAsync(int itemId)
    {
        var item = await _serviceOfferingRepository.GetItemByIdAsync(itemId);
        if (item == null) return false;

        // Delete the image from wwwroot if it exists
        if (!string.IsNullOrWhiteSpace(item.ImageUrl))
        {
            _fileService.DeleteFile(item.ImageUrl);
        }

        await _serviceOfferingRepository.DeleteServiceItemAsync(item);
        return true;
    }


    public async Task<List<ServiceOfferingDTOItem>> GetServiceItemsAsync()
    {
        var items = await _serviceOfferingRepository.GetServiceItemsAsync();
        return _mapper.Map<List<ServiceOfferingDTOItem>>(items);
    }

    public async Task<int> GetTotalServicesCountAsync()
    {
        var Items =await _serviceOfferingRepository.GetServiceItemsAsync();
        return Items.Count;

    }
}
