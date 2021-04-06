using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class PaymentsPayModalViewModel : ViewModelBase
    {
        public List<Request> Requests { get; set; }
        public Payment Payment { get; set; }
        public List<CompanyAccount> CompanyAccounts {get; set;}
        public AppUtility.SidebarEnum AccountingEnum { get; set; }

    }
}
