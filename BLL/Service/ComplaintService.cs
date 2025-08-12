using AutoMapper;
using BLL.ServiceAbstraction;
using DAL.Repositories.RepositoryIntrfaces;
using DAL.Data.Models;
using Shared.DTOS.ComplaintDTOs;
using Microsoft.EntityFrameworkCore;
using Shared.DTOS.NotificationDTOs;
using Microsoft.AspNetCore.Identity;
using DAL.Data.Models.IdentityModels;

namespace BLL.Service
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComplaintService(
            IComplaintRepository complaintRepository,
            IMapper mapper,
            INotificationService notificationService,
            IEmailService emailService,
            UserManager<ApplicationUser> userManager)
        {
            _complaintRepository = complaintRepository;
            _mapper = mapper;
            _notificationService = notificationService;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<List<ComplaintDTO>> GetAllComplaintsAsync()
        {
            var complaints = await _complaintRepository.GetAllComplaintsAsync();
            return _mapper.Map<List<ComplaintDTO>>(complaints);
        }

        public async Task<ComplaintDTO> CreateComplaintAsync(CreateComplaintDTO createComplaintDto)
        {
            var complaint = _mapper.Map<Complaint>(createComplaintDto);
            complaint.Status = ComplaintStatus.Pending;
            complaint.CreatedAt = DateTime.UtcNow;

            var createdComplaint = await _complaintRepository.AddAsync(complaint);

            // جلب كل الأدمنز
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            foreach (var admin in admins)
            {
                var notification = new NotificationCreateDTO
                {
                    UserId = admin.Id,
                    Title = "شكوى جديدة",
                    Message = "تم تقديم شكوى جديدة",
                    Type = NotificationType.Complaint
                };

                await _notificationService.AddNotificationAsync(notification);
                await _emailService.SendEmailAsync(admin.Email, notification.Title, notification.Message);
            }

            return _mapper.Map<ComplaintDTO>(createdComplaint);
        }

        public async Task<ComplaintDTO> UpdateComplaintAsync(int id, UpdateComplaintDTO updateComplaintDto)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return null;

            if (!string.IsNullOrEmpty(updateComplaintDto.Description))
                complaint.Description = updateComplaintDto.Description;

            if (updateComplaintDto.Category.HasValue)
                complaint.Category = updateComplaintDto.Category.Value;

            if (updateComplaintDto.Status.HasValue)
                complaint.Status = updateComplaintDto.Status.Value;

            if (!string.IsNullOrEmpty(updateComplaintDto.Resolution))
                complaint.Resolution = updateComplaintDto.Resolution;

            complaint.UpdatedAt = DateTime.UtcNow;

            if (complaint.Status == ComplaintStatus.Resolved && !complaint.ResolvedAt.HasValue)
                complaint.ResolvedAt = DateTime.UtcNow;

            var updatedComplaint = await _complaintRepository.UpdateAsync(complaint);
            return _mapper.Map<ComplaintDTO>(updatedComplaint);
        }

        public async Task<bool> DeleteComplaintAsync(int id)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return false;

            await _complaintRepository.DeleteAsync(id);
            return true;
        }

        public async Task<ComplaintDTO> UpdateComplaintStatusAsync(int id, ComplaintStatus status)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return null;

            complaint.Status = status;
            complaint.UpdatedAt = DateTime.UtcNow;

            if (status == ComplaintStatus.Resolved && !complaint.ResolvedAt.HasValue)
                complaint.ResolvedAt = DateTime.UtcNow;

            var updatedComplaint = await _complaintRepository.UpdateAsync(complaint);

            return _mapper.Map<ComplaintDTO>(updatedComplaint);
        }

        public async Task<object> GetComplaintStatisticsAsync()
        {
            var complaints = await _complaintRepository.GetAllAsync();

            return new
            {
                TotalComplaints = complaints.Count(),
                PendingComplaints = complaints.Count(c => c.Status == ComplaintStatus.Pending),
                InProgressComplaints = complaints.Count(c => c.Status == ComplaintStatus.InProgress),
                ResolvedComplaints = complaints.Count(c => c.Status == ComplaintStatus.Resolved),
                ClosedComplaints = complaints.Count(c => c.Status == ComplaintStatus.Closed)
            };
        }

        public async Task<int> GetTotalComplaintsCountAsync()
        {
            return await _complaintRepository.CountAsync();
        }
    }

}