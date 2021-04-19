using PrototypeWithAuth.AppData;
using System;

namespace PrototypeWithAuth.ViewModels
{
    public class IconPopoverViewModel
    {
        public IconPopoverViewModel(string? icon = "", string? color = "" , AppUtility.PopoverDescription description = AppUtility.PopoverDescription.More , string? action = "", string? controller = "", AppUtility.PopoverEnum currentLocation = AppUtility.PopoverEnum.None)
        {
            Icon = icon;
            Color = color;
            Description = description;
            Action = action;
            Controller = controller;
            CurrentLocation = currentLocation;
        }
        public String Icon { get; set; }
        public String Color { get; set; }
        public AppUtility.PopoverDescription Description { get; set; }
        public String Action { get; set; }
        public String Controller { get; set; }
        public AppUtility.PopoverEnum CurrentLocation { get; set; }
    }
}