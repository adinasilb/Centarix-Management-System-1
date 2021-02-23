using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserItemViewModel : ViewModelBase
    {
        public ApplicationUser ApplicationUser { get; set; }
    }
}
