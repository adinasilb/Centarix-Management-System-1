using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AccountingPaymentsViewModel : ViewModelBase
    {
        public AppUtility.SidebarEnum AccountingEnum { get; set; }
        public ILookup<Vendor, Request> Requests { get; set; }
        public RequestProtocolsIndexViewModel RequestIndexPartialViewModel { get; set; }
        public int PayNowListNum { get; set; }
        public List<PriceSortViewModel> PriceSortEnums { get; set; }
        public AppUtility.CurrencyEnum SelectedCurrency { get; set; }
        public List<string> SelectedPriceSort { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
    }
}
