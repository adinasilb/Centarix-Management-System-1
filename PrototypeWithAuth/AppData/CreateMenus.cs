using Abp.Extensions;
using Microsoft.AspNetCore.Routing;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public static class CreateMenus
    {
        private static string SideBarActiveClasses = " text-dark section-filter ";
        private static string OrigClasses = " opac87 align-text-bottom  ";

        public static List<MenuItems> CreateMainMenu(AppUtility.MenuItems SectionType, AppUtility.PageTypeEnum pageType)
        {
            List<MenuItems> MainMenuItems = new List<MenuItems>();
            string ActiveClasses = " activeNavLink ";
  
            string AllClasses = OrigClasses;
            switch (SectionType)
            {

                case AppUtility.MenuItems.Requests:                  
                    if (pageType == AppUtility.PageTypeEnum.RequestRequest) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Requests",
                        Controller = "Requests",
                        Action = "Index",
                        RouteValues = new RouteValueDictionary()
                        {
                            {"PageType",  AppUtility.PageTypeEnum.RequestRequest },
                            { "SectionType", AppUtility.MenuItems.Requests },
                            { "SidebarType", AppUtility.SidebarEnum.List }
                        },
                        Classes = AllClasses,
                        ID = "request-link"
                    });
                    if (pageType == AppUtility.PageTypeEnum.RequestSummary) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Inventory",
                        Controller = "Requests",
                        Action = "IndexInventory",
                        RouteValues = new RouteValueDictionary()
                        {
                            {"PageType",  AppUtility.PageTypeEnum.RequestSummary },
                            { "SectionType", AppUtility.MenuItems.Requests },
                            { "SidebarType", AppUtility.SidebarEnum.List },
                               { "RequestStatusID", 3 },
                        },
                        Classes = AllClasses,
                        ID = "summary-link"
                    });
                    //    if (pageType == AppUtility.PageTypeEnum.RequestSearch) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    //    MainMenuItems.Add(new MenuItems()
                    //    {
                    //        Description = "Search",
                    //        Controller = "Requests",
                    //        Action = "Search",
                    //        RouteValues = new RouteValueDictionary()
                    //{
                    //    {"SectionType", AppUtility.MenuItems.Requests }
                    //},
                    //        Classes = AllClasses,
                    //        ID = "search-link"
                    //    });
                    if (pageType == AppUtility.PageTypeEnum.RequestLocation) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Locations",
                        Controller = "Locations",
                        Action = "Index",
                        RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Requests }
                },
                        Classes = AllClasses,
                        ID = "location-link"
                    });
                    if (pageType == AppUtility.PageTypeEnum.RequestCart) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Personal",
                        Controller = "Requests",
                        Action = "Cart",
                        RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.PageTypeEnum.RequestCart }
                },
                        Classes = AllClasses,
                        ID = "personal-link"
                    });
                    //if (MainMenu == AppUtility.RequestPageTypeEnum.Inventory.ToString()) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    //MainMenuItems.Add(new MenuItems()
                    //{
                    //    Description = "Full Inventory",
                    //    Controller = "Requests",
                    //    Action = "Index",
                    //    RouteValues = new RouteValueDictionary()
                    //    {
                    //                {"PageType",  AppUtility.PageTypeEnum.RequestRequest },
                    //        { "SectionType", AppUtility.MenuItems.Requests },
                    //                { "SidebarType", AppUtility.SidebarEnum.List }
                    //},
                    //        //    Classes = AllClasses,
                    //    ID = "inventory-link"
                    //});

                    break;
                case AppUtility.MenuItems.Operations:
                    if (pageType == AppUtility.PageTypeEnum.OperationsRequest) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Requests",
                        Controller = "Requests", //redo to operations
                        Action = "Index",
                        RouteValues = new RouteValueDictionary()
                        {
                            {"PageType",  AppUtility.PageTypeEnum.OperationsRequest },
                            { "SectionType", AppUtility.MenuItems.Operations },
                            { "SidebarType", AppUtility.SidebarEnum.List },
                            { "RequestStatusID", 2 }
                        },
                        Classes = AllClasses,
                        ID = "request-link"
                    });
                    if (pageType == AppUtility.PageTypeEnum.OperationsInventory) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Inventory",
                        Controller = "Requests", //redo to operations
                        Action = "IndexInventory",
                        RouteValues = new RouteValueDictionary()
                        {
                            {"PageType",  AppUtility.PageTypeEnum.OperationsInventory },
                            { "SectionType", AppUtility.MenuItems.Operations },
                            { "SidebarType", AppUtility.SidebarEnum.List },
                            { "RequestStatusID", 3 },
                        },
                        Classes = AllClasses,
                        ID = "inventory-link"
                    });
                    //    if (pageType == AppUtility.PageTypeEnum.OperationsSearch) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    //    MainMenuItems.Add(new MenuItems()
                    //    {
                    //        Description = "Search",
                    //        Controller = "Requests",
                    //        Action = "Search",
                    //        RouteValues = new RouteValueDictionary()
                    //{
                    //    {"SectionType", AppUtility.MenuItems.Operations }
                    //},
                    //        Classes = AllClasses,
                    //        ID = "search-link"
                    //    });
                    break;
                case AppUtility.MenuItems.Accounting:
                    if (pageType == AppUtility.PageTypeEnum.AccountingPayments) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Payments",
                        Controller = "Requests",
                        Action = "AccountingPayments",
                        RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.SidebarEnum.MonthlyPayment }
                },
                        Classes = AllClasses
                    });
                    if (pageType == AppUtility.PageTypeEnum.AccountingNotifications) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Notifications",
                        Controller = "Requests",
                        Action = "AccountingNotifications",
                        RouteValues = new RouteValueDictionary()
                {
                    {"PageType", AppUtility.PageTypeEnum.AccountingNotifications }
                },
                        Classes = AllClasses
                    });
                    if (pageType == AppUtility.PageTypeEnum.AccountingGeneral) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "General",
                        Controller = "Requests",
                        Action = "AccountingGeneral",
                        RouteValues = new RouteValueDictionary()
                        {
                            {"PageType",  AppUtility.PageTypeEnum.AccountingGeneral },
                            { "SectionType", AppUtility.MenuItems.Accounting },
                            { "SidebarType", AppUtility.SidebarEnum.List }
                        },
                        Classes = AllClasses
                    });
                    if (pageType == AppUtility.PageTypeEnum.AccountingSuppliers) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Suppliers",
                        Controller = "Vendors",
                        Action = "IndexForPayment",
                        RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Accounting }
                },
                        Classes = AllClasses
                    });
                    break;
                case AppUtility.MenuItems.Income:
                    break;
                case AppUtility.MenuItems.Biomarkers:                    
                    if (pageType == AppUtility.PageTypeEnum.BiomarkersExperiments) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Experiments",
                        Controller = "Biomarkers",
                        Action = "HumanTrialsList",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses
                    });
                    break;
                case AppUtility.MenuItems.LabManagement:
                    if (pageType == AppUtility.PageTypeEnum.LabManagementSuppliers) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Suppliers",
                        Controller = "Vendors",
                        Action = "IndexForPayment",
                        RouteValues = new RouteValueDictionary()
                    {
                        {"PageType", AppUtility.PageTypeEnum.LabManagementSuppliers },
                        {"SectionType", AppUtility.MenuItems.LabManagement }
                    },
                        Classes = AllClasses
                    });
                    if (pageType == AppUtility.PageTypeEnum.LabManagementLocations) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
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
                    //if (pageType == AppUtility.PageTypeEnum.LabManagementEquipment) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    //MainMenuItems.Add(new MenuItems()
                    //{
                    //    Description = "Equipment",
                    //    Controller = "Calibrations",
                    //    Action = "Index",
                    //    RouteValues = new RouteValueDictionary()
                    //    {

                    //    },
                    //    Classes = AllClasses
                    //});
                    if (pageType == AppUtility.PageTypeEnum.LabManagementQuotes) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Orders",
                        Controller = "Requests",
                        Action = "LabManageQuotes",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses
                    });
                    if (pageType == AppUtility.PageTypeEnum.LabManagementSettings) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Settings",
                        Controller = "Requests",
                        Action = "SettingsInventory",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses
                    });
                    //    if (pageType == AppUtility.PageTypeEnum.LabManagementSearch) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    //    MainMenuItems.Add(new MenuItems()
                    //    {
                    //        Description = "Search",
                    //        Controller = "Requests",
                    //        Action = "Search",
                    //        RouteValues = new RouteValueDictionary()
                    //{
                    //    {"SectionType", AppUtility.MenuItems.LabManagement }
                    //},
                    //        Classes = AllClasses
                    //    });
                    break;
                case AppUtility.MenuItems.Protocols:
                    if (pageType == AppUtility.PageTypeEnum.ProtocolsWorkflow) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Workflow",
                        Controller = "Protocols",
                        Action = "CurrentProtocols",
                        RouteValues = new RouteValueDictionary()
                        {
                        {"PageType", AppUtility.PageTypeEnum.ProtocolsWorkflow },
                        {"SidebarType", AppUtility.SidebarEnum.CurrentProtocols }
                        },
                        Classes = AllClasses,
                        ID = ""
                    });
                    if (pageType == AppUtility.PageTypeEnum.ProtocolsProtocols) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Protocols",
                        Controller = "Protocols",
                        Action = "Index",
                        RouteValues = new RouteValueDictionary()
                        {
                        {"PageType", AppUtility.PageTypeEnum.ProtocolsProtocols },
                        {"SidebarType", AppUtility.SidebarEnum.List }
                        },
                        Classes = AllClasses,
                        ID = ""
                    });
                    if (pageType == AppUtility.PageTypeEnum.ProtocolsCreate) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Create",
                        Controller = "Protocols",
                        Action = "ResearchProtocol",
                        RouteValues = new RouteValueDictionary()
                        {
                        {"PageType", AppUtility.PageTypeEnum.ProtocolsCreate},
                        {"SidebarType", AppUtility.SidebarEnum.ResearchProtocol }
                        },
                        Classes = AllClasses,
                        ID = ""
                    });
                    if (pageType == AppUtility.PageTypeEnum.ProtocolsReports) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Report",
                        Controller = "Protocols",
                        Action = "ReportsCategories",
                        RouteValues = new RouteValueDictionary()
                        {
                        {"PageType", AppUtility.PageTypeEnum.ProtocolsReports },
                        {"SidebarType", AppUtility.SidebarEnum.WeeklyReports }
                        },
                        Classes = AllClasses,
                        ID = ""
                    });
                    if (pageType == AppUtility.PageTypeEnum.ProtocolsResources) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Resources",
                        Controller = "Protocols",
                        Action = "Library",
                        RouteValues = new RouteValueDictionary()
                        {
                        {"PageType", AppUtility.PageTypeEnum.ProtocolsResources },
                        {"SidebarType", AppUtility.SidebarEnum.Library }
                        },
                        Classes = AllClasses,
                        ID = ""
                    });
                    break;
                case AppUtility.MenuItems.Reports:
                    if (pageType == AppUtility.PageTypeEnum.ExpensesSummary) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Summary",
                        Controller = "Expenses",
                        Action = "SummaryPieCharts",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses,
                        ID = ""
                    });
                    if (pageType == AppUtility.PageTypeEnum.ExpensesStatistics) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Statistics",
                        Controller = "Expenses",
                        Action = "StatisticsProject",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses,
                        ID = ""
                    });
                    if (pageType == AppUtility.PageTypeEnum.ExpensesCost) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Costs",
                        Controller = "Expenses",
                        Action = "CostsProject",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses,
                        ID = ""
                    });
                    //if (MainMenu == AppUtility.ExpensesPageTypeEnum.ExpensesWorkers.ToString()) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
                    //MainMenuItems.Add(new MenuItems()
                    //{
                    //    Description = "Workers",
                    //    Controller = "Expenses",
                    //    Action = "WorkersDetails",
                    //    RouteValues = new RouteValueDictionary(),
                    //    Classes = CurrentClasses,
                    //    ID = ""
                    //});

                    break;
                case AppUtility.MenuItems.TimeKeeper:
                    if (pageType == AppUtility.PageTypeEnum.TimeKeeperReport) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Reports",
                        Controller = "Timekeeper",
                        Action = "ReportHours",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses,
                        ID = "reports-link"
                    });
                    if (pageType == AppUtility.PageTypeEnum.TimekeeperSummary) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Summary",
                        Controller = "Timekeeper",
                        Action = "SummaryHours",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses,
                        ID = "summary-link"
                    });
                    break;
                case AppUtility.MenuItems.Users:
                    if (pageType == AppUtility.PageTypeEnum.UsersUser) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Users",
                        Controller = "Admin",
                        Action = "Index",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses,
                        ID = "inventory-link"
                    });
                    if (pageType == AppUtility.PageTypeEnum.UsersWorkers) { AllClasses += ActiveClasses; } else { AllClasses = OrigClasses; }
                    MainMenuItems.Add(new MenuItems()
                    {
                        Description = "Employees",
                        Controller = "ApplicationUsers",
                        Action = "Details",
                        RouteValues = new RouteValueDictionary(),
                        Classes = AllClasses,
                        ID = "inventory-link"
                    });

                    break;

            }
            return MainMenuItems;
        }


        public static List<MenuItems> GetOrdersAndInventoryRequestsSidebarMenuItems(AppUtility.SidebarEnum SidebarTitle, AppUtility.PageTypeEnum pageType, AppUtility.CategoryTypeEnum categoryType)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string menuClass = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.List) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Last Item",
                Controller = "Requests",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                        {
                            {"PageType", pageType },
                            { "SectionType", AppUtility.MenuItems.Requests },
                            { "SidebarType", AppUtility.SidebarEnum.List }
                        },
                Classes = menuClass,
                IconName = "icon-format_list_bulleted-24px-01"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Add) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Add Item",
                Controller = "Requests",
                Action = "AddItemView",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType }
                },
                Classes = menuClass,
                IconName = "icon-add_circle_outline-24px-1",
                AjaxLink = "add-item-sidebar"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Type) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
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
                IconName = "icon-category-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Vendors) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Vendors",
                Controller = "Vendors",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType },
                    { "SectionType", AppUtility.MenuItems.Requests}
                },
                Classes = menuClass,
                IconName = "icon-storefront-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Owner) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
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
            if (SidebarTitle == AppUtility.SidebarEnum.Search) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType",  pageType },
                    { "SectionType", AppUtility.MenuItems.Requests },
                    { "SidebarType", AppUtility.SidebarEnum.Search }
                },
                Classes = menuClass,
                IconName = "icon-zoom_in-24px-01"
            });
            return SidebarMenuItems;
        }
        public static List<MenuItems> GetOrdersAndInventorySummarySidebarMenuItems(AppUtility.SidebarEnum SidebarTitle, AppUtility.PageTypeEnum pageType, AppUtility.CategoryTypeEnum categoryType)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string menuClass = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.List) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Last Item",
                Controller = "Requests",
                Action = "IndexInventory",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType",  pageType },
                    { "SectionType", AppUtility.MenuItems.Requests },
                    { "SidebarType", AppUtility.SidebarEnum.List },
                    { "RequestStatusID", 3 },
                },
                Classes = menuClass,
                IconName = "icon-format_list_bulleted-24px-01"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Add) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Add Item",
                Controller = "Requests",
                Action = "AddItemView",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", AppUtility.PageTypeEnum.RequestSummary}
                },
                Classes = menuClass,
                IconName = "icon-add_circle_outline-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Type) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
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
                IconName = "icon-category-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Vendors) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Vendors",
                Controller = "Vendors",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType },
                    { "SectionType", AppUtility.MenuItems.Requests}
                },
                Classes = menuClass,
                IconName = "icon-storefront-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Owner) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
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
            //if (SidebarTitle == AppUtility.SidebarEnum.Location) { menuClass += ActiveClasses; } else { menuClass = OrigClasses; }
            //SidebarMenuItems.Add(new MenuItems()
            //{
            //    Description = "Location",
            //    Controller = "Locations",
            //    Action = "IndexForInventory",
            //    RouteValues = new RouteValueDictionary()
            //    {
            //        { "PageType", pageType },
            //        { "CategoryType", categoryType }
            //    },
            //    Classes = menuClass,
            //    IconName = "icon-place-24px"
            //});
            if (SidebarTitle == AppUtility.SidebarEnum.Search) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType",  pageType },
                    { "SectionType", AppUtility.MenuItems.Requests },
                    { "SidebarType", AppUtility.SidebarEnum.Search }
                },
                Classes = menuClass,
                IconName = "icon-zoom_in-24px-01"
            });
            return SidebarMenuItems;
        }
        public static List<MenuItems> GetOrdersAndInventorySearchSidebarMenuItems( AppUtility.PageTypeEnum pageType, AppUtility.CategoryTypeEnum categoryType)
        {

            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string menuClass = OrigClasses;

            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Inventory",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    { "SectionType", AppUtility.MenuItems.Requests }
                },
                Classes = menuClass + SideBarActiveClasses,
                IconName = "icon-zoom_in-24px-01"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> GetOrdersAndInventoryCartSidebarMenuItems(AppUtility.SidebarEnum SidebarTitle, AppUtility.PageTypeEnum pageType, AppUtility.CategoryTypeEnum categoryType)
        {

            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string menuClass = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.Cart) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Orders",
                Controller = "Requests",
                Action = "Cart",
                RouteValues = new RouteValueDictionary(),
                Classes = menuClass,
                IconName = "icon-shopping_cart-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Favorites) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Favorite",
                Controller = "Requests",
                Action = "IndexFavorites",
                Classes = menuClass,
                IconName = "icon-favorite_border-24px"
                //Description = "Favorites",
                //Controller = "Requests",
                //Action = "Favorites",
                //RouteValues = new RouteValueDictionary(),
                //Classes = menuClass,
                //IconName = "icon-favorite_border-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.SharedRequests) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Shared",
                Controller = "Requests",
                Action = "IndexShared",
                Classes = menuClass,
                IconName = "icon-notification_shared-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Notifications) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Notifications",
                Controller = "Requests",
                Action = "NotificationsView",
                RouteValues = new RouteValueDictionary(),
                Classes = menuClass,
                IconName = "icon-notifications-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.DidntArrive) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Didn't Arrive",
                Controller = "Requests",
                Action = "NotificationsView",
                RouteValues = new RouteValueDictionary()
                {
                    {"DidntArrive", true }
                },
                Classes = menuClass,
                IconName = "icon-local_shipping-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.MyLists) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "My Lists",
                Controller = "Requests",
                Action = "IndexLists",
                Classes = menuClass,
                IconName = "icon-library_books-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.SharedLists) { menuClass += SideBarActiveClasses; } else { menuClass = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Shared Lists",
                Controller = "Requests",
                Action = "IndexSharedLists",
                Classes = menuClass,
                IconName = "icon-centarix-icons-04"
            });
            return SidebarMenuItems;
        }
        public static List<MenuItems> GetOrdersAndInventoryLocationSidebarMenuItems(AppUtility.CategoryTypeEnum categoryType)
        {

            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string menuClass = OrigClasses;

            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "List",
                Controller = "Locations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Requests }
                },
                Classes = menuClass + SideBarActiveClasses,
                IconName = "icon-format_list_bulleted-24px-01"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateLabManageSuppliersSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string Classes = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.AllSuppliers) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "All",
                Controller = "Vendors",
                Action = "IndexForPayment",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                IconName = "icon-storefront-24px-1",
                Classes = Classes
            });
            if (SidebarTitle == AppUtility.SidebarEnum.NewSupplier) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "New Supplier",
                Controller = "Vendors",
                Action = "Create",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement }
                },
                IconName = "icon-add_circle_outline-24px-1",
                Classes = Classes
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Search) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
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
        public static List<MenuItems> CreateLabManageEquipmentSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string Classes = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.Calibrate) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
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
            if (SidebarTitle == AppUtility.SidebarEnum.List) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "List",
                Controller = "Requests",
                Action = "ItemTableEquipment",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.LabManagement },
                    { "PageType", AppUtility.SidebarEnum.List }
                },
                IconName = "icon-format_list_bulleted-24px-01",
                Classes = Classes
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Type) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Categories",
                Controller = "ProductSubcategories",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {AppUtility.TempDataTypes.PageType.ToString(), AppUtility.PageTypeEnum.LabManagementEquipment}
                },
                IconName = "icon-category-24px-1",
                Classes = Classes
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Search) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
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

        public static List<MenuItems> CreateLabManageSettingsSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string Classes = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.Inventory) { Classes += SideBarActiveClasses; } else { Classes += OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Inventory",
                Controller = "Requests",
                Action = "SettingsInventory",
                Classes = Classes,
                IconName = "icon-centarix-icons-05"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateLabManageLocationsSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string Classes = OrigClasses;
            
            if (SidebarTitle == AppUtility.SidebarEnum.List) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
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
            if (SidebarTitle == AppUtility.SidebarEnum.Add) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Add Location",
                AjaxLink = "load-add-location",
                Classes = Classes,
                IconName = "icon-add_circle_outline-24px-1"
            });
            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateLabManageOrdersSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string Classes = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.Quotes) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Quotes",
                Controller = "Requests",
                Action = "LabManageQuotes",
                RouteValues = new RouteValueDictionary(),
                Classes = Classes,
                IconName = "icon-book-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Orders) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
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

        public static List<MenuItems> CreateLabManageSearchSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string Classes = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.Search) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
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

        public static List<MenuItems> CreateAccountingPaymentsSidebar(AppUtility.SidebarEnum SidebarTitle, string payNowCount)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string Classes = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.MonthlyPayment) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 1,
                Description = "Monthly Payment",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.SidebarEnum.MonthlyPayment }
                },
                Classes = Classes,
                IconName = "icon-monetization_on-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.PayNow) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 2,
                Description = "Pay Now\n[" + payNowCount + "]",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.SidebarEnum.PayNow }
                },
                Classes = Classes,
                IconName = "icon-payment-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.PayLater) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 3,
                Description = "Pay Later",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.SidebarEnum.PayLater }
                },
                Classes = Classes,
                IconName = "icon-centarix-icons-19"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Installments) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 4,
                Description = "Installments",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.SidebarEnum.Installments }
                },
                Classes = Classes,
                IconName = "icon-centarix-icons-20"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.StandingOrders) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 5,
                Description = "Standing Orders",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.SidebarEnum.StandingOrders }
                },
                Classes = Classes,
                IconName = "icon-standing_orders-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.SpecifyPayment) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                MenuItemsID = 5,
                Description = "Specify Payment",
                Controller = "Requests",
                Action = "AccountingPayments",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingPaymentsEnum", AppUtility.SidebarEnum.SpecifyPayment }
                },
                Classes = Classes,
                IconName = "icon-standing_orders-24px"
            });
            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateAccountingNotificationsSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();

            string Classes = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.NoInvoice) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "No Invoice",
                Controller = "Requests",
                Action = "AccountingNotifications",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingNotificationsEnum", AppUtility.SidebarEnum.NoInvoice }
                },
                Classes = Classes,
                IconName = "icon-cancel_presentation-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.DidntArrive) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Didn't Arrive",
                Controller = "Requests",
                Action = "AccountingNotifications",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingNotificationsEnum", AppUtility.SidebarEnum.DidntArrive }
                },
                Classes = Classes,
                IconName = "icon-local_shipping-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.PartialDelivery) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Partial Delivery",
                Controller = "Requests",
                Action = "AccountingNotifications",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingNotificationsEnum", AppUtility.SidebarEnum.PartialDelivery }
                },
                Classes = Classes,
                IconName = "icon-remove_shopping_cart-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.ForClarification) { Classes += SideBarActiveClasses; } else { Classes = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "For Clarification",
                Controller = "Requests",
                Action = "AccountingNotifications",
                RouteValues = new RouteValueDictionary()
                {
                    {"accountingNotificationsEnum", AppUtility.SidebarEnum.ForClarification }
                },
                Classes = Classes,
                IconName = "icon-notification_didnt_arrive-24px"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> CreateAccountingSuppliersSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.AllSuppliers) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
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
                IconName = "icon-storefront-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.NewSupplier) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
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
                IconName = "icon-add_circle_outline-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Search) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
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

        public static List<MenuItems> CreateAccountingGeneralSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.List) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "List",
                Controller = "Requests",
                Action = "AccountingGeneral",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType",  AppUtility.PageTypeEnum.AccountingGeneral },
                    { "SectionType", AppUtility.MenuItems.Accounting },
                    { "SidebarType", AppUtility.SidebarEnum.List }
                },
                Classes = CurrentClasses,
                IconName = "icon-format_list_bulleted-24px-01"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Search) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType",  AppUtility.PageTypeEnum.AccountingGeneral },
                    { "SectionType", AppUtility.MenuItems.Accounting },
                    { "SidebarType", AppUtility.SidebarEnum.Search }
                },
                Classes = CurrentClasses,
                IconName = "icon-zoom_in-24px-01"
            });

            return SidebarMenuItems;
        }


        public static List<MenuItems> CreateOperationsRequestsSidebar(AppUtility.PageTypeEnum PageType, AppUtility.SidebarEnum SidebarTitle, AppUtility.PageTypeEnum pageType, AppUtility.CategoryTypeEnum categoryType)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.List) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Last Item",
                Controller = "Requests",//"Operations",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                        {
                            {"PageType",  AppUtility.PageTypeEnum.OperationsRequest },
                            { "SectionType", AppUtility.MenuItems.Operations },
                            { "SidebarType", AppUtility.SidebarEnum.List },
                            { "RequestStatusID", 2 }
                        },
                Classes = CurrentClasses,
                IconName = "icon-format_list_bulleted-24px-01"
            });

            if (SidebarTitle == AppUtility.SidebarEnum.Add) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Add Item",
                Controller = "Requests",//"Operations",
                Action = "AddItemView",
                RouteValues = new RouteValueDictionary()
                {
                    { "PageType", pageType },
                    { "CategoryType", categoryType },
                    { "SectionType", AppUtility.MenuItems.Operations }
                },
                Classes = CurrentClasses,
                IconName = "icon-add_circle_outline-24px-1",
                AjaxLink = "add-item-sidebar"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Type) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Type",
                Controller = "ProductSubcategories",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", PageType },
                    { "SectionType", AppUtility.MenuItems.Operations }
                },
                Classes = CurrentClasses,
                IconName = "icon-category-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Vendors) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Vendor",
                Controller = "Vendors",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", PageType },
                    { "SectionType", AppUtility.MenuItems.Operations}
                },
                Classes = CurrentClasses,
                IconName = "icon-storefront-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Owner) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Owner",
                Controller = "ApplicationUsers",
                Action = "Index",
                RouteValues = new RouteValueDictionary()
                {
                    {"PageType", PageType },
                    { "SectionType", AppUtility.MenuItems.Operations }
                },
                Classes = CurrentClasses,
                IconName = "icon-face-24px"
            });

            return SidebarMenuItems;
        }


        public static List<MenuItems> CreateOperationsSearchSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.Search) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Search",
                Controller = "Requests",
                Action = "Search",
                RouteValues = new RouteValueDictionary()
                {
                    {"SectionType", AppUtility.MenuItems.Operations.ToString() }
                },
                Classes = CurrentClasses,
                IconName = "icon-zoom_in-24px-01"
            });

            return SidebarMenuItems;
        }


        public static List<MenuItems> CreateUsersUsersSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.List) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "List",
                Controller = "Admin",
                Action = "Index",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-format_list_bulleted-24px-01"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Add) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Add",
                Controller = "Admin",
                Action = "CreateUser",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-add_circle_outline-24px-1"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> CreateUsersWorkersSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.Details) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Details",
                Controller = "ApplicationUsers",
                Action = "Details",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-assignment_ind-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Hours) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Hours",
                Controller = "ApplicationUsers",
                Action = "Hours",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });
            //if (SidebarTitle == AppUtility.SidebarEnum.Salary) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            //SidebarMenuItems.Add(new MenuItems()
            //{
            //    Description = "Salary",
            //    Controller = "ApplicationUsers",
            //    Action = "Salary",
            //    RouteValues = new RouteValueDictionary(),
            //    Classes = CurrentClasses,
            //    IconName = "icon-monetization_on-24px"
            //});
            if (SidebarTitle == AppUtility.SidebarEnum.AwaitingApproval) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
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

        public static List<MenuItems> CreateTimekeeperReportsSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.ReportHours) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Hours",
                Controller = "Timekeeper",
                Action = "ReportHours",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-assignment-24px"
            });

            if (SidebarTitle == AppUtility.SidebarEnum.ReportDaysOff) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Days Off",
                Controller = "Timekeeper",
                Action = "ReportDaysOff",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-event-24px"
            });


            return SidebarMenuItems;
        }
        public static List<MenuItems> CreateTimekeeperSummarySidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;
            if (SidebarTitle == AppUtility.SidebarEnum.SummaryHours) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Hours",
                Controller = "Timekeeper",
                Action = "SummaryHours",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });

            if (SidebarTitle == AppUtility.SidebarEnum.SummaryDaysOff) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Days Off",
                Controller = "Timekeeper",
                Action = "SummaryDaysOff",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-highlight_off-24px"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateExpensesSummarySidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.PieCharts) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Pie Charts",
                Controller = "Expenses",
                Action = "SummaryPieCharts",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-pie_chart-24px2"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Tables) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Tables",
                Controller = "Expenses",
                Action = "SummaryTables",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-table_chart-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Graphs) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
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

        public static List<MenuItems> CreateExpensesStatisticsSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.Project) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Project",
                Controller = "Expenses",
                Action = "StatisticsProject",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-folder_open-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Item) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Item",
                Controller = "Expenses",
                Action = "StatisticsItem",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-centarix-icons-05"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Worker) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Worker",
                Controller = "Expenses",
                Action = "StatisticsWorker",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-face-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Category) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Category",
                Controller = "Expenses",
                Action = "StatisticsCategory",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-storefront-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Vendors) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
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

        public static List<MenuItems> CreateExpensesCostsSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.Project) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Project",
                Controller = "Expenses",
                Action = "CostsProject",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-zoom_in-24px-01"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Search) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Advanced Search",
                Controller = "Expenses",
                Action = "CostsAdvancedSearch",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-zoom_in-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.List) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
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

        public static List<MenuItems> CreateExpensesWorkersSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.Details) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Details",
                Controller = "Expenses",
                Action = "WorkersDetails",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-assignment_ind-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Hours) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Hours",
                Controller = "Expenses",
                Action = "WorkersHours",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Salary) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Salary",
                Controller = "Expenses",
                Action = "WorkersSalary",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-monetization_on-24px"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateProtocolsWorkflowSidebar(AppUtility.SidebarEnum SidebarTitle
            )
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.CurrentProtocols) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Current Protocols",
                Controller = "Protocols",
                Action = "CurrentProtocols",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-assignment_ind-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Projects) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Projects",
                Controller = "Protocols",
                Action = "Projects",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.SharedProjects) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Shared Projects",
                Controller = "Protocols",
                Action = "SharedProjects",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-notification_shared-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Calendar) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Calendar",
                Controller = "Protocols",
                Action = "Calendar",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-monetization_on-24px"
            });

            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateProtocolsProtocolsSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.List) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "List",
                Controller = "Protocols",
                Action = "Index",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-format_list_bulleted-24px-01"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.MyProtocols) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "My Protocols",
                Controller = "Protocols",
                Action = "MyProtocols",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Favorites) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Favorites",
                Controller = "Protocols",
                Action = "ProtocolsFavorites",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-favorite_border-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.SharedWithMe) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Shared With Me",
                Controller = "Protocols",
                Action = "ProtocolsSharedWithMe",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-notification_shared-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.LastProtocol) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Last Protocol",
                Controller = "Protocols",
                Action = "LastProtocol",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-monetization_on-24px"
            });

            return SidebarMenuItems;
        }
        public static List<MenuItems> CreateProtocolsCreateSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.ResearchProtocol) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Research Protocol",
                Controller = "Protocols",
                Action = "ResearchProtocol",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-assignment_ind-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.KitProtocol) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Kit Protocol",
                Controller = "Protocols",
                Action = "KitProtocol",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.SOPProtocol) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "SOP Protocol",
                Controller = "Protocols",
                Action = "SOPProtocol",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-monetization_on-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.BufferCreating) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Buffer Creating",
                Controller = "Protocols",
                Action = "BufferCreating",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-monetization_on-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.RoboticProtocol) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Robotic Protocol",
                Controller = "Protocols",
                Action = "RobioticProtocol",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-monetization_on-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.MaintenanceProtocol) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Maintenance Protocol",
                Controller = "Protocols",
                Action = "MaintenanceProtocol",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-monetization_on-24px"
            });
            return SidebarMenuItems;
        }
        public static List<MenuItems> CreateProtocolsReportsSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            //if (SidebarTitle == AppUtility.SidebarEnum.DailyReports) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            //SidebarMenuItems.Add(new MenuItems()
            //{
            //    Description = "Daily Reports",
            //    Controller = "Protocols",
            //    Action = "DailyReports",
            //    RouteValues = new RouteValueDictionary(),
            //    Classes = CurrentClasses,
            //    IconName = "icon-assignment_ind-24px-1"
            //});
            if (SidebarTitle == AppUtility.SidebarEnum.WeeklyReports) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Weekly Reports",
                Controller = "Protocols",
                Action = "ReportsCategories",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });
            //if (SidebarTitle == AppUtility.SidebarEnum.MonthlyReports) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            //SidebarMenuItems.Add(new MenuItems()
            //{
            //    Description = "Monthly Reports",
            //    Controller = "Protocols",
            //    Action = "MonthlyReports",
            //    RouteValues = new RouteValueDictionary(),
            //    Classes = CurrentClasses,
            //    IconName = "icon-monetization_on-24px"
            //});
            if (SidebarTitle == AppUtility.SidebarEnum.Favorites) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Favorites",
                Controller = "Protocols",
                Action = "IndexFavoriteReports",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-favorite_border-24px"
            });
            //if (SidebarTitle == AppUtility.SidebarEnum.SharedWithMe) { CurrentClasses += ActiveClasses; } else { CurrentClasses = OrigClasses; }
            //SidebarMenuItems.Add(new MenuItems()
            //{
            //    Description = "Shared With Me",
            //    Controller = "Protocols",
            //    Action = "ReportsSharedWithMe",
            //    RouteValues = new RouteValueDictionary(),
            //    Classes = CurrentClasses,
            //    IconName = "icon-notification_shared-24px"
            //});

            return SidebarMenuItems;

        }
        public static List<MenuItems> CreateProtocolsResourcesSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;

            if (SidebarTitle == AppUtility.SidebarEnum.Library) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Library",
                Controller = "Protocols",
                Action = "Library",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-assignment_ind-24px-1"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Personal) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Personal",
                Controller = "Protocols",
                Action = "Personal",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-access_time-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.SharedWithMe) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Shared With Me",
                Controller = "Protocols",
                Action = "ResourcesSharedWithMe",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-notification_shared-24px"
            });
            if (SidebarTitle == AppUtility.SidebarEnum.Favorites) { CurrentClasses += SideBarActiveClasses; } else { CurrentClasses = OrigClasses; }
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Favorites",
                Controller = "Protocols",
                Action = "ResourcesFavorites",
                RouteValues = new RouteValueDictionary() {
                    { "SidebarEnum" , AppUtility.SidebarEnum.Favorites }
                },
                Classes = CurrentClasses,
                IconName = "icon-favorite_border-24px"
            });
            return SidebarMenuItems;
        }

        public static List<MenuItems> CreateBiomarkersExperimentsSidebar(AppUtility.SidebarEnum SidebarTitle)
        {
            List<MenuItems> SidebarMenuItems = new List<MenuItems>();
            string CurrentClasses = OrigClasses;
            CurrentClasses = SidebarTitle == AppUtility.SidebarEnum.HumanTrials ? CurrentClasses += SideBarActiveClasses : OrigClasses;
            SidebarMenuItems.Add(new MenuItems()
            {
                Description = "Human Trials",
                Controller = "Biomarkers",
                Action = "HumanTrialsList",
                RouteValues = new RouteValueDictionary(),
                Classes = CurrentClasses,
                IconName = "icon-favorite_border-24px"
            });
            return SidebarMenuItems;
        }
    }
}
