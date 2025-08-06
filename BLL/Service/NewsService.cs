using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.NewsDTOs;
using BLL.Services.FileService;

namespace BLL.Service
{
    public class NewsService : INewsService
    {
        private readonly INewsItemRepository _newsItemRepository;
        private readonly INewsImageRepository _newsImageRepository;
        private readonly IMapper _mapper;

        public NewsService(INewsItemRepository newsItemRepository, INewsImageRepository newsImageRepository, IMapper mapper)
        {
            _newsItemRepository = newsItemRepository;
            _newsImageRepository = newsImageRepository;
            _mapper = mapper;
        }

        public async Task<List<NewsItemDTO>> GetAllNewsAsync()
        {
            var news = await _newsItemRepository.GetAllWithImagesAsync();
            return _mapper.Map<List<NewsItemDTO>>(news);
        }

        public async Task<List<NewsItemDTO>> GetActiveNewsAsync()
        {
            var news = await _newsItemRepository.GetActiveNewsWithImagesAsync();
            return _mapper.Map<List<NewsItemDTO>>(news);
        }

        public async Task<NewsItemDTO> GetNewsByIdAsync(int id)
        {
            var news = await _newsItemRepository.GetByIdWithImagesAsync(id);
            return _mapper.Map<NewsItemDTO>(news);
        }

        public async Task<NewsItemDTO> CreateNewsAsync(string adminId, CreateNewsItemDTO createNewsDto)
        {
            var news = _mapper.Map<NewsItem>(createNewsDto);
            news.Author = adminId;
            news.CreatedAt = DateTime.UtcNow;

            if (news.IsPublished)
                news.PublishedAt = DateTime.UtcNow;

            FileService fs = new FileService();

            // Handle single image (backward compatibility)
            if (createNewsDto.Image != null)
            {
                var imgUrl = await fs.UploadFileAsync(createNewsDto.Image, fs._newsFileName);
                news.ImageUrl = imgUrl;
            }

            // Save news item first
            var createdNews = await _newsItemRepository.AddAsync(news);

            // Handle multiple images
            if (createNewsDto.Images != null && createNewsDto.Images.Any())
            {
                foreach (var imageFile in createNewsDto.Images)
                {
                    if (imageFile != null)
                    {
                        var imageUrl = await fs.UploadFileAsync(imageFile, fs._newsFileName);
                        var newsImage = new NewsImage
                        {
                            ImageUrl = imageUrl,
                            NewsItemId = createdNews.Id
                        };
                        await _newsImageRepository.AddAsync(newsImage);
                    }
                }
            }

            // Get the news with images to return
            var newsWithImages = await _newsItemRepository.GetByIdWithImagesAsync(createdNews.Id);
            return _mapper.Map<NewsItemDTO>(newsWithImages);
        }

