using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class VendorDetailsViewModel : ViewModelBase
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int SelectedParentCategoryID { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public Vendor Vendor { get; set; }
        public IEnumerable<Request> Requests { get; set; } //not sure if we need this 

        public IQueryable<Vendor> ReturnVendors { get; set; }
    }
}
