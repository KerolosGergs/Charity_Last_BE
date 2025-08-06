using BLL.ServiceAbstraction;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.Common;
using Shared.DTOS.HomePageDTOS;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : ControllerBase
    {
        private readonly IHeroSectionService _heroSectionService;
        private readonly IHomeVideoSectionService _homeVideoSectionService;
        private readonly ITrendSectionService _trendSectionService;

        public HomePageController(IHeroSectionService heroSectionService, IHomeVideoSectionService homeVideoSectionService, ITrendSectionService trendSectionService)
        {
            _heroSectionService = heroSectionService;
            _homeVideoSectionService = homeVideoSectionService;
            _trendSectionService = trendSectionService;
        }

        // GET: api/homepage/hero-section
        [HttpGet("hero-section")]
        public async Task<ActionResult<ApiResponse<HeroSectionDTOs>>> GetHeroSection()
        {
            try
            {
                var heroSection = await _heroSectionService.GetHeroSectionAsync();
                if (heroSection == null)
                    return NotFound(ApiResponse<HeroSectionDTOs>.ErrorResult("Hero section not found", 404));

                return Ok(ApiResponse<HeroSectionDTOs>.SuccessResult(heroSection));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<HeroSectionDTOs>.ErrorResult($"Internal server error: {ex.Message}", 500));
            }
        }

        // PUT: api/homepage/hero-section
        [HttpPut("hero-section")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateHeroSection([FromForm] UpdateHeroSectionDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<string>.ErrorResult("Invalid input data", 400, errors));
            }

            try
            {
                var result = await _heroSectionService.UpdateHeroSectionAsync(updateDto);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResult("Hero section not found or update failed", 404));

                return Ok(ApiResponse<string>.SuccessResult("Hero section updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult($"Internal server error: {ex.Message}", 500));
            }
        }
        // GET: api/homepage/video-section
        [HttpGet("video-section")]
        public async Task<ActionResult<ApiResponse<HomeVideoSectionDTO>>> GetHomeVideoSection()
        {
            try
            {
                var videoSection = await _homeVideoSectionService.GetHomeVideoSectionAsync();
                if (videoSection == null)
                    return NotFound(ApiResponse<HomeVideoSectionDTO>.ErrorResult("Video section not found", 404));
                return Ok(ApiResponse<HomeVideoSectionDTO>.SuccessResult(videoSection));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<HomeVideoSectionDTO>.ErrorResult($"Internal server error: {ex.Message}", 500));
            }
        }
        // PUT: api/homepage/video-section
        [HttpPut("video-section")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateHomeVideoSection([FromForm] UpdateHomeVideoSectionDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<string>.ErrorResult("Invalid input data", 400, errors));
            }
            try
            {
                var result = await _homeVideoSectionService.UpdateHomeVideoSectionAsync(updateDto);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResult("Video section not found or update failed", 404));
                return Ok(ApiResponse<string>.SuccessResult("Video section updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult($"Internal server error: {ex.Message}", 500));
            }
        }
        //Trend Section
        // GET: api/homepage/trend-section
        [HttpGet("trend-section")]
        public async Task<ActionResult<ApiResponse<TrendSectionDTO>>> GetTrendSection()
        {
            try
            {
                var trendSection = await _trendSectionService.GetTrendingAsync();
                if (trendSection == null)
                    return NotFound(ApiResponse<TrendSectionDTO>.ErrorResult("Trend section not found", 404));
                return Ok(ApiResponse<TrendSectionDTO>.SuccessResult(trendSection));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TrendSectionDTO>.ErrorResult($"Internal server error: {ex.Message}", 500));
            }
        }
        // PUT: api/homepage/trend-section
        [HttpPut("trend-section")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateTrendSection([FromForm] UpdateTrendSectionDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<string>.ErrorResult("Invalid input data", 400, errors));
            }
            try
            {
                var result = await _trendSectionService.UpdateTrendSection(updateDto);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResult("Trend section not found or update failed", 404));
                return Ok(ApiResponse<string>.SuccessResult("Trend section updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult($"Internal server error: {ex.Message}", 500));
            }
        }
    }
}
