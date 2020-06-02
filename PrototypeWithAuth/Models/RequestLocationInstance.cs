using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RequestLocationInstance
    {
        public int RequestID { get; set; }
        public Request Request { get; set; }
        public int LocationInstanceID { get; set; }
        public LocationInstance LocationInstance { get; set; }
        public bool IsDeleted { get; set; }
    }
}
