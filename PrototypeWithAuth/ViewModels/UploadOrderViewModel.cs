using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UploadOrderViewModel : ViewModelBase
    {
        public ParentRequest ParentRequest { get; set; }
        public List<string> FileStrings { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public RequestIndexObject RequestIndexObject { get; set;}

    }
}
