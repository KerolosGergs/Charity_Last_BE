using BLL.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.DynamicPage;
using System.Security.Claims;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicPageController : ControllerBase
    {
        private readonly IDynamicPageService _dynamicPageService;

        public DynamicPageController(IDynamicPageService dynamicPageService)
        {
            _dynamicPageService = dynamicPageService;
        }

        // GET: api/dynamicpage
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            var result = await _dynamicPageService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/dynamicpage/active
        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllActive()
        {
            var result = await _dynamicPageService.GetAllActiveAsync();
            return Ok(result);
        }

        // GET: api/dynamicpage/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _dynamicPageService.GetByIdAsync(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        // GET: api/dynamicpage/slug/{slug}
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetBySlug(string slug)
        {
            var result = await _dynamicPageService.GetBySlugAsync(slug);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        // GET: api/dynamicpage/{id}/items
        [HttpGet("{id}/items")]
        [AllowAnonymous]
        public async Task<ActionResult> GetItems(int id)
        {
            var result = await _dynamicPageService.GetItemsAsync(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        // POST: api/dynamicpage

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] CreateDynamicPageDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var result = await _dynamicPageService.CreateAsync(createDto, "1");
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        // PUT: api/dynamicpage/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateDynamicPageDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var result = await _dynamicPageService.UpdateAsync(id, updateDto, "1");
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        // DELETE: api/dynamicpage/{id}
        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var result = await _dynamicPageService.DeleteAsync(id);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        // PATCH: api/dynamicpage/{id}/toggle-active
        [HttpPatch("{id}/toggle-active")]
        public async Task<ActionResult> ToggleActive(int id)
        {

            var result = await _dynamicPageService.ToggleActiveAsync(id, "1");
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        // POST: api/dynamicpage/upload
        [HttpPost("upload")]

        public async Task<ActionResult> UploadFile(IFormFile file, [FromQuery] string fileType)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            if (string.IsNullOrEmpty(fileType))
                return BadRequest("File type is required");

            var result = await _dynamicPageService.UploadFileAsync(file, fileType);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        // DELETE: api/dynamicpage/delete-file
        [HttpDelete("delete-file")]

        public async Task<ActionResult> DeleteFile([FromQuery] string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return BadRequest("File URL is required");

            var result = await _dynamicPageService.DeleteFileAsync(fileUrl);
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }
    }
}