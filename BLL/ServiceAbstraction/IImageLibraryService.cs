using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.ImageLibraryDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IImageLibraryService
    {
        public Task<ImageLibraryDTO> CreateImageLibraryAsync(CreateImageLibraryDTO createImageLibraryDTO);
        public Task<List<ImageLibraryDTO>> GetAllImagesAsync();
        public Task<ImageLibraryDTO> GetImageByIdAsync(int id);
        public Task<bool> UpdateImageAsync(int id, UpdateImageLibraryDTO updateImageLibraryDTO);
        public Task<bool> DeleteImageAsync(int id);
        public Task<List<ImageLibraryDTO>> GetActiveImages();

    }
}
