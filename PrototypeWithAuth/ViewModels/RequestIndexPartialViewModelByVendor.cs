using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialViewModelByVendor : ViewModelBase
    {
        public ILookup<Vendor, IEnumerable<RequestIndexPartialRowViewModel>> RequestsByVendor { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public List<PriceSortViewModel> PriceSortEnums { get; set; }
        public List<String> PriceSortEnumsList { get; set; }
        public AppUtility.CurrencyEnum SelectedCurrency { get; set; }
    }
}
