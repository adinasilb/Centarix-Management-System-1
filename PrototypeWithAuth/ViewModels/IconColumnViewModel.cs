using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class IconColumnViewModel
    {
        public string IconClass { get; set; }
        public string Color { get; set; }
        public string IconAjaxLink { get; set; }
        public string IconUrlAction { get; set; }
        public string TooltipTitle { get; set; }
    }
}
