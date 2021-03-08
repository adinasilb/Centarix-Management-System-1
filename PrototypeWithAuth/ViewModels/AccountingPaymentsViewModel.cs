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
        public RequestIndexPartialViewModel RequestIndexPartialViewModel { get; set; }

        public int PayNowListNum { get; set; }
    }
}
