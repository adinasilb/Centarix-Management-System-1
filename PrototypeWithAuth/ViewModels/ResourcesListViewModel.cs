using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ResourcesListViewModel
    {
        public List<String> PaginationTabs { get; set; }
        public ResourcesListIndexViewModel ResourcesListIndexViewModel { get; set; }
    }
}
