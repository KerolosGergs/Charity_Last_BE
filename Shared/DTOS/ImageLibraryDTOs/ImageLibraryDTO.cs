using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOS.ImageLibraryDTOs
{
    public class ImageLibraryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class CreateImageLibraryDTO
    {
        public string Name { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string Description { get; set; }
    }
    public class UpdateImageLibraryDTO 
    {
        public string Name { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
