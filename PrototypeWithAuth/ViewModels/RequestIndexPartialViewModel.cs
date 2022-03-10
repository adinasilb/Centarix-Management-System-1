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
        public List<RequestIndexPartialRowViewModel> PagedList { get; set; }
        public List<IndexTab> Tabs { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public AppUtility.SidebarEnum SideBarType  { get; set; }
        public PricePopoverViewModel PricePopoverViewModel { get; set; }
        public CategoryPopoverViewModel CategoryPopoverViewModel{ get; set;}
        public int? PageNumber { get; set; }
        public LinkedList<PageNumbers> PageNumbersToShow { get; set; }
        public int RequestStatusID { get; set; }
        public string SidebarFilterID { get; set; }
        public string? SidebarFilterName { get; set; }
        public InventoryFilterViewModel InventoryFilterViewModel { get; set; }
        public RequestsSearchViewModel RequestsSearchViewModel { get; set; }
        public List<int> Months { get; set; }
        public List<int> Years { get; set; }
        public AppUtility.IndexTabs TabName { get; set; }

    }
}
