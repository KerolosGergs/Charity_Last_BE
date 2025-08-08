using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Models
{
    public class ServiceOffering
    {
        [Key]
        public int Id { get; set; } = 1;
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }


        public List<ServiceOfferingItem> ServiceItem {get;set;}
    }
    public class ServiceOfferingItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Url { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        public int ServiceOfferingId { get; set; }

        [ForeignKey("ServiceOfferingId")]
        public ServiceOffering ServiceOffering { get; set; }


    }

}

