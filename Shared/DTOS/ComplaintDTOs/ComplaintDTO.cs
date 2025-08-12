using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.ComplaintDTOs
{
    public class ComplaintDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public ComplaintCategory Category { get; set; }
        public ComplaintStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Resolution { get; set; }
    }

    public class CreateComplaintDTO
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(25)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public ComplaintCategory Category { get; set; }
    }

    public class UpdateComplaintDTO
    {
        [StringLength(1000)]
        public string? Description { get; set; }
        public ComplaintCategory? Category { get; set; }
        public ComplaintStatus? Status { get; set; }
        public string? Resolution { get; set; }
    }

    public enum ComplaintStatus
    {
        Pending,
        InProgress,
        Resolved,
        Closed
    }

    public enum ComplaintCategory
    {
        Employee = 0, // شكوى عن موظف
        Service = 1,   // شكوى عن خدمة
        Facility = 2,  // شكوى عن مرفق
        Other = 3      // أخرى
    }
}