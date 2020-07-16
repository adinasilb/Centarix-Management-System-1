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
        public string VendorContactName { get; set; }
        public string VendorContactEmail { get; set; }
        public string VendorContactPhone { get; set; }
        public int VendorID { get; set; }
        public Vendor Vendor { get; set; }

    }
}
