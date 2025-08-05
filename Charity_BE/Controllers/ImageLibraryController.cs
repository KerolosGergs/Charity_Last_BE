using BLL.ServiceAbstraction;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.Common;
using Shared.DTOS.ImageLibraryDTOs;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageLibraryController : ControllerBase
    {
        private readonly IImageLibraryService _imageLibraryService;

        public ImageLibraryController(IImageLibraryService imageLibraryService)
        {
            _imageLibraryService = imageLibraryService;
        }

        // POST: api/ImageLibrary
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ImageLibraryDTO>>> Create([FromForm] CreateImageLibraryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<ImageLibraryDTO>.ErrorResult("Invalid input data", 400, errors));
            }

            try
            {
                var result = await _imageLibraryService.CreateImageLibraryAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id },
                    ApiResponse<ImageLibraryDTO>.SuccessResult(result, "Image created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ImageLibraryDTO>.ErrorResult("Failed to create image", 500));
            }
        }

        // GET: api/ImageLibrary
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ImageLibraryDTO>>>> GetAll()
        {
            try
            {
                var result = await _imageLibraryService.GetAllImagesAsync();
                return Ok(ApiResponse<List<ImageLibraryDTO>>.SuccessResult(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ImageLibraryDTO>>.ErrorResult("Failed to retrieve images", 500));
            }
        }

        // GET: api/ImageLibrary/active
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<List<ImageLibraryDTO>>>> GetActive()
        {
            try
            {
                var result = await _imageLibraryService.GetActiveImages();
                return Ok(ApiResponse<List<ImageLibraryDTO>>.SuccessResult(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ImageLibraryDTO>>.ErrorResult("Failed to retrieve active images", 500));
            }
        }

        // GET: api/ImageLibrary/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ImageLibraryDTO>>> GetById(int id)
        {
            try
            {
                var result = await _imageLibraryService.GetImageByIdAsync(id);
                if (result == null)
                    return NotFound(ApiResponse<ImageLibraryDTO>.ErrorResult($"Image with ID {id} not found", 404));

                return Ok(ApiResponse<ImageLibraryDTO>.SuccessResult(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ImageLibraryDTO>.ErrorResult("Failed to retrieve image", 500));
            }
        }

        // PUT: api/ImageLibrary/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, [FromForm] UpdateImageLibraryDTO dto)
        {
            try
            {
                var success = await _imageLibraryService.UpdateImageAsync(id, dto);
                if (!success)
                    return NotFound(ApiResponse<string>.ErrorResult($"Image with ID {id} not found", 404));

                return Ok(ApiResponse<string>.SuccessResult("Image updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult("Failed to update image", 500));
            }
        }

        // DELETE: api/ImageLibrary/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            try
            {
                var success = await _imageLibraryService.DeleteImageAsync(id);
                if (!success)
                    return NotFound(ApiResponse<string>.ErrorResult($"Image with ID {id} not found", 404));

                return Ok(ApiResponse<string>.SuccessResult("Image deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult("Failed to delete image", 500));
            }
        }
    }
}
