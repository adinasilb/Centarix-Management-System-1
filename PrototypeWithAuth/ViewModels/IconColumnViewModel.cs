using Microsoft.AspNetCore.Routing;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class IconColumnViewModel : ViewModelBase
    {
        public IconColumnViewModel(int? iconColumnViewModelID, string? iconClass = null, string? color = null, string? iconAjaxLink = null, string? tooltipTitle = null)
        {
            IconColumnViewModelID = iconColumnViewModelID ?? 0;
            IconClass = iconClass;
            Color = color;
            IconAjaxLink = iconAjaxLink;
            TooltipTitle = tooltipTitle;
        }
        public int IconColumnViewModelID { get; set; }
        public string IconClass { get; private set; }
        public string Color { get; private set; }
        public string IconAjaxLink { get; private set; }
        public string TooltipTitle { get; private set; }
    }
}
