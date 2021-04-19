using PrototypeWithAuth.AppData;
using System;

namespace PrototypeWithAuth.ViewModels
{
    public class IconPopoverViewModel
    {
        public IconPopoverViewModel(string? icon = "", string? color = "" , AppUtility.PopoverDescription description = AppUtility.PopoverDescription.More , string? action = "", string? controller = "", AppUtility.PopoverEnum currentLocation = AppUtility.PopoverEnum.None, string? ajaxcall = "")
        {
            Icon = icon;
            Color = color;
            Description = description;
            Action = action;
            Controller = controller;
            CurrentLocation = currentLocation;
            AjaxCall = ajaxcall;
        }
        public String Icon { get; private set; }
        public String Color { get; private set; }
        public AppUtility.PopoverDescription Description { get; private set; }
        public String Action { get; private set; }
        public String Controller { get; private set; }
        public AppUtility.PopoverEnum CurrentLocation { get; private set; }
        public String AjaxCall { get; private set; }
    }
}