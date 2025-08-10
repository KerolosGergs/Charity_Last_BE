using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.ServiceOfferingDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOfferingController : ControllerBase
    {
        private readonly IServiceOfferingService _service;

        public ServiceOfferingController(IServiceOfferingService service)
        {
            _service = service;
        }

        // GET: api/serviceoffering
        [HttpGet]
        public async Task<ActionResult<ApiResponse<ServiceOfferingDTO>>> GetServiceOffering()
        {
            var result = await _service.GetServiceOfferingAsync();
            return Ok(ApiResponse<ServiceOfferingDTO>.SuccessResult(result));
        }
        [HttpGet("Avalible")]
        public async Task<ActionResult<ApiResponse<ServiceOfferingDTO>>> GetServiceOfferingAvaliable()
        {
            var result = await _service.GetServiceOfferingAvaliableAsync();
            return Ok(ApiResponse<ServiceOfferingDTO>.SuccessResult(result));
        }

        // PUT: api/serviceoffering/title-description
        [HttpPut("title-description")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateTitleAndDescription([FromBody] UpdateTitleDescriptionDTO dto)
        {
            var result = await _service.UpdateTitleAndDescriptionAsync(dto.Title, dto.Description);
            return result
                ? Ok(ApiResponse<bool>.SuccessResult(true, "Updated successfully"))
                : NotFound(ApiResponse<bool>.ErrorResult("ServiceOffering not found", 404));
        }

        // GET: api/serviceoffering/items
        [HttpGet("items")]
        public async Task<ActionResult<ApiResponse<List<ServiceOfferingDTOItem>>>> GetItems()
        {
            var items = await _service.GetServiceItemsAsync();
            return Ok(ApiResponse<List<ServiceOfferingDTOItem>>.SuccessResult(items));
        }
       
        // POST: api/serviceoffering/items
        [HttpPost("items")]
        public async Task<ActionResult<ApiResponse<ServiceOfferingDTOItem>>> AddItem([FromForm] CreateServiceOfferingDTOItem dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<ServiceOfferingDTOItem>.ErrorResult("Invalid input", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            var result = await _service.AddServiceItemAsync(dto);
            return Ok(ApiResponse<ServiceOfferingDTOItem>.SuccessResult(result, "Item created successfully"));
        }

        // PUT: api/serviceoffering/items/{id}
        [HttpPut("items/{id}")]
        public async Task<ActionResult<ApiResponse<ServiceOfferingDTOItem>>> UpdateItem(int id, [FromForm] UpdateServiceOfferingDTOItem dto)
        {
            var result = await _service.UpdateServiceItemAsync(id, dto);
            return result != null
                ? Ok(ApiResponse<ServiceOfferingDTOItem>.SuccessResult(result, "Item updated successfully"))
                : NotFound(ApiResponse<ServiceOfferingDTOItem>.ErrorResult("Item not found", 404));
        }

        // DELETE: api/serviceoffering/items/{id}
        [HttpDelete("items/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteItem(int id)
        {
            var result = await _service.DeleteServiceItemAsync(id);
            return result
                ? Ok(ApiResponse<bool>.SuccessResult(true, "Item deleted successfully"))
                : NotFound(ApiResponse<bool>.ErrorResult("Item not found", 404));
        }
    }
}
