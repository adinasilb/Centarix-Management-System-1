using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;

namespace PrototypeWithAuth.ViewModels
{
    public class AddNewItemViewModel
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public Product Product { get; set; }
        public Request Request { get; set; } // requests already include the product, do we still need to include products into the view model?
    }
}
