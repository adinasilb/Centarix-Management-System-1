using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserRoleViewModel : ViewModelBase
    {
        public int MenuItemsID { get; set; }
        public bool Selected { get; set; }
        public StringWithName StringWithName { get; set; }

        //public List<UserRoleViewModel> UserRoleViewModels { get; set }
    }
}
