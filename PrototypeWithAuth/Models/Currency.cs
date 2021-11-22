using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Currency
    {
        [Key]
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}
