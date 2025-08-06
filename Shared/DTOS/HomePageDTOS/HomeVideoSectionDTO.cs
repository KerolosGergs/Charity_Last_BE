using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.HomePageDTOS
{
    public class HomeVideoSectionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
    }
    public class UpdateHomeVideoSectionDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
    }
}
