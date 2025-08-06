using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models.HomePage
{
    public class HomeVideoSection
    {
        public int Id { get; set; }
        public string VideoUrl { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
