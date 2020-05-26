using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace PrototypeWithAuth.Models
{
    public class Vendor
    {
        [Key]
        [Display(Name = "No")]
        public int VendorID { get; set; } 
        
        [Required, MaxLength(50)]
        [Display(Name ="Supplier")]
        public string VendorEnName { get; set; }
        
        [MaxLength (50)]
        [Display(Name = "Name He")]
        public string VendorHeName { get; set; }
       
        [Required, MinLength (9), MaxLength(9)]
        [Display(Name = "Company ID")]
        public string VendorBuisnessID { get; set; }

        [MaxLength(50)]
        [Display(Name = "Contact Name")]
        public string ContactPerson { get; set; }
        
        [MaxLength (50)]
        [Display(Name = "Contact Mail")]
        public string ContactEmail { get; set; }
        
        [MaxLength (50)]
        [Display(Name = "Email Info")]
        public string OrderEmail { get; set; }
        
        [MinLength(9)]
        [Display(Name = "Tel")]
        public string VendorContactPhone1 { get; set; }
        
        [MinLength(9)]
        public string VendorContactPhone2 { get; set; }
        
        [MinLength(9)]
        [Display(Name = "Fax")]
        public string VendorFax { get; set; }
        
        [MaxLength(50)]
        [Display(Name = "City")]
        public string VendorCity { get; set; }
        
        [MaxLength(50)]
        [Display(Name = "Street")]
        public string VendorStreet { get; set; }
        
        [Display(Name = "Zip")]
        public string VendorZip { get; set; }
        
        [Display(Name = "Website")]
        public string VendorWebsite { get; set; }
        
        [Required, MaxLength (50)]
        [Display(Name = "Bank")]
        public string VendorBank { get; set; }
        
        [Required, MaxLength(4)]
        [Display(Name = "Branch")]
        public string VendorBankBranch { get; set; }
        
        [Required]
        [Display(Name = "Account Number")]
        public string VendorAccountNum{ get; set; }
        
        [Display(Name = "Swift")]
        public string VendorSwift { get; set; }
        
        [Display(Name = "BIC")]
        public string VendorBIC { get; set; }

        [Display(Name = "Gold Account")]
        public string VendorGoldAccount { get; set; }
        public IEnumerable<Product> Products { get; set; }


    }
}
