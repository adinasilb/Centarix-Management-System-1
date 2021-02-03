using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class OrdersByVendorViewModel
    {
        public ILookup<Vendor, Request> RequestsByVendor { get; set; }
        public List<PriceSortViewModel> PriceSortEnums { get; set; }
        public AppUtility.CurrencyEnum SelectedCurrency { get; set; }
        public ILookup<Vendor,List<string>> PriceSortEnumsList { get; set; }
        public List<string> SelectedPriceSort { get; set; }
    }
}
