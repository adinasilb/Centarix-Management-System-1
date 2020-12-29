using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public class RequestIndexObject
    {
        public int PageNumber { get; set} 
        public int RequestStatusID { get; set; }
        public AppUtility.SidebarEnum SidebarType { get; set; }
        public String SidebarFilterID { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public List<String> SelectedPriceSort { get; set; }
        public AppUtility.CurrencyEnum SelectedCurrency { get; set; }
         //ExpensesFilter = null, List<int> CategoryTypeIDs = null, List<int> Months = null, List<int> Years = null
    }
}
