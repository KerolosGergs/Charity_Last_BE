using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Shared.DTOS.VideosLibraryDTOs;

namespace BLL.Service
{
    public class VideosLibraryService : IVideosLibraryService
    {
        private readonly IVideosLibraryRepository _videosLibraryRepository;
        private readonly IMapper _mapper;

        public VideosLibraryService(
            IVideosLibraryRepository videosLibraryRepository,
            IMapper mapper)
        {
            _videosLibraryRepository = videosLibraryRepository;
            _mapper = mapper;
        }

        public async Task<VideosLibraryDTO> CreateVideoAsync(CreateVideosLibraryDTO dto)
        {
            var videoEntity = _mapper.Map<VideosLibrary>(dto);
            videoEntity.CreatedAt = DateTime.UtcNow;
            videoEntity.IsActive = true;

            var result = await _videosLibraryRepository.AddAsync(videoEntity);
            return _mapper.Map<VideosLibraryDTO>(result);
        }

        public async Task<List<VideosLibraryDTO>> GetAllVideosAsync()
        {
            var videos = await _videosLibraryRepository.GetAllAsync();
            return _mapper.Map<List<VideosLibraryDTO>>(videos);
        }

        public async Task<VideosLibraryDTO> GetVideoByIdAsync(int id)
        {
            var video = await _videosLibraryRepository.GetByIdAsync(id);
            return video == null ? null : _mapper.Map<VideosLibraryDTO>(video);
        }

        public async Task<bool> UpdateVideoAsync(int id, UpdateVideosLibraryDTO updateDTO)
        {
            var video = await _videosLibraryRepository.GetByIdAsync(id);
            if (video == null) return false;

            video.Name = updateDTO.Name ?? video.Name;
            video.Description = updateDTO.Description ?? video.Description;
            video.VideoUrl = updateDTO.VideoUrl ?? video.VideoUrl;
            video.IsActive = updateDTO.IsActive;

            await _videosLibraryRepository.UpdateAsync(video);
            return true;
        }

        public async Task<bool> DeleteVideoAsync(int id)
        {
            var video = await _videosLibraryRepository.GetByIdAsync(id);
            if (video == null) return false;

            return await _videosLibraryRepository.DeleteAsync(id);
        }

        public async Task<List<VideosLibraryDTO>> GetActiveVideosAsync()
        {
            var videos = await _videosLibraryRepository.GetAllAsync();
            return videos
                .Where(v => v.IsActive)
                .Select(v => _mapper.Map<VideosLibraryDTO>(v))
                .ToList();
        }
    }
}
