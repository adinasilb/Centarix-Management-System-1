using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        public string UserID { get; set; }
        public int UserNum { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
