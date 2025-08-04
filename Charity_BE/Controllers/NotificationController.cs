using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.ServiceAbstraction;
using Shared.DTOS.NotificationDTOs;
using Shared.DTOS.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        public NotificationController(INotificationService notificationService, IEmailService emailService)
        {
            _notificationService = notificationService;
            _emailService = emailService;
        }

        // GET: api/notification
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<NotificationDTO>>>> GetMyNotifications(
        [FromQuery] string userId,
        [FromQuery] bool onlyUnread = false)
        {
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponse<List<NotificationDTO>>.ErrorResult("User ID is missing", 401));

            var notifications = await _notificationService.GetUserNotificationsAsync(userId, onlyUnread);
            return Ok(ApiResponse<List<NotificationDTO>>.SuccessResult(notifications));
        }
        [HttpPatch("{notificationId}/read")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> MarkNotificationAsRead(int notificationId)
        {
            try
            {
                await _notificationService.MarkAsReadAsync(notificationId);
                return Ok(ApiResponse<string>.SuccessResult("Notification marked as read successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<string>.ErrorResult("Notification not found", 404));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult("An error occurred while marking the notification as read", 500));
            }
        }
        [HttpDelete("{notificationId}")]
        public ActionResult DeleteNotification(int notificationId)
        {
            var result = _notificationService.DeleteNotificationAsync(notificationId);
            if (result.Result)
            {
                return Ok(ApiResponse<string>.SuccessResult("Notification deleted successfully"));
            }
            else
            {
                return NotFound(ApiResponse<string>.ErrorResult("Notification not found", 404));
            }
        }
    }
} 