using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserWithCentarixIDViewModel : ViewModelBase
    {
        public Employee Employee {get; set;}
        public string CentarixID { get; set; }
    }
}
