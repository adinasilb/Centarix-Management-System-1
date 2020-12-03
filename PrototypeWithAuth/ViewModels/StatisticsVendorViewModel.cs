using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class StatisticsVendorViewModel
    {
        public Dictionary<Vendor, List<Request>> Vendors { get; set; }
        public List<CategoryType> CategoryTypes { get; set; }
        public List<CategoryType> CategoryTypesSelected { get; set; }
        public List<int> Months { get; set; }
        public List<int> Years { get; set; }
    }
}
