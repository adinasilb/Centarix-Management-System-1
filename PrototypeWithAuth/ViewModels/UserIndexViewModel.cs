using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserIndexViewModel
    {
        public IEnumerable<PrototypeWithAuth.Data.ApplicationUser> ApplicationUsers { get; set; }
        public bool IsCEO { get; set; }
    }
}
