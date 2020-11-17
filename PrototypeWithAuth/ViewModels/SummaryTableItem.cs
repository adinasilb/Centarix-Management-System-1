using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryTableItem
    {
        public DateTime Month { get; set; }
        public string Salary { get; set; }
        public string Lab { get; set; }
        public string Operation { get; set; }
        public string Instrument { get; set; }
        public string Reagents { get; set; }
        public string Plastics { get; set; }
        public string Reusable { get; set; }
        public string Total { get; set; }
    }
}
