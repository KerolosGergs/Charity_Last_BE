using BLL.ServiceAbstraction;
using DAL.Data.Models;
using DAL.Repositories.RepositoryIntrfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.Common;
using Shared.DTOS.NavItemDto;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavController : Controller
    {

        private readonly INavItemService _service;
        public NavController(INavItemService service) => _service = service;

        // ===== NavItems =====
        [HttpGet("navitems")]
        public async Task<IActionResult> GetAllNavItems()
        {
            var data = await _service.GetAllNavItemsAsync();
            return Ok(ApiResponse<List<NavItems>>.SuccessResult(data, "تم جلب عناصر القائمة بنجاح."));
        }

        [HttpGet("navitems/{id}")]
        public async Task<IActionResult> GetNavItem(int id)
        {
            var item = await _service.GetNavItemByIdAsync(id);
            if (item is null)
                return NotFound(ApiResponse<NavItemDto>.ErrorResult("عنصر القائمة غير موجود.", 404));
            return Ok(ApiResponse<NavItemDto>.SuccessResult(item, "تم جلب عنصر القائمة بنجاح."));
        }

        [HttpPost("navitems")]
        public async Task<IActionResult> AddNavItem([FromBody] NavItems navItem)
        {
            await _service.AddNavItemAsync(navItem);
            return Ok(ApiResponse<NavItems>.SuccessResult(navItem, "تم إنشاء عنصر القائمة بنجاح."));
        }

        [HttpPut("navitems/{id}")]
        public async Task<IActionResult> UpdateNavItem(int id, [FromBody] NavItems navItem)
        {
            var ok = await _service.UpdateNavItemAsync(id, navItem);
            if (!ok) return NotFound(ApiResponse<NavItems>.ErrorResult("عنصر القائمة غير موجود.", 404));
            return Ok(ApiResponse<NavItems>.SuccessResult(navItem, "تم تحديث عنصر القائمة بنجاح."));
        }

        [HttpDelete("navitems/{id}")]
        public async Task<IActionResult> DeleteNavItem(int id)
        {
            var ok = await _service.DeleteNavItemAsync(id);
            if (!ok) return NotFound(ApiResponse<object>.ErrorResult("عنصر القائمة غير موجود.", 404));
            return Ok(ApiResponse<object>.SuccessResult(null, "تم حذف عنصر القائمة بنجاح."));
        }

        // ===== Pages =====
        [HttpGet("navitems/{navItemId}/pages")]
        public async Task<IActionResult> GetPages(int navItemId)
        {
            var pages = await _service.GetPagesAsync(navItemId);
            // لو عايز تتحقق إن الـ NavItem موجود أصلاً: اعمل FindNavItemEntityAsync في السيرفس/الريبو واستعمله هنا
            return Ok(ApiResponse<List<Pages>>.SuccessResult(pages, "تم جلب الصفحات بنجاح."));
        }

        [HttpPost("navitems/{navItemId}/pages")]
        public async Task<IActionResult> AddPage(int navItemId, [FromBody]PageDto page)
        {
            var ok = await _service.AddPageAsync(navItemId, page);
            if (!ok) return NotFound(ApiResponse<Pages>.ErrorResult("عنصر القائمة غير موجود.", 404));
            return Ok(ApiResponse<PageDto>.SuccessResult(page, "تم إنشاء الصفحة بنجاح."));
        }

        [HttpPut("pages/{id}")]
        public async Task<IActionResult> UpdatePage(int id, [FromBody] PageDto page)
        {
            var ok = await _service.UpdatePageAsync(id, page);
            if (!ok) return NotFound(ApiResponse<Pages>.ErrorResult("الصفحة غير موجودة.", 404));
            return Ok(ApiResponse<PageDto>.SuccessResult(page, "تم تحديث الصفحة بنجاح."));
        }

        [HttpDelete("pages/{id}")]
        public async Task<IActionResult> DeletePage(int id)
        {
            var ok = await _service.DeletePageAsync(id);
            if (!ok) return NotFound(ApiResponse<object>.ErrorResult("الصفحة غير موجودة.", 404));
            return Ok(ApiResponse<object>.SuccessResult(null, "تم حذف الصفحة بنجاح."));
        }
    }
}
