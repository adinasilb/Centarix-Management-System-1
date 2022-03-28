using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels.ReactUtilityModels
{
    public class IndexTableJsonViewModel
    {
        public SelectedRequestFilters SelectedFilters { get; set; }
        public CategoryPopoverViewModel CategoryPopoverViewModel { get; set; }
        public PricePopoverViewModel PricePopoverViewModel { get; set; }
        public NavigationInfo NavigationInfo { get; set; }
        public string TabValue { get; set; }
        public int PageNumber { get; set; }
    }
}
