using Shared.DTOS.ComplaintDTOs;

namespace BLL.ServiceAbstraction
{
    public interface IComplaintService
    {
        // Complaint Management
        Task<List<ComplaintDTO>> GetAllComplaintsAsync();
        Task<ComplaintDTO> CreateComplaintAsync(CreateComplaintDTO createComplaintDto);
        Task<ComplaintDTO> UpdateComplaintAsync(int id, UpdateComplaintDTO updateComplaintDto);
        Task<bool> DeleteComplaintAsync(int id);
        Task<ComplaintDTO> UpdateComplaintStatusAsync(int id, ComplaintStatus status);

        // Statistics
        Task<object> GetComplaintStatisticsAsync();
        Task<int> GetTotalComplaintsCountAsync();
    }
}