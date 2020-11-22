using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ChartViewModel
    {
        public List<String> SectionName { get; set; }
        public List<String> SectionColor { get; set; }
        public List<double> SectionValue { get; set; }
        public String Currency { get; set; }
    }
}
