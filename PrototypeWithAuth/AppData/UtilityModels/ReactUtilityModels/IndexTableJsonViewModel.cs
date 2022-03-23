﻿using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels.ReactUtilityModels
{
    public class IndexTableJsonViewModel
    {
        public InventoryFilterViewModel InventoryFilterViewModel { get; set; }
        public CategoryPopoverViewModel CategoryPopoverViewModel { get; set; }
        public PricePopoverViewModel PricePopoverViewModel { get; set; }
        public NavigationInfo NavigationInfo { get; set; }
        public TabInfo TabInfo { get; set; }
        public int PageNumber { get; set; }
    }
}
