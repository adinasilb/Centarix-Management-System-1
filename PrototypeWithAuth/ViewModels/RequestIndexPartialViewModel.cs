using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialViewModel : ViewModelBase
    {
        public string ControllerName { get; set; }
        public IPagedList<RequestIndexPartialRowViewModel> PagedList { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public PricePopoverViewModel PricePopoverViewModel { get; set; }
        public CategoryPopoverViewModel CategoryPopoverViewModel{ get; set;}
        public int? PageNumber { get; set; }
        public int RequestStatusID { get; set; }
        public int NewCount { get; set; }
        public int ApprovedCount { get; set; }
        public int OrderedCount { get; set; }
        public int ReceivedCount { get; set; }
        public int NonProprietaryCount { get; set; }
        public int ProprietaryCount { get; set; }
        public string SidebarFilterID { get; set; }
        public string? SidebarFilterName { get; set; }
        public InventoryFilterViewModel InventoryFilterViewModel { get; set; }

    }
}
