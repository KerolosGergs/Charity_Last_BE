using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.ServiceAbstraction;
using BLL.Services.FileService;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.ImageLibraryDTOs;

namespace BLL.Service
{
    public class ImageLibraryService : IImageLibraryService
    {
        private readonly IImagesLibraryRepository _imagesLibraryRepository;
        private readonly IMapper _mapper;

        public ImageLibraryService(
            IImagesLibraryRepository imagesLibraryRepository,
            IMapper mapper)
        {
            _imagesLibraryRepository = imagesLibraryRepository;
            _mapper = mapper;
        }

        public async Task<ImageLibraryDTO> CreateImageLibraryAsync(CreateImageLibraryDTO createImageLibraryDTO)
        {
            var entity = _mapper.Map<ImagesLibrary>(createImageLibraryDTO);
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;

            var fileService = new FileService();
            var imageUrl = await fileService.UploadFileAsync(createImageLibraryDTO.ImageUrl, "imageslibrary");

            entity.ImageUrl = imageUrl;
            var result = await _imagesLibraryRepository.AddAsync(entity);
            return _mapper.Map<ImageLibraryDTO>(result);
        }

        public async Task<List<ImageLibraryDTO>> GetAllImagesAsync()
        {
            var images = await _imagesLibraryRepository.GetAllAsync();
            return _mapper.Map<List<ImageLibraryDTO>>(images);
        }

        public async Task<ImageLibraryDTO> GetImageByIdAsync(int id)
        {
            var image = await _imagesLibraryRepository.GetByIdAsync(id);
            return image == null ? null : _mapper.Map<ImageLibraryDTO>(image);
        }

        public async Task<bool> UpdateImageAsync(int id, UpdateImageLibraryDTO updateDTO)
        {
            var image = await _imagesLibraryRepository.GetByIdAsync(id);
            if (image == null) return false;

            image.Name = updateDTO.Name ?? image.Name;
            image.Description = updateDTO.Description ?? image.Description;
            image.IsActive = updateDTO.IsActive;

            if (updateDTO.ImageUrl != null)
            {
                var fileService = new FileService();

                fileService.DeleteFile(image.ImageUrl);

                var newImageUrl = await fileService.UploadFileAsync(updateDTO.ImageUrl, "images");
                image.ImageUrl = newImageUrl;
            }

            await _imagesLibraryRepository.UpdateAsync(image);
            return true;
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            var image = await _imagesLibraryRepository.GetByIdAsync(id);
            if (image == null) return false;

            var fileService = new FileService(); 
            fileService.DeleteFile(image.ImageUrl);

            return await _imagesLibraryRepository.DeleteAsync(id);
        }

        public async Task<List<ImageLibraryDTO>> GetActiveImages()
        {
            var images = await _imagesLibraryRepository.GetAllAsync();
            return images
                .Where(i => i.IsActive)
                .Select(i => _mapper.Map<ImageLibraryDTO>(i))
                .ToList();
        }
    }

}
