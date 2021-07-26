using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class LineChange
    {

        public int LineID { get; set; }
        public Line Line { get; set; }
        public int ProtocolInstanceID { get; set; }
        public ProtocolInstance ProtocolInstance { get; set; }
        public string ChangeText { get; set; }
    }
}
