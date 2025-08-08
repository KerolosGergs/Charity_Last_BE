using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOS.HomePageDTOS
{
    public class HeroSectionDTOs
    {
        public string MainTitle { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string Stats1Label { get; set; }
        public string Stats1Value { get; set; }
        public string Stats2Label { get; set; }
        public string Stats2Value { get; set; }
        public string Stats3Label { get; set; }
        public string Stats3Value { get; set; }
        public string Stats4Label { get; set; }
        public string Stats4Value { get; set; }
    }
    public class UpdateHeroSectionDTO
    {
        public string? MainTitle { get; set; }
        public IFormFile? BackgroundImageUrl { get; set; }
        public string? Stats1Label { get; set; }
        public int? Stats1Value { get; set; }
        public string Stats2Label { get; set; }
        public int? Stats2Value { get; set; }
        public string Stats3Label { get; set; }
        public int? Stats3Value { get; set; }
        public string Stats4Label { get; set; }
        public int? Stats4Value { get; set; }
    }
}
