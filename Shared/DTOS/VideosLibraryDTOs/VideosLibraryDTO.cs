using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.VideosLibraryDTOs
{
    public class VideosLibraryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class CreateVideosLibraryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
    }
    public class UpdateVideosLibraryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
