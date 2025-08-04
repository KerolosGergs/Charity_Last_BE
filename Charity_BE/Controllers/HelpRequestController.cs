using Microsoft.AspNetCore.Mvc;
using BLL.ServiceAbstraction;
using Shared.DTOS.HelpDTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using BLL.Service;
using Microsoft.AspNetCore.Identity;
using Shared.DTOS.NotificationDTOs;

namespace Charity_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpRequestController : ControllerBase
    {
        private readonly IHelpRequestService _service;
        IAdminService _adminService;
        INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public HelpRequestController(IHelpRequestService service, IAdminService adminService, INotificationService notificationService, IEmailService emailService)
        {
            _service = service;
            _adminService = adminService;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        // POST: api/helprequest
        [HttpPost]
        public async Task<ActionResult<HelpRequestDTO>> Create([FromBody] CreateHelpRequestDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            var admins = await _adminService.GetAllAdminsAsync();

            foreach (var admin in admins)
            {
                var notification = new NotificationCreateDTO
                {
                    UserId = admin.UserId,
                    Title = "طلب مساعدة جديد",
                    Message = $"قام المستخدم {dto.Name}. يُرجى مراجعته.",
                    Type = NotificationType.General
                };

                await _notificationService.AddNotificationAsync(notification);
                if (!string.IsNullOrEmpty(admin.Email))
                {
                    string subject = "طلب مساعدة جديد";
                    string body = $"قام المستخدم <b>{dto.Name} {dto.Email}</b>  بارسال طلب مساعدة جديد. يُرجى مراجعته في أقرب وقت.";
                    await _emailService.SendEmailAsync(admin.Email, subject, body);
                }
            }

            return CreatedAtAction(nameof(GetAll), null, created);
        }
        // GET: api/helprequest
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<HelpRequestDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
    }
}