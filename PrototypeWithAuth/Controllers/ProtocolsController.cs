using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Controllers
{
    public class ProtocolsController : Controller
    {
        public async Task<IActionResult> Index(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.ProtocolsWorkflow, AppUtility.SidebarEnum SidebarType = AppUtility.SidebarEnum.CurrentProtocols)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = SidebarType;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            return View();
        }
    }
}
