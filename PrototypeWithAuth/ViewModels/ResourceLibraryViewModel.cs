using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ResourceLibraryViewModel
    {
        public int PageType { get; set; }
        public IEnumerable<ResourceCategory> ResourceCategories { get; set; }
    }
}
