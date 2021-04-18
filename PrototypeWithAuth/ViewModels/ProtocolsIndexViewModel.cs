using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PrototypeWithAuth.ViewModels
{
    public class ProtocolsIndexViewModel : ViewModelBase
    {
        public IPagedList<RequestIndexPartialRowViewModel> PagedList { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public PricePopoverViewModel PricePopoverViewModel { get; set; }
        public int? PageNumber { get; set; }
        public string SidebarFilterID { get; set; }
        public string? SidebarFilterName { get; set; }
        public ProtocolsInventoryFilterViewModel ProtocolsInventoryFilterViewModel { get; set; }

    }
}
