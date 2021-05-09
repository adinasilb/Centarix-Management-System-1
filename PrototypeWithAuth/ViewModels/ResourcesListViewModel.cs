using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ResourcesListViewModel
    {
        public List<String> PaginationTabs { get; set; }
        public bool IsFavoritesPage { get; set; }
        public List<ResourceWithFavorite> ResourcesWithFavorites { get; set; } //used this instead of dictionary so it won't crash if doubles are accidentally inserted
    }
}
