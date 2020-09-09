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
                    { "CategoryType", categoryType }
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
                    { "CategoryType", categoryType }
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
    }
}
