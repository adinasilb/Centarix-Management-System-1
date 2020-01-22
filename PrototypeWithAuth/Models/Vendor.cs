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
        public int VendorID { get; set; } 
        [Required, MaxLength(50)]
        public string VendorEnName { get; set; }
        [MaxLength (50)]
        public string VendorHeName { get; set; }
        [Required, MinLength (9), MaxLength(9)]
        public string VendorBuisnessID { get; set; }

        //[MaxLength(50)]
        //public List<VendorTypeVendor> VendorTypeVendors  { get; set; } // setting up the many to many relatiosnship with vendortype, making a list for it
        [MaxLength(50)]
        public string ContactPerson { get; set; }
        [MaxLength (50)]
        public string ContactEmail { get; set; }
        [MaxLength (50)]
        public string OrderEmail { get; set; }
        [MinLength(9)]
        public string VendorContactPhone1 { get; set; }
        [MinLength(9)]
        public string VendorContactPhone2 { get; set; }
        [MinLength(9)]
        public string VendorFax { get; set; }
        [MaxLength(50)]
        public string VendorCity { get; set; }
        [MaxLength(50)]
        public string VendorStreet { get; set; }
        public string VendorZip { get; set; }
        public string VendorWebsite { get; set; }
        [Required, MaxLength (50)]
        public string VendorBank { get; set; }
        [Required, MaxLength(4)]
        public string VendorBankBranch { get; set; }
        [Required]
        public string VendorAccountNum{ get; set; }
        public string VendorSwift { get; set; }
        public string VendorBIC { get; set; }
        public string VendorGoldAccount { get; set; }


    }
}
