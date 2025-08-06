using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models.HomePage
{
    public class HeroSection
    {
        public int Id { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string? MainTitle { get; set; }
        public string? Stats1Label { get; set; }
        public int? Stats1Value { get; set; }

        public string? Stats2Label { get; set; }
        public int? Stats2Value { get; set; }

        public string? Stats3Label { get; set; }
        public int? Stats3Value { get; set; }

        public string? Stats4Label { get; set; } 
        public int? Stats4Value { get; set; }
    }
}
