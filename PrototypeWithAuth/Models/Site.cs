using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Site
    {
        [Key]
        public int SiteID { get; set; }
        public string Name { get; set; }
        public string Line1Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string PrimaryContactID { get; set; }
        public ApplicationUser PrimaryContact { get; set; }
        public List<Test> Tests { get; set; }

    }
}
