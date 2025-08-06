using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOS.HomePageDTOS
{
    public class TrendSectionDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
    }
    public class UpdateTrendSectionDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
    }
}
