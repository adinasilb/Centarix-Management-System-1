using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryTablesViewModel : ViewModelBase
    {
        public int CurrentYear { get; set; }
        public List<SummaryTableItem> SummaryTableItems { get; set; }
        public AppUtility.CurrencyEnum Currency { get; set; }
    }
}
