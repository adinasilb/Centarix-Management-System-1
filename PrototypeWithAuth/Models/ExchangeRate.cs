using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ExchangeRate
    {
        public int ExchangeRateID { get; set; }
        public decimal LatestExchangeRate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
