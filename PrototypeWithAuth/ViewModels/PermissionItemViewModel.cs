using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class PermissionItemViewModel : ViewModelBase
    {
        public AppUtility.MenuItems MenuItem { get; set; }
        public string ValueName { get; set; }
        public bool Selected { get; set; }
    }
}
