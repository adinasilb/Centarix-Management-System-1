using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using X.PagedList;
using Microsoft.AspNetCore.Hosting;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SelectPdf;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Ocsp;
using System.Xml.Linq;
using System.Diagnostics;
using Abp.Extensions;
using SQLitePCL;
using Microsoft.AspNetCore.Localization;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Query;
using System.Text.Json;
using Newtonsoft.Json;
//using Org.BouncyCastle.Asn1.X509;
//using System.Data.Entity.Validation;f
//using System.Data.Entity.Infrastructure;

namespace PrototypeWithAuth.Controllers
{
    public class RequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //take this out?
        private readonly IHostingEnvironment _hostingEnvironment;
        private ISession _session;

        //private readonly IHttpContextAccessor _Context;

        //take this out?
        private readonly List<Request> _cartRequests = new List<Request>();

        private IQueryable<Request> _searchList = Enumerable.Empty<Request>().AsQueryable();
        private ICompositeViewEngine _viewEngine;

        //public MyController(ICompositeViewEngine viewEngine)
        //{
        //    _viewEngine = viewEngine;
        //}
        public RequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment hostingEnvironment, ICompositeViewEngine viewEngine /*IHttpContextAccessor Context*/, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            //_Context = Context;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            //use the hosting environment for the file uploads
            _hostingEnvironment = hostingEnvironment;
            _viewEngine = viewEngine;
            //_session = httpContextAccessor.HttpContext.c
        }

        [HttpGet]
        [Authorize(Roles = "Requests, Operations")]
        // GET: Requests
        //IMPORTANT!!! When adding more parameters into the Index Get make sure to add them to the ViewData and follow them through to the Index page
        //ALSO when changing defaults -> change the defaults on the index page for paged list 
        public async Task<IActionResult> Index(RequestIndexObject requestIndexObject)
        {

            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;

            var viewmodel = await GetIndexViewModel(requestIndexObject);

            SetViewModelCounts(requestIndexObject, viewmodel);

            //todo: move to view model or the like
            TempData["Email1"] = TempData["Email1"];
            TempData["Email2"] = TempData["Email2"];
            TempData["Email3"] = TempData["Email3"];
            TempData["Email4"] = TempData["Email4"];
            TempData["Email5"] = TempData["Email5"];



            if (ViewBag.ErrorMessage != null)
            {
                ViewBag.ErrorMessage = ViewBag.ErrorMessage;
            }

            return View(viewmodel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests, Operations")]
        // GET: Requests
        //IMPORTANT!!! When adding more parameters into the Index Get make sure to add them to the ViewData and follow them through to the Index page
        //ALSO when changing defaults -> change the defaults on the index page for paged list 
        public async Task<IActionResult> IndexInventory(RequestIndexObject requestIndexObject)
        {

            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;

            var viewmodel = await GetIndexViewModel(requestIndexObject);

            if (ViewBag.ErrorMessage != null)
            {
                ViewBag.ErrorMessage = ViewBag.ErrorMessage;
            }

            return View(viewmodel);
        }

        private void SetViewModelCounts(RequestIndexObject requestIndexObject, RequestIndexPartialViewModel viewmodel)
        {
            int categoryID = 0;
            if (requestIndexObject.SectionType == AppUtility.MenuItems.Requests)
            {
                categoryID = 1;
            }
            else if (requestIndexObject.SectionType == AppUtility.MenuItems.Operations)
            {
                categoryID = 2;
            }
            IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ApplicationUserCreator).Include(r => r.Product).ThenInclude(p => p.Vendor)
              .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID);

            int newCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 1, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int orderedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 2, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int receivedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 3, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int approvedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 6, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            newCount += AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 4, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            newCount += AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 5, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            viewmodel.NewCount = newCount;
            viewmodel.ApprovedCount = approvedCount;
            viewmodel.OrderedCount = orderedCount;
            viewmodel.ReceivedCount = receivedCount;
        }

        [Authorize(Roles = "Reports")]
        private async Task<RequestIndexViewModel> GetExpensesItemsViewModel(int page = 1, List<int> CategoryTypeIDs = null, List<int> Months = null, List<int> Years = null, String SortType = null)
        {
            RequestIndexViewModel viewModel = new RequestIndexViewModel()
            {
                Page = page,
                MenuType = AppUtility.MenuItems.Reports
            };
            return viewModel;
        }

        [Authorize(Roles = "Requests, LabManagement, Operations")]
        private async Task<RequestIndexPartialViewModel> GetIndexViewModel(RequestIndexObject requestIndexObject)
        {
            int categoryID = 1;
            if (requestIndexObject.SectionType == AppUtility.MenuItems.Operations)
            {
                categoryID = 2;
            }
            IQueryable<Request> RequestsPassedIn = Enumerable.Empty<Request>().AsQueryable();
            IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ApplicationUserCreator)
         .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).Include(r => r.ParentQuote)
         .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID).Include(x => x.ParentRequest);

            int sideBarID = 0;
            if (requestIndexObject.SidebarType != AppUtility.SidebarEnum.Owner)
            {
                int.TryParse(requestIndexObject.SidebarFilterID, out sideBarID);
            }

            if (requestIndexObject.PageType == AppUtility.PageTypeEnum.LabManagementEquipment)
            {
                if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Categories)
                {
                    RequestsPassedIn = _context.Requests.Where(r => r.RequestStatusID == 3).Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5).Include(r => r.Product.ProductSubcategory)
                        .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance)
                        .Include(r => r.ParentRequest).Where(r => r.Product.ProductSubcategoryID == sideBarID).Include(r => r.ParentRequest);

                }
                else
                {
                    RequestsPassedIn = _context.Requests.Where(r => r.RequestStatusID == 3).Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5).Include(r => r.Product.ProductSubcategory)
                        .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance)
                        .Include(r => r.ParentRequest);
                }
                RequestsPassedIn.OrderByDescending(e => e.ParentRequest.OrderDate);
            }
            else if (ViewData["ReturnRequests"] != null)
            {
                RequestsPassedIn = TempData["ReturnRequests"] as IQueryable<Request>;
            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestRequest || requestIndexObject.PageType == AppUtility.PageTypeEnum.OperationsRequest)
            {
                /*
                 * In order to combine all the requests each one needs to be in a separate list
                 * Then you need to union them one at a time into separate variables
                 * you only need the separate union variable in order for the union to work
                 * and the original queries are on separate lists because each is querying the full database with a separate where
                 */

                IQueryable<Request> TempRequestList = Enumerable.Empty<Request>().AsQueryable();
                if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 1)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 1);
                    RequestsPassedIn = TempRequestList;
                }

                if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 6)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 6);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }

                if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 2)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 2);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }

                if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 3)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 3, 50);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }
                if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 4)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 4);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }
                if (requestIndexObject.RequestStatusID == 0 || requestIndexObject.RequestStatusID == 5)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 5);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }


            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestSummary)
            {
                RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3).Include(r => r.Product.ProductSubcategory)
                     .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).ToList().GroupBy(r => r.ProductID).Select(e => e.First()).AsQueryable();
            }
            else //we just want what is in inventory
            {
                RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3);
            }
            AppUtility.SidebarEnum SidebarTitle = AppUtility.SidebarEnum.List;
            //now that the lists are created sort by vendor or subcategory
            var sidebarFilterDescription = "";
            switch (requestIndexObject.SidebarType)
            {
                case AppUtility.SidebarEnum.Vendors:
                    sidebarFilterDescription = _context.Vendors.Where(v => v.VendorID == sideBarID).Select(v => v.VendorEnName).FirstOrDefault();
                    RequestsPassedIn = RequestsPassedIn
                     .OrderByDescending(r => r.ProductID)
                     .Where(r => r.Product.VendorID == sideBarID);
                    break;
                case AppUtility.SidebarEnum.Type:
                    sidebarFilterDescription = _context.ProductSubcategories.Where(p => p.ProductSubcategoryID==sideBarID).Select(p => p.ProductSubcategoryDescription).FirstOrDefault();
                    RequestsPassedIn = RequestsPassedIn
                   .OrderByDescending(r => r.ProductID)
                   .Where(r => r.Product.ProductSubcategoryID == sideBarID);
                    break;
                case AppUtility.SidebarEnum.Owner:
                    var owner = _context.Employees.Where(e => e.Id.Equals(requestIndexObject.SidebarFilterID)).FirstOrDefault();
                    sidebarFilterDescription = owner.FirstName + " " + owner.LastName;
                    RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.ApplicationUserCreatorID == requestIndexObject.SidebarFilterID);
                    break;
            }


            RequestIndexPartialViewModel requestIndexViewModel = new RequestIndexPartialViewModel();
            requestIndexViewModel.PageNumber = requestIndexObject.PageNumber;
            requestIndexViewModel.RequestStatusID = requestIndexObject.RequestStatusID;
            requestIndexViewModel.PageType = requestIndexObject.PageType;
            requestIndexViewModel.SidebarFilterID = requestIndexObject.SidebarFilterID;
            var onePageOfProducts = Enumerable.Empty<RequestIndexPartialRowViewModel>().ToPagedList();

            try
            {
                var RequestPassedInWithInclude = RequestsPassedIn.Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.ParentRequest)
                    .Include(r => r.Product.Vendor).Include(r => r.RequestStatus)
                    .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                    .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance);

                onePageOfProducts = await GetColumnsAndRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                // Redirect("~/Views/Shared/RequestError.cshtml");
            }
            requestIndexViewModel.PagedList = onePageOfProducts;
            List<PriceSortViewModel> priceSorts = new List<PriceSortViewModel>();
            Enum.GetValues(typeof(AppUtility.PriceSortEnum)).Cast<AppUtility.PriceSortEnum>().ToList().ForEach(p => priceSorts.Add(new PriceSortViewModel { PriceSortEnum = p, Selected = requestIndexObject.SelectedPriceSort.Contains(p.ToString()) }));
            requestIndexViewModel.PriceSortEnums = priceSorts;
            requestIndexViewModel.SelectedCurrency = requestIndexObject.SelectedCurrency;
            requestIndexViewModel.PageType = requestIndexObject.PageType;
            requestIndexViewModel.SidebarFilterName = sidebarFilterDescription;
            return requestIndexViewModel;
        }

        private async Task<IPagedList<RequestIndexPartialRowViewModel>> GetColumnsAndRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude)
        {
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var reorderIcon = new IconColumnViewModel(" icon-add_circle_outline-24px1 ", "#00CA72", "load-order-details", "Reorder");
            var orderOperations = new IconColumnViewModel(" icon-add_circle_outline-24px1 ", "#00CA72", "order-approved-operation", "Order");
            var deleteIcon = new IconColumnViewModel(" icon-delete-24px ", "black", "load-confirm-delete", "Delete");
            var receiveIcon = new IconColumnViewModel(" icon-done-24px ", "#00CA72", "load-receive-and-location", "Receive");
            var approveIcon = new IconColumnViewModel(" icon-centarix-icons-03 ", "#00CA72", "approve-order", "Approve");
            var equipmentIcon = new IconColumnViewModel(" icon-settings-24px-1 ", "var(--lab-man-color);", "create-calibration", "Create Calibration");
            var defaultImage = "/images/css/CategoryImages/placeholder.png";
            switch (requestIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.RequestRequest:
                    switch (requestIndexObject.RequestStatusID)
                    {
                        case 1:
                            iconList.Add(approveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetForApprovalRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 6:
                            iconList.Add(reorderIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetApprovedRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 2:
                            iconList.Add(receiveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetOrderedRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 3:
                            iconList.Add(reorderIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetReceivedInventoryRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                    }
                    break;
                case AppUtility.PageTypeEnum.OperationsRequest:
                    switch (requestIndexObject.RequestStatusID)
                    {
                        case 1:
                            iconList.Add(approveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetForApprovalOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 6:
                            iconList.Add(orderOperations);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetApprovedOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 2:
                            iconList.Add(receiveIcon);
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetOrderedOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                        case 3:
                            iconList.Add(deleteIcon);
                            onePageOfProducts = await GetReceivedInventoryOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                            break;
                    }
                    break;
                case AppUtility.PageTypeEnum.RequestInventory:
                    iconList.Add(reorderIcon);
                    iconList.Add(deleteIcon);
                    onePageOfProducts = await GetReceivedInventoryRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                    break;
                case AppUtility.PageTypeEnum.RequestSummary:
                    iconList.Add(reorderIcon);
                    iconList.Add(deleteIcon);
                    onePageOfProducts = await GetSummaryRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                    break;
                case AppUtility.PageTypeEnum.OperationsInventory:
                    iconList.Add(orderOperations);
                    iconList.Add(deleteIcon);
                    onePageOfProducts = await GetReceivedInventoryOperationsRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                    break;
                case AppUtility.PageTypeEnum.LabManagementEquipment:
                    iconList.Add(equipmentIcon);
                    iconList.Add(reorderIcon);
                    iconList.Add(deleteIcon);
                    onePageOfProducts = await GetReceivedInventoryRows(requestIndexObject, onePageOfProducts, RequestPassedInWithInclude, iconList, defaultImage);
                    break;
                case AppUtility.PageTypeEnum.ExpensesStatistics:
                    break;
            }

            return onePageOfProducts;
        }

        private static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetForApprovalRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.CreationDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width=12, Value = new List<string>(){ r.CreationDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel()
                                 {
                                     Title = "", Width=10, Icons = iconList,  AjaxID = r.RequestID
                                 }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        private static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetApprovedRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.CreationDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width=12, Value = new List<string>(){ r.CreationDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel()
                                 {
                                     Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                                 }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        private static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetOrderedRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=12, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel()
                                 {
                                     Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                                 }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        private async Task<IPagedList<RequestIndexPartialRowViewModel>> GetReceivedInventoryRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                             new RequestIndexPartialColumnViewModel() { Title = "", Width=9, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                             new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=14, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                             new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=9, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=9, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                             new RequestIndexPartialColumnViewModel() { Title = "Location", Width=9, Value = new List<string>(){ GetLocationInstanceNameBefore(r.RequestLocationInstances.FirstOrDefault().LocationInstance) } },
                             new RequestIndexPartialColumnViewModel() { Title = "Category", Width=9, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                             new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=10, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                             new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=10, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                             new RequestIndexPartialColumnViewModel()
                             {
                                 Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                             }
                        }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }
        private static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetForApprovalOperationsRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.CreationDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width=12, Value = new List<string>(){ r.CreationDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel()
                                 {
                                     Title = "", Width=10, Icons = iconList,  AjaxID = r.RequestID
                                 }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        private static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetApprovedOperationsRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.CreationDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width=12, Value = new List<string>(){ r.CreationDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel()
                                 {
                                     Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                                 }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        private static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetOrderedOperationsRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=12, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel()
                                 {
                                     Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                                 }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }

        private async Task<IPagedList<RequestIndexPartialRowViewModel>> GetReceivedInventoryOperationsRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                             new RequestIndexPartialColumnViewModel() { Title = "", Width=9, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                             new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=14, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                             new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=9, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Category", Width=9, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                             new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=10, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                             new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=10, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                             new RequestIndexPartialColumnViewModel()
                             {
                                 Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                             }
                        }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }
        private async Task<IPagedList<RequestIndexPartialRowViewModel>> GetSummaryRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.ParentRequest.OrderDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                             new RequestIndexPartialColumnViewModel() { Title = "", Width=9, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                             new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=14, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details-summary", AjaxID=r.RequestID},
                             new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=9, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=9, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                             new RequestIndexPartialColumnViewModel() { Title = "Location", Width=9, Value = new List<string>(){ GetLocationInstanceNameBefore(r.RequestLocationInstances.FirstOrDefault().LocationInstance) } },
                             new RequestIndexPartialColumnViewModel() { Title = "Category", Width=9, Value = new List<string>(){ r.Product.ProductSubcategory.ProductSubcategoryDescription} },
                             new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=10, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                             new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                             new RequestIndexPartialColumnViewModel() { Title = "Date Ordered", Width=10, Value = new List<string>(){ r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                             new RequestIndexPartialColumnViewModel()
                             {
                                 Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID
                             }
                        }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }
        private string GetLocationInstanceNameBefore(LocationInstance locationInstance)
        {
            var newLIName = _context.LocationInstances.Where(li => li.LocationInstanceID == locationInstance.LocationInstanceParentID).FirstOrDefault().LocationInstanceName;
            return newLIName;
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTableData(RequestIndexObject requestIndexObject)
        {
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTableWithCounts(RequestIndexObject requestIndexObject)
        {
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            SetViewModelCounts(requestIndexObject, viewModel);
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTable(RequestIndexObject requestIndexObject)
        {
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ItemTableOwner(RequestIndexObject requestIndexObject)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Owner;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            SetViewModelCounts(requestIndexObject, viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ItemTableVendor(RequestIndexObject requestIndexObject)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Vendors;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            SetViewModelCounts(requestIndexObject, viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ItemTableType(RequestIndexObject requestIndexObject)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Type;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            SetViewModelCounts(requestIndexObject, viewModel);
            return View(viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ItemTableEquipment()
        {
            RequestIndexObject requestIndexObject = new RequestIndexObject() { PageType = AppUtility.PageTypeEnum.LabManagementEquipment, SectionType = AppUtility.MenuItems.LabManagement, SidebarType = AppUtility.SidebarEnum.List };
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;

            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            return View(viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ItemTableEquipmentType()
        {
            RequestIndexObject requestIndexObject = new RequestIndexObject() { PageType = AppUtility.PageTypeEnum.LabManagementEquipment, SectionType = AppUtility.MenuItems.LabManagement, SidebarType = AppUtility.SidebarEnum.Type };
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;

            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            return View(viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteModal(int? id, bool isQuote = false, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Product not found (no id). Unable to delete.";
                return NotFound();
            }

            var request = await _context.Requests
               .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.Product.Vendor)
               .FirstOrDefaultAsync(m => m.RequestID == id);

            if (request == null)
            {
                ViewBag.ErrorMessage = "Product not found (no request). Unable to delete";
                return NotFound();
            }

            DeleteRequestViewModel deleteRequestViewModel = new DeleteRequestViewModel()
            {
                Request = request,
                IsReorder = isQuote,
                SectionType = SectionType
            };

            return View(deleteRequestViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteModal(int id, RequestIndexObject requestIndexObject)
        {
            var request = _context.Requests.Where(r => r.RequestID == id)
                .Include(r => r.RequestLocationInstances).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory)
                .ThenInclude(ps => ps.ParentCategory)
                .FirstOrDefault();
            request.IsDeleted = true;
            try
            {
                _context.Update(request);
                await  _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Unable to Delete. " + ex.Message;
                return NotFound();
            }
            var parentRequest = _context.ParentRequests.Where(pr => pr.ParentRequestID == request.ParentRequestID).FirstOrDefault();
            parentRequest.Requests = _context.Requests.Where(r => r.ParentRequestID == parentRequest.ParentRequestID && r.IsDeleted != true);

            if (parentRequest != null)
            {
                if (parentRequest.Requests.Count() == 0)
                {
                    parentRequest.IsDeleted = true;
                    try
                    {
                        _context.Update(parentRequest);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorHeader += "/n Parent Request Not Deleted";
                        ViewBag.ErrorText += "/n Data Alert: Request was deleted, but not the Parent Request";
                    }
                }

            }
            var parentQuote = _context.ParentQuotes.Where(pr => pr.ParentQuoteID == request.ParentQuoteID).FirstOrDefault();
            parentQuote.Requests = _context.Requests.Where(r => r.ParentQuoteID == parentQuote.ParentQuoteID && r.IsDeleted != true);
            if (parentQuote != null)
            {
                //todo figure out the soft delete with child of a parent entity so we could chnage it to 0 or null
                if (parentQuote.Requests.Count() == 0)
                {
                    parentQuote.IsDeleted = true;
                    try
                    {
                        _context.Update(parentQuote);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorHeader += "/n Parent Quote Not Deleted";
                        ViewBag.ErrorText += "/n Data Alert: Request was deleted, but not the Parent Quote";
                    }
                }

            }
            foreach (var requestLocationInstance in request.RequestLocationInstances)
            {
                requestLocationInstance.IsDeleted = true;
                var locationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstance.LocationInstanceID).FirstOrDefault();
                locationInstance.IsFull = false;
                try
                {
                    _context.Update(requestLocationInstance);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    //TODO: make sure the name won't be null over here
                    ViewBag.ErrorHeader += "/n Request not removed from location";
                    ViewBag.ErrorText += "/n Data Alert: Request was deleted, but not fully removed from the location: " + requestLocationInstance.LocationInstance.LocationInstanceName;
                }
                try
                {
                    _context.Update(locationInstance);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorHeader += "/n Request not removed from location";
                    ViewBag.ErrorText += "/n Data Alert: Request was deleted, but not fully removed from the location: " + locationInstance.LocationInstanceName;
                }
            }
            if (requestIndexObject.PageType == AppUtility.PageTypeEnum.LabManagementQuotes)
            {
                if(requestIndexObject.SidebarType == AppUtility.SidebarEnum.Quotes)
                {
                    return RedirectToAction("LabManageQuotes");
                }
                else 
                {
                    return RedirectToAction("LabManageOrders");
                }

            }
            else if(requestIndexObject.PageType== AppUtility.PageTypeEnum.RequestInventory || requestIndexObject.PageType == AppUtility.PageTypeEnum.OperationsInventory)
            {
                return RedirectToAction("_IndexTableData", requestIndexObject);
            }
            else
            {
                return RedirectToAction("_IndexTableWithCounts", requestIndexObject);
            }

        }
        [HttpGet]
        public async Task<IActionResult> CategoryModal()
        {
            ChooseCategoryViewModel categoryViewModel = new ChooseCategoryViewModel()
            {
                ParentCategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 1).ToListAsync()
            };

            return PartialView(categoryViewModel);
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> CreateModalView(int parentCategoryId, AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            var parentcategories = await _context.ParentCategories.Where(pc => pc.ParentCategoryID == parentCategoryId).FirstOrDefaultAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.ParentCategoryID == parentCategoryId).ToListAsync();
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 1).Count() > 0).ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var subprojects = await _context.SubProjects.ToListAsync();

            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var unittypeslookup = _context.UnitTypes.Include(u => u.UnitParentType).ToLookup(u => u.UnitParentType);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategory = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                Projects = projects,
                SubProjects = subprojects,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                UnitTypes = unittypeslookup,
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts,
                CommentTypes = commentTypes,
                Comments = new List<Comment>(),
                EmailAddresses = new List<string>() { "", "", "", "", "" },
                ModalType = AppUtility.RequestModalType.Create
                //CurrentUser = 
            };

            requestItemViewModel.Request = new Request();
            requestItemViewModel.Request.ExchangeRate = _context.ExchangeRates.FirstOrDefault().LatestExchangeRate;
            requestItemViewModel.Request.Product = new Product();
            requestItemViewModel.Request.ParentQuote = new ParentQuote();
            requestItemViewModel.Request.SubProject = new SubProject();

            requestItemViewModel.Request.CreationDate = DateTime.Now;

            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string requestFolder = Path.Combine(uploadFolder, "0");

            if (Directory.Exists(requestFolder))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(requestFolder);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(requestFolder);
            }
            Directory.CreateDirectory(requestFolder);

            //TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.RequestPageTypeEnum.Request;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            if (AppUtility.IsAjaxRequest(this.Request))
            {
                return PartialView(requestItemViewModel);
            }
            else
            {
                return View(requestItemViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> CreateModalView(RequestItemViewModel requestItemViewModel, AppUtility.OrderTypeEnum OrderType)
        {
            //why do we need this here?
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.Include(ps => ps.ParentCategory).FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

            //in case we need to return to the modal view
            requestItemViewModel.ParentCategory = await _context.ParentCategories.Where(pc => pc.ParentCategoryID == requestItemViewModel.Request.Product.ProductSubcategory.ParentCategory.ParentCategoryID).FirstOrDefaultAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 1).ToListAsync();
            requestItemViewModel.Projects = await _context.Projects.ToListAsync();
            requestItemViewModel.SubProjects = await _context.SubProjects.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            requestItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();
            //formatting the select list of the unit types
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");

            //declared outside the if b/c it's used farther down too 
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            requestItemViewModel.Request.ApplicationUserCreatorID = currentUser.Id;
            requestItemViewModel.Request.ApplicationUserCreator = currentUser;
            requestItemViewModel.Request.ParentQuote.ApplicationUser = currentUser;
            requestItemViewModel.Request.CreationDate = DateTime.Now;

            //all new ones will be "new" until actually ordered after the confirm email
            requestItemViewModel.Request.RequestStatusID = 1;

            //in case we need to redirect to action
            //TempData["ModalView"] = true;
            //todo why is this here?
            //todo terms and installements and paid
            var context = new ValidationContext(requestItemViewModel.Request, null, null);
            var results = new List<ValidationResult>();
            var validatorCreate = Validator.TryValidateObject(requestItemViewModel.Request, context, results, true);
            if (validatorCreate)
            {
                /*
                 * the viewmodel loads the request.product with a primary key of 0
                 * so if you don't insert the request.productid into the request.product.productid
                 * it will create a new one instead of updating the existing one
                 * only need this if using an existing product
                 */
                //CREATE MODAL - may need to take this out? shouldn't it always create a new product??
                //requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;

                bool UpdateContextHere = false;

                var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                HttpContext.Session.SetObject(requestNum, requestItemViewModel.Request);
                //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.RequestList.ToString(), new List<Request>() { requestItemViewModel.Request });


                //if it is out of the budget get sent to get approved automatically and user is not in role admin !User.IsInRole("Admin")
                if (/*!User.IsInRole("Admin") &&*/ (OrderType.Equals(AppUtility.OrderTypeEnum.AskForPermission) || !checkIfInBudget(requestItemViewModel.Request)))
                {
                    UpdateContextHere = true;
                    requestItemViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == requestItemViewModel.Request.SubProjectID).FirstOrDefault(); //Why do we need this here?
                    try
                    {
                        requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                        //update session parentquote
                        //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.Request_ParentQuote.ToString(), requestItemViewModel.Request.ParentQuote);
                        requestItemViewModel.Request.RequestStatusID = 1; //new request
                        //update request
                        //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.Request.ToString(), requestItemViewModel.Request);
                        _context.Update(requestItemViewModel.Request);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorHeader += "/n Unable to assign data";
                        ViewBag.ErrorText += "/n Error assigning the following fields: subproject, quote status, request status";
                    }
                }
                else
                {
                    requestItemViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == requestItemViewModel.Request.SubProjectID).FirstOrDefault(); //Why do we need this here?

                    switch (OrderType)
                    {
                        case AppUtility.OrderTypeEnum.AddToCart:
                            try
                            {
                                requestItemViewModel.Request.RequestStatusID = 6; //approved
                                requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                                UpdateContextHere = true;
                                //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.Request.ToString(), requestItemViewModel.Request);
                                //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.Request_ParentQuote.ToString(), requestItemViewModel.Request.ParentQuote);
                                _context.Update(requestItemViewModel.Request);
                                _context.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                TempData["ErrorMessage"] = ex.Message;
                                TempData["InnerMessage"] = ex.InnerException;
                                return View("~/Views/Shared/RequestError.cshtml");
                            }
                            break;
                        //case AppUtility.OrderTypeEnum.WithoutOrder:

                        /*
                         * There is NO without order right now
                         * IF this is put back in -- IMPT: Must change over to sessions instead of using the context
                        */


                        //    requestItemViewModel.Request.ParentRequest = new ParentRequest();
                        //    int lastParentRequestOrderNum = 0;
                        //    requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
                        //    if (_context.ParentRequests.Any())
                        //    {
                        //        lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber ?? 0;
                        //    }
                        //    requestItemViewModel.Request.ParentRequest.OrderNumber = lastParentRequestOrderNum + 1;
                        //    requestItemViewModel.Request.ParentRequest.OrderDate = DateTime.Now;
                        //    requestItemViewModel.Request.ParentRequest.WithoutOrder = true;
                        //    requestItemViewModel.Request.RequestStatusID = 2;
                        //    requestItemViewModel.RequestStatusID = 2;
                        //    requestItemViewModel.Request.ParentQuote = null;
                        //    _context.Update(requestItemViewModel.Request);
                        //    _context.SaveChanges();
                        //    RequestNotification requestNotification = new RequestNotification();
                        //    requestNotification.RequestID = requestItemViewModel.Request.RequestID;
                        //    requestNotification.IsRead = false;
                        //    requestNotification.RequestName = requestItemViewModel.Request.Product.ProductName;
                        //    requestNotification.ApplicationUserID = requestItemViewModel.Request.ApplicationUserCreatorID;
                        //    requestNotification.Description = "item ordered";
                        //    requestNotification.NotificationStatusID = 2;
                        //    requestNotification.TimeStamp = DateTime.Now;
                        //    requestNotification.Controller = "Requests";
                        //    requestNotification.Action = "NotificationsView";
                        //    requestNotification.OrderDate = DateTime.Now;
                        //    requestNotification.Vendor = requestItemViewModel.Request.Product.Vendor.VendorEnName;
                        //    _context.Update(requestNotification);
                        //    _context.SaveChanges();
                        //    break;
                        case AppUtility.OrderTypeEnum.OrderNow:
                            requestItemViewModel.Request.RequestStatusID = 1; //new request
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            requestItemViewModel.RequestStatusID = 1;
                            requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                            HttpContext.Session.SetObject(requestNum, requestItemViewModel.Request);
                            //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.RequestList.ToString(), new List<Request>() { requestItemViewModel.Request });


                            TempData["OpenTermsModal"] = "Single";
                            TempData["Email1"] = requestItemViewModel.EmailAddresses[0];
                            TempData["Email2"] = requestItemViewModel.EmailAddresses[1];
                            TempData["Email3"] = requestItemViewModel.EmailAddresses[2];
                            TempData["Email4"] = requestItemViewModel.EmailAddresses[3];
                            TempData["Email5"] = requestItemViewModel.EmailAddresses[4];
                            //TempData["OpenConfirmEmailModal"] = true; //now we want it to go to the terms instead
                            TempData["RequestID"] = requestItemViewModel.Request.RequestID;
                            break;
                        default:
                            requestItemViewModel.Request.RequestStatusID = 1; //needs approval
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                            HttpContext.Session.SetObject(requestNum, requestItemViewModel.Request);
                            //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.RequestList.ToString(), new List<Request>() { requestItemViewModel.Request });

                            //_context.Update(requestItemViewModel.Request);
                            //_context.SaveChanges();
                            break;
                    }
                    //if (OrderType.Equals(AppUtility.OrderTypeEnum.AddToCart))
                    //{
                    //    try
                    //    {
                    //        requestItemViewModel.Request.RequestStatusID = 6; //approved
                    //        requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                    //        _context.Update(requestItemViewModel.Request);
                    //        _context.SaveChanges();
                    //    } 
                    //    catch (Exception ex)
                    //    {
                    //        TempData["ErrorMessage"] = ex.Message;
                    //        TempData["InnerMessage"] = ex.InnerException;
                    //        return View("~/Views/Shared/RequestError.cshtml");
                    //    }
                    //}
                    //else if (OrderType.Equals(AppUtility.OrderTypeEnum.WithoutOrder))
                    //{
                    //    requestItemViewModel.Request.ParentRequest = new ParentRequest();
                    //    int lastParentRequestOrderNum = 0;
                    //    requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
                    //    if (_context.ParentRequests.Any())
                    //    {
                    //        lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber ?? 0;
                    //    }
                    //    requestItemViewModel.Request.ParentRequest.OrderNumber = lastParentRequestOrderNum + 1;
                    //    requestItemViewModel.Request.ParentRequest.OrderDate = DateTime.Now;
                    //    requestItemViewModel.Request.ParentRequest.WithoutOrder = true;
                    //    requestItemViewModel.Request.RequestStatusID = 2;
                    //    requestItemViewModel.RequestStatusID = 2;
                    //    requestItemViewModel.Request.ParentQuote = null;
                    //    _context.Update(requestItemViewModel.Request);
                    //    _context.SaveChanges();
                    //    RequestNotification requestNotification = new RequestNotification();
                    //    requestNotification.RequestID = requestItemViewModel.Request.RequestID;
                    //    requestNotification.IsRead = false;
                    //    requestNotification.RequestName = requestItemViewModel.Request.Product.ProductName;
                    //    requestNotification.ApplicationUserID = requestItemViewModel.Request.ApplicationUserCreatorID;
                    //    requestNotification.Description = "item ordered";
                    //    requestNotification.NotificationStatusID = 2;
                    //    requestNotification.TimeStamp = DateTime.Now;
                    //    requestNotification.Controller = "Requests";
                    //    requestNotification.Action = "NotificationsView";
                    //    requestNotification.OrderDate = DateTime.Now;
                    //    requestNotification.Vendor = requestItemViewModel.Request.Product.Vendor.VendorEnName;
                    //    _context.Update(requestNotification);
                    //    _context.SaveChanges();
                    //}
                    //else if (OrderType.Equals(AppUtility.OrderTypeEnum.OrderNow))
                    //{
                    //    requestItemViewModel.Request.RequestStatusID = 1; //new request
                    //    requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                    //    requestItemViewModel.RequestStatusID = 1;
                    //    _context.Add(requestItemViewModel.Request);
                    //    _context.SaveChanges();

                    //    TempData["OpenTermsModal"] = "Single";
                    //    TempData["Email1"] = requestItemViewModel.EmailAddresses[0];
                    //    TempData["Email2"] = requestItemViewModel.EmailAddresses[1];
                    //    TempData["Email3"] = requestItemViewModel.EmailAddresses[2];
                    //    TempData["Email4"] = requestItemViewModel.EmailAddresses[3];
                    //    TempData["Email5"] = requestItemViewModel.EmailAddresses[4];
                    //    //TempData["OpenConfirmEmailModal"] = true; //now we want it to go to the terms instead
                    //    TempData["RequestID"] = requestItemViewModel.Request.RequestID;
                    //}
                    //else
                    //{
                    //    requestItemViewModel.Request.RequestStatusID = 1; //needs approvall
                    //    requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                    //    _context.Update(requestItemViewModel.Request);
                    //    _context.SaveChanges();
                    //}
                }
                try
                {
                    //var subprojectid = requestItemViewModel.Request.Product.SubProjectID;
                    //var subproject = requestItemViewModel.Request.Product.SubProject;

                    //WHY IS THIS HERE????
                    //requestItemViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == requestItemViewModel.Request.SubProjectID).FirstOrDefault(); //Why do we need this here?

                    try
                    {
                        if (requestItemViewModel.Comments != null)
                        {
                            var x = 1; //to name the comments in session
                            foreach (var comment in requestItemViewModel.Comments)
                            {
                                if (comment.CommentText.Length != 0)
                                {
                                    //save the new comment
                                    comment.ApplicationUserID = currentUser.Id;
                                  
                                    comment.RequestID = requestItemViewModel.Request.RequestID;
                                    if (UpdateContextHere)
                                    {
                                        _context.Add(comment);
                                    }
                                    else
                                    {
                                        var SessionCommentName = AppData.SessionExtensions.SessionNames.Comment.ToString() + x;
                                        HttpContext.Session.SetObject(SessionCommentName, comment);
                                    }
                                    //_context.Add(comment);
                                }

                                x++; //to name the comments in session
                            }
                        }
                        if (UpdateContextHere)
                        {
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        //do something here. comment didn't save
                    }

                    //rename temp folder to the request id ====> WILLL NOW BE DONE LATER
                    //string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    //string requestFolderFrom = Path.Combine(uploadFolder, "0");
                    //string requestFolderTo = Path.Combine(uploadFolder, requestItemViewModel.Request.RequestID.ToString());
                    //if (Directory.Exists(requestFolderTo)){
                    //    Directory.Delete(requestFolderTo);
                    //}
                    //Directory.Move(requestFolderFrom, requestFolderTo);


                    return RedirectToAction("Index", new
                    {
                        //page = requestItemViewModel.Page, //don't need this here b/c create is not a modal anymore
                        requestStatusID = requestItemViewModel.Request.RequestStatusID,
                        PageType = AppUtility.PageTypeEnum.RequestRequest
                    });
                }
                catch (DbUpdateException ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    if (ex.Message != null)
                    {
                        TempData["ErrorMessage"] = ex.Message.ToString();
                    }
                    if (ex.InnerException != null)
                    {
                        TempData["InnerMessage"] = ex.InnerException.ToString();
                    }
                    return View("~/Views/Shared/RequestError.cshtml");
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    if (ex.Message != null)
                    {
                        TempData["ErrorMessage"] = ex.Message.ToString();
                    }
                    if (ex.InnerException != null)
                    {
                        TempData["InnerMessage"] = ex.InnerException.ToString();
                    }
                    return View("~/Views/Shared/RequestError.cshtml");
                }
            }
            else
            {
                ViewBag.ErrorHeader += "/n Unable to create";
                ViewBag.ErrorText += "/n The request model failed to validate. Please ensure that all fields were filled in correctly /n";
                ViewBag.ErrorText += validatorCreate.ToString();
                return View();
                //TempData["InnerMessage"] = "The request model failed to validate. Please ensure that all fields were filled in correctly";
                //return View("~/Views/Shared/RequestError.cshtml");
            }
        }



        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DetailsSummaryModalView(int? id, bool NewRequestFromProduct = false)
        {
            //string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }

            var parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 1).ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 1).ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var subprojects = await _context.SubProjects.ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                //ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Projects = projects,
                SubProjects = subprojects,
                Vendors = vendors,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts,
                Comments = await _context.Comments
                    .Include(c => c.ApplicationUser)
                    .Where(c => c.Request.RequestID == id).ToListAsync(),
                CommentTypes = commentTypes
            };

            if (id == 0)
            {
                return RedirectToAction("CreateModalView");
            }

            else
            {

                requestItemViewModel.Request = _context.Requests.Include(r => r.Product)
                    .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .Include(r => r.RequestStatus)
                    .Include(r => r.ParentQuote)
                    .Include(r => r.ApplicationUserCreator)
                    //.Include(r => r.Payments) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .Include(r => r.SubProject)
                    .ThenInclude(sp => sp.Project)
                    .SingleOrDefault(x => x.RequestID == id);
                //for the history tab
                requestItemViewModel.RequestsByProduct = _context.Requests.Where(r => r.ProductID == requestItemViewModel.Request.ProductID && (r.RequestStatusID == 2 || r.RequestStatusID == 3))
                //     .Include(r => r.ParentRequest)
                    .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.ApplicationUserCreator) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                    .ToList();


                //may be able to do this together - combining the path for the orders folders
                string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                string uploadFolder2 = Path.Combine(uploadFolder1, requestItemViewModel.Request.RequestID.ToString());
                string uploadFolderInfo = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Info.ToString());
                string uploadFolderPictures = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Pictures.ToString());

                //the partial file name that we will search for (1- because we want the first one)
                //creating the directory from the path made earlier


                if (Directory.Exists(uploadFolderInfo))
                {
                    DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderInfo);
                    FileInfo[] infofilesfound = DirectoryToSearch.GetFiles("*.*");
                    requestItemViewModel.InfoFileStrings = new List<string>();
                    foreach (var infofile in infofilesfound)
                    {
                        string newFileString = AppUtility.GetLastFiles(infofile.FullName, 4);
                        requestItemViewModel.InfoFileStrings.Add(newFileString);
                    }
                }
                if (Directory.Exists(uploadFolderPictures))
                {
                    DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderPictures);
                    FileInfo[] picturefilesfound = DirectoryToSearch.GetFiles("*.*");
                    requestItemViewModel.PictureFileStrings = new List<string>();
                    foreach (var picturefile in picturefilesfound)
                    {
                        string newFileString = AppUtility.GetLastFiles(picturefile.FullName, 4);
                        requestItemViewModel.PictureFileStrings.Add(newFileString);
                    }
                }


                //GET PAYMENTS HERE
                //var payments = _context.Payments
                //    .Include(p => p.CompanyAccount).ThenInclude(ca => ca.PaymentType)
                //    .Where(p => p.RequestID == requestItemViewModel.Request.RequestID).ToList();
                //requestItemViewModel.NewPayments = payments;

                //if (payments.Count > 0)
                //{
                //    var amountPerPayment = requestItemViewModel.Request.Cost / payments.Count; //shekel
                //    var totalPaymentsToDate = 0;
                //    foreach (var payment in payments)
                //    {
                //        if (payment.PaymentDate <= DateTime.Now)
                //        {
                //            totalPaymentsToDate++;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //    requestItemViewModel.Debt = requestItemViewModel.Request.Cost - (totalPaymentsToDate * amountPerPayment);
                //}
                //else
                //{
                //    requestItemViewModel.Debt = 0;
                //}


                ////locations:
                ////get the list of requestLocationInstances in this request
                ////can't look for _context.RequestLocationInstances b/c it's a join table and doesn't have a dbset
                //var request1 = _context.Requests.Where(r => r.RequestID == id).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).FirstOrDefault();
                //var requestLocationInstances = request1.RequestLocationInstances.ToList();
                ////if it has => (which it should once its in a details view)
                //if (requestLocationInstances.Any())
                //{
                //    //get the parent location instances of the first one
                //    //can do this now b/c can only be in one box - later on will have to be a list or s/t b/c will have more boxes
                //    //int? locationInstanceParentID = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstanceID).FirstOrDefault().LocationInstanceParentID;
                //    requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                //    //requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                //    //need to test b/c the model is int? which is nullable
                //    if (requestItemViewModel.ParentLocationInstance != null)
                //    {
                //        //inserting list of childrenslocationinstances to show on the frontend
                //        requestItemViewModel.ChildrenLocationInstances = _context.LocationInstances
                //            .Where(li => li.LocationInstanceParentID == requestItemViewModel.ParentLocationInstance.LocationInstanceID)
                //            .Include(li => li.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();
                //        //var x = 0; //place in cli
                //        //requestItemViewModel.ChildrenLocationInstancesRequests = new List<Request>();
                //        //foreach (var cli in requestItemViewModel.ChildrenLocationInstances)
                //        //{
                //        //    var req = _context.Requests
                //        //        .Include(r => r.RequestLocationInstances.Select(rli => rli.LocationInstanceID == cli.LocationInstanceID)).Include(r => r.Product)
                //        //        .FirstOrDefault();
                //        //    if (req != null)
                //        //    {
                //        //        requestItemViewModel.ChildrenLocationInstancesRequests.Add(req);
                //        //    }
                //        //}

                //    }
                //}

                if (requestItemViewModel.Request == null)
                {
                    TempData["InnerMessage"] = "The request sent in was null";
                }
            }

            //ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", addNewItemViewModel.Request.ParentRequest.ApplicationUserID);
            //ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", addNewItemViewModel.Request.ProductID);
            //ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", addNewItemViewModel.Request.RequestStatusID);
            if (AppUtility.IsAjaxRequest(this.Request))
            {
                TempData["IsFull"] = false;
                return PartialView(requestItemViewModel);
            }
            else
            {
                TempData["IsFull"] = true;
                return View(requestItemViewModel);
            }

        }

        [Authorize(Roles = "Requests")]
        public ActionResult DownloadPDF(string filename)
        {
            //string filename = orderFileInfo.FullName.ToString();
            string concatShortFilename = "inline; filename=" +
                filename.Substring(filename.LastIndexOf("\\") + 2); //follow through with this
            Response.Headers.Add("Content-Disposition", concatShortFilename);
            return File(filename, "application/pdf");
        }


        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> EditModalView(int? id, bool NewRequestFromProduct = false, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, bool isEditable = true)
        {
            return await editModalViewFunction(id, 0, SectionType, isEditable);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> EditModalViewPartial(int? id, int? Tab, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, bool isEditable = true)
        {
            return await editModalViewFunction(id, Tab, SectionType, isEditable);
        }
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> editModalViewFunction(int? id, int? Tab = 0, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests,
            bool isEditable = true)
        {
            string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }
            var productId = _context.Requests.Where(r => r.RequestID == id).Select(r => r.ProductID).FirstOrDefault();
            var requestsByProduct = _context.Requests.Where(r => r.ProductID == productId && (r.RequestStatusID == 3))
                 .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.ApplicationUserCreator) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                    .ToList();
            var parentcategory =await _context.ParentCategories.Where(pc => pc.ParentCategoryID == _context.Requests.Where(r => r.RequestID == id).Select(r => r.Product.ProductSubcategory.ParentCategory.ParentCategoryID).FirstOrDefault()).FirstOrDefaultAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 1).ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 1).Count() > 0).ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategory = parentcategory,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                Projects = projects,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts,
                Tab = Tab ?? 0,
                Comments = await _context.Comments
                .Include(r => r.ApplicationUser)
                .Where(r => r.Request.RequestID == id).ToListAsync(),
                CommentTypes = commentTypes,
                SectionType = SectionType,
                RequestsByProduct = requestsByProduct
            };
            if (isEditable)
            {
                requestItemViewModel.ModalType = AppUtility.RequestModalType.Edit;
            }
            else
            {
                requestItemViewModel.ModalType = AppUtility.RequestModalType.Summary;
            }

            ModalViewType = "Edit";

            requestItemViewModel.Request = _context.Requests.Include(r => r.Product)
                .Include(r => r.ParentQuote)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product.ProductSubcategory)
                .Include(r => r.Product.ProductSubcategory.ParentCategory)
                     .Include(r => r.Product.Vendor)
                .Include(r => r.RequestStatus)
                .Include(r => r.ApplicationUserCreator)
                //.Include(r => r.Payments) //do we have to have a separate list of payments to include thefix c inside things (like company account and payment types?)
                .Include(r => r.SubProject)
                .Include(r => r.SubProject.Project)
                .SingleOrDefault(x => x.RequestID == id);

            //load the correct list of subprojects
            var subprojects = await _context.SubProjects
                .Where(sp => sp.ProjectID == requestItemViewModel.Request.SubProject.ProjectID)
                .ToListAsync();
            requestItemViewModel.SubProjects = subprojects;

            //may be able to do this together - combining the path for the orders folders
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string uploadFolder2 = Path.Combine(uploadFolder1, requestItemViewModel.Request.RequestID.ToString());
            string uploadFolderOrders = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Orders.ToString());
            string uploadFolderInvoices = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Invoices.ToString());
            string uploadFolderShipments = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Shipments.ToString());
            string uploadFolderQuotes = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Quotes.ToString());
            string uploadFolderInfo = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Info.ToString());
            string uploadFolderPictures = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Pictures.ToString());
            string uploadFolderReturns = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Returns.ToString());
            string uploadFolderCredits = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Credits.ToString());
            //the partial file name that we will search for (1- because we want the first one)
            //creating the directory from the path made earlier

            if (Directory.Exists(uploadFolderOrders))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderOrders);
                //searching for the partial file name in the directory
                FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                requestItemViewModel.OrderFileStrings = new List<String>();
                foreach (var orderfile in orderfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                    requestItemViewModel.OrderFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderInvoices))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderInvoices);
                FileInfo[] invoicefilesfound = DirectoryToSearch.GetFiles("*.*");
                requestItemViewModel.InvoiceFileStrings = new List<string>();
                foreach (var invoicefile in invoicefilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(invoicefile.FullName, 4);
                    requestItemViewModel.InvoiceFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderShipments))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderShipments);
                FileInfo[] shipmentfilesfound = DirectoryToSearch.GetFiles("*.*");
                requestItemViewModel.ShipmentFileStrings = new List<string>();
                foreach (var shipmentfile in shipmentfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(shipmentfile.FullName, 4);
                    requestItemViewModel.ShipmentFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderQuotes))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderQuotes);
                FileInfo[] quotefilesfound = DirectoryToSearch.GetFiles("*.*");
                requestItemViewModel.QuoteFileStrings = new List<string>();
                foreach (var quotefile in quotefilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(quotefile.FullName, 4);
                    requestItemViewModel.QuoteFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderInfo))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderInfo);
                FileInfo[] infofilesfound = DirectoryToSearch.GetFiles("*.*");
                requestItemViewModel.InfoFileStrings = new List<string>();
                foreach (var infofile in infofilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(infofile.FullName, 4);
                    requestItemViewModel.InfoFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderPictures))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderPictures);
                FileInfo[] picturefilesfound = DirectoryToSearch.GetFiles("*.*");
                requestItemViewModel.PictureFileStrings = new List<string>();
                foreach (var picturefile in picturefilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(picturefile.FullName, 4);
                    requestItemViewModel.PictureFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderReturns))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderReturns);
                FileInfo[] returnfilesfound = DirectoryToSearch.GetFiles("*.*");
                requestItemViewModel.ReturnFileStrings = new List<string>();
                foreach (var returnfile in returnfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(returnfile.FullName, 4);
                    requestItemViewModel.ReturnFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderCredits))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderCredits);
                FileInfo[] creditfilesfound = DirectoryToSearch.GetFiles("*.*");
                requestItemViewModel.CreditFileStrings = new List<string>();
                foreach (var creditfile in creditfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(creditfile.FullName, 4);
                    requestItemViewModel.CreditFileStrings.Add(newFileString);
                }
            }

            //first get the list of payment types there are
            var paymentTypeIds = _context.CompanyAccounts.Select(ca => ca.PaymentTypeID).Distinct().ToList();
            //initialize the dictionary
            requestItemViewModel.CompanyAccountListsByPaymentTypeID = new Dictionary<int, IEnumerable<CompanyAccount>>();
            //foreach paymenttype
            foreach (var paymentTypeID in paymentTypeIds)
            {
                var caList = _context.CompanyAccounts.Where(ca => ca.PaymentTypeID == paymentTypeID);
                requestItemViewModel.CompanyAccountListsByPaymentTypeID.Add(paymentTypeID, caList);
            }



            //locations:
            //get the list of requestLocationInstances in this request
            //can't look for _context.RequestLocationInstances b/c it's a join table and doesn't have a dbset
            var request1 = _context.Requests.Where(r => r.RequestID == id).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).FirstOrDefault();
            var requestLocationInstances = request1.RequestLocationInstances.ToList();
            //if it has => (which it should once its in a details view)
            requestItemViewModel.LocationInstances = new List<LocationInstance>();
            requestLocationInstances.ForEach(rli => requestItemViewModel.LocationInstances.Add(rli.LocationInstance));
            if (request1.RequestStatusID == 3 || request1.RequestStatusID == 5 || request1.RequestStatusID == 4)
            {
                ReceivedLocationViewModel receivedLocationViewModel = new ReceivedLocationViewModel()
                {
                    Request = _context.Requests.Where(r => r.RequestID == request1.RequestID).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                   .FirstOrDefault(),
                    locationTypesDepthZero = _context.LocationTypes.Where(lt => lt.Depth == 0),
                    locationInstancesSelected = new List<LocationInstance>(),
                };


                if (requestLocationInstances.Any())
                {
                    //get the parent location instances of the first one
                    //can do this now b/c can only be in one box - later on will have to be a list or s/t b/c will have more boxes
                    //int? locationInstanceParentID = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstanceID).FirstOrDefault().LocationInstanceParentID;
                    LocationInstance parentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).Include(li => li.LocationType).FirstOrDefault();
                    //requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                    //need to test b/c the model is int? which is nullable
                    receivedLocationViewModel.locationInstancesSelected.Add(parentLocationInstance);
                    var locationType = parentLocationInstance.LocationType;
                    while (locationType.Depth != 0)
                    {
                        locationType = _context.LocationTypes.Where(l => l.LocationTypeID == locationType.LocationTypeParentID).FirstOrDefault();
                    }
                    requestItemViewModel.ParentDepthZeroOfSelected = locationType;
                    requestItemViewModel.ReceivedLocationViewModel = receivedLocationViewModel;

                    ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel = new ReceivedModalSublocationsViewModel()
                    {
                        locationInstancesDepthZero = _context.LocationInstances.Where(li => li.LocationTypeID == locationType.LocationTypeID),
                        locationTypeNames = new List<string>(),
                        locationInstancesSelected = new List<LocationInstance>()
                    };
                    bool finished = false;
                    int locationTypeIDLoop = locationType.LocationTypeID;
                    var parent = parentLocationInstance;
                    receivedModalSublocationsViewModel.locationInstancesSelected.Add(parent);
                    requestItemViewModel.ChildrenLocationInstances = new List<List<LocationInstance>>();
                    requestItemViewModel.ChildrenLocationInstances.Add(_context.LocationInstances.Where(l => l.LocationInstanceParentID == parent.LocationInstanceParentID).ToList());
                    while (parent.LocationInstanceParentID != null)
                    {
                        parent = _context.LocationInstances.Where(li => li.LocationInstanceID == parent.LocationInstanceParentID).FirstOrDefault();
                        requestItemViewModel.ChildrenLocationInstances.Add(_context.LocationInstances.Where(l => l.LocationInstanceParentID == parent.LocationInstanceParentID).ToList());
                        receivedModalSublocationsViewModel.locationInstancesSelected.Add(parent);
                    }
                    while (!finished)
                    {
                        //need to get the whole thing b/c need both the name and the child id so it's instead of looping through the list twice
                        var nextType = _context.LocationTypes.Where(lt => lt.LocationTypeID == locationTypeIDLoop).FirstOrDefault();
                        string nextTYpeName = nextType.LocationTypeName;
                        int? tryNewLocationType = nextType.LocationTypeChildID;
                        //add it to the list in the viewmodel
                        receivedModalSublocationsViewModel.locationTypeNames.Add(nextTYpeName);

                        //while we're still looping through we'll instantiate the locationInstancesSelected so we can have dropdownlistfors on the view
                        //receivedModalSublocationsViewModel.locationInstancesSelected.Add(new LocationInstance());

                        if (tryNewLocationType == null)
                        {
                            //if its not null we can convert it and pass it in
                            finished = true;
                        }
                        else
                        {
                            locationTypeIDLoop = (Int32)tryNewLocationType;
                        }
                    }
                    requestItemViewModel.ReceivedModalSublocationsViewModel = receivedModalSublocationsViewModel;
                    ReceivedModalVisualViewModel receivedModalVisualViewModel = new ReceivedModalVisualViewModel()
                    {
                        IsEditModalTable = true,
                        ParentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == parentLocationInstance.LocationInstanceID).FirstOrDefault()
                    };

                    if (receivedModalVisualViewModel.ParentLocationInstance != null)
                    {
                        receivedModalVisualViewModel.RequestChildrenLocationInstances =
                            _context.LocationInstances.Where(m => m.LocationInstanceParentID == parentLocationInstance.LocationInstanceID)
                            .Include(m => m.RequestLocationInstances)
                            .Select(li => new RequestChildrenLocationInstances()
                            {
                                LocationInstance = li,
                                IsThisRequest = li.RequestLocationInstances.Select(rli => rli.RequestID).Where(i => i == id).Any()
                            }).ToList();

                        //return NotFound();
                    }
                    requestItemViewModel.ReceivedModalVisualViewModel = receivedModalVisualViewModel;
                }


            }


            if (requestItemViewModel.Request == null)
            {
                TempData["InnerMessage"] = "The request sent in was null";
            }

            ViewData["ModalViewType"] = ModalViewType;
            //ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", addNewItemViewModel.Request.ParentRequest.ApplicationUserID);
            //ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", addNewItemViewModel.Request.ProductID);
            //ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", addNewItemViewModel.Request.RequestStatusID);

            return PartialView(requestItemViewModel);

        }

        //[Authorize(Roles = "Admin, OrdersAndInventory")]
        //public async Task<IActionResult> EditSummaryModalView(int? id, bool NewRequestFromProduct = false)
        //{

        //    //not imlemented yet
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> EditModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            TempData.Keep();
            //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
            requestItemViewModel.Request.ParentRequest = null;
            //requestItemViewModel.Request.ParentQuote.ParentQuoteID = (Int32)requestItemViewModel.Request.ParentQuoteID;
            var parentQuote = _context.ParentQuotes.Where(pq => pq.ParentQuoteID == requestItemViewModel.Request.ParentQuoteID).FirstOrDefault();
            if (parentQuote != null)
            {

                parentQuote.QuoteNumber = requestItemViewModel.Request.ParentQuote.QuoteNumber;
                parentQuote.QuoteDate = requestItemViewModel.Request.ParentQuote.QuoteDate;
                requestItemViewModel.Request.ParentQuote = parentQuote;
            }
            else
            {
                parentQuote = new ParentQuote();
                parentQuote.QuoteNumber = requestItemViewModel.Request.ParentQuote.QuoteNumber;
                parentQuote.QuoteDate = requestItemViewModel.Request.ParentQuote.QuoteDate;
                requestItemViewModel.Request.ParentQuote = parentQuote;
            }
            //else if(requestItemViewModel.Request.ParentQuote?.QuoteNumber !=null || requestItemViewModel.Request.ParentQuote?.QuoteDate != null)
            //{ 
            //    parentQuote= new ParentQuote();
            //    parentQuote.QuoteNumber = requestItemViewModel.Request.ParentQuote.QuoteNumber;
            //    parentQuote.QuoteDate = requestItemViewModel.Request.ParentQuote.QuoteDate;
            //    requestItemViewModel.Request.ParentQuote = parentQuote;

            //}

            var product = _context.Products.Include(p => p.Vendor).Include(p => p.ProductSubcategory).FirstOrDefault(v => v.ProductID == requestItemViewModel.Request.ProductID);
            product.ProductSubcategoryID = requestItemViewModel.Request.Product.ProductSubcategoryID;
            product.VendorID = requestItemViewModel.Request.Product.VendorID;
            //in case we need to return to the modal view
            product.ProductName = requestItemViewModel.Request.Product.ProductName;
            requestItemViewModel.ParentCategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 1).ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 1).ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");

            //declared outside the if b/c it's used farther down to (for parent request the new comment too)
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            //in case we need to redirect to action
            //TempData["ModalView"] = true;
            TempData["RequestID"] = requestItemViewModel.Request.RequestID;

            //todo figure out payments
            //if (requestItemViewModel.Request.Terms == -1)
            //{
            //    requestItemViewModel.Request.Payed = true;
            //}


            var context = new ValidationContext(requestItemViewModel.Request, null, null);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(requestItemViewModel.Request, context, results, true))
            {
                /*
                 * the viewmodel loads the request.product with a primary key of 0
                 * so if you don't insert the request.productid into the request.product.productid
                 * it will create a new one instead of updating the existing one
                 * only need this if using an existing product
                 */
                requestItemViewModel.Request.Product = product;
                // requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;
                requestItemViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == requestItemViewModel.Request.SubProjectID).FirstOrDefault();
                try
                {
                    //_context.Update(requestItemViewModel.Request.Product.SubProject);
                    //_context.Update(requestItemViewModel.Request.Product);
                    _context.Update(requestItemViewModel.Request.ParentQuote);
                    await _context.SaveChangesAsync();
                    requestItemViewModel.Request.ParentQuoteID = requestItemViewModel.Request.ParentQuote.ParentQuoteID;
                    _context.Update(requestItemViewModel.Request);
                    await _context.SaveChangesAsync();


                    try
                    {
                        if (requestItemViewModel.Comments != null)
                        {

                            foreach (var comment in requestItemViewModel.Comments)
                            {
                                if (!String.IsNullOrEmpty(comment.CommentText))
                                {
                                    //save the new comment
                                    comment.ApplicationUserID = currentUser.Id;
                                    comment.CommentTimeStamp = DateTime.Now; 
                                    comment.RequestID = requestItemViewModel.Request.RequestID;
                                    _context.Update(comment);
                                }
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        //Tell the user that the comment didn't save here
                    }




                    ////Saving the Payments - each one should come in with a 1) date 2) companyAccountID
                    //if (requestItemViewModel.NewPayments != null)
                    //{
                    //    foreach (Payment payment in requestItemViewModel.NewPayments)
                    //    {
                    //        payment.RequestID = (Int32)requestItemViewModel.Request.RequestID;
                    //        payment.CompanyAccount = null;
                    //        //payment.Reference = "TEST";
                    //        try
                    //        {
                    //            _context.Payments.Update(payment);
                    //            await _context.SaveChangesAsync();
                    //        }
                    //        catch (Exception ex)
                    //        {

                    //        }
                    //    }
                    //}
                }
                catch (DbUpdateException ex)
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    TempData["InnerMessage"] = ex.InnerException.ToString();
                    return View("~/Views/Shared/RequestError.cshtml");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    TempData["InnerMessage"] = ex.InnerException.ToString();
                    return View("~/Views/Shared/RequestError.cshtml");
                }
            }
            else
            {
                foreach (var result in results) Debug.WriteLine(result.ErrorMessage);
                return View("~/Views/Shared/RequestError.cshtml");
            }
            //return RedirectToAction("Index");
            AppUtility.PageTypeEnum requestPageTypeEnum = (AppUtility.PageTypeEnum)requestItemViewModel.PageType;
            return RedirectToAction("Index", new
            {
                requestStatusID = requestItemViewModel.RequestStatusID,
                subcategoryID = requestItemViewModel.SubCategoryID,
                vendorID = requestItemViewModel.VendorID,
                applicationUserID = requestItemViewModel.ApplicationUserID,
                PageType = requestPageTypeEnum
            });
        }

        //not implemented yet
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin, OrdersAndInventory")]
        //public async Task<IActionResult> EditSummaryModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        //{

        //    {
        //        page = requestItemViewModel.Page,
        //        requestStatusID = requestItemViewModel.RequestStatusID,
        //        subcategoryID = requestItemViewModel.SubCategoryID,
        //        vendorID = requestItemViewModel.VendorID,
        //        applicationUserID = requestItemViewModel.ApplicationUserID,
        //        PageType = requestPageTypeEnum
        //    });
        //}

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(RequestIndexObject requestIndexObject, int? id, bool NewRequestFromProduct = false, String SectionType = "")
        {
            var parentcategories = await _context.ParentCategories.ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var subprojects = await _context.SubProjects.ToListAsync();
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;

            //to the best of my knowledge we do not need a list of request status so I commented it out
            //var requeststatuses = await _context.RequestStatuses.ToListAsync();

            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            Request request = _context.Requests
                .Include(r => r.Product)
                .Include(r => r.UnitType)
                .Include(r => r.SubUnitType)
                .Include(r => r.SubSubUnitType)
                .SingleOrDefault(x => x.RequestID == id);

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                Projects = projects,
                SubProjects = subprojects,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts,
                Request = request,
            };
            var reorderViewModel = new ReorderViewModel() { RequestIndexObject = requestIndexObject, RequestItemViewModel = requestItemViewModel };
            //initiating the  following models so that we can use them in an asp-for in the view 
            return PartialView(reorderViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(ReorderViewModel reorderViewModel)
        {
          //  ReorderViewModel reorderViewModel = JsonConvert.DeserializeObject<ReorderViewModel>(json);
            //get the old request that we are reordering
            var oldRequest = _context.Requests.Where(r => r.RequestID == reorderViewModel.RequestItemViewModel.Request.RequestID)
                .Include(r => r.Product)
                .ThenInclude(p => p.ProductSubcategory).FirstOrDefault();

            //get current user
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            //copy over request to new request with new id
            Reorder reorderRequest = new Reorder();
            reorderRequest.ProductID = oldRequest.ProductID;
            reorderRequest.ApplicationUserCreatorID = currentUser.Id;
            reorderRequest.CreationDate = DateTime.Now;
            reorderRequest.SubProjectID = oldRequest.SubProjectID;
            reorderRequest.SerialNumber = oldRequest.SerialNumber;
            reorderRequest.URL = oldRequest.URL;
            reorderRequest.Warranty = oldRequest.Warranty;
            reorderRequest.ExchangeRate = oldRequest.ExchangeRate;
            reorderRequest.Terms = oldRequest.Terms;
            reorderRequest.Cost = reorderViewModel.RequestItemViewModel.Request.Cost;
            reorderRequest.Currency = oldRequest.Currency;
            reorderRequest.CatalogNumber = oldRequest.CatalogNumber;
            reorderRequest.RequestStatusID = 1; //waiting approval status of new
            reorderRequest.UnitTypeID = reorderViewModel.RequestItemViewModel.Request.UnitTypeID;
            reorderRequest.Unit = reorderViewModel.RequestItemViewModel.Request.Unit;
            reorderRequest.SubSubUnit = reorderViewModel.RequestItemViewModel.Request.SubSubUnit;
            reorderRequest.SubUnit = reorderViewModel.RequestItemViewModel.Request.SubUnit;
            reorderRequest.SubUnitTypeID = reorderViewModel.RequestItemViewModel.Request.SubUnitTypeID;
            reorderRequest.SubSubUnitTypeID = reorderViewModel.RequestItemViewModel.Request.SubSubUnitTypeID;
            reorderRequest.UnitsOrdered = oldRequest.UnitsOrdered;
            reorderRequest.UnitsInStock = oldRequest.UnitsInStock;
            reorderRequest.Quantity = oldRequest.Quantity;
            reorderRequest.ParentQuote = new ParentQuote();
            reorderRequest.ParentQuote.QuoteStatusID = -1;


            var context = new ValidationContext(reorderRequest, null, null);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(reorderRequest, context, results, true))
            {
                try
                {
                    _context.Add(reorderRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.InnerException.ToString();

                    await populateRequestItemViewModel(reorderViewModel.RequestItemViewModel, oldRequest);
                    return PartialView(reorderViewModel.RequestItemViewModel);
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.InnerException.ToString();

                    await populateRequestItemViewModel(reorderViewModel.RequestItemViewModel, oldRequest);
                    return PartialView(reorderViewModel.RequestItemViewModel);
                }
            }
            else
            {
                //in case we need to redirect to action
                //TempData["ModalView"] = true;
                TempData["RequestID"] = reorderViewModel.RequestItemViewModel.Request.RequestID;

                await populateRequestItemViewModel(reorderViewModel.RequestItemViewModel, oldRequest);
                return PartialView(reorderViewModel.RequestItemViewModel);
            }
            return RedirectToAction("_IndexTableWithCounts", reorderViewModel.RequestIndexObject);
        }

        [Authorize(Roles = "Requests")]
        private async Task<bool> populateRequestItemViewModel(RequestItemViewModel requestItemViewModel, Request oldRequest)
        {
            //in case of error we need to populate these fields
            requestItemViewModel.Request.Product = oldRequest.Product;
            requestItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            requestItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");
            return true;
        }
        /*
         * END MODAL VIEW COPY
         */

        /*
         * BEGIN SEND EMAIL
         */
        //this could be used as a static function - for now we only need to convert the purchase order html into a pdf so it is located locally

        [Authorize(Roles = "Requests")]
        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> TermsModal(int id, bool isSingleRequest = false, bool IsCart = false) //either it'll be a request or parentrequest and then it'll send it to all the requests in that parent request
        {

            var usingSession = false;
            //List<Request> Requests = new List<Request>();
            //ParentQuote Request_ParentQuote = null;
            //Product Request_Product = null;
            var firstRequest = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
            if (isSingleRequest && HttpContext.Session.GetObject<Request>(firstRequest) != null)
            {
                usingSession = true;
            }
            bool IsOperations = false;
            List<Request> requests = null;
            if (usingSession)
            {
                var isRequests = true;
                var RequestNum = 1;
                while (isRequests)
                {
                    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                    if (HttpContext.Session.GetObject<Request>(requestName) != null)
                    {
                        requests.Add(HttpContext.Session.GetObject<Request>(requestName));
                    }
                    else
                    {
                        isRequests = false;
                    }
                    RequestNum++;
                }
                //requests = HttpContext.Session.GetObject<List<Request>>(AppData.SessionExtensions.SessionNames.RequestList.ToString());
            }
            else if (IsCart)
            {
                //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.RequestList.ToString(), new List<Request>() { requestItemViewModel.Request });
                requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                         .Where(r => r.Product.VendorID == id && r.RequestStatusID == 6 && !(r is Reorder))
                         .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
                               .Include(r => r.Product).ThenInclude(r => r.Vendor)
                               .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();

                //foreach (Request req in Requests)
                //{
                //    req.ParentRequestID = termsViewModel.ParentRequest.ParentRequestID;
                //    _context.Update(req);
                //    await _context.SaveChangesAsync();
                //}
            }


            int lastParentRequestOrderNum = 0;
            var prs = _context.ParentRequests;
            if (_context.ParentRequests.Any())
            {
                lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber ?? 0;
            }
            ParentRequest pr = new ParentRequest()
            {
                ApplicationUserID = _userManager.GetUserId(User),
                OrderNumber = lastParentRequestOrderNum + 1,
                OrderDate = DateTime.Now
            };
            //_context.Add(pr);
            //await _context.SaveChangesAsync();
            //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.Request_ParentRequest.ToString(), pr);

            var requestNum = 1;
            foreach (var req in requests)
            {
                req.ParentRequest = pr;
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + requestNum;
                HttpContext.Session.SetObject(requestName, req);
                requestNum++;
            }
            //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.RequestList.ToString(), requests);

            TermsViewModel termsViewModel = new TermsViewModel()
            {
                ParentRequest = pr,
                TermsList = new List<SelectListItem>()
                {
                    new SelectListItem{ Text="Pay Now", Value="0"},
                    new SelectListItem{ Text="+15", Value="15"},
                    new SelectListItem{ Text="+30", Value="30"},
                    new SelectListItem{ Text="+45", Value="45"}
                }
            };

            IsOperations = requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2;


            //if (isSingleRequest)
            //{
            //    var request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.Product.ProductSubcategory.ParentCategory).FirstOrDefault();
            //    IsOperations = request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2;
            //    request.ParentRequestID = termsViewModel.ParentRequest.ParentRequestID;
            //    _context.Update(request);
            //    await _context.SaveChangesAsync();
            //}
            //else 
            //if (IsCart)
            //{

            //}
            termsViewModel.SectionType = IsOperations ? AppUtility.MenuItems.Operations : AppUtility.MenuItems.Requests;
            TempData.Keep();
            return PartialView(termsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> TermsModal(TermsViewModel termsViewModel)
        {
            //_context.Update(termsViewModel.ParentRequest);
            //await _context.SaveChangesAsync();var isRequests = true;
            var requests = new List<Request>();
            var isRequests = true;
            var RequestNum = 1;
            while (isRequests)
            {
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                if (HttpContext.Session.GetObject<Request>(requestName) != null)
                {
                    requests.Add(HttpContext.Session.GetObject<Request>(requestName));
                }
                else
                {
                    isRequests = false;
                }
                RequestNum++;
            }
            //var requests = HttpContext.Session.GetObject<List<Request>>(AppData.SessionExtensions.SessionNames.RequestList.ToString());


            //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.Request_ParentRequest.ToString(), termsViewModel.ParentRequest);

            //IEnumerable<Request> requests = null;
            //if (termsViewModel.ParentRequest.ParentRequestID != 0)
            //{
            //    //the requests are from the cart and already have a parent request
            //    requests = _context.Requests.Where(r => r.ParentRequestID == termsViewModel.ParentRequest.ParentRequestID);
            //}

            /*
             * If INSTALLMENTS are put back in will need this
             */

            //if (termsViewModel.NewPayments != null)
            //{
            //    var np = 1;
            //    var sessionPaymentNum = AppData.SessionExtensions.SessionNames.Payment.ToString() + np;
            //    foreach (var payment in termsViewModel.NewPayments)
            //    {
            //        payment.CompanyAccount = _context.CompanyAccounts.Where(ca => ca.CompanyAccountID == payment.CompanyAccountID).FirstOrDefault();
            //        //payment.ParentRequestID = termsViewModel.ParentRequest.ParentRequestID;
            //        //_context.Add(payment);
            //        HttpContext.Session.SetObject(sessionPaymentNum,payment)
            //    }
            //    //await _context.SaveChangesAsync();
            //};

            var paymentStatusID = 2;
            //Request sessionRequest = HttpContext.Session.GetObject<Request>(AppData.SessionExtensions.SessionNames.Request.ToString());
            //if (requests != null)
            //    //need to assign a list<request> in session for carts 
            //{
            //    foreach (var request in requests)
            //    {
            if (termsViewModel.Paid)
            {
                paymentStatusID = 6;
            }
            else if (termsViewModel.Terms == "0")
            {
                paymentStatusID = 3;
            }
            else if (termsViewModel.Terms == "15" || termsViewModel.Terms == "30" || termsViewModel.Terms == "45")
            {
                paymentStatusID = 4;
            }
            else if (termsViewModel.Installments > 0) //again : should we check if it needs more than 1?
            {
                paymentStatusID = 5;
                //the payments don't go here otherwise it would add for every request (needs to be added just once for the parent request)
            }
            //else
            //{
            //    request.PaymentStatusID = 2;
            //}
            //        _context.Update(request);
            //    }
            //    await _context.SaveChangesAsync();
            //}

            var requestNum = 1;
            foreach (var req in requests)
            {
                req.ParentRequest = termsViewModel.ParentRequest;
                req.PaymentStatusID = paymentStatusID;
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                HttpContext.Session.SetObject(requestName, req);
                RequestNum++;
            }

            //HttpContext.Session.SetObject(AppData.SessionExtensions.SessionNames.RequestList.ToString(), requests);
            //HttpContext.Session.SetInt32(AppData.SessionExtensions.SessionNames.Request_PaymentStatusID.ToString(), paymentStatusID);

            TempData["ParentRequestConfirmEmail"] = true;
            TempData["ParentRequestID"] = termsViewModel.ParentRequest.ParentRequestID;
            TempData.Keep();
            TempData["OpenTermsModal"] = null;
            return RedirectToAction("Index"); //todo: put in tempdata memory here
            //return RedirectToAction("ConfirmEmailModal", new { id = termsViewModel.ParentRequest.ParentRequestID });
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmEmailModal(int id)
        {
            var allRequests = new List<Request>();
            var isRequests = true;
            var RequestNum = 1;
            while (isRequests)
            {
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                if (HttpContext.Session.GetObject<Request>(requestName) != null)
                {
                    allRequests.Add(HttpContext.Session.GetObject<Request>(requestName));
                }
                else
                {
                    isRequests = false;
                }
                RequestNum++;
            }
            //var allRequests = HttpContext.Session.GetObject<List<Request>>(AppData.SessionExtensions.SessionNames.RequestList.ToString());
            var parentRequest = allRequests.FirstOrDefault().ParentRequest;
            //.Include(pr => pr.Requests).ThenInclude(r => r.Product).ThenInclude(p => p.Vendor)
            //    .Include(pr => pr.Requests).ThenInclude(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory);

            //var parentRequest = HttpContext.Session.GetObject<ParentRequest>(AppData.SessionExtensions.SessionNames.Request_ParentRequest.ToString());
            //List<Request> parentRequestRequests = new List<Request>();
            //parentRequestRequests.Add(HttpContext.Session.GetObject<Request>(AppData.SessionExtensions.SessionNames.Request.ToString()));
            //parentRequestRequests.FirstOrDefault().Product = HttpContext.Session.GetObject<Product>(AppData.SessionExtensions.SessionNames.Request_Product.ToString());
            //parentRequestRequests.FirstOrDefault().Product.Vendor = _context.Vendors.Where(v => v.VendorID == parentRequestRequests.FirstOrDefault().Product.VendorID).FirstOrDefault();
            //parentRequestRequests.FirstOrDefault().Product.ProductSubcategory =
            //    _context.ProductSubcategories.Where(ps => ps.ProductSubcategoryID == parentRequestRequests.FirstOrDefault().Product.ProductSubcategoryID)
            //    .Include(ps => ps.ParentCategory)
            //    .FirstOrDefault();
            //get multiple requests???? in list????


            //var parentRequest = _context.ParentRequests.Where(pr => pr.ParentRequestID == id)
            //        .Include(pr => pr.Requests).ThenInclude(r => r.Product).ThenInclude(p => p.Vendor)
            //        .Include(pr => pr.Requests).ThenInclude(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
            //        .FirstOrDefault();
            ConfirmEmailViewModel confirm = new ConfirmEmailViewModel
            {
                ParentRequest = parentRequest,
                Requests = allRequests,
                VendorId = id,
                RequestID = id,
                //IsSingleOrder = isSingleOrder,
                //Cart = cart
                SectionType = parentRequest.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1 ? AppUtility.MenuItems.Requests : AppUtility.MenuItems.Operations
            };
            //base url needs to be declared - perhaps should be getting from js?
            //once deployed need to take base url and put in the parameter for converter.convertHtmlString
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value.ToString()}";

            //render the purchase order view into a string using a the confirmEmailViewModel
            string renderedView = await RenderPartialViewToString("PurchaseOrderView", confirm);
            //instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            PdfDocument doc = new PdfDocument();
            // create a new pdf document converting an url
            doc = converter.ConvertHtmlString(renderedView, baseUrl);

            //save this as confirmemailtempdoc
            string path1 = Path.Combine("wwwroot", "files");
            string fileName = Path.Combine(path1, "ConfirmEmailTempDoc.pdf");
            doc.Save(fileName);
            doc.Close();

            //foreach (var request in confirm.Requests)
            //{
            //    //creating the path for the file to be saved
            //    string path1 = Path.Combine("wwwroot", "files");
            //    string path2 = Path.Combine(path1, request.RequestID.ToString());
            //    //create file
            //    string folderPath = Path.Combine(path2, AppUtility.RequestFolderNamesEnum.Orders.ToString());
            //    Directory.CreateDirectory(folderPath);
            //    string uniqueFileName = "OrderPDF.pdf";
            //    string filePath = Path.Combine(folderPath, uniqueFileName);
            //    // save pdf document
            //    doc.Save(filePath);
            //}
            // close pdf document
            //doc.Close();
            TempData["ParentRequestConfirmEmail"] = null;

            return PartialView(confirm);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmEmailModal(ConfirmEmailViewModel confirmEmail)
        {
            //var firstRequest = _context.Requests.Where(r => r.ParentRequestID == confirmEmail.ParentRequest.ParentRequestID)
            //    .Include(r => r.Product).ThenInclude(p => p.Vendor)
            //    .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).FirstOrDefault();

            //string uploadFolder1 = Path.Combine("~", "files");
            string uploadFolder = Path.Combine("wwwroot", "files");
            string uploadFile = Path.Combine(uploadFolder, "ConfirmEmailTempDoc.pdf");
            //string uploadFolder3 = Path.Combine(uploadFolder2, "Orders");
            //string uploadFile = Path.Combine(uploadFolder3, "OrderPDF.pdf");

            var isRequests = true;
            var RequestNum = 1;
            var requests = new List<Request>();
            while (isRequests)
            {
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                if (HttpContext.Session.GetObject<Request>(requestName) != null)
                {
                    requests.Add(HttpContext.Session.GetObject<Request>(requestName));
                }
                else
                {
                    isRequests = false;
                }
                RequestNum++;
            }
            //var requests = HttpContext.Session.GetObject<List<Request>>(AppData.SessionExtensions.SessionNames.RequestList.ToString());

            //var parentrequest = HttpContext.Session.GetObject<ParentRequest>(AppData.SessionExtensions.SessionNames.Request_ParentRequest.ToString());
            //List<Request> requests = new List<Request>();
            ////this is only done for the first!!!
            //requests.Add(HttpContext.Session.GetObject<Request>(AppData.SessionExtensions.SessionNames.Request.ToString()));
            //requests.FirstOrDefault().Product = HttpContext.Session.GetObject<Product>(AppData.SessionExtensions.SessionNames.Request_Product.ToString());
            //requests.FirstOrDefault().Product.Vendor = _context.Vendors.Where(v => v.VendorID == requests.FirstOrDefault().Product.VendorID).FirstOrDefault();
            //requests.FirstOrDefault().Product.ProductSubcategory =
            //    _context.ProductSubcategories.Where(ps => ps.ProductSubcategoryID == requests.FirstOrDefault().Product.ProductSubcategoryID)
            //    .Include(ps => ps.ParentCategory)
            //    .FirstOrDefault();

            if (System.IO.File.Exists(uploadFile))
            {
                //instatiate mimemessage
                var message = new MimeMessage();

                //instantiate the body builder
                var builder = new BodyBuilder();


                var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
                //var users = _context.Users.ToList();
                //currentUser = _context.Users.Where(u => u.Id == "702fe06c-22e1-4be8-a515-ea89d6e5ee00").FirstOrDefault();
                string ownerEmail = currentUser.Email;
                string ownerUsername = currentUser.FirstName + " " + currentUser.LastName;
                string ownerPassword = currentUser.SecureAppPass;
                string vendorEmail = /*firstRequest.Product.Vendor.OrdersEmail;*/ TempData["Email1"].ToString() == "" ? requests.FirstOrDefault().Product.Vendor.OrdersEmail : TempData["Email1"].ToString();
                string vendorName = requests.FirstOrDefault().Product.Vendor.VendorEnName;

                //add a "From" Email
                message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

                // add a "To" Email
                message.To.Add(new MailboxAddress(vendorName, vendorEmail));
                if (TempData["Email2"].ToString() != "")
                {
                    message.Cc.Add(new MailboxAddress(TempData["Email2"]?.ToString() ?? ""));
                }
                if (TempData["Email3"].ToString() != "")
                {
                    message.Cc.Add(new MailboxAddress(TempData["Email3"]?.ToString() ?? ""));
                }
                if (TempData["Email4"].ToString() != "")
                {
                    message.Cc.Add(new MailboxAddress(TempData["Email4"]?.ToString() ?? ""));
                }
                if (TempData["Email5"].ToString() != "")
                {
                    message.Cc.Add(new MailboxAddress(TempData["Email5"]?.ToString() ?? ""));
                }
                //add CC's to email





                //subject
                message.Subject = "Order from Centarix to " + vendorName;

                //body
                builder.TextBody = @"Please see attached order" + "\n" + "Thank you";
                builder.Attachments.Add(uploadFile);

                message.Body = builder.ToMessageBody();

                bool wasSent = false;

                using (var client = new SmtpClient())
                {

                    client.Connect("smtp.gmail.com", 587, false);
                    //var SecureAppPass = _context.Users.Where(u => u.Id == confirmEmail.ParentRequest.ApplicationUserID).FirstOrDefault().SecureAppPass;
                    client.Authenticate(ownerEmail, ownerPassword);// ownerPassword);//

                    //"FakeUser@123"); // set up two step authentication and get app password

                    /*
                     * SAVE THE INFORMATION HERE
                     */
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            //var product = HttpContext.Session.GetObject<Product>(AppData.SessionExtensions.SessionNames.Request_Product.ToString());
                            //_context.Add(product);
                            //await _context.SaveChangesAsync();

                            //var pr = HttpContext.Session.GetObject<ParentRequest>(AppData.SessionExtensions.SessionNames.Request_ParentRequest.ToString());
                            //_context.Add(pr);
                            //await _context.SaveChangesAsync();

                            //var req = HttpContext.Session.GetObject<Request>(AppData.SessionExtensions.SessionNames.Request.ToString());
                            //req.ProductID = product.ProductID;
                            //req.ParentRequestID = pr.ParentRequestID;
                            //req.PaymentStatusID = HttpContext.Session.GetInt32(AppData.SessionExtensions.SessionNames.Request_PaymentStatusID.ToString());
                            isRequests = true;
                            RequestNum = 1;
                            var listRequests = new List<Request>();
                            while (isRequests)
                            {
                                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                                if (HttpContext.Session.GetObject<Request>(requestName) != null)
                                {
                                    var requestFromContext = HttpContext.Session.GetObject<Request>(requestName);
                                    _context.Add(requestFromContext);
                                }
                                else
                                {
                                    isRequests = false;
                                }
                                RequestNum++;
                            }
                            //var listRequests = HttpContext.Session.GetObject<List<Request>>(AppData.SessionExtensions.SessionNames.RequestList.ToString());
                            //foreach (var req in listRequests)
                            //{
                            //    _context.Add(req);
                            //}
                            await _context.SaveChangesAsync();

                            var commentExists = true;
                            var n = 1;
                            do
                            {
                                var commentNumber = AppData.SessionExtensions.SessionNames.Comment.ToString() + n;
                                var comment = HttpContext.Session.GetObject<Comment>(commentNumber);
                                if (comment != null)
                                //will only go in here if there are comments so will only work if it's there
                                //IMPT look how to clear the session information if it fails somewhere...
                                {
                                    comment.RequestID = listRequests.FirstOrDefault().RequestID;
                                    _context.Add(comment);
                                }
                                else
                                {
                                    commentExists = false;
                                }
                            } while (commentExists);
                            await _context.SaveChangesAsync();

                            //save the document
                            foreach (var request in listRequests)
                            {
                                string NewFolder = Path.Combine(uploadFolder, request.RequestID.ToString());
                                string folderPath = Path.Combine(NewFolder, AppUtility.RequestFolderNamesEnum.Orders.ToString());
                                Directory.CreateDirectory(folderPath); //make sure we don't need one above also??

                                string uniqueFileName = 1 + "OrderEmail.pdf";
                                string filePath = Path.Combine(folderPath, uniqueFileName);

                                System.IO.File.Copy(uploadFile, filePath); //make sure this works for each of them
                            }

                            try
                            {
                                client.Send(message);
                                wasSent = true;
                            }
                            catch (Exception ex)
                            {
                            }
                            client.Disconnect(true);

                            if (wasSent)
                            {
                                foreach (var request in requests /*_context.Requests.Where(r => r.ParentRequestID == confirmEmail.ParentRequest.ParentRequestID)
                            .Include(r => r.Product).ThenInclude(p => p.Vendor)*/)
                                {
                                    request.RequestStatusID = 2;
                                    _context.Update(request);
                                    RequestNotification requestNotification = new RequestNotification();
                                    requestNotification.RequestID = request.RequestID;
                                    requestNotification.IsRead = false;
                                    requestNotification.RequestName = request.Product.ProductName;
                                    requestNotification.ApplicationUserID = request.ApplicationUserCreatorID;
                                    requestNotification.Description = "item ordered";
                                    requestNotification.NotificationStatusID = 2;
                                    requestNotification.TimeStamp = DateTime.Now;
                                    requestNotification.Controller = "Requests";
                                    requestNotification.Action = "NotificationsView";
                                    requestNotification.OrderDate = DateTime.Now;
                                    requestNotification.Vendor = request.Product.Vendor.VendorEnName;
                                    _context.Update(requestNotification);

                                }
                                await _context.SaveChangesAsync();

                            }
                            HttpContext.Session.Clear(); //will clear the session for the future
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            //should we clear or keep the session and open a new createmodal? or go back to the terms??
                        }

                    }
                    /*
                     * END SAVE THE INFORMATION HERE
                     */

                }
                TempData.Keep();

                AppUtility.PageTypeEnum requestPageTypeEnum = (AppUtility.PageTypeEnum)confirmEmail.PageType;
                if (requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                {
                    TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
                    //return RedirectToAction("Index", new
                    //{
                    //    page = confirmEmail.Page,
                    //    requestStatusID = 2,
                    //    PageType = AppUtility.RequestPageTypeEnum.Request
                    //});

                    return RedirectToAction("Index"); //temp: todo: must add Tempdata
                }
                else if (requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
                {

                    return RedirectToAction("Index", "Operations");
                }
                else
                {
                    TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
                    //return RedirectToAction("Index", "Operations", new
                    //{
                    //    page = confirmEmail.Page,
                    //    requestStatusID = 2,
                    //    PageType = AppUtility.RequestPageTypeEnum.Request
                    //});

                    return RedirectToAction("Index"); //temp: todo: must add Tempdata
                }

            }

            else
            {
                return RedirectToAction("Error");
            }


        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> CancelEmailModal(ConfirmEmailViewModel confirmEmailViewModel)
        {
            //TempData["OpenTermsModal"] = "Single";

            return RedirectToAction("Index");
        }
        /*
         * END SEND EMAIL
         */


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmQuoteEmailModal(ConfirmQuoteEmailViewModel confirmQuoteEmail)
        {
            List<Reorder> requests;
            if (confirmQuoteEmail.IsResend)
            {
                requests = _context.Requests.OfType<Reorder>().Where(r => r.RequestID == confirmQuoteEmail.RequestID)
           .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote).ToList();
            }
            else
            {
                requests = _context.Requests.OfType<Reorder>().Where(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.ParentQuote.QuoteStatusID == 1)
                         .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote).ToList();
            }
            if (requests.Count() == 0)
            {
                requests = _context.Requests.OfType<Reorder>().Where(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.ParentQuote.QuoteStatusID == 2)
                         .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote).ToList();
            }
            string uploadFolder1 = Path.Combine("~", "files");
            string uploadFolder = Path.Combine("wwwroot", "files");
            string uploadFolder2 = Path.Combine(uploadFolder, requests.FirstOrDefault().RequestID.ToString());
            string uploadFolder3 = Path.Combine(uploadFolder2, "Quotes");
            string uploadFile = Path.Combine(uploadFolder3, "QuotePDF.pdf");

            if (System.IO.File.Exists(uploadFile))
            {
                //instatiate mimemessage
                var message = new MimeMessage();

                //instantiate the body builder
                var builder = new BodyBuilder();

                var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
                //   currentUser = _context.Users.Where(u => u.Id == "702fe06c-22e1-4be8-a515-ea89d6e5ee00").FirstOrDefault();
                string ownerEmail = currentUser.Email;
                string ownerUsername = currentUser.FirstName + " " + currentUser.LastName;
                string ownerPassword = currentUser.SecureAppPass;
                string vendorEmail = requests.FirstOrDefault().Product.Vendor.OrdersEmail;
                string vendorName = requests.FirstOrDefault().Product.Vendor.VendorEnName;

                //add a "From" Email
                message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

                // add a "To" Email
                message.To.Add(new MailboxAddress(vendorName, vendorEmail));

                //subject
                message.Subject = "Order from Centarix to " + vendorName;

                //body
                builder.TextBody = @"Please see attached order" + "\n" + "Thank you";
                builder.Attachments.Add(uploadFile);

                message.Body = builder.ToMessageBody();

                bool wasSent = false;

                using (var client = new SmtpClient())
                {

                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate(ownerEmail, ownerPassword);// ownerPassword);//

                    //"FakeUser@123"); // set up two step authentication and get app password
                    try
                    {
                        client.Send(message);
                        wasSent = true;
                    }
                    catch (Exception ex)
                    {
                    }

                    client.Disconnect(true);
                    if (wasSent)
                    {
                        foreach (var quote in requests)
                        {
                            quote.ParentQuote.QuoteStatusID = 2;
                            quote.ParentQuote.ApplicationUserID = currentUser.Id;
                            //_context.Update(quote.ParentQuote);
                            //_context.SaveChanges();
                            _context.Update(quote);
                            _context.SaveChanges();
                        }

                    }

                }
                return RedirectToAction("LabManageQuotes", new
                {
                    RequestsByVendor = _context.Requests.OfType<Reorder>().Where(r => r.ParentQuote.QuoteStatusID == 1 || r.ParentQuote.QuoteStatusID == 2)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                    .Include(r => r.ApplicationUserCreator).Include(r => r.ParentQuote)
                    .ToLookup(r => r.Product.Vendor)
                });
            }

            else
            {
                return RedirectToAction("Error");
            }


        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmQuoteEmailModal(int id, bool isResend = false)
        {
            List<Reorder> requests;
            if (isResend)
            {
                requests = _context.Requests.OfType<Reorder>().Where(r => r.RequestID == id)
               .Include(r => r.Product).ThenInclude(r => r.Vendor)
               .ToList();
            }
            else
            {
                requests = _context.Requests.OfType<Reorder>().Where(r => r.Product.VendorID == id && r.ParentQuote.QuoteStatusID == 1)
                         .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote).ToList();
            }
            if (requests.Count() == 0)
            {
                requests = _context.Requests.OfType<Reorder>().Where(r => r.Product.VendorID == id && r.ParentQuote.QuoteStatusID == 2)
                         .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote).ToList();
            }

            ConfirmQuoteEmailViewModel confirmEmail = new ConfirmQuoteEmailViewModel
            {
                Requests = requests,
                VendorId = id,
                RequestID = id

            };
            //base url needs to be declared - perhaps should be getting from js?
            //once deployed need to take base url and put in the parameter for converter.convertHtmlString
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value.ToString()}";

            //render the purchase order view into a string using a the confirmEmailViewModel
            string renderedView = await RenderPartialViewToString("PurchaseQuoteView", confirmEmail);
            //instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            PdfDocument doc = new PdfDocument();
            // create a new pdf document converting an url
            doc = converter.ConvertHtmlString(renderedView, baseUrl);

            foreach (var request in requests)
            {
                //creating the path for the file to be saved
                string path1 = Path.Combine("wwwroot", "files");
                string path2 = Path.Combine(path1, request.RequestID.ToString());
                //create file
                string folderPath = Path.Combine(path2, AppUtility.RequestFolderNamesEnum.Quotes.ToString());
                Directory.CreateDirectory(folderPath);
                string uniqueFileName = "QuotePDF.pdf";
                string filePath = Path.Combine(folderPath, uniqueFileName);
                // save pdf document
                doc.Save(filePath);
            }
            // close pdf document
            doc.Close();

            return PartialView(confirmEmail);
        }


        [HttpGet]

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmQuoteOrderEmailModal(int id)
        {
            var requests = _context.Requests.OfType<Reorder>().Where(r => r.Product.VendorID == id && r.ParentQuote.QuoteStatusID == 4 && r.RequestStatusID == 6)
                .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentRequest).Include(r => r.ParentQuote).ToList();
            ParentRequest parentRequest = new ParentRequest();
            parentRequest.OrderDate = DateTime.Now;
            parentRequest.ApplicationUserID = _userManager.GetUserId(User);
            int lastParentRequestOrderNum = 0;
            if (_context.ParentRequests.Any())
            {
                lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber.Value;
            }
            parentRequest.OrderNumber = lastParentRequestOrderNum;
            _context.Update(parentRequest);
            _context.SaveChanges();
            foreach (var request in requests)
            {
                request.ParentRequestID = parentRequest.ParentRequestID;
                _context.Update(request);
                _context.SaveChanges();
            }
            ConfirmQuoteOrderEmailViewModel confirmEmail = new ConfirmQuoteOrderEmailViewModel
            {
                Requests = requests,
                VendorId = id

            };
            //base url needs to be declared - perhaps should be getting from js?
            //once deployed need to take base url and put in the parameter for converter.convertHtmlString
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value.ToString()}";

            //render the purchase order view into a string using a the confirmEmailViewModel
            string renderedView = await RenderPartialViewToString("PurchaseQuoteOrderView", confirmEmail);
            //instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            PdfDocument doc = new PdfDocument();
            // create a new pdf document converting an url
            doc = converter.ConvertHtmlString(renderedView, baseUrl);

            foreach (var request in requests)
            {
                //creating the path for the file to be saved
                string path1 = Path.Combine("wwwroot", "files");
                string path2 = Path.Combine(path1, request.RequestID.ToString());
                //create file
                string folderPath = Path.Combine(path2, AppUtility.RequestFolderNamesEnum.Orders.ToString());
                Directory.CreateDirectory(folderPath);
                string uniqueFileName = "OrderPDF.pdf";
                string filePath = Path.Combine(folderPath, uniqueFileName);
                // save pdf document
                doc.Save(filePath);
            }
            // close pdf document
            doc.Close();

            return PartialView(confirmEmail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmQuoteOrderEmailModal(ConfirmQuoteOrderEmailViewModel confirmQuoteOrderEmail)
        {
            var requests = _context.Requests.OfType<Reorder>().Where(r => r.Product.VendorID == confirmQuoteOrderEmail.VendorId && r.ParentQuote.QuoteStatusID == 4 && r.RequestStatusID == 6)
                     .Include(r => r.ParentRequest).ThenInclude(r => r.ApplicationUser).Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote).ToList();
            string uploadFolder1 = Path.Combine("~", "files");
            string uploadFolder = Path.Combine("wwwroot", "files");
            string uploadFolder2 = Path.Combine(uploadFolder, requests.FirstOrDefault().RequestID.ToString());
            string uploadFolder3 = Path.Combine(uploadFolder2, "Orders");
            string uploadFile = Path.Combine(uploadFolder3, "OrderPDF.pdf");

            if (System.IO.File.Exists(uploadFile))
            {
                //instatiate mimemessage
                var message = new MimeMessage();

                //instantiate the body builder
                var builder = new BodyBuilder();


                string ownerEmail = requests.FirstOrDefault().ParentRequest.ApplicationUser.Email;
                string ownerUsername = requests.FirstOrDefault().ParentRequest.ApplicationUser.FirstName + " " + requests.FirstOrDefault().ParentRequest.ApplicationUser.LastName;
                string ownerPassword = requests.FirstOrDefault().ParentRequest.ApplicationUser.SecureAppPass;
                string vendorEmail = requests.FirstOrDefault().Product.Vendor.OrdersEmail;
                string vendorName = requests.FirstOrDefault().Product.Vendor.VendorEnName;

                //add a "From" Email
                message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

                // add a "To" Email
                message.To.Add(new MailboxAddress(vendorName, vendorEmail));

                //subject
                message.Subject = "Order from Centarix to " + vendorName;

                //body
                builder.TextBody = @"Please see attached order" + "\n" + "Thank you";
                builder.Attachments.Add(uploadFile);

                message.Body = builder.ToMessageBody();

                bool wasSent = false;

                using (var client = new SmtpClient())
                {

                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate(ownerEmail, ownerPassword);// ownerPassword);//

                    //"FakeUser@123"); // set up two step authentication and get app password
                    try
                    {
                        client.Send(message);
                        wasSent = true;
                    }
                    catch (Exception ex)
                    {
                    }

                    client.Disconnect(true);
                    if (wasSent)
                    {
                        var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
                        foreach (var quote in requests)
                        {
                            quote.RequestStatusID = 2;
                            quote.ParentRequest.OrderDate = DateTime.Now;
                            //_context.Update(quote.ParentQuote);
                            //_context.SaveChanges();
                            _context.Update(quote);
                            _context.SaveChanges();
                            RequestNotification requestNotification = new RequestNotification();
                            requestNotification.RequestID = quote.RequestID;
                            requestNotification.IsRead = false;
                            requestNotification.RequestName = quote.Product.ProductName;
                            requestNotification.ApplicationUserID = quote.ApplicationUserCreatorID;
                            requestNotification.Description = "item ordered";
                            requestNotification.NotificationStatusID = 2;
                            requestNotification.TimeStamp = DateTime.Now;
                            requestNotification.Controller = "Requests";
                            requestNotification.Action = "NotificationsView";
                            requestNotification.OrderDate = DateTime.Now;
                            requestNotification.Vendor = quote.Product.Vendor.VendorEnName;
                            _context.Update(requestNotification);
                            _context.SaveChanges();
                        }

                    }

                }
                return RedirectToAction("LabManageOrders", new
                {
                    RequestsByVendor = _context.Requests.OfType<Reorder>().Where(r => r.ParentQuote.QuoteStatusID == 4 && r.RequestStatusID == 6)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                    .Include(r => r.ApplicationUserCreator).Include(r => r.ParentQuote)
                    .ToLookup(r => r.Product.Vendor)
                });
            }

            else
            {
                return RedirectToAction("Error");
            }


        }



        /*LABMANAGEMENT*/
        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> LabManageQuotes()
        {
            LabManageQuotesViewModel labManageQuotesViewModel = new LabManageQuotesViewModel();
            var labmanagerequests = _context.Requests.OfType<Reorder>();
            labManageQuotesViewModel.RequestsByVendor = _context.Requests.OfType<Reorder>().Where(r => r.ParentQuote.QuoteStatusID == 1 || r.ParentQuote.QuoteStatusID == 2)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                .Include(r => r.ParentQuote).Include(r => r.ApplicationUserCreator)
                .ToLookup(r => r.Product.Vendor);
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementQuotes;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Quotes;
            //TempData["SideBarPageType"] = AppUtility.LabManagementSidebarEnum.Quotes;
            return View(labManageQuotesViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> LabManageOrders()
        {
            LabManageQuotesViewModel labManageQuotesViewModel = new LabManageQuotesViewModel();
            labManageQuotesViewModel.RequestsByVendor = _context.Requests.OfType<Reorder>().Where(r => r.ParentQuote.QuoteStatusID == 4 && r.RequestStatusID == 6)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.ApplicationUserCreator)
                .ToLookup(r => r.Product.Vendor);
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementQuotes;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Orders;
            //TempData["SideBarPageType"] = AppUtility.LabManagementSidebarEnum.Orders;
            return View(labManageQuotesViewModel);
        }

        /*
         * BEGIN SEARCH
         */
        [HttpGet]
        [Authorize(Roles = "Requests, LabManagement, Operations")]
        public async Task<IActionResult> Search(AppUtility.MenuItems SectionType)
        {
            int categoryID = 0;
            if (SectionType == AppUtility.MenuItems.Requests)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestSearch;
                categoryID = 2;
            }
            else if (SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSearch;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Search;
            }
            else if (SectionType == AppUtility.MenuItems.Operations)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsSearch;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Search;
                categoryID = 1;
            }


            RequestsSearchViewModel requestsSearchViewModel = new RequestsSearchViewModel
            {
                ParentCategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID != categoryID).ToListAsync(),
                ProductSubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID != categoryID).ToListAsync(),
                Projects = await _context.Projects.ToListAsync(),
                SubProjects = await _context.SubProjects.ToListAsync(),
                Vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID != categoryID).Count() > 0).ToListAsync(),
                Request = new Request(),
                Inventory = false,
                Ordered = false,
                ForApproval = false,
                SectionType = SectionType
                //check if we need this here
            };

            requestsSearchViewModel.Request.ParentRequest = new ParentRequest();

            return View(requestsSearchViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Requests, Operations")]
        public async Task<IActionResult> Search(RequestsSearchViewModel requestsSearchViewModel, int? page)
        {
            var categoryType = requestsSearchViewModel.SectionType == AppUtility.MenuItems.Operations ? 2 : 1;
            int RSRecieved = 0;
            int RSOrdered = 0;
            int RSNew = 0;
            IQueryable<Request> requestsSearched = _context.Requests.AsQueryable().Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType);

            //convert the bools into their corresponding IDs
            if (requestsSearchViewModel.Inventory)
            {
                RSRecieved = 3;
            }
            if (requestsSearchViewModel.Ordered)
            {
                RSOrdered = 2;
            }
            if (requestsSearchViewModel.ForApproval)
            {
                RSNew = 1;
            }
            if (requestsSearchViewModel.Inventory || requestsSearchViewModel.Ordered || requestsSearchViewModel.ForApproval) //if any of the checkboxes were selected then filter accordingly
            {
                requestsSearched = requestsSearched.Where(rs => rs.RequestStatusID == RSRecieved || rs.RequestStatusID == RSOrdered || rs.RequestStatusID == RSNew);
            }


            requestsSearchViewModel.Request.Product.ProductSubcategory = await _context.ProductSubcategories.Include(ps => ps.ParentCategory).Where(ps => ps.ProductSubcategoryID == requestsSearchViewModel.Request.Product.ProductSubcategoryID).FirstOrDefaultAsync();
            if (requestsSearchViewModel.Request.Product.ProductName != null)
            {
                requestsSearched = requestsSearched.Where(r => r.Product.ProductName.Contains(requestsSearchViewModel.Request.Product.ProductName));
            }
            if (requestsSearchViewModel.Request.Product?.ProductSubcategory?.ParentCategoryID != 0 && requestsSearchViewModel.Request.Product?.ProductSubcategory?.ParentCategoryID != null)
            {
                requestsSearched = requestsSearched.Where(r => r.Product.ProductSubcategory.ParentCategoryID == requestsSearchViewModel.Request.Product.ProductSubcategory.ParentCategoryID);
            }
            if (requestsSearchViewModel.Request.Product?.ProductSubcategoryID != 0 && requestsSearchViewModel.Request.Product?.ProductSubcategoryID != null)
            {
                requestsSearched = requestsSearched.Where(r => r.Product.ProductSubcategoryID == requestsSearchViewModel.Request.Product.ProductSubcategoryID);
            }
            //check for project
            //check for sub project
            if (requestsSearchViewModel.Request.Product?.VendorID != 0 && requestsSearchViewModel.Request.Product?.VendorID != null)
            {
                requestsSearched = requestsSearched.Where(r => r.Product.VendorID == requestsSearchViewModel.Request.Product.VendorID);
            }
            if (requestsSearchViewModel.Request.ParentRequest?.OrderNumber != null)
            {
                requestsSearched = requestsSearched.Where(r => r.ParentRequest.OrderNumber.ToString().Contains(requestsSearchViewModel.Request.ParentRequest.OrderNumber.ToString()));
            }
            if (requestsSearchViewModel.Request.ParentRequest.OrderDate != DateTime.MinValue) //should this be datetime.min?
            {
                requestsSearched = requestsSearched.Where(r => r.ParentRequest.OrderDate == requestsSearchViewModel.Request.ParentRequest.OrderDate);
            }
            if (requestsSearchViewModel.Request.ParentRequest.InvoiceNumber != null)
            {
                requestsSearched = requestsSearched.Where(r => r.ParentRequest.InvoiceNumber.Contains(requestsSearchViewModel.Request.ParentRequest.InvoiceNumber));
            }
            if (requestsSearchViewModel.Request.ParentRequest.InvoiceDate != DateTime.MinValue) //should this be datetime.min?
            {
                requestsSearched = requestsSearched.Where(r => r.ParentRequest.InvoiceDate == requestsSearchViewModel.Request.ParentRequest.InvoiceDate);
            }
            if (requestsSearchViewModel.Request.ExpectedSupplyDays != null)//should this be on the parent request
            {
                requestsSearched = requestsSearched.Where(r => r.ExpectedSupplyDays == requestsSearchViewModel.Request.ExpectedSupplyDays);
            }

            //not sure what the to date and the from date are on????

            bool IsRequest = true;
            bool IsInventory = false;
            bool IsAll = false;
            if (requestsSearchViewModel.Inventory)
            {
                IsRequest = false;
                IsInventory = true;
            }
            else
            {
                foreach (Request r in requestsSearched)
                {
                    if (r.RequestStatusID != 6)
                    {
                        break;
                    }
                }
                IsRequest = false;
                IsInventory = true;
            }

            //also need to get the list smaller to just request or inventory

            var PageType = AppUtility.PageTypeEnum.None;
            if (IsRequest)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestRequest;
            }
            else if (IsInventory)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestInventory;
            }
            else if (IsAll)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestRequest;
            }
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;

            //ViewData["ReturnRequests"] = requestsSearched;


            //Getting the page that is going to be seen (if no page was specified it will be one)
            var pageNumber = page ?? 1;
            var onePageOfProducts = Enumerable.Empty<Request>().ToPagedList();
            try
            {
                onePageOfProducts = await requestsSearched.Include(r => r.ParentRequest).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).ToPagedListAsync(pageNumber, 25);
                //onePageOfProducts;


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }

            TempData["Search"] = "True";
            if (requestsSearchViewModel.SectionType == AppUtility.MenuItems.Requests)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestRequest;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
                return RedirectToAction("Index", new { pagetype = TempData[AppUtility.TempDataTypes.PageType.ToString()], vendorID = requestsSearchViewModel.Request.Product.VendorID, subcategoryID = requestsSearchViewModel.Request.Product.ProductSubcategoryID, requestsSearchViewModel = onePageOfProducts });
            }
            else if (requestsSearchViewModel.SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSearch;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Search;
                return RedirectToAction("IndexForLabManage", "Vendors", onePageOfProducts);
            }
            else if (requestsSearchViewModel.SectionType == AppUtility.MenuItems.Operations)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsInventory;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
                return RedirectToAction("Index", "Operations", new { vendorID = requestsSearchViewModel.Request.Product.VendorID, subcategoryID = requestsSearchViewModel.Request.Product.ProductSubcategoryID, requestsSearchViewModel = onePageOfProducts });
            }

            return RedirectToAction("Index", new { pagetype = TempData[AppUtility.TempDataTypes.PageType.ToString()], vendorID = requestsSearchViewModel.Request.Product.VendorID, subcategoryID = requestsSearchViewModel.Request.Product.ProductSubcategoryID, requestsSearchViewModel = onePageOfProducts });

        }


        /*
         * END SEARCH
         */



        /*
         * START RECEIVED MODAL
         */

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReceivedModal(int RequestID, RequestIndexObject requestIndexObject)
        {
            //foreach(var li in _context.LocationInstances)
            //{
            //    li.IsFull = false;
            //    _context.Update(li);
            //}
            //_context.SaveChanges();
            var request = _context.Requests.Where(r => r.RequestID == RequestID).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                    .FirstOrDefault();

            ReceivedLocationViewModel receivedLocationViewModel = new ReceivedLocationViewModel()
            {
                Request = request,
                locationTypesDepthZero = _context.LocationTypes.Where(lt => lt.Depth == 0),
                locationInstancesSelected = new List<LocationInstance>(),
                ApplicationUsers = await _context.Users.Where(u => !u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null).ToListAsync(),
                RequestIndexObject = requestIndexObject,
                PageRequestStatusID = request.RequestStatusID ?? 0
            };
            receivedLocationViewModel.locationInstancesSelected.Add(new LocationInstance());
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            receivedLocationViewModel.Request.ApplicationUserReceiverID = currentUser.Id;
            receivedLocationViewModel.Request.ArrivalDate = DateTime.Today;
            receivedLocationViewModel.CategoryType = receivedLocationViewModel.Request.Product.ProductSubcategory.ParentCategory.CategoryTypeID;
            return PartialView(receivedLocationViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public IActionResult ReceivedModalSublocations(int LocationTypeID)
        {
            ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel = new ReceivedModalSublocationsViewModel()
            {
                locationInstancesDepthZero = _context.LocationInstances.Where(li => li.LocationTypeID == LocationTypeID),
                locationTypeNames = new List<string>(),
                locationInstancesSelected = new List<LocationInstance>()
            };
            bool finished = false;
            int locationTypeIDLoop = LocationTypeID;
            while (!finished)
            {
                //need to get the whole thing b/c need both the name and the child id so it's instead of looping through the list twice
                var nextType = _context.LocationTypes.Where(lt => lt.LocationTypeID == locationTypeIDLoop).FirstOrDefault();
                string nextTYpeName = nextType.LocationTypeName;
                int? tryNewLocationType = nextType.LocationTypeChildID;
                //add it to the list in the viewmodel
                receivedModalSublocationsViewModel.locationTypeNames.Add(nextTYpeName);

                //while we're still looping through we'll instantiate the locationInstancesSelected so we can have dropdownlistfors on the view
                receivedModalSublocationsViewModel.locationInstancesSelected.Add(new LocationInstance());

                if (tryNewLocationType == null)
                {
                    //if its not null we can convert it and pass it in
                    finished = true;
                }
                else
                {
                    locationTypeIDLoop = (Int32)tryNewLocationType;
                }
            }
            return PartialView(receivedModalSublocationsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public IActionResult ReceivedModalVisual(int LocationInstanceID)
        {
            ReceivedModalVisualViewModel receivedModalVisualViewModel = new ReceivedModalVisualViewModel()
            {
                IsEditModalTable = false
            };

            var parentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == LocationInstanceID).FirstOrDefault();

            //if it's an empty shelf- reset the location to the parent location instance id:
            if (parentLocationInstance.LocationTypeID == 201 && parentLocationInstance.IsEmptyShelf)
            {
                parentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == parentLocationInstance.LocationInstanceParentID).FirstOrDefault();
            }

            var firstChildLI = _context.LocationInstances.Where(li => li.LocationInstanceParentID == parentLocationInstance.LocationInstanceID).FirstOrDefault();
            LocationInstance secondChildLi = null;
            bool Is80Freezer = false;
            var hasEmptyShelves = false;
            if (firstChildLI != null)
            {
                secondChildLi = _context.LocationInstances.Where(li => li.LocationInstanceParentID == firstChildLI.LocationInstanceID).FirstOrDefault(); //second child is to ensure it doesn't have any box units
            }
            if (parentLocationInstance.LocationTypeID == 200) //check if it containes empty shelves ONLY IF -80
            {
                Is80Freezer = true;
                var shelves = _context.LocationInstances.Where(li => li.LocationInstanceParentID == parentLocationInstance.LocationInstanceID && li.IsEmptyShelf == true).ToList();
                if (shelves.Any())
                {
                    hasEmptyShelves = true;
                }
            }

            if (parentLocationInstance.IsEmptyShelf == true || (secondChildLi != null && !Is80Freezer) || (Is80Freezer && !hasEmptyShelves)) //secondChildLi will be null if first child is null
            {
                receivedModalVisualViewModel.DeleteTable = true;
            }
            else
            {
                receivedModalVisualViewModel.ParentLocationInstance = parentLocationInstance;

                if (receivedModalVisualViewModel.ParentLocationInstance != null)
                {
                    receivedModalVisualViewModel.ChildrenLocationInstances =
                        _context.LocationInstances.Where(m => m.LocationInstanceParentID == LocationInstanceID)
                        .Include(m => m.RequestLocationInstances).ToList();


                    //add placeholders for new places
                    List<LocationInstancePlace> liPlaces = new List<LocationInstancePlace>();
                    foreach (var cli in receivedModalVisualViewModel.ChildrenLocationInstances)
                    {
                        liPlaces.Add(new LocationInstancePlace()
                        {
                            LocationInstanceId = cli.LocationInstanceID,
                            Placed = false
                        });

                    }
                    receivedModalVisualViewModel.LocationInstancePlaces = liPlaces;
                    //return NotFound();
                }
            }
            return PartialView(receivedModalVisualViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReceivedModal(ReceivedLocationViewModel receivedLocationViewModel, ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel, ReceivedModalVisualViewModel receivedModalVisualViewModel)
        {
            var requestReceived = _context.Requests.Where(r => r.RequestID == receivedLocationViewModel.Request.RequestID)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).FirstOrDefault();

            bool hasLocationInstances = false;
            if (receivedLocationViewModel.CategoryType == 1)
            {
                foreach (var place in receivedModalVisualViewModel.LocationInstancePlaces)
                {
                    if (place.Placed)
                    {
                        hasLocationInstances = true;
                        //getting the parentlocationinstanceid
                        var liParent = _context.LocationInstances.Where(li => li.LocationInstanceID == receivedModalVisualViewModel.ParentLocationInstance.LocationInstanceID).FirstOrDefault();
                        var mayHaveParent = true;
                        while (mayHaveParent)
                        {
                            if (liParent.LocationInstanceParentID != null)
                            {
                                liParent = _context.LocationInstances.Where(li => li.LocationInstanceID == liParent.LocationInstanceParentID).FirstOrDefault();
                            }
                            else
                            {
                                mayHaveParent = false;
                            }
                        }

                        //adding the requestlocationinstance
                        var rli = new RequestLocationInstance()
                        {
                            LocationInstanceID = place.LocationInstanceId,
                            RequestID = requestReceived.RequestID,
                            ParentLocationInstanceID = liParent.LocationInstanceID
                        };
                        _context.Add(rli);
                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {

                        }

                        //updating the locationinstance
                        var locationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == place.LocationInstanceId).FirstOrDefault();
                        if (locationInstance.LocationTypeID == 103 || locationInstance.LocationTypeID == 204)
                        {
                            locationInstance.IsFull = true;
                        }
                        else
                        {
                            locationInstance.ContainsItems = true;
                        }
                        _context.Update(locationInstance);
                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                //foreach (LocationInstance locationInstance in receivedModalVisualViewModel.ChildrenLocationInstances)
                //{
                //    bool flag = false;
                //    LocationInstance parentLocationInstance = locationInstance;
                //    while (!flag)
                //    {
                //        var pli = _context.LocationInstances.Where(li => li.LocationInstanceID == parentLocationInstance.LocationInstanceParentID).FirstOrDefault();
                //        if (pli != null)
                //        {
                //            parentLocationInstance = pli;
                //        }
                //        else
                //        {
                //            flag = true;
                //        }
                //    }

                //    var tempLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == locationInstance.LocationInstanceID).FirstOrDefault();
                //    if (!tempLocationInstance.IsFull && locationInstance.IsFull)//only putting in the locationInstance.IsFull if it's false b/c sometimes it doesn't pass in the true value so we can end up taking things out by mistake
                //    {
                //        tempLocationInstance.IsFull = locationInstance.IsFull;
                //        _context.Update(tempLocationInstance);
                //        //coule be later on we'll want to save here too

                //        //this only works because we're using a one to many relationship with request and locationinstance instead of a many to many
                //        var requestLocationInstances = _context.LocationInstances
                //            .Where(li => li.LocationInstanceID == locationInstance.LocationInstanceID)
                //            .FirstOrDefault().RequestLocationInstances;

                //        //if it doesn't have any requestlocationinstances
                //        //WHY DO WE NEED THIS??????
                //        if (requestLocationInstances == null)
                //        {
                //            RequestLocationInstance requestLocationInstance = new RequestLocationInstance()
                //            {
                //                RequestID = receivedLocationViewModel.Request.RequestID,
                //                LocationInstanceID = locationInstance.LocationInstanceID,
                //                ParentLocationInstanceID = parentLocationInstance.LocationInstanceID
                //            };
                //            _context.Add(requestLocationInstance);
                //            hasLocationInstances = true;
                //        }
                //        _context.SaveChanges();
                //    }
                //}
                if (hasLocationInstances)
                {
                    if (receivedLocationViewModel.Clarify)
                    {
                        requestReceived.RequestStatusID = 5;
                    }
                    else if (receivedLocationViewModel.PartialDelivery)
                    {
                        requestReceived.RequestStatusID = 4;
                    }
                    else
                    {
                        requestReceived.RequestStatusID = 3;
                    }
                }

            }
            else
            {
                if (receivedLocationViewModel.Clarify)
                {
                    requestReceived.RequestStatusID = 5;
                }
                else if (receivedLocationViewModel.PartialDelivery)
                {
                    requestReceived.RequestStatusID = 4;
                }
                else
                {
                    requestReceived.RequestStatusID = 3;
                }
            }
            try
            {
                requestReceived.ArrivalDate = receivedLocationViewModel.Request.ArrivalDate;
                requestReceived.ApplicationUserReceiverID = receivedLocationViewModel.Request.ApplicationUserReceiverID;
                requestReceived.ApplicationUserReceiver = _context.Users.Where(u => u.Id == receivedLocationViewModel.Request.ApplicationUserReceiverID).FirstOrDefault();
                _context.Update(requestReceived);
                await _context.SaveChangesAsync();

                RequestNotification requestNotification = new RequestNotification();
                requestNotification.RequestID = requestReceived.RequestID;
                requestNotification.IsRead = false;
                requestNotification.ApplicationUserID = requestReceived.ApplicationUserCreatorID;
                requestNotification.RequestName = requestReceived.Product.ProductName;
                requestNotification.NotificationStatusID = 4;
                var FName = _context.Users.Where(u => u.Id == requestReceived.ApplicationUserReceiverID).FirstOrDefault().FirstName;
                requestNotification.Description = "received by " + FName;
                requestNotification.TimeStamp = DateTime.Now;
                requestNotification.Controller = "Requests";
                requestNotification.Action = "NotificationsView";
                requestNotification.Vendor = requestReceived.Product.Vendor.VendorEnName;
                _context.Update(requestNotification);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }
            return RedirectToAction("_IndexTableWithCounts", receivedLocationViewModel.RequestIndexObject);

        }


        /*
         * END RECEIVED MODAL
         */
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public ActionResult DocumentsModal(int? id, int[]? ids, AppUtility.RequestFolderNamesEnum RequestFolderNameEnum, bool IsEdittable,
            AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, bool IsNotifications = false, bool isSummary = false)
        {
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                Requests = new List<Request>(),
                //Request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.Product).FirstOrDefault(),
                RequestFolderName = RequestFolderNameEnum,
                IsEdittable = IsEdittable,
                //Files = new List<FileInfo>(),
                SectionType = SectionType,
                IsNotifications = IsNotifications,
                IsSummary = isSummary

            };

            if (id != null)
            {
                documentsModalViewModel.Requests.Add(_context.Requests.Where(r => r.RequestID == id).Include(r => r.Product).FirstOrDefault());
            }
            else if (ids != null)
            {
                foreach (int requestID in ids)
                {
                    documentsModalViewModel.Requests.Add(_context.Requests.Where(r => r.RequestID == requestID).Include(r => r.Product).FirstOrDefault());
                }
            }

            if (!IsNotifications) //Don't want to see old ones if it's notifications b/c has multiple requests
            {
                string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                string uploadFolder2 = Path.Combine(uploadFolder1, id.ToString());
                string uploadFolder3 = Path.Combine(uploadFolder2, RequestFolderNameEnum.ToString());

                if (Directory.Exists(uploadFolder3))
                {
                    DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolder3);
                    //searching for the partial file name in the directory
                    FileInfo[] docfilesfound = DirectoryToSearch.GetFiles("*.*");
                    documentsModalViewModel.FileStrings = new List<String>();
                    foreach (var docfile in docfilesfound)
                    {
                        string newFileString = AppUtility.GetLastFiles(docfile.FullName, 4);
                        documentsModalViewModel.FileStrings.Add(newFileString);
                        //documentsModalViewModel.Files.Add(docfile);
                    }
                }
            }

            return View(documentsModalViewModel);
        }


        [HttpPost]
        public void DocumentsModal(/*[FromBody]*/ DocumentsModalViewModel documentsModalViewModel)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string requestFolder = Path.Combine(uploadFolder, documentsModalViewModel.Requests[0].RequestID.ToString());
            Directory.CreateDirectory(requestFolder);
            if (documentsModalViewModel.FilesToSave != null) //test for more than one???
            {
                var x = 1;
                foreach (IFormFile file in documentsModalViewModel.FilesToSave)
                {
                    //create file
                    string folderPath = Path.Combine(requestFolder, documentsModalViewModel.RequestFolderName.ToString());
                    Directory.CreateDirectory(folderPath);
                    string uniqueFileName = x + file.FileName;
                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    FileStream filestream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(filestream);
                    filestream.Close();
                    x++;
                }
            }
        }

        [HttpGet]
        public ActionResult DeleteDocumentModal(String FileString, int id, AppUtility.RequestFolderNamesEnum RequestFolderNameEnum, bool IsEdittable, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            DeleteDocumentsViewModel deleteDocumentsViewModel = new DeleteDocumentsViewModel()
            {
                FileName = FileString,
                RequestID = id,
                FolderName = RequestFolderNameEnum,
                IsEdittable = IsEdittable,
                SectionType = SectionType
            };
            return PartialView(deleteDocumentsViewModel);
        }

        [HttpPost]
        public void DeleteDocumentModal(DeleteDocumentsViewModel deleteDocumentsViewModel)
        {
            string[] FileNameParts = deleteDocumentsViewModel.FileName.Split('\\');
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, deleteDocumentsViewModel.FileName);
            if (System.IO.File.Exists(uploadFolder))
            {
                try
                {
                    System.IO.File.Delete(uploadFolder);
                }
                catch (Exception ex)
                {
                    //do something here
                }
            }
        }



        [HttpGet]
        [Authorize(Roles = "Requests")]
        public ActionResult DocumentView(List<String> FileNames)
        {
            return View();
        }

        /*
         * JSONS
         */

        [HttpGet] //send a json to that the subcategory list is filered
        public JsonResult GetSubCategoryList(int ParentCategoryId)
        {
            var subCategoryList = _context.ProductSubcategories.Where(c => c.ParentCategoryID == ParentCategoryId).ToList();
            return Json(subCategoryList);

        }


        [HttpGet]
        public JsonResult GetSubProjectList(int ProjectID)
        {
            //var projectName = _context.Projects.Where(pr => pr.ProjectID == ProjectID).FirstOrDefault().ProjectDescription;
            var subprojectList = _context.SubProjects.Where(sp => sp.ProjectID == ProjectID).ToList();
            return Json(subprojectList);
        }
        [HttpGet]
        public JsonResult FilterByProjects(List<int> ProjectIDs)
        {
            var requests = _context.Requests.Where(r => ProjectIDs.Contains(r.SubProject.ProjectID)).Include(r => r.ApplicationUserCreator).Include(r => r.SubProject);
            var subProjectList = _context.SubProjects.Where(sp => ProjectIDs.Contains(sp.ProjectID)).Select(sp => new { subProjectID = sp.SubProjectID, subProjectDescription = sp.SubProjectDescription });
            var workers = requests.Select(r => r.ApplicationUserCreator).Select(e => new { workerID = e.Id, workerName = e.FirstName + " " + e.LastName }).Distinct();
            return Json(new { SubProjects = subProjectList, Employees = workers });

        }
        [HttpGet]
        public JsonResult FilterBySubProjects(List<int> SubProjectIDs)
        {
            var requests = _context.Requests.Where(r => SubProjectIDs.Contains(r.SubProjectID ?? 0)).Include(r => r.ApplicationUserCreator);
            var workers = requests.Select(r => r.ApplicationUserCreator).Select(e => new { workerID = e.Id, workerName = e.FirstName + " " + e.LastName }).Distinct();
            return Json(new { Employees = workers });
        }

        public bool CheckUniqueVendorAndCatalogNumber(int VendorID, string CatalogNumber, int? ProductID = null)
        {
            var boolCheck = true;
            //validation for the create
            if (VendorID != null && CatalogNumber != null && (ProductID == null && _context.Requests.Where(r => r.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID).Any()))
            {
                return false;
            }
            //validation for the edit
            var product = _context.Requests.Where(r => r.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID && r.ProductID != ProductID);
            if ( ProductID != null && _context.Requests.Where(r => r.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID && r.ProductID != ProductID).Any())
            {
                return false;
            }
            return boolCheck;
        }
        //[HttpGet]
        //public JsonResult GetCompanyAccountList(int PaymentTypeID)
        //{
        //    var companyAccountList = _context.CompanyAccounts.Where(c => c.PaymentTypeID == PaymentTypeID).ToList();
        //    return Json(companyAccountList);
        //}

        [HttpGet]
        public JsonResult GetSublocationInstancesList(int locationInstanceParentId)
        {
            var locationInstanceList = _context.LocationInstances.Where(li => li.LocationInstanceParentID == locationInstanceParentId).ToList();
            return Json(locationInstanceList);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests, Operations")]
        public IActionResult Approve(int id, RequestIndexObject requestIndex)
        {
            var request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.ParentQuote).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(px => px.ParentCategory).Include(r => r.Product.Vendor).FirstOrDefault();
            try
            {
                request.RequestStatusID = 6; //approved
                if(request is Reorder)
                {
                    request.ParentQuote.QuoteStatusID = 1;
                }
                _context.Update(request);
                _context.SaveChanges();
                RequestNotification requestNotification = new RequestNotification();
                requestNotification.RequestID = request.RequestID;
                requestNotification.IsRead = false;
                requestNotification.RequestName = request.Product.ProductName;
                requestNotification.ApplicationUserID = request.ApplicationUserCreatorID;
                requestNotification.Description = "item approved";
                requestNotification.NotificationStatusID = 3;
                requestNotification.TimeStamp = DateTime.Now;
                requestNotification.Controller = "Requests";
                requestNotification.Action = "NotificationsView";
                requestNotification.Vendor = request.Product.Vendor.VendorEnName;
                _context.Update(requestNotification);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }
            return RedirectToAction("_IndexTableWithCounts", requestIndex);


        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public IActionResult EditQuoteDetails(int id, int requestID = 0)
        {
            if (requestID != 0)
            {
                //user wants to edit only one quote
                var requests = _context.Requests.OfType<Reorder>().Where(r => r.RequestID == requestID)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.ParentQuote)
                    .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).ToList();
                var vendor = _context.Vendors.Where(v => v.VendorID == id).FirstOrDefault();
                EditQuoteDetailsViewModel editQuoteDetailsViewModel = new EditQuoteDetailsViewModel()
                {
                    Reorders = requests,
                    Vendor = vendor,
                    QuoteDate = DateTime.Now,
                    ParentQuoteID = requests.FirstOrDefault().ParentQuoteID
                };
                return PartialView(editQuoteDetailsViewModel);
            }
            //needs testing 
            //not implemented at all on the client side
            //just here for now for future implmentation
            else
            {
                var requests = _context.Requests.OfType<Reorder>()
              .Where(r => r.Product.VendorID == id && (r.ParentQuote.QuoteStatusID == 2 || r.ParentQuote.QuoteStatusID == 1) && r.RequestStatusID == 6)
              .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(p => p.Product).ThenInclude(p => p.ProductSubcategory)
              .Include(r => r.ParentQuote).Include(r => r.UnitType).Include(r => r.SubSubUnitType).Include(r => r.SubUnitType).ToList();

                return PartialView(requests);
            }
        }
        [HttpPost]
        [Authorize(Roles = "LabManagement")]
        public IActionResult EditQuoteDetails(EditQuoteDetailsViewModel editQuoteDetailsViewModel)
        {
            try
            {
                var requests = _context.Requests.OfType<Reorder>().Include(x => x.ParentQuote).Select(r => r);
                var quoteDate = editQuoteDetailsViewModel.QuoteDate;
                var quoteNumber = editQuoteDetailsViewModel.QuoteNumber;
                foreach (var quote in editQuoteDetailsViewModel.Reorders)
                {
                    var request = requests.Where(r => r.RequestID == quote.RequestID).FirstOrDefault();

                    request.ParentQuote.QuoteStatusID = 4;
                    request.ParentQuote.QuoteDate = quoteDate;
                    request.ParentQuote.QuoteNumber = quoteNumber.ToString(); ;
                    request.Cost = quote.Cost;
                    request.ExpectedSupplyDays = quote.ExpectedSupplyDays;
                    _context.Update(request);
                    _context.SaveChanges();
                    //save file
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    string requestFolder = Path.Combine(uploadFolder, quote.RequestID.ToString());
                    string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Quotes.ToString());
                    Directory.CreateDirectory(folderPath);
                    string uniqueFileName = 1 + editQuoteDetailsViewModel.QuoteFileUpload.FileName;
                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    editQuoteDetailsViewModel.QuoteFileUpload.CopyTo(new FileStream(filePath, FileMode.Create));

                }
                return RedirectToAction("LabManageOrders");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }


        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> NotificationsView(int requestID = 0)
        {
            if (requestID != 0)
            {
                var notification = _context.RequestNotifications.Where(rn => rn.NotificationID == requestID).FirstOrDefault();
                notification.IsRead = true;
                _context.Update(notification);
                await _context.SaveChangesAsync();
            }
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Notifications;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            ApplicationUser currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            var requests = _context.RequestNotifications.Include(n => n.NotificationStatus).Where(n => n.ApplicationUserID == currentUser.Id).OrderByDescending(n => n.TimeStamp).ToList();
            return View(requests);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> Cart()
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Cart;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            CartViewModel cartViewModel = new CartViewModel();
            cartViewModel.RequestsByVendor = _context.Requests.Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
                .Where(r => r.RequestStatusID == 6 && !(r is Reorder))
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                .Include(r => r.ApplicationUserCreator)
                .ToLookup(r => r.Product.Vendor);

            return View(cartViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests, Users, Biomarkers, Accounting, Admin, Reports, Timekeeper, Operations, Protocols, Income, Operation, Expenses, LabManagement")]
        public async Task<IActionResult> ConfirmEdit(AppUtility.MenuItems MenuItem = AppUtility.MenuItems.Requests)
        {
            return PartialView(MenuItem);
        }

        [HttpGet]
        [Authorize(Roles = "Requests, Users, Biomarkers, Accounting, Admin, Reports, Timekeeper, Operations, Protocols, Income, Operation, Expenses, LabManagement")]
        public async Task<IActionResult> ConfirmExit(AppUtility.MenuItems MenuItem = AppUtility.MenuItems.Requests)
        {
            return PartialView(MenuItem);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> CommentInfoPartialView(String type, int index)
        {
            Comment comment = new Comment();
            comment.CommentType = type;
            CommentsInfoViewModel commentsInfoViewModel = new CommentsInfoViewModel { Comment = comment, Index = index };
            return PartialView(commentsInfoViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> InstallmentsPartial(int index)
        {
            return PartialView(index);
        }



        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> OrderLateModal(int id)
        {
            var request = _context.Requests
                .Where(r => r.RequestID == id)
                .Include(r => r.ApplicationUserCreator)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).FirstOrDefault();
            return PartialView(request);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> OrderLateModal(Request request)
        {
            request = _context.Requests.Where(r => r.RequestID == request.RequestID).Include(r => r.ApplicationUserCreator).Include(r => r.ParentRequest).Include(r => r.Product).ThenInclude(p => p.Vendor).FirstOrDefault();
            //instatiate mimemessage
            var message = new MimeMessage();

            //instantiate the body builder
            var builder = new BodyBuilder();


            string ownerEmail = request.ApplicationUserCreator.Email;
            string ownerUsername = request.ApplicationUserCreator.FirstName + " " + request.ApplicationUserCreator.LastName;
            string ownerPassword = request.ApplicationUserCreator.SecureAppPass;
            string vendorEmail = request.Product.Vendor.OrdersEmail;
            string vendorName = request.Product.Vendor.VendorEnName;

            //add a "From" Email
            message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

            // add a "To" Email
            message.To.Add(new MailboxAddress(vendorName, vendorEmail));

            //subject
            message.Subject = "Message to " + vendorName;

            //body
            builder.TextBody = $"The order number {request.ParentRequest.OrderNumber} for {request.Product.ProductName} , has not arrived yet.\n" +
                    $"Please update us on the matter.\n" +
                    $"Best regards,\n" +
                    $"{request.ApplicationUserCreator.FirstName} { request.ApplicationUserCreator.FirstName}\n" +
                    $"Centarix";

            message.Body = builder.ToMessageBody();


            using (var client = new SmtpClient())
            {

                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(ownerEmail, ownerPassword);
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {

                }

                client.Disconnect(true);

            }
            return RedirectToAction("NotificationsView");
        }

        private bool checkIfInBudget(Request request)
        {
            DateTime firstOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
            { //lab
                var pricePerUnit = request.Cost / request.Unit;
                if (pricePerUnit > request.ApplicationUserCreator.LabUnitLimit)
                {
                    return false;
                }
                if (request.Cost > request.ApplicationUserCreator.LabOrderLimit)
                {
                    return false;
                }
                var monthsSpending = _context.Requests
                      .Where(r => request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                      .Where(r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID)
                      .Where(r => r.ParentRequest.OrderDate >= firstOfMonth)
                      .Sum(r => r.Cost);
                if (monthsSpending + request.Cost > request.ApplicationUserCreator.LabMonthlyLimit)
                {
                    return false;
                }
                return true;
            }

            else
            {
                //should never reach here because we are in the lab section
                //probably will never happen
                return false; //not any type of operation and therefore cannot be ordered without being approved
            }
        }



        /*
         *NEW NOTIFICATIONS VIEW
         */

        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AccountingPayments(AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment)
        {

            var requestsList = _context.Requests
                .Include(r => r.ParentRequest)
                .Include(r => r.Product).ThenInclude(p => p.Vendor)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(pc => pc.ParentCategory)
                .Where(r => r.InvoiceID != null)
                .Where(r => r.ParentRequest.WithoutOrder == false)
                .Where(r => r.IsDeleted == false);

            var payNowList = requestsList
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .Where(r => r.PaymentStatusID == 3);

            var payNowListCount = payNowList.ToList().Count;

            TempData["PayNowCount"] = payNowListCount;

            switch (accountingPaymentsEnum)
            {
                case AppUtility.SidebarEnum.MonthlyPayment:
                    requestsList = requestsList
                        .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                        .Where(r => r.PaymentStatusID == 1);
                    break;
                case AppUtility.SidebarEnum.PayNow:
                    requestsList = payNowList;
                    break;
                case AppUtility.SidebarEnum.PayLater:
                    requestsList = requestsList
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .Where(r => r.PaymentStatusID == 4);
                    break;
                case AppUtility.SidebarEnum.Installments:
                    requestsList = requestsList
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .Where(r => r.PaymentStatusID == 5);
                    break;
                case AppUtility.SidebarEnum.StandingOrders:
                    requestsList = requestsList
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2);
                    break;
            }
            AccountingPaymentsViewModel accountingPaymentsViewModel = new AccountingPaymentsViewModel()
            {
                AccountingEnum = accountingPaymentsEnum,
                Requests = requestsList.ToLookup(r => r.Product.Vendor),
                PayNowListNum = payNowListCount
            };

            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Accounting;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingPayments;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = accountingPaymentsEnum;

            return View(accountingPaymentsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> ChangePaymentStatus(AppUtility.PaymentsPopoverEnum newStatus, int requestID, AppUtility.PaymentsPopoverEnum currentStatus)
        {

            var request = _context.Requests.Where(r => r.RequestID == requestID).FirstOrDefault();
            try
            {
                request.PaymentStatusID = (int)newStatus;
                _context.Update(request);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }

            var accountingPaymentsEnum = (AppUtility.SidebarEnum)Enum.Parse(typeof(AppUtility.SidebarEnum), currentStatus.ToString());

            return RedirectToAction("AccountingPayments", new { accountingPaymentsEnum = accountingPaymentsEnum });
        }
        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AccountingNotifications(AppUtility.SidebarEnum accountingNotificationsEnum = AppUtility.SidebarEnum.NoInvoice)
        {
            var requestsList = _context.Requests
                .Include(r => r.ParentRequest)
                .Include(r => r.Product).ThenInclude(p => p.Vendor)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(pc => pc.ParentCategory)
                .Where(r => r.ParentRequest.WithoutOrder == false) //TODO: check if this is here
                .Where(r => r.IsDeleted == false).AsQueryable();
            switch (accountingNotificationsEnum)
            {
                case AppUtility.SidebarEnum.NoInvoice: //NOTE EXACT SAME QUERY IN ADDINVOICE MODAL
                    requestsList = requestsList.Where(r => r.InvoiceID == null);
                    break;
                case AppUtility.SidebarEnum.DidntArrive:
                    requestsList = requestsList.Where(r => r.RequestStatusID == 2).Where(r => r.ExpectedSupplyDays != null).Where(r => r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0).Date < DateTime.Today);
                    break;
                case AppUtility.SidebarEnum.PartialDelivery:
                    requestsList = requestsList.Where(r => r.RequestStatusID == 4);
                    break;
                case AppUtility.SidebarEnum.ForClarification:
                    requestsList = requestsList.Where(r => r.RequestStatusID == 5);
                    break;
            }
            AccountingPaymentsViewModel accountingPaymentsViewModel = new AccountingPaymentsViewModel()
            {
                AccountingEnum = accountingNotificationsEnum,
                Requests = requestsList.ToList().ToLookup(r => r.Product.Vendor)
            };
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Accounting;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingNotifications;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = accountingNotificationsEnum;
            return View(accountingPaymentsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> PaymentsPayModal(int? vendorid, int? paymentstatusid, AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment  /*, List<int>? requestIds*/)
        {
            List<Request> requestsToPay = new List<Request>();

            if (vendorid != null && paymentstatusid != null)
            {
                requestsToPay = _context.Requests
                .Include(r => r.Product).ThenInclude(p => p.Vendor)
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .Where(r => r.Product.VendorID == vendorid)
                .Where(r => r.PaymentStatusID == paymentstatusid).ToList();
            }

            PaymentsPayModalViewModel paymentsPayModalViewModel = new PaymentsPayModalViewModel()
            {
                Requests = requestsToPay,
                AccountingEnum = accountingPaymentsEnum
            };

            //check if payment status type is installments to show the installments in the view model

            return PartialView(paymentsPayModalViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> PaymentsPayModal(PaymentsPayModalViewModel paymentsPayModalViewModel)
        {
            foreach (Request request in paymentsPayModalViewModel.Requests)
            {
                var requestToUpdate = _context.Requests.Where(r => r.RequestID == request.RequestID).FirstOrDefault();
                requestToUpdate.PaymentStatusID = 6;
                _context.Update(requestToUpdate);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("AccountingPayments", new { accountingPaymentsEnum = paymentsPayModalViewModel.AccountingEnum });
        }

        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AddInvoiceModal(int? vendorid, int? requestid, int[] requestIds)
        {
            List<Request> Requests = new List<Request>();
            var queryableRequests = _context.Requests
                .Include(r => r.ParentRequest)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                    .Where(r => r.IsDeleted == false);
            if (vendorid != null)
            {
                Requests = queryableRequests //NOTE THIS QUERY MUST MATCH ABOVE QUERY
                    .Where(r => r.ParentRequest.WithoutOrder == false) //TODO: check if this is here
                    .Where(r => r.InvoiceID == null)
                    .Where(r => r.Product.VendorID == vendorid).ToList();
            }
            else if (requestid != null)
            {
                Requests = queryableRequests.Where(r => r.RequestID == requestid).ToList();
            }
            else if (requestIds != null)
            {
                foreach (int rId in requestIds)
                {
                    Requests.Add(queryableRequests.Where(r => r.RequestID == rId).FirstOrDefault());
                }
            }
            AddInvoiceViewModel addInvoiceViewModel = new AddInvoiceViewModel()
            {
                Requests = Requests,
                Invoice = new Invoice()
                {
                    InvoiceDate = DateTime.Today
                }
            };
            return PartialView(addInvoiceViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AddInvoiceModal(AddInvoiceViewModel addInvoiceViewModel)
        {
            _context.Add(addInvoiceViewModel.Invoice); //TODO Return To Modal if Not filled in
            await _context.SaveChangesAsync();

            foreach (var request in addInvoiceViewModel.Requests)
            {
                var RequestToSave = _context.Requests.Where(r => r.RequestID == request.RequestID).FirstOrDefault();
                RequestToSave.Cost = request.Cost;
                RequestToSave.InvoiceID = addInvoiceViewModel.Invoice.InvoiceID;
                _context.Update(RequestToSave);
            }
            await _context.SaveChangesAsync();

            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string requestFolder = Path.Combine(uploadFolder, addInvoiceViewModel.Requests[0].RequestID.ToString());
            Directory.CreateDirectory(requestFolder);
            if (addInvoiceViewModel.InvoiceImage != null)
            {
                int x = 1;
                //create file
                string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Invoices.ToString());
                if (Directory.Exists(folderPath))
                {
                    var filesInDirectory = Directory.GetFiles(folderPath);
                    x = filesInDirectory.Length + 1;
                }
                else
                {
                    Directory.CreateDirectory(folderPath);
                }
                string uniqueFileName = x + addInvoiceViewModel.InvoiceImage.FileName;
                string filePath = Path.Combine(folderPath, uniqueFileName);
                FileStream filestream = new FileStream(filePath, FileMode.Create);
                addInvoiceViewModel.InvoiceImage.CopyTo(filestream);
                filestream.Close();
            }

            return RedirectToAction("AccountingNotifications");
        }


        /*
         * 
         */


    }


}
