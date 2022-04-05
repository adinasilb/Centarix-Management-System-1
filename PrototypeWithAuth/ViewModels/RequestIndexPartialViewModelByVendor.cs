using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialViewModelByVendor : ViewModelBase
    {
        public ILookup<Vendor, RequestIndexPartialRowViewModel> RequestsByVendor { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public AppUtility.SidebarEnum SidebarType { get; set; }
        public PricePopoverViewModel PricePopoverViewModel { get; set; }
        public CategoryPopoverViewModel CategoryPopoverViewModel { get; set; }
        public SelectedRequestFilters SelectedRequestFilters { get; set; }
        public InventoryFilterViewModel InventoryFilterViewModel { get; set; }
        public string ButtonText { get;set; }
        public string AjaxLink { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
    }
}
