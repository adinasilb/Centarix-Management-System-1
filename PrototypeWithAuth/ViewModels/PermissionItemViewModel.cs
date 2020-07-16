using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class PermissionItemViewModel
    {
        public AppUtility.MenuItems MenuItem { get; set; }
        public string ValueName { get; set; }
        public bool Selected { get; set; }
    }
}
