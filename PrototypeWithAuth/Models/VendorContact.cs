using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class VendorContact
    {
        [Key]
        public int VendorContactID { get; set; }
        [Required]
        [Display(Name = "Contact Name")]
        public string VendorContactName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Contact Email")]
        public string VendorContactEmail { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Contact Phone")]
        public string VendorContactPhone { get; set; }
        public int VendorID { get; set; }
        public Vendor Vendor { get; set; }

    }
}
