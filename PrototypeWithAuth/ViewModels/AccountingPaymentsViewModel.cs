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
        public AppUtility.AccountingPaymentsEnum AccountingPaymentsEnum { get; set; }
        public ILookup<Vendor, Request> Requests { get; set; }
    }
}
