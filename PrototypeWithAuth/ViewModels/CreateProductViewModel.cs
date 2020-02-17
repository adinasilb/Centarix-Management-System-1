using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class CreateProductViewModel
    {
        public IEnumerable<ParentCategory>  ParentCategories{ get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }
        public bool IsNewItem { get; set; }
    }
}
