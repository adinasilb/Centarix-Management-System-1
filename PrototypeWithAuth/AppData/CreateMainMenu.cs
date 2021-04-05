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
                    MenuImageURL = "/images/css/main_menu_icons/inventory.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/inventory_menu_button.png"
                },
                new Menu()
                {
                    menuID = 2,
                    MenuDescription = AppUtility.MenuItems.Protocols.ToString(),
                    MenuViewName = "Protocols",
                    ControllerName = "Protocols",
                    ActionName = "CurrentProtocols",
                    MenuImageURL = "/images/css/main_menu_icons/protocols.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/protocols_menu_button.png"
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
                        { "SidebarType", AppUtility.SidebarEnum.List },
                        { "RequestStatusID", 2 },
                    },
                    MenuImageURL = "/images/css/main_menu_icons/operation.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/operations_menu_button.png"
                },
                new Menu()
                {
                    menuID = 4,
                    MenuDescription = AppUtility.MenuItems.Biomarkers.ToString(),
                    MenuViewName = "Biomarkers",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/biomarkers.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/biomarkers_menu_button.png"
                },
                new Menu()
                {
                    menuID = 5,
                    MenuDescription = AppUtility.MenuItems.TimeKeeper.ToString(),
                    MenuViewName = "Timekeeper",
                    ControllerName = "Timekeeper",
                    ActionName = "ReportHours",
                    MenuImageURL = "/images/css/main_menu_icons/timekeeper.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/timekeeper_menu_button.png"
                },
                new Menu()
                {
                    menuID = 6,
                    MenuDescription = AppUtility.MenuItems.LabManagement.ToString(),
                    MenuViewName = "Lab Management",
                    ControllerName = "Vendors",
                    ActionName = "IndexForPayment",
                    MenuImageURL = "/images/css/main_menu_icons/lab.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/lab_managment_menu_button.png"
                },
                new Menu()
                {
                    menuID = 7,
                    MenuDescription = AppUtility.MenuItems.Accounting.ToString(),
                    MenuViewName = "Accounting",
                    ControllerName = "Requests",
                    ActionName = "AccountingPayments",
                    MenuImageURL = "/images/css/main_menu_icons/accounting.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/accounting_menu_button.png"
                },
                new Menu()
                {
                    menuID = 8,
                    MenuDescription = AppUtility.MenuItems.Reports.ToString(),
                    MenuViewName = "Reports",
                    ControllerName = "Expenses",
                    ActionName = "SummaryPieCharts",
                    MenuImageURL = "/images/css/main_menu_icons/expenses.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/reports_menu_button.png"
                },
                new Menu()
                {
                    menuID = 9,
                    MenuDescription = AppUtility.MenuItems.Income.ToString(),
                    MenuViewName = "Income",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/income.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/income_menu_button.png"
                },
                new Menu()
                {
                    menuID = 10,
                    MenuDescription = AppUtility.MenuItems.Users.ToString(),
                    MenuViewName = "Users",
                    ControllerName = "Admin",
                    ActionName = "Index",
                    MenuImageURL = "/images/css/main_menu_icons/users.png",
                    SmallMenuImageURL = "/images/css/main_menu_small_icons/users_menu_button.png"
                }

            };
        }
    }
}