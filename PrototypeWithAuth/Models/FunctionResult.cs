using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FunctionResult :  FunctionBase
    {
        public int ProtocolInstanceID { get; set; }
        public ProtocolInstance ProtocolInstance { get; set; }
        public bool IsTemporary { get; set; }
    }
}
