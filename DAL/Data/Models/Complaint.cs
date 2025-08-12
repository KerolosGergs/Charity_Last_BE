using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models.IdentityModels;
using Shared.DTOS.ComplaintDTOs;

namespace DAL.Data.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }
        [Required]
        [StringLength(25)]
        public string PhoneNumber { get; set; }

        [Required]
        public ComplaintCategory Category { get; set; }

        [Required]
        public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }

        [StringLength(2000)]
        public string? Resolution { get; set; }

    }

    //public enum ComplaintStatus
    //{
    //    Pending,
    //    InProgress,
    //    Resolved,
    //    Closed
    //}
}