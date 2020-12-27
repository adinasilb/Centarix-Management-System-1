using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserWithCentarixIDViewModel
    {
        public Employee Employee { get; set; }
        public string CentarixID { get; set; }
    }
}
