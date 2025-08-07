using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Models.IdentityModels;

namespace DAL.Data.Models.IdentityModels
{
    public class DynamicPage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string PageName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Slug { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [StringLength(450)]
        public string? CreatedBy { get; set; }

        [StringLength(450)]
        public string? UpdatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<DynamicPageItem> Items { get; set; } = new List<DynamicPageItem>();

        // Foreign Key Relationships

    }

    public class DynamicPageItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DynamicPageId { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } // "text", "image_text", "file"

        [Required]
        public string Content { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(500)]
        public string? FileUrl { get; set; }

        [StringLength(255)]
        public string? FileName { get; set; }

        public int Order { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey("DynamicPageId")]
        public virtual DynamicPage DynamicPage { get; set; }
    }
}