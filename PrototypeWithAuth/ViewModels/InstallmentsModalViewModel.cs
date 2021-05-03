using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class InstallmentsModalViewModel : ViewModelBase
    {
        public int RequestID { get; set; }
        public AppUtility.SidebarEnum AccountingPaymentsEnum { get; set; }
        public int Installments { get; set; }
        public DateTime InstallmentDate { get; set; }
    }
}

