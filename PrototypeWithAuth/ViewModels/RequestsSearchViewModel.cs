using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestsSearchViewModel : ViewModelBase
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public string InvoiceNumer { get; set; }
        public DateTime? InvoiceDate {get; set;}
        public string ItemName { get; set; }
        public int? VendorID { get; set; }
        public int? ParentCategoryID { get; set; }
        public int? ProductSubCategoryID { get; set; }
    }
}
