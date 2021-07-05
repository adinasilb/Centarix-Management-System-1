using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ResourceWithFavorite
    {
        public Resource Resource { get; set; }
        public bool IsFavorite { get; set; }
        //public ShareResource ShareResouce { get; set; } //IWANT TO USE THIS IN THE FUTURE
        public ApplicationUser SharedByApplicationUser { get; set; }
        public int ShareResourceID { get; set; }
    }
}
