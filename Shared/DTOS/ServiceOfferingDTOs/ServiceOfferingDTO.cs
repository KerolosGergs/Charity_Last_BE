using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.ServiceOfferingDTOs
{
    public class ServiceOfferingDTO
    {
        [Key]
        public int Id { get; set; } = 1;
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description {  get; set; }
        public List<ServiceOfferingDTOItem> ServiceItem { get; set; } 
    }
    public class ServiceOfferingDTOItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
  
    }

    public class CreateServiceOfferingDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<CreateServiceOfferingDTOItem> ServiceItem { get; set; }

    }
    public class CreateServiceOfferingDTOItem
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Url { get; set; }

        public IFormFile Image { get; set; }


        public bool IsActive { get; set; }

    

    }

    public class UpdateServiceOfferingDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<UpdateServiceOfferingDTOItem> ServiceItem { get; set; }
    }
    public class UpdateServiceOfferingDTOItem
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }


        public bool? IsActive { get; set; }

        public IFormFile? Image { get; set; }

    }
    public class UpdateTitleDescriptionDTO
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
    }

}