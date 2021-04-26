using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddResourceViewModel
    {
        public int ResourceType { get; set; }
        public Resource Resource { get; set; }
        public List<ResourceCategory> ResourceCategories { get; set; }
    }
}
