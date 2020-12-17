using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class CostsProjectViewModel
    {
        public IEnumerable<ParentCategory> ParentCategoryList { get; set; }
        public int SelectedParentCategory { get; set; }
        public IEnumerable<ProductSubcategory> SubCategoryList { get; set; }
        public int SelectedProductSubCategory { get; set; }
        public IEnumerable<Vendor> VendorList {get; set;}
        public int SelectedVendor { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
