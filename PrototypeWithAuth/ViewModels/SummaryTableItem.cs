using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryTableItem
    {
        public DateTime Month { get; set; }
        public int Salary { get; set; }
        public int Lab { get; set; }
        public int Operation { get; set; }
        public int Instrument { get; set; }
        public int Reagents { get; set; }
    }
}
