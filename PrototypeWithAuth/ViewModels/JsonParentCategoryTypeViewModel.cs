using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class JsonParentCategoryTypeViewModel
    {
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
    }
}
