using BLL.ServiceAbstraction;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.Common;
using Shared.DTOS.VideosLibraryDTOs;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosLibraryController : ControllerBase
    {
        private readonly IVideosLibraryService _videosLibraryService;

        public VideosLibraryController(IVideosLibraryService videosLibraryService)
        {
            _videosLibraryService = videosLibraryService;
        }

        // POST: api/VideosLibrary
        [HttpPost]
        public async Task<ActionResult<ApiResponse<VideosLibraryDTO>>> Create([FromForm] CreateVideosLibraryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<VideosLibraryDTO>.ErrorResult("Invalid input data", 400, errors));
            }

            try
            {
                var result = await _videosLibraryService.CreateVideoAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id },
                    ApiResponse<VideosLibraryDTO>.SuccessResult(result, "Video created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<VideosLibraryDTO>.ErrorResult("Failed to create video", 500));
            }
        }

        // GET: api/VideosLibrary
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<VideosLibraryDTO>>>> GetAll()
        {
            try
            {
                var result = await _videosLibraryService.GetAllVideosAsync();
                return Ok(ApiResponse<List<VideosLibraryDTO>>.SuccessResult(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<VideosLibraryDTO>>.ErrorResult("Failed to retrieve videos", 500));
            }
        }

        // GET: api/VideosLibrary/active
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<List<VideosLibraryDTO>>>> GetActive()
        {
            try
            {
                var result = await _videosLibraryService.GetActiveVideosAsync();
                return Ok(ApiResponse<List<VideosLibraryDTO>>.SuccessResult(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<VideosLibraryDTO>>.ErrorResult("Failed to retrieve active videos", 500));
            }
        }

        // GET: api/VideosLibrary/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<VideosLibraryDTO>>> GetById(int id)
        {
            try
            {
                var result = await _videosLibraryService.GetVideoByIdAsync(id);
                if (result == null)
                    return NotFound(ApiResponse<VideosLibraryDTO>.ErrorResult($"Video with ID {id} not found", 404));

                return Ok(ApiResponse<VideosLibraryDTO>.SuccessResult(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<VideosLibraryDTO>.ErrorResult("Failed to retrieve video", 500));
            }
        }

        // PUT: api/VideosLibrary/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, [FromForm] UpdateVideosLibraryDTO dto)
        {
            try
            {
                var success = await _videosLibraryService.UpdateVideoAsync(id, dto);
                if (!success)
                    return NotFound(ApiResponse<string>.ErrorResult($"Video with ID {id} not found", 404));

                return Ok(ApiResponse<string>.SuccessResult("Video updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult("Failed to update video", 500));
            }
        }

        // DELETE: api/VideosLibrary/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            try
            {
                var success = await _videosLibraryService.DeleteVideoAsync(id);
                if (!success)
                    return NotFound(ApiResponse<string>.ErrorResult($"Video with ID {id} not found", 404));

                return Ok(ApiResponse<string>.SuccessResult("Video deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult("Failed to delete video", 500));
            }
        }
    }
}
