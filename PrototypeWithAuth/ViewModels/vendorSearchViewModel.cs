using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class VendorSearchViewModel : ViewModelBase
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public int SelectedParentCategoryID { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }

        [ MaxLength(50)]
        [Display(Name = "Supplier name [EN]")]
        public string VendorEnName { get; set; }

        [ MaxLength(50)]
        [Display(Name = "Supplier name [He]")]
        public string VendorHeName { get; set; }

        [ MinLength(9), MaxLength(9)]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "field must be a number")]
        [Display(Name = "Company ID")]
        public string VendorBuisnessID { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string InfoEmail { get; set; }
        [MaxLength(50)]
        [EmailAddress]
        public string OrdersEmail { get; set; }
        [Phone]
        [Display(Name = "Telephone")]
        public string VendorTelephone { get; set; }
        [Phone]
        [Display(Name = "Cell")]
        public string VendorCellPhone { get; set; }

        [Phone]
        [Display(Name = "Fax")]
        public string VendorFax { get; set; }
        [MaxLength(50)]
        [Display(Name = "City")]
        public string VendorCity { get; set; }
        [MaxLength(50)]
        [Display(Name = "Country")]
        public string VendorCountry { get; set; }
        [MaxLength(50)]
        [Display(Name = "Street")]
        public string VendorStreet { get; set; }

        [Display(Name = "Zip")]
        public string VendorZip { get; set; }

        [Display(Name = "Website")]
        public string VendorWebsite { get; set; }

        [ MaxLength(50)]
        [Display(Name = "Bank Name")]
        public string VendorBank { get; set; }

        [ MaxLength(4)]
        [Display(Name = "Branch")]
        public string VendorBankBranch { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "field must be a number")]
        [Display(Name = "Account Number")]
        public string VendorAccountNum { get; set; }

        [Display(Name = "Swift")]
        public string VendorSwift { get; set; }

        [Display(Name = "BIC")]
        public string VendorBIC { get; set; }

        [Display(Name = "Routing Number")]
        public string VendorRoutingNum { get; set; }

        [Display(Name = "Gold Account")]
        public string VendorGoldAccount { get; set; }

        public IEnumerable<Request> Requests { get; set; } //not sure if we need this 

        public IQueryable<Vendor> ReturnVendors { get; set; }
        public PrototypeWithAuth.AppData.AppUtility.MenuItems SectionType { get; set; }
    }
}
