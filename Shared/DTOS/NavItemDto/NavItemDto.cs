using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.NavItemDto
{
    public class PageDto { public string subTilte { get; set; } public string subLink { get; set; } }
    public class NavItemDto { public string label { get; set; } public string href { get; set; } public List<PageDto> pages { get; set; } }

}
