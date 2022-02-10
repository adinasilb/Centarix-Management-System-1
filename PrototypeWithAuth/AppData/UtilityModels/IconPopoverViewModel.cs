using PrototypeWithAuth.AppData;
using System;
using System.Text.Json.Serialization;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class IconPopoverViewModel
    {
        public IconPopoverViewModel(string icon = "", string color = "" , AppUtility.PopoverDescription description = AppUtility.PopoverDescription.More , string action = "", string controller = "",  string ajaxcall = "")
        {
            Icon = icon;
            Color = color;
            Description = description;
            Action = action;
            Controller = controller;
            AjaxCall = ajaxcall;
        }
        public String Icon { get; private set; }
        public String Color { get; private set; }
        public AppUtility.PopoverDescription Description { get; private set; }

        public string DescriptionDisplayName { get { return AppUtility.GetDisplayNameOfEnumValue(Description.ToString()); } }
        public String Action { get; private set; }
        public String Controller { get; private set; }
        public String AjaxCall { get; private set; }
    }
}