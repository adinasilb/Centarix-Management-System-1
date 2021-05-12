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
        public ApplicationUser SharedByApplicationUser { get; set; }
        public List<IconColumnViewModel> IconColumnViewModels { get; set; }
    }
}
