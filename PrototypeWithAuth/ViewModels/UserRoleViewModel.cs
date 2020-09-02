using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserRoleViewModel
    {
        public AppUtility.MenuItems MenuItemsID { get; set; }
        public bool Selected { get; set; } 
        public string Name { get; set; }
    }
}
