using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ParentRequestListViewModel : ViewModelBase
    {
        //public double SumRequestCost { get; set; }
        public ParentRequest ParentRequest { get; set; } //because this is set in the controller as a .select list then the normally included models need to be put in here
        public Request Request { get; set; }
        public Product Product { get; set; }
        public ProductSubcategory ProductSubcategory { get; set; }
        public Vendor Vendor { get; set; }
        public ParentCategory ParentCategory { get; set; }
        public UnitType UnitType { get; set; }
        public UnitType SubUnitType { get; set; }
        public UnitType SubSubUnitType { get; set; }
    }
}
