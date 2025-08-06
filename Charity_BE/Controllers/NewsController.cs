using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.NewsDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;
using Shared.DTOS.ServiceOfferingDTOs;
using System.Security.Claims;
using BLL.Services.FileService;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: api/news
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<NewsItemDTO>>>> GetAllNews()
        {
            try
            {
                var news = await _newsService.GetAllNewsAsync();
                return Ok(ApiResponse<List<NewsItemDTO>>.SuccessResult(news));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<NewsItemDTO>>.ErrorResult(ex.Message, 500));
            }
        }

        // GET: api/news/active
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<List<NewsItemDTO>>>> GetActiveNews()
        {
            try
            {
                var news = await _newsService.GetActiveNewsAsync();
                return Ok(ApiResponse<List<NewsItemDTO>>.SuccessResult(news));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<NewsItemDTO>>.ErrorResult("Failed to retrieve active news", 500));
            }
        }

        // GET: api/news/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<NewsItemDTO>>> GetNewsById(int id)
        {
            try
            {
                var news = await _newsService.GetNewsByIdAsync(id);
                if (news == null)
                    return NotFound(ApiResponse<NewsItemDTO>.ErrorResult($"News with ID {id} not found", 404));

                return Ok(ApiResponse<NewsItemDTO>.SuccessResult(news));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<NewsItemDTO>.ErrorResult("Failed to retrieve news", 500));
            }
        }

        // POST: api/news
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<NewsItemDTO>>> CreateNews([FromForm] CreateNewsItemDTO createNewsDto, [FromQuery] string adminId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<NewsItemDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                if (string.IsNullOrEmpty(adminId))
                    return BadRequest(ApiResponse<NewsItemDTO>.ErrorResult("Admin ID is required", 400));

                var news = await _newsService.CreateNewsAsync(adminId, createNewsDto);
                return CreatedAtAction(nameof(GetNewsById), new { id = news.Id },
                    ApiResponse<NewsItemDTO>.SuccessResult(news, "News created successfully"));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<NewsItemDTO>.ErrorResult("Failed to create news", 500));
            }
        }

        // PUT: api/news/{id}
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<NewsItemDTO>>> UpdateNews(int id, [FromForm] UpdateNewsItemDTO updateNewsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<NewsItemDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                var news = await _newsService.UpdateNewsAsync(id, updateNewsDto);
                if (news == null)
                    return NotFound(ApiResponse<NewsItemDTO>.ErrorResult($"News with ID {id} not found", 404));

                return Ok(ApiResponse<NewsItemDTO>.SuccessResult(news, "News updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<NewsItemDTO>.ErrorResult("Failed to update news", 500));
            }
        }

        // DELETE: api/news/{id}
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteNews(int id)
        {
            try
            {
                var result = await _newsService.DeleteNewsAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"News with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "News deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult(ex.Message, 500));
            }
        }

        // PUT: api/news/{id}/view
        [HttpPut("{id}/view")]
        public async Task<ActionResult<ApiResponse<bool>>> IncrementViewCount(int id)
        {
            try
            {
                var result = await _newsService.IncrementViewCountAsync(id);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"News with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "View count incremented"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to increment view count", 500));
            }
        }

        // GET: api/news/statistics
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetNewsStatistics()
        {
            try
            {
                var statistics = await _newsService.GetNewsStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve statistics", 500));
            }
        }

        // NEW ENDPOINTS FOR IMAGE MANAGEMENT

        // GET: api/news/{id}/images
        [HttpGet("{id}/images")]
        public async Task<ActionResult<ApiResponse<List<NewsImageDTO>>>> GetNewsImages(int id)
        {
            try
            {
                var images = await _newsService.GetNewsImagesAsync(id);
                return Ok(ApiResponse<List<NewsImageDTO>>.SuccessResult(images));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<NewsImageDTO>>.ErrorResult("Failed to retrieve news images", 500));
            }
        }

        // POST: api/news/{id}/images
        [HttpPost("{id}/images")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> AddImageToNews(int id, [FromForm] IFormFile image)
        {
            try
            {
                if (image == null)
                    return BadRequest(ApiResponse<bool>.ErrorResult("Image file is required", 400));

                // You'll need to upload the image first using FileService
                var fileService = new FileService();
                var imageUrl = await fileService.UploadFileAsync(image, fileService._newsFileName);

                var result = await _newsService.AddImageToNewsAsync(id, imageUrl);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"News with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Image added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to add image", 500));
            }
        }

        // DELETE: api/news/{id}/images
        [HttpDelete("{id}/images")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveImageFromNews(int id, [FromQuery] string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return BadRequest(ApiResponse<bool>.ErrorResult("Image URL is required", 400));

                var result = await _newsService.RemoveImageFromNewsAsync(id, imageUrl);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult("Image not found or doesn't belong to this news", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Image removed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to remove image", 500));
            }
        }
    }
} 