using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ProtocolLink : ReportSection
    {
        public int ProtocolID { get; set; }
        public Protocol Protocol { get; set; }
    }
}
