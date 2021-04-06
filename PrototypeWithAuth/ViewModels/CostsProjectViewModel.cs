using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class CostsProjectViewModel : ViewModelBase
    {
        public string ItemName { get; set; }
        public IEnumerable<ParentCategory> ParentCategoryList { get; set; }
        public int SelectedParentCategory { get; set; }
        public IEnumerable<ProductSubcategory> SubCategoryList { get; set; }
        public int SelectedProductSubCategory { get; set; }
        public IEnumerable<Vendor> VendorList {get; set;}
        public int SelectedVendor { get; set; }
        public string SelectedVendorNumber { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public String CatalogNumber { get; set; }
        public String SerialNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public String BRNumber { get; set; }
        public double Price { get; set; }
        public String SearchName { get; set; }
    }
}
