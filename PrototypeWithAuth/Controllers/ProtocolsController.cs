using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Controllers
{
    public class ProtocolsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
           // var viewmodel = await GetIndexViewModel(requestIndexObject);

            return View(/*viewmodel*/);
        }
        private static IQueryable<Protocol> filterListBySelectFilters(SelectedProtocolsFilters selectedFilters, IQueryable<Protocol> fullRequestsListProprietary)
        {
            if (selectedFilters != null)
            {
                if (selectedFilters.SelectedCategoriesIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(p => selectedFilters.SelectedCategoriesIDs.Contains(p.ProtocolSubCategory.ProtocolCategoryTypeID));
                }
                if (selectedFilters.SelectedProtocolsSubcategoriesIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(p => selectedFilters.SelectedProtocolsSubcategoriesIDs.Contains(p.ProtocolSubCategoryID));
                }
                if (selectedFilters.SelectedOwnersIDs.Count() > 0)
                {
                    fullRequestsListProprietary = fullRequestsListProprietary.Where(p => selectedFilters.SelectedOwnersIDs.Contains(p.ApplicationUserCreatorID));
                }
            }

            return fullRequestsListProprietary;
        }

        //private async Task<RequestIndexPartialViewModel> GetIndexViewModel(RequestIndexObject requestIndexObject, List<int> Months = null, List<int> Years = null, SelectedFilters selectedFilters = null)
        //{
        //    int categoryID = 1;
        //    if (requestIndexObject.SectionType == AppUtility.MenuItems.Operations)
        //    {
        //        categoryID = 2;
        //    }
        //    IQueryable<Request> RequestsPassedIn = Enumerable.Empty<Request>().AsQueryable();
        //    IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ApplicationUserCreator)
        // .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).Include(r => r.ParentQuote)
        // .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID).Include(x => x.ParentRequest);

        //    int sideBarID = 0;
        //    if (requestIndexObject.SidebarType != AppUtility.SidebarEnum.Owner)
        //    {
        //        int.TryParse(requestIndexObject.SidebarFilterID, out sideBarID);
        //    }

        //    if (requestIndexObject.PageType == AppUtility.PageTypeEnum.LabManagementEquipment)
        //    {
        //        if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Category)
        //        {
        //            RequestsPassedIn = _context.Requests.Where(r => r.RequestStatusID == 3).Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5).Include(r => r.Product.ProductSubcategory)
        //                .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance)
        //                .Include(r => r.ParentRequest).Where(r => r.Product.ProductSubcategoryID == sideBarID).Include(r => r.ParentRequest);

        //        }
        //        else
        //        {
        //            RequestsPassedIn = _context.Requests.Where(r => r.RequestStatusID == 3).Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5).Include(r => r.Product.ProductSubcategory)
        //                .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance)
        //                .Include(r => r.ParentRequest);
        //        }
        //        RequestsPassedIn.OrderByDescending(e => e.ParentRequest.OrderDate);
        //    }
        //    else if (ViewData["ReturnRequests"] != null)
        //    {
        //        RequestsPassedIn = TempData["ReturnRequests"] as IQueryable<Request>;
        //    }
        //    else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestRequest || requestIndexObject.PageType == AppUtility.PageTypeEnum.OperationsRequest)
        //    {
        //        /*
        //         * In order to combine all the requests each one needs to be in a separate list
        //         * Then you need to union them one at a time into separate variables
        //         * you only need the separate union variable in order for the union to work
        //         * and the original queries are on separate lists because each is querying the full database with a separate where
        //         */

        //        IQueryable<Request> TempRequestList = Enumerable.Empty<Request>().AsQueryable();
        //        if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 1)
        //        {
        //            TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 1);
        //            RequestsPassedIn = TempRequestList;
        //        }

        //        if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 6)
        //        {
        //            TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 6);
        //            RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
        //        }

        //        if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 2)
        //        {
        //            TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 2);
        //            RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
        //        }

        //        if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 3)
        //        {
        //            TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 3, 50);
        //            RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
        //            TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 4);
        //            RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
        //            TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 5);
        //            RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
        //        }

        //    }
        //    else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestSummary)
        //    {
        //        if (requestIndexObject.RequestStatusID == 7)
        //        {
        //            RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 7).Include(r => r.Product.ProductSubcategory)
        //         .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).ToList().GroupBy(r => r.ProductID).Select(e => e.First()).AsQueryable();
        //        }
        //        else
        //        {
        //            RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3 || r.RequestStatus.RequestStatusID == 4 || r.RequestStatus.RequestStatusID == 5).Include(r => r.Product.ProductSubcategory)
        //           .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).ToList().GroupBy(r => r.ProductID).Select(e => e.First()).AsQueryable();
        //        }

        //    }
        //    else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.AccountingGeneral)
        //    {
        //        //we need both categories
        //        RequestsPassedIn = _context.Requests.Include(r => r.ApplicationUserCreator)
        //             .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).Include(r => r.ParentQuote)
        //             .Include(r => r.ParentRequest).Where(r => Years.Contains(r.ParentRequest.OrderDate.Year)).Where(r => r.HasInvoice && r.Paid && !r.IsClarify && !r.IsPartial);
        //        if (Months != null)
        //        {
        //            RequestsPassedIn = RequestsPassedIn.Where(r => Months.Contains(r.ParentRequest.OrderDate.Month));
        //        }
        //    }
        //    else //we just want what is in inventory
        //    {
        //        RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3 || r.RequestStatus.RequestStatusID == 4 || r.RequestStatus.RequestStatusID == 5);
        //    }
        //    AppUtility.SidebarEnum SidebarTitle = AppUtility.SidebarEnum.List;
        //    //now that the lists are created sort by vendor or subcategory
        //    var sidebarFilterDescription = "";
        //    switch (requestIndexObject.SidebarType)
        //    {
        //        case AppUtility.SidebarEnum.Vendors:
        //            sidebarFilterDescription = _context.Vendors.Where(v => v.VendorID == sideBarID).Select(v => v.VendorEnName).FirstOrDefault();
        //            RequestsPassedIn = RequestsPassedIn
        //             .OrderByDescending(r => r.ProductID)
        //             .Where(r => r.Product.VendorID == sideBarID);
        //            break;
        //        case AppUtility.SidebarEnum.Type:
        //            sidebarFilterDescription = _context.ProductSubcategories.Where(p => p.ProductSubcategoryID == sideBarID).Select(p => p.ProductSubcategoryDescription).FirstOrDefault();
        //            RequestsPassedIn = RequestsPassedIn
        //           .OrderByDescending(r => r.ProductID)
        //           .Where(r => r.Product.ProductSubcategoryID == sideBarID);
        //            break;
        //        case AppUtility.SidebarEnum.Owner:
        //            var owner = _context.Employees.Where(e => e.Id.Equals(requestIndexObject.SidebarFilterID)).FirstOrDefault();
        //            sidebarFilterDescription = owner.FirstName + " " + owner.LastName;
        //            RequestsPassedIn = RequestsPassedIn
        //            .OrderByDescending(r => r.ProductID)
        //            .Where(r => r.ApplicationUserCreatorID == requestIndexObject.SidebarFilterID);
        //            break;
        //    }


        //    RequestIndexPartialViewModel requestIndexViewModel = new RequestIndexPartialViewModel();
        //    requestIndexViewModel.PricePopoverViewModel = new PricePopoverViewModel();
        //    requestIndexViewModel.PageNumber = requestIndexObject.PageNumber;
        //    requestIndexViewModel.RequestStatusID = requestIndexObject.RequestStatusID;
        //    requestIndexViewModel.PageType = requestIndexObject.PageType;
        //    requestIndexViewModel.SidebarFilterID = requestIndexObject.SidebarFilterID;
        //    requestIndexViewModel.ErrorMessage = requestIndexObject.ErrorMessage;
        //    var onePageOfProducts = Enumerable.Empty<RequestIndexPartialRowViewModel>().ToPagedList();

        //    var RequestPassedInWithInclude = RequestsPassedIn.Include(r => r.Product.ProductSubcategory)
        //        .Include(r => r.ParentRequest).Include(r => r.ApplicationUserCreator)
        //        .Include(r => r.Product.Vendor).Include(r => r.RequestStatus)
        //        .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
        //        .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).AsQueryable();


        //    RequestPassedInWithInclude = filterListBySelectFilters(selectedFilters, RequestPassedInWithInclude);

        //    onePageOfProducts = await GetColumnsAndRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude);

        //    requestIndexViewModel.PagedList = onePageOfProducts;
        //    List<PriceSortViewModel> priceSorts = new List<PriceSortViewModel>();
        //    Enum.GetValues(typeof(AppUtility.PriceSortEnum)).Cast<AppUtility.PriceSortEnum>().ToList().ForEach(p => priceSorts.Add(new PriceSortViewModel { PriceSortEnum = p, Selected = requestIndexObject.SelectedPriceSort.Contains(p.ToString()) }));
        //    requestIndexViewModel.PricePopoverViewModel.PriceSortEnums = priceSorts;
        //    requestIndexViewModel.PricePopoverViewModel.SelectedCurrency = requestIndexObject.SelectedCurrency;
        //    requestIndexViewModel.SidebarFilterName = sidebarFilterDescription;
        //    bool isProprietary = requestIndexObject.RequestStatusID == 7 ? true : false;
        //    requestIndexViewModel.InventoryFilterViewModel = GetInventoryFilterViewModel(selectedFilters, isProprietary: isProprietary);
        //    return requestIndexViewModel;
        //}
        public async Task<IActionResult> CurrentProtocols()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.CurrentProtocols;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            return View();
        }
        public async Task<IActionResult> Projects()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Projects;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            return View();
        }
        public async Task<IActionResult> SharedProjects()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedProjects;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            return View();
        }
        public async Task<IActionResult> Calendar()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Calendar;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsWorkflow;
            return View();
        }
        public async Task<IActionResult> MyProtocols()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MyProtocols;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            return View();
        }

        public async Task<IActionResult> ProtocolsFavorites()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MyProtocols;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            return View();
        }

        public async Task<IActionResult> ProtocolsSharedWithMe()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedWithMe;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            return View();
        }
        public async Task<IActionResult> LastProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.LastProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsProtocols;
            return View();
        }
        public async Task<IActionResult> ResearchProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ResearchProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            return View();
        }

        public async Task<IActionResult> KitProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.KitProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            return View();
        }

        public async Task<IActionResult> SopProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SOPProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            return View();
        }
        public async Task<IActionResult> BufferCreating()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.BufferCreating;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            return View();
        }
        public async Task<IActionResult> RobioticProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.RoboticProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            return View();
        }
        public async Task<IActionResult> MaintenanceProtocol()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MaintenanceProtocol;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsCreate;
            return View();
        }

        public async Task<IActionResult> DailyReports()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.DailyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            return View();
        }
        public async Task<IActionResult> WeeklyReports()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.WeeklyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            return View();
        }
        public async Task<IActionResult> MonthlyReports()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MonthlyReports;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            return View();
        }

        public async Task<IActionResult> ReportsSharedWithMe()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedWithMe;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsReports;
            return View();
        }

        public async Task<IActionResult> Library()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Library;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;
            return View();
        }
        public async Task<IActionResult> Personal()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Personal;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;
            return View();
        }


        public async Task<IActionResult> ResourcesSharedWithMe()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedWithMe;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;
            return View();
        }
        public async Task<IActionResult> ResourcesFavorites()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Favorites;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsResources;
            return View();
        }

        public async Task<IActionResult> Active()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Active;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsTask;
            return View();
        }
      

        public async Task<IActionResult> Done()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Done;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsTask;
            return View();

        }
      

  

       
        public async Task<IActionResult> Search()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Protocols;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Search;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.ProtocolsSearch;
            return View();
        }

       
    }
}
