using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ShareModalViewModel
    {
        public int ID { get; set; } //universal pk for whatever object is being shared
        public string ObjectDescription { get; set; }
        public List<SelectListItem> ApplicationUsers { get; set; }
        public String ApplicationUserID { get; set; }
        public AppUtility.ModelsEnum ModelsEnum { get; set; }
        public AppUtility.MenuItems MenuItem { get; set; }
    }
}
