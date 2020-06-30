using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ParentRequestListViewModel
    {
        public double SumRequestCost { get; set; }
        public ParentRequest ParentRequest { get; set; }
        public Request Request { get; set; }
        public string ProductName { get; set; }
        public string ParentCategoryDescription { get; set; }
        public string ProductSubcategoryDescription { get; set; }
        public string VendorEnName { get; set; }
        public double Cost { get; set; }
        public DateTime DateToBePaid { get; set; }
    }
}
