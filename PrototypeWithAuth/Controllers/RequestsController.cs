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
        private readonly IHostingEnvironment _hostingEnvironment;
        private ISession _session;
        private ICompositeViewEngine _viewEngine;

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

            if (ViewBag.ErrorMessage != null)
            {
                ViewBag.ErrorMessage = ViewBag.ErrorMessage;
            }

            return View(viewmodel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests, Operations")]
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
                if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Category)
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
                    sidebarFilterDescription = _context.ProductSubcategories.Where(p => p.ProductSubcategoryID == sideBarID).Select(p => p.ProductSubcategoryDescription).FirstOrDefault();
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
            requestIndexViewModel.OrderStepsEnum = requestIndexObject.OrderStep;
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
            onePageOfProducts = await RequestPassedInWithInclude.OrderBy(r => r.ArrivalDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
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
                             new RequestIndexPartialColumnViewModel() { Title = "Arrival Date", Width=10, Value = new List<string>(){ r.ArrivalDate.ToString("dd'/'MM'/'yyyy") } },
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
                await _context.SaveChangesAsync();
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
                if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Quotes)
                {
                    return RedirectToAction("LabManageQuotes");
                }
                else
                {
                    return RedirectToAction("LabManageOrders");
                }

            }
            else if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestInventory || requestIndexObject.PageType == AppUtility.PageTypeEnum.OperationsInventory)
            {
                return RedirectToAction("_IndexTableData", requestIndexObject);
            }
            else
            {
                return RedirectToAction("_IndexTableWithCounts", requestIndexObject);
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddItemView(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            ChooseCategoryViewModel categoryViewModel = new ChooseCategoryViewModel()
            {
                ParentCategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 1).ToListAsync()
            };

            return View(categoryViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> AddItemView(RequestItemViewModel requestItemViewModel, AppUtility.OrderTypeEnum OrderType)
        {
            //why do we need this here?
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.Include(ps => ps.ParentCategory).FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

            //in case we need to return to the modal view
            //requestItemViewModel.ParentCategory = await _context.ParentCategories.Where(pc => pc.ParentCategoryID == requestItemViewModel.Request.Product.ProductSubcategory.ParentCategory.ParentCategoryID).FirstOrDefaultAsync();

            //declared outside the if b/c it's used farther down too 
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            requestItemViewModel.Request.ApplicationUserCreatorID = currentUser.Id;           
            requestItemViewModel.Request.CreationDate = DateTime.Now;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var isInBudget = checkIfInBudget(requestItemViewModel.Request);
                    await AddItemAccordingToOrderType(requestItemViewModel.Request, OrderType, isInBudget);
                    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                    var isSavedUsingSession = HttpContext.Session.GetObject<Request>(requestName) != null;
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

                                if (!isSavedUsingSession)
                                {
                                    _context.Add(comment);
                                }
                                else
                                {
                                    var SessionCommentName = AppData.SessionExtensions.SessionNames.Comment.ToString() + x;
                                    HttpContext.Session.SetObject(SessionCommentName, comment);
                                }
                            }

                            x++; //to name the comments in session
                        }
                    }
                    if (!isSavedUsingSession)
                    {

                        await _context.SaveChangesAsync();
                        MoveDocumentsOutOfTempFolder(requestItemViewModel.Request);
                        await transaction.CommitAsync();
                        base.RemoveRequestSessions();
                    }
                    else
                    {
                        var emailNum = 1;
                        foreach(var e in requestItemViewModel.EmailAddresses)
                        {
                            var SessionEmailName = AppData.SessionExtensions.SessionNames.Email.ToString() + emailNum;
                            HttpContext.Session.SetObject(SessionEmailName, e);
                            emailNum++;
                        }
                       
                    }
                    var orderStep = AppUtility.OrderStepsEnum.None;
                    var action = "Index";
                    switch (OrderType)
                    {
                        case AppUtility.OrderTypeEnum.AlreadyPurchased:
                            orderStep = AppUtility.OrderStepsEnum.UploadOrderModal;
                            action = "UploadOrderModal";
                            break;
                        case AppUtility.OrderTypeEnum.OrderNow:
                            orderStep = AppUtility.OrderStepsEnum.UploadQuoteModal;
                            action = "UploadQuoteModal";
                            break;
                        case AppUtility.OrderTypeEnum.AddToCart:
                            orderStep = AppUtility.OrderStepsEnum.UploadQuoteModal;
                            action = "UploadQuoteModal";
                            break;
                    }

                    return RedirectToAction(action, "Requests", new RequestIndexObject()
                    {
                        PageType = AppUtility.PageTypeEnum.RequestRequest,
                        SectionType = AppUtility.MenuItems.Requests,
                        SidebarType = AppUtility.SidebarEnum.List,
                        RequestStatusID = requestItemViewModel.Request.RequestStatusID ?? 1,
                        OrderStep = orderStep
                    });

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    base.RemoveRequestSessions();
                    ViewBag.ErrorMessage = ex.InnerException?.ToString();
                    requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 1).ToListAsync();
                    requestItemViewModel.Projects = await _context.Projects.ToListAsync();
                    requestItemViewModel.SubProjects = await _context.SubProjects.ToListAsync();
                    requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();
                    requestItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();
                    var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
                    requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");
                    return PartialView("CreateItemTabs", requestItemViewModel);
                }
            }
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> CreateItemTabs(int parentCategoryId, AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            var parentcategory = await _context.ParentCategories.Where(pc => pc.ParentCategoryID == parentCategoryId).FirstOrDefaultAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == parentCategoryId).ToListAsync();
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
            requestItemViewModel.Request.Product.ProductSubcategory = new ProductSubcategory();
            requestItemViewModel.Request.Product.ProductSubcategory.ParentCategory = parentcategory;
            requestItemViewModel.Request.Product.ProductSubcategory.ParentCategoryID = parentcategory.ParentCategoryID;
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

            return PartialView(requestItemViewModel);
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> EditModalView(int? id, bool NewRequestFromProduct = false, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, bool isEditable = true)
        {
            return await editModalViewFunction(id, 0, SectionType, isEditable);
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

            var request = _context.Requests.Include(r => r.Product)
                .Include(r => r.ParentQuote)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product.ProductSubcategory)
                .Include(r => r.Product.ProductSubcategory.ParentCategory)
                     .Include(r => r.Product.Vendor)
                .Include(r => r.RequestStatus)
                .Include(r => r.ApplicationUserCreator)
                //.Include(r => r.Payments) //do we have to have a separate list of payments to include thefix c inside things (like company account and payment types?)
                .SingleOrDefault(x => x.RequestID == id);

            var requestsByProduct = _context.Requests.Where(r => r.ProductID == productId && (r.RequestStatusID == 3))
                 .Include(r => r.Product.ProductSubcategory).Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .Include(r => r.ApplicationUserCreator) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                    .ToList();
            var productsubactegories = await _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == request.Product.ProductSubcategory.ParentCategoryID).ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 1).Count() > 0).ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
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

            requestItemViewModel.Request = request;

            //load the correct list of subprojects
            //var subprojects = await _context.SubProjects
            //    .Where(sp => sp.ProjectID == requestItemViewModel.Request.SubProject.ProjectID)
            //    .ToListAsync();
            //requestItemViewModel.SubProjects = subprojects;

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
            //else
            //{
            //    parentQuote = new ParentQuote();
            //    parentQuote.QuoteNumber = requestItemViewModel.Request.ParentQuote.QuoteNumber;
            //    parentQuote.QuoteDate = requestItemViewModel.Request.ParentQuote.QuoteDate;
            //    requestItemViewModel.Request.ParentQuote = parentQuote;
            //}
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
            var parentCategoryId = requestItemViewModel.Request.Product.ProductSubcategory.ParentCategoryID;
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 1).Where(ps => ps.ParentCategoryID == parentCategoryId).ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");

            //declared outside the if b/c it's used farther down to (for parent request the new comment too)
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

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

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(RequestIndexObject requestIndexObject, int? id, bool NewRequestFromProduct = false, String SectionType = "")
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            Request request = _context.Requests
                .Include(r => r.Product).ThenInclude(p=>p.ProductSubcategory)
                .Include(r => r.UnitType)
                .Include(r => r.SubUnitType)
                .Include(r => r.SubSubUnitType)
                .SingleOrDefault(x => x.RequestID == id);

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                Request = request,
            };
            var reorderViewModel = new ReorderViewModel() { RequestIndexObject = requestIndexObject, RequestItemViewModel = requestItemViewModel };
            return PartialView(reorderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(ReorderViewModel reorderViewModel, AppUtility.OrderTypeEnum OrderTypeEnum)
        {
            //  ReorderViewModel reorderViewModel = JsonConvert.DeserializeObject<ReorderViewModel>(json);
            //get the old request that we are reordering
            var oldRequest = _context.Requests.Where(r => r.RequestID == reorderViewModel.RequestItemViewModel.Request.RequestID)
                .Include(r => r.Product)
                .ThenInclude(p => p.ProductSubcategory).ThenInclude(ps=>ps.ParentCategory).FirstOrDefault();

            
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            //need to include product to check if in budget
            reorderViewModel.RequestItemViewModel.Request.Product = oldRequest.Product;

            reorderViewModel.RequestItemViewModel.Request.RequestID = 0;
            reorderViewModel.RequestItemViewModel.Request.ProductID = oldRequest.ProductID;
            reorderViewModel.RequestItemViewModel.Request.ApplicationUserCreatorID = currentUser.Id;
            reorderViewModel.RequestItemViewModel.Request.CreationDate = DateTime.Now;
            reorderViewModel.RequestItemViewModel.Request.SubProjectID = oldRequest.SubProjectID;
            reorderViewModel.RequestItemViewModel.Request.SerialNumber = oldRequest.SerialNumber;
            reorderViewModel.RequestItemViewModel.Request.URL = oldRequest.URL;
            reorderViewModel.RequestItemViewModel.Request.Warranty = oldRequest.Warranty;
            reorderViewModel.RequestItemViewModel.Request.ExchangeRate = oldRequest.ExchangeRate; 
            reorderViewModel.RequestItemViewModel.Request.Currency = oldRequest.Currency;
            reorderViewModel.RequestItemViewModel.Request.CatalogNumber = oldRequest.CatalogNumber;
            var isInBudget = checkIfInBudget(reorderViewModel.RequestItemViewModel.Request);
            var orderStep = AppUtility.OrderStepsEnum.None;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    await AddItemAccordingToOrderType(reorderViewModel.RequestItemViewModel.Request, OrderTypeEnum, isInBudget);
                    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                    var isSavedUsingSession = HttpContext.Session.GetObject<Request>(requestName) != null;
                    if (!isSavedUsingSession)
                    {
                        MoveDocumentsOutOfTempFolder(reorderViewModel.RequestItemViewModel.Request);
                        await transaction.CommitAsync();
                        base.RemoveRequestSessions();
                    }
                    switch (OrderTypeEnum)
                    {
                        case AppUtility.OrderTypeEnum.AlreadyPurchased:
                            orderStep = AppUtility.OrderStepsEnum.UploadOrderModal;
                            break;
                        case AppUtility.OrderTypeEnum.OrderNow:
                            orderStep = AppUtility.OrderStepsEnum.UploadQuoteModal;
                            break;
                        case AppUtility.OrderTypeEnum.AddToCart:
                            orderStep = AppUtility.OrderStepsEnum.UploadQuoteModal;
                            break;
                    }                    
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    base.RemoveRequestSessions();
                    reorderViewModel.ErrorMessages += ex.InnerException + "/n";
                    var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
                    reorderViewModel.RequestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");
                    return PartialView("ReOrderFloatModalView", reorderViewModel);
                }
             }
           e
            reorderViewModel.RequestIndexObject.OrderStep = orderStep;
            return RedirectToAction("Index", reorderViewModel.RequestIndexObject);
        }

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
        public async Task<IActionResult> ConfirmEmailModal(int id, RequestIndexObject requestIndexObject)
        {
            var allRequests = new List<Request>();
            var isRequests = true;
            var RequestNum = 1;
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
            while (isRequests)
            {
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                if (HttpContext.Session.GetObject<Request>(requestName) != null)
                {
                    var request = HttpContext.Session.GetObject<Request>(requestName);
                    request.ParentRequest = pr;
                    HttpContext.Session.SetObject(requestName, request);
                    allRequests.Add(request);
                }
                else
                {
                    isRequests = false;
                }
                RequestNum++;
            }
            
            ConfirmEmailViewModel confirm = new ConfirmEmailViewModel
            {
                ParentRequest = pr,
                Requests = allRequests,
                RequestIndexObject = requestIndexObject
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
            confirm.RequestIndexObject = requestIndexObject;
            return PartialView(confirm);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmEmailModal(ConfirmEmailViewModel confirmEmailViewModel)
        {

            string uploadFolder = Path.Combine("wwwroot", "files");
            string uploadFile = Path.Combine(uploadFolder, "ConfirmEmailTempDoc.pdf");

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
            var action = "Index";


            if (requests.FirstOrDefault().OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote)
            {
                action = "LabManageOrders";
            }
            var isEmail = true;
            var emailNum = 1;
            var emails = new List<string>();
            while (isEmail)
            {
                var emailName = AppData.SessionExtensions.SessionNames.Email.ToString() + emailNum;
                var email = HttpContext.Session.GetObject<string>(emailName);
                if (email!=null)
                {
                    emails.Add(email);
                }
                else
                {
                    isEmail = false;
                }
                emailNum++;
            }

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
                string vendorEmail = /*firstRequest.Product.Vendor.OrdersEmail;*/ emails.Count()<1 ? requests.FirstOrDefault().Product.Vendor.OrdersEmail : emails[0];
                string vendorName = requests.FirstOrDefault().Product.Vendor.VendorEnName;

                //add a "From" Email
                message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

                // add a "To" Email
                message.To.Add(new MailboxAddress(vendorName, vendorEmail));
                if (emails.Count>1 )
                {
                    message.Cc.Add(new MailboxAddress(emails[1]));
                }
                if (emails.Count > 2)
                {
                    message.Cc.Add(new MailboxAddress(emails[2]));
                }
                if (emails.Count > 3)
                {
                    message.Cc.Add(new MailboxAddress(emails[3]));
                }
                if (emails.Count > 5)
                {
                    message.Cc.Add(new MailboxAddress(emails[4]));
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


                    /*
                     * SAVE THE INFORMATION HERE
                     */
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {                            

                            try
                            {
                                client.Send(message);
                                wasSent = true;
                            }
                            catch (Exception ex)
                            {
                                ViewBag.ErrorMessage = ex.InnerException?.ToString();
                            }
                            client.Disconnect(true);

                            if (wasSent)
                            {
                                foreach (var r in requests)
                                {
                                    r.RequestStatusID = 2;
                                    confirmEmailViewModel.RequestIndexObject.RequestStatusID = 2;
                                    //remove all includes
                                    r.Product.ProductSubcategory = null;
                                    r.Product.Vendor = null;

                                    if(r.OrderType != AppUtility.OrderTypeEnum.OrderNow)
                                    {
                                        r.Product = null;
                                    }

                                    _context.Update(r);
                                    await _context.SaveChangesAsync();
                                }


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
                                        comment.RequestID = requests.FirstOrDefault().RequestID;
                                        _context.Add(comment);
                                    }
                                    else
                                    {
                                        commentExists = false;
                                    }
                                } while (commentExists);
                                await _context.SaveChangesAsync();
                                if (requests.FirstOrDefault().OrderType == AppUtility.OrderTypeEnum.OrderNow)
                                {
                                    MoveDocumentsOutOfTempFolder(requests.FirstOrDefault());
                                }
                      
                                //save the document
                                foreach (var request in requests)
                                {

                                    string NewFolder = Path.Combine(uploadFolder, request.RequestID.ToString());
                                    string folderPath = Path.Combine(NewFolder, AppUtility.RequestFolderNamesEnum.Orders.ToString());
                                    Directory.CreateDirectory(folderPath); //make sure we don't need one above also??

                                    string uniqueFileName = 1 + "OrderEmail.pdf";
                                    string filePath = Path.Combine(folderPath, uniqueFileName);
                                    if(System.IO.File.Exists(filePath))
                                    {
                                        System.IO.File.Delete(filePath);
                                    }

                                    System.IO.File.Copy(uploadFile, filePath); //make sure this works for each of them

                                    request.Product = await _context.Products.Where(p => p.ProductID == request.ProductID).Include(p=>p.Vendor).FirstOrDefaultAsync();
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
                                    requestNotification.Vendor =request.Product.Vendor.VendorEnName;
                                    _context.Update(requestNotification);
                                }
                                await _context.SaveChangesAsync();

                            }
                            await transaction.CommitAsync();
                            
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            base.RemoveRequestSessions();
                            ViewBag.ErrorMessage = e.InnerException;
                        }

                    }
                    /*
                     * END SAVE THE INFORMATION HERE
                     */

                }


                return RedirectToAction(action, confirmEmailViewModel.RequestIndexObject);

            }
            return RedirectToAction(action, confirmEmailViewModel.RequestIndexObject);

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
            List<Request> requests;
            if (confirmQuoteEmail.IsResend)
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Where(r => r.RequestID == confirmQuoteEmail.RequestID)
           .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote).ToList();
            }
            else
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Where(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.ParentQuote.QuoteStatusID == 1)
                         .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote).ToList();
            }
            if (requests.Count() == 0)
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Where(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.ParentQuote.QuoteStatusID == 2)
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
                return RedirectToAction("LabManageQuotes");
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
            List<Request> requests;
            if (isResend)
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Where(r => r.RequestID == id)
               .Include(r => r.Product).ThenInclude(r => r.Vendor)
               .ToList();
            }
            else
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Where(r => r.Product.VendorID == id && r.ParentQuote.QuoteStatusID == 1)
                         .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote).ToList();
            }
            if (requests.Count() == 0)
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Where(r => r.Product.VendorID == id && r.ParentQuote.QuoteStatusID == 2)
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


        /*LABMANAGEMENT*/
        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> LabManageQuotes()
        {
            LabManageQuotesViewModel labManageQuotesViewModel = new LabManageQuotesViewModel();
            labManageQuotesViewModel.RequestsByVendor = _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1).Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Where(r => (r.ParentQuote.QuoteStatusID == 1 || r.ParentQuote.QuoteStatusID == 2) && r.RequestStatusID==6)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                .Include(r => r.ParentQuote).Include(r => r.ApplicationUserCreator)
                .ToLookup(r => r.Product.Vendor);
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementQuotes;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Quotes;
            return View(labManageQuotesViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> LabManageOrders( RequestIndexObject requestIndexObject)
        {
            LabManageQuotesViewModel labManageQuotesViewModel = new LabManageQuotesViewModel();
            labManageQuotesViewModel.RequestsByVendor = _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1).Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Where(r => r.ParentQuote.QuoteStatusID == 4 && r.RequestStatusID==6)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.ApplicationUserCreator)
                .ToLookup(r => r.Product.Vendor);
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementQuotes;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Orders;
            labManageQuotesViewModel.RequestIndexObjext = requestIndexObject;
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
            if (ProductID != null && _context.Requests.Where(r => r.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID && r.ProductID != ProductID).Any())
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
        [Authorize(Roles = "Requests, Operations")]
        public IActionResult Approve(int id, RequestIndexObject requestIndex)
        {
            var request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.ParentQuote).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(px => px.ParentCategory).Include(r => r.Product.Vendor).FirstOrDefault();
            try
            {
                request.RequestStatusID = 6; //approved
                if (request.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote)
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
                var requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Where(r => r.RequestID == requestID)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.ParentQuote)
                    .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).ToList();
                var vendor = _context.Vendors.Where(v => v.VendorID == id).FirstOrDefault();
                EditQuoteDetailsViewModel editQuoteDetailsViewModel = new EditQuoteDetailsViewModel()
                {
                    Requests = requests,
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
                var requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote)
              .Where(r => r.Product.VendorID == id && (r.ParentQuote.QuoteStatusID == 2 || r.ParentQuote.QuoteStatusID == 1))
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
                var requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote).Include(x => x.ParentQuote).Select(r => r);
                var quoteDate = editQuoteDetailsViewModel.QuoteDate;
                var quoteNumber = editQuoteDetailsViewModel.QuoteNumber;
                foreach (var quote in editQuoteDetailsViewModel.Requests)
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
                .Where(r => r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.AddToCart)
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
            var user = _context.Users.Where(u => u.Id == request.ApplicationUserCreatorID).FirstOrDefault();
            DateTime firstOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
            { //lab
                var pricePerUnit = request.Cost / request.Unit;
                if (pricePerUnit >user.LabUnitLimit)
                {
                    return false;
                }
                if (request.Cost > user.LabOrderLimit)
                {
                    return false;
                }
                var monthsSpending = _context.Requests
                      .Where(r => request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                      .Where(r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID && r.Product.VendorID == request.Product.VendorID)
                      .Where(r => r.ParentRequest.OrderDate >= firstOfMonth)
                      .Sum(r => r.Cost);
                if (monthsSpending + request.Cost > user.LabMonthlyLimit)
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
         *NEW Accounting VIEW
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
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadQuoteModal(RequestIndexObject requestIndexObject)
        {       

            var UploadQuoteViewModel = new UploadQuoteViewModel() {RequestIndexObject = requestIndexObject };

            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string uploadFolder2 = Path.Combine(uploadFolder1, "0");
            string uploadFolderQuotes = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Quotes.ToString());

            if (Directory.Exists(uploadFolderQuotes))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderQuotes);
                //searching for the partial file name in the directory
                FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                UploadQuoteViewModel.FileStrings = new List<String>();
                foreach (var orderfile in orderfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                    UploadQuoteViewModel.FileStrings.Add(newFileString);
                }
            }
            return PartialView(UploadQuoteViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadOrderModal(RequestIndexObject requestIndexObject)
        {
           
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
            var UploadQuoteViewModel = new UploadOrderViewModel() { ParentRequest = pr, RequestIndexObject = requestIndexObject };

            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string uploadFolder2 = Path.Combine(uploadFolder1, "0");
            string uploadFolderOrders = Path.Combine(uploadFolder2, AppUtility.RequestFolderNamesEnum.Orders.ToString());

            if (Directory.Exists(uploadFolderOrders))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderOrders);
                //searching for the partial file name in the directory
                FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                UploadQuoteViewModel.FileStrings = new List<String>();
                foreach (var orderfile in orderfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                    UploadQuoteViewModel.FileStrings.Add(newFileString);
                }
            }

            return PartialView(UploadQuoteViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadQuoteModal(UploadQuoteViewModel uploadQuoteOrderViewModel)
        {
            var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
            var request = HttpContext.Session.GetObject<Request>(requestName);
            uploadQuoteOrderViewModel.ParentQuote.QuoteStatusID = 4;
            request.ParentQuote = uploadQuoteOrderViewModel.ParentQuote;
            if (request.RequestStatusID == 6  && request.OrderType!=AppUtility.OrderTypeEnum.AddToCart )
            {
                uploadQuoteOrderViewModel.RequestIndexObject.OrderStep = AppUtility.OrderStepsEnum.TermsModal;
                var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                HttpContext.Session.SetObject(requestNum, request);
            }
            else
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Update(request);
                        await _context.SaveChangesAsync();
                        await SaveCommentsFromSession(request);
                        //rename temp folder to the request id
                        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                        string requestFolderFrom = Path.Combine(uploadFolder, "0");
                        string requestFolderTo = Path.Combine(uploadFolder, request.RequestID.ToString());
                        if (Directory.Exists(requestFolderTo))
                        {
                            Directory.Delete(requestFolderTo);
                        }
                        Directory.Move(requestFolderFrom, requestFolderTo);

                        await transaction.CommitAsync();
                        base.RemoveRequestSessions();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        base.RemoveRequestSessions();
                        ViewBag.ErrorMessage = ex.InnerException;
                    }
                }
            }
            
            return RedirectToAction("Index", uploadQuoteOrderViewModel.RequestIndexObject);
        }

        private async Task SaveCommentsFromSession(Request request)
        {
            try
            {
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
                        comment.RequestID = request.RequestID;
                        _context.Add(comment);
                    }
                    else
                    {
                        commentExists = false;
                    }
                } while (commentExists);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<object> UploadOrderModal(UploadOrderViewModel uploadQuoteOrderViewModel) {
            var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
            var request = HttpContext.Session.GetObject<Request>(requestName);
            request.ParentRequest = uploadQuoteOrderViewModel.ParentRequest;
            request.ParentQuote = null;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    _context.Update(request);
                    await _context.SaveChangesAsync();
                    await SaveCommentsFromSession(request);
                    //rename temp folder to the request id
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    string requestFolderFrom = Path.Combine(uploadFolder, "0");
                    string requestFolderTo = Path.Combine(uploadFolder, request.RequestID.ToString());
                    if (Directory.Exists(requestFolderTo))
                    {
                        Directory.Delete(requestFolderTo);
                    }
                    Directory.Move(requestFolderFrom, requestFolderTo);

                    await transaction.CommitAsync();
                    base.RemoveRequestSessions();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    base.RemoveRequestSessions();
                    ViewBag.ErrorMessage = ex.InnerException;
                }
            }
            return RedirectToAction("Index", uploadQuoteOrderViewModel.RequestIndexObject);
        }

        private async Task AddItemAccordingToOrderType(Request newRequest, AppUtility.OrderTypeEnum OrderTypeEnum, bool isInBudget)
        {
   
            var context = new ValidationContext(newRequest, null, null);
            var results = new List<ValidationResult>();
            var validatorCreate = Validator.TryValidateObject(newRequest, context, results, true);
            if (validatorCreate)
            {
                try
                {
                    switch (OrderTypeEnum)
                    {
                        case AppUtility.OrderTypeEnum.AddToCart:
                            await AddToCart(newRequest, isInBudget);
                            break;
                        case AppUtility.OrderTypeEnum.AlreadyPurchased:
                            AlreadyPurchased(newRequest);
                            break;
                        case AppUtility.OrderTypeEnum.OrderNow:
                            OrderNow(newRequest, isInBudget);
                            break;
                        case AppUtility.OrderTypeEnum.RequestPriceQuote:
                            await RequestItem(newRequest, isInBudget);
                            break;
                    }

                }
                catch (DbUpdateException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        private async Task<bool> RequestItem(Request newRequest, bool isInBudget)
        {

            try
            {
                if (isInBudget)
                {
                    newRequest.RequestStatusID = 6;
                }
                else
                {
                    newRequest.RequestStatusID = 1;
                }
                newRequest.ParentQuote = new ParentQuote();
                newRequest.ParentQuote.QuoteStatusID = 1;
                newRequest.OrderType = AppUtility.OrderTypeEnum.RequestPriceQuote;
                _context.Add(newRequest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        private async Task<bool> AddToCart(Request request, bool isInBudget)
        {
            try
            {
                if (isInBudget)
                {
                    request.RequestStatusID = 6;
                }
                else
                {
                    request.RequestStatusID = 1;
                }
                request.OrderType = AppUtility.OrderTypeEnum.AddToCart;
                var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                HttpContext.Session.SetObject(requestNum, request);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AlreadyPurchased(Request request)
        {
            try
            {
                request.RequestStatusID = 2;
                request.ParentQuoteID = null;
                request.OrderType = AppUtility.OrderTypeEnum.AlreadyPurchased;
                var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                HttpContext.Session.SetObject(requestNum, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OrderNow(Request request, bool isInBudget)
        {
            try
            {
                if (isInBudget)
                {
                    request.RequestStatusID = 6;
                }
                else
                {
                    request.RequestStatusID = 1;
                }
                request.OrderType = AppUtility.OrderTypeEnum.OrderNow;
                var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                HttpContext.Session.SetObject(requestNum, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void MoveDocumentsOutOfTempFolder(Request request)
        {
            //rename temp folder to the request id
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string requestFolderFrom = Path.Combine(uploadFolder, "0");
            string requestFolderTo = Path.Combine(uploadFolder, request.RequestID.ToString());
            if (Directory.Exists(requestFolderFrom))
            {
                if (Directory.Exists(requestFolderTo))
                {
                    Directory.Delete(requestFolderTo);
                }
                Directory.Move(requestFolderFrom, requestFolderTo);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> TermsModal(int vendorID, RequestIndexObject requestIndexObject) //either it'll be a request or parentrequest and then it'll send it to all the requests in that parent request
        {
            List<Request> requests = new List<Request>();
            if (vendorID != 0)
            {
                if(requestIndexObject.SidebarType == AppUtility.SidebarEnum.Cart)
                {
                    requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
          .Where(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.AddToCart && r.ParentQuote.QuoteStatusID == 4)
          .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
                .Include(r => r.Product).ThenInclude(r => r.Vendor)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
                }
                else if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Orders)
                {
                    requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
          .Where(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote && r.ParentQuote.QuoteStatusID==4)
          .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
                .Include(r => r.Product).ThenInclude(r => r.Vendor)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
                }
  
            }
            else
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
            }
            var requestNum = 1;
            foreach (var req in requests)
            {
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + requestNum;
                HttpContext.Session.SetObject(requestName, req);
                requestNum++;
            }

            TermsViewModel termsViewModel = new TermsViewModel()
            {
                ParentRequest = new ParentRequest(),
                TermsList = new List<SelectListItem>()
                {
                    new SelectListItem{ Text="Pay Now", Value=AppUtility.TermsModalEnum.PayNow.ToString()},
                    new SelectListItem{ Text="+30", Value=AppUtility.TermsModalEnum.PayWithInMonth.ToString()},
                    new SelectListItem{ Text="Installements", Value=AppUtility.TermsModalEnum.Installments.ToString()},
                    new SelectListItem{ Text="Paid", Value=AppUtility.TermsModalEnum.Paid.ToString()}
                }
            };
            termsViewModel.RequestIndexObject = requestIndexObject;
            termsViewModel.RequestIndexObject.PageType = AppUtility.PageTypeEnum.RequestRequest;
            if (termsViewModel.RequestIndexObject.SidebarType == AppUtility.SidebarEnum.Cart)
            {
                termsViewModel.RequestIndexObject.SidebarType = AppUtility.SidebarEnum.List;
            }
            return PartialView(termsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> TermsModal(TermsViewModel termsViewModel)
        {
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


            var paymentStatusID = 0;

            switch (termsViewModel.Terms)
            {
                case AppUtility.TermsModalEnum.PayNow:
                    paymentStatusID = 3;
                    break;
                case AppUtility.TermsModalEnum.PayWithInMonth:
                    paymentStatusID = 1;
                    break;
                case AppUtility.TermsModalEnum.Installments:
                    paymentStatusID = 5;
                    break;
                case AppUtility.TermsModalEnum.Paid:
                    paymentStatusID = 6;
                    break;
            }
            foreach (var req in requests)
            {
                req.ParentRequest = termsViewModel.ParentRequest;
                req.PaymentStatusID = paymentStatusID;
                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                HttpContext.Session.SetObject(requestName, req);
                RequestNum++;
            }
            termsViewModel.RequestIndexObject.OrderStep = AppUtility.OrderStepsEnum.ConfirmEmail;
            var action = "Index";
            if(requests.FirstOrDefault().OrderType==AppUtility.OrderTypeEnum.RequestPriceQuote)
            {
                action = "LabManageOrders";
            }
            return RedirectToAction(action, termsViewModel.RequestIndexObject);
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


    }


}
