using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Models
{
    public class Vendor : ModelBase, IEquatable<Vendor>
    {
        [Key]
        [Display(Name = "No")]
        public int VendorID { get; set; } 
        
        [Required, MaxLength(50)]
        [Display(Name = "Supplier name [EN]")]
        public string VendorEnName { get; set; }
        
        [Required, MaxLength (50)]
        [Display(Name = "Supplier name [He]")]
        public string VendorHeName { get; set; }
       
        [Required]
        [Display(Name = "Company ID")]
        public string VendorBuisnessID { get; set; }
        
        [MaxLength (50)]
        [EmailAddress]
        [Display(Name = "Info Email")]
        public string InfoEmail { get; set; }
        [Required]
        [MaxLength (50)]
        [EmailAddress]
        [Display(Name = "Orders Email")]
        public string OrdersEmail { get; set; }
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        [Display(Name = "Quotes Email")]
        public string QuotesEmail { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Telephone")]
        public string VendorTelephone { get; set; }
        [Phone]
        [Display(Name = "Cell")]
        public string VendorCellPhone { get; set; }
        
        [Phone]
        [Display(Name = "Fax")]
        public string VendorFax { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "City")]
        public string VendorCity { get; set; }
        //[Required]
        //[MaxLength(50)]
        //[Display(Name = "Country")]
        //public string VendorCountry { get; set; }
        
        [MaxLength(50)]
        [Display(Name = "Street")]
        public string VendorStreet { get; set; }
        
        [Display(Name = "Zip")]
        public string VendorZip { get; set; }
        
        [Display(Name = "Website")]
        public string VendorWebsite { get; set; }
        
        [MaxLength (50)]
        [Display(Name = "Bank Name")]
        public string VendorBank { get; set; }
        
        [MaxLength(4)]
        [Display(Name = "Branch")]
        public string VendorBankBranch { get; set; }
        
        
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "field must be a number")]
        [Display(Name = "Account Number")]
        public string VendorAccountNum{ get; set; }

        [RegularExpression("([0-9]*)", ErrorMessage = "field must be a number")]
        [Display(Name = "Routing Number")]
        public string VendorRoutingNum { get; set; }

        [Display(Name = "Swift")]
        public string VendorSwift { get; set; }
        
        [Display(Name = "BIC")]
        public string VendorBIC { get; set; }

        [Display(Name = "Gold Account")]
        public string VendorGoldAccount { get; set; }

        [Display(Name = "Country")]
        public int CountryID { get; set; }
        public Country Country { get; set; }
        public ListImplementsModelBase<VendorCategoryType> VendorCategoryTypes { get; set; }
        public ListImplementsModelBase<Product> Products { get; set; }

        public ListImplementsModelBase<VendorContact> VendorContacts { get; set; }
        public ListImplementsModelBase<VendorComment> VendorComments { get; set; }



        public bool Equals(Vendor other)
        {
            if (VendorID == other.VendorID)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = VendorID.GetHashCode();

            return hash;
        }

    }
}
