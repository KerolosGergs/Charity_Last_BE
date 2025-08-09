using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class NavItems
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string label { get; set; }
        [Required]
        public string href { get; set; }
        public List<Pages>? pages { get; set; }

    }
    public class Pages
    {
        public int Id { get; set; }
        public string subTilte { get; set; }
        public string subLink { get; set; }
        public int NavItemsId { get; set; }  // Foreign Key
        public NavItems NavItems { get; set; } // Navigation Property


    }
}
