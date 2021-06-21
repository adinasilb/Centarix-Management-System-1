using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialColumnViewModel
    {
        public string Title { get; set; } = "";
        public int Width { get; set; } = 0;
        public List<string> Value { get; set; } = new List<string>() { "" };
        public IEnumerable<IconColumnViewModel> Icons { get; set; }
        public int AjaxID { get; set; } = 0;
        public string Image { get; set; } = "";
        public string AjaxLink { get; set; } = "";
        public AppUtility.FilterEnum FilterEnum { get; set; } = AppUtility.FilterEnum.None;
        public string Note { get; set; } = "";
        public bool ShowTooltip { get; set; }

    }
}
