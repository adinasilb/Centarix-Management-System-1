using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class AccountingPopoverLink
    {
        public String Icon { get; set; }
        public String Color { get; set; }
        public AppUtility.PaymentsPopoverEnum Description {get; set;}
        public String Action { get; set; }
        public String Controller { get; set; }
        public AppUtility.PaymentsPopoverEnum CurrentLocation { get; set; }
    }
}
