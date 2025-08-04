using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS.AdviceRequestDTOs;
using Shared.DTOS.Common;
using BLL.ServiceAbstraction;
using Shared.DTOS.AdvisorDTOs;
using System.Security.Claims;
using BLL.Service;
using Shared.DTOS.NotificationDTOs;

namespace Charity_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdviceRequestController : ControllerBase
    {
        private readonly IAdviceRequestService _adviceRequestService;
        private readonly INotificationService _notificationService;
        private readonly IAdvisorService _advisorService;
        private readonly IEmailService _emailService;
        public AdviceRequestController(IAdviceRequestService adviceRequestService, INotificationService notificationService,IAdvisorService advisorService, IEmailService emailService)
        {
            _adviceRequestService = adviceRequestService;
            _notificationService = notificationService;
            _advisorService = advisorService;
            _emailService = emailService;
        }

        // GET: api/advicerequest
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetAllRequests()
        {
            try
            {
                var requests = await _adviceRequestService.GetAllRequestsAsync();
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/user
        [HttpGet("user")]
        //[Authorize]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetUserRequests()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ApiResponse<List<AdviceRequestDTO>>.ErrorResult("User not authenticated", 401));

                var requests = await _adviceRequestService.GetUserRequestsAsync(userId);
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/{id}
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> GetRequestById(int id)
        {
            try
            {
                var request = await _adviceRequestService.GetRequestByIdAsync(id);
                if (request == null)
                    return NotFound(ApiResponse<AdviceRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));

                return Ok(ApiResponse<AdviceRequestDTO>.SuccessResult(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult("Failed to retrieve request", 500));
            }
        }

        // POST: api/advicerequest
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> CreateRequest(
            [FromQuery] string userId,
            [FromBody] CreateAdviceRequestDTO createRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("Invalid input data", 400,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("User ID is required", 400));

                var request = await _adviceRequestService.CreateRequestAsync(userId, createRequestDto);


                if (request.AdvisorId.HasValue)
                {
                    var notification = new NotificationCreateDTO
                    {
                        UserId = request.AdvisorId.Value.ToString(),
                        Title = "تم إنشاء طلب جديد",
                        Message = $"هناك طلب من  {request.UserName} عنوانه: \"{request.Title}\"من النوع {request.ConsultationName}.",
                        Type = NotificationType.General
                    };

                    await _notificationService.AddNotificationAsync(notification);
                }

                return CreatedAtAction(nameof(GetRequestById), new { id = request.Id },
                    ApiResponse<AdviceRequestDTO>.SuccessResult(request, "Request created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult(ex.Message, 500));
            }
        }


        // PUT: api/advicerequest/{id}
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> UpdateRequest(
             int id,
             [FromQuery] string userId,
             [FromBody] UpdateAdviceRequestDTO updateRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("Invalid input data", 400));

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("User ID is required", 400));

            try
            {
                var request = await _adviceRequestService.UpdateRequestAsync(id, userId, updateRequestDto);
                if (request == null)
                    return NotFound(ApiResponse<AdviceRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));

                return Ok(ApiResponse<AdviceRequestDTO>.SuccessResult(request, "Request updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult("Failed to update request", 500));
            }
        }
        // DELETE: api/advicerequest/{id}
        [HttpDelete("{id}/{UserId}")]
        //[Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> CancelRequest(int id,string UserId)
        {
            try
            {
                //var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(UserId))
                    return Unauthorized(ApiResponse<bool>.ErrorResult("User not authenticated", 401));

                var result = await _adviceRequestService.CancelRequestAsync(id, UserId);
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResult($"Request with ID {id} not found", 404));

                return Ok(ApiResponse<bool>.SuccessResult(true, "Request cancelled successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult("Failed to cancel request", 500));
            }
        }

        // PUT: api/advicerequest/{id}/confirm
       [HttpPut("{id}/confirm")]
        //[Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> ConfirmRequest(int id, [FromQuery] string advisorId)
        {
            try
            {
                if (string.IsNullOrEmpty(advisorId))
                    return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("Advisor ID is required", 400));

                var request = await _adviceRequestService.ConfirmRequestAsync(id, advisorId);
                if (request == null)
                    return NotFound(ApiResponse<AdviceRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));

                var message = $"قام المستشار {request.AdvisorName} بتأكيد موعد الاستشارة بعنوان \"{request.Title}\"";

                // الحصول على ZoomLink لو الاستشارة Online
                string zoomLinkText = "";
                if (request.ConsultationType == ConsultationType.Online)
                {
                    var advisor = await _advisorService.GetAdvisorByIdAsync(request.AdvisorId.Value);
                    if (advisor != null && !string.IsNullOrEmpty(advisor.ZoomRoomUrl))
                    {
                        zoomLinkText = $"\nرابط الجلسة عبر Zoom: {advisor.ZoomRoomUrl}";
                        message += $"\nرابط الجلسة: {advisor.ZoomRoomUrl}";
                    }
                }

                var notification = new NotificationCreateDTO
                {
                    UserId = request.UserId,
                    Title = "تأكيد موعد الاستشارة",
                    Message = message,
                    Type = NotificationType.General
                };

                await _notificationService.AddNotificationAsync(notification);

                // إرسال بريد إلكتروني
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    string subject = "تأكيد موعد الاستشارة";
                    string body = $"<p>{message}</p>";

                    if (!string.IsNullOrEmpty(zoomLinkText))
                        body += $"<p>{zoomLinkText}</p>";

                    body += "<p>يرجى متابعة لوحة التحكم الخاصة بك للاطلاع على التفاصيل.</p>";

                    await _emailService.SendEmailAsync(request.UserName, subject, body);
                }

                return Ok(ApiResponse<AdviceRequestDTO>.SuccessResult(request, "Request confirmed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult("Failed to confirm request", 500));
            }
        }
        // PUT: api/advicerequest/{id}/complete
        [HttpPut("{id}/complete")]
        //[Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<AdviceRequestDTO>>> CompleteRequest(
             int id,
            [FromQuery] string advisorId,
            [FromBody] CompleteRequestDTO completeRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("Invalid input data", 400));

            try
            {
                if (string.IsNullOrEmpty(advisorId))
                    return BadRequest(ApiResponse<AdviceRequestDTO>.ErrorResult("Advisor ID is required", 400));

                var request = await _adviceRequestService.CompleteRequestAsync(id, advisorId, completeRequestDto);
                if (request == null)
                    return NotFound(ApiResponse<AdviceRequestDTO>.ErrorResult($"Request with ID {id} not found", 404));

                var message = $"تم الانتهاء من جلسة الاستشارة الخاصة بك بعنوان \"{request.Title}\".\nيرجى إرسال تقييمك عبر مركز الشكاوى والمقترحات.";

                var notification = new NotificationCreateDTO
                {
                    UserId = request.UserId,
                    Title = "تم الانتهاء من الاستشارة",
                    Message = message,
                    Type = NotificationType.General
                };

                await _notificationService.AddNotificationAsync(notification);

                // إرسال بريد إلكتروني
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    string subject = "تم الانتهاء من الاستشارة";
                    string body = $"<p>{message.Replace("\n", "<br>")}</p><p>نشكر لك للتواصل معنا.</p>";
                    await _emailService.SendEmailAsync(request.UserName, subject, body);
                }

                return Ok(ApiResponse<AdviceRequestDTO>.SuccessResult(request, "Request completed successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AdviceRequestDTO>.ErrorResult("Failed to complete request", 500));
            }
        }

        // GET: api/advicerequest/status/{status}
        [HttpGet("status/{status}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetRequestsByStatus(ConsultationStatus status)
        {
            try
            {
                var requests = await _adviceRequestService.GetRequestsByStatusAsync(status);
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/advisor/{advisorId}
        [HttpGet("advisor/{advisorId}")]
        //[Authorize(Roles = "Advisor")]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetRequestsByAdvisor(int advisorId)
        {
            try
            {
                var requests = await _adviceRequestService.GetRequestsByAdvisorAsync(advisorId);
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/consultation/{consultationId}
        [HttpGet("consultation/{consultationId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<AdviceRequestDTO>>>> GetRequestsByConsultation(int consultationId)
        {
            try
            {
                var requests = await _adviceRequestService.GetRequestsByConsultationAsync(consultationId);
                return Ok(ApiResponse<List<AdviceRequestDTO>>.SuccessResult(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AdviceRequestDTO>>.ErrorResult("Failed to retrieve requests", 500));
            }
        }

        // GET: api/advicerequest/statistics
        [HttpGet("statistics")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> GetRequestStatistics()
        {
            try
            {
                var statistics = await _adviceRequestService.GetRequestStatisticsAsync();
                return Ok(ApiResponse<object>.SuccessResult(statistics));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult("Failed to retrieve statistics", 500));
            }
        }
        
       
    }
}