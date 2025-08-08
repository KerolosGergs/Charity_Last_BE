using BLL.ServiceAbstraction;
using DAL.Data.Models.IdentityModels;
using DAL.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Shared.DTOS.Common;
using Shared.DTOS.DynamicPage;
using System.Text.RegularExpressions;

namespace BLL.Service
{
    public class DynamicPageService : IDynamicPageService
    {
        private readonly IDynamicPageRepository _dynamicPageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DynamicPageService(IDynamicPageRepository dynamicPageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _dynamicPageRepository = dynamicPageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ApiResponse<IEnumerable<DynamicPageDto>>> GetAllAsync()
        {
            try
            {
                var pages = await _dynamicPageRepository.GetAllAsync();
                var dtos = pages.Select(page => MapToDto(page));
                return ApiResponse<IEnumerable<DynamicPageDto>>.SuccessResult(dtos, "Pages retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<DynamicPageDto>>.ErrorResult("Failed to retrieve pages", 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<DynamicPageDto>>> GetAllActiveAsync()
        {
            try
            {
                var pages = await _dynamicPageRepository.GetAllActiveAsync();
                var dtos = pages.Select(page => MapToDto(page));
                return ApiResponse<IEnumerable<DynamicPageDto>>.SuccessResult(dtos, "Active pages retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<DynamicPageDto>>.ErrorResult("Failed to retrieve active pages", 500);
            }
        }

        public async Task<ApiResponse<DynamicPageDto>> GetByIdAsync(int id)
        {
            try
            {
                var page = await _dynamicPageRepository.GetByIdAsync(id);
                if (page == null)
                    return ApiResponse<DynamicPageDto>.ErrorResult("Page not found", 404);

                var dto = MapToDto(page);
                return ApiResponse<DynamicPageDto>.SuccessResult(dto, "Page retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DynamicPageDto>.ErrorResult("Failed to retrieve page", 500);
            }
        }

        public async Task<ApiResponse<DynamicPageDto>> GetBySlugAsync(string slug)
        {
            try
            {
                var page = await _dynamicPageRepository.GetBySlugAsync(slug);
                if (page == null)
                    return ApiResponse<DynamicPageDto>.ErrorResult("Page not found", 404);

                var dto = MapToDto(page);
                return ApiResponse<DynamicPageDto>.SuccessResult(dto, "Page retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DynamicPageDto>.ErrorResult("Failed to retrieve page", 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<DynamicPageItemDto>>> GetItemsAsync(int id)
        {
            try
            {
                var page = await _dynamicPageRepository.GetByIdAsync(id);
                if (page == null)
                    return ApiResponse<IEnumerable<DynamicPageItemDto>>.ErrorResult("Page not found", 404);

                var items = page.Items.Select(item => new DynamicPageItemDto
                {
                    Id = item.Id,
                    DynamicPageId = item.DynamicPageId,
                    Type = item.Type,
                    Content = item.Content,
                    ImageUrl = item.ImageUrl,
                    FileUrl = item.FileUrl,
                    FileName = item.FileName,
                    VideoUrl = item.VideoUrl,
                    Order = item.Order,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                }).OrderBy(item => item.Order);

                return ApiResponse<IEnumerable<DynamicPageItemDto>>.SuccessResult(items, "Items retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<DynamicPageItemDto>>.ErrorResult("Failed to retrieve items", 500);
            }
        }

        public async Task<ApiResponse<DynamicPageDto>> CreateAsync(CreateDynamicPageDto createDto, string userId)
        {
            try
            {
                // Validate slug uniqueness
                if (!string.IsNullOrEmpty(createDto.Slug))
                {
                    if (await _dynamicPageRepository.ExistsBySlugAsync(createDto.Slug))
                        return ApiResponse<DynamicPageDto>.ErrorResult("Slug already exists", 400);
                }

                var dynamicPage = new DynamicPage
                {
                    PageName = createDto.PageName,
                    Description = createDto.Description,
                    Slug = createDto.Slug ?? GenerateSlug(createDto.PageName),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    Items = createDto.Items.Select((item, index) => new DynamicPageItem
                    {
                        Type = item.Type,
                        Content = item.Content,
                        ImageUrl = item.ImageUrl,
                        FileUrl = item.FileUrl,
                        FileName = item.FileName,
                        VideoUrl = item.VideoUrl,
                        Order = item.Order,
                        CreatedAt = DateTime.UtcNow
                    }).ToList()
                };

                var createdPage = await _dynamicPageRepository.CreateAsync(dynamicPage);
                var dto = MapToDto(createdPage);
                return ApiResponse<DynamicPageDto>.SuccessResult(dto, "Page created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DynamicPageDto>.ErrorResult("Failed to create page", 500);
            }
        }

        public async Task<ApiResponse<DynamicPageDto>> UpdateAsync(int id, UpdateDynamicPageDto updateDto, string userId)
        {
            try
            {
                var existingPage = await _dynamicPageRepository.GetByIdAsync(id);
                if (existingPage == null)
                    return ApiResponse<DynamicPageDto>.ErrorResult("Page not found", 404);

                // Validate slug uniqueness
                if (!string.IsNullOrEmpty(updateDto.Slug) && updateDto.Slug != existingPage.Slug)
                {
                    if (await _dynamicPageRepository.ExistsBySlugAsync(updateDto.Slug))
                        return ApiResponse<DynamicPageDto>.ErrorResult("Slug already exists", 400);
                }

                // Update page properties
                existingPage.PageName = updateDto.PageName;
                existingPage.Description = updateDto.Description;
                existingPage.Slug = updateDto.Slug ?? GenerateSlug(updateDto.PageName);
                existingPage.UpdatedAt = DateTime.UtcNow;
                existingPage.UpdatedBy = userId;

                // Update items
                var existingItems = existingPage.Items.ToList();
                var updatedItems = new List<DynamicPageItem>();

                foreach (var itemDto in updateDto.Items)
                {
                    if (itemDto.Id.HasValue)
                    {
                        // Update existing item
                        var existingItem = existingItems.FirstOrDefault(i => i.Id == itemDto.Id);
                        if (existingItem != null)
                        {
                            existingItem.Type = itemDto.Type;
                            existingItem.Content = itemDto.Content;
                            existingItem.ImageUrl = itemDto.ImageUrl;
                            existingItem.FileUrl = itemDto.FileUrl;
                            existingItem.FileName = itemDto.FileName;
                            existingItem.VideoUrl = itemDto.VideoUrl;
                            existingItem.Order = itemDto.Order;
                            existingItem.UpdatedAt = DateTime.UtcNow;
                            updatedItems.Add(existingItem);
                        }
                    }
                    else
                    {
                        // Add new item
                        var newItem = new DynamicPageItem
                        {
                            DynamicPageId = id,
                            Type = itemDto.Type,
                            Content = itemDto.Content,
                            ImageUrl = itemDto.ImageUrl,
                            FileUrl = itemDto.FileUrl,
                            FileName = itemDto.FileName,
                            VideoUrl = itemDto.VideoUrl,
                            Order = itemDto.Order,
                            CreatedAt = DateTime.UtcNow
                        };
                        updatedItems.Add(newItem);
                    }
                }

                // Remove items that are no longer in the update
                var itemsToRemove = existingItems.Where(item => !updateDto.Items.Any(dto => dto.Id == item.Id)).ToList();
                foreach (var item in itemsToRemove)
                {
                    existingPage.Items.Remove(item);
                }

                // Update items collection
                existingPage.Items = updatedItems;

                var updatedPage = await _dynamicPageRepository.UpdateAsync(existingPage);
                var dto = MapToDto(updatedPage);
                return ApiResponse<DynamicPageDto>.SuccessResult(dto, "Page updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DynamicPageDto>.ErrorResult("Failed to update page", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var exists = await _dynamicPageRepository.ExistsAsync(id);
                if (!exists)
                    return ApiResponse<bool>.ErrorResult("Page not found", 404);

                var result = await _dynamicPageRepository.DeleteAsync(id);
                return ApiResponse<bool>.SuccessResult(result, "Page deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Failed to delete page", 500);
            }
        }

        public async Task<ApiResponse<bool>> ToggleActiveAsync(int id, string userId)
        {
            try
            {
                var page = await _dynamicPageRepository.GetByIdAsync(id);
                if (page == null)
                    return ApiResponse<bool>.ErrorResult("Page not found", 404);

                page.IsActive = !page.IsActive;
                page.UpdatedAt = DateTime.UtcNow;
                page.UpdatedBy = userId;

                await _dynamicPageRepository.UpdateAsync(page);
                return ApiResponse<bool>.SuccessResult(page.IsActive, $"Page {(page.IsActive ? "activated" : "deactivated")} successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Failed to toggle page status", 500);
            }
        }

        public async Task<ApiResponse<FileUploadResponseDto>> UploadFileAsync(IFormFile file, string fileType)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return ApiResponse<FileUploadResponseDto>.ErrorResult("No file uploaded", 400);

                // Validate file type
                var allowedExtensions = fileType.ToLower() == "image"
                    ? new[] { ".jpg", ".jpeg", ".png", ".gif" }
                    : new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };

                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                    return ApiResponse<FileUploadResponseDto>.ErrorResult("Invalid file type", 400);

                // Validate file size
                var maxSize = fileType.ToLower() == "image" ? 5 * 1024 * 1024 : 10 * 1024 * 1024; // 5MB for images, 10MB for documents
                if (file.Length > maxSize)
                    return ApiResponse<FileUploadResponseDto>.ErrorResult("File size too large", 400);

                // Create upload directory
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "dynamic-pages", fileType);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return file URL
                var fileUrl = $"/uploads/dynamic-pages/{fileType}/{fileName}";
                var response = new FileUploadResponseDto
                {
                    Url = fileUrl,
                    FileName = file.FileName
                };

                return ApiResponse<FileUploadResponseDto>.SuccessResult(response, "File uploaded successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<FileUploadResponseDto>.ErrorResult("Failed to upload file", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteFileAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                    return ApiResponse<bool>.ErrorResult("File URL is required", 400);

                // Validate file URL
                if (!fileUrl.StartsWith("/uploads/dynamic-pages/"))
                    return ApiResponse<bool>.ErrorResult("Invalid file URL", 400);

                // Delete file
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileUrl.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return ApiResponse<bool>.SuccessResult(true, "File deleted successfully");
                }
                else
                {
                    return ApiResponse<bool>.ErrorResult("File not found", 404);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult("Failed to delete file", 500);
            }
        }

        private DynamicPageDto MapToDto(DynamicPage page)
        {
            return new DynamicPageDto
            {
                Id = page.Id,
                PageName = page.PageName,
                Description = page.Description,
                Slug = page.Slug,
                IsActive = page.IsActive,
                CreatedAt = page.CreatedAt,
                UpdatedAt = page.UpdatedAt,
                CreatedBy = page.CreatedBy,
                UpdatedBy = page.UpdatedBy,
                Items = page.Items.Select(item => new DynamicPageItemDto
                {
                    Id = item.Id,
                    DynamicPageId = item.DynamicPageId,
                    Type = item.Type,
                    Content = item.Content,
                    ImageUrl = item.ImageUrl,
                    FileUrl = item.FileUrl,
                    FileName = item.FileName,
                    VideoUrl = item.VideoUrl,
                    Order = item.Order,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                }).ToList()
            };
        }

        private string GenerateSlug(string title)
        {
            // Remove special characters and convert to lowercase
            var slug = Regex.Replace(title.ToLower(), @"[^a-z0-9\s-]", "");
            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");
            // Remove multiple hyphens
            slug = Regex.Replace(slug, @"-+", "-");
            // Remove leading and trailing hyphens
            slug = slug.Trim('-');
            return slug;
        }
    }
}