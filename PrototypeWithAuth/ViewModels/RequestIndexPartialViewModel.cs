using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialViewModel
    {
        public IPagedList<RequestIndexPartialRowViewModel> PagedList { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public List<PriceSortViewModel> PriceSortEnums { get; set; }
        public List<String> PriceSortEnumsList { get; set; }
        public int? PageNumber { get; set; }
        public int RequestStatusID { get; set; }
        public int NewCount { get; set; }
        public int ApprovedCount { get; set; }
        public int OrderedCount { get; set; }
        public int ReceivedCount { get; set; }
        public string SidebarFilterID { get; set; }
        public AppUtility.CurrencyEnum SelectedCurrency { get; set; }
        public string? SidebarFilterName { get; set; }
        public AppUtility.OrderStepsEnum OrderStepsEnum { get; set; }

    }
}
