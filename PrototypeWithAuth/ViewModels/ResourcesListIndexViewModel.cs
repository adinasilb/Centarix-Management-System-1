using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.ViewModels
{
    public class ResourcesListIndexViewModel
    {
        public bool IsFavoritesPage { get; set; }
        public AppUtility.SidebarEnum SidebarEnum { get; set; }
        public List<ResourceWithFavorite> ResourcesWithFavorites { get; set; } //used this instead of dictionary so it won't crash if doubles are accidentally inserted
        public List<IconColumnViewModel> IconColumnViewModels { get; set; }
    }
}
