using Microsoft.AspNetCore.Routing;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class IconColumnViewModel : ViewModelBase
    {
        public IconColumnViewModel(string? iconClass = null, string? color = null, string? iconAjaxLink = null, string? tooltipTitle = null)
        {
            IconClass = iconClass;
            Color = color;
            IconAjaxLink = iconAjaxLink;
            TooltipTitle = tooltipTitle;
        }
        public string IconClass { get; private set; }
        public string Color { get; private set; }
        public string IconAjaxLink { get; private set; }
        public string TooltipTitle { get; private set; }
    }
}
