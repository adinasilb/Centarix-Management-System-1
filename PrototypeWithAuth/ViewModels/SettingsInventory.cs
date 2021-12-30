using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SettingsInventory
    {
        public IEnumerable<ParentCategory> Categories { get; set; }
        public IEnumerable<ProductSubcategory> Subcategories { get; set; }
    }
}
