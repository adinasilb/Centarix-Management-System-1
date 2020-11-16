using Abp.Extensions;
using Microsoft.AspNetCore.Routing;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public static class CreateMenus
    {
        public static List<MenuItems> CreateOrdersAndInventoryMainMenu(string MainMenu, string OrigClasses, string ActiveClasses)
        {
            var MainMenuItems = new List<MenuItems>();

            var AllClasses = OrigClasses;
            if (MainMenu == AppUtility.RequestPageTypeEnum.Request.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Requests",
                Controller = "Requests",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.RequestPageTypeEnum.Request }
                },
                Classes = AllClasses,
                ID = "request-link"
            });
            if (MainMenu == AppUtility.RequestPageTypeEnum.Summary.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Inventory",
                Controller = "Requests",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.RequestPageTypeEnum.Summary }
                },
                Classes = AllClasses,
                ID = "summary-link"
            });
            if (MainMenu == AppUtility.RequestPageTypeEnum.Search.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.OrdersAndInventory }
                },
                Classes = AllClasses,
                ID = "search-link"
            });
            if (MainMenu == AppUtility.RequestPageTypeEnum.Location.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Locations",
                Controller = "Locations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.OrdersAndInventory }
                },
                Classes = AllClasses,
                ID = "location-link"
            });
            if (MainMenu == AppUtility.RequestPageTypeEnum.Cart.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Personal",
                Controller = "Requests",
                Action = "Cart",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.RequestPageTypeEnum.Cart }
                },
                Classes = AllClasses,
                ID = "personal-link"
            });
            if (MainMenu == AppUtility.RequestPageTypeEnum.Inventory.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Full Inventory",
                Controller = "Requests",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.RequestPageTypeEnum.Inventory }
                },
                Classes = AllClasses,
                ID = "inventory-link"
            });

            return MainMenuItems;
        }
        public static List<MenuItems> CreateLabManagementMainMenu(string MainMenu, string OrigClasses, string ActiveClasses)
        {
            var MainMenuItems = new List<MenuItems>();

            var AllClasses = OrigClasses;
            if (MainMenu == AppUtility.LabManagementPageTypeEnum.Suppliers.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Suppliers",
                Controller = "Vendors",
                Action = "IndexForPayment",
                RouteValues = new RouteValueDictionary()
                    {
                        {"PageType", AppUtility.LabManagementPageTypeEnum.Suppliers },
                        {"SectionType", AppUtility.MenuItems.LabManagement }
                    },
                Classes = AllClasses
            });
            if (MainMenu == AppUtility.LabManagementPageTypeEnum.Locations.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Locations",
                Controller = "Locations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                    {
                        {"SectionType", AppUtility.MenuItems.LabManagement }
                    },
                Classes = AllClasses
            });
            if (MainMenu == AppUtility.LabManagementPageTypeEnum.Equipment.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Equipment",
                Controller = "Calibrations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {

                },
                Classes = AllClasses
            });
            if (MainMenu == AppUtility.LabManagementPageTypeEnum.Quotes.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Orders",
                Controller = "Requests",
                Action = "LabManageQuotes",
                RouteValues = new RouteValueDictionary(),
                Classes = AllClasses
            });
            if (MainMenu == AppUtility.LabManagementPageTypeEnum.SearchLM.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                Classes = AllClasses
            });

            return MainMenuItems;
        }
        public static List<MenuItems> CreateAccountingMainMenu(string MainMenu, string OrigClasses, string ActiveClasses)
        {
            var MainMenuItems = new List<MenuItems>();

            var MainClasses = OrigClasses;
            if (MainMenu == AppUtility.PaymentPageTypeEnum.Payments.ToString()) { MainClasses += ActiveClasses; } else { MainClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Payments",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.AccountingPaymentsEnum.MonthlyPayment }
                },
                Classes = MainClasses
            });
            if (MainMenu == AppUtility.PaymentPageTypeEnum.Notifications.ToString()) { MainClasses += ActiveClasses; } else { MainClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Notifications",
                Controller = "Requests",
                Action = "AccountingNotifications",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.PaymentPageTypeEnum.Notifications }
                },
                Classes = MainClasses
            });
            if (MainMenu == AppUtility.PaymentPageTypeEnum.General.ToString()) { MainClasses += ActiveClasses; } else { MainClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "General",
                Controller = "ParentRequests",
                Action = "GeneralPaymentList",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.PaymentPageTypeEnum.General }
                },
                Classes = MainClasses
            });
            if (MainMenu == AppUtility.PaymentPageTypeEnum.SuppliersAC.ToString()) { MainClasses += ActiveClasses; } else { MainClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Suppliers",
                Controller = "Vendors",
                Action = "IndexForPayment",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Accounting }
                },
                Classes = MainClasses
            });

            return MainMenuItems;
        }
        public static List<MenuItems> CreateOperationsMainMenu(string MainMenu, string OrigClasses, string ActiveClasses)
        {
            var MainMenuItems = new List<MenuItems>();

            var CurrentClasses = OrigClasses;
            if (MainMenu == AppUtility.OperationsPageTypeEnum.RequestOperations.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Requests",
                Controller = "Operations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.RequestPageTypeEnum.Request.ToString() }
                },
                Classes = CurrentClasses,
                ID = "request-link"
            });
            if (MainMenu == AppUtility.OperationsPageTypeEnum.InventoryOperations.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Inventory",
                Controller = "Operations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.RequestPageTypeEnum.Inventory.ToString() }
                },
                Classes = CurrentClasses,
                ID = "inventory-link"
            });
            if (MainMenu == AppUtility.OperationsPageTypeEnum.SearchOperations.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Operation.ToString() }
                },
                Classes = CurrentClasses,
                ID = "search-link"
            });

            return MainMenuItems;
        }
        public static List<MenuItems> CreateUsersMainMenu(string MainMenu, string OrigClasses, string ActiveClasses)
        {
            var MainMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (MainMenu == AppUtility.UserPageTypeEnum.User.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Users",
                Controller = "Admin",
                Action = "Index",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                ID = "inventory-link"
            });
            if (MainMenu == AppUtility.UserPageTypeEnum.Workers.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Workers",
                Controller = "ApplicationUsers",
                Action = "Details",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                ID = "inventory-link"
            });

            return MainMenuItems;
        }
        public static List<MenuItems> CreateTimekeeperMainMenu(string MainMenu, string OrigClasses, string ActiveClasses)
        {
            var MainMenuItems = new List<MenuItems>();

            var CurrentClasses = OrigClasses;
            if (MainMenu == AppUtility.TimeKeeperPageTypeEnum.Report.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Reports",
                Controller = "Timekeeper",
                Action = "ReportHours",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                ID = "reports-link"
            });
            if (MainMenu == AppUtility.TimeKeeperPageTypeEnum.TimekeeperSummary.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Summary",
                Controller = "Timekeeper",
                Action = "Documents",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                ID = "summary-link"
            });

            return MainMenuItems;
        }

        public static List<MenuItems> CreateExpensesMainMenu(string MainMenu, string OrigClasses, string ActiveClasses)
        {
            var MainMenuItems = new List<MenuItems>();

            var CurrentClasses = OrigClasses;
            if (MainMenu == AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Summary",
                Controller = "Expenses",
                Action = "SummaryPieCharts",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                ID = ""
            });
            if (MainMenu == AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Statistics",
                Controller = "Expenses",
                Action = "StatisticsProject",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                ID = ""
            });
            if (MainMenu == AppUtility.ExpensesPageTypeEnum.ExpensesCost.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            MainMenuItems.Add(new MenuItems()
            {
                Description = "Costs",
                Controller = "Expenses",
                Action = "CostsProject",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                ID = ""
            });

            return MainMenuItems;
        }

        public static List<MenuItems> GetOrdersAndInventoryRequestsSidebarMenuItems(string SidebarTitle, string OrigClasses, string ActiveClasses, string pageType, AppUtility.CategoryTypeEnum categoryType)
        {
            var SidebarMenuItems = new List<MenuItems>();

            var menuClass = OrigClasses;
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.LastItem.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Last Item",
                Controller = "Requests",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType }
                },
                Classes = menuClass,
                IconName = "icon-format_list_bulleted-24px-01"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.AddItem.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Add Item",
                Controller = "Requests",
                Action = "CreateModalView",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType }
                },
                Classes = menuClass,
                IconName = "icon-add_circle_outline-24px"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.Type.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Type",
                Controller = "ProductSubcategories",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType }
                },
                Classes = menuClass,
                IconName = "icon-category-24px1"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.Vendor.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Vendors",
                Controller = "Vendors",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType },
                    { "SectionType", AppUtility.MenuItems.OrdersAndInventory}
                },
                Classes = menuClass,
                IconName = "icon-storefront-24px"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.Owner.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Owner",
                Controller = "ApplicationUsers",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType }
                },
                Classes = menuClass,
                IconName = "icon-face-24px"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> GetOrdersAndInventorySummarySidebarMenuItems(string SidebarTitle, string OrigClasses, string ActiveClasses, string pageType, AppUtility.CategoryTypeEnum categoryType)
        {
            var SidebarMenuItems = new List<MenuItems>();

            var menuClass = OrigClasses;
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.LastItem.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Last Item",
                Controller = "Requests",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType",pageType},
                    { "CategoryType",categoryType}
                },
                Classes = menuClass,
                IconName = "icon-format_list_bulleted-24px-01"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.AddItem.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Add Item",
                Controller = "Requests",
                Action = "CreateModalView",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType }
                },
                Classes = menuClass,
                IconName = "icon-add_circle_outline-24px"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.Type.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Type",
                Controller = "ProductSubcategories",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType }
                },
                Classes = menuClass,
                IconName = "icon-category-24px1"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.Vendor.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Vendors",
                Controller = "Vendors",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType },
                    { "SectionType", AppUtility.MenuItems.OrdersAndInventory}
                },
                Classes = menuClass,
                IconName = "icon-storefront-24px"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.Owner.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Owner",
                Controller = "ApplicationUsers",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType }
                },
                Classes = menuClass,
                IconName = "icon-face-24px"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.Location.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Location",
                Controller = "Locations",
                Action = "IndexForInventory",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType }
                },
                Classes = menuClass,
                IconName = "icon-place-24px"
            });
            return SidebarMenuItems;
        }
        public static List<MenuItems> GetOrdersAndInventorySearchSidebarMenuItems(string OrigClasses, string ActiveClasses, string pageType, AppUtility.CategoryTypeEnum categoryType)
        {

            var SidebarMenuItems = new List<MenuItems>();

            var menuClass = OrigClasses;

            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Inventory",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    { "SectionType", AppUtility.MenuItems.OrdersAndInventory }
                },
                Classes = menuClass + ActiveClasses,
                IconName = "icon-zoom_in-24px-01"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> GetOrdersAndInventoryCartSidebarMenuItems(string SidebarTitle, string OrigClasses, string ActiveClasses, string pageType, AppUtility.CategoryTypeEnum categoryType)
        {

            var SidebarMenuItems = new List<MenuItems>();

            var menuClass = OrigClasses;
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.Cart.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Orders",
                Controller = "Requests",
                Action = "Cart",
                RouteValues = new RouteValueDictionary(),
                Classes = menuClass,
                IconName = "icon-shopping_cart-24px"
            });
            if (SidebarTitle == AppUtility.OrdersAndInventorySidebarEnum.Notifications.ToString()) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Notifications",
                Controller = "Requests",
                Action = "NotificationsView",
                RouteValues = new RouteValueDictionary(),
                Classes = menuClass,
                IconName = "icon-notifications-24px"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> GetOrdersAndInventoryLocationSidebarMenuItems(string OrigClasses, string ActiveClasses, string pageType, AppUtility.CategoryTypeEnum categoryType)
        {

            var SidebarMenuItems = new List<MenuItems>();

            var menuClass = OrigClasses;

            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "List",
                Controller = "Locations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.OrdersAndInventory }
                },
                Classes = menuClass + ActiveClasses,
                IconName = "icon-format_list_bulleted-24px-01"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateLabManageSuppliersSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();

            var Classes = OrigClasses;
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.AllSuppliers.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "All",
                Controller = "Vendors",
                Action = "IndexForPayment",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                IconName = "icon-storefront-24px1",
                Classes = Classes
            });
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.NewSupplier.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "New Supplier",
                Controller = "Vendors",
                Action = "Create",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                IconName = "icon-add_circle_outline-24px",
                Classes = Classes
            });
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.SearchSupplier.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Vendors",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                IconName = "icon-zoom_in-24px-01",
                Classes = Classes
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> CreateLabManageEquipmentSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();

            var Classes = OrigClasses;
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.Calibrate.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Calibrate",
                Controller = "Calibrations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                IconName = "icon-calibrate-24px",
                Classes = Classes
            });
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.EquipmentList.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "List",
                Controller = "Requests",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                IconName = "icon-format_list_bulleted-24px-01",
                Classes = Classes
            });
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.EquipmentCategories.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Categories",
                Controller = "ProductSubcategories",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {AppUtility.TempDataTypes.PageType.ToString(), AppUtility.LabManagementPageTypeEnum.Equipment}
                },
                IconName = "icon-category-24px1",
                Classes = Classes
            });
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.SearchEquipment.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "",
                Action = "",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                IconName = "icon-zoom_in-24px-01",
                Classes = Classes
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateLabManageLocationsSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();

            var Classes = OrigClasses;
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.LocationsList.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "List",
                Controller = "Locations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                Classes = Classes,
                IconName = "icon-zoom_in-24px-01"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateLabManageOrdersSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();

            var Classes = OrigClasses;
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.Quotes.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Quotes",
                Controller = "Requests",
                Action = "LabManageQuotes",
                RouteValues = new RouteValueDictionary(),
                Classes = Classes,
                IconName = "icon-book-24px"
            });
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.Orders.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Orders",
                Controller = "Requests",
                Action = "LabManageOrders",
                RouteValues = new RouteValueDictionary(),
                Classes = Classes,
                IconName = "icon-shopping_cart-24px"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateLabManageSearchSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();

            var Classes = OrigClasses;
            if (SidebarTitle == AppUtility.LabManagementSidebarEnum.SearchRequests.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                Classes = Classes,
                IconName = "icon-zoom_in-24px-01"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateAccountingPaymentsSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();

            var Classes = OrigClasses;
            if (SidebarTitle == AppUtility.AccountingPaymentsEnum.MonthlyPayment.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 1,
                Description = "Monthly Payment",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.AccountingPaymentsEnum.MonthlyPayment }
                },
                Classes = Classes,
                IconName = "icon-monetization_on-24px"
            });
            if (SidebarTitle == AppUtility.AccountingPaymentsEnum.PayNow.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 2,
                Description = "Pay Now",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.AccountingPaymentsEnum.PayNow }
                },
                Classes = Classes,
                IconName = "icon-payment-24px"
            });
            if (SidebarTitle == AppUtility.AccountingPaymentsEnum.PayLater.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 3,
                Description = "Pay Later",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.AccountingPaymentsEnum.PayLater }
                },
                Classes = Classes,
                IconName = "icon-centarix-icons-19"
            });
            if (SidebarTitle == AppUtility.AccountingPaymentsEnum.Installments.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 4,
                Description = "Installments",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.AccountingPaymentsEnum.Installments }
                },
                Classes = Classes,
                IconName = "icon-centarix-icons-20"
            });
            if (SidebarTitle == AppUtility.AccountingPaymentsEnum.StandingOrders.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 5,
                Description = "Standing Orders",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.AccountingPaymentsEnum.StandingOrders }
                },
                Classes = Classes,
                IconName = "icon-standing_orders-24px"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateAccountingNotificationsSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();

            var Classes = OrigClasses;
            if (SidebarTitle == AppUtility.AccountingNotificationsEnum.NoInvoice.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "No Invoice",
                Controller = "Requests",
                Action = "AccountingNotifications",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingNotificationsEnum", AppUtility.AccountingNotificationsEnum.NoInvoice }
                },
                Classes = Classes,
                IconName = "icon-cancel_presentation-24px"
            });
            if (SidebarTitle == AppUtility.AccountingNotificationsEnum.DidntArrive.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Didn't Arrive",
                Controller = "Requests",
                Action = "AccountingNotifications",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingNotificationsEnum", AppUtility.AccountingNotificationsEnum.DidntArrive }
                },
                Classes = Classes,
                IconName = "icon-local_shipping-24px"
            });
            if (SidebarTitle == AppUtility.AccountingNotificationsEnum.PartialDelivery.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Partial Delivery",
                Controller = "Requests",
                Action = "AccountingNotifications",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingNotificationsEnum", AppUtility.AccountingNotificationsEnum.PartialDelivery }
                },
                Classes = Classes,
                IconName = "icon-remove_shopping_cart-24px"
            });
            if (SidebarTitle == AppUtility.AccountingNotificationsEnum.ForClarification.ToString()) { Classes += ActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "For Clarification",
                Controller = "Requests",
                Action = "AccountingNotifications",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingNotificationsEnum", AppUtility.AccountingNotificationsEnum.ForClarification }
                },
                Classes = Classes,
                IconName = "icon-notification_didnt_arrive-24px"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> CreateAccountingSuppliersSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.AccountingSidebarEnum.AllSuppliersAC.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "All",
                Controller = "Vendors",
                Action = "IndexForPayment",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Accounting }
                },
                Classes = CurrentClasses,
                IconName = "icon-storefront-24px1"
            });
            if (SidebarTitle == AppUtility.AccountingSidebarEnum.NewSupplierAC.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "New Supplier",
                Controller = "Vendors",
                Action = "Create",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Accounting }
                },
                Classes = CurrentClasses,
                IconName = "icon-add_circle_outline-24px"
            });
            if (SidebarTitle == AppUtility.AccountingSidebarEnum.SearchSupplierAC.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Vendors",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Accounting }
                },
                Classes = CurrentClasses,
                IconName = "icon-zoom_in-24px-01"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateOperationsRequestsSidebar(string PageType, string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.OperationsSidebarEnum.LastItem.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            var typePageType = "Request";
            if (PageType == AppUtility.OperationsPageTypeEnum.RequestOperations.ToString())
            {
                typePageType = AppUtility.RequestPageTypeEnum.Request.ToString();
            }
            if (PageType == AppUtility.OperationsPageTypeEnum.InventoryOperations.ToString())
            {
                typePageType = AppUtility.RequestPageTypeEnum.Inventory.ToString();
            }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Last Item",
                Controller = "Operations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", PageType }
                },
                Classes = CurrentClasses,
                IconName = "icon-format_list_bulleted-24px-01"
            });
            if (SidebarTitle == AppUtility.OperationsSidebarEnum.AddItem.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Add Item",
                Controller = "Operations",
                Action = "CreateModalView",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", PageType },
                    {"categoryType", 2 }
                },
                Classes = CurrentClasses,
                IconName = "icon-add_circle_outline-24px"
            });
            if (SidebarTitle == AppUtility.OperationsSidebarEnum.Type.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Type",
                Controller = "ProductSubcategories",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", typePageType },
                    {"categoryType", 2 }
                },
                Classes = CurrentClasses,
                IconName = "icon-category-24px1"
            });
            if (SidebarTitle == AppUtility.OperationsSidebarEnum.Vendors.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Vendor",
                Controller = "Vendors",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", typePageType },
                    {"categoryType", 2 },
                           { "SectionType", AppUtility.MenuItems.Operation}
                },
                Classes = CurrentClasses,
                IconName = "icon-storefront-24px"
            });
            if (SidebarTitle == AppUtility.OperationsSidebarEnum.Owner.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Owner",
                Controller = "ApplicationUsers",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", typePageType },
                    {"categoryType", AppUtility.CategoryTypeEnum.Operations }
                },
                Classes = CurrentClasses,
                IconName = "icon-face-24px"
            });

            return SidebarMenuItems;
        }


        public static List<MenuItems> CreateOperationsSearchSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.OperationsSidebarEnum.Search.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Operation.ToString() }
                },
                Classes = CurrentClasses,
                IconName = "icon-zoom_in-24px-01"
            });

            return SidebarMenuItems;
        }


        public static List<MenuItems> CreateUsersUsersSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.UserSideBarEnum.UsersList.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "List",
                Controller = "Admin",
                Action = "Index",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-format_list_bulleted-24px-01"
            });
            if (SidebarTitle == AppUtility.UserSideBarEnum.UsersAdd.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Add",
                Controller = "Admin",
                Action = "CreateUser",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-add_circle_outline-24px"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> CreateUsersWorkersSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.UserSideBarEnum.WorkersDetails.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Details",
                Controller = "ApplicationUsers",
                Action = "Details",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-assignment_ind-24px-1"
            });
            if (SidebarTitle == AppUtility.UserSideBarEnum.WorkersHours.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Hours",
                Controller = "ApplicationUsers",
                Action = "Hours",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });
            if (SidebarTitle == AppUtility.UserSideBarEnum.WorkersSalary.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Salary",
                Controller = "ApplicationUsers",
                Action = "Salary",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-monetization_on-24px"
            });
            if (SidebarTitle == AppUtility.UserSideBarEnum.WorkersAwaitingApproval.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Awaiting Approval",
                Controller = "ApplicationUsers",
                Action = "AwaitingApproval",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-pending-24px-1"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateTimekeeperReportsSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.TimeKeeperSidebarEnum.ReportHours.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Report Hours",
                Controller = "Timekeeper",
                Action = "ReportHours",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-assignment-24px"
            });
            if (SidebarTitle == AppUtility.TimeKeeperSidebarEnum.SummaryHours.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Summary Hours",
                Controller = "Timekeeper",
                Action = "SummaryHours",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });
            if (SidebarTitle == AppUtility.TimeKeeperSidebarEnum.ReportDaysOff.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Report Days Off",
                Controller = "Timekeeper",
                Action = "ReportDaysOff",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-event-24px"
            });
            if (SidebarTitle == AppUtility.TimeKeeperSidebarEnum.SummaryDaysOff.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Summary Days Off",
                Controller = "Timekeeper",
                Action = "SummaryDaysOff",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-highlight_off-24px"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> CreateTimekeeperSummarySidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.TimeKeeperSidebarEnum.Documents.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Documents",
                Controller = "Timekeeper",
                Action = "Documents",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-description-24px-2"
            });
            if (SidebarTitle == AppUtility.TimeKeeperSidebarEnum.CompanyAbsences.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Company Absences",
                Controller = "Timekeeper",
                Action = "CompanyAbsences",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-event-24px"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateExpensesSummarySidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.SummaryPieCharts.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Pie Charts",
                Controller = "Expenses",
                Action = "SummaryPieCharts",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-pie_chart-24px-2"
            });
            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.SummaryTables.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Tables",
                Controller = "Expenses",
                Action = "SummaryTables",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-table_chart-24px-1"
            });
            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.SummaryGraphs.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Graphs",
                Controller = "Expenses",
                Action = "SummaryGraphs",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-insert_chart-24px"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateExpensesStatisticsSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.StatisticsProject.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Project",
                Controller = "Expenses",
                Action = "StatisticsProject",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-folder_open-24px"
            });
            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.StatisticsItem.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Item",
                Controller = "Expenses",
                Action = "StatisticsItem",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-centarix-icons-05"
            });
            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.StatisticsWorker.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Worker",
                Controller = "Expenses",
                Action = "StatisticsWorker",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-face-24px"
            });
            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.StatisticsCategory.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Category",
                Controller = "Expenses",
                Action = "StatisticsCategory",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-storefront-24px"
            });
            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.StatisticsVendor.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Vendor",
                Controller = "Expenses",
                Action = "StatisticsVendor",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-storefront-24px"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateExpensesCostsSidebar(string SidebarTitle, string OrigClasses, string ActiveClasses)
        {
            var SidebarMenuItems = new List<MenuItems>();
            var CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.CostsProject.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Project",
                Controller = "Expenses",
                Action = "CostsProject",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-zoom_in-24px-01"
            });
            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.CostsAdvancedSearch.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Advanced Search",
                Controller = "Expenses",
                Action = "CostsAdvancedSearch",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-zoom_in-24px"
            });
            if (SidebarTitle == AppUtility.ExpensesSidebarEnum.CostsAdvancedLists.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Advanced List",
                Controller = "Expenses",
                Action = "CostsAdvancedList",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-search-list-24px"
            });

            return SidebarMenuItems;
        }

    }
}
