using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AccountingPaymentsViewModel
    {
        public AppUtility.SidebarEnum AccountingEnum { get; set; }
        public ILookup<Vendor, Request> Requests { get; set; }

        public int PayNowListNum { get; set; }
    }
}
