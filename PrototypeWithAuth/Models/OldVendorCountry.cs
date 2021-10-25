using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class OldVendorCountry
    {
        [Key]
        public int OldVendorCountryID { get; set; }
        public int VendorID { get; set; }
        public string OldVendorCountryName { get; set; }
    }
}
