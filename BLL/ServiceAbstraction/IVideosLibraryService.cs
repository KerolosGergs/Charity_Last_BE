using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.VideosLibraryDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IVideosLibraryService
    {
        Task<VideosLibraryDTO> CreateVideoAsync(CreateVideosLibraryDTO dto);
        Task<List<VideosLibraryDTO>> GetAllVideosAsync();
        Task<VideosLibraryDTO> GetVideoByIdAsync(int id);
        Task<bool> UpdateVideoAsync(int id, UpdateVideosLibraryDTO updateDTO);
        Task<bool> DeleteVideoAsync(int id);
        Task<List<VideosLibraryDTO>> GetActiveVideosAsync();
    }
}
