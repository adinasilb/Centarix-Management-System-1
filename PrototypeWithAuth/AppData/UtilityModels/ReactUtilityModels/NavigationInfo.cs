using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels.ReactUtilityModels
{
    public class NavigationInfo
    {
        public AppUtility.MenuItems SectionType { get; set; }
        public AppUtility.SidebarEnum SideBarType { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
    }
}
