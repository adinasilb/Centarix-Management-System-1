using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class IpRange : ModelBase
    {
        public int IpRangeID { get; set; }
        public string FromIpAddress { get; set; }
        public string ToIpAddress { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

