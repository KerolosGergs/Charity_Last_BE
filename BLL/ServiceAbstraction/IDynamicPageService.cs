using Microsoft.AspNetCore.Http;
using Shared.DTOS.Common;
using Shared.DTOS.DynamicPage;

namespace BLL.ServiceAbstraction
{
    public interface IDynamicPageService
    {
        Task<ApiResponse<IEnumerable<DynamicPageDto>>> GetAllAsync();
        Task<ApiResponse<IEnumerable<DynamicPageDto>>> GetAllActiveAsync();
        Task<ApiResponse<DynamicPageDto>> GetByIdAsync(int id);
        Task<ApiResponse<DynamicPageDto>> GetBySlugAsync(string slug);
        Task<ApiResponse<IEnumerable<DynamicPageItemDto>>> GetItemsAsync(int id);
        Task<ApiResponse<DynamicPageDto>> CreateAsync(CreateDynamicPageDto createDto, string userId);
        Task<ApiResponse<DynamicPageDto>> UpdateAsync(int id, UpdateDynamicPageDto updateDto, string userId);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<bool>> ToggleActiveAsync(int id, string userId);
        Task<ApiResponse<FileUploadResponseDto>> UploadFileAsync(IFormFile file, string fileType);
        Task<ApiResponse<bool>> DeleteFileAsync(string fileUrl);
    }
}