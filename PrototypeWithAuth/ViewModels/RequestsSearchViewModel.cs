using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestsSearchViewModel : ViewModelBase
    {
        public AppUtility.MenuItems SectionType { get; set; }
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public int? ParentCategoryID { get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public int? ProductSubcategoryID { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public int? VendorID { get; set; }
        public string ItemName { get; set; }
        public string ProductHebrewName { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate {get; set;}
        //public Request Request { get; set; }
/*        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }*/
        public int? Batch { get; set; }
        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }
        [Display(Name ="Date Created")]
        public DateTime? CreationDate { get; set; }
        public string Currency { get; set; }
        [Display(Name = "Arrival Date")]
        public DateTime ArrivalDate { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public string ApplicationUserReceiverID { get; set; }
        [Display(Name = "Quote Number")]
        public string QuoteNumber { get; set; }
        [Display(Name = "Catalog Number")]
        public string CatalogNumber { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }
        [Display(Name = "Order Date")]
        public DateTime? OrderDate { get; set; }
        [Display(Name = "Company Order Number")]
        public long? OrderNumber { get; set; }
        [Display(Name = "Supplier Order Number")]
        public string SupplierOrderNumber { get; set; }
    }
}
