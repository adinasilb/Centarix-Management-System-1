using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryTablesViewModel
    {
        public int CurrentYear { get; set; }
        public List<SummaryTableItem> SummaryTableItems { get; set; }
    }
}
