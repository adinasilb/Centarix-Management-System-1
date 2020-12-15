using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ExchangeRate
    {
        public int ExchangeRateID { get; set; }
        public Double LatestExchangeRate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