        public async Task<NewsItemDTO> UpdateNewsAsync(int id, UpdateNewsItemDTO updateNewsDto)
        {
            var news = await _newsItemRepository.GetByIdWithImagesAsync(id);
            if (news == null)
                return null;

            // Update properties
            if (!string.IsNullOrEmpty(updateNewsDto.Title))
                news.Title = updateNewsDto.Title;

            if (!string.IsNullOrEmpty(updateNewsDto.Content))
                news.Content = updateNewsDto.Content;

            if (!string.IsNullOrEmpty(updateNewsDto.Summary))
                news.Summary = updateNewsDto.Summary;

            if (!string.IsNullOrEmpty(updateNewsDto.Category))
                news.Category = updateNewsDto.Category;

            if (updateNewsDto.IsPublished.HasValue)
            {
                news.IsPublished = updateNewsDto.IsPublished.Value;
                if (news.IsPublished && !news.PublishedAt.HasValue)
                    news.PublishedAt = DateTime.UtcNow;
            }

            if (updateNewsDto.Tags != null)
                news.Tags = updateNewsDto.Tags;

            news.UpdatedAt = DateTime.UtcNow;

            FileService fs = new FileService();

            // Handle single image update (backward compatibility)
            if (updateNewsDto.Image != null)
            {
                // Delete old single image if exists
                if (!string.IsNullOrEmpty(news.ImageUrl))
                    fs.DeleteFile(news.ImageUrl);

                var imgUrl = await fs.UploadFileAsync(updateNewsDto.Image, fs._newsFileName);
                news.ImageUrl = imgUrl;
            }

            // Handle deleting specific images
            if (updateNewsDto.ImagesToDelete != null && updateNewsDto.ImagesToDelete.Any())
            {
                foreach (var imageUrl in updateNewsDto.ImagesToDelete)
                {
                    var imageToDelete = await _newsImageRepository.GetByImageUrlAsync(imageUrl);
                    if (imageToDelete != null)
                    {
                        fs.DeleteFile(imageUrl);
                        await _newsImageRepository.DeleteAsync(imageToDelete.Id);
                    }
                }
            }

            // Handle adding new images
            if (updateNewsDto.Images != null && updateNewsDto.Images.Any())
            {
                foreach (var imageFile in updateNewsDto.Images)
                {
                    if (imageFile != null)
                    {
                        var imageUrl = await fs.UploadFileAsync(imageFile, fs._newsFileName);
                        var newsImage = new NewsImage
                        {
                            ImageUrl = imageUrl,
                            NewsItemId = news.Id
                        };
                        await _newsImageRepository.AddAsync(newsImage);
                    }
                }
            }

            var updatedNews = await _newsItemRepository.UpdateAsync(news);

            // Get the updated news with images
            var newsWithImages = await _newsItemRepository.GetByIdWithImagesAsync(updatedNews.Id);
            return _mapper.Map<NewsItemDTO>(newsWithImages);
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _newsItemRepository.GetByIdWithImagesAsync(id);
            if (news == null)
                return false;

            FileService fs = new FileService();

            // Delete single image if exists
            if (!string.IsNullOrEmpty(news.ImageUrl))
                fs.DeleteFile(news.ImageUrl);

            // Delete all associated images
            foreach (var image in news.Images)
            {
                fs.DeleteFile(image.ImageUrl);
            }

            await _newsItemRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> IncrementViewCountAsync(int id)
        {
            var news = await _newsItemRepository.GetByIdAsync(id);
            if (news == null)
                return false;

            news.ViewCount++;
            await _newsItemRepository.UpdateAsync(news);
            return true;
        }

        public async Task<object> GetNewsStatisticsAsync()
        {
            var news = await _newsItemRepository.GetAllAsync();

            return new
            {
                TotalNews = news.Count(),
                PublishedNews = news.Count(n => n.IsPublished),
                TotalViews = news.Sum(n => n.ViewCount),
                MostViewedNews = await _newsItemRepository.GetMostViewedNewsAsync(5)
            };
        }

        // New methods for image management
        public async Task<bool> AddImageToNewsAsync(int newsId, string imageUrl)
        {
            var news = await _newsItemRepository.GetByIdAsync(newsId);
            if (news == null)
                return false;

            var newsImage = new NewsImage
            {
                ImageUrl = imageUrl,
                NewsItemId = newsId
            };

            await _newsImageRepository.AddAsync(newsImage);
            return true;
        }

        public async Task<bool> RemoveImageFromNewsAsync(int newsId, string imageUrl)
        {
            var image = await _newsImageRepository.GetByImageUrlAsync(imageUrl);
            if (image == null || image.NewsItemId != newsId)
                return false;

            FileService fs = new FileService();
            fs.DeleteFile(imageUrl);
            await _newsImageRepository.DeleteAsync(image.Id);
            return true;
        }

        public async Task<List<NewsImageDTO>> GetNewsImagesAsync(int newsId)
        {
            var images = await _newsImageRepository.GetByNewsItemIdAsync(newsId);
            return _mapper.Map<List<NewsImageDTO>>(images);
        }
    }
}