using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ResourceWithFavorite
    {
        public Resource Resource { get; set; }
        public bool IsFavorite { get; set; }
    }
}
