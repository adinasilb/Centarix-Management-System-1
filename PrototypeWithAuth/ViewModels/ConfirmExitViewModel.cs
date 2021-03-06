using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    namespace PrototypeWithAuth.ViewModels
{
    public class ConfirmExitViewModel : ViewModelBase
    {
        public AppUtility.MenuItems SectionType { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public string URL { get; set; }
        public Guid GUID { get; set; }
    }
}
