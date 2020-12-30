using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public static class CreateMainMenu
    {
        public static IEnumerable<Menu> GetMainMenu()
        {
            return new List<Menu>()
            {
                new  Menu()
                {
                    menuID = 1,
                    MenuDescription = AppUtility.MenuItems.Requests.ToString(),
                    MenuViewName = "Orders & Inventory",
                    ControllerName = "Requests",
                    ActionName = "Index",
                    RouteValues = new Microsoft.AspNetCore.Routing.RouteValueDictionary()
                    {
                        {"PageType",  AppUtility.PageTypeEnum.RequestRequest
    },
                        { "SectionType", AppUtility.MenuItems.Requests
},
                        { "SidebarType", AppUtility.SidebarEnum.List }
                    },
                    MenuImageURL = "/images/css/main_menu_icons/inventory.png"
                },
                new Menu()
                {
                    menuID = 2,
                    MenuDescription = AppUtility.MenuItems.Protocols.ToString(),
                    MenuViewName = "Protocols",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/protocols.png"
                },
                new Menu()
                {
                    menuID = 3,
                    MenuDescription = AppUtility.MenuItems.Operations.ToString(),
                    MenuViewName = "Operation",
                    ControllerName = "Requests",
                    ActionName = "Index",
                    RouteValues = new Microsoft.AspNetCore.Routing.RouteValueDictionary()
                    {
                        {"PageType",  AppUtility.PageTypeEnum.OperationsRequest },
                        { "SectionType", AppUtility.MenuItems.Operations },
                        { "SidebarType", AppUtility.SidebarEnum.List }
                    },
                    MenuImageURL = "/images/css/main_menu_icons/operation.png"
                },
                new Menu()
                {
                    menuID = 4,
                    MenuDescription = AppUtility.MenuItems.Biomarkers.ToString(),
                    MenuViewName = "Biomarkers",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/biomarkers.png"
                },
                new Menu()
                {
                    menuID = 5,
                    MenuDescription = AppUtility.MenuItems.TimeKeeper.ToString(),
                    MenuViewName = "Timekeeper",
                    ControllerName = "Timekeeper",
                    ActionName = "ReportHours",
                    MenuImageURL = "/images/css/main_menu_icons/timekeeper.png"
                },
                new Menu()
                {
                    menuID = 6,
                    MenuDescription = AppUtility.MenuItems.LabManagement.ToString(),
                    MenuViewName = "Lab Management",
                    ControllerName = "Vendors",
                    ActionName = "IndexForPayment",
                    MenuImageURL = "/images/css/main_menu_icons/lab.png"
                },
                new Menu()
                {
                    menuID = 7,
                    MenuDescription = AppUtility.MenuItems.Accounting.ToString(),
                    MenuViewName = "Accounting",
                    ControllerName = "Requests",
                    ActionName = "AccountingPayments",
                    MenuImageURL = "/images/css/main_menu_icons/accounting.png"
                },
                new Menu()
                {
                    menuID = 8,
                    MenuDescription = AppUtility.MenuItems.Reports.ToString(),
                    MenuViewName = "Reports",
                    ControllerName = "Expenses",
                    ActionName = "SummaryPieCharts",
                    MenuImageURL = "/images/css/main_menu_icons/expenses.png"
                },
                new Menu()
                {
                    menuID = 9,
                    MenuDescription = AppUtility.MenuItems.Income.ToString(),
                    MenuViewName = "Income",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/income.png"
                },
                new Menu()
                {
                    menuID = 10,
                    MenuDescription = AppUtility.MenuItems.Users.ToString(),
                    MenuViewName = "Users",
                    ControllerName = "Admin",
                    ActionName = "Index",
                    MenuImageURL = "/images/css/main_menu_icons/users.png"
                }

            };
        }
    }
}