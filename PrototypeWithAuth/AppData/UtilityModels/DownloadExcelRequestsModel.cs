using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class DownloadExcelRequestsModel
    {
        [Display (Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Subcategory Name")]
        public string SubCategoryName { get; set; }

        [Display(Name = "Vendor")]
        public string Vendor { get; set; }

        [Display(Name = "Expiration Date")]
        public string ExpirationDate { get; set; }

        [Display(Name = "Quote Expiration Date")]
        public string QuoteExpirationDate { get; set; }

        [Display(Name = "Order Date")]
        public string OrderDate { get; set; }

        [Display(Name = "Arrival Date")]
        public string ArrivalDate { get; set; }
    }
}
