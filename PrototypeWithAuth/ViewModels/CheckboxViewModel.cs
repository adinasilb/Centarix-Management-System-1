using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class CheckboxViewModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public bool Value { get; set; }
        public double CostShekel { get; set; }
        public double CostDollar { get; set; }
        public string Currency { get; set; }
        //public bool Required { get; set; }
    }
}
