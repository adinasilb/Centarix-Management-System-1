using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UpdatePaymentStatusInfo
    {
        public AppUtility.PaymentsPopoverEnum NewStatus { get; set; }
        public int? NewInstallmentAmount { get; set; }
    }
}
