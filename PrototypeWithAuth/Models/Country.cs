using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int CurrencyID { get; set; }
        public Currency Currency { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
    }
}
