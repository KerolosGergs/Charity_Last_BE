using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.DynamicPage
{
    public class DynamicPageDto
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public List<DynamicPageItemDto> Items { get; set; } = new List<DynamicPageItemDto>();
    }

    public class DynamicPageItemDto
    {
        public int Id { get; set; }
        public int DynamicPageId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? VideoUrl { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateDynamicPageDto
    {
        [Required]
        [StringLength(200)]
        public string PageName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Slug { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one content item is required")]
        public List<CreateDynamicPageItemDto> Items { get; set; } = new List<CreateDynamicPageItemDto>();
    }

    public class CreateDynamicPageItemDto
    {
        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(500)]
        public string? FileUrl { get; set; }

        [StringLength(255)]
        public string? FileName { get; set; }

        [StringLength(500)]
        public string? VideoUrl { get; set; }

        public int Order { get; set; }
    }

    public class UpdateDynamicPageDto
    {
        [Required]
        [StringLength(200)]
        public string PageName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Slug { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one content item is required")]
        public List<UpdateDynamicPageItemDto> Items { get; set; } = new List<UpdateDynamicPageItemDto>();
    }

    public class UpdateDynamicPageItemDto
    {
        public int? Id { get; set; } // null for new items
        public int DynamicPageId { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(500)]
        public string? FileUrl { get; set; }

        [StringLength(255)]
        public string? FileName { get; set; }

        [StringLength(500)]
        public string? VideoUrl { get; set; }

        public int Order { get; set; }
    }

    public class FileUploadResponseDto
    {
        public string Url { get; set; }
        public string FileName { get; set; }
    }

    public class DynamicPageListDto
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ItemsCount { get; set; }
        public string? CreatedByUserName { get; set; }
        public string? UpdatedByUserName { get; set; }
    }
} 