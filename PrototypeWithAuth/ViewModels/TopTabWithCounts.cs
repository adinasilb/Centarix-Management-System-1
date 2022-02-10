using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class TopTabWithCounts
    {
        public string Name { get; set; }
        public string Page { get; set; }
        public BoolIntViewModel Counts { get; set; }
    }
}
