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
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.AppData.Exceptions;
using System.Drawing;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text;
using LinqToExcel;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
//using Org.BouncyCastle.Asn1.X509;
//using System.Data.Entity.Validation;f
//using System.Data.Entity.Infrastructure;

namespace PrototypeWithAuth.Controllers
{
    public class RequestsController : SharedController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private ISession _session;
        private ICompositeViewEngine _viewEngine;

        public RequestsController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
            //_Context = Context;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            //use the hosting environment for the file uploads
            _hostingEnvironment = hostingEnvironment;
            //_viewEngine = viewEngine;
        }

        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests, Operations")]
        // GET: Requests
        public async Task<IActionResult> Index(RequestIndexObject requestIndexObject, RequestsSearchViewModel requestsSearchViewModel)
        {

            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;

            if (await base.IsAuthorizedAsync(requestIndexObject.SectionType))
            {
                var viewmodel = await base.GetIndexViewModel(requestIndexObject, requestsSearchViewModel: requestsSearchViewModel);

                // SetViewModelCounts(requestIndexObject, viewmodel);
                return View(viewmodel);
            }
            else
            {
                return Redirect(base.AccessDeniedPath);
            }

        }


        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests, Operations")]
        public async Task<IActionResult> IndexInventory(RequestIndexObject requestIndexObject, RequestsSearchViewModel requestsSearchViewModel)
        {
            if (await base.IsAuthorizedAsync(requestIndexObject.SectionType))
            {

                TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;

                var viewmodel = await base.GetIndexViewModel(requestIndexObject, requestsSearchViewModel: requestsSearchViewModel);
                //  SetViewModelProprietaryCounts(requestIndexObject, viewmodel);
                //viewmodel.InventoryFilterViewModel = GetInventoryFilterViewModel();

                if (ViewBag.ErrorMessage != null)
                {
                    ViewBag.ErrorMessage = ViewBag.ErrorMessage;
                }

                return View(viewmodel);
            }
            else
            {
                return Redirect(base.AccessDeniedPath);
            }
        }



        [Authorize(Roles = "Requests, LabManagement, Operations")]
        private async Task<RequestIndexPartialViewModelByVendor> GetIndexViewModelByVendor(RequestIndexObject requestIndexObject, NotificationFilterViewModel notificationFilterViewModel = null)
        {
            RequestIndexPartialViewModelByVendor viewModelByVendor = new RequestIndexPartialViewModelByVendor();
            if (!requestIndexObject.CategorySelected && !requestIndexObject.SubcategorySelected)
            {
                requestIndexObject.SubcategorySelected = true;
            }
            viewModelByVendor.CategoryPopoverViewModel = new CategoryPopoverViewModel()
            {
                SelectedCategoryOption = new List<bool>()
                {
                    requestIndexObject.CategorySelected,
                    requestIndexObject.SubcategorySelected,
                    requestIndexObject.SourceSelected
                }
            };
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var editQuoteDetailsIcon = new IconColumnViewModel(" icon-monetization_on-24px ", "var(--lab-man-color)", "load-quote-details", "Upload Quote");
            var payNowIcon = new IconColumnViewModel(" icon-monetization_on-24px green-overlay ", "", "pay-one", "Pay");
            var addInvoiceIcon = new IconColumnViewModel(" icon-cancel_presentation-24px  green-overlay ", "", "invoice-add-one", "Add Invoice");

            var deleteIcon = new IconColumnViewModel(" icon-delete-24px ", "black", "/DeleteModal", "Delete");
            var favoriteIcon = new IconColumnViewModel(" icon-favorite_border-24px", "var(--order-inv-color)", "request-favorite", "Favorite");
            var popoverMoreIcon = new IconColumnViewModel("icon-more_vert-24px", "black", "popover-more", "More");
            var popoverPartialClarifyIcon = new IconColumnViewModel("Clarify");
            var resendIcon = new IconColumnViewModel("Resend");
            string checkboxString = "Checkbox";
            string buttonText = "";
            var defaultImage = "/images/css/CategoryImages/placeholder.png";

            List<Expression<Func<Request, bool>>> wheres = new List<Expression<Func<Request, bool>>>();
            List<ComplexIncludes<Request, ModelBase>> includes = new List<ComplexIncludes<Request, ModelBase>>();
            Expression<Func<Request, RequestIndexPartialRowViewModel>> select = null;
            Func<RequestPaymentsViewModel, RequestIndexPartialRowViewModel> selectForPayments = null;
            Expression<Func<Request, DateTime>> orderby = null;
            Func<RequestPaymentsViewModel, DateTime> orderbyForPayments = null;
            //generic includes
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((Product)p).Vendor } });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((ProductSubcategory)p).ParentCategory } });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.UnitType });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.SubUnitType });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.SubSubUnitType });
            switch (requestIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.LabManagementQuotes:
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.Orders:
                            wheres.Add(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1);
                            wheres.Add(r => r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.RequestPriceQuote.ToString());
                            wheres.Add(r => r.QuoteStatusID == 4 && r.RequestStatusID == 6);
                            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ApplicationUserCreator });
                            select = r => new RequestIndexPartialRowViewModel
                             (AppUtility.IndexTableTypes.LabOrders, r, r.OrderMethod, r.ApplicationUserCreator, r.Product, r.Product.Vendor, r.Product.ProductSubcategory,
                        r.Product.ProductSubcategory.ParentCategory, r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, checkboxString)

                            {
                                ButtonClasses = " load-terms-modal ",
                                ButtonText = "Order"
                            };
                            orderby = r => r.CreationDate;
                            iconList.Add(deleteIcon);

                            break;
                        case AppUtility.SidebarEnum.Quotes:
                            wheres.Add(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1);
                            wheres.Add(r => r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.RequestPriceQuote.ToString());
                            wheres.Add(r => (r.QuoteStatusID == 1 || r.QuoteStatusID == 2) && r.RequestStatusID == 6);
                            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentQuote });
                            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ApplicationUserCreator });
                            iconList.Add(resendIcon);
                            iconList.Add(editQuoteDetailsIcon);
                            iconList.Add(deleteIcon);
                            orderby = r => r.CreationDate;
                            select = r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.LabQuotes, r, r.OrderMethod, r.ApplicationUserCreator, r.Product, r.Product.Vendor, r.Product.ProductSubcategory,
                        r.Product.ProductSubcategory.ParentCategory, r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, checkboxString, r.ParentQuote)

                            {
                                ButtonClasses = " confirm-quote  ",
                                ButtonText = "Ask For Quote"
                            };
                            break;
                    }
                    break;
                case AppUtility.PageTypeEnum.AccountingNotifications:
                    wheres.Add(r => r.RequestStatusID != 7);
                    includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentRequest });
                    includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Payments, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((Payment)p).Invoice } });
                    includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((ProductSubcategory)p).ParentCategory, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = pc => ((ParentCategory)pc).CategoryType } } });

            if (notificationFilterViewModel == null)
                    {
                        notificationFilterViewModel = new NotificationFilterViewModel() { Vendors = _vendorsProc.Read(new List<Expression<Func<Vendor, bool>>> { v => v.VendorCategoryTypes.Select(v => v.CategoryTypeID).Contains(1) }).ToList() };
                    }
                    else
                    {
                        wheres.Add(r => (notificationFilterViewModel.SelectedVendor == null || r.Product.VendorID == notificationFilterViewModel.SelectedVendor)
                        && (notificationFilterViewModel.NameOrCentarixOrderNumber == null || r.Product.ProductName.ToLower().Contains(notificationFilterViewModel.NameOrCentarixOrderNumber.ToLower())
                        || r.ParentRequest.OrderNumber.ToString().Contains(notificationFilterViewModel.NameOrCentarixOrderNumber)));
                    }

                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.DidntArrive:
                            wheres.Add(r => r.RequestStatusID == 2);
                            wheres.Add(r => r.ExpectedSupplyDays != null);
                            wheres.Add(r => r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0).Date < DateTime.Today);
                            checkboxString = "";
                            break;
                        case AppUtility.SidebarEnum.PartialDelivery:
                            wheres.Add(r => r.RequestStatusID == 2 && r.IsPartial);
                            checkboxString = "";
                            break;
                        case AppUtility.SidebarEnum.ForClarification:
                            wheres.Add(r => r.IsClarify);
                            checkboxString = "";
                            iconList.Add(popoverPartialClarifyIcon);
                            break;
                        case AppUtility.SidebarEnum.NoInvoice:
                            wheres.Add(r => r.Payments.FirstOrDefault().HasInvoice == false);
                            wheres.Add(r => (r.PaymentStatusID == 2/*+30*/ && r.RequestStatusID == 3) || (r.PaymentStatusID == 3/*pay now*/) || (r.PaymentStatusID == 8/*specify payment*/ && r.RequestStatusID == 3) || (r.PaymentStatusID == 5/*installments*/));
                            iconList.Add(addInvoiceIcon);
                            buttonText = "Add To All";
                            break;
                    }
                    orderby = r => r.ParentRequest.OrderDate;
                    select = r => new RequestIndexPartialRowViewModel
                    (AppUtility.IndexTableTypes.AccountingNotifications, r, r.OrderMethod, r.Product, r.Product.Vendor, r.Product.ProductSubcategory,
                        r.Product.ProductSubcategory.ParentCategory, r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, checkboxString, new Request())
                    {
                        ButtonClasses = " invoice-add-all ",
                        ButtonText = buttonText
                    };
                    break;
                case AppUtility.PageTypeEnum.AccountingPayments:
                    if (notificationFilterViewModel == null)
                    {
                        notificationFilterViewModel = new NotificationFilterViewModel() { Vendors = _vendorsProc.Read(new List<Expression<Func<Vendor, bool>>> { v => v.VendorCategoryTypes.Select(v => v.CategoryTypeID).Contains(1) }).ToList() };
                    }
                    else
                    {
                        wheres.Add(r => (notificationFilterViewModel.SelectedVendor == null || r.Product.VendorID == notificationFilterViewModel.SelectedVendor)
                        && (notificationFilterViewModel.NameOrCentarixOrderNumber == null || r.Product.ProductName.ToLower().Contains(notificationFilterViewModel.NameOrCentarixOrderNumber.ToLower())
                        || r.ParentRequest.OrderNumber.ToString().Contains(notificationFilterViewModel.NameOrCentarixOrderNumber)));
                    }
                    var paymentList = await GetPaymentRequests(requestIndexObject.SidebarType, wheres);
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.Installments:
                        case AppUtility.SidebarEnum.StandingOrders:
                            payNowIcon = new IconColumnViewModel(" icon-monetization_on-24px green-overlay ", "", "pay-invoice-one", "Pay");
                            checkboxString = "";
                            iconList.Add(payNowIcon);

                            orderbyForPayments = r => r.Request.ParentRequest.OrderDate;
                            selectForPayments = r => new RequestIndexPartialRowViewModel
                             (AppUtility.IndexTableTypes.AccountingPaymentsInstallments, r.Request, r.Request.OrderMethod, r.Request.Product, r.Request.Product.Vendor, r.Request.Product.ProductSubcategory,
                                r.Request.Product.ProductSubcategory.ParentCategory, r.Request.Product.UnitType, r.Request.Product.SubUnitType, r.Request.Product.SubSubUnitType, requestIndexObject, iconList,
                                defaultImage, r.Request.ParentRequest, checkboxString, new List<Payment>() { r.Payment })

                            {
                                ButtonText = "",
                            };
                            break;
                        default:
                            iconList.Add(payNowIcon);
                            iconList.Add(popoverMoreIcon);
                            orderbyForPayments = r => r.Request.ParentRequest.OrderDate;
                            selectForPayments = r => new RequestIndexPartialRowViewModel
                             (AppUtility.IndexTableTypes.AccountingPaymentsDefault, r.Request, r.Request.OrderMethod, r.Request.Product, r.Request.Product.Vendor, r.Request.Product.ProductSubcategory,
                        r.Request.Product.ProductSubcategory.ParentCategory, r.Request.Product.UnitType, r.Request.Product.SubUnitType, r.Request.Product.SubSubUnitType, requestIndexObject, iconList,
                        defaultImage, r.Request.ParentRequest, checkboxString, new List<Payment>() { r.Payment })

                            {
                                ButtonClasses = " payments-pay-now ",
                                ButtonText = "Pay All"
                            };
                            buttonText = "Pay All";
                            break;
                    }
                    popoverMoreIcon.IconPopovers = AppUtility.GetPaymentsPopoverLinks(requestIndexObject.SidebarType);
                    viewModelByVendor.RequestsByVendor = paymentList.OrderByDescending(orderbyForPayments).Select(selectForPayments).ToLookup(r => r.Vendor);
                    break;
                case AppUtility.PageTypeEnum.RequestCart:
                    wheres.Add(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User));
                    wheres.Add(r => r.RequestStatusID == 6 && r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.AddToCart.ToString());
                    wheres.Add(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1);
                    includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ApplicationUserCreator });
                    iconList.Add(deleteIcon);
                    orderby = r => r.CreationDate;
                    select = r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Cart, r, r.OrderMethod, r.ApplicationUserCreator, r.Product, r.Product.Vendor, r.Product.ProductSubcategory,
                        r.Product.ProductSubcategory.ParentCategory, r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, checkboxString)
                    {
                        ButtonClasses = " load-terms-modal  ",
                        ButtonText = "Order",
                    };
                    break;
                case AppUtility.PageTypeEnum.OperationsCart:
                    wheres.Add(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User));
                    wheres.Add(r => r.RequestStatusID == 6 && r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.AddToCart.ToString());
                    wheres.Add(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2);
                    wheres.Add(r => r.OccurenceNumber == 1);
                    includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ApplicationUserCreator });
                    iconList.Add(deleteIcon);
                    orderby = r => r.CreationDate;
                    select = r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.CartOperations, r, r.OrderMethod, r.ApplicationUserCreator, r.Product, r.Product.Vendor, r.Product.ProductSubcategory,
                        r.Product.ProductSubcategory.ParentCategory, r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, checkboxString)
                    {
                        ButtonClasses = " load-terms-modal  ",
                        ButtonText = "Order",
                    };
                    break;

            }
            if (requestIndexObject.PageType != AppUtility.PageTypeEnum.AccountingPayments)
            {
                viewModelByVendor.RequestsByVendor = _requestsProc.Read(wheres, includes).OrderByDescending(orderby).Select(select).ToLookup(c => c.Vendor);
            }
            List<PriceSortViewModel> priceSorts = new List<PriceSortViewModel>();
            Enum.GetValues(typeof(AppUtility.PriceSortEnum)).Cast<AppUtility.PriceSortEnum>().ToList().ForEach(p => priceSorts.Add(new PriceSortViewModel { PriceSortEnum = p, Selected = requestIndexObject.SelectedPriceSort.Contains(p.ToString()) }));
            viewModelByVendor.NotificationFilterViewModel = notificationFilterViewModel;
            viewModelByVendor.PricePopoverViewModel = new PricePopoverViewModel() { };
            viewModelByVendor.PricePopoverViewModel.PriceSortEnums = priceSorts;
            viewModelByVendor.PricePopoverViewModel.SelectedCurrency = requestIndexObject.SelectedCurrency;
            viewModelByVendor.PricePopoverViewModel.PopoverSource = 1;
            viewModelByVendor.PageType = requestIndexObject.PageType;
            viewModelByVendor.SidebarType = requestIndexObject.SidebarType;
            viewModelByVendor.ErrorMessage = requestIndexObject.ErrorMessage;
            viewModelByVendor.InventoryFilterViewModel = GetInventoryFilterViewModel();
            return viewModelByVendor;
        }

        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests, Operations")] //redo this later
        public async Task<IActionResult> _IndexTableWithCounts(RequestIndexObject requestIndexObject, RequestsSearchViewModel requestsSearchViewModel, SelectedRequestFilters selectedFilters = null, int numFilters = 0)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject, selectedFilters: selectedFilters, numFilters: numFilters, requestsSearchViewModel: requestsSearchViewModel);
            return PartialView(viewModel);
        }


        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTable(RequestIndexObject requestIndexObject, List<int> months, List<int> years, RequestsSearchViewModel requestsSearchViewModel, SelectedRequestFilters selectedFilters = null, int numFilters = 0)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject, months, years, selectedFilters, numFilters, requestsSearchViewModel);
            return PartialView(viewModel);
        }


        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTableWithProprietaryTabs(RequestIndexObject requestIndexObject, List<int> months, List<int> years, RequestsSearchViewModel requestsSearchViewModel, SelectedRequestFilters selectedFilters = null, int numFilters = 0)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject, months, years, selectedFilters, numFilters, requestsSearchViewModel: requestsSearchViewModel);
            return PartialView(viewModel);
        }


        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTableData(RequestIndexObject requestIndexObject, List<int> months, List<int> years, RequestsSearchViewModel requestsSearchViewModel, SelectedRequestFilters selectedFilters = null)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject, months, years, selectedFilters, requestsSearchViewModel: requestsSearchViewModel);

            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexSharedTable()
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            RequestIndexPartialViewModel viewModel = await GetSharedRequestIndexObjectAsync();
            return PartialView(viewModel);
        }

        [Authorize(Roles = "Requests")]
        public async Task<RequestIndexPartialViewModel> GetSharedRequestIndexObjectAsync()
        {

            RequestIndexObject requestIndexObject = new RequestIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = AppUtility.SidebarEnum.SharedRequests
            };
            RequestIndexPartialViewModel viewModel = await base.GetIndexViewModel(requestIndexObject);
            return viewModel;
        }

        [Authorize(Roles = "Requests")]
        public async Task<RequestListIndexViewModel> GetRequestListIndexObjectAsync(RequestIndexObject requestIndexObject)
        {
            var userLists = _requestListsProc.Read(new List<Expression<Func<RequestList, bool>>> { l => l.ApplicationUserOwnerID == _userManager.GetUserId(User) }).OrderBy(l => l.DateCreated).ToList();

            if (userLists.Count == 0)
            {
                RequestList requestList = await _requestListsProc.CreateAndGetDefaultListAsync(_userManager.GetUserId(User));
                requestIndexObject.ListID = requestList.ListID;
                userLists.Add(requestList);
            }

            if (requestIndexObject.ListID == 0)
            {
                requestIndexObject.ListID = userLists.Where(l => l.IsDefault).FirstOrDefault().ListID;
            }
            RequestIndexPartialViewModel requestIndexViewModel = await base.GetIndexViewModel(requestIndexObject);
            RequestListIndexViewModel viewModel = new RequestListIndexViewModel
            {
                RequestIndexPartialViewModel = requestIndexViewModel,
                ListID = requestIndexObject.ListID,
                Lists = userLists,
                SidebarType = AppUtility.SidebarEnum.MyLists
            };
            return viewModel;
        }



        [Authorize(Roles = "Requests")]
        public async Task<RequestListIndexViewModel> GetSharedRequestListIndexObjectAsync(RequestIndexObject requestIndexObject)
        {
            var userLists = _shareRequestListsProc.Read(new List<Expression<Func<ShareRequestList, bool>>> { l => l.ToApplicationUserID == _userManager.GetUserId(User) }, new List<ComplexIncludes<ShareRequestList, ModelBase>> { new ComplexIncludes<ShareRequestList, ModelBase> { Include = l => l.RequestList } }).OrderBy(l => l.TimeStamp).Select(l => l.RequestList).ToList();
            if (userLists.Count > 0 && requestIndexObject.ListID == 0)
            {
                requestIndexObject.ListID = userLists.FirstOrDefault().ListID;
            }
            RequestIndexPartialViewModel requestIndexViewModel = await base.GetIndexViewModel(requestIndexObject);
            RequestListIndexViewModel viewModel = new RequestListIndexViewModel
            {
                RequestIndexPartialViewModel = requestIndexViewModel,
                ListID = requestIndexObject.ListID,
                Lists = userLists,
                SidebarType = AppUtility.SidebarEnum.SharedLists
            };

            return viewModel;
        }

        [Authorize(Roles = "Requests,Operations")]
        public async Task<TempRequestListViewModel> SaveAddItemView(RequestItemViewModel requestItemViewModel, AppUtility.OrderMethod OrderMethod, ReceivedModalVisualViewModel receivedModalVisualViewModel = null)
        {
            var trlvm = new TempRequestListViewModel() { TempRequestViewModels = new List<TempRequestViewModel>() };
            try
            {
                switch (requestItemViewModel.OrderType)
                {
                    case AppUtility.OrderType.SingleOrder:
                        requestItemViewModel.Requests[0].Product = requestItemViewModel.Requests[0].SingleOrder;
                        requestItemViewModel.Requests[0].SingleOrder = null;
                        break;
                    case AppUtility.OrderType.RecurringOrder:
                        requestItemViewModel.Requests[0].Product = requestItemViewModel.Requests[0].RecurringOrder;
                        requestItemViewModel.Requests[0].RecurringOrder = null;
                        break;
                    case AppUtility.OrderType.StandingOrder:
                        requestItemViewModel.Requests[0].Product = requestItemViewModel.Requests[0].StandingOrder;
                        requestItemViewModel.Requests[0].StandingOrder = null;
                        break;
                };

                var vendor = await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == requestItemViewModel.Requests.FirstOrDefault().Product.VendorID });
                var exchangeRate = requestItemViewModel.Requests.FirstOrDefault().ExchangeRate;
                var currency = requestItemViewModel.Requests.FirstOrDefault().Currency;
                var orderMethod = await _orderMethodsProc.ReadOneAsync(new List<Expression<Func<OrderMethod, bool>>> { o => o.DescriptionEnum == OrderMethod.ToString() });
                //declared outside the if b/c it's used farther down too 
                var currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == _userManager.GetUserId(User) });

                var RequestNum = 1;
                var i = 1;
                var additionalRequests = false;

                foreach (var request in requestItemViewModel.Requests)
                {
                    //throw new Exception();
                    request.OrderMethod = orderMethod;
                    request.ApplicationUserCreatorID = currentUser.Id;
                    if (!requestItemViewModel.IsProprietary)
                    {
                        request.Product.VendorID = vendor.VendorID;
                        request.Product.Vendor = vendor;
                        request.CreationDate = DateTime.Now;
                    }

                    request.Product.ProductSubcategory = await _productSubcategoriesProc.ReadOneAsync(new List<Expression<Func<ProductSubcategory, bool>>> { ps => ps.ID == request.Product.ProductSubcategoryID }, new List<ComplexIncludes<ProductSubcategory, ModelBase>> { new ComplexIncludes<ProductSubcategory, ModelBase> { Include = ps => ps.ParentCategory } });
                    var isInBudget = false;
                    if (!request.Product.ProductSubcategory.ParentCategory.IsProprietary)
                    {
                        if (request.Currency == null)
                        {
                            request.Currency = AppUtility.CurrencyEnum.NIS.ToString();
                        }
                        isInBudget = await checkIfInBudgetAsync(request);
                    }
                    request.ExchangeRate = exchangeRate;


                    request.Product.ProductName = AppUtility.TrimNewLines(request.Product.ProductName);
                    TempRequestViewModel trvm = await AddItemAccordingToOrderMethod(request, OrderMethod, isInBudget, requestItemViewModel, requestNum: RequestNum, receivedModalVisualViewModel);
                    if (requestItemViewModel.SectionType == AppUtility.MenuItems.Operations && OrderMethod == AppUtility.OrderMethod.AddToCart)
                    {
                        break;
                    }
                    if (requestItemViewModel.Comments != null)
                    {
                        trvm.Comments = new List<CommentBase>();
                        foreach (var comment in requestItemViewModel.Comments)
                        {
                            trvm.Comments.Add(comment);
                        }
                    }

                    trlvm.TempRequestViewModels.Add(trvm);
                    i++;
                }
                //await _tempRequestJsonsProc.UpdateAsync(requestItemViewModel.TempRequestListViewModel.GUID, requestItemViewModel.TempRequestListViewModel.RequestIndexObject, trlvm, currentUser.Id, false); ;
            }

            catch (Exception ex)
            {
                //Redirect Results Need to be checked here
                //requestItemViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                await _tempRequestJsonsProc.RemoveAllAsync(requestItemViewModel.TempRequestListViewModel.GUID, _userManager.GetUserId(User));
                //Response.WriteAsync(ex.Message?.ToString());
                if (requestItemViewModel.RequestStatusID == 7)
                {
                    //return RedirectToAction("IndexInventory", new { ErrorMessage = AppUtility.GetExceptionMessage(ex) });
                }
                /* return new RedirectToActionResult(actionName: "_OrderTab", controllerName: "Requests", routeValues: requestItemViewModel );*/


            }
            requestItemViewModel.TempRequestListViewModel.RequestIndexObject = new RequestIndexObject()
            {
                OrderMethod = OrderMethod,
                SectionType = requestItemViewModel.SectionType
            };
            requestItemViewModel.TempRequestListViewModel.RequestIndexObject.GUID = requestItemViewModel.TempRequestListViewModel.GUID;
            return trlvm;
            //if (requestItemViewModel.SectionType == AppUtility.MenuItems.Requests)
            //{
            //        return trlvm;
            //}
            //else if (requestItemViewModel.SectionType == AppUtility.MenuItems.Operations)
            //{
            //    switch (OrderMethod)
            //    {
            //        case AppUtility.OrderMethod.AlreadyPurchased:
            //            return new RedirectToActionResult("UploadOrderModal", "Requests", requestItemViewModel.TempRequestListViewModel.RequestIndexObject);
            //        case AppUtility.OrderMethod.OrderNow:
            //            return new RedirectToActionResult("UploadQuoteModal", "Requests", requestItemViewModel.TempRequestListViewModel.RequestIndexObject);
            //        case AppUtility.OrderMethod.AddToCart:
            //            await _tempRequestJsonsProc.RemoveAllAsync(requestItemViewModel.TempRequestListViewModel.GUID, _userManager.GetUserId(User));
            //            if (requestItemViewModel.AdditionalRequests)
            //            {
            //                return new RedirectToActionResult("EmptyResult", "Requests", "");
            //            }
            //            else
            //            {
            //                return new RedirectToActionResult("Index", "Requests", new
            //                {
            //                    PageType = requestItemViewModel.PageType,
            //                    SectionType = requestItemViewModel.SectionType,
            //                    SidebarType = AppUtility.SidebarEnum.List,
            //                    RequestStatusID = requestItemViewModel.Requests.FirstOrDefault().RequestStatusID,
            //                });
            //            }
            //        default:
            //            await _tempRequestJsonsProc.RemoveAllAsync(requestItemViewModel.TempRequestListViewModel.GUID, _userManager.GetUserId(User));
            //            return new RedirectToActionResult("Index", "Requests", new
            //            {
            //                PageType = requestItemViewModel.PageType,
            //                SectionType = requestItemViewModel.SectionType,
            //                SidebarType = AppUtility.SidebarEnum.List,
            //                RequestStatusID = requestItemViewModel.Requests.FirstOrDefault().RequestStatusID,
            //            });
            //    }
            //}
            //return new RedirectToActionResult("EmptyResult", "Requests", "");
        }

        protected ActionResult EmptyResult()
        {
            return new EmptyResult();
        }
        protected async Task<bool> checkIfInBudgetAsync(Request request, Product oldProduct = null)
        {
            if (oldProduct == null)
            {
                oldProduct = request.Product;
            }
            var user = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == request.ApplicationUserCreatorID });
            DateTime firstOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (oldProduct.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
            { //lab
                var pricePerUnit = request.Cost / request.Unit;
                if (pricePerUnit > user.LabUnitLimit)
                {
                    return false;
                }
                if (request.TotalWithVat > user.LabOrderLimit)
                {
                    return false;
                }
                var monthsSpending = _requestsProc.Read(new List<Expression<Func<Request, bool>>> {
                      r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1,
                      r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID && r.Product.VendorID == oldProduct.VendorID,
                      r => r.ParentRequest.OrderDate >= firstOfMonth}).AsEnumerable()
                      .Sum(r => r.TotalWithVat);
                if (monthsSpending + request.TotalWithVat > user.LabMonthlyLimit)
                {
                    return false;
                }
                return true;
            }

            else
            {
                var pricePerUnit = request.Cost;
                if (pricePerUnit > user.OperationUnitLimit)
                {
                    return false;
                }
                if (request.Cost > user.OperationOrderLimit)
                {
                    return false;
                }

                var monthsSpending = _requestsProc.Read(new List<Expression<Func<Request, bool>>> {
                    r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2,
                    r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID,
                    r => r.ParentRequest.OrderDate >= firstOfMonth})
                    .Sum(r => r.Cost);
                if (monthsSpending + request.Cost > user.OperationMonthlyLimit)
                {
                    return false;
                }
                return true;
            }
        }

        protected async Task<TempRequestViewModel> AddItemAccordingToOrderMethod(Request newRequest, AppUtility.OrderMethod OrderMethodEnum, bool isInBudget, RequestItemViewModel requestItemViewModel, int requestNum = 1, ReceivedModalVisualViewModel receivedModalVisualViewModel = null)
        {
            var context = new ValidationContext(newRequest, null, null);
            var results = new List<ValidationResult>();
            var validatorCreate = Validator.TryValidateObject(newRequest, context, results, true);
            TempRequestViewModel trvm = new TempRequestViewModel();
            if (validatorCreate)
            {
                try
                {
                    switch (OrderMethodEnum)
                    {
                        case AppUtility.OrderMethod.AddToCart:
                            if (requestItemViewModel.SectionType == AppUtility.MenuItems.Requests)
                            {
                                trvm = await AddToCart(newRequest, isInBudget, requestItemViewModel.TempRequestListViewModel);
                            }
                            else if (requestItemViewModel.SectionType == AppUtility.MenuItems.Operations)
                            {
                                trvm = await AddToCartOperations(newRequest, isInBudget, requestItemViewModel.TempRequestListViewModel);
                            }
                            break;
                        case AppUtility.OrderMethod.AlreadyPurchased:
                            trvm = await AlreadyPurchased(newRequest, requestItemViewModel.TempRequestListViewModel);
                            break;
                        case AppUtility.OrderMethod.OrderNow:
                            trvm = await OrderNow(newRequest, isInBudget, requestItemViewModel.TempRequestListViewModel);
                            break;
                        case AppUtility.OrderMethod.RequestPriceQuote:
                            trvm = await RequestItem(newRequest, isInBudget);
                            break;
                        case AppUtility.OrderMethod.Save: //proprietary
                            trvm = await SaveItem(newRequest, requestItemViewModel, requestItemViewModel.TempRequestListViewModel.GUID, receivedModalVisualViewModel);
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
            return trvm;
        }
        protected async Task<TempRequestViewModel> AddToCart(Request request, bool isInBudget, TempRequestListViewModel tempRequestListViewModel)
        {
            if (isInBudget)
            {
                request.RequestStatusID = 6;
            }
            else
            {
                request.RequestStatusID = 1;
            }
            request.OrderMethod.DescriptionEnum = AppUtility.OrderMethod.AddToCart.ToString();

            tempRequestListViewModel.TempRequestViewModels = new List<TempRequestViewModel>() { new TempRequestViewModel() { Request = request } };

            return tempRequestListViewModel.TempRequestViewModels.FirstOrDefault();
        }
        protected async Task<TempRequestViewModel> AddToCartOperations(Request request, bool isInBudget, TempRequestListViewModel tempRequestListViewModel)
        {
            if (isInBudget)
            {
                request.RequestStatusID = 6;
            }
            else
            {
                request.RequestStatusID = 1;
            }
            if (!request.IsReceived)
            {
                request.ApplicationUserReceiverID = null;
            }
            using (var transaction = _applicationDbContextTransaction.Transaction)
            {
                try
                {
                    request.Product.SerialNumber = await _requestsProc.GetSerialNumberAsync(true);
                    long lastParentRequestOrderNum = 0;
                    if (_parentRequestsProc.Read().Any())
                    {
                        lastParentRequestOrderNum = _parentRequestsProc.ReadWithIgnoreQueryFilters().OrderByDescending(x => x.OrderNumber).Select(pr => pr.OrderNumber).FirstOrDefault() ?? 0;
                    }
                    ParentRequest pr = new ParentRequest()
                    {
                        ApplicationUserID = request.ApplicationUserCreatorID,
                        OrderNumber = lastParentRequestOrderNum + 1,
                        OrderDate = DateTime.Now
                    };
                    request.ParentRequest = pr;
                    await SaveTempRequestAndCommentsAsync(new TempRequestViewModel() { Request = request });
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {

                }
            }

            return tempRequestListViewModel.TempRequestViewModels.FirstOrDefault();
        }

        private async Task<TempRequestViewModel> AlreadyPurchased(Request request, TempRequestListViewModel tempRequestListViewModel)
        {
            request.RequestStatusID = 2;
            request.ParentQuoteID = null;
            request.OrderMethod.DescriptionEnum = AppUtility.OrderMethod.AlreadyPurchased.ToString();

            tempRequestListViewModel.TempRequestViewModels = new List<TempRequestViewModel>() { new TempRequestViewModel() { Request = request } };

            return tempRequestListViewModel.TempRequestViewModels.FirstOrDefault();
        }
        private async Task<TempRequestViewModel> OrderNow(Request request, bool isInBudget, TempRequestListViewModel tempRequestListViewModel)
        {
            /*try
            {*/
            request.OrderMethod.DescriptionEnum = AppUtility.OrderMethod.OrderNow.ToString();
            if (isInBudget)
            {
                request.RequestStatusID = 6;
            }
            else
            {
                request.RequestStatusID = 1;
            }

            tempRequestListViewModel.TempRequestViewModels = new List<TempRequestViewModel>() {
                    new TempRequestViewModel() {
                    Request = request
                   } };
            //TempRequestJson tempRequestJson = CreateTempRequestJson(tempRequestListViewModel.GUID);
            //TempRequestJson tempRequestJson = new TempRequestJson()
            //{
            //    CookieGUID = tempRequestListViewModel.GUID,
            //    ApplicationUserID = _userManager.GetUserId(User),
            //    IsOriginal = true,
            //    IsCurrent = true
            //};
            //await SetTempRequestAsync(tempRequestJson, tempRequestListViewModel);
            //await transaction.CommitAsync();
            /*}
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();
                throw ex;
            }*/
            return tempRequestListViewModel.TempRequestViewModels.FirstOrDefault();
        }
        private async Task<TempRequestViewModel> RequestItem(Request newRequest, bool isInBudget)
        {
            using (var transaction = _applicationDbContextTransaction.Transaction)
            {
                try
                {
                    newRequest.RequestStatusID = 6;
                    newRequest.Cost = 0;
                    //newRequest.ParentQuote = new ParentQuote();
                    newRequest.QuoteStatusID = 1;
                    newRequest.OrderMethod.DescriptionEnum = AppUtility.OrderMethod.RequestPriceQuote.ToString();
                    //this is assuming that we only reorder request price quotes
                    var ModelStates = new List<ModelAndState>()
                    {
                        new ModelAndState(){ Model = newRequest, StateEnum = EntityState.Added },
                        new ModelAndState(){ Model = newRequest.Product, StateEnum = EntityState.Modified }
                    };
                    await _requestsProc.UpdateModelsAsync(ModelStates);
                    await transaction.CommitAsync();
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
            return new TempRequestViewModel() { Request = newRequest };
        }
        private async Task<TempRequestViewModel> SaveItem(Request newRequest, RequestItemViewModel requestItemViewModel, Guid guid, ReceivedModalVisualViewModel receivedModalVisualViewModel)
        {
            using (var transaction = _applicationDbContextTransaction.Transaction)
            {
                try
                {
                    newRequest.RequestStatusID = 7;
                    newRequest.OrderMethod.DescriptionEnum = AppUtility.OrderMethod.Save.ToString();
                    newRequest.Unit = 1;
                    newRequest.Product.UnitTypeID = 5;
                    newRequest.Product.SerialNumber = await _requestsProc.GetSerialNumberAsync(false);
                    if (newRequest.CreationDate == DateTime.Today) //if it's today, add seconds to be now so it shows up on top
                    {
                        newRequest.CreationDate = DateTime.Now;
                    }
                    var ModelStates = new List<ModelAndState>()
                    {
                        new ModelAndState(){Model = newRequest.Product, StateEnum = EntityState.Added},
                        new ModelAndState(){Model = newRequest, StateEnum = EntityState.Added},
                    };
                    await _requestsProc.UpdateModelsAsync(ModelStates);

                    if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                    {
                        await _requestLocationInstancesProc.SaveLocationsWithoutTransactionAsync(receivedModalVisualViewModel, newRequest, false);
                    }
                    var currentUserID = _userManager.GetUserId(User);
                    await _requestCommentsProc.UpdateWithoutTransactionAsync(AppData.Json.Deserialize<List<RequestComment>>(AppData.Json.Serialize(requestItemViewModel.Comments?.Where(c => c.CommentTypeID == 1))), newRequest.RequestID, currentUserID);
                    await _productCommentsProc.UpdateWithoutTransactionAsync(AppData.Json.Deserialize<List<ProductComment>>(AppData.Json.Serialize(requestItemViewModel.Comments?.Where(c => c.CommentTypeID == 2))), newRequest.ProductID, currentUserID);

                    MoveDocumentsOutOfTempFolder(newRequest.RequestID, AppUtility.ParentFolderName.Requests, false, guid);

                    RequestNotification requestNotification = new RequestNotification();
                    requestNotification.RequestID = newRequest.RequestID;
                    requestNotification.IsRead = false;
                    requestNotification.RequestName = newRequest.Product.ProductName;
                    requestNotification.ApplicationUserID = newRequest.ApplicationUserCreatorID;
                    requestNotification.Description = "item created";
                    requestNotification.NotificationStatusID = 2;
                    requestNotification.NotificationDate = DateTime.Now;
                    requestNotification.Controller = "Requests";
                    requestNotification.Action = "NotificationsView";
                    requestNotification.NotificationDate = DateTime.Now;
                    await _requestNotificationsProc.CreateWithoutTransactionAsync(requestNotification);
                    await transaction.CommitAsync();
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
            return new TempRequestViewModel() { Request = newRequest };
        }


        public async Task SaveTempRequestAndCommentsAsync(TempRequestViewModel tempRequest)
        {

            var ModelStates = new List<ModelAndState>();
            ModelStates.Add(new ModelAndState
            {
                Model = tempRequest.Request.Product,
                StateEnum = tempRequest.Request.ProductID == 0 ? EntityState.Added : EntityState.Modified
            });
            var parentQuoteState = new ModelAndState() { Model = tempRequest.Request.ParentQuote };
            if ((tempRequest.Request.ParentQuoteID == 0 || tempRequest.Request.ParentQuoteID == null) && tempRequest.Request.ParentQuote != null && (tempRequest.Request.ParentQuote?.ParentQuoteID == 0))
            {
                parentQuoteState.StateEnum = EntityState.Added;
            }
            else if ((tempRequest.Request.ParentQuote != null))
            {
                parentQuoteState.StateEnum = EntityState.Unchanged;
            }
            ModelStates.Add(parentQuoteState);
            if (tempRequest.Request.Product is RecurringOrder)
            {
                var recurringRequestsModelStates = await CreateRecurringOrderRequests(tempRequest.Request);
                ModelStates = ModelStates.Concat(recurringRequestsModelStates).ToList();

            }
            else
            {
                ModelStates.Add(new ModelAndState { Model = tempRequest.Request, StateEnum = EntityState.Added });
                ModelStates.Add(new ModelAndState { Model = tempRequest.Request.ParentRequest, StateEnum = EntityState.Added });
            }
            await _requestsProc.UpdateModelsAsync(ModelStates);

            var userID = _userManager.GetUserId(User);
            if (tempRequest.Comments != null && tempRequest.Comments.Any())
            {
                await _requestCommentsProc.UpdateWithoutTransactionAsync(AppData.Json.Deserialize<List<RequestComment>>(AppData.Json.Serialize(tempRequest.Comments.Where(c => c.CommentTypeID == 1))), tempRequest.Request.RequestID, userID);
                await _productCommentsProc.UpdateWithoutTransactionAsync(AppData.Json.Deserialize<List<ProductComment>>(AppData.Json.Serialize(tempRequest.Comments.Where(c => c.CommentTypeID == 2))), tempRequest.Request.ProductID, userID);
            }
        }
        public async Task<List<ModelAndState>> CreateRecurringOrderRequests(Request request)
        {
            RecurringOrder recurringOrder = (RecurringOrder)request.Product;
            var nextRequestDate = recurringOrder.StartDate;
            var nextRequest = request;
            request.CreationDate = recurringOrder.StartDate;
            var ModelStates = new List<ModelAndState>();
            ModelStates.Add(new ModelAndState { Model = request, StateEnum = EntityState.Added });
            var occurrenceCounter = 2;
            while (nextRequestDate.Year == DateTime.Now.Year &&
                ((recurringOrder.RecurrenceEndStatusID == 2 && nextRequestDate > recurringOrder.EndDate)
                    || (recurringOrder.RecurrenceEndStatusID == 3 && occurrenceCounter <= recurringOrder.Occurrences)
                    || recurringOrder.RecurrenceEndStatusID == 1))
            {
                nextRequest = AppUtility.DeepClone(request);
                recurringOrder.TimePeriod = await _timePeriodProc.ReadOneAsync(new List<Expression<Func<TimePeriod, bool>>> { tp => tp.ID == recurringOrder.TimePeriodID });
                switch (Enum.Parse(typeof(AppUtility.TimePeriods), recurringOrder.TimePeriod.DescriptionEnum))
                {
                    case AppUtility.TimePeriods.Days:
                        nextRequestDate = nextRequestDate.AddDays(recurringOrder.TimePeriodAmount);
                        break;
                    case AppUtility.TimePeriods.Weeks:
                        nextRequestDate = nextRequestDate.AddDays(recurringOrder.TimePeriodAmount * 7);
                        break;
                    case AppUtility.TimePeriods.Months:
                        nextRequestDate = nextRequestDate.AddMonths(recurringOrder.TimePeriodAmount);
                        break;
                }

                nextRequest.RequestID = 0;
                nextRequest.CreationDate = nextRequestDate;
                nextRequest.OccurenceNumber = occurrenceCounter;
                nextRequest.Product = recurringOrder;
                nextRequest.Cost = 0;
                nextRequest.Unit = 0;

                ModelStates.Add(new ModelAndState { Model = nextRequest, StateEnum = EntityState.Added });
                occurrenceCounter++;
            }

            return ModelStates;
        }
        public async Task<RedirectToActionResult> RollbackTempRequestHiddenFors(Guid Guid, int SequencePosition, bool isRollBack = true)
        {
            try
            {
                if (SequencePosition == 0)
                {
                    throw new Exception();
                }
                int jsonID = 0;
                if (isRollBack)
                {
                    jsonID = _tempRequestJsonsProc.RollbackAsync(Guid, SequencePosition).Result.TempRequestJsonID;
                }
                else
                {
                    jsonID = _tempRequestJsonsProc.GetTempRequest(Guid, _userManager.GetUserId(User)).FirstOrDefault().TempRequestJsonID;
                }
                return RedirectToAction("_TempRequestHiddenFors", new { ID = jsonID });
            }
            catch (Exception ex)
            {
                return RedirectToAction("JavascriptError");
            }
        }

        public async Task<PartialViewResult> _TempRequestHiddenFors(int ID)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            TempRequestJson tempRequestJson = await _tempRequestJsonsProc.ReadOneAsync(new List<Expression<Func<TempRequestJson, bool>>> { t => t.TempRequestJsonID == ID });
            var fullRequestJson = tempRequestJson.DeserializeJson<FullRequestJson>();
            var trlvm = new TempRequestListViewModel()
            {
                GUID = tempRequestJson.GuidID,
                SequencePosition = tempRequestJson.SequencePosition,
                RequestIndexObject = fullRequestJson == null ? null : fullRequestJson.RequestIndexObject,
                TempRequestViewModels = fullRequestJson == null ? null : fullRequestJson.TempRequestViewModels
            };
            //var trlvm = await LoadTempListFromRequestIndexObjectAsync(requestIndexObject);
            return PartialView(trlvm);
        }

        [Authorize(Roles = "Requests, Operations")]
        public async Task<TermsViewModel> GetTermsViewModelAsync(int vendorID, List<int> requestIds, TempRequestListViewModel tempRequestListViewModel)
        {
            StringWithBool Error = new StringWithBool();
            List<ComplexIncludes<Request, ModelBase>> includes = new List<ComplexIncludes<Request, ModelBase>>();
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.OrderMethod });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentQuote });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((Product)p).Vendor } });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = p => ((ProductSubcategory)p).ParentCategory } });
            List<Expression<Func<Request, bool>>> wheres = new List<Expression<Func<Request, bool>>> { };

            if (vendorID != 0 || (requestIds != null && requestIds.Count != 0))
            {
                if (vendorID != 0)
                {
                    List<Request> reqsFromDB = new List<Request>();
                    if (tempRequestListViewModel.RequestIndexObject.SidebarType == AppUtility.SidebarEnum.Cart)
                    {
                        wheres.Add(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.AddToCart.ToString()
                            && r.QuoteStatusID == 4);
                        wheres.Add(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User));

                    }
                    else if (tempRequestListViewModel.RequestIndexObject.SidebarType == AppUtility.SidebarEnum.Orders)
                    {
                        wheres.Add(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.RequestPriceQuote.ToString()
                         && r.QuoteStatusID == 4);
                    }
                    tempRequestListViewModel.TempRequestViewModels = new List<TempRequestViewModel>();
                }
                else if (requestIds != null && requestIds.Count != 0)
                {
                    tempRequestListViewModel.GUID = Guid.NewGuid();
                    tempRequestListViewModel.TempRequestViewModels = new List<TempRequestViewModel>();
                    wheres.Add(r => requestIds.Contains(r.RequestID));
                }
                AppUtility.CurrencyEnum CurrencyUsed = AppUtility.CurrencyEnum.None;
                var requests = _requestsProc.Read(wheres, includes).AsEnumerable();
                Enum.TryParse(requests.FirstOrDefault().Currency, out CurrencyUsed);
                var VendorID = requests.FirstOrDefault().Product.VendorID;
                foreach (var request in requests)
                {
                    if (request.Currency != CurrencyUsed.ToString())
                    {
                        Error.SetStringAndBool(true, ElixirStrings.ServerDifferentCurrencyErrorMessage);
                    }
                    if (request.Product.VendorID != VendorID)
                    {
                        Error.SetStringAndBool(true, ElixirStrings.ServerDifferentVendorErrorMessage);
                    }
                    tempRequestListViewModel.TempRequestViewModels.Add(new TempRequestViewModel() { Request = request });
                }
            }
            var termsList = new List<SelectListItem>() { };
            await _paymentStatusesProc.Read().ForEachAsync(ps =>
            {
                var selected = false;
                if (ps.PaymentStatusID == 2) { selected = true; }
                if (ps.PaymentStatusID != 4 /*pay upon arrival*/ && ps.PaymentStatusID != 7 /*standing order*/)
                {
                    termsList.Add(new SelectListItem() { Value = ps.PaymentStatusID + "", Text = ps.PaymentStatusDescription, Selected = selected });
                }
            });
            TermsViewModel termsViewModel = new TermsViewModel()
            {
                ParentRequest = new ParentRequest(),
                TermsList = termsList,
                InstallmentDate = DateTime.Now,
                EmailAddresses = new List<string>() { tempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Product.Vendor.OrdersEmail, "", "", "", "" },
                Error = Error
            };
            tempRequestListViewModel.RequestIndexObject.SelectedCurrency = (AppUtility.CurrencyEnum)Enum.Parse(typeof(AppUtility.CurrencyEnum),
                tempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Currency);
            termsViewModel.TempRequestListViewModel = tempRequestListViewModel;
            await _tempRequestJsonsProc.UpdateAsync(tempRequestListViewModel.GUID, tempRequestListViewModel.RequestIndexObject, tempRequestListViewModel, _userManager.GetUserId(User), true);
            return termsViewModel;
        }

        public async Task<RedirectAndModel> SaveTermsModalAsync(TermsViewModel termsViewModel, TempRequestListViewModel tempRequestListViewModel)
        {
            var controller = "Requests";
            var needsToBeApproved = false;
            var userID = _userManager.GetUserId(User);
            var tempRequestJson = await _tempRequestJsonsProc.GetTempRequest(tempRequestListViewModel.GUID, _userManager.GetUserId(User)).FirstOrDefaultAsync();


            try
            {
                //var fullRequestJson = 
                var newTRLVM = new TempRequestListViewModel { TempRequestViewModels = tempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels };
                newTRLVM.GUID = tempRequestListViewModel.GUID;
                newTRLVM.RequestIndexObject = tempRequestListViewModel.RequestIndexObject;
                newTRLVM.SequencePosition = tempRequestJson.SequencePosition;

                long lastParentRequestOrderNum = 0;
                //var prs = _proc.ParentRequests;
                if (_parentRequestsProc.Read().Any())
                {
                    lastParentRequestOrderNum = _parentRequestsProc.ReadWithIgnoreQueryFilters().OrderByDescending(x => x.OrderNumber).Select(pr => pr.OrderNumber).FirstOrDefault() ?? 0;
                }
                //ParentRequest pr = new ParentRequest()
                //{
                //    ApplicationUserID = userID,
                //    OrderNumber = lastParentRequestOrderNum + 1,
                //    OrderDate = DateTime.Now
                //};

                termsViewModel.ParentRequest.OrderNumber = lastParentRequestOrderNum + 1;
                termsViewModel.ParentRequest.OrderDate = DateTime.Now;
                var SaveUsingTempRequest = true;
                bool hasShippingOnPayment;
                if (termsViewModel.ParentRequest.Shipping == 0)
                {
                    termsViewModel.ParentRequest.IsShippingPaid = true;
                    hasShippingOnPayment = true;
                }
                else
                {
                    termsViewModel.ParentRequest.IsShippingPaid = false;
                    hasShippingOnPayment = false;
                }
                using (var transaction = _applicationDbContextTransaction.Transaction)
                {
                    try
                    {

                        foreach (var tempRequest in newTRLVM.TempRequestViewModels)
                        {
                            int counter = 0;
                            //throw new Exception();
                            tempRequest.Request.PaymentStatusID = termsViewModel.SelectedTerm;
                            tempRequest.Request.Installments = (uint)termsViewModel.Installments != 0 ? (uint)termsViewModel.Installments : 1;
                            if (newTRLVM.TempRequestViewModels.Count() == 1 && tempRequest.Request.RequestStatusID == 1) //item is ordernow and needs to be approved
                            {
                                needsToBeApproved = true;
                            }
                            //check if we still need this
                            if (tempRequest.Request.Product == null)
                            {
                                tempRequest.Request.Product = await _productsProc.ReadOneAsync(new List<Expression<Func<Product, bool>>> { p => p.ProductID == tempRequest.Request.ProductID }, new List<ComplexIncludes<Product, ModelBase>> { new ComplexIncludes<Product, ModelBase> { Include = p => p.ProductSubcategory } });
                            }

                            if (tempRequest.Request.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.AlreadyPurchased.ToString() || needsToBeApproved)
                            {
                                SaveUsingTempRequest = false;
                            }
                            if (tempRequest.Request.ParentRequest == null)
                            {
                                tempRequest.Request.ParentRequest = termsViewModel.ParentRequest;
                            }
                            //test if we need to add the shipping
                            tempRequest.Request.ParentRequest.Shipping = termsViewModel.ParentRequest.Shipping;

                            if (!SaveUsingTempRequest)
                            {
                                var isOperations = false;
                                tempRequest.Request.Product.SerialNumber = await _requestsProc.GetSerialNumberAsync(isOperations);
                                await SaveTempRequestAndCommentsAsync(tempRequest);

                                var additionalRequests = counter + 1 < newTRLVM.TempRequestViewModels.Count ? true : false;
                                MoveDocumentsOutOfTempFolder(tempRequest.Request.RequestID, AppUtility.ParentFolderName.Requests, additionalRequests, newTRLVM.GUID);
                                tempRequest.Request.Product.Vendor = await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == tempRequest.Request.Product.VendorID });
                                if (!needsToBeApproved)
                                {
                                    RequestNotification requestNotification = new RequestNotification();
                                    requestNotification.RequestID = tempRequest.Request.RequestID;
                                    requestNotification.IsRead = false;
                                    requestNotification.RequestName = tempRequest.Request.Product.ProductName;
                                    requestNotification.ApplicationUserID = tempRequest.Request.ApplicationUserCreatorID;
                                    requestNotification.Description = "item ordered";
                                    requestNotification.NotificationStatusID = 2;
                                    requestNotification.NotificationDate = DateTime.Now;
                                    requestNotification.Controller = "Requests";
                                    requestNotification.Action = "NotificationsView";
                                    requestNotification.NotificationDate = DateTime.Now;
                                    _requestNotificationsProc.CreateWithoutSaveChanges(requestNotification);
                                }

                            }
                            else if (SaveUsingTempRequest)
                            {
                                tempRequest.Payments = new List<Payment>();
                                tempRequest.Emails = termsViewModel.EmailAddresses.Where(e => e != null).ToList();
                            }

                            for (int i = 0; i < tempRequest.Request.Installments; i++)
                            {
                                Payment payment = UpdatePaymentFields(termsViewModel, ref hasShippingOnPayment, tempRequest, i);
                                if (SaveUsingTempRequest)
                                {
                                    tempRequest.Payments.Add(payment);
                                }
                                else
                                {
                                    payment.RequestID = tempRequest.Request.RequestID;

                                    _paymentsProc.CreateWithoutSaveChanges(payment);

                                }
                            }
                            counter++;
                        }
                        await _requestsProc.SaveDbChangesAsync();
                        if (SaveUsingTempRequest)
                        {
                            //this should be false, setting to true for testing purposes
                            await _tempRequestJsonsProc.UpdateWithoutTransactionAsync(tempRequestListViewModel.GUID, tempRequestListViewModel.RequestIndexObject, newTRLVM, userID, false);
                            await transaction.CommitAsync();
                        }
                        if (!SaveUsingTempRequest)
                        {
                            MoveDocumentsOutOfTempFolder((int)newTRLVM.TempRequestViewModels[0].Request.ParentRequestID, AppUtility.ParentFolderName.ParentRequest, false, newTRLVM.GUID); //either they all have same parentrequests are already outof the temp folder
                            if (newTRLVM.TempRequestViewModels[0].Request.ParentQuoteID != null)
                            {
                                MoveDocumentsOutOfTempFolder((int)newTRLVM.TempRequestViewModels[0].Request.ParentQuoteID, AppUtility.ParentFolderName.ParentQuote, false, newTRLVM.GUID); //either they all have same parentrequests are already outof the temp folder

                            }

                            await _tempRequestJsonsProc.RemoveAllAsync(newTRLVM.GUID, _userManager.GetUserId(User));
                            tempRequestListViewModel.RequestIndexObject.GUID = tempRequestListViewModel.GUID;
                            await transaction.CommitAsync();
                            if (!needsToBeApproved)
                            {
                                return new RedirectAndModel() { RedirectToActionResult = new RedirectToActionResult("Index", controller, tempRequestListViewModel.RequestIndexObject) };
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        await _tempRequestJsonsProc.RollbackAsync(newTRLVM.GUID, tempRequestJson.SequencePosition);
                        transaction.Rollback();
                        throw ex;
                    }

                }

                //if (termsViewModel.ParentRequest.Requests.FirstOrDefault().RequestStatusID == 6 && request.OrderType != AppUtility.OrderTypeEnum.AddToCart.ToString())
                //{
                //
                if (!needsToBeApproved)
                {
                    //await KeepTempRequestJsonCurrentAsOriginal(tempRequestListViewModel.GUID);
                    //get rid of old trlvm?
                    tempRequestListViewModel.RequestIndexObject.GUID = tempRequestListViewModel.GUID;

                    return new RedirectAndModel() { RedirectToActionResult = new RedirectToActionResult("ConfirmEmailModal", controller, tempRequestListViewModel.RequestIndexObject) };
                }
                else
                {
                    //get rid of old trlvm?
                    return new RedirectAndModel() { RedirectToActionResult = new RedirectToActionResult("NeedsToBeApproved", "", "") };
                }
            }
            catch (Exception ex)
            {
                await _tempRequestJsonsProc.RollbackAsync(tempRequestListViewModel.GUID, tempRequestJson.SequencePosition);
                termsViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                //var termsList = new List<SelectListItem>() { };
                //await _proc.PaymentStatuses.ForEachAsync(ps => termsList.Add(new SelectListItem() { Value = ps.PaymentStatusID + "", Text = ps.PaymentStatusDescription }));
                //termsViewModel.TermsList = termsList;
                //termsViewModel.TempRequestListViewModel = tempRequestListViewModel;
                return new RedirectAndModel() { RedirectToActionResult = new RedirectToActionResult("", "", ""), TermsViewModel = termsViewModel };
            }
        }

        private static Payment UpdatePaymentFields(TermsViewModel termsViewModel, ref bool hasShippingOnPayment, TempRequestViewModel tempRequest, int i)
        {
            var payment = new Payment()
            {
                InstallmentNumber = i + 1,
                ShippingPaidHere = hasShippingOnPayment ? false : true
            };
            hasShippingOnPayment = true;
            if (tempRequest.Request.PaymentStatusID == 5)
            {
                payment.PaymentDate = termsViewModel.InstallmentDate.AddMonths(i);
                payment.Sum = ((tempRequest.Request.Cost ?? 0) / (tempRequest.Request.Installments ?? 0));
            }
            else if (tempRequest.Request.PaymentStatusID == 7)
            {
                payment.PaymentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(1);
                payment.Sum = tempRequest.Request.Cost ?? 0;
            }
            else
            {
                payment.PaymentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                payment.Sum = tempRequest.Request.Cost ?? 0;
            }

            return payment;
        }

        public async Task<IActionResult> RedirectRequestsToShared(string action, RequestIndexObject requestIndexObject)
        {
            return RedirectToAction(action, requestIndexObject);
        }



        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AccountingGeneral(RequestIndexObject requestIndexObject, RequestsSearchViewModel requestsSearchViewModel)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            RequestIndexPartialViewModel requestIndexPartialViewModel = await GetIndexViewModel(requestIndexObject, Years: new List<int>() { DateTime.Now.Year }, Months: new List<int>() { DateTime.Now.Month }, requestsSearchViewModel: requestsSearchViewModel);
            //AccountingGeneralViewModel viewModel = new AccountingGeneralViewModel() { RequestIndexPartialViewModel = requestIndexPartialViewModel };
            return PartialView(requestIndexPartialViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ItemTableOwner(RequestIndexObject requestIndexObject)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Owner;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            //SetViewModelCounts(requestIndexObject, viewModel);
            //SetViewModelProprietaryCounts(requestIndexObject, viewModel);
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
            //SetViewModelCounts(requestIndexObject, viewModel);
            //SetViewModelProprietaryCounts(requestIndexObject, viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> IndexFavorites()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Favorites;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            RequestIndexObject requestIndexObject = new RequestIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = AppUtility.SidebarEnum.Favorites
            };
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> IndexShared()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedRequests;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            //RequestIndexObject requestIndexObject = new RequestIndexObject()
            //{
            //    PageType = AppUtility.PageTypeEnum.RequestCart,
            //    SidebarType = AppUtility.SidebarEnum.SharedRequests
            //};
            RequestIndexPartialViewModel viewModel = await GetSharedRequestIndexObjectAsync();
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> IndexLists()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.MyLists;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            RequestIndexObject requestIndexObject = new RequestIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = AppUtility.SidebarEnum.MyLists
            };

            RequestListIndexViewModel viewModel = await GetRequestListIndexObjectAsync(requestIndexObject);
            return View(viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> IndexSharedLists()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SharedLists;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            RequestIndexObject requestIndexObject = new RequestIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = AppUtility.SidebarEnum.SharedLists
            };

            RequestListIndexViewModel viewModel = await GetSharedRequestListIndexObjectAsync(requestIndexObject);
            return View(viewModel);
        }
        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _IndexTableWithListTabs(RequestIndexObject requestIndexObject, String errorMessage)
        {
            RequestListIndexViewModel viewModel = new RequestListIndexViewModel();
            if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.MyLists)
            {
                viewModel = await GetRequestListIndexObjectAsync(requestIndexObject);
            }
            else
            {
                viewModel = await GetSharedRequestListIndexObjectAsync(requestIndexObject);
            }
            viewModel.ErrorMessage = errorMessage;
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ItemTableType(RequestIndexObject requestIndexObject)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Type;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            RequestIndexPartialViewModel viewModel = await GetIndexViewModel(requestIndexObject);
            //SetViewModelCounts(requestIndexObject, viewModel);
            // SetViewModelProprietaryCounts(requestIndexObject, viewModel);
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
        public async Task<JsonResult> DeleteModalJson(int? id, RequestIndexObject requestIndexObject)
        {
            //if (!AppUtility.IsAjaxRequest(Request))
            //{
            //    return PartialView("InvalidLinkPage");
            //}


            var request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { m => m.RequestID == id },
                new List<ComplexIncludes<Request, ModelBase>>{
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.Product,
                        ThenInclude= new ComplexIncludes<ModelBase, ModelBase>{ Include = p => ((Product)p).ProductSubcategory,
                            ThenInclude =  new ComplexIncludes<ModelBase, ModelBase> { Include = ps => ((ProductSubcategory)ps).ParentCategory }}},
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.Vendor } });



            DeleteRequestViewModel deleteRequestViewModel = new DeleteRequestViewModel()
            {
                Request = request
            };
            if (request == null)
            {
                deleteRequestViewModel.ErrorMessage = "Product not found (no request). Unable to delete";
                Response.StatusCode = 500;
            }
            if (id == null)
            {
                deleteRequestViewModel.ErrorMessage = "Product not found (no id). Unable to delete.";
                Response.StatusCode = 500;
            }
            return Json(JsonConvert.SerializeObject(deleteRequestViewModel, Formatting.Indented, new JsonSerializerSettings
            {

                Converters = new List<JsonConverter> { new StringEnumConverter() },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<JsonResult> DeleteModal(DeleteRequestViewModel deleteRequestViewModel, RequestIndexObject requestIndexObject, RequestsSearchViewModel requestsSearchViewModel, SelectedRequestFilters selectedFilters, int numFilters = 0)
        {
            try
            {
                var success = await _requestsProc.DeleteAsync(deleteRequestViewModel.Request.RequestID);
                if (!success.Bool)
                {
                    throw new Exception(success.String);
                }
            }

            catch (Exception ex)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                return null;
            }
            return await GetIndexTableJson(requestIndexObject, requestsSearchViewModel: requestsSearchViewModel, selectedFilters: selectedFilters, numFilters: numFilters);

        }

        public async Task<JsonResult> GetIndexTableJson(RequestIndexObject requestIndexObject, List<int> Months = null, List<int> Years = null,
                                                                              SelectedRequestFilters selectedFilters = null, int numFilters = 0, RequestsSearchViewModel? requestsSearchViewModel = null)
        {
            string json = "";
            if (CheckIfIndexTableByVendor(requestIndexObject.SectionType, requestIndexObject.PageType, requestIndexObject.SidebarType))
            {
                var viewModelByVendor = await GetIndexViewModelByVendor(requestIndexObject);
                json = JsonConvert.SerializeObject(viewModelByVendor, Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       Converters = new List<JsonConverter> { new StringEnumConverter() },
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
            }
            else
            {
                var viewModel = await GetIndexViewModel(requestIndexObject, Months, Years, selectedFilters: selectedFilters, numFilters, requestsSearchViewModel: requestsSearchViewModel);
                json = JsonConvert.SerializeObject(viewModel, Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       Converters = new List<JsonConverter> { new StringEnumConverter() },
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
            }

            return Json(json);
        }

        private bool CheckIfIndexTableByVendor(AppUtility.MenuItems SectionType, AppUtility.PageTypeEnum PageType, AppUtility.SidebarEnum SidebarType)
        {
            bool isByVendor = false;
            switch (SectionType)
            {
                case AppUtility.MenuItems.Accounting:
                    switch (PageType)
                    {
                        case AppUtility.PageTypeEnum.AccountingPayments:
                        case AppUtility.PageTypeEnum.AccountingNotifications:
                            isByVendor = true;
                            break;
                    }
                    break;
                case AppUtility.MenuItems.LabManagement:
                    isByVendor = true;
                    break;
                case AppUtility.MenuItems.Requests:
                    switch (PageType)
                    {
                        case AppUtility.PageTypeEnum.RequestCart:
                            switch (SidebarType)
                            {
                                case AppUtility.SidebarEnum.Orders:
                                    isByVendor = true;
                                    break;
                            }
                            break;
                    }
                    break;

                    break;
                case AppUtility.MenuItems.Operations:
                    break;
            }
            return isByVendor;
        }


        [HttpGet]
        [Authorize(Roles = "Requests, Operations")]
        public async Task<IActionResult> AddItemView(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel();
            var categoryType = 1;
            if (SectionType == AppUtility.MenuItems.Operations)
            {
                categoryType = 2;
            }

            if (PageType == AppUtility.PageTypeEnum.RequestSummary)
            {
                requestItemViewModel.IsProprietary = true;
            }
            requestItemViewModel = await FillRequestItemViewModel(requestItemViewModel, categoryType);
            requestItemViewModel.Requests[0].IncludeVAT = true;
            requestItemViewModel.PageType = PageType;
            requestItemViewModel.SectionType = SectionType;
            
            TempRequestListViewModel tempRequestListViewModel = new TempRequestListViewModel()
            {
                GUID = Guid.NewGuid(),
                TempRequestViewModels = new List<TempRequestViewModel>()
            };
            await _tempRequestJsonsProc.UpdateAsync(tempRequestListViewModel.GUID, new RequestIndexObject { }, tempRequestListViewModel, _userManager.GetUserId(User), true);
            requestItemViewModel.TempRequestListViewModel = tempRequestListViewModel;

            TempListWithRequestItem tempViewModel = new TempListWithRequestItem()
            {
                TempRequestListViewModel = tempRequestListViewModel,
                RequestItemViewModel = requestItemViewModel
            };

            return View(tempViewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> AddItemView(AppUtility.OrderMethod OrderMethod, TempRequestListViewModel tempRequestListViewModel, RequestItemViewModel requestItemViewModel, ReceivedModalVisualViewModel receivedModalVisualViewModel = null)
        {
            requestItemViewModel.TempRequestListViewModel = tempRequestListViewModel;

            try
            {
                var trlvm = SaveAddItemView(requestItemViewModel, OrderMethod, receivedModalVisualViewModel).Result;
                var json = JsonConvert.SerializeObject(trlvm, Formatting.Indented,
                  new JsonSerializerSettings
                  {
                      Converters = new List<JsonConverter> { new StringEnumConverter() },
                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                  });
                return Json(json);
            }
            catch (Exception ex)
            {
                requestItemViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                return PartialView("_ErrorMessage", requestItemViewModel.ErrorMessage);
            }
        }


        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> CreateItemTabs(int productSubCategoryId, Guid guid, AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest, string itemName = "",
            bool isRequestQuote = false)
        { //TODO : CHECK IF WE NEED TO DELETE GUID DOCS HERE

            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;

            var categoryType = 1;
            var sectionType = AppUtility.MenuItems.Requests;
            if (PageType.ToString().StartsWith("Operations"))
            {
                sectionType = AppUtility.MenuItems.Operations;
                categoryType = 2;
            }

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel();

            requestItemViewModel = await FillRequestItemViewModel(requestItemViewModel, categoryType, productSubCategoryId);
            requestItemViewModel.Requests.FirstOrDefault().IncludeVAT = true;
            requestItemViewModel.SectionType = sectionType;
            requestItemViewModel.PageType = PageType;
            requestItemViewModel.Requests.FirstOrDefault().Product.ProductName = itemName;
            requestItemViewModel.IsRequestQuote = isRequestQuote;
            requestItemViewModel.GUID = guid;

            //TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.RequestPageTypeEnum.Request;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;

            return PartialView(requestItemViewModel);
        }
        private async Task<RequestItemViewModel> FillRequestItemViewModel(RequestItemViewModel requestItemViewModel, int categoryTypeId, int productSubcategoryId = 0)
        {
            var productSubcategory = await _productSubcategoriesProc.ReadOneAsync(new List<Expression<Func<ProductSubcategory, bool>>> { ps => ps.ID == productSubcategoryId }, new List<ComplexIncludes<ProductSubcategory, ModelBase>> { new ComplexIncludes<ProductSubcategory, ModelBase> { Include = p => p.ParentCategory } });
            requestItemViewModel = await FillRequestDropdowns(requestItemViewModel, productSubcategory, categoryTypeId);

            if (productSubcategory == null)
            {
                ParentCategory parentCategory = new ParentCategory();
                if (requestItemViewModel.IsProprietary)
                {
                    parentCategory = await _parentCategoriesProc.ReadOneAsync(new List<Expression<Func<ParentCategory, bool>>> { pc => pc.Description == AppUtility.ParentCategoryEnum.Samples.ToString() });
                }

                productSubcategory = new ProductSubcategory()
                {
                    ParentCategory = parentCategory
                };
            }
            else if (productSubcategory.ParentCategory.Description == AppUtility.ParentCategoryEnum.Samples.ToString())
            {
                requestItemViewModel.IsProprietary = true;
            }
            if (categoryTypeId == 2)
            {
                requestItemViewModel.ApplicationUsers = _employeesProc.Read()
                              .Select(
                                  u => new SelectListItem
                                  {
                                      Text = u.FirstName + " " + u.LastName,
                                      Value = u.Id,
                                      Selected = u.Id == _userManager.GetUserId(User)
                                  }
                              ).ToList();
            }

            requestItemViewModel.Comments = new List<CommentBase>();

            requestItemViewModel.ModalType = AppUtility.RequestModalType.Create;

            requestItemViewModel.Requests = new List<Request>();
            requestItemViewModel.Requests.Add(new Request());
            requestItemViewModel.Requests.FirstOrDefault().ExchangeRate = await GetExchangeRateAsync();
            requestItemViewModel.Requests.FirstOrDefault().Product = new SingleOrder();
            requestItemViewModel.Requests.FirstOrDefault().ParentQuote = new ParentQuote();
            requestItemViewModel.Requests.FirstOrDefault().SubProject = new SubProject();
            requestItemViewModel.Requests.FirstOrDefault().Product.ProductSubcategory = productSubcategory;
            requestItemViewModel.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategory = productSubcategory.ParentCategory;
            requestItemViewModel.Requests.FirstOrDefault().Product.ProductSubcategory.ParentCategoryID = productSubcategory.ParentCategoryID;
            requestItemViewModel.Requests.FirstOrDefault().Product.ProductSubcategoryID = productSubcategoryId;
            requestItemViewModel.Requests.FirstOrDefault().CreationDate = DateTime.Now;
            requestItemViewModel.Requests.FirstOrDefault().Cost = 0;


            if (productSubcategory != null && productSubcategory.ParentCategory.IsProprietary)
            {
                requestItemViewModel.ReceivedLocationViewModel = new ReceivedLocationViewModel()
                {
                    Request = requestItemViewModel.Requests.FirstOrDefault(),
                    locationTypesDepthZero = _locationTypesProc.Read(new List<Expression<Func<LocationType, bool>>> { lt => lt.Depth == 0 }).AsEnumerable(),
                    locationInstancesSelected = new List<LocationInstance>(),
                };
                requestItemViewModel.RequestStatusID = 7;
            }
            FillDocumentsInfo(requestItemViewModel, productSubcategory);
            //base.DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests);
            //base.DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote);
            return requestItemViewModel;
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _PartialItemOperationsTab(int index, int subcategoryID = 0)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var operationsItemViewModel = new OperationsItemViewModel()
            {
                RequestIndex = index,
                ModalType = AppUtility.RequestModalType.Create,
                ParentCategories = _parentCategoriesProc.Read(new List<Expression<Func<ParentCategory, bool>>> { pc => pc.CategoryTypeID == 2 }).ToList(),
                ProductSubcategories = new List<ProductSubcategory>()
            };
            operationsItemViewModel.Request = new Request() { IncludeVAT = true };
            if (subcategoryID > 0)
            {
                operationsItemViewModel.Request.Product = new SingleOrder();
                operationsItemViewModel.Request.Product.ProductSubcategoryID = subcategoryID;
                operationsItemViewModel.Request.Product.ProductSubcategory =
                  await _productSubcategoriesProc.ReadOneAsync(new List<Expression<Func<ProductSubcategory, bool>>> { ps => ps.ID == subcategoryID });
                operationsItemViewModel.ProductSubcategories =
                    _productSubcategoriesProc.Read(new List<Expression<Func<ProductSubcategory, bool>>> { ps => ps.ParentCategoryID == operationsItemViewModel.Request.Product.ProductSubcategory.ParentCategoryID }).AsEnumerable();
            }
            //operationsItemViewModel.Request.Product = new Product();
            //operationsItemViewModel.Request.Product.ProductSubcategory = new ProductSubcategory();
            return PartialView(operationsItemViewModel);
        }
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> RequestFavorite(int requestID, string FavType, AppUtility.SidebarEnum sidebarType)
        {
            var userID = _userManager.GetUserId(User);
            if (FavType == "favorite")
            {
                var favoriteRequest = await _favoriteRequestsProc.ReadOneAsync(new List<Expression<Func<FavoriteRequest, bool>>> { fr => fr.RequestID == requestID && fr.ApplicationUserID == userID });
                if (favoriteRequest == null)
                {
                    var success = await _favoriteRequestsProc.CreateAsync(requestID, userID);
                    if (!success.Bool)
                    {
                        Response.StatusCode = 500;
                        await Response.WriteAsync(success.String);
                    }
                }
            }
            else if (FavType == "unlike")
            {
                var success = await _favoriteRequestsProc.DeleteAsync(requestID, userID);
                if (!success.Bool)
                {
                    Response.StatusCode = 500;
                    await Response.WriteAsync(success.String);
                    return new EmptyResult();
                }
                if (sidebarType == AppUtility.SidebarEnum.Favorites)
                {
                    RequestIndexObject requestIndexObject = new RequestIndexObject()
                    {
                        PageType = AppUtility.PageTypeEnum.RequestCart,
                        SidebarType = sidebarType,
                    };
                    return await RedirectRequestsToShared("_IndexTable", requestIndexObject);
                }
            }
            return new EmptyResult();
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ShareModal(int ID, AppUtility.ModelsEnum ModelsEnum)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var shareViewModel = base.GetShareModalViewModel(ID);
            switch (ModelsEnum)
            {
                case AppUtility.ModelsEnum.Request:
                    shareViewModel.ObjectDescription = _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == ID }, new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include = r => r.Product } }).Result.Product.ProductName;
                    break;
                case AppUtility.ModelsEnum.RequestLists:
                    shareViewModel.ObjectDescription = _requestListsProc.ReadOneAsync(new List<Expression<Func<RequestList, bool>>> { rl => rl.ListID == ID }).Result.Title;
                    break;
            }

            return PartialView(shareViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<JsonResult> ShareModalJson(int ID, AppUtility.ModelsEnum ModelsEnum)
        {
            //if (!AppUtility.IsAjaxRequest(Request))
            //{
            //    return PartialView("InvalidLinkPage");
            //}
            var shareViewModel = base.GetShareModalViewModel(ID);
            switch (ModelsEnum)
            {
                case AppUtility.ModelsEnum.Request:
                    shareViewModel.ObjectDescription = _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == ID }, new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include = r => r.Product } }).Result.Product.ProductName;
                    break;
                case AppUtility.ModelsEnum.RequestLists:
                    shareViewModel.ObjectDescription = _requestListsProc.ReadOneAsync(new List<Expression<Func<RequestList, bool>>> { rl => rl.ListID == ID }).Result.Title;
                    break;
            }


            var json = JsonConvert.SerializeObject(shareViewModel, Formatting.Indented,
               new JsonSerializerSettings
               {
                   Converters = new List<JsonConverter> { new StringEnumConverter() },
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });
            return Json(json);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<bool> ShareModal(ShareModalViewModel shareModalViewModel)
        {
            var success = await _shareRequestsProc.UpdateAsync(shareModalViewModel.ID, _userManager.GetUserId(User), shareModalViewModel.ApplicationUserIDs);
            if (!success.Bool)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(success.String);
            }
            return success.Bool;
        }

        [Authorize(Roles = "Requests")]
        public async Task<bool> RemoveShare(int ID, AppUtility.ModelsEnum ModelsEnum = AppUtility.ModelsEnum.Request)
        {
            var error = await _shareRequestsProc.DeleteAsync(ID, _userManager.GetUserId(User));
            return error.Bool;
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ItemData(int? id, int productSubCategoryID = 0, int? Tab = 0, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, bool isEditable = true)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            List<string> selectedPriceSort = null;
            selectedPriceSort = new List<string>() { AppUtility.PriceSortEnum.Unit.ToString(), AppUtility.PriceSortEnum.TotalVat.ToString() };
            var requestItemViewModel = await editModalViewFunction(id, Tab, SectionType, isEditable, selectedPriceSort, productSubCategoryID: productSubCategoryID);
            return PartialView(requestItemViewModel);
        }


        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _ItemHeader(int? id, AppUtility.MenuItems SectionType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var categoryTypeId = 1;
            if (SectionType == AppUtility.MenuItems.Operations)
            {
                categoryTypeId = 2;
            }
            var requestItemViewModel = new RequestItemViewModel();
            requestItemViewModel.Vendors = await _vendorsProc.Read(new List<Expression<Func<Vendor, bool>>> { v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == categoryTypeId).Count() > 0 }).ToListAsync();
            requestItemViewModel.SectionType = SectionType;
            var request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { x => x.RequestID == id }, new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include = r => r.Product }, new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.Vendor } });
            requestItemViewModel.Requests = new List<Request>() { request };
            return PartialView(requestItemViewModel);
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> EditModalView(int? id, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, bool isEditable = true, List<string> selectedPriceSort = null, bool isProprietary = false, int? Tab = 0, string ErrorMessage = null)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            selectedPriceSort = selectedPriceSort.Count == 0 ? new List<string>() { AppUtility.PriceSortEnum.Unit.ToString(), AppUtility.PriceSortEnum.TotalVat.ToString() } : selectedPriceSort;
            var requestItemViewModel = await editModalViewFunction(id, Tab, SectionType, isEditable, selectedPriceSort, isProprietary: isProprietary);
            requestItemViewModel.ErrorMessage = ErrorMessage;
            return PartialView(requestItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> EditModalView(RequestItemViewModel requestItemViewModel, ReceivedModalVisualViewModel receivedModalVisualViewModel)
        {
            using (var transaction = _applicationDbContextTransaction.Transaction)
            {
                try
                {
                    var request = requestItemViewModel.Requests.FirstOrDefault();

                    switch (requestItemViewModel.OrderType)
                    {
                        case AppUtility.OrderType.SingleOrder:
                            request.Product = request.SingleOrder;
                            break;
                        case AppUtility.OrderType.RecurringOrder:
                            request.Product = request.RecurringOrder;
                            break;
                        case AppUtility.OrderType.StandingOrder:
                            request.Product = request.StandingOrder;
                            break;
                    };
                    //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
                    request.ParentRequest = null;
                    var parentQuote = await _parentQuotesProc.ReadOneAsync(new List<Expression<Func<ParentQuote, bool>>> { pq => pq.ParentQuoteID == request.ParentQuoteID });

                    if (parentQuote != null && request.ParentQuote != null)
                    {
                        parentQuote.QuoteNumber = request.ParentQuote.QuoteNumber;
                        parentQuote.QuoteDate = request.ParentQuote.QuoteDate;
                        request.ParentQuote = parentQuote;
                    }

                    var product = await _productsProc.ReadOneWithIgnoreQueryFiltersAsync(new List<Expression<Func<Product, bool>>> { v => v.ProductID == request.ProductID }, new List<ComplexIncludes<Product, ModelBase>> { new ComplexIncludes<Product, ModelBase> { Include = p => p.Vendor }, new ComplexIncludes<Product, ModelBase> { Include = p => p.ProductSubcategory } });
                    product.VendorID = request.Product.VendorID;
                    product.CatalogNumber = request.Product.CatalogNumber;
                    //in case we need to return to the modal view
                    product.ProductName = request.Product.ProductName;
                    product.ProductSubcategoryID = request.Product.ProductSubcategoryID;
                    product.UnitTypeID = request.Product.UnitTypeID;
                    product.SubUnit = request.Product.SubUnit;
                    product.SubUnitTypeID = request.Product.SubUnitTypeID;
                    product.SubSubUnit = request.Product.SubSubUnit;
                    product.SubSubUnitTypeID = request.Product.SubSubUnitTypeID;

                    var parentCategoryId = request.Product.ProductSubcategory.ParentCategoryID;
                    requestItemViewModel.ProductSubcategories = _productSubcategoriesProc.Read(new List<Expression<Func<ProductSubcategory, bool>>> {
                        ps => ps.ParentCategory.CategoryTypeID == 1,
                        ps => ps.ParentCategoryID == parentCategoryId
                    });
                    requestItemViewModel.Vendors = _vendorsProc.Read();
                    //redo the unit types when seeded
                    var unittypes = _unitTypesProc.Read(includes: new List<ComplexIncludes<UnitType, ModelBase>> {
                            new ComplexIncludes<UnitType, ModelBase>{ Include = u => u.UnitParentType} })
                       .OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);

                    requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");

                    //declared outside the if b/c it's used farther down to (for parent request the new comment too)
                    var currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == _userManager.GetUserId(User) });

                    var context = new ValidationContext(request, null, null);
                    var results = new List<ValidationResult>();

                    if (Validator.TryValidateObject(request, context, results, true))
                    {
                        /*
                         * the viewmodel loads the request.product with a primary key of 0
                         * so if you don't insert the request.productid into the request.product.productid
                         * it will create a new one instead of updating the existing one
                         * only need this if using an existing product
                         */
                        request.Product = product;
                        List<ModelAndState> UpdateModelStates = new List<ModelAndState>();
                        UpdateModelStates.Add(new ModelAndState { Model = request, StateEnum = EntityState.Modified });
                        UpdateModelStates.Add(new ModelAndState { Model = request.Product, StateEnum = EntityState.Modified });

                        if (request.Payments?[0].InvoiceID != null)
                        {
                            UpdateModelStates.Add(new ModelAndState { Model = request.Payments[0].Invoice, StateEnum = EntityState.Modified });
                        }

                        await _requestsProc.UpdateModelsAsync(UpdateModelStates);


                        if (requestItemViewModel.Comments != null)
                        {
                            await _requestCommentsProc.UpdateWithoutTransactionAsync(AppData.Json.Deserialize<List<RequestComment>>(AppData.Json.Serialize(requestItemViewModel.Comments.Where(c => c.CommentTypeID == 1))), request.RequestID, currentUser.Id);
                            await _productCommentsProc.UpdateWithoutTransactionAsync(AppData.Json.Deserialize<List<ProductComment>>(AppData.Json.Serialize(requestItemViewModel.Comments.Where(c => c.CommentTypeID == 2))), request.ProductID, currentUser.Id);
                        }

                        await _requestLocationInstancesProc.UpdateWithoutTransactionAsync(receivedModalVisualViewModel, request.RequestID);
                    }
                    else
                    {
                        foreach (var result in results)
                        {
                            requestItemViewModel.ErrorMessage += result.ErrorMessage;
                        }
                        throw new ModelStateInvalidException(requestItemViewModel.ErrorMessage);
                    }
                    AppUtility.PageTypeEnum requestPageTypeEnum = (AppUtility.PageTypeEnum)requestItemViewModel.PageType;
                    await transaction.CommitAsync();
                    requestItemViewModel.Requests[0] = request;

                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    var viewModel = await editModalViewFunction(id: requestItemViewModel.Requests.FirstOrDefault().RequestID, Tab: 1, SectionType: requestItemViewModel.SectionType, selectedPriceSort: new List<string>() { AppUtility.PriceSortEnum.Unit.ToString(), AppUtility.PriceSortEnum.TotalVat.ToString() });
                    viewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    ControllerContext.ModelState.Clear();
                    return PartialView("_EditModalView", viewModel);
                }
            }
            requestItemViewModel = await editModalViewFunction(id: requestItemViewModel.Requests.FirstOrDefault().RequestID, Tab: requestItemViewModel.Tab, SectionType: requestItemViewModel.SectionType, selectedPriceSort: new List<string>() { AppUtility.PriceSortEnum.Unit.ToString(), AppUtility.PriceSortEnum.TotalVat.ToString() });
            return PartialView("_EditModalView", requestItemViewModel);
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(RequestIndexObject requestIndexObject, int? id, String SectionType = "")
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }

            Request request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { x => x.RequestID == id },
               new List<ComplexIncludes<Request, ModelBase>> {
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include= p => ((Product)p).ProductSubcategory} },
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.UnitType},
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.UnitType },
                    new ComplexIncludes<Request, ModelBase>{ Include = r=>r.Product.SubUnitType },
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.SubSubUnitType }
               });
            var unittypes = _unitTypesProc.Read(new List<Expression<Func<UnitType, bool>>> { ut => ut.UnitTypeParentCategory.Where(up => up.ParentCategoryID == request.Product.ProductSubcategory.ParentCategoryID).Count() > 0 },
                new List<ComplexIncludes<UnitType, ModelBase>> {
                                new ComplexIncludes<UnitType, ModelBase>{ Include = u => u.UnitParentType}
                }).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                UnitTypes = unittypes
            };
            request.RequestStatusID = 1;
            requestItemViewModel.Requests = new List<Request>() { request };
            requestItemViewModel.ModalType = AppUtility.RequestModalType.Reorder;
            requestItemViewModel.HasWarnings = _productCommentsProc.Read(new List<Expression<Func<ProductComment, bool>>> { pc => pc.ObjectID == request.ProductID && pc.CommentTypeID == 2 }).Count() > 0;
            requestItemViewModel.HasQuote = _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.ProductID == request.ProductID && r.ParentQuote.ExpirationDate >= DateTime.Now.Date }).Select(r => r.ParentQuote).OrderByDescending(r => r.QuoteDate).Count() > 0;


            TempRequestListViewModel trlvm = new TempRequestListViewModel
            {
                GUID = Guid.NewGuid(),
                RequestIndexObject = requestIndexObject,
                SequencePosition = 0,
                TempRequestViewModels = new List<TempRequestViewModel> { new TempRequestViewModel { Request = request } },
            };

            requestItemViewModel.TempRequestListViewModel = trlvm;
            requestItemViewModel.RequestRoles = await GetUserRequestRoles();

            await _tempRequestJsonsProc.UpdateAsync(trlvm.GUID, trlvm.RequestIndexObject, trlvm, _userManager.GetUserId(User), true);

            return PartialView(requestItemViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(RequestItemViewModel requestItemViewModel, TempRequestListViewModel tempRequestListViewModel, AppUtility.OrderMethod OrderMethod/*, bool isCancel = false*/)
        {
            /*if (isCancel)
            {
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests, tempRequestListViewModel.GUID);
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote, tempRequestListViewModel.GUID);
                await RemoveTempRequestAsync(tempRequestListViewModel.GUID);
                return new EmptyResult();
            }*/
            var userID = _userManager.GetUserId(User);
            try
            {
                switch (requestItemViewModel.OrderType)
                {
                    case AppUtility.OrderType.SingleOrder:
                        requestItemViewModel.Requests[0].Product = requestItemViewModel.Requests[0].SingleOrder;
                        break;
                    case AppUtility.OrderType.RecurringOrder:
                        requestItemViewModel.Requests[0].Product = requestItemViewModel.Requests[0].RecurringOrder;
                        break;
                    case AppUtility.OrderType.StandingOrder:
                        requestItemViewModel.Requests[0].Product = requestItemViewModel.Requests[0].StandingOrder;
                        break;
                };
                //  ReorderViewModel reorderViewModel = JsonConvert.DeserializeObject<ReorderViewModel>(json);
                //get the old request that we are reordering
                var oldRequest = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestItemViewModel.Requests.FirstOrDefault().RequestID },
                   new List<ComplexIncludes<Request, ModelBase>> {
                       new ComplexIncludes<Request, ModelBase> { Include = r => r.Product, ThenInclude =
                         new ComplexIncludes<ModelBase, ModelBase>{ Include = p => ((Product)p).ProductSubcategory, ThenInclude =
                         new ComplexIncludes<ModelBase, ModelBase>{ Include = ps => ((ProductSubcategory)ps).ParentCategory } } },
                       new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.Vendor },
                   new ComplexIncludes<Request, ModelBase> { Include = r => r.OrderMethod }});

                var orderMethodDB = await _orderMethodsProc.ReadOneAsync(new List<Expression<Func<OrderMethod, bool>>> { o => o.DescriptionEnum == OrderMethod.ToString() });
                var currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == _userManager.GetUserId(User) });
                requestItemViewModel.Requests.FirstOrDefault().RequestID = 0;
                requestItemViewModel.Requests.FirstOrDefault().OrderMethod = orderMethodDB;
                requestItemViewModel.Requests.FirstOrDefault().ApplicationUserCreatorID = currentUser.Id;
                requestItemViewModel.Requests.FirstOrDefault().CreationDate = DateTime.Now;
                requestItemViewModel.Requests.FirstOrDefault().SubProjectID = oldRequest.SubProjectID;
                requestItemViewModel.Requests.FirstOrDefault().URL = oldRequest.URL;
                requestItemViewModel.Requests.FirstOrDefault().Warranty = oldRequest.Warranty;
                requestItemViewModel.Requests.FirstOrDefault().ExchangeRate = oldRequest.ExchangeRate;
                requestItemViewModel.Requests.FirstOrDefault().Currency = oldRequest.Currency;
                requestItemViewModel.Requests.FirstOrDefault().IncludeVAT = oldRequest.IncludeVAT;
                oldRequest.Product.UnitTypeID = requestItemViewModel.Requests.FirstOrDefault().Product.UnitTypeID;
                oldRequest.Product.SubUnitTypeID = requestItemViewModel.Requests.FirstOrDefault().Product.SubUnitTypeID;
                oldRequest.Product.SubSubUnitTypeID = requestItemViewModel.Requests.FirstOrDefault().Product.SubSubUnitTypeID;
                oldRequest.Product.SubUnit = requestItemViewModel.Requests.FirstOrDefault().Product.SubUnit;
                oldRequest.Product.SubSubUnit = requestItemViewModel.Requests.FirstOrDefault().Product.SubSubUnit;
                requestItemViewModel.Requests.FirstOrDefault().ProductID = oldRequest.ProductID;
                requestItemViewModel.Requests.FirstOrDefault().Product = oldRequest.Product;
                var isInBudget = await checkIfInBudgetAsync(requestItemViewModel.Requests.FirstOrDefault(), oldRequest.Product);
                requestItemViewModel.TempRequestListViewModel = tempRequestListViewModel;
                TempRequestViewModel newTrvm = await AddItemAccordingToOrderMethod(requestItemViewModel.Requests.FirstOrDefault(), OrderMethod, isInBudget, requestItemViewModel);
                if (OrderMethod != AppUtility.OrderMethod.RequestPriceQuote)
                {
                    try
                    {
                        await _tempRequestJsonsProc.UpdateAsync(tempRequestListViewModel.GUID, tempRequestListViewModel.RequestIndexObject, new TempRequestListViewModel() { TempRequestViewModels = new List<TempRequestViewModel>() { newTrvm } }, userID, false);
                    }
                    catch (Exception ex)
                    {
                        await _tempRequestJsonsProc.RemoveAllAsync(tempRequestListViewModel.GUID, userID);
                        throw new Exception(AppUtility.GetExceptionMessage(ex)); ;
                    }
                }
                else if (tempRequestListViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestRequest)
                {
                    tempRequestListViewModel.RequestIndexObject.RequestStatusID = 6; //redirect to requests instead of received
                }
                else if (tempRequestListViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestRequest)
                {
                    tempRequestListViewModel.RequestIndexObject.RequestStatusID = 6; //redirect to requests instead of received
                }

                var action = "UploadQuoteModal"; //for order now and add to cart
                if (OrderMethod == AppUtility.OrderMethod.AlreadyPurchased)
                {
                    action = "UploadOrderModal";
                }
                else if (OrderMethod == AppUtility.OrderMethod.RequestPriceQuote)
                {
                    return new EmptyResult();
                }
                tempRequestListViewModel.RequestIndexObject.OrderMethod = OrderMethod;
                tempRequestListViewModel.RequestIndexObject.IsReorder = true;
                tempRequestListViewModel.RequestIndexObject.GUID = tempRequestListViewModel.GUID;
                //throw new Exception();
                return RedirectToAction(action, "Requests", tempRequestListViewModel.RequestIndexObject);
            }
            catch (Exception ex)
            {
                await _tempRequestJsonsProc.RemoveAllAsync(tempRequestListViewModel.GUID, userID);
                requestItemViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                /*var unittypes = _proc.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
                requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");*/
                return PartialView("_ErrorMessage", requestItemViewModel.ErrorMessage);
            }
        }




        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmEmailModal(int id, RequestIndexObject requestIndexObject)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }

            TempRequestListViewModel tempRequestListViewModel =
                await LoadTempListFromRequestIndexObjectAsync(requestIndexObject);
            string userID = _userManager.GetUserId(User);
            //var allRequests = new List<Request>();
            //var isRequests = true;
            //var RequestNum = 1;



            TempRequestListViewModel newTRLVM = new TempRequestListViewModel()
            {
                RequestIndexObject = requestIndexObject,
                GUID = requestIndexObject.GUID,
                SequencePosition = tempRequestListViewModel.SequencePosition
            };

            List<Request> allRequests = new List<Request>();
            var pr = tempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest; //will never come into here with more than one parent request
            pr.OrderDate = DateTime.Now;
            tempRequestListViewModel.TempRequestViewModels.ForEach(trvm => allRequests.Add(trvm.Request));
            newTRLVM.TempRequestViewModels = tempRequestListViewModel.TempRequestViewModels;


            if (id != 0) //already has terms, being sent from approve order button -- not in a temprequestjson
            {

            }
            else
            {
                foreach (var tempRequest in newTRLVM.TempRequestViewModels)
                {
                    tempRequest.Request.PaymentStatus = await _paymentStatusesProc.ReadOneAsync(new List<Expression<Func<PaymentStatus, bool>>> { ps => ps.PaymentStatusID == tempRequest.Request.PaymentStatusID });

                    if (tempRequest.Request.Product == null)
                    {
                        tempRequest.Request.Product = await _productsProc.ReadOneAsync(new List<Expression<Func<Product, bool>>> { p => p.ProductID == tempRequest.Request.ProductID }, new List<ComplexIncludes<Product, ModelBase>>{
                        new ComplexIncludes<Product, ModelBase> { Include = p => p.Vendor },
                        new ComplexIncludes<Product, ModelBase> { Include = p => p.ProductSubcategory },
                        new ComplexIncludes<Product, ModelBase> { Include = p => p.ProductSubcategory.ParentCategory },
                    });
                    }
                    else
                    {
                        tempRequest.Request.Product.ProductSubcategory = await _productSubcategoriesProc.ReadOneAsync(new List<Expression<Func<ProductSubcategory, bool>>> { ps => ps.ID == tempRequest.Request.Product.ProductSubcategoryID }, new List<ComplexIncludes<ProductSubcategory, ModelBase>> { new ComplexIncludes<ProductSubcategory, ModelBase> { Include = ps => ps.ParentCategory } });
                        tempRequest.Request.Product.Vendor = await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == tempRequest.Request.Product.VendorID });
                    }
                }

            }


            await _tempRequestJsonsProc.UpdateAsync(newTRLVM.GUID, requestIndexObject, newTRLVM, userID, true);

            //IMPORTANT!!! Check that payments and comments are coming in
            ConfirmEmailViewModel confirm = new ConfirmEmailViewModel
            {
                ParentRequest = pr,
                Requests = allRequests,
                TempRequestListViewModel = newTRLVM
            };

            //render the purchase order view into a string using a the co.nfirmEmailViewModel
            string renderedView = await RenderPartialViewToString("OrderEmailView", confirm);

            string path1 = Path.Combine("wwwroot", AppUtility.ParentFolderName.ParentRequest.ToString());
            string path2 = Path.Combine(path1, requestIndexObject.GUID.ToString());
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }
            string fileName = Path.Combine(path2, "Order.txt");

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                await writer.WriteAsync(renderedView);
            }
            //confirm.RequestIndexObject = requestIndexObject;
            return PartialView(confirm);
        }


        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue)]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmEmailModal(ConfirmEmailViewModel confirmEmailViewModel, TempRequestListViewModel tempRequestListViewModel)
        {
            bool rolledBackTempRequest = false;
            var oldTempRequestJson = new TempRequestJson() { SequencePosition = tempRequestListViewModel.SequencePosition };
            try
            {
                oldTempRequestJson = await _tempRequestJsonsProc.GetTempRequest(tempRequestListViewModel.GUID, _userManager.GetUserId(User)).FirstOrDefaultAsync();

                List<ModelAndID> ModelsCreated = new List<ModelAndID>(); //for rollback
                List<ModelAndID> ModelsModified = new List<ModelAndID>(); //for rollback
                //var isRequests = true;
                //var RequestNum = 1;
                //var PaymentNum = 1;
                //var requests = new List<Request>();
                //var payments = new List<Payment>();


                var deserializedTempRequestListViewModel = new TempRequestListViewModel()
                {
                    TempRequestViewModels =
                    oldTempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels
                };
                try
                {
                    //var pr = tempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest; //eventually(when ready to test all cases) put this in instead of next line and put it in for loop below
                    //deserializedTempRequestListViewModel.TempRequestViewModels.ForEach(t => t.Request.ParentRequest = confirmEmailViewModel.ParentRequest);

                    var userId = deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.ApplicationUserCreatorID ?? _userManager.GetUserId(User); //do we need to do this? (will it ever be null?)

                    var currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == userId });

                    using (var transaction = _applicationDbContextTransaction.Transaction)
                    {
                        try
                        {
                            var parentRequestModelState = new ModelAndState
                            {
                                Model = deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest,
                            };
                            var ParentRequestRollbackList = ModelsModified;
                            if (deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequestID == 0 || deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequestID == null)
                            {
                                parentRequestModelState.StateEnum = EntityState.Added;

                                ParentRequestRollbackList = ModelsCreated;
                            }
                            else //if coming from approve order
                            {
                                parentRequestModelState.StateEnum = EntityState.Modified;
                            }
                            await _requestsProc.UpdateModelsAsync(new List<ModelAndState> { parentRequestModelState });
                            ParentRequestRollbackList.Add(new ModelAndID()
                            {
                                ID = Convert.ToInt32(deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest.ParentRequestID),
                                ModelsEnum = AppUtility.ModelsEnum.ParentRequest
                            });

                            for (int tr = 0; tr < deserializedTempRequestListViewModel.TempRequestViewModels.Count(); tr++)
                            {
                                var tempRequest = deserializedTempRequestListViewModel.TempRequestViewModels[tr];
                                tempRequest.Request.RequestStatusID = 2;
                                tempRequest.Request.ParentRequestID = deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequestID;
                                var ProductRollbackList = ModelsModified;
                                var RequestRollbackList = ModelsModified;
                                var ParentQuoteRollbackList = ModelsModified;
                                var ModelStates = new List<ModelAndState>();

                                var requestModelState = new ModelAndState
                                {
                                    Model = tempRequest.Request,
                                    StateEnum = tempRequest.Request.RequestID == 0 ? EntityState.Added : EntityState.Modified
                                };
                                ModelStates.Add(requestModelState);
                                if (tempRequest.Request.RequestID == 0)
                                {
                                    RequestRollbackList = ModelsCreated;
                                    if (tempRequest.Request.ProductID == 0)
                                    {
                                        tempRequest.Request.Product.SerialNumber = await _requestsProc.GetSerialNumberAsync(false);
                                        ProductRollbackList = ModelsCreated;
                                    }
                                    var productModelState = new ModelAndState
                                    {
                                        Model = tempRequest.Request.Product,
                                        StateEnum = tempRequest.Request.ProductID == 0 ? EntityState.Added : EntityState.Modified
                                    };
                                    await _requestsProc.UpdateModelsAsync(new List<ModelAndState> { productModelState });

                                    var parentQuoteModelState = new ModelAndState
                                    {
                                        Model = tempRequest.Request.ParentQuote,
                                        StateEnum = tempRequest.Request.ParentQuoteID == null || tempRequest.Request.ParentQuoteID == 0 ? EntityState.Added : EntityState.Modified
                                    };
                                    ModelStates.Add(parentQuoteModelState);
                                    if (tempRequest.Request.ParentQuoteID == null || tempRequest.Request.ParentQuoteID == 0)
                                    {
                                        ParentQuoteRollbackList = ModelsCreated;
                                    }
                                }
                                await _requestsProc.UpdateModelsAsync(ModelStates);
                                //await _requestsProc.UpdateModelsAsync(ModelStates);

                                //set up rollback lists in case of exception
                                ProductRollbackList.Add(new ModelAndID()
                                {
                                    ID = Convert.ToInt32(tempRequest.Request.ProductID),
                                    ModelsEnum = AppUtility.ModelsEnum.Product
                                });
                                RequestRollbackList.Add(new ModelAndID()
                                {
                                    ID = Convert.ToInt32(tempRequest.Request.RequestID),
                                    ModelsEnum = AppUtility.ModelsEnum.Request
                                });
                                ParentQuoteRollbackList.Add(new ModelAndID()
                                {
                                    ID = Convert.ToInt32(tempRequest.Request.ParentQuoteID),
                                    ModelsEnum = AppUtility.ModelsEnum.ParentQuote
                                });
                                //if there are no payments it means that the payments were saved previously
                                //bool AddedPayments = false;
                                if (tempRequest.Payments != null)
                                {
                                    foreach (var p in tempRequest.Payments)
                                    {
                                        p.RequestID = tempRequest.Request.RequestID;
                                        _paymentsProc.CreateWithoutSaveChanges(p);
                                        await _paymentsProc.SaveDbChangesAsync();
                                        ModelsCreated.Add(new ModelAndID()
                                        {
                                            ID = p.PaymentID,
                                            ModelsEnum = AppUtility.ModelsEnum.Payment
                                        });
                                    }
                                }
                                if (tempRequest.Comments != null)
                                {
                                    var requestComments = AppData.Json.Deserialize<List<RequestComment>>(AppData.Json.Serialize(tempRequest.Comments.Where(c => c.CommentTypeID == 1)));
                                    await _requestCommentsProc.UpdateWithoutTransactionAsync(requestComments, tempRequest.Request.RequestID, currentUser.Id);
                                    foreach (var c in requestComments)
                                    {
                                        ModelsCreated.Add(new ModelAndID()
                                        {
                                            ID = c.CommentID,
                                            ModelsEnum = AppUtility.ModelsEnum.RequestComment
                                        });
                                    }
                                    var productComments = AppData.Json.Deserialize<List<ProductComment>>(AppData.Json.Serialize(tempRequest.Comments.Where(c => c.CommentTypeID == 2)));
                                    await _productCommentsProc.UpdateWithoutTransactionAsync(productComments, tempRequest.Request.ProductID, currentUser.Id);

                                    foreach (var c in productComments)
                                    {
                                        ModelsCreated.Add(new ModelAndID()
                                        {
                                            ID = c.CommentID,
                                            ModelsEnum = AppUtility.ModelsEnum.ProductComment
                                        });
                                    }
                                }

                                if (tempRequest.Request.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.OrderNow.ToString())
                                {
                                    var additionalRequests = tr + 1 < deserializedTempRequestListViewModel.TempRequestViewModels.Count() ? true : false;
                                    MoveDocumentsOutOfTempFolder(tempRequest.Request.RequestID, AppUtility.ParentFolderName.Requests, additionalRequests, tempRequestListViewModel.GUID);
                                }

                                //tempRequest.Request.Product = await _productsProc.ReadOneAsync( new List<Expression<Func<Product, bool>>> { p => p.ProductID == tempRequest.Request.ProductID }, new List<ComplexIncludes<Product, ModelBase>> { new ComplexIncludes<Product, ModelBase> { Include =p => p.Vendor } });
                                RequestNotification requestNotification = new RequestNotification();
                                requestNotification.RequestID = tempRequest.Request.RequestID;
                                requestNotification.IsRead = false;
                                requestNotification.RequestName = tempRequest.Request.Product.ProductName;
                                requestNotification.ApplicationUserID = tempRequest.Request.ApplicationUserCreatorID;
                                requestNotification.Description = "item ordered";
                                requestNotification.NotificationStatusID = 2;
                                requestNotification.TimeStamp = DateTime.Now;
                                requestNotification.Controller = "Requests";
                                requestNotification.Action = "NotificationsView";
                                requestNotification.NotificationDate = DateTime.Now;
                                await _requestNotificationsProc.CreateWithoutTransactionAsync(requestNotification);
                                ModelsCreated.Add(new ModelAndID()
                                {
                                    ID = requestNotification.NotificationID,
                                    ModelsEnum = AppUtility.ModelsEnum.RequestNotification
                                });
                            }

                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw new Exception("Adding request to db failed-" + AppUtility.GetExceptionMessage(ex));
                        }
                    }
                    try
                    {
                        string uploadFolder = Path.Combine("wwwroot", AppUtility.ParentFolderName.ParentRequest.ToString());
                        string folder2 = Path.Combine(uploadFolder, tempRequestListViewModel.GUID.ToString());
                        string fileName = Path.Combine(folder2, "Order.txt");
                        //read the text file to convert to pdf
                        string renderedView = System.IO.File.ReadAllText(fileName);
                        //delete file
                        System.IO.File.Delete(fileName);
                        //base url needs to be declared - perhaps should be getting from js?
                        //once deployed need to take base url and put in the parameter for converter.convertHtmlString
                        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value.ToString()}";
                        //instantiate a html to pdf converter object
                        HtmlToPdf converter = new HtmlToPdf();

                        PdfDocument doc = new PdfDocument();
                        // create a new pdf document converting an url
                        doc = converter.ConvertHtmlString(renderedView, baseUrl);

                        //save this as orderform
                        string id;
                        if (deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.ParentRequestID != null)
                        {
                            id = deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.ParentRequestID.ToString();
                        }
                        else
                        {
                            id = tempRequestListViewModel.GUID.ToString();
                        }
                        string NewFolder = Path.Combine(uploadFolder, id);
                        //string NewFolder = Path.Combine(uploadFolder, deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.ParentRequestID.ToString());
                        string folderPath = Path.Combine(NewFolder, AppUtility.FolderNamesEnum.Orders.ToString());
                        Directory.CreateDirectory(folderPath); //make sure we don't need one above also??
                        string filePath = Path.Combine(folderPath, "CentarixOrder" + deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.ParentRequest.OrderNumber + ".pdf");

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        doc.Save(filePath);
                        doc.Close();


                        //instatiate mimemessage
                        var message = new MimeMessage();

                        //instantiate the body builder
                        var builder = new BodyBuilder();
                        string ownerEmail = currentUser.Email;
                        string ownerUsername = currentUser.FirstName + " " + currentUser.LastName;
                        string ownerPassword = currentUser.SecureAppPass;
                        deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Product.Vendor = await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Product.VendorID });
                        string vendorEmail = deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Product.Vendor.OrdersEmail;
                        //string vendorEmail = /*firstRequest.Product.Vendor.OrdersEmail;*/ emails.Count() < 1 ? requests.FirstOrDefault().Product.Vendor.OrdersEmail : emails[0];
                        string vendorName = deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Product.Vendor.VendorEnName;

                        //add a "From" Email
                        message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

                        // add a "To" Email
                        string ToEmail = deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Emails?.FirstOrDefault() != null ? deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Emails.FirstOrDefault() : vendorEmail;
                        message.To.Add(new MailboxAddress(ToEmail));

                        //add CC's to email
                        //TEST THIS STATEMENT IF VENDOR IS MISSING AN ORDERS EMAIL
                        if (deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Emails != null)
                        {
                            for (int e = 1; e < deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Emails.Count(); e++)
                            {
                                message.Cc.Add(new MailboxAddress(deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Emails[e]));
                            }
                        }

                        //subject
                        message.Subject = "Order from Centarix to " + vendorName;

                        List<string> quoteNumbers = new List<string>();
                        ParentQuote quoteNumberFromJson = deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.ParentQuote;
                        if (quoteNumberFromJson != null)
                        {
                            quoteNumbers = deserializedTempRequestListViewModel.TempRequestViewModels.Select(trvm => trvm.Request.ParentQuote.QuoteNumber).Distinct().ToList();
                        }
                        else
                        {
                            var parentQuoteIDs = deserializedTempRequestListViewModel.TempRequestViewModels.Select(trvm => trvm.Request.ParentQuoteID).Distinct();
                            quoteNumbers = _parentQuotesProc.Read(new List<Expression<Func<ParentQuote, bool>>> { pq => parentQuoteIDs.Contains(pq.ParentQuoteID) }).Select(pq => pq.QuoteNumber).ToList();
                        }

                        //body
                        builder.TextBody = @"Hello," + "\n\n" + "Please see the attached order for quote number(s) " + string.Join(", ", quoteNumbers) +
                            ". \n\nPlease confirm that you received the order. \n\nThank you.\n"
                            + ownerUsername + "\nCentarix";
                        builder.Attachments.Add(filePath);

                        message.Body = builder.ToMessageBody();

                        //move docs before sending message - sending message should be last thing done
                        if (deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.OrderNow.ToString())
                        {
                            MoveDocumentsOutOfTempFolder(deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentQuoteID == null ? 0 : Convert.ToInt32(deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentQuoteID), AppUtility.ParentFolderName.ParentQuote, false, tempRequestListViewModel.GUID);
                        }

                        using (var client = new SmtpClient())
                        {

                            client.Connect("smtp.gmail.com", 587, false);
                            client.Authenticate(ownerEmail, ownerPassword);// ownerPassword);//
                            client.Timeout = 500000; // 500 seconds
                            try
                            {
                                client.Send(message);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Failed to send email - " + AppUtility.GetExceptionMessage(ex));
                            }
                            client.Disconnect(true);
                        }

                        await _tempRequestJsonsProc.RemoveAllAsync(tempRequestListViewModel.GUID, userId);
                    }
                    catch (Exception ex)
                    {
                        await RollbackRequest(ModelsCreated, ModelsModified, tempRequestListViewModel.GUID, oldTempRequestJson.SequencePosition);
                        rolledBackTempRequest = true;
                        throw new Exception(AppUtility.GetExceptionMessage(ex));

                    }

                    tempRequestListViewModel.RequestIndexObject.RequestStatusID = 2;
                    tempRequestListViewModel.RequestIndexObject.GUID = tempRequestListViewModel.GUID;
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    if (!rolledBackTempRequest)
                    {
                        await _tempRequestJsonsProc.RollbackAsync(tempRequestListViewModel.GUID, oldTempRequestJson.SequencePosition);
                    }
                    await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {

                Response.StatusCode = 500;
                if (!rolledBackTempRequest)
                {
                    await _tempRequestJsonsProc.RollbackAsync(tempRequestListViewModel.GUID, oldTempRequestJson.SequencePosition);
                }
                await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                return new EmptyResult();
            }
            return new EmptyResult();
        }

        public async Task<StringWithBool> RollbackRequest(List<ModelAndID> ModelsCreated, List<ModelAndID> ModelModified, Guid guid, int sequencePosition)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var entries = _applicationDbContextEntries.Entries;
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var ModelStates = new List<ModelAndState>();

                //Move parentquote docs back to parentquote:
                await _tempRequestJsonsProc.RollbackAsync(guid, sequencePosition);
                var oldTempRequestJson = await _tempRequestJsonsProc.GetTempRequest(guid, _userManager.GetUserId(User)).FirstOrDefaultAsync();
                var originalJson = await _tempRequestJsonsProc.GetTempRequest(guid, _userManager.GetUserId(User), 0).FirstOrDefaultAsync();
                var deTLVM = new TempRequestListViewModel()
                {
                    TempRequestViewModels = originalJson.DeserializeJson<FullRequestJson>().TempRequestViewModels
                };
                foreach (var ModelWithID in ModelModified.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.RequestComment))
                {
                    var comment = deTLVM.TempRequestViewModels.Select(t => t.Comments.Where(c => c.CommentID == ModelWithID.ID).FirstOrDefault()).FirstOrDefault();
                    ModelStates.Add(new ModelAndState { Model = comment, StateEnum = EntityState.Modified });
                }
                foreach (var ModelWithID in ModelModified.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.ProductComment))
                {
                    var comment = deTLVM.TempRequestViewModels.Select(t => t.Comments.Where(c => c.CommentID == ModelWithID.ID).FirstOrDefault()).FirstOrDefault();
                    ModelStates.Add(new ModelAndState { Model = comment, StateEnum = EntityState.Modified });
                }
                foreach (var ModelWithID in ModelModified.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.Payment))
                {
                    var payment = deTLVM.TempRequestViewModels.Select(t => t.Payments.Where(c => c.PaymentID == ModelWithID.ID).FirstOrDefault()).FirstOrDefault();
                    ModelStates.Add(new ModelAndState { Model = payment, StateEnum = EntityState.Modified });
                }
                foreach (var ModelWithID in ModelModified.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.Request))
                {
                    var request = deTLVM.TempRequestViewModels.Where(c => c.Request.RequestID == ModelWithID.ID).Select(r => r.Request).FirstOrDefault();
                    ModelStates.Add(new ModelAndState { Model = request, StateEnum = EntityState.Modified });
                }
                foreach (var ModelWithID in ModelModified.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.ParentRequest))
                {
                    var parentRequest = deTLVM.TempRequestViewModels.Where(c => c.Request.ParentRequestID == ModelWithID.ID).Select(r => r.Request.ParentRequest).FirstOrDefault();
                    ModelStates.Add(new ModelAndState { Model = parentRequest, StateEnum = EntityState.Modified });
                    MoveDocumentsBackToTempFolder(Convert.ToInt32(ModelWithID.ID), AppUtility.ParentFolderName.ParentRequest, guid.ToString(), false, true);

                }
                foreach (var ModelWithID in ModelModified.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.Product))
                {
                    var product = deTLVM.TempRequestViewModels.Where(c => c.Request.ProductID == ModelWithID.ID).Select(r => r.Request.Product).FirstOrDefault();
                    ModelStates.Add(new ModelAndState { Model = product, StateEnum = EntityState.Modified });
                }

                await _requestsProc.UpdateModelsAsync(ModelStates);
                ModelStates.Clear();
                foreach (var ModelWithID in ModelsCreated.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.RequestComment))
                {
                    var model6 = await _requestCommentsProc.ReadOneAsync(new List<Expression<Func<RequestComment, bool>>> { pr => pr.CommentID == ModelWithID.ID });
                    ModelStates.Add(new ModelAndState { Model = model6, StateEnum = EntityState.Deleted });
                }
                foreach (var ModelWithID in ModelsCreated.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.ProductComment))
                {
                    var model6 = await _productCommentsProc.ReadOneAsync(new List<Expression<Func<ProductComment, bool>>> { pr => pr.CommentID == ModelWithID.ID });
                    ModelStates.Add(new ModelAndState { Model = model6, StateEnum = EntityState.Deleted });
                }
                foreach (var ModelWithID in ModelsCreated.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.Payment))
                {
                    var model5 = await _paymentsProc.ReadOneAsync(new List<Expression<Func<Payment, bool>>> { pr => pr.PaymentID == ModelWithID.ID });
                    ModelStates.Add(new ModelAndState { Model = model5, StateEnum = EntityState.Deleted });
                }
                foreach (var ModelWithID in ModelsCreated.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.RequestNotification))
                {
                    var model7 = await _requestNotificationsProc.ReadOneAsync(new List<Expression<Func<RequestNotification, bool>>> { pr => pr.NotificationID == ModelWithID.ID });
                    ModelStates.Add(new ModelAndState { Model = model7, StateEnum = EntityState.Deleted });
                }
                foreach (var ModelWithID in ModelsCreated.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.Request))
                {
                    var model = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == ModelWithID.ID });
                    ModelStates.Add(new ModelAndState { Model = model, StateEnum = EntityState.Deleted });
                    MoveDocumentsBackToTempFolder(Convert.ToInt32(ModelWithID.ID), AppUtility.ParentFolderName.Requests, guid.ToString(), true, true);

                }
                foreach (var ModelWithID in ModelsCreated.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.ParentQuote))
                {
                    var model2 = await _parentQuotesProc.ReadOneAsync(new List<Expression<Func<ParentQuote, bool>>> { pr => pr.ParentQuoteID == ModelWithID.ID });
                    ModelStates.Add(new ModelAndState { Model = model2, StateEnum = EntityState.Deleted });
                    MoveDocumentsBackToTempFolder(Convert.ToInt32(ModelWithID.ID), AppUtility.ParentFolderName.ParentQuote, guid.ToString(), false, true);

                }
                foreach (var ModelWithID in ModelsCreated.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.ParentRequest))
                {
                    var model3 = await _parentRequestsProc.ReadOneAsync(new List<Expression<Func<ParentRequest, bool>>> { pr => pr.ParentRequestID == ModelWithID.ID });
                    ModelStates.Add(new ModelAndState { Model = model3, StateEnum = EntityState.Deleted });
                    MoveDocumentsBackToTempFolder(Convert.ToInt32(ModelWithID.ID), AppUtility.ParentFolderName.ParentRequest, guid.ToString(), false, true);

                }
                foreach (var ModelWithID in ModelsCreated.Where(mc => mc.ModelsEnum == AppUtility.ModelsEnum.Product))
                {
                    var model4 = await _productsProc.ReadOneAsync(new List<Expression<Func<Product, bool>>> { pr => pr.ProductID == ModelWithID.ID });
                    ModelStates.Add(new ModelAndState { Model = model4, StateEnum = EntityState.Deleted });
                }
                await _requestsProc.UpdateModelsAsync(ModelStates);
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception e)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(e));
            }
            return ReturnVal;
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmQuoteEmailModal(ConfirmEmailViewModel confirmQuoteEmail, RequestIndexObject requestIndexObject)
        {
            try
            {
                List<Expression<Func<Request, bool>>> wheres = new List<Expression<Func<Request, bool>>>();
                List<ComplexIncludes<Request, ModelBase>> includes = new List<ComplexIncludes<Request, ModelBase>>();
                includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product });
                includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.Vendor });
                includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory });
                includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory.ParentCategory });
                includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentQuote });
                wheres.Add(r => r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.RequestPriceQuote.ToString());

                if (confirmQuoteEmail.IsResend)
                {
                    wheres.Add(r => r.RequestID == confirmQuoteEmail.RequestID);
                }
                else if (confirmQuoteEmail.VendorId != null)
                {
                    wheres.Add(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.QuoteStatusID == 1);
                    wheres.Add(r => r.RequestStatusID == 6);
                }
                else
                {
                    wheres.Add(r => confirmQuoteEmail.Requests.Select(rid => rid.RequestID).Contains(r.RequestID) && (r.QuoteStatusID == 1 || r.QuoteStatusID == 2));
                    wheres.Add(r => r.RequestStatusID == 6);
                }
                var requests = _requestsProc.Read(wheres, includes).ToList();
                if (requests.Count() == 0)
                {
                    wheres.Clear();
                    wheres.Add(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.QuoteStatusID == 2);
                    wheres.Add(r => r.RequestStatusID == 6);
                    requests = _requestsProc.Read(wheres, includes).ToList();
                }
                var success = await _requestsProc.UpdateQuoteStatusAsync(requests, 2);
                if (!success.Bool)
                {
                    throw new Exception(success.String);
                }
                //base url needs to be declared - perhaps should be getting from js?
                //once deployed need to take base url and put in the parameter for converter.convertHtmlString
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value.ToString()}";


                confirmQuoteEmail.Requests = requests.ToList();
                confirmQuoteEmail.TempRequestListViewModel = new TempRequestListViewModel()
                {
                    RequestIndexObject = requestIndexObject
                };

                //render the purchase order view into a string using a the confirmEmailViewModel
                string renderedView = await RenderPartialViewToString("OrderEmailView", confirmQuoteEmail);
                //instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                PdfDocument doc = new PdfDocument();
                // create a new pdf document converting an url
                doc = converter.ConvertHtmlString(renderedView, baseUrl);

                //creating the path for the file to be saved
                string path1 = Path.Combine("wwwroot", AppUtility.ParentFolderName.Requests.ToString());
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                string uniqueFileName = "PriceQuoteRequest.pdf";
                string filePath = Path.Combine(path1, uniqueFileName);
                // save pdf document
                doc.Save(filePath);

                // close pdf document
                doc.Close();

                if (System.IO.File.Exists(filePath))
                {
                    //instatiate mimemessage
                    var message = new MimeMessage();

                    //instantiate the body builder
                    var builder = new BodyBuilder();

                    var currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == _userManager.GetUserId(User) });
                    //   currentUser = _proc.Users.Where(u => u.Id == "702fe06c-22e1-4be8-a515-ea89d6e5ee00").FirstOrDefault();
                    string ownerEmail = currentUser.Email;
                    string ownerUsername = currentUser.FirstName + " " + currentUser.LastName;
                    string ownerPassword = currentUser.SecureAppPass;
                    string vendorQuotesEmail = requests.FirstOrDefault().Product.Vendor.QuotesEmail;
                    string vendorName = requests.FirstOrDefault().Product.Vendor.VendorEnName;

                    //add a "From" Email
                    message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

                    // add a "To" Email
                    message.To.Add(new MailboxAddress(vendorName, vendorQuotesEmail));

                    //subject
                    message.Subject = "Request for a Price Quote from Centarix to " + vendorName;

                    //body
                    builder.TextBody = @"Hello," + "\n\n" + "Please send a price quote for the items listed in the attached pdf. \n\nThank you.\n"
                            + ownerUsername + "\nCentarix"; ;
                    builder.Attachments.Add(filePath);

                    message.Body = builder.ToMessageBody();

                    using (var client = new SmtpClient())
                    {

                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate(ownerEmail, ownerPassword);// ownerPassword);//

                        //"FakeUser@123"); // set up two step authentication and get app password
                        try
                        {
                            client.Send(message);
                        }
                        catch (Exception ex)
                        {
                            await _requestsProc.UpdateQuoteStatusAsync(requests, 1);
                            throw new Exception("Failed to send quote request- " + AppUtility.GetExceptionMessage(ex));
                        }

                        client.Disconnect(true);

                    }
                    return RedirectToAction("LabManageQuotes");
                }

                else
                {
                    await _requestsProc.UpdateQuoteStatusAsync(requests, 1);
                    throw new FileNotFoundException();
                    //return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                var errMessage = AppUtility.GetExceptionMessage(ex);
                return RedirectToAction("LabManageQuotes", new { errorMessage = errMessage });
            }

        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmQuoteEmailModal(int? id = null, int[] requestIds = null, bool isResend = false)
        {
            List<Expression<Func<Request, bool>>> wheres = new List<Expression<Func<Request, bool>>>();
            List<ComplexIncludes<Request, ModelBase>> includes = new List<ComplexIncludes<Request, ModelBase>>();
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.Vendor });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory });
            includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory.ParentCategory });
            wheres.Add(r => r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.RequestPriceQuote.ToString());
            if (isResend)
            {
                wheres.Add(r => r.RequestID == id);
            }
            else
            {
                includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentQuote });
                if (id != null)
                {
                    wheres.Add(r => r.Product.VendorID == id && r.QuoteStatusID == 1);
                    wheres.Add(r => r.RequestStatusID == 6);
                }
                else if (requestIds != null)
                {
                    wheres.Add(r => requestIds.Contains(r.RequestID) && (r.QuoteStatusID == 1 || r.QuoteStatusID == 2));
                    wheres.Add(r => r.RequestStatusID == 6);
                }

            }
            var requests = _requestsProc.Read(wheres, includes).AsEnumerable();
            if (requests.Count() == 0)
            {
                wheres.Clear();
                wheres.Add(r => r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.RequestPriceQuote.ToString());
                wheres.Add(r => r.Product.VendorID == id && r.QuoteStatusID == 2);
                wheres.Add(r => r.RequestStatusID == 6);
                includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentQuote });
                requests = _requestsProc.Read(wheres, includes).AsEnumerable();
            }
            RequestIndexObject requestIndexObject = new RequestIndexObject
            {
                PageType = AppUtility.PageTypeEnum.LabManagementQuotes,
                SidebarType = AppUtility.SidebarEnum.Quotes
            };
            ConfirmEmailViewModel confirmEmail = new ConfirmEmailViewModel
            {
                Requests = requests.ToList(),
                VendorId = id,
                RequestID = id,
                TempRequestListViewModel = new TempRequestListViewModel() { RequestIndexObject = requestIndexObject }
            };

            return PartialView(confirmEmail);
        }



        /*LABMANAGEMENT*/
        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> LabManageQuotes(string errorMessage)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementQuotes;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Quotes;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            return View(await GetIndexViewModelByVendor(new RequestIndexObject
            { SectionType = AppUtility.MenuItems.LabManagement, PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SidebarType = AppUtility.SidebarEnum.Quotes, ErrorMessage = errorMessage }));
        }

        public async Task<IActionResult> _LabManageQuotes(RequestIndexPartialViewModelByVendor labManageQuotesViewModel)
        {
            return PartialView(await GetIndexViewModelByVendor(new RequestIndexObject
            { SectionType = AppUtility.MenuItems.LabManagement, PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SidebarType = AppUtility.SidebarEnum.Quotes }));
        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> LabManageOrders(string errorMessage)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementQuotes;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Orders;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            return View(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.LabManagement, PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SidebarType = AppUtility.SidebarEnum.Orders, ErrorMessage = errorMessage }));
        }
        public async Task<IActionResult> _LabManageOrders(RequestIndexPartialViewModelByVendor labManageQuotesViewModel)
        {
            return PartialView(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.LabManagement, PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SidebarType = AppUtility.SidebarEnum.Orders }));
        }
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> _IndexTableDataByVendor(RequestIndexObject requestIndexObject, NotificationFilterViewModel notificationFilterViewModel)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return PartialView(await GetIndexViewModelByVendor(requestIndexObject, notificationFilterViewModel));
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> _IndexTableByVendor(RequestIndexObject requestIndexObject, NotificationFilterViewModel notificationFilterViewModel)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return PartialView(await GetIndexViewModelByVendor(requestIndexObject, notificationFilterViewModel));
        }

        /*
         * BEGIN SEARCH
         */
        [HttpGet]
        [Authorize(Roles = "Requests, LabManagement, Operations, Accounting")]
        public async Task<IActionResult> Search(AppUtility.MenuItems SectionType, AppUtility.PageTypeEnum PageType, RequestsSearchViewModel requestsSearchViewModel)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Search;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            var categoryID = new List<int> { 1 };
            if (SectionType == AppUtility.MenuItems.Operations)
            {
                categoryID = new List<int> { 2 };
            }
            else if (SectionType == AppUtility.MenuItems.Accounting)
            {
                categoryID = new List<int> { 1, 2 };
            }

            requestsSearchViewModel.ParentCategories = await _parentCategoriesProc.Read(new List<Expression<Func<ParentCategory, bool>>> { pc => categoryID.Contains(pc.CategoryTypeID) }).ToListAsync();
            requestsSearchViewModel.ProductSubcategories = await _productSubcategoriesProc.Read(new List<Expression<Func<ProductSubcategory, bool>>> { ps => categoryID.Contains(ps.ParentCategory.CategoryTypeID) }).ToListAsync();
            requestsSearchViewModel.Vendors = await _vendorsProc.Read(new List<Expression<Func<Vendor, bool>>> { v => v.VendorCategoryTypes.Where(vc => categoryID.Contains(vc.CategoryTypeID)).Count() > 0 }).ToListAsync();
            requestsSearchViewModel.ApplicationUsers = await _employeesProc.Read().ToListAsync();
            requestsSearchViewModel.SectionType = SectionType;
            requestsSearchViewModel.PageType = PageType;
            requestsSearchViewModel.SidebarEnum = AppUtility.SidebarEnum.Search;
            requestsSearchViewModel.Payment = new Payment();
            if (PageType == AppUtility.PageTypeEnum.AccountingGeneral)
            {
                requestsSearchViewModel.PaymentTypes = _paymentTypesProc.Read().AsEnumerable();
                requestsSearchViewModel.CompanyAccounts = _companyAccountsProc.Read().AsEnumerable();
            }

            return View(requestsSearchViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Requests, LabManagement, Operations, Accounting")]
        public async Task<IActionResult> Search(RequestsSearchViewModel requestsSearchViewModel, RequestIndexObject requestIndexObject)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;

            var viewModel = await base.GetIndexViewModel(requestIndexObject, Years: new List<int>() { DateTime.Now.Year }, Months: new List<int>() { DateTime.Now.Month }, requestsSearchViewModel: requestsSearchViewModel);
            viewModel.RequestsSearchViewModel = requestsSearchViewModel;
            viewModel.SidebarFilterName = AppUtility.SidebarEnum.Search.ToString();
            return PartialView("SearchResults", viewModel);
        }


        /*
         * START RECEIVED MODAL
         */

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReceivedModal(int RequestID, RequestIndexObject requestIndexObject)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            //foreach(var li in _proc.LocationInstances)
            //{
            //    li.IsFull = false;
            //    _proc.Update(li);
            //}
            //_proc.SaveChanges();
            var request = _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.RequestID == RequestID }).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.Product.UnitType)
                    .FirstOrDefault();

            AppUtility.OrderType orderType = AppUtility.OrderType.SingleOrder;
            Enum.TryParse(request.Product.GetType().ToString(), out orderType);
            ReceivedLocationViewModel receivedLocationViewModel = new ReceivedLocationViewModel()
            {
                Request = request,
                locationTypesDepthZero = _locationTypesProc.Read(new List<Expression<Func<LocationType, bool>>> { lt => lt.Depth == 0 }).AsEnumerable(),
                locationInstancesSelected = new List<LocationInstance>(),
                //ApplicationUsers = await _proc.Users.Where(u => !u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null).ToListAsync(),
                RequestIndexObject = requestIndexObject,
                PageRequestStatusID = request.RequestStatusID,
                OrderType = orderType
            };
            receivedLocationViewModel.locationInstancesSelected.Add(new LocationInstance());
            var currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == _userManager.GetUserId(User) });
            receivedLocationViewModel.Request.ApplicationUserReceiverID = currentUser.Id;
            receivedLocationViewModel.Request.ApplicationUserReceiver = currentUser;
            receivedLocationViewModel.Request.ArrivalDate = DateTime.Today;
            receivedLocationViewModel.CategoryType = receivedLocationViewModel.Request.Product.ProductSubcategory.ParentCategory.CategoryTypeID;
            return PartialView(receivedLocationViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReceivedModalSublocations(int LocationTypeID)
        {
            ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel = new ReceivedModalSublocationsViewModel()
            {
                locationInstancesDepthZero = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationTypeID == LocationTypeID && !(li is TemporaryLocationInstance) },
                new List<ComplexIncludes<LocationInstance, ModelBase>> { new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LocationRoomInstance }, new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LabPart } })
                .OrderBy(li => li.LocationNumber),
                locationTypeNames = new List<string>(),
                locationInstancesSelected = new List<LocationInstance>(),
            };
            bool finished = false;
            int locationTypeIDLoop = LocationTypeID;
            if (LocationTypeID == 500)
            {
                receivedModalSublocationsViewModel.LabPartTypes = _labPartsProc.Read().AsEnumerable();
            }
            while (!finished)
            {
                //need to get the whole thing b/c need both the name and the child id so it's instead of looping through the list twice
                var nextType = await _locationTypesProc.ReadOneAsync(new List<Expression<Func<LocationType, bool>>> { lt => lt.LocationTypeID == locationTypeIDLoop });
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
        public async Task<IActionResult> ReceivedModalVisual(int LocationInstanceID, int RequestID, bool ShowIcons = false)
        {
            ReceivedModalVisualViewModel receivedModalVisualViewModel = new ReceivedModalVisualViewModel()
            {
                IsEditModalTable = false,
                ShowIcons = ShowIcons
            };

            var parentLocationInstance = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { m => m.LocationInstanceID == LocationInstanceID });

            var firstChildLI = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceParentID == parentLocationInstance.LocationInstanceID });
            LocationInstance secondChildLi = null;
            if (firstChildLI != null)
            {
                secondChildLi = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceParentID == firstChildLI.LocationInstanceID }); //second child is to ensure it doesn't have any box units
            }

            if (secondChildLi != null || parentLocationInstance.LocationTypeID == 500)//case for 25
            {
                receivedModalVisualViewModel.DeleteTable = true;
            }
            else
            {
                //if it's an empty shelf- reset the location to the parent location instance id:
                if (/*parentLocationInstance.LocationTypeID == 201 &&*/ parentLocationInstance.IsEmptyShelf && parentLocationInstance.LabPartID == null)
                {
                    parentLocationInstance = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == parentLocationInstance.LocationInstanceParentID });
                    LocationInstanceID = parentLocationInstance.LocationInstanceID;
                }

                receivedModalVisualViewModel.ParentLocationInstance = parentLocationInstance;

                if (receivedModalVisualViewModel.ParentLocationInstance != null)
                {
                    var request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == RequestID },
                        new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include = r => r.RequestLocationInstances, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = rli => ((RequestLocationInstance)rli).LocationInstance } } });
                    IQueryable<LocationInstance> childrenLocationInstances = null;
                    if (receivedModalVisualViewModel.ParentLocationInstance.IsEmptyShelf && receivedModalVisualViewModel.ParentLocationInstance.LabPartID != null)
                    {
                        childrenLocationInstances =
                        _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { m => m.LocationInstanceID == LocationInstanceID },
                        new List<ComplexIncludes<LocationInstance, ModelBase>> { new ComplexIncludes<LocationInstance, ModelBase> { Include = m => m.RequestLocationInstances } }).OrderBy(m => m.LocationNumber);
                        receivedModalVisualViewModel.ChildrenLocationInstances = childrenLocationInstances.ToList();
                    }
                    else
                    {
                        childrenLocationInstances =
                             _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { m => m.LocationInstanceParentID == LocationInstanceID },
                        new List<ComplexIncludes<LocationInstance, ModelBase>> { new ComplexIncludes<LocationInstance, ModelBase> { Include = m => m.RequestLocationInstances } })
                             .OrderBy(m => m.LocationNumber);
                        receivedModalVisualViewModel.ChildrenLocationInstances = childrenLocationInstances.ToList();
                    }

                    List<LocationInstancePlace> liPlaces = new List<LocationInstancePlace>();
                    if (request != null)
                    {
                        var requestLocationInstances = request.RequestLocationInstances.ToList();
                        receivedModalVisualViewModel.RequestChildrenLocationInstances =
                             childrenLocationInstances.Select(li => new RequestChildrenLocationInstances()
                             {
                                 LocationInstance = li,
                                 IsThisRequest = li.RequestLocationInstances.Select(rli => rli.RequestID).Where(i => i == RequestID).Any()
                             }).OrderBy(m => m.LocationInstance.LocationNumber).ToList();

                        foreach (var cli in receivedModalVisualViewModel.RequestChildrenLocationInstances)
                        {
                            liPlaces.Add(new LocationInstancePlace()
                            {
                                LocationInstanceId = cli.LocationInstance.LocationInstanceID,
                                Placed = cli.IsThisRequest
                            });
                        }
                    }
                    else
                    {
                        foreach (var cli in receivedModalVisualViewModel.ChildrenLocationInstances)
                        {
                            liPlaces.Add(new LocationInstancePlace()
                            {
                                LocationInstanceId = cli.LocationInstanceID,
                                Placed = false
                            });
                        }
                    }
                    receivedModalVisualViewModel.LocationInstancePlaces = liPlaces;
                }
            }
            return PartialView(receivedModalVisualViewModel);
        }
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReceivedModal(ReceivedLocationViewModel receivedLocationViewModel, ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel, ReceivedModalVisualViewModel receivedModalVisualViewModel, RequestsSearchViewModel requestsSearchViewModel = null, SelectedRequestFilters selectedFilters = null, int numFilters = 0)
        {
            using (var transaction = _applicationDbContextTransaction.Transaction)
            {
                try
                {
                    var requestReceived = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == receivedLocationViewModel.Request.RequestID },
                        new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include = r => r.Product } ,
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.Vendor}, new ComplexIncludes<Request, ModelBase>{ Include =r => r.Product.ProductSubcategory},
                         new ComplexIncludes<Request, ModelBase>{ Include =r => r.Product.ProductSubcategory.ParentCategory}});

                    decimal pricePerUnit;
                    if (receivedLocationViewModel.IsPartial)
                    {
                        _requestsProc.CreatePartialRequest(receivedLocationViewModel, requestReceived, out pricePerUnit);
                        MoveDocumentsOutOfTempFolder(requestReceived.RequestID, AppUtility.ParentFolderName.Requests, receivedLocationViewModel.Request.RequestID, true);

                        await _requestCommentsProc.CopyCommentsAsync(receivedLocationViewModel.Request.RequestID, requestReceived.RequestID);
                        await _paymentsProc.CopyPaymentsAsync(receivedLocationViewModel.Request.RequestID, requestReceived.RequestID);

                        requestReceived = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == receivedLocationViewModel.Request.RequestID },
                            new List<ComplexIncludes<Request, ModelBase>> {
                            new ComplexIncludes<Request, ModelBase> { Include = r => r.Product },
                            new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.Vendor },
                            new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory },
                            new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory.ParentCategory }
                        });
                        pricePerUnit = requestReceived.PricePerUnit;
                        requestReceived.Unit = (uint)receivedLocationViewModel.AmountArrived;
                        requestReceived.Cost = pricePerUnit * requestReceived.Unit;
                    }
                    await _requestsProc.ReceiveRequestWithoutTransactionAsync(receivedLocationViewModel, receivedModalVisualViewModel, requestReceived);
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    receivedLocationViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    Response.StatusCode = 500;
                    /*receivedLocationViewModel.locationTypesDepthZero = _proc.LocationTypes.Where(lt => lt.Depth == 0);
                    var userid = _userManager.GetUserId(User);
                    receivedLocationViewModel.Request.ApplicationUserReceiver = _proc.Users.Where(u => u.Id == userid).FirstOrDefault();
                    receivedLocationViewModel.Request.ApplicationUserReceiverID = userid;
                    receivedLocationViewModel.Request = _proc.Requests.Where(r => r.RequestID == receivedLocationViewModel.Request.RequestID).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                    .FirstOrDefault();
                    return PartialView("ReceivedModal", receivedLocationViewModel);*/
                    return PartialView("_ErrorMessage", receivedLocationViewModel.ErrorMessage);
                }

            }

            return PartialView("_IndexTableData", await GetIndexViewModel(receivedLocationViewModel.RequestIndexObject, selectedFilters: selectedFilters, numFilters: numFilters, requestsSearchViewModel: requestsSearchViewModel));


        }





        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReceivedModalVisual(ReceivedModalVisualViewModel receivedModalVisualViewModel, List<Request> Requests)
        {
            var success = await _requestLocationInstancesProc.UpdateAsync(receivedModalVisualViewModel, Requests.FirstOrDefault().RequestID);
            if (!success.Bool)
            {
                var requestItemViewModel = await editModalViewFunction(Requests.FirstOrDefault().RequestID, isEditable: false);
                requestItemViewModel.ErrorMessage = success.String;
                Response.StatusCode = 500;
                return PartialView("_LocationTab", requestItemViewModel);
            }
            else
            {
                return new EmptyResult();
            }
        }

        /*
         * END RECEIVED MODAL
         */
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public ActionResult DocumentsModal(string id, Guid Guid, AppUtility.FolderNamesEnum RequestFolderNameEnum,
            AppUtility.ParentFolderName parentFolderName)
        {
            //if (!AppUtility.IsAjaxRequest(Request))
            //{
            //    return PartialView("InvalidLinkPage");
            //}
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                FolderName = RequestFolderNameEnum,
                ParentFolderName = parentFolderName,
                ObjectID = id == "" ? "0" : id,
                Guid = Guid
            };

            FillDocumentsViewModel(documentsModalViewModel);
            var json = JsonConvert.SerializeObject(documentsModalViewModel, Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       Converters = new List<JsonConverter> { new StringEnumConverter() },
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
            return Json(json);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public ActionResult _DocumentsModalData(string id, Guid Guid, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
          AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Requests)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                FolderName = RequestFolderNameEnum,
                IsEdittable = IsEdittable,
                ParentFolderName = parentFolderName,
                ObjectID = id == "" ? "0" : id,
                SectionType = SectionType,
                ShowSwitch = showSwitch,
                Guid = Guid
            };

            FillDocumentsViewModel(documentsModalViewModel);
            return PartialView(documentsModalViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public JsonResult _DocumentsCard(AppUtility.FolderNamesEnum requestFolderNameEnum, string id, AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Requests)
        {
            //if (!AppUtility.IsAjaxRequest(Request))
            //{
            //    return PartialView("InvalidLinkPage");
            //}
            string requestParentFolder = Path.Combine(_hostingEnvironment.WebRootPath, parentFolderName.ToString());
            var requestFolder = Path.Combine(requestParentFolder, id);
            DocumentFolder DocumentInfo = GetExistingFileStrings(requestFolderNameEnum, parentFolderName, requestFolder, id);
            var options = new JsonSerializerSettings { Converters = new List<JsonConverter> { new StringEnumConverter() } };
            var docinfo = Json(JsonConvert.SerializeObject(DocumentInfo, options));
            return docinfo;
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> DocumentsModal(/*[FromBody]*/ DocumentsModalViewModel documentsModalViewModel)
        {
            base.DocumentsModal(documentsModalViewModel);
            return RedirectToAction("_DocumentsCard", new { requestFolderNameEnum = documentsModalViewModel.FolderName, id= documentsModalViewModel.ObjectID, parentFolderName = documentsModalViewModel.ParentFolderName});
        }


        [HttpGet]
        public ActionResult DeleteDocumentModal(String FileString, int id, AppUtility.FolderNamesEnum RequestFolderNameEnum, AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Requests)
        {

            //if (!AppUtility.IsAjaxRequest(Request))
            //{
            //    return PartialView("InvalidLinkPage");
            //}
            DeleteDocumentsViewModel deleteDocumentsViewModel = new DeleteDocumentsViewModel()
            {
                FileName = FileString,
                ObjectID = id,
                ParentFolderName = parentFolderName,
                FolderName = RequestFolderNameEnum,
            };

            var json = JsonConvert.SerializeObject(deleteDocumentsViewModel, Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       Converters = new List<JsonConverter> { new StringEnumConverter() },
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
            return Json(json);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocumentModal(DeleteDocumentsViewModel deleteDocumentsViewModel)
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
                }
            }
            return RedirectToAction("DocumentsModal", new { id=deleteDocumentsViewModel.ObjectID, Guid=new Guid(), RequestFolderNameEnum= deleteDocumentsViewModel.FolderName, parentFolderName = deleteDocumentsViewModel.ParentFolderName });
        }

        [HttpGet] //send a json to that the subcategory list is filered
        public JsonResult GetSubCategoryList(int ParentCategoryId)
        {
            var subCategoryList = _productSubcategoriesProc.Read(new List<Expression<Func<ProductSubcategory, bool>>> { c => c.ParentCategoryID == ParentCategoryId }).ToList();
            return Json(subCategoryList);

        }


        public bool CheckUniqueVendorAndCatalogNumber(int VendorID, string CatalogNumber, int? ProductID = null)
        {
            var boolCheck = true;
            //validation for the create
            if (VendorID != 0 && CatalogNumber != null && (ProductID == null && _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.Product.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID }).Any()))
            {
                return false;
            }
            //validation for the edit
            //var product = _proc.Requests.Where(r => r.Product.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID && r.ProductID != ProductID);
            if (ProductID != null && _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.Product.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID && r.ProductID != ProductID }).Any())
            {
                return false;
            }
            return boolCheck;
        }

        public bool CheckUniqueVendorAndInvoiceNumber(int VendorID, string InvoiceNumber)
        {
            return _invoicesProc.CheckUniqueVendorAndInvoiceNumber(VendorID, InvoiceNumber);
        }


        [HttpGet]
        public JsonResult GetSublocationInstancesList(int locationInstanceParentId, int labPartID)
        {
            List<LocationInstance> locationInstanceList = new List<LocationInstance>();
            if (labPartID != 0)
            {
                locationInstanceList = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceParentID == locationInstanceParentId && li.LabPartID == labPartID }, new List<ComplexIncludes<LocationInstance, ModelBase>> { new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LabPart } }).OrderBy(li => li.LocationNumber).ToList();

            }
            else
            {
                locationInstanceList = _locationInstancesProc.Read(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceParentID == locationInstanceParentId }, new List<ComplexIncludes<LocationInstance, ModelBase>> { new ComplexIncludes<LocationInstance, ModelBase> { Include = li => li.LabPart } }).OrderBy(li => li.LocationNumber).ToList();
            }
            return Json(locationInstanceList);
        }

        [HttpGet]
        [Authorize(Roles = "Requests, Operations")]
        public async Task<IActionResult> Approve(int id, RequestIndexObject requestIndex)
        {
            var request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == id },
                new List<ComplexIncludes<Request, ModelBase>>
                {
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentQuote },
                     new ComplexIncludes<Request, ModelBase> { Include = r => r.OrderMethod },
                      new ComplexIncludes<Request, ModelBase> { Include = r => r.PaymentStatus },
                    new ComplexIncludes<Request, ModelBase> {
                        Include = r => r.Product,
                        ThenInclude = new ComplexIncludes<ModelBase, ModelBase>
                        {
                            Include = p => ((Product)p).ProductSubcategory,
                            ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = ps => ((ProductSubcategory)ps).ParentCategory}
                        }
                    },
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.Vendor },
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.ParentRequest }
                });
            try
            {
                switch (Enum.Parse(typeof(AppUtility.OrderMethod), request.OrderMethod.DescriptionEnum))
                {
                    case AppUtility.OrderMethod.OrderNow:
                        TempRequestListViewModel trlvm = new TempRequestListViewModel()
                        {
                            GUID = Guid.NewGuid(),
                            TempRequestViewModels = new List<TempRequestViewModel>()
                            {
                                new TempRequestViewModel()
                                {
                                    Request = request
                                }
                            }
                        };
                        await _tempRequestJsonsProc.UpdateAsync(trlvm.GUID, requestIndex, trlvm, _userManager.GetUserId(User), true);
                        return RedirectToAction("ConfirmEmailModal", new { id = id, Guid = trlvm.GUID });
                        break;
                    case AppUtility.OrderMethod.AlreadyPurchased:
                        break;
                    case AppUtility.OrderMethod.RequestPriceQuote:
                    case AppUtility.OrderMethod.AddToCart:
                        using (var transaction = _applicationDbContextTransaction.Transaction)
                        {
                            try
                            {
                                request.RequestStatusID = 6; //approved
                                var requestModelState = new ModelAndState { Model = request, StateEnum = EntityState.Modified };
                                await _requestsProc.UpdateModelsAsync(new List<ModelAndState> { requestModelState });
                                RequestNotification requestNotification = new RequestNotification();
                                requestNotification.RequestID = request.RequestID;
                                requestNotification.IsRead = false;
                                requestNotification.RequestName = request.Product.ProductName;
                                requestNotification.ApplicationUserID = request.ApplicationUserCreatorID;
                                requestNotification.Description = "item approved";
                                requestNotification.NotificationStatusID = 3;
                                requestNotification.NotificationDate = DateTime.Now;
                                requestNotification.Controller = "Requests";
                                requestNotification.Action = "NotificationsView";
                                await _requestNotificationsProc.CreateWithoutTransactionAsync(requestNotification);
                                //throw new Exception();
                                await transaction.CommitAsync();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception(ex.Message);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                //await Response.WriteAsync(errorMessage);
                return PartialView("_ErrorMessage", errorMessage);
            }
            return await RedirectRequestsToShared("_IndexTableWithCounts", requestIndex);

        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> EditQuoteDetails(int id, int[] requestIds = null)
        {
            StringWithBool Error = new StringWithBool();
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            List<Expression<Func<Request, bool>>> Wheres = new List<Expression<Func<Request, bool>>>();
            List<ComplexIncludes<Request, ModelBase>> Includes = new List<ComplexIncludes<Request, ModelBase>>();
            Wheres.Add(r => r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.RequestPriceQuote.ToString());
            Wheres.Add(r => requestIds.Contains(r.RequestID));
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.Vendor, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = v => ((Vendor)v).Country } });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.UnitType });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.SubUnitType });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.SubSubUnitType });

            var requests = await _requestsProc.Read(Wheres, Includes).ToListAsync();

            var exchangeRate = await GetExchangeRateAsync();

            var VendorID = requests.FirstOrDefault().Product.VendorID;

            foreach (var request in requests)
            {
                request.ExchangeRate = exchangeRate;
                request.IncludeVAT = true;
                if (request.Product.VendorID != VendorID)
                {
                    Error.SetStringAndBool(true, ElixirStrings.ServerDifferentVendorErrorMessage);
                }
            }
            EditQuoteDetailsViewModel editQuoteDetailsViewModel = new EditQuoteDetailsViewModel()
            {
                Requests = requests,
                ParentQuote = new ParentQuote() { QuoteDate = DateTime.Now },
                Error = Error
            };
            return PartialView(editQuoteDetailsViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> EditQuoteDetails(EditQuoteDetailsViewModel editQuoteDetailsViewModel)
        {
            try
            {
                using (var transaction = _applicationDbContextTransaction.Transaction)
                {
                    var requests = _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.RequestPriceQuote.ToString() },
                        new List<ComplexIncludes<Request, ModelBase>>
                        {
                            new ComplexIncludes<Request, ModelBase>{ Include = x => x.ParentQuote}
                        }
                    );
                    /*var firstRequest = requests.Where(r => r.RequestID == editQuoteDetailsViewModel.Requests[0].RequestID).FirstOrDefault();
                    int? parentQuoteId = firstRequest.ParentQuoteID;*/
                    try
                    {
                        //var quoteDate = editQuoteDetailsViewModel.QuoteDate;
                        //var quoteNumber = editQuoteDetailsViewModel.QuoteNumber;
                        //firstRequest.ParentQuote.QuoteDate = quoteDate;
                        var parentQuoteModelState = new ModelAndState
                        {
                            Model = editQuoteDetailsViewModel.ParentQuote,
                            StateEnum = EntityState.Added
                        };

                        await _requestsProc.UpdateModelsAsync(new List<ModelAndState> { parentQuoteModelState });

                        await _requestsProc.UpdateQuoteDetailsAsync(editQuoteDetailsViewModel.Requests, editQuoteDetailsViewModel.ParentQuote);

                        //save file
                        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.ParentQuote.ToString());
                        string requestFolder = Path.Combine(uploadFolder, requests.Where(r => r.RequestID == editQuoteDetailsViewModel.Requests[0].RequestID).FirstOrDefault().ParentQuoteID.ToString());
                        string folderPath = Path.Combine(requestFolder, AppUtility.FolderNamesEnum.Quotes.ToString());
                        Directory.CreateDirectory(folderPath);
                        string uniqueFileName = 1 + editQuoteDetailsViewModel.QuoteFileUpload.FileName;
                        string filePath = Path.Combine(folderPath, uniqueFileName);
                        var fileStream = new FileStream(filePath, FileMode.Create);
                        editQuoteDetailsViewModel.QuoteFileUpload.CopyTo(fileStream);
                        fileStream.Close();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote, Guid.Empty, (int)requests.FirstOrDefault().ParentQuoteID);
                        throw new Exception(AppUtility.GetExceptionMessage(ex));
                    }
                }

                return RedirectToAction("_IndexTableDataByVendor", new { PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SectionType = AppUtility.MenuItems.LabManagement, SideBarType = AppUtility.SidebarEnum.Quotes });
            }
            catch (Exception ex)
            {
                for (int i = 0; i < editQuoteDetailsViewModel.Requests.Count(); i++)
                {
                    var previousRequest = await _requestsProc.ReadOneAsync(
                        new List<Expression<Func<Request, bool>>>
                        {
                        r => r.RequestID == editQuoteDetailsViewModel.Requests[i].RequestID,
                        },
                        new List<ComplexIncludes<Request, ModelBase>>
                        {
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = p => ((Product)p).Vendor } },
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.Vendor.Country },
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.ProductSubcategory },
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.UnitType },
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.SubUnitType },
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product.SubSubUnitType },
                        new ComplexIncludes<Request, ModelBase>{ Include = r => r.ParentQuote}
                        }
                    );
                    var newRequest = editQuoteDetailsViewModel.Requests[i];
                    previousRequest.Cost = newRequest.Cost;
                    previousRequest.Currency = newRequest.Currency;
                    previousRequest.ExpectedSupplyDays = newRequest.ExpectedSupplyDays;
                    editQuoteDetailsViewModel.Requests[i] = previousRequest;
                }
                editQuoteDetailsViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                return PartialView(editQuoteDetailsViewModel);
            }

        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> NotificationsView(int requestID = 0, bool DidntArrive = false)
        {
            IEnumerable<RequestNotification> requestNotifications = null;
            if (DidntArrive)
            {
                requestNotifications = _requestNotificationsProc.Read(new List<Expression<Func<RequestNotification, bool>>> { rn => rn.NotificationStatusID == 1 }, new List<ComplexIncludes<RequestNotification, ModelBase>> { new ComplexIncludes<RequestNotification, ModelBase> { Include = n => n.NotificationStatus }, new ComplexIncludes<RequestNotification, ModelBase> { Include = r => r.Request } }).AsEnumerable();
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.DidntArrive;
            }
            else
            {
                requestNotifications = _requestNotificationsProc.Read(new List<Expression<Func<RequestNotification, bool>>> { rn => rn.NotificationStatusID != 1 }, new List<ComplexIncludes<RequestNotification, ModelBase>> { new ComplexIncludes<RequestNotification, ModelBase> { Include = n => n.NotificationStatus }, new ComplexIncludes<RequestNotification, ModelBase> { Include = r => r.Request } }).AsEnumerable();
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Notifications;
            }
            if (requestID != 0)
            {
                var notification = await _requestNotificationsProc.ReadOneAsync(new List<Expression<Func<RequestNotification, bool>>> { rn => rn.NotificationID == requestID });
                await _requestNotificationsProc.MarkNotficationAsReadAsync(notification);
            }

            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            ApplicationUser currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == _userManager.GetUserId(User) });
            var requests = requestNotifications.Where(n => n.ApplicationUserID == currentUser.Id).OrderByDescending(n => n.TimeStamp).ToList();
            return View(requests);
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> Cart(RequestIndexObject requestIndexObject)
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            return View(await GetIndexViewModelByVendor(requestIndexObject));
        }


        [HttpGet]
        [Authorize(Roles = "Requests, Users, Biomarkers, Accounting, Admin, Reports, Timekeeper, Operations, Protocols, Income, Operation, Expenses, LabManagement")]
        public async Task<IActionResult> ConfirmEdit(AppUtility.MenuItems MenuItem = AppUtility.MenuItems.Requests)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return PartialView(MenuItem);
        }

        [HttpGet]
        [Authorize(Roles = "Requests, Users, Biomarkers, Accounting, Admin, Reports, Timekeeper, Operations, Protocols, Income, Operation, Expenses, LabManagement")]
        public async Task<IActionResult> ConfirmExit(String TempRequestGUID, AppUtility.MenuItems MenuItem = AppUtility.MenuItems.Requests, string url = "")
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            ConfirmExitViewModel confirmExit = new ConfirmExitViewModel()
            {
                SectionType = MenuItem,
                URL = url,
                GUID = TempRequestGUID == Guid.NewGuid().ToString() || TempRequestGUID == "undefined" || TempRequestGUID == null ? Guid.NewGuid() : Guid.Parse(TempRequestGUID)
            };
            return PartialView(confirmExit);
        }
        [HttpPost]
        [Authorize(Roles = "Requests, Users, Biomarkers, Accounting, Admin, Reports, Timekeeper, Operations, Protocols, Income, Operation, Expenses, LabManagement")]
        public async Task<IActionResult> ConfirmExit(ConfirmExitViewModel confirmExit)
        {
            DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests, confirmExit.GUID);
            DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote, confirmExit.GUID);
            await _tempRequestJsonsProc.RemoveAllAsync(confirmExit.GUID, _userManager.GetUserId(User));

            if (confirmExit.URL.IsEmpty())
            {
                var requestIndex = new RequestIndexObject()
                {
                    PageType = confirmExit.PageType,
                    SectionType = confirmExit.SectionType
                };
                return RedirectToAction("Index", requestIndex);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> InstallmentsPartial(int index)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return PartialView(index);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> OrderLateModal(int id)

        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == id },
                new List<ComplexIncludes<Request, ModelBase>> {
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.ApplicationUserCreator },
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.ParentRequest },
                    new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = p =>( (Product)p).Vendor} },
                });

            return PartialView(request);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task OrderLateModal(Request requestFromView)
        {
            var request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestFromView.RequestID },
                new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include =  r => r.ApplicationUserCreator },
                new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentRequest},
                new ComplexIncludes<Request, ModelBase> { Include =  r => r.Product },
                new ComplexIncludes<Request, ModelBase> { Include =  r => r.Product.Vendor }
                }
             );
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
            message.Subject = "Please update on delayed supply for Centarix's Order #" + request.ParentRequest.OrderNumber;

            //body
            builder.TextBody = $"Hello,\n\nOrder number {request.ParentRequest.OrderNumber} for {request.Product.ProductName}" +
                $" which was scheduled to arrive on {AppUtility.GetElixirDateFormat(request.ParentRequest.OrderDate.AddDays((double)request.ExpectedSupplyDays))}, " +
                $"has not arrived yet. \n" +
                    $"Please update us on the matter.\n\n" +
                    $"Best regards,\n" +
                    $"{ownerUsername}\n" +
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
                    Response.StatusCode = 500;
                    await Response.WriteAsync(AppUtility.GetExceptionMessage(ex));
                }

                client.Disconnect(true);

            }
            //return RedirectToAction("NotificationsView", new { DidntArrive = true });

        }


        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AccountingPayments(AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment, String ErrorMessage = null)
        {
            var payNowCount = await GetPaymentRequests(AppUtility.SidebarEnum.PayNow);
            TempData["PayNowCount"] = payNowCount.Count();
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Accounting;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingPayments;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = accountingPaymentsEnum;
            var viewModel = await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.Accounting, PageType = AppUtility.PageTypeEnum.AccountingPayments, SidebarType = accountingPaymentsEnum });
            viewModel.ErrorMessage = ErrorMessage;
            return View(viewModel);

        }
        private async Task<List<RequestPaymentsViewModel>> GetPaymentRequests(AppUtility.SidebarEnum accountingPaymentsEnum, List<Expression<Func<Request, bool>>> wheres = null)
        {

            if (wheres == null)
            {
                wheres = new List<Expression<Func<Request, bool>>>();
            }

            var includes = new List<ComplexIncludes<Request, ModelBase>> {
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.Product, ThenInclude= new ComplexIncludes<ModelBase, ModelBase> { Include =p => ((Product)p).Vendor } },
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory, ThenInclude= new ComplexIncludes<ModelBase, ModelBase> { Include =p => ((ProductSubcategory)p).ParentCategory, ThenInclude = new ComplexIncludes<ModelBase, ModelBase> { Include = pc=> ((ParentCategory)pc).CategoryType } } },
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.UnitType },
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.SubUnitType },
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.SubSubUnitType },
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentRequest },
                    new ComplexIncludes<Request, ModelBase> { Include = r => r.Payments, ThenInclude= new ComplexIncludes<ModelBase, ModelBase> { Include =p => ((Payment)p).Invoice } } };
            wheres.Add(r => r.RequestStatusID != 7 && r.Payments.Where(p => !p.IsPaid).Count() > 0);
            var requestPaymentList = new List<RequestPaymentsViewModel>();
            Func<Payment, bool> paymentWheres;
            switch (accountingPaymentsEnum)
            {
                case AppUtility.SidebarEnum.MonthlyPayment:
                    wheres.Add(r => (r.PaymentStatusID == 2/*+30*/ && r.Payments.FirstOrDefault().HasInvoice && r.Payments.FirstOrDefault().IsPaid == false)
                    || (
                          (r.PaymentStatusID == 5/*installments*/ || r.PaymentStatusID == 7/*standingorder*/ || r.Product is RecurringOrder)
                          && r.Payments.Where(p => ((p.PaymentDate.Month <= DateTime.Today.Month && p.PaymentDate.Year == DateTime.Today.Year) || p.PaymentDate.Year < DateTime.Today.Year) && p.IsPaid == false).Count() > 0)
                       );
                    paymentWheres = p => p.IsPaid == false && ((p.PaymentDate.Month <= DateTime.Today.Month && p.PaymentDate.Year == DateTime.Today.Year) || p.PaymentDate.Year < DateTime.Today.Year);
                    requestPaymentList = GetPaymentsForEachRequest(wheres, includes, paymentWheres);
                    return requestPaymentList;
                    break;
                case AppUtility.SidebarEnum.PayNow:
                    wheres.Add(r => r.PaymentStatusID == 3 && r.Payments.FirstOrDefault().IsPaid == false);
                    break;
                case AppUtility.SidebarEnum.PayLater:
                    wheres.Add(r => r.PaymentStatusID == 4 && r.Payments.FirstOrDefault().IsPaid == false);
                    break;
                case AppUtility.SidebarEnum.Installments:
                    wheres.Add(r => r.PaymentStatusID == 5);
                    wheres.Add(r => r.Payments.Where(p => p.IsPaid == false && p.HasInvoice && p.PaymentDate < DateTime.Now.AddDays(5)).Count() > 0);
                    paymentWheres = p => p.IsPaid == false && p.PaymentDate < DateTime.Now.AddDays(5);
                    requestPaymentList = GetPaymentsForEachRequest(wheres, includes, paymentWheres);
                    return requestPaymentList;
                    break;
                case AppUtility.SidebarEnum.StandingOrders:
                    wheres.Add(r => r.PaymentStatusID == 7);
                    wheres.Add(r => r.Payments.Where(p => p.IsPaid == false && p.PaymentDate < DateTime.Now.AddDays(5)).Count() > 0);
                    break;
                case AppUtility.SidebarEnum.SpecifyPayment:
                    wheres.Add(r => r.PaymentStatusID == 8 && r.Payments.FirstOrDefault().HasInvoice);
                    break;
            }

            var requestList = _requestsProc.Read(wheres, includes).ToList();

            requestList.ForEach(r => requestPaymentList.Add(new RequestPaymentsViewModel { Request = r, Payment = r.Payments.FirstOrDefault() }));

            return requestPaymentList;
        }

        private List<RequestPaymentsViewModel> GetPaymentsForEachRequest(List<Expression<Func<Request, bool>>> wheres, List<ComplexIncludes<Request, ModelBase>> includes, Func<Payment, bool> paymentWheres)
        {
            var requestPaymentList = new List<RequestPaymentsViewModel>();
            var installmentRequests = _requestsProc.Read(wheres, includes).ToList();
            foreach (var request in installmentRequests)
            {
                var currentInstallments = request.Payments.Where(paymentWheres).ToList();
                requestPaymentList.Add(new RequestPaymentsViewModel { Request = request, Payment = currentInstallments.ElementAt(0) });
                if (currentInstallments.Count() > 1)
                {
                    for (var i = 1; i < currentInstallments.Count(); i++)
                    {
                        requestPaymentList.Add(new RequestPaymentsViewModel { Request = request, Payment = currentInstallments.ElementAt(i) });
                    }
                }
            }
            return requestPaymentList;
        }

        [HttpGet]
        [Authorize(Roles = " Accounting")]
        public async Task<IActionResult> ChangePaymentStatus(RequestIndexObject requestIndexObject, int requestID, AppUtility.PaymentsPopoverEnum newStatus)
        {
            var StringWithBool = await _requestsProc.UpdatePaymentStatusAsync(newStatus, requestID);
            requestIndexObject.ErrorMessage = StringWithBool.String;
            return RedirectToAction("_IndexTableByVendor", requestIndexObject);
        }

        [HttpGet]
        [Authorize(Roles = " Accounting")]
        public async Task<IActionResult> HandleNotifications(AppUtility.SidebarEnum type, int requestID)
        {
            var StringWithBool = await _requestsProc.UpdatePartialClarifyStatusAsync(type, requestID);
            return RedirectToAction("AccountingNotifications", new { accountingNotificationsEnum = type, ErrorMessage = StringWithBool.String });
        }

        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AccountingNotifications(AppUtility.SidebarEnum accountingNotificationsEnum = AppUtility.SidebarEnum.NoInvoice, string ErrorMessage = null)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Accounting;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingNotifications;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = accountingNotificationsEnum;
            var viewmodel = await GetIndexViewModelByVendor(new RequestIndexObject
            {
                SectionType = AppUtility.MenuItems.Accounting,
                PageType = AppUtility.PageTypeEnum.AccountingNotifications,
                SidebarType = accountingNotificationsEnum
            });
            viewmodel.ErrorMessage = ErrorMessage;
            return View(viewmodel);

        }



        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> PaymentsPayModal(int? vendorId, int[] paymentIds, AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var payments = new List<Payment>();
            if (vendorId != null)
            {
                var vm = await GetPaymentRequests(accountingPaymentsEnum);
                payments = vm.Where(rp => rp.Request.Product.VendorID == vendorId).Where(rp => rp.Request.PaymentStatusID != 7/*standingorder*/).Select(rp => rp.Payment).ToList();
            }
            else
            {
                payments = _paymentsProc.Read(new List<Expression<Func<Payment, bool>>> { p => paymentIds.Contains(p.PaymentID) },
                new List<ComplexIncludes<Payment, ModelBase>>
                {
                    new ComplexIncludes<Payment, ModelBase>{ Include = p => p.Request , ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = r => ((Request)r).ParentRequest } },
                    new ComplexIncludes<Payment, ModelBase>{ Include = p => p.Request, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{Include =  r => ((Request)r).Product,
                        ThenInclude =  new ComplexIncludes<ModelBase, ModelBase>{Include =  p => ((Product)p).Vendor  }} }
                }).ToList();
            }
            /*            var requestsToPay = _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r =>  payments.Select(p => p.RequestID).Contains(r.RequestID) },
                            new List<ComplexIncludes<Request, ModelBase>>
                            {
                                //new ComplexIncludes<Request, ModelBase>{ Include =  r => r.ParentRequest},
                                new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = p => ((Product)p).Vendor } },
                                //new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Product.ProductSubcategory},
                                //new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Product.UnitType},
                                //new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Product.SubUnitType},
                                //new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Product.SubSubUnitType},
                                //new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Payments}
                            }).ToList();*/

            //can you edit amount to pay? if yes, if last installment calc how much is left to pay

            StringWithBool Error = new StringWithBool();
            if (payments.Select(p => p.Request.Currency).Distinct().Count() > 1)
            {
                Error.SetStringAndBool(true, ElixirStrings.ServerDifferentCurrencyErrorMessage);
            }
            if (payments.Select(p => p.Request.Product.Vendor).Distinct().Count() > 1)
            {
                Error.SetStringAndBool(true, ElixirStrings.ServerDifferentVendorErrorMessage);
            }
            PaymentsPayModalViewModel paymentsPayModalViewModel = new PaymentsPayModalViewModel()
            {
                Payments = payments,
                //Requests = requestsToPay,
                AccountingEnum = accountingPaymentsEnum,
                Payment = new Payment(),
                PaymentTypes = _paymentTypesProc.Read().Select(pt => pt).ToList(),
                CompanyAccounts = _companyAccountsProc.Read().Select(ca => ca).ToList(),
                ShippingToPay = await GetShippingsToPay(payments),
                Error = Error
            };
            return PartialView(paymentsPayModalViewModel);
        }

        public async Task<List<CheckboxViewModel>> GetShippingsToPay(List<Payment> payments)
        {
            List<CheckboxViewModel> shippings = new List<CheckboxViewModel>();
            foreach (var p in payments)
            {
                if (p.ShippingPaidHere && !p.Request.ParentRequest.IsShippingPaid && p.Request.ParentRequest.Shipping > 0)
                {
                    var r = p.Request;
                    shippings.Add(new CheckboxViewModel()
                    {
                        ID = Convert.ToInt32(r.ParentRequestID),
                        Name = r.Product.ProductName,
                        Value = false,
                        CostDollar = r.Currency == "USD" ? r.ParentRequest.Shipping : r.ParentRequest.Shipping / Convert.ToDouble(r.ExchangeRate),
                        CostShekel = r.Currency == "NIS" ? r.ParentRequest.Shipping : r.ParentRequest.Shipping * Convert.ToDouble(r.ExchangeRate),
                        Currency = r.Currency
                    });
                }
            }
            return shippings;
        }

        [HttpPost]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> PaymentsPayModal(PaymentsPayModalViewModel paymentsPayModalViewModel)
        {
            var stringWithBool = await _paymentsProc.UpdateAsync(paymentsPayModalViewModel);
            return RedirectToAction("AccountingPayments", new { accountingPaymentsEnum = paymentsPayModalViewModel.AccountingEnum, ErrorMessage = stringWithBool.String });

        }
        /*        [HttpGet]
                [Authorize(Roles = "Accounting")]
                public async Task<IActionResult> PaymentsInvoiceModal(int? vendorid, int? paymentid, AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment)
                {
                    if (!AppUtility.IsAjaxRequest(Request))
                    {
                        return PartialView("InvalidLinkPage");
                    }
                    var payment = await _paymentsProc.ReadOneAsync(new List<Expression<Func<Payment, bool>>> { p => p.PaymentID == paymentid });
                    var requestToPay = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == payment.RequestID },
                        new List<ComplexIncludes<Request, ModelBase>>
                        {
                            new ComplexIncludes<Request, ModelBase>{ Include =  r => r.ParentRequest},
                            new ComplexIncludes<Request, ModelBase>{ Include = r => r.Product, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = p => ((Product)p).Vendor } },
                            new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Product.ProductSubcategory},
                            new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Product.UnitType},
                            new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Product.SubUnitType},
                            new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Product.SubSubUnitType},
                            new ComplexIncludes<Request, ModelBase>{ Include =  r => r.Payments}
                        });

                    var paidSum = requestToPay.Payments.Where(p => p.IsPaid).Select(p => p.Sum).Sum();
                    var amtLeftToFullPayment = (decimal)requestToPay.Cost - paidSum;
                    *//*            if (payment.InstallmentNumber == requestToPay.FirstOrDefault().Installments)
                    *//*
                    if (payment.Sum > amtLeftToFullPayment)
                    {
                        payment.Sum = amtLeftToFullPayment;
                    }
                    PaymentsInvoiceViewModel paymentsInvoiceViewModel = new PaymentsInvoiceViewModel()
                    {
                        Request = requestToPay,
                        AccountingEnum = accountingPaymentsEnum,
                        Payment = payment,
                        PaymentTypes = _paymentTypesProc.Read().Select(pt => pt).ToList(),
                        CompanyAccounts = _companyAccountsProc.Read().Select(ca => ca).ToList(),
                        AmtLeftToPay = amtLeftToFullPayment,
                        Invoice = new Invoice()
                        {
                            InvoiceDate = DateTime.Today
                        },
                        ShippingToPay = await GetShippingsToPay(new List<Request> { requestToPay })
                    };


                    return PartialView(paymentsInvoiceViewModel);
                }

                [HttpPost]
                [Authorize(Roles = "Accounting")]
                public async Task<IActionResult> PaymentsInvoiceModal(PaymentsInvoiceViewModel paymentsInvoiceViewModel)
                {
                    using (var transaction = _applicationDbContextTransaction.Transaction)
                    {
                        try
                        {
                            await _paymentsProc.UpdateWithoutTransactionAsync(paymentsInvoiceViewModel);
                            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
                            string requestFolder = Path.Combine(uploadFolder, paymentsInvoiceViewModel.Request.RequestID.ToString());
                            Directory.CreateDirectory(requestFolder);
                            if (paymentsInvoiceViewModel.InvoiceImage != null)
                            {
                                int x = 1;
                                //create file
                                string folderPath = Path.Combine(requestFolder, AppUtility.FolderNamesEnum.Invoices.ToString());
                                if (Directory.Exists(folderPath))
                                {
                                    var filesInDirectory = Directory.GetFiles(folderPath);
                                    x = filesInDirectory.Length + 1;
                                }
                                else
                                {
                                    Directory.CreateDirectory(folderPath);
                                }
                                string uniqueFileName = x + paymentsInvoiceViewModel.InvoiceImage.FileName;
                                string filePath = Path.Combine(folderPath, uniqueFileName);
                                FileStream filestream = new FileStream(filePath, FileMode.Create);
                                paymentsInvoiceViewModel.InvoiceImage.CopyTo(filestream);
                                filestream.Close();
                            }
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            Response.StatusCode = 500;
                            paymentsInvoiceViewModel.Request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == paymentsInvoiceViewModel.Request.RequestID },
                                new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include = r => r.Product }, new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.Vendor } }
                                );
                            paymentsInvoiceViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                            return PartialView(paymentsInvoiceViewModel);
                        }
                    }

                    return RedirectToAction("AccountingPayments", new { accountingPaymentsEnum = paymentsInvoiceViewModel.AccountingEnum });
                }*/

        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AddInvoiceModal(int? vendorid, int? requestid, int[] requestIds)
        {
            List<Request> Requests = new List<Request>();
            StringWithBool Error = new StringWithBool();
            var vendor = new Vendor();
            AppUtility.CurrencyEnum CurrencyUsed = AppUtility.CurrencyEnum.None;
            List<Expression<Func<Request, bool>>> Wheres = new List<Expression<Func<Request, bool>>>();
            List<ComplexIncludes<Request, ModelBase>> Includes = new List<ComplexIncludes<Request, ModelBase>>();
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.ParentRequest });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.Vendor });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.ProductSubcategory });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.UnitType });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.SubSubUnitType });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Product.SubUnitType });
            Includes.Add(new ComplexIncludes<Request, ModelBase> { Include = r => r.Payments });
            Wheres.Add(r => r.Payments.FirstOrDefault().HasInvoice == false && ((r.PaymentStatusID == 2/*+30*/ && r.RequestStatusID == 3) || (r.PaymentStatusID == 3/*pay now*/) || (r.PaymentStatusID == 8/*specify payment*/ && r.RequestStatusID == 3) || (r.PaymentStatusID == 5)));
            Wheres.Add(r => r.RequestStatusID != 7);
            if (vendorid != null)
            {
                Wheres.Add(r => r.Product.VendorID == vendorid);
            }
            else if (requestid != null)
            {
                Wheres.Add(r => r.RequestID == requestid);
            }
            else if (requestIds != null)
            {
                Wheres.Add(r => requestIds.Contains(r.RequestID));

            }
            Requests = await _requestsProc.Read(Wheres, Includes).ToListAsync();
            vendor = Requests.FirstOrDefault().Product.Vendor;
            if (Requests.Select(r => r.Currency).Distinct().Count() > 1)
            {
                Error.SetStringAndBool(true, ElixirStrings.ServerDifferentCurrencyErrorMessage);
            }
            if (Requests.Select(r => r.Product.VendorID).Distinct().Count() > 1)
            {
                Error.SetStringAndBool(true, ElixirStrings.ServerDifferentVendorErrorMessage);
            }
            AddInvoiceViewModel addInvoiceViewModel = new AddInvoiceViewModel()
            {
                Requests = Requests,
                Invoice = new Invoice()
                {
                    InvoiceDate = DateTime.Today
                },
                Guid = Guid.NewGuid(),
                Error = Error,
                Vendor = vendor
            };
            return PartialView(addInvoiceViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AddInvoiceModal(AddInvoiceViewModel addInvoiceViewModel)
        {
            using (var transaction = _applicationDbContextTransaction.Transaction)
            {
                try
                {

                    var createInvoiceSuccess = await _invoicesProc.CreateAsync(addInvoiceViewModel);
                    if (!createInvoiceSuccess.Bool)
                    {
                        addInvoiceViewModel.ErrorMessage = ElixirStrings.ServerExistingInvoiceNumberVendorErrorMessage;
                        Response.StatusCode = 500;
                        return PartialView("_ErrorMessage", addInvoiceViewModel.ErrorMessage);
                    }

                    foreach (var request in addInvoiceViewModel.Requests)
                    {
                        await _requestsProc.UpdateRequestInvoiceInfoAsync(addInvoiceViewModel, request);
                        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString(),
                            addInvoiceViewModel.Guid.ToString(), AppUtility.FolderNamesEnum.Invoices.ToString());
                        if (!Directory.Exists(uploadFolder) || Directory.GetFiles(uploadFolder).Length == 0)
                        {
                            addInvoiceViewModel.ErrorMessage = ElixirStrings.ServerMissingFile;
                            Response.StatusCode = 500;
                            return PartialView("_ErrorMessage", addInvoiceViewModel.ErrorMessage);
                        }

                        MoveDocumentsOutOfTempFolder(request.RequestID, AppUtility.ParentFolderName.Requests, AppUtility.FolderNamesEnum.Invoices, true, addInvoiceViewModel.Guid);
                    }
                    await _requestsProc.SaveDbChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    addInvoiceViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    return PartialView("AddInvoiceModal", addInvoiceViewModel);
                }
            }
            var indexViewModel = await GetIndexViewModelByVendor(new RequestIndexObject { PageType = AppUtility.PageTypeEnum.AccountingNotifications, SectionType = AppUtility.MenuItems.Accounting, SidebarType = AppUtility.SidebarEnum.NoInvoice });
            return PartialView("_IndexTableDataByVendor", indexViewModel);
        }

        public async Task<TempRequestListViewModel> LoadTempListFromRequestIndexObjectAsync(RequestIndexObject requestIndexObject)
        {
            var oldJsonSequenceNumber = _tempRequestJsonsProc.Read(new List<Expression<Func<TempRequestJson, bool>>> { trj => trj.GuidID == requestIndexObject.GUID }).Select(trj => trj.SequencePosition)
            .OrderByDescending(p => p).FirstOrDefault();

            var oldJson = _tempRequestJsonsProc.Read(new List<Expression<Func<TempRequestJson, bool>>>
            { trj => trj.GuidID == requestIndexObject.GUID && trj.SequencePosition==oldJsonSequenceNumber })
                .FirstOrDefault();

            var deSerializedJson = oldJson.DeserializeJson<FullRequestJson>().TempRequestViewModels;
            return new TempRequestListViewModel()
            {
                GUID = requestIndexObject.GUID,
                SequencePosition = oldJson.SequencePosition,
                RequestIndexObject = requestIndexObject,
                TempRequestViewModels = oldJson.DeserializeJson<FullRequestJson>().TempRequestViewModels
            };
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadQuoteModal(string id, List<Request> requests = null)
        {
            //if (!AppUtility.IsAjaxRequest(Request))
            //{
            //    return PartialView("InvalidLinkPage");
            //}
            

            var uploadQuoteViewModel = new UploadQuoteViewModel() { ParentQuote = new ParentQuote() { ExpirationDate = DateTime.Now } };
            uploadQuoteViewModel.DocumentsCardViewModel = new DocumentsCardViewModel()
            {
                DocumentInfo = new DocumentFolder()
                {
                    ParentFolderName = AppUtility.ParentFolderName.ParentQuote,
                    FolderName = AppUtility.FolderNamesEnum.Quotes,
                    Icon = "icon-centarix-icons-03",
                    ObjectID = id
                }
            };

            if (requests != null)
            {
                foreach (var request in requests)
                {
                    var oldQuote = _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.ProductID == request.ProductID && r.ParentQuote.ExpirationDate >= DateTime.Now.Date }).Select(r => r.ParentQuote).OrderByDescending(r => r.QuoteDate).FirstOrDefault();

                    if (oldQuote != null)
                    {
                        string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.ParentQuote.ToString());
                        string uploadFolder2 = Path.Combine(uploadFolder1, oldQuote.ParentQuoteID.ToString());
                        string uploadFolderQuotes = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.Quotes.ToString());

                        if (Directory.Exists(uploadFolderQuotes))
                        {
                            DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderQuotes);
                            //searching for the partial file name in the directory
                            FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                            var fileStrings = new List<String>();
                            foreach (var orderfile in orderfilesfound)
                            {
                                string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                                fileStrings.Add(newFileString);
                            }
                            uploadQuoteViewModel.DocumentsCardViewModel.DocumentInfo.FileStrings = fileStrings;
                            uploadQuoteViewModel.DocumentsCardViewModel.ModalType = AppUtility.RequestModalType.Summary;
                        }
                        uploadQuoteViewModel.DocumentsCardViewModel.DocumentInfo.ObjectID = oldQuote.ParentQuoteID.ToString();

                    }
                    else
                    {
                        oldQuote = new ParentQuote() { ExpirationDate = DateTime.Now };
                        uploadQuoteViewModel.DocumentsCardViewModel.DocumentInfo.ObjectID = id.ToString();
                        uploadQuoteViewModel.DocumentsCardViewModel.ModalType = AppUtility.RequestModalType.Edit;
                    }
                    uploadQuoteViewModel.ParentQuote = oldQuote;

                }
            }

            //create new sequence
            // await _tempRequestJsonsProc.UpdateAsync(uploadQuoteViewModel.TempRequestListViewModel.GUID, requestIndexObject, uploadQuoteViewModel.TempRequestListViewModel, _userManager.GetUserId(User), true);
            var json = JsonConvert.SerializeObject(uploadQuoteViewModel, Formatting.Indented,
                   new JsonSerializerSettings
                   {
                       Converters = new List<JsonConverter> { new StringEnumConverter() },
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
            return Json(json);
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _UploadQuoteModal(Guid guid)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var uploadQuoteViewModel = new UploadQuoteViewModel() { ParentQuote = new ParentQuote() { ExpirationDate = DateTime.Now } };

            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.ParentQuote.ToString());
            string uploadFolder2 = Path.Combine(uploadFolder1, guid.ToString());
            string uploadFolderQuotes = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.Quotes.ToString());

            var fileStrings = new List<String>();
            if (Directory.Exists(uploadFolderQuotes))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderQuotes);
                //searching for the partial file name in the directory
                FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                foreach (var orderfile in orderfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                    fileStrings.Add(newFileString);
                }
            }
            uploadQuoteViewModel.DocumentsCardViewModel = new DocumentsCardViewModel()
            {
                SectionType = AppUtility.MenuItems.Requests,
                DocumentInfo = new DocumentFolder()
                {
                    ParentFolderName = AppUtility.ParentFolderName.ParentQuote,
                    FolderName = AppUtility.FolderNamesEnum.Quotes,
                    Icon = "icon-centarix-icons-03",
                    FileStrings = fileStrings,
                    ObjectID = guid.ToString()
                },
                ModalType = AppUtility.RequestModalType.Edit
            };
            uploadQuoteViewModel.TempRequestListViewModel = new TempRequestListViewModel()
            {
                GUID = guid
            };


            return PartialView(uploadQuoteViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadQuoteModal(UploadQuoteViewModel uploadQuoteOrderViewModel, TempRequestListViewModel tempRequestListViewModel, bool isCancel = false)
        {

            
            var tempRequestJson = await _tempRequestJsonsProc.GetTempRequest(tempRequestListViewModel.GUID, _userManager.GetUserId(User)).FirstOrDefaultAsync();
            try
            {
                var deserializedTempRequestListViewModel = new TempRequestListViewModel()
                {
                    TempRequestViewModels = tempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels,
                    GUID = tempRequestListViewModel.GUID,
                    RequestIndexObject = tempRequestListViewModel.RequestIndexObject,
                    SequencePosition = tempRequestJson.SequencePosition
                };

                foreach (var tempRequestViewModel in deserializedTempRequestListViewModel.TempRequestViewModels)
                {

                    tempRequestViewModel.Request.QuoteStatusID = 4;
                    tempRequestViewModel.Request.ParentQuoteID = uploadQuoteOrderViewModel.ParentQuote.ParentQuoteID;
                    tempRequestViewModel.Request.ParentQuote = uploadQuoteOrderViewModel.ParentQuote;


                    if (uploadQuoteOrderViewModel.ExpectedSupplyDays != null)
                    {
                        tempRequestViewModel.Request.ExpectedSupplyDays = uploadQuoteOrderViewModel.ExpectedSupplyDays;
                    }
                    if (tempRequestViewModel.Request.RequestStatusID == 1)
                    {
                        TempData["RequestStatus"] = 1;
                    }
                }

                if (deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.OrderMethod.DescriptionEnum != AppUtility.OrderMethod.AddToCart.ToString())
                {
                    tempRequestListViewModel.RequestIndexObject.GUID = tempRequestListViewModel.GUID;
                    await _tempRequestJsonsProc.UpdateAsync(tempRequestListViewModel.GUID, tempRequestListViewModel.RequestIndexObject, deserializedTempRequestListViewModel, _userManager.GetUserId(User), false);
                    return RedirectToAction("TermsModal", tempRequestListViewModel.RequestIndexObject);
                }
                else
                {
                    using (var transaction = _applicationDbContextTransaction.Transaction)
                    {
                        try
                        {
                            var ModelStates = new List<ModelAndState>();
                            foreach (var tempRequestViewModel in deserializedTempRequestListViewModel.TempRequestViewModels)
                            {
                                ModelStates.Add(new ModelAndState
                                {
                                    Model = tempRequestViewModel.Request.ParentQuote,
                                    StateEnum = tempRequestViewModel.Request.ParentQuoteID == null || tempRequestViewModel.Request.ParentQuoteID == 0 ? EntityState.Added : EntityState.Modified
                                });

                                ModelStates.Add(new ModelAndState
                                {
                                    Model = tempRequestViewModel.Request.Product,
                                    StateEnum = tempRequestViewModel.Request.ProductID == 0 ? EntityState.Added : EntityState.Modified
                                });
                                if (tempRequestViewModel.Request.ProductID == 0)
                                {
                                    tempRequestViewModel.Request.Product.SerialNumber = await _requestsProc.GetSerialNumberAsync(false);
                                }
                                ModelStates.Add(new ModelAndState { Model = tempRequestViewModel.Request, StateEnum = EntityState.Added });


                                await _requestsProc.UpdateModelsAsync(ModelStates);

                                if (tempRequestViewModel.Comments != null)
                                {
                                    var currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id == _userManager.GetUserId(User) });
                                    await _requestCommentsProc.UpdateWithoutTransactionAsync(AppData.Json.Deserialize<List<RequestComment>>(AppData.Json.Serialize(tempRequestViewModel.Comments.Where(c => c.CommentTypeID == 1))), tempRequestViewModel.Request.RequestID, currentUser.Id);
                                    await _productCommentsProc.UpdateWithoutTransactionAsync(AppData.Json.Deserialize<List<ProductComment>>(AppData.Json.Serialize(tempRequestViewModel.Comments.Where(c => c.CommentTypeID == 2))), tempRequestViewModel.Request.ProductID, currentUser.Id);
                                }
                                //await SaveCommentsFromSession(request);
                                MoveDocumentsOutOfTempFolder(tempRequestViewModel.Request.RequestID, AppUtility.ParentFolderName.Requests, false, tempRequestListViewModel.GUID);
                                MoveDocumentsOutOfTempFolder(tempRequestViewModel.Request.ParentQuoteID == null ? 0 : Convert.ToInt32(tempRequestViewModel.Request.ParentQuoteID), AppUtility.ParentFolderName.ParentQuote, false, tempRequestListViewModel.GUID);

                                try
                                {
                                    //throw new Exception();
                                    await transaction.CommitAsync();
                                }
                                catch (Exception ex)
                                {
                                    RevertDocuments(tempRequestViewModel.Request.RequestID, AppUtility.ParentFolderName.Requests, tempRequestListViewModel.GUID);
                                    RevertDocuments(tempRequestViewModel.Request.ParentQuoteID == null ? 0 : Convert.ToInt32(tempRequestViewModel.Request.ParentQuoteID), AppUtility.ParentFolderName.ParentQuote, tempRequestListViewModel.GUID);
                                    //Directory.Move(requestFolderTo, requestFolderFrom);
                                    transaction.Rollback();
                                    throw ex;
                                }
                            }
                            await _tempRequestJsonsProc.RemoveAllAsync(deserializedTempRequestListViewModel.GUID, _userManager.GetUserId(User));
                            return new EmptyResult();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _tempRequestJsonsProc.RollbackAsync(tempRequestListViewModel.GUID, tempRequestJson.SequencePosition);
                uploadQuoteOrderViewModel.TempRequestListViewModel = tempRequestListViewModel;
                uploadQuoteOrderViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                return PartialView("_ErrorMessage", uploadQuoteOrderViewModel.ErrorMessage);
            }
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadOrderModal(RequestIndexObject requestIndexObject)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var userID = _userManager.GetUserId(User);
            var uploadOrderViewModel = new UploadOrderViewModel();

            uploadOrderViewModel.TempRequestListViewModel = await LoadTempListFromRequestIndexObjectAsync(requestIndexObject);

            long lastParentRequestOrderNum = 0;
            if (_parentRequestsProc.Read().Any())
            {
                lastParentRequestOrderNum = _parentRequestsProc.ReadWithIgnoreQueryFilters().OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber ?? 0;
            }
            ParentRequest pr = new ParentRequest()
            {
                ApplicationUserID = userID,
                OrderNumber = lastParentRequestOrderNum + 1,
                OrderDate = DateTime.Now
            };
            uploadOrderViewModel.TempRequestListViewModel.TempRequestViewModels.ForEach(t => t.Request.ParentRequest = pr);
            uploadOrderViewModel.ParentRequest = pr;

            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.ParentRequest.ToString());
            string uploadFolder2 = Path.Combine(uploadFolder1, requestIndexObject.GUID.ToString());
            string uploadFolderOrders = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.Orders.ToString());

            if (Directory.Exists(uploadFolderOrders))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderOrders);
                //searching for the partial file name in the directory
                FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                uploadOrderViewModel.FileStrings = new List<String>();
                foreach (var orderfile in orderfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                    uploadOrderViewModel.FileStrings.Add(newFileString);
                }
            }
            await _tempRequestJsonsProc.UpdateAsync(requestIndexObject.GUID, requestIndexObject, uploadOrderViewModel.TempRequestListViewModel, userID, true);

            return PartialView(uploadOrderViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadOrderModal(UploadOrderViewModel uploadOrderViewModel, TempRequestListViewModel tempRequestListViewModel, bool isCancel = false)
        {
            try
            {
                var userID = _userManager.GetUserId(User);
                var oldTempRequestJson = await _tempRequestJsonsProc.GetTempRequest(tempRequestListViewModel.GUID, userID).FirstOrDefaultAsync();
                var deserializedTempRequestListViewModel = new TempRequestListViewModel()
                {
                    TempRequestViewModels = oldTempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels
                };
                if (uploadOrderViewModel.ParentRequest.OrderDate == DateTime.Today) //if it's today, add seconds to be now so it shows up on top
                {
                    uploadOrderViewModel.ParentRequest.OrderDate = DateTime.Now;
                }
                uploadOrderViewModel.ParentRequest.ApplicationUserID = _userManager.GetUserId(User);
                foreach (var tempRequest in deserializedTempRequestListViewModel.TempRequestViewModels)
                {
                    if (uploadOrderViewModel.ExpectedSupplyDays != null)
                    {
                        tempRequest.Request.ExpectedSupplyDays = uploadOrderViewModel.ExpectedSupplyDays;
                    }
                    tempRequest.Request.ParentRequest = uploadOrderViewModel.ParentRequest;
                }

                await _tempRequestJsonsProc.UpdateAsync(tempRequestListViewModel.GUID, tempRequestListViewModel.RequestIndexObject, deserializedTempRequestListViewModel, userID, false);
                string action;
                tempRequestListViewModel.RequestIndexObject.GUID = tempRequestListViewModel.GUID;
                action = "TermsModal";
                Response.StatusCode = 200;
                return RedirectToAction(action, tempRequestListViewModel.RequestIndexObject);
            }
            catch (Exception ex)
            {
                uploadOrderViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                return PartialView("_ErrorMessage", uploadOrderViewModel.ErrorMessage);
            }
        }


        [HttpGet]
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> OrderOperationsModalJson(RequestIndexObject requestIndexObject)
        {
            var userID = _userManager.GetUserId(User);
            var orderOperationsViewModel = new OrderOperationsViewModel();

            orderOperationsViewModel.TempRequestListViewModel = await LoadTempListFromRequestIndexObjectAsync(requestIndexObject);

            long lastParentRequestOrderNum = 0;
            if (_parentRequestsProc.Read().Any())
            {
                lastParentRequestOrderNum = _parentRequestsProc.ReadWithIgnoreQueryFilters().OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber ?? 0;
            }
            ParentRequest pr = new ParentRequest()
            {
                ApplicationUserID = userID,
                OrderNumber = lastParentRequestOrderNum + 1,
                OrderDate = DateTime.Now
            };
            orderOperationsViewModel.TempRequestListViewModel.TempRequestViewModels.ForEach(t => t.Request.ParentRequest = pr);
            orderOperationsViewModel.ParentRequest = pr;

            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.ParentRequest.ToString());
            string uploadFolder2 = Path.Combine(uploadFolder1, requestIndexObject.GUID.ToString());
            string uploadFolderOrders = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.Orders.ToString());

            await _tempRequestJsonsProc.UpdateAsync(requestIndexObject.GUID, requestIndexObject, orderOperationsViewModel.TempRequestListViewModel, userID, true);

            return Json(JsonConvert.SerializeObject(orderOperationsViewModel, Formatting.Indented, new JsonSerializerSettings
            {

                Converters = new List<JsonConverter> { new StringEnumConverter() },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }



        [HttpGet]
        [Authorize(Roles = "Requests, LabManagement")]
        public async Task<IActionResult> TermsModal(int vendorID, List<int> requestIds, RequestIndexObject requestIndexObject) //either it'll be a request or parentrequest and then it'll send it to all the requests in that parent request
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            TempRequestListViewModel tempRequestListViewModel = new TempRequestListViewModel()
            {
                TempRequestViewModels = new List<TempRequestViewModel>()
            };
            if (vendorID == 0 && (requestIds == null || requestIds.Count == 0))
            {
                tempRequestListViewModel = await LoadTempListFromRequestIndexObjectAsync(requestIndexObject);
            }
            else
            {
                tempRequestListViewModel.RequestIndexObject = requestIndexObject;
                if (tempRequestListViewModel.GUID == Guid.Empty) { tempRequestListViewModel.GUID = Guid.NewGuid(); }

            }
            //IF ANYTHING IS CHANGED TO THE JSON (RequestIndexObject or TempRequestViewModels) MUST BE UPDATED HERE
            return PartialView(await GetTermsViewModelAsync(vendorID, requestIds, tempRequestListViewModel));
        }

        public string JavascriptError()
        {
            return "Error";
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> TermsModal(TermsViewModel termsViewModel, TempRequestListViewModel tempRequestListViewModel, bool isCancel = false)
        {

            if (isCancel)
            {
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests, tempRequestListViewModel.GUID);
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote, tempRequestListViewModel.GUID);
                await _tempRequestJsonsProc.RemoveAllAsync(tempRequestListViewModel.GUID, _userManager.GetUserId(User));
                return new EmptyResult();
            }

            var r = await SaveTermsModalAsync(termsViewModel, tempRequestListViewModel);
            if (r.RedirectToActionResult.ActionName == "" && r.RedirectToActionResult.ControllerName == "")
            {
                return PartialView("_ErrorMessage", r.TermsViewModel.ErrorMessage);
            }
            else if (r.RedirectToActionResult.ActionName == "NeedsToBeApproved")
            {
                //return PartialView("_IndexTableWithCounts", new { requestIndexObject = tempRequestListViewModel.RequestIndexObject });
                return new EmptyResult();
            }
            else if (r.RedirectToActionResult.ActionName == "Index")
            {
                return new EmptyResult();
            }
            return RedirectToAction(r.RedirectToActionResult.ActionName, r.RedirectToActionResult.ControllerName, r.RedirectToActionResult.RouteValues);
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
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public IActionResult _InventoryFilterResults(SelectedRequestFilters selectedFilters, int numFilters, AppUtility.MenuItems sectionType, bool isProprietary)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            try
            {
                InventoryFilterViewModel inventoryFilterViewModel = base.GetInventoryFilterViewModel(selectedFilters, numFilters, sectionType, isProprietary);
                //throw new Exception();
                return PartialView(inventoryFilterViewModel);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return PartialView("_ErrorMessage", AppUtility.GetExceptionMessage(ex));
            }
        }
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _CartTotalModal(int requestID, AppUtility.MenuItems sectionType = AppUtility.MenuItems.Requests)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var request = await _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestID }, new List<ComplexIncludes<Request, ModelBase>>
                { new ComplexIncludes<Request, ModelBase> { Include = r => r.ApplicationUserCreator },
                new ComplexIncludes<Request, ModelBase>{ Include = r=>r.Product.Vendor } }).FirstOrDefaultAsync();
            var vendor = request.Product.Vendor;
            var vendorCartTotal = _requestsProc.Read(new List<Expression<Func<Request, bool>>>{ r => r.Product.VendorID == vendor.VendorID && r.ApplicationUserCreatorID == request.ApplicationUserCreatorID &&
            r.OrderMethod.DescriptionEnum == AppUtility.OrderMethod.AddToCart.ToString() && r.RequestStatusID != 1 })
                .Select(r => r.Cost).Sum();
            vendorCartTotal = Math.Round(vendorCartTotal ?? 0, 2);
            CartTotalViewModel viewModel = new CartTotalViewModel()
            {
                Request = request,
                Vendor = vendor,
                SectionType = sectionType,
                VendorCartTotal = vendorCartTotal ?? 0
            };
            return PartialView(viewModel);
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> HistoryItemModal(int? id, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var requestItemViewModel = await editModalViewFunction(id, 0, SectionType, false);
            requestItemViewModel.IsHistory = true;
            return PartialView(requestItemViewModel);
        }
        [Authorize(Roles = "Requests")]
        [HttpPost]
        public async Task<IActionResult> _HistoryTab(int? id, List<string> selectedPriceSort, string selectedCurrency, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            try
            {
                var requestItemViewModel = await editModalViewFunction(id, 0, SectionType, false, selectedPriceSort, selectedCurrency);
                //throw new Exception();
                return PartialView(requestItemViewModel);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return PartialView("_ErrorMessage", AppUtility.GetExceptionMessage(ex));
            }

        }
        [Authorize(Roles = "Requests")]
        [HttpGet]
        public async Task<IActionResult> _LocationTab(int id)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var requestItemViewModel = await editModalViewFunction(id, isEditable: false);
            return PartialView(requestItemViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public IActionResult ConfirmArchiveModal(string locationName)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            ConfirmArchiveViewModel confirmArchiveViewModel = new ConfirmArchiveViewModel();
            confirmArchiveViewModel.LocationName = locationName;
            return PartialView(confirmArchiveViewModel);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ArchiveRequest(int requestId, ReceivedModalVisualViewModel receivedModalVisualViewModel)
        {
            var success = await _requestLocationInstancesProc.ArchiveAsync(requestId, receivedModalVisualViewModel);
            if (!success.Bool)
            {
                var requestItemViewModel = await editModalViewFunction(requestId, isEditable: false);
                requestItemViewModel.ErrorMessage = success.String;
                Response.StatusCode = 500;
                return PartialView("_LocationTab", requestItemViewModel);
            }
            return RedirectToAction("_LocationTab", new { id = requestId });
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<JsonResult> MoveToListModalJson(int requestID, int prevListID = 0)
        {

            var pageType = AppUtility.PageTypeEnum.RequestCart;
            var userLists = _requestListsProc.Read(new List<Expression<Func<RequestList, bool>>> { rl => rl.ApplicationUserOwnerID == _userManager.GetUserId(User) })
               .OrderBy(rl => rl.DateCreated).ToList();
            var sharedLists = _shareRequestListsProc.Read(new List<Expression<Func<ShareRequestList, bool>>> { srl => srl.ToApplicationUserID == _userManager.GetUserId(User) && !srl.ViewOnly },
                new List<ComplexIncludes<ShareRequestList, ModelBase>> { new ComplexIncludes<ShareRequestList, ModelBase> { Include = srl => srl.RequestList } }).OrderBy(srl => srl.TimeStamp).Select(srl => srl.RequestList).ToList();
            sharedLists.ForEach(sl => userLists.Add(sl));
            if (userLists.Count == 0)
            {
                var requestList = await _requestListsProc.CreateAndGetDefaultListAsync(_userManager.GetUserId(User));
                userLists.Add(requestList);
            }
            else
            {
                userLists = userLists.Where(ul => ul.ListID != prevListID).ToList();
            }

            if (prevListID == 0)
            {
                pageType = AppUtility.PageTypeEnum.RequestInventory;
            }
            MoveListViewModel viewModel = new MoveListViewModel()
            {
                Request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestID }, new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include = r => r.Product } }),
                PreviousListID = prevListID,
                RequestLists = userLists,
                PageType = pageType
            };
            var json = JsonConvert.SerializeObject(viewModel, Formatting.Indented,
              new JsonSerializerSettings
              {
                  Converters = new List<JsonConverter> { new StringEnumConverter() },
                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore
              });
            return Json(json);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]

        public async Task<IActionResult> MoveToListModal(MoveListViewModel moveListViewModel)
        {
            var success = await _requestListRequestsProc.MoveList(moveListViewModel.Request.RequestID, moveListViewModel.NewListID, moveListViewModel.PreviousListID);
            if (!success.Bool)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(success.String);
            }
            return new EmptyResult();
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> NewListModalJson(int requestToAddId = 0, int requestPreviousListID = 0)
        {
            NewListViewModel viewModel = new NewListViewModel()
            {
                OwnerID = _userManager.GetUserId(User),
                RequestToAddID = requestToAddId,
                RequestPreviousListID = requestPreviousListID
            };
            var json = JsonConvert.SerializeObject(viewModel, Formatting.Indented,
             new JsonSerializerSettings
             {
                 Converters = new List<JsonConverter> { new StringEnumConverter() },
                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
             });
            return Json(json);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<JsonResult> NewListModal(NewListViewModel newListViewModel, RequestIndexObject requestIndexObject)
        {

            var newList = await _requestListsProc.CreateAndGetAsync(newListViewModel);
            requestIndexObject.ListID = newList.ListID;
            if (requestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart)
            {
                return await GetIndexTableJson(requestIndexObject);
            }
            else
            {
                return Json( "");
            }

        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteListRequestModal(int requestID, int listID)
        {
            DeleteListRequestViewModel viewModel = new DeleteListRequestViewModel()
            {
                Request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestID }, new List<ComplexIncludes<Request, ModelBase>> { new ComplexIncludes<Request, ModelBase> { Include = r => r.Product } }),
                List = await _requestListsProc.ReadOneAsync(new List<Expression<Func<RequestList, bool>>> { l => l.ListID == listID })
            };
            return PartialView(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteListRequestModal(DeleteListRequestViewModel viewModel)
        {
            var success = await _requestListRequestsProc.DeleteByListIDAndRequestIDsAsync(viewModel.List.ListID, viewModel.Request.RequestID);
            return RedirectToAction("_IndexTableWithListTabs", new
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = AppUtility.SidebarEnum.MyLists,
                ListID = viewModel.List.ListID,
                ErrorMessage = success.String
            });
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ListSettingsModal(AppUtility.SidebarEnum SidebarType, int selectedListID = 0, string errorMessage = null)
        {
            var listSettings = await GetListSettingsInfoAsync(selectedListID, SidebarType);
            listSettings.ErrorMessage = errorMessage;
            return PartialView(listSettings);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ListSettingsModal(ListSettingsViewModel listSettings, int selectedIndexListID, string errorMessage)
        {
            var error = await _requestListsProc.UpdateAsync(listSettings, _userManager.GetUserId(User));
            return RedirectToAction("_IndexTableWithListTabs", new
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = listSettings.SidebarType,
                ListID = selectedIndexListID,
                errorMessage = errorMessage + error.String
            });
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _ListSettings(int listID, AppUtility.SidebarEnum SidebarType)
        {
            var listSettings = await GetListSettingsInfoAsync(listID, SidebarType);

            return PartialView(listSettings);
        }



        [Authorize(Roles = "Requests")]
        private async Task<ListSettingsViewModel> GetListSettingsInfoAsync(int selectedListID, AppUtility.SidebarEnum sidebarType)
        {

            var viewModel = new ListSettingsViewModel();
            if (sidebarType == AppUtility.SidebarEnum.SharedLists)
            {
                viewModel = await GetSharedListSettingsAsync(selectedListID);
            }
            else
            {
                viewModel = await GetMyListSettingsAsync(selectedListID);
            }

            if (viewModel.SelectedList == null && viewModel.SelectedSharedList == null)
            {
                return new ListSettingsViewModel();
            }


            viewModel.SharedUsers = _shareRequestListsProc.Read(new List<Expression<Func<ShareRequestList, bool>>> { l => l.ObjectID == selectedListID }, new List<ComplexIncludes<ShareRequestList, ModelBase>> { new ComplexIncludes<ShareRequestList, ModelBase> { Include = l => l.ToApplicationUser } }).Select(
                l => new ShareRequestListViewModel
                {
                    ShareRequestList = l
                }).ToList();

            viewModel.ApplicationUsers = GetListUsersDropdown(viewModel);

            return viewModel;
        }

        [Authorize(Roles = "Requests")]
        private async Task<ListSettingsViewModel> GetSharedListSettingsAsync(int selectedListID)
        {
            var userLists = new List<RequestList>();
            var defaultList = await _shareRequestListsProc.ReadOneAsync(new List<Expression<Func<ShareRequestList, bool>>> { srl => srl.ObjectID == selectedListID && srl.ToApplicationUserID == _userManager.GetUserId(User) },
                new List<ComplexIncludes<ShareRequestList, ModelBase>> { new ComplexIncludes<ShareRequestList, ModelBase> { Include = srl => srl.RequestList }, new ComplexIncludes<ShareRequestList, ModelBase> { Include = srl => srl.FromApplicationUser } });
            userLists = _shareRequestListsProc.Read(new List<Expression<Func<ShareRequestList, bool>>> { srl => srl.ToApplicationUserID == _userManager.GetUserId(User) }, new List<ComplexIncludes<ShareRequestList, ModelBase>> { new ComplexIncludes<ShareRequestList, ModelBase> { Include = srl => srl.RequestList } }).OrderBy(srl => srl.TimeStamp).Select(srl => srl.RequestList).ToList();
            ListSettingsViewModel viewModel = new ListSettingsViewModel()
            {
                RequestLists = userLists,
                SelectedSharedList = defaultList,
                SidebarType = AppUtility.SidebarEnum.SharedLists
            };
            return viewModel;
        }

        [Authorize(Roles = "Requests")]
        private async Task<ListSettingsViewModel> GetMyListSettingsAsync(int selectedListID)
        {
            var userLists = new List<RequestList>();
            var defaultList = await _requestListsProc.ReadOneAsync(new List<Expression<Func<RequestList, bool>>> { rl => rl.ListID == selectedListID });
            userLists = _requestListsProc.Read(new List<Expression<Func<RequestList, bool>>> { rl => rl.ApplicationUserOwnerID == _userManager.GetUserId(User) }).OrderBy(rl => rl.DateCreated).ToList();
            if (selectedListID == 0)
            {
                defaultList = userLists.Where(l => l.IsDefault).FirstOrDefault();
            }
            ListSettingsViewModel viewModel = new ListSettingsViewModel()
            {
                RequestLists = userLists,
                SelectedList = defaultList,
                SidebarType = AppUtility.SidebarEnum.MyLists
            };
            return viewModel;
        }

        [Authorize(Roles = "Requests")]
        private List<SelectListItem> GetListUsersDropdown(ListSettingsViewModel listSettings)
        {
            var selectedList = listSettings.SidebarType == AppUtility.SidebarEnum.MyLists ? listSettings.SelectedList : listSettings.SelectedSharedList.RequestList;
            listSettings.ApplicationUsers = _employeesProc.Read(new List<Expression<Func<Employee, bool>>> { u => u.Id != _userManager.GetUserId(User)
                              && (!listSettings.SharedUsers.Select(su => su.ShareRequestList.ToApplicationUserID).Contains(u.Id))
                              && u.Id != selectedList.ApplicationUserOwnerID})
                              .Select(
                                  u => new SelectListItem
                                  {
                                      Text = u.FirstName + " " + u.LastName,
                                      Value = u.Id
                                  }
                              ).ToList();
            listSettings.ApplicationUsers.Insert(0, new SelectListItem() { Selected = true, Disabled = true, Text = "Select User" });
            return listSettings.ApplicationUsers;
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _ListUsers(ListSettingsViewModel listSettings)
        {
            var sharedUsersToRemove = new List<ShareRequestListViewModel>();
            if (listSettings.SharedUsers != null)
            {
                foreach (var user in listSettings.SharedUsers)
                {
                    if (user.IsRemoved && user.ShareRequestList.ShareID == 0)
                    {
                        sharedUsersToRemove.Add(user);
                    }
                }
            }
            else
            {
                listSettings.SharedUsers = new List<ShareRequestListViewModel>();
            }
            sharedUsersToRemove.ForEach(u => listSettings.SharedUsers.Remove(u));
            if (listSettings.ApplicationUserIDs != null)
            {
                foreach (var id in listSettings.ApplicationUserIDs)
                {
                    if (listSettings.SharedUsers != null && listSettings.SharedUsers.Select(su => su.ShareRequestList.ToApplicationUserID).Contains(id))
                    {
                        listSettings.SharedUsers.Where(su => su.ShareRequestList.ToApplicationUserID == id).FirstOrDefault().IsRemoved = false;
                    }
                    else
                    {
                        listSettings.SharedUsers.Add(
                            new ShareRequestListViewModel()
                            {
                                ShareRequestList = new ShareRequestList()
                                {
                                    ToApplicationUserID = id,
                                    ToApplicationUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id == id }),
                                    ViewOnly = true
                                }
                            }
                        );
                    }
                }
            }
            listSettings.ApplicationUsers = GetListUsersDropdown(listSettings);

            listSettings.SharedUsers.Where(su => su.IsRemoved).ToList().ForEach(su =>
            {
                listSettings.ApplicationUsers.Add(new SelectListItem
                {
                    Text = su.ShareRequestList.ToApplicationUser.FirstName + " " + su.ShareRequestList.ToApplicationUser.LastName,
                    Value = su.ShareRequestList.ToApplicationUserID
                }
                );
            });

            return PartialView(listSettings);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteListModal(int listID)
        {
            var viewModel = await _requestListsProc.ReadOneAsync(new List<Expression<Func<RequestList, bool>>> { rl => rl.ListID == listID });
            return PartialView(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteListModal(RequestList deleteList)
        {
            var error = await _requestListsProc.DeleteAsync(deleteList);
            return RedirectToAction("ListSettingsModal", new { ErrorMessage = "From delete list: " + error.String });
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> SaveListModal(int listID)
        {
            return PartialView(listID);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<string> SaveListModal(ListSettingsViewModel listSettings)
        {
            var error = await _requestListsProc.UpdateAsync(listSettings, _userManager.GetUserId(User));
            return error.String;
        }



        public async Task<String> GetUrlFromUserData(String inputtedUrl)
        {
            return AppUtility.GetUrlFromUserData(inputtedUrl);
        }



        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _CommentInfoPartialView(int typeID, int index)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return await base._CommentInfoPartialView(typeID, index);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ProductsWarningModal(int productID)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            // requestItemViewModel.HasQuote =_proc.Requests.Where(r => r.ProductID == request.ProductID && r.ParentQuote.ExpirationDate >= DateTime.Now.Date).Select(r => r.ParentQuote).OrderByDescending(r => r.QuoteDate).Count()>0;

            var viewModel = await _productCommentsProc.Read(new List<Expression<Func<ProductComment, bool>>> { p => p.ObjectID == productID && p.CommentTypeID == 2 },
                new List<ComplexIncludes<ProductComment, ModelBase>> { new ComplexIncludes<ProductComment, ModelBase> { Include = p => p.ApplicationUser }, new ComplexIncludes<ProductComment, ModelBase> { Include = p => p.CommentType } }).ToListAsync();
            return PartialView(viewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UnitWarningModal(AppUtility.MenuItems SectionType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return PartialView(SectionType);
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> VendorFloatDetails(int vendorID, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            CreateSupplierViewModel viewModel = await GetCreateSupplierViewModel(SectionType, vendorID);
            viewModel.IsReadOnly = true;
            viewModel.ModalType = AppUtility.VendorModalType.SummaryFloat;

            return PartialView(viewModel);
        }


        public IActionResult DownloadRequestsToExcel()
        {
            var subcategoryList = new List<int>() { 1502 };
            var results1 = _requestsProc.ReadWithIgnoreQueryFilters(
                new List<Expression<Func<Request, bool>>> { r => subcategoryList.Contains(r.Product.ProductSubcategoryID) },
                new List<ComplexIncludes<Request, ModelBase>>{ new ComplexIncludes<Request, ModelBase>() { Include = r => r.RequestLocationInstances, ThenInclude =
                new ComplexIncludes<ModelBase, ModelBase>(){ Include = rli => ((RequestLocationInstance)rli).LocationInstance } }});
            var results = results1.Select(r => new
            {
                ProductName = r.Product.ProductName,
                InvoiceNumber = r.Payments.FirstOrDefault().Invoice.InvoiceNumber,
                CategoryName = r.Product.ProductSubcategory.ParentCategory.Description,
                SubCategoryName = r.Product.ProductSubcategory.Description,
                Vendor = r.Product.Vendor.VendorEnName,
                CompanyID = r.Product.Vendor.VendorBuisnessID,
                CatalogNumber = r.Product.CatalogNumber,
                BatchLot = r.Batch,
                ExpirationDate = AppUtility.GetExcelDateFormat(r.BatchExpiration),
                QuoteNumber = r.ParentQuote.QuoteNumber,
                QuoteExpirationDate = AppUtility.GetExcelDateFormat(r.ParentQuote.ExpirationDate),
                ExpectedSupplyDays = r.ExpectedSupplyDays,
                ExpectedSupplyDate = r.ParentRequest.OrderDate.AddDays(Convert.ToDouble(r.ExpectedSupplyDays)),
                URL = AppUtility.GetUrlFromUserData(r.URL),
                CentarixOrderNumber = r.ParentRequest.OrderNumber,
                OrderDate = AppUtility.GetExcelDateFormat(r.ParentRequest.OrderDate),
                ArrivalDate = AppUtility.GetExcelDateFormat(r.ArrivalDate),
                RequestedBy = r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName,
                OrderedBy = r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName,
                ReceivedBy = r.ApplicationUserReceiver.FirstName + " " + r.ApplicationUserReceiver.LastName,
                Currency = r.Currency,
                ExchangeRate = r.ExchangeRate,
                Amount = r.Unit,
                Unit = r.Product.UnitType.UnitTypeDescription,
                SubUnitAmount = r.Product.SubUnit,
                SubUnit = r.Product.SubUnitType.UnitTypeDescription,
                SubSubUnitAmount = r.Product.SubSubUnit,
                SubSubUnit = r.Product.SubSubUnitType.UnitTypeDescription,
                Total = r.Cost,
                IncludeVat = r.IncludeVAT,
                Discount = r.ParentQuote.Discount,
                Terms = r.PaymentStatus.PaymentStatusDescription,
                IsPaid = r.Payments.FirstOrDefault().IsPaid,
                Partial = r.IsPartial,
                Clarify = r.IsClarify,
                Payments = r.Payments.Count()
            }).ToList();

            var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
                {
                    using (var cw = new CsvWriter(sw, cc))
                    {
                        cw.WriteRecords(results);
                    }// The stream gets flushed here.
                    return File(ms.ToArray(), "text/csv", $"ElixirRequestsDownload_{DateTime.UtcNow.Ticks}.csv");
                }
            }
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public string GetCategoryImageSrc(int productSubCategoryID)
        {
            return _productSubcategoriesProc.ReadOneAsync(new List<Expression<Func<ProductSubcategory, bool>>> { ps => ps.ID == productSubCategoryID }).Result.ImageURL;
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> SettingsInventory()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSettings;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Inventory;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;

            SettingsInventory settings = new SettingsInventory()
            {
                TopTabsList = new List<TopTabWithCounts>()
                {
                    new TopTabWithCounts()
                    {
                        Name = "Main",
                        Page = "Main",
                        Counts = new BoolIntViewModel()
                        {
                            Bool = false
                        }
                    },
                    new TopTabWithCounts()
                    {
                        Name = "Samples",
                        Page = "Samples",
                        Counts = new BoolIntViewModel()
                        {
                            Bool = false
                        }
                    }
                },
                Categories = GetCategoryList(new ParentCategory().GetType().Name, 1),
            };
            settings.Subcategories = GetCategoryList(new ProductSubcategory().GetType().Name, 2, settings.Categories.CategoryBases.FirstOrDefault().ID);
            settings.SettingsForm = GetSettingsFormViewModel(settings.Subcategories.CategoryBases.FirstOrDefault().GetType().Name, settings.Subcategories.CategoryBases.FirstOrDefault().ID);
            return View(settings);
        }

        [HttpGet]
        public IActionResult _CategoryList(String modelType, int ColumnNumber, int? ParentCategoryID)
        {
            var categoryBases = GetCategoryList(modelType, ColumnNumber, ParentCategoryID);
            return PartialView(categoryBases);
        }

        public CategoryListViewModel GetCategoryList(String modelType, int ColumnNumber, int? ParentCategoryID = null)
        {
            IEnumerable<CategoryBase> categoryBases = new List<CategoryBase>();
            switch (modelType)
            {
                case "ProductSubcategory":
                    var wheres = new List<Expression<Func<ProductSubcategory, bool>>>();
                    if (ParentCategoryID != null)
                    {
                        wheres.Add(ps => ps.ParentCategoryID == ParentCategoryID);
                    }
                    categoryBases = _productSubcategoriesProc.Read(wheres);
                    break;
                case "ParentCategory":
                    var wheres2 = new List<Expression<Func<ParentCategory, bool>>>();
                    categoryBases = _parentCategoriesProc.Read(wheres2);
                    break;
            }
            CategoryListViewModel clvm = new CategoryListViewModel()
            {
                CategoryBases = categoryBases.OrderBy(pc => pc.Description).ToList(),
                ColumnNumber = ColumnNumber
            };
            return clvm;
        }

        [HttpGet]
        public IActionResult _SettingsForm(string ModelType, int CategoryID)
        {
            return PartialView(GetSettingsFormViewModel(ModelType, CategoryID));
        }

        private SettingsForm GetSettingsFormViewModel(string ModelType, int CategoryID)
        {
            SettingsForm settingsForm = new SettingsForm();
            if (ModelType == new ParentCategory().GetType().Name)
            {
                settingsForm.Category = _parentCategoriesProc.Read(new List<Expression<Func<ParentCategory, bool>>> { cb => cb.ID == CategoryID }).FirstOrDefault();
            }
            else if (ModelType == new ProductSubcategory().GetType().Name)
            {
                settingsForm.Category = _productSubcategoriesProc.Read(new List<Expression<Func<ProductSubcategory, bool>>> { ps => ps.ID == CategoryID }).FirstOrDefault();
            }
            settingsForm.RequestCount = _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.Product.ProductSubcategoryID == settingsForm.Category.ID }).Count();
            settingsForm.ItemCount = _productsProc.Read(new List<Expression<Func<Product, bool>>> { p => p.ProductSubcategoryID == settingsForm.Category.ID }).Count();
            settingsForm.CustomFieldData = this._CustomField();

            return settingsForm;
        }

        [HttpPost]
        public string UploadFile(DocumentsModalViewModel documentsModalViewModel)
        {
            return base.UploadFile(documentsModalViewModel);
        }


        public async Task<bool> UpdateExchangeRate()
        {
            return _requestsProc.UpdateExchangeRateByHistory().Result.Bool;
        }

        public CustomField _CustomField()
        {
            var CustomField = new CustomField()
            {
                CustomDataTypes = _customDataTypesProc.Read()
            };
            return CustomField;
        }
    }
}