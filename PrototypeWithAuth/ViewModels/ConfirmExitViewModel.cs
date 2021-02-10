using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    namespace PrototypeWithAuth.ViewModels
{
    public class ConfirmExitViewModel
    {
        public AppUtility.MenuItems SectionType { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public string URL { get; set; }
    }
}
