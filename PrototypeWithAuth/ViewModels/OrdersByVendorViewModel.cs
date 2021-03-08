using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class OrdersByVendorViewModel : ViewModelBase
    {
        public ILookup<Vendor, Request> RequestsByVendor { get; set; }
        public List<PriceSortViewModel> PriceSortEnums { get; set; }
        public AppUtility.CurrencyEnum SelectedCurrency { get; set; }
        public List<string> SelectedPriceSort { get; set; }
    }
}
