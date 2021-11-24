﻿using System;
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
                    requestIndexObject.SubcategorySelected
                }
            };
            List<IconColumnViewModel> iconList = new List<IconColumnViewModel>();
            var editQuoteDetailsIcon = new IconColumnViewModel(" icon-monetization_on-24px ", "var(--lab-man-color);", "load-quote-details", "Upload Quote");
            var payNowIcon = new IconColumnViewModel(" icon-monetization_on-24px green-overlay ", "", "pay-one", "Pay");
            var addInvoiceIcon = new IconColumnViewModel(" icon-cancel_presentation-24px  green-overlay ", "", "invoice-add-one", "Add Invoice");

            var deleteIcon = new IconColumnViewModel(" icon-delete-24px ", "black", "load-confirm-delete", "Delete");
            var favoriteIcon = new IconColumnViewModel(" icon-favorite_border-24px", "var(--order-inv-color);", "request-favorite", "Favorite");
            var popoverMoreIcon = new IconColumnViewModel("More", "icon-more_vert-24px", "black", "More");
            var popoverPartialClarifyIcon = new IconColumnViewModel("PartialClarify");
            var resendIcon = new IconColumnViewModel("Resend");
            string checkboxString = "Checkbox";
            string buttonText = "";
            var defaultImage = "/images/css/CategoryImages/placeholder.png";
            switch (requestIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.LabManagementQuotes:
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.Orders:
                            var ordersRequests = _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1).Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.QuoteStatusID == 4 && r.RequestStatusID == 6)
                                     .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                                     .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType).Include(r => r.ApplicationUserCreator);

                            iconList.Add(deleteIcon);
                            viewModelByVendor.RequestsByVendor = ordersRequests.OrderByDescending(r => r.CreationDate).Select(r => new RequestIndexPartialRowViewModel
                             (AppUtility.IndexTableTypes.LabOrders, r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory,
                        r.Product.ProductSubcategory.ParentCategory, r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, checkboxString)

                            {
                                ButtonClasses = " load-terms-modal lab-man-background-color ",
                                ButtonText = "Order"
                            }).ToLookup(c => c.Vendor);
                            break;
                        case AppUtility.SidebarEnum.Quotes:
                            var quoteRequests = _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                                .Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString())
                                .Where(r => (r.QuoteStatusID == 1 || r.QuoteStatusID == 2) && r.RequestStatusID == 6)
                                .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                                .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType)
                                .Include(r => r.ParentQuote).Include(r => r.ApplicationUserCreator);
                            iconList.Add(resendIcon);
                            iconList.Add(editQuoteDetailsIcon);
                            iconList.Add(deleteIcon);
                            viewModelByVendor.RequestsByVendor = quoteRequests.OrderByDescending(r => r.CreationDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.LabQuotes, r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory,
                        r.Product.ProductSubcategory.ParentCategory, r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, checkboxString, r.ParentQuote)

                            {
                                ButtonClasses = " confirm-quote lab-man-background-color ",
                                ButtonText = "Ask For Quote"
                            }).ToLookup(c => c.Vendor);
                            break;
                    }
                    break;
                case AppUtility.PageTypeEnum.AccountingNotifications:

                    var accountingNotificationsList = GetPaymentNotificationRequests(requestIndexObject.SidebarType);
                    if (notificationFilterViewModel == null)
                    {
                        notificationFilterViewModel = new NotificationFilterViewModel() { Vendors = _context.Vendors.Where(v => v.VendorCategoryTypes.Select(v => v.CategoryTypeID).Contains(1)).ToList() };
                    }
                    else
                    {
                        accountingNotificationsList = accountingNotificationsList.Where(r => (notificationFilterViewModel.SelectedVendor == null || r.Product.VendorID == notificationFilterViewModel.SelectedVendor)
                        && (notificationFilterViewModel.CentarixOrderNumber == null || r.ParentRequest.OrderNumber == notificationFilterViewModel.CentarixOrderNumber)
                        && (notificationFilterViewModel.ProductName == null || r.Product.ProductName.ToLower().Contains(notificationFilterViewModel.ProductName.ToLower())));
                    }
                    iconList.Add(popoverPartialClarifyIcon);
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.DidntArrive:
                            checkboxString = "";
                            break;
                        case AppUtility.SidebarEnum.PartialDelivery:
                            checkboxString = "";
                            break;
                        case AppUtility.SidebarEnum.ForClarification:
                            checkboxString = "";
                            break;
                        case AppUtility.SidebarEnum.NoInvoice:
                            iconList.Add(addInvoiceIcon);
                            buttonText = "Add To All";
                            break;
                    }
                    viewModelByVendor.RequestsByVendor = accountingNotificationsList.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel
                    (AppUtility.IndexTableTypes.AccountingNotifications, r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory,
                        r.Product.ProductSubcategory.ParentCategory, r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.ParentRequest, checkboxString, _context.Requests.Where(pr => pr.ParentRequestID == pr.ParentRequestID && r.ProductID == pr.ProductID && r.RequestStatusID == 2).FirstOrDefault())
                    {
                        ButtonClasses = " invoice-add-all accounting-background-color ",
                        ButtonText = buttonText
                    }).ToLookup(c => c.Vendor);
                    viewModelByVendor.NotificationFilterViewModel = notificationFilterViewModel;
                    break;
                case AppUtility.PageTypeEnum.AccountingPayments:

                    var paymentList = GetPaymentRequests(requestIndexObject.SidebarType).Result;
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.Installments:
                        case AppUtility.SidebarEnum.StandingOrders:
                            payNowIcon = new IconColumnViewModel(" icon-monetization_on-24px green-overlay ", "", "pay-invoice-one", "Pay");
                            checkboxString = "";
                            iconList.Add(payNowIcon);
                            iconList.Add(popoverMoreIcon);
                            var requestPaymentList =
                            viewModelByVendor.RequestsByVendor = paymentList.OrderByDescending(r => r.Request.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel
                             (AppUtility.IndexTableTypes.AccountingPaymentsInstallments, r.Request, r.Request.Product, r.Request.Product.Vendor, r.Request.Product.ProductSubcategory,
                                r.Request.Product.ProductSubcategory.ParentCategory, r.Request.Product.UnitType, r.Request.Product.SubUnitType, r.Request.Product.SubSubUnitType, requestIndexObject, iconList,
                                defaultImage, r.Request.ParentRequest, checkboxString, new List<Payment>() { r.Payment })

                            {
                                ButtonText = "",
                            }).ToLookup(c => c.Vendor);
                            break;
                        default:
                            iconList.Add(payNowIcon);
                            iconList.Add(popoverMoreIcon);
                            viewModelByVendor.RequestsByVendor = paymentList.OrderByDescending(r => r.Request.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel
                             (AppUtility.IndexTableTypes.AccountingPaymentsDefault, r.Request, r.Request.Product, r.Request.Product.Vendor, r.Request.Product.ProductSubcategory,
                        r.Request.Product.ProductSubcategory.ParentCategory, r.Request.Product.UnitType, r.Request.Product.SubUnitType, r.Request.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, r.Request.ParentRequest, checkboxString, new List<Payment>() { r.Payment })

                            {
                                ButtonClasses = " payments-pay-now accounting-background-color ",
                                ButtonText = "Pay All"
                            }).ToLookup(c => c.Vendor);
                            buttonText = "Pay All";
                            break;
                    }


                    break;
                case AppUtility.PageTypeEnum.RequestCart:
                    var cartRequests = _context.Requests
              .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
              .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType)
              .Include(r => r.ApplicationUserCreator)
              .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
              .Where(r => r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.AddToCart.ToString())
              .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1);

                    iconList.Add(deleteIcon);
                    viewModelByVendor.RequestsByVendor = cartRequests.OrderByDescending(r => r.CreationDate).Select(r => new RequestIndexPartialRowViewModel(AppUtility.IndexTableTypes.Cart, r, r.Product, r.Product.Vendor, r.Product.ProductSubcategory,
                        r.Product.ProductSubcategory.ParentCategory, r.Product.UnitType, r.Product.SubUnitType, r.Product.SubSubUnitType, requestIndexObject, iconList, defaultImage, checkboxString)
                    {
                        ButtonClasses = " load-terms-modal order-inv-background-color ",
                        ButtonText = "Order",
                    }).ToLookup(c => c.Vendor);

                    break;

            }
            List<PriceSortViewModel> priceSorts = new List<PriceSortViewModel>();
            Enum.GetValues(typeof(AppUtility.PriceSortEnum)).Cast<AppUtility.PriceSortEnum>().ToList().ForEach(p => priceSorts.Add(new PriceSortViewModel { PriceSortEnum = p, Selected = requestIndexObject.SelectedPriceSort.Contains(p.ToString()) }));
            viewModelByVendor.PricePopoverViewModel = new PricePopoverViewModel() { };
            viewModelByVendor.PricePopoverViewModel.PriceSortEnums = priceSorts;
            viewModelByVendor.PricePopoverViewModel.SelectedCurrency = requestIndexObject.SelectedCurrency;
            viewModelByVendor.PricePopoverViewModel.PopoverSource = 1;
            viewModelByVendor.PageType = requestIndexObject.PageType;
            viewModelByVendor.SidebarType = requestIndexObject.SidebarType;
            viewModelByVendor.ErrorMessage = requestIndexObject.ErrorMessage;
            return viewModelByVendor;
        }

        //protected void FillDocumentsInfo(RequestItemViewModel requestItemViewModel, string uploadFolder, ProductSubcategory productSubcategory)
        //{
        //    requestItemViewModel.DocumentsInfo = new List<DocumentFolder>();

        //    if (productSubcategory.ParentCategory.IsProprietary)
        //    {
        //        if (productSubcategory.ProductSubcategoryDescription == "Blood" || productSubcategory.ProductSubcategoryDescription == "Serum")
        //        {
        //            GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.S, uploadFolder);

        //        }
        //        if (productSubcategory.ProductSubcategoryDescription != "Blood" && productSubcategory.ProductSubcategoryDescription != "Serum"
        //            && productSubcategory.ProductSubcategoryDescription != "Cells")
        //        {
        //            GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, uploadFolder);

        //        }
        //        if (productSubcategory.ProductSubcategoryDescription != "Blood" && productSubcategory.ProductSubcategoryDescription != "Serum"
        //            && productSubcategory.ProductSubcategoryDescription != "Cells" && productSubcategory.ProductSubcategoryDescription != "Probes")
        //        {
        //            GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Map, uploadFolder);

        //        }
        //    }
        //    else if (requestItemViewModel.ParentCategories.FirstOrDefault().CategoryTypeID == 2)
        //    {
        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Orders, uploadFolder);
        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Invoices, uploadFolder);
        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Details, uploadFolder);
        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Quotes, uploadFolder);
        //    }
        //    else
        //    {
        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Quotes, uploadFolder);
        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Orders, uploadFolder);
        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Invoices, uploadFolder);
        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Shipments, uploadFolder);

        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, uploadFolder);
        //        GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Pictures, uploadFolder);
        //        //GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Returns, uploadFolder);
        //        //GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Credits, uploadFolder);
        //    }
        //}
        //protected async Task<RequestItemViewModel> FillRequestDropdowns(RequestItemViewModel requestItemViewModel, ProductSubcategory productSubcategory, int categoryTypeId)
        //{
        //    var parentcategories = new List<ParentCategory>();
        //    var productsubcategories = new List<ProductSubcategory>();
        //    var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
        //    if (productSubcategory != null)
        //    {
        //        if (categoryTypeId == 1)
        //        {
        //            parentcategories = await _context.ParentCategories.Where(pc => pc.ParentCategoryID == productSubcategory.ParentCategoryID).ToListAsync();
        //        }
        //        else
        //        {
        //            parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 2).ToListAsync();
        //        }
        //        productsubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == productSubcategory.ParentCategoryID).ToListAsync();
        //        unittypes = _context.UnitTypes.Where(ut => ut.UnitTypeParentCategory.Where(up => up.ParentCategoryID == productSubcategory.ParentCategoryID).Count() > 0).Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
        //    }
        //    else
        //    {
        //        if (requestItemViewModel.IsProprietary)
        //        {
        //            var proprietarycategory = await _context.ParentCategories.Where(pc => pc.ParentCategoryDescription == AppUtility.ParentCategoryEnum.Samples.ToString()).FirstOrDefaultAsync();
        //            productsubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == proprietarycategory.ParentCategoryID).ToListAsync();
        //        }
        //        else
        //        {
        //            parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == categoryTypeId && !pc.IsProprietary).ToListAsync();
        //        }
        //    }
        //    var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == categoryTypeId).Count() > 0).ToListAsync();
        //    var projects = await _context.Projects.ToListAsync();
        //    var subprojects = await _context.SubProjects.ToListAsync();
        //    var unittypeslookup = unittypes.ToLookup(u => u.UnitParentType);
        //    var paymenttypes = await _context.PaymentTypes.ToListAsync();
        //    var companyaccounts = await _context.CompanyAccounts.ToListAsync();
        //    List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();

        //    requestItemViewModel.ParentCategories = parentcategories;
        //    requestItemViewModel.ProductSubcategories = productsubcategories;
        //    requestItemViewModel.Vendors = vendors;
        //    requestItemViewModel.Projects = projects;
        //    requestItemViewModel.SubProjects = subprojects;

        //    requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");
        //    requestItemViewModel.UnitTypes = unittypeslookup;
        //    requestItemViewModel.CommentTypes = commentTypes;
        //    requestItemViewModel.PaymentTypes = paymenttypes;
        //    requestItemViewModel.CompanyAccounts = companyaccounts;
        //    return requestItemViewModel;
        //}


        protected bool SetFavorite<T1, T2>(T1 ModelInstanceID, T2 FavoriteTable, bool IsFavorite)
        {
            return true;
        }


        protected void SetViewModelCounts(RequestIndexObject requestIndexObject, RequestIndexPartialViewModel viewmodel, SelectedRequestFilters selectedFilters = null)
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
            IQueryable<Request> fullRequestsList = _context.Requests
              .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID)
              .Include(r => r.ApplicationUserCreator).Include(r => r.Product).ThenInclude(p => p.Vendor);
            IQueryable<Request> changingList = _context.Requests.Where(r => r.Product.ProductName.Contains(selectedFilters.SearchText))
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID)
                .Include(r => r.ApplicationUserCreator).Include(r => r.Product).ThenInclude(p => p.Vendor);
            changingList = FilterListBySelectFilters(selectedFilters, changingList);

            int[] requestStatusIds = { 1, 2, 3, 6 };
            int[] newRequestStatusIds = new int[2];
            if (requestIndexObject.RequestStatusID != 6)
            {
                newRequestStatusIds[0] = requestIndexObject.RequestStatusID;
            }
            else //for approval and approved are combined
            {
                newRequestStatusIds[0] = 1;
                newRequestStatusIds[1] = 6;
            }
            requestStatusIds = requestStatusIds.Where(id => !newRequestStatusIds.Contains(id)).ToArray();
            //foreach (int statusId in requestStatusIds)
            //{
            //    SetCountByStatusId(requestIndexObject, viewmodel, fullRequestsList, statusId);
            //}
            //foreach (int statusId in newRequestStatusIds)
            //{
            //    SetCountByStatusId(requestIndexObject, viewmodel, changingList, statusId);
            //}

            /*int newCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 1, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int orderedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 2, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int receivedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 3, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            int approvedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 6, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            viewmodel.NewCount = newCount;
            viewmodel.ApprovedCount = approvedCount;
            viewmodel.OrderedCount = orderedCount;
            viewmodel.ReceivedCount = receivedCount;*/
        }
        protected static void SetCountByStatusId(RequestIndexObject requestIndexObject, RequestIndexPartialViewModel viewmodel, IQueryable<Request> requestsList, int statusId)
        {
            int count = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(requestsList, statusId, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            switch (statusId)
            {
                case 1:
                    viewmodel.ApprovedCount += count;
                    break;
                case 2:
                    viewmodel.OrderedCount = count;
                    break;
                case 3:
                    viewmodel.ReceivedCount = count;
                    break;
                case 6:
                    viewmodel.ApprovedCount += count;
                    break;
                default:
                    break;
            }
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
            //SetViewModelCounts(requestIndexObject, viewModel, selectedFilters);
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
            //SetViewModelProprietaryCounts(requestIndexObject, viewModel, selectedFilters);
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



        protected void SetViewModelProprietaryCounts(RequestIndexObject requestIndexObject, RequestIndexPartialViewModel viewmodel, SelectedRequestFilters selectedFilters = null)
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
            IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ApplicationUserCreator).Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID)
                .Where(r => r.RequestStatus.RequestStatusID == 3).Include(r => r.Product).ThenInclude(p => p.Vendor).ToList().GroupBy(r => r.ProductID).Select(e => e.First()).AsQueryable();
            IQueryable<Request> fullRequestsListProprietary = _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryID)/*.Where(r => r.IsArchived == false)*/
                .Include(r => r.ApplicationUserCreator).Include(r => r.Product).ThenInclude(p => p.Vendor);
            if (requestIndexObject.RequestStatusID == 7)
            {
                fullRequestsListProprietary = FilterListBySelectFilters(selectedFilters, fullRequestsListProprietary);
                fullRequestsListProprietary = fullRequestsListProprietary.Where(r => r.Product.ProductName.Contains(selectedFilters.SearchText));
            }
            else
            {
                fullRequestsList = FilterListBySelectFilters(selectedFilters, fullRequestsList);
                fullRequestsList = fullRequestsList.Where(r => r.Product.ProductName.Contains(selectedFilters.SearchText));
            }

            //int nonProprietaryCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 3, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            //int proprietaryCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsListProprietary, 7, requestIndexObject.SidebarType, requestIndexObject.SidebarFilterID);
            //viewmodel.ProprietaryCount = proprietaryCount;
            //viewmodel.NonProprietaryCount = nonProprietaryCount;
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
            var userLists = _context.RequestLists.Where(l => l.ApplicationUserOwnerID == _userManager.GetUserId(User)).OrderBy(l => l.DateCreated).ToList();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (userLists.Count == 0)
                    {
                        RequestList requestList = new RequestList
                        {
                            Title = "List 1",
                            ApplicationUserOwnerID = _userManager.GetUserId(User),
                            RequestListRequests = new List<RequestListRequest>(),
                            DateCreated = DateTime.Now,
                            IsDefault = true
                        };

                        _context.Entry(requestList).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        requestIndexObject.ListID = requestList.ListID;
                        userLists.Add(requestList);
                    }
                }
                catch
                { }
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
            var userLists = _context.ShareRequestLists.Where(l => l.ToApplicationUserID == _userManager.GetUserId(User)).Include(l => l.RequestList).OrderBy(l=>l.TimeStamp).Select(l => l.RequestList).ToList();
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
        public async Task<RedirectToActionResult> SaveAddItemView(RequestItemViewModel requestItemViewModel, AppUtility.OrderTypeEnum OrderType, ReceivedModalVisualViewModel receivedModalVisualViewModel = null)
        {
            try
            {
                var vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Requests.FirstOrDefault().Product.VendorID);
                var exchangeRate = requestItemViewModel.Requests.FirstOrDefault().ExchangeRate;
                var currency = requestItemViewModel.Requests.FirstOrDefault().Currency;

                //in case we need to return to the modal view
                //requestItemViewModel.ParentCategory = await _context.ParentCategories.Where(pc => pc.ParentCategoryID == requestItemViewModel.Request.Product.ProductSubcategory.ParentCategory.ParentCategoryID).FirstOrDefaultAsync();

                //declared outside the if b/c it's used farther down too 
                var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

                var RequestNum = 1;
                var i = 1;
                var additionalRequests = false;
                var trlvm = new TempRequestListViewModel() { TempRequestViewModels = new List<TempRequestViewModel>() };
                foreach (var request in requestItemViewModel.Requests)
                {
                    //throw new Exception();
                    if (!request.Ignore)
                    {
                        request.ApplicationUserCreatorID = currentUser.Id;
                        if (!requestItemViewModel.IsProprietary)
                        {
                            request.Product.VendorID = vendor.VendorID;
                            request.Product.Vendor = vendor;
                            request.CreationDate = DateTime.Now;
                        }

                        request.Product.ProductSubcategory = await _context.ProductSubcategories.Include(ps => ps.ParentCategory).Where(ps => ps.ProductSubcategoryID == request.Product.ProductSubcategoryID).FirstOrDefaultAsync();
                        var isInBudget = false;
                        if (!request.Product.ProductSubcategory.ParentCategory.IsProprietary)
                        {
                            if (request.Currency == null)
                            {
                                request.Currency = AppUtility.CurrencyEnum.NIS.ToString();
                            }
                            isInBudget = checkIfInBudget(request);
                        }
                        request.ExchangeRate = exchangeRate;
                        TempRequestViewModel trvm = await AddItemAccordingToOrderType(request, OrderType, isInBudget, requestItemViewModel.TempRequestListViewModel, requestNum: RequestNum);


                        //var tempRequestJson = GetTempRequestAsync(requestItemViewModel.TempRequestListViewModel.GUID);
                        //TempRequestJson trj = new TempRequestJson();
                        //var t = trj.DeserializeJson<List<TempRequestViewModel>>();
                        if (requestItemViewModel.Comments != null)
                        {
                            trvm.Comments = new List<RequestComment>();
                            foreach (var comment in requestItemViewModel.Comments)
                            {
                                if (comment.CommentText != null && comment.CommentText?.Length != 0)
                                {
                                    //save the new comment
                                    comment.ApplicationUserID = currentUser.Id;

                                    comment.ObjectID = request.RequestID;

                                    trvm.Comments.Add(comment);
                                }
                            }
                        }
                        if (trvm.Request.RequestStatusID == 7)//issavedusingsessions
                        {
                            //await _context.SaveChangesAsync(); //what is this for?
                            if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                            {
                                await SaveLocations(receivedModalVisualViewModel, request, false);
                            }
                            if (i < requestItemViewModel.Requests.Count)
                            {
                                additionalRequests = true;
                            }
                            else
                            {
                                additionalRequests = false;
                            }
                            await SaveCommentFromTempRequestListViewModelAsync(request, trvm);
                            MoveDocumentsOutOfTempFolder(request.RequestID, AppUtility.ParentFolderName.Requests, additionalRequests, trlvm.GUID);
                            if (request.ParentQuoteID != null)
                            {
                                MoveDocumentsOutOfTempFolder((int)request.ParentQuoteID, AppUtility.ParentFolderName.ParentQuote, false, trlvm.GUID);
                            }
                            //await saveItemTransaction.CommitAsync();
                        }
                        else if (OrderType != AppUtility.OrderTypeEnum.SaveOperations)
                        {
                            trvm.Emails = new List<string>();
                            foreach (var e in requestItemViewModel.EmailAddresses.Where(e => e != null))
                            {
                                trvm.Emails.Add(e);
                            }
                        }
                        trlvm.TempRequestViewModels.Add(trvm);
                        i++;

                    }
                }
                using (var saveItemTransaction = _context.Database.BeginTransaction())
                {

                    try
                    {
                        //if (OrderType != AppUtility.OrderTypeEnum.AddToCart)
                        //{
                        //if (isInBudget)
                        //{
                        TempRequestJson trj = CreateTempRequestJson(requestItemViewModel.TempRequestListViewModel.GUID, 1);
                        await SetTempRequestAsync(trj, trlvm, requestItemViewModel.TempRequestListViewModel.RequestIndexObject);
                        await saveItemTransaction.CommitAsync();
                        //}
                        //else
                        //{
                        //    await SaveTempRequestAndCommentsAsync(trvm);
                        //    base.DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests);
                        //}
                        //}
                    }
                    catch (Exception ex)
                    {
                        await saveItemTransaction.RollbackAsync();
                        await RollbackCurrentTempAsync(requestItemViewModel.TempRequestListViewModel.GUID);
                        throw ex;
                    }
                }
            }

            catch (Exception ex)
            {
                //Redirect Results Need to be checked here
                //requestItemViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                await RemoveTempRequestAsync(requestItemViewModel.TempRequestListViewModel.GUID);
                //Response.WriteAsync(ex.Message?.ToString());
                /*if (requestItemViewModel.RequestStatusID == 7)
                {
                    return new RedirectToActionResult(actionName: "CreateItemTabs", controllerName: "Requests", routeValues: requestItemViewModel );
                }
                return new RedirectToActionResult(actionName: "_OrderTab", controllerName: "Requests", routeValues: requestItemViewModel );*/
                throw ex;

            }
            requestItemViewModel.TempRequestListViewModel.RequestIndexObject = new RequestIndexObject()
            {
                OrderType = OrderType,
                SectionType = requestItemViewModel.SectionType
            };
            //uncurrent the one we're on
            //await KeepTempRequestJsonCurrentAsOriginal(requestItemViewModel.TempRequestListViewModel.GUID);
            requestItemViewModel.TempRequestListViewModel.RequestIndexObject.GUID = requestItemViewModel.TempRequestListViewModel.GUID;
            switch (OrderType)
            {
                case AppUtility.OrderTypeEnum.AlreadyPurchased:
                    return new RedirectToActionResult("UploadOrderModal", "Requests", requestItemViewModel.TempRequestListViewModel.RequestIndexObject);
                case AppUtility.OrderTypeEnum.OrderNow:
                    return new RedirectToActionResult("UploadQuoteModal", "Requests", requestItemViewModel.TempRequestListViewModel.RequestIndexObject);
                case AppUtility.OrderTypeEnum.AddToCart:
                    return new RedirectToActionResult("UploadQuoteModal", "Requests", requestItemViewModel.TempRequestListViewModel.RequestIndexObject);
                case AppUtility.OrderTypeEnum.SaveOperations:
                    return new RedirectToActionResult("UploadOrderModal", "Requests", requestItemViewModel.TempRequestListViewModel.RequestIndexObject);
                default:
                    await RemoveTempRequestAsync(requestItemViewModel.TempRequestListViewModel.GUID);
                    if (requestItemViewModel.PageType == AppUtility.PageTypeEnum.RequestSummary)
                    {
                        return new RedirectToActionResult("IndexInventory", "Requests", new
                        {
                            PageType = requestItemViewModel.PageType,
                            SectionType = requestItemViewModel.SectionType,
                            SidebarType = AppUtility.SidebarEnum.List,
                            RequestStatusID = requestItemViewModel.Requests.FirstOrDefault().RequestStatusID,
                        });
                    }
                    return new RedirectToActionResult("Index", "Requests", new
                    {
                        PageType = requestItemViewModel.PageType,
                        SectionType = requestItemViewModel.SectionType,
                        SidebarType = AppUtility.SidebarEnum.List,
                        RequestStatusID = requestItemViewModel.Requests.FirstOrDefault().RequestStatusID,
                    });
            }
        }

        protected async Task SaveLocations(ReceivedModalVisualViewModel receivedModalVisualViewModel, Request requestReceived, bool archiveOneRequest)
        {
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
            foreach (var place in receivedModalVisualViewModel.LocationInstancePlaces)
            {
                if (place.Placed)
                {

                    //adding the requestlocationinstance
                    var rli = new RequestLocationInstance()
                    {
                        LocationInstanceID = place.LocationInstanceId,
                        RequestID = requestReceived.RequestID,
                        ParentLocationInstanceID = liParent.LocationInstanceID
                    };
                    var locationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == place.LocationInstanceId).FirstOrDefault();
                    if (locationInstance.IsFull == false)
                    {
                        _context.Add(rli);
                    }
                    else
                    {
                        throw new LocationAlreadyFullException();
                    }
                    try
                    {
                        if (archiveOneRequest)
                        {
                            requestReceived.IsArchived = true;
                            _context.Update(requestReceived);
                            await _context.SaveChangesAsync();
                            rli.IsArchived = true;
                            _context.Update(rli);
                            await _context.SaveChangesAsync();
                            MarkLocationAvailable(requestReceived.RequestID, place.LocationInstanceId);
                            await _context.SaveChangesAsync();
                            return;
                        }
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    //updating the locationinstance
                    //var locationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == place.LocationInstanceId).FirstOrDefault();
                    if (locationInstance.LocationTypeID == 103 || locationInstance.LocationTypeID == 205)
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
                        throw ex;
                    }
                }
            }
        }

        protected bool checkIfInBudget(Request request, Product oldProduct = null)
        {
            if (oldProduct == null)
            {
                oldProduct = request.Product;
            }
            var user = _context.Users.Where(u => u.Id == request.ApplicationUserCreatorID).FirstOrDefault();
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
                var monthsSpending = _context.Requests
                      .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                      .Where(r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID && r.Product.VendorID == oldProduct.VendorID)
                      .Where(r => r.ParentRequest.OrderDate >= firstOfMonth).AsEnumerable()
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

                var monthsSpending = _context.Requests
                    .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
                    .Where(r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID)
                    .Where(r => r.ParentRequest.OrderDate >= firstOfMonth)
                    .Sum(r => r.Cost);
                if (monthsSpending + request.Cost > user.OperationMonthlyLimit)
                {
                    return false;
                }
                return true;
            }
        }

        protected async Task<TempRequestViewModel> AddItemAccordingToOrderType(Request newRequest, AppUtility.OrderTypeEnum OrderTypeEnum, bool isInBudget, TempRequestListViewModel tempRequestListViewModel, int requestNum = 1)
        {
            var context = new ValidationContext(newRequest, null, null);
            var results = new List<ValidationResult>();
            var validatorCreate = Validator.TryValidateObject(newRequest, context, results, true);
            TempRequestViewModel trvm = new TempRequestViewModel();
            if (validatorCreate)
            {
                try
                {
                    switch (OrderTypeEnum)
                    {
                        case AppUtility.OrderTypeEnum.AddToCart:
                            trvm = await AddToCart(newRequest, isInBudget, tempRequestListViewModel);
                            break;
                        case AppUtility.OrderTypeEnum.AlreadyPurchased:
                            trvm = await AlreadyPurchased(newRequest, tempRequestListViewModel);
                            break;
                        case AppUtility.OrderTypeEnum.OrderNow:
                            trvm = await OrderNow(newRequest, isInBudget, tempRequestListViewModel);
                            break;
                        case AppUtility.OrderTypeEnum.RequestPriceQuote:
                            trvm = await RequestItem(newRequest, isInBudget);
                            break;
                        case AppUtility.OrderTypeEnum.Save: //proprietary
                            trvm = await SaveItem(newRequest, tempRequestListViewModel.GUID);
                            break;
                        case AppUtility.OrderTypeEnum.SaveOperations:
                            trvm = await SaveOperationsItem(newRequest, requestNum, tempRequestListViewModel);
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
            /*using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {*/
            if (isInBudget)
            {
                request.RequestStatusID = 6;
            }
            else
            {
                request.RequestStatusID = 1;
            }
            request.OrderType = AppUtility.OrderTypeEnum.AddToCart.ToString();

            //if (request.ProductID == 0 || request.ProductID == null)
            //{
            //    _context.Entry(request.Product).State = EntityState.Added;
            //}
            //_context.Entry(request).State = EntityState.Added;
            ////tempRequestListViewModel.TempRequestViewModels[0].Request = request;
            ////await SaveCommentFromTempRequestListViewModelAsync(request, tempRequestListViewModel.TempRequestViewModels[0]);
            //await _context.SaveChangesAsync();
            //await transaction.CommitAsync();
            tempRequestListViewModel.TempRequestViewModels = new List<TempRequestViewModel>() { new TempRequestViewModel() { Request = request } };
            /*TempRequestJson tempRequestJson = CreateTempRequestJson(tempRequestListViewModel.GUID);
            await SetTempRequestAsync(tempRequestJson, tempRequestListViewModel);*/
            //base.MoveDocumentsOutOfTempFolder(request.RequestID, AppUtility.ParentFolderName.Requests);
            //await transaction.CommitAsync();
            //}
            /*catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }*/
            //}
            return tempRequestListViewModel.TempRequestViewModels.FirstOrDefault();
        }

        private async Task SaveCommentFromTempRequestListViewModelAsync(Request request, TempRequestViewModel tempRequestViewModel)
        {
            if (tempRequestViewModel.Comments != null)
            {
                foreach (var comment in tempRequestViewModel.Comments)
                {
                    comment.ObjectID = request.RequestID;
                    _context.Add(comment);
                }
                await _context.SaveChangesAsync();
            }

        }

        private async Task<TempRequestViewModel> AlreadyPurchased(Request request, TempRequestListViewModel tempRequestListViewModel)
        {
            /*using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {*/
            request.RequestStatusID = 2;
            request.ParentQuoteID = null;
            request.OrderType = AppUtility.OrderTypeEnum.AlreadyPurchased.ToString();

            tempRequestListViewModel.TempRequestViewModels = new List<TempRequestViewModel>() { new TempRequestViewModel() { Request = request } };
            /*
                                TempRequestJson tempRequestJson = CreateTempRequestJson(tempRequestListViewModel.GUID);*/
            //TempRequestJson tempRequestJson = new TempRequestJson()
            //{
            //    CookieGUID = tempRequestListViewModel.GUID,
            //    ApplicationUserID = _userManager.GetUserId(User),
            //    IsOriginal = true,
            //    IsCurrent = true
            //};
            /*await SetTempRequestAsync(tempRequestJson, tempRequestListViewModel);
            await transaction.CommitAsync();*/
            /*}
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }*/
            return tempRequestListViewModel.TempRequestViewModels.FirstOrDefault();
        }
        private async Task<TempRequestViewModel> OrderNow(Request request, bool isInBudget, TempRequestListViewModel tempRequestListViewModel)
        {
            /*try
            {*/
            request.OrderType = AppUtility.OrderTypeEnum.OrderNow.ToString();
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
                    Request = request, Emails = new List<string>(){ request.Product.Vendor.OrdersEmail }
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

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    newRequest.RequestStatusID = 6;
                    newRequest.Cost = 0;
                    //newRequest.ParentQuote = new ParentQuote();
                    newRequest.QuoteStatusID = 1;
                    newRequest.OrderType = AppUtility.OrderTypeEnum.RequestPriceQuote.ToString();
                    //this is assuming that we only reorder request price quotes
                    _context.Entry(newRequest).State = EntityState.Added;
                    //_context.Entry(newRequest.ParentQuote).State = EntityState.Added;
                    _context.Entry(newRequest.Product).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
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
            return new TempRequestViewModel()
            {
                Request = newRequest
            };
        }
        private async Task<TempRequestViewModel> SaveItem(Request newRequest, Guid guid)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    newRequest.RequestStatusID = 7;
                    newRequest.OrderType = AppUtility.OrderTypeEnum.Save.ToString();
                    newRequest.Unit = 1;
                    newRequest.Product.UnitTypeID = 5;
                    newRequest.Product.SerialNumber = GetSerialNumber(false);
                    if (newRequest.CreationDate == DateTime.Today) //if it's today, add seconds to be now so it shows up on top
                    {
                        newRequest.CreationDate = DateTime.Now;
                    }
                    _context.Add(newRequest);
                    await _context.SaveChangesAsync();
                    MoveDocumentsOutOfTempFolder(newRequest.RequestID, AppUtility.ParentFolderName.Requests, false, guid);

                    newRequest.Product = await _context.Products.Where(p => p.ProductID == newRequest.ProductID).FirstOrDefaultAsync();
                    RequestNotification requestNotification = new RequestNotification();
                    requestNotification.RequestID = newRequest.RequestID;
                    requestNotification.IsRead = false;
                    requestNotification.RequestName = newRequest.Product.ProductName;
                    requestNotification.ApplicationUserID = newRequest.ApplicationUserCreatorID;
                    requestNotification.Description = "item created";
                    requestNotification.NotificationStatusID = 2;
                    requestNotification.TimeStamp = DateTime.Now;
                    requestNotification.Controller = "Requests";
                    requestNotification.Action = "NotificationsView";
                    requestNotification.OrderDate = DateTime.Now;
                    _context.Update(requestNotification);
                    await _context.SaveChangesAsync();
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
        private async Task<TempRequestViewModel> SaveOperationsItem(Request request, int requestNum, TempRequestListViewModel tempRequestListViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (request.IsReceived)
                    {
                        request.RequestStatusID = 3;
                        request.ApplicationUserReceiverID = _userManager.GetUserId(User);
                        request.ArrivalDate = DateTime.Now;
                        request.IsInInventory = true;
                    }
                    else
                    {
                        request.RequestStatusID = 2;
                    }
                    request.Product.UnitTypeID = 5;
                    request.OrderType = AppUtility.OrderTypeEnum.SaveOperations.ToString();

                    return new TempRequestViewModel() { Request = request };

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
                return tempRequestListViewModel.TempRequestViewModels[0];
            }
        }
        public TempRequestJson CreateTempRequestJson(Guid guid, int SequencePosition) //NEW!! --check after
        {
            return new TempRequestJson()
            {
                GuidID = guid,
                ApplicationUserID = _userManager.GetUserId(User),
                SequencePosition = SequencePosition,
                IsCurrent = true,
                IsOriginal = true
            };
        }

        public async Task SaveTempRequestAndCommentsAsync(TempRequestViewModel tempRequest)
        {
            if (tempRequest.Request.Product.ProductID == 0)
            {
                _context.Entry(tempRequest.Request.Product).State = EntityState.Added;
            }
            else
            {
                _context.Entry(tempRequest.Request.Product).State = EntityState.Modified;
            }
            if ((tempRequest.Request.ParentQuoteID == 0 || tempRequest.Request.ParentQuoteID == null) && tempRequest.Request.ParentQuote != null)
            {
                _context.Entry(tempRequest.Request.ParentQuote).State = EntityState.Added;
            }
            else if ((tempRequest.Request.ParentQuote != null))
            {
                _context.Entry(tempRequest.Request.ParentQuote).State = EntityState.Unchanged;
            }
            _context.Entry(tempRequest.Request).State = EntityState.Added;
            _context.Entry(tempRequest.Request.ParentRequest).State = EntityState.Added;

            await _context.SaveChangesAsync();

            if (tempRequest.Comments != null && tempRequest.Comments.Any()) //do we need this check?
            {
                foreach (var comment in tempRequest.Comments)
                {
                    comment.ObjectID = tempRequest.Request.RequestID;
                    _context.Add(comment);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetTempRequestAsync(TempRequestJson tempRequestJson, TempRequestListViewModel tempRequestListViewModel, RequestIndexObject requestIndexObject)
        {
            var fullRequestJson = new FullRequestJson()
            {
                TempRequestViewModels = tempRequestListViewModel.TempRequestViewModels,
                RequestIndexObject = requestIndexObject
            };
            tempRequestJson.SerializeViewModel(fullRequestJson);

            _context.Update(tempRequestJson);
            await _context.SaveChangesAsync();
        }
        public async Task<TempRequestJson> GetTempRequestAsync(Guid cookieID)
        {
            return await _context.TempRequestJsons
                .Where(t => t.GuidID == cookieID && t.ApplicationUserID == _userManager.GetUserId(User))
                .OrderByDescending(t => t.SequencePosition)
                .FirstOrDefaultAsync();
        }
        public async Task RollbackCurrentTempAsync(Guid GUID)
        //remove the one that is current if it is NOT the original
        {
            var currentTRJ = _context.TempRequestJsons
                .Where(t => t.GuidID == GUID && t.ApplicationUserID == _userManager.GetUserId(User)
                && t.IsCurrent && !t.IsOriginal)
                .FirstOrDefault();
            if (currentTRJ != null)
            {
                _context.Remove(currentTRJ);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task<TempRequestListViewModel> RollbackTempAndReturnTRLVM(Guid Guid, int SequencePosition)
        //{

        //}
        public async Task<TempRequestJson> CopyToNewCurrentTempRequestAsync(TempRequestJson original, int SequencePosition)
        {
            original.IsCurrent = false; //just to make sure but i think this should be taken out here b/c it's done before...
            original.IsOriginal = true; //just to make sure that there wont be any mistakes...
            _context.Update(original);
            await _context.SaveChangesAsync();

            TempRequestJson newTempRequestJson = new TempRequestJson()
            {
                Json = original.Json,
                ApplicationUserID = original.ApplicationUserID,
                GuidID = original.GuidID,
                SequencePosition = SequencePosition,
                IsOriginal = false,
                IsCurrent = true
            };
            _context.Add(newTempRequestJson);
            await _context.SaveChangesAsync();

            return newTempRequestJson;
        }

        public async Task<RedirectToActionResult> RollbackTempRequestHiddenFors(Guid Guid, int SequencePosition)
        {
            try
            {
                if (SequencePosition == 0)
                {
                    throw new Exception();
                }
                var current = _context.TempRequestJsons.Where(t => t.GuidID == Guid && t.SequencePosition == SequencePosition).FirstOrDefault();
                _context.Remove(current);
                _context.SaveChanges();
                var oneStepBackID = _context.TempRequestJsons.Where(t => t.GuidID == Guid && t.SequencePosition == SequencePosition - 1).FirstOrDefault().TempRequestJsonID;

                return RedirectToAction("_TempRequestHiddenFors", new { ID = oneStepBackID });
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
            TempRequestJson tempRequestJson = _context.TempRequestJsons.Where(t => t.TempRequestJsonID == ID).FirstOrDefault();
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
        //public async Task KeepTempRequestJsonCurrentAsOriginal(Guid GUID)
        ////take away current from original
        ////or switch current to original and remove current from table
        //{
        //    var allGUIDTRJs = _context.TempRequestJsons.Where(t => t.GuidID == GUID && t.ApplicationUserID == _userManager.GetUserId(User)).ToList();
        //    var origAndCurrent = allGUIDTRJs.Where(t => t.IsCurrent && t.IsOriginal).FirstOrDefault();
        //    if (origAndCurrent != null)
        //    {
        //        var olderTemps = allGUIDTRJs.Where(t => !t.IsCurrent);
        //        foreach (var temp in olderTemps)
        //        {
        //            _context.Remove(temp);
        //        };
        //        await _context.SaveChangesAsync();
        //        origAndCurrent.IsCurrent = false;
        //        _context.Update(origAndCurrent);
        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        var current = allGUIDTRJs.Where(t => t.IsCurrent).FirstOrDefault();
        //        var original = allGUIDTRJs.Where(t => t.IsOriginal).FirstOrDefault();
        //        if (current != null)
        //        {
        //            current.IsCurrent = false;
        //            current.IsOriginal = true;
        //            _context.Update(current);
        //            _context.Remove(original);
        //            await _context.SaveChangesAsync();
        //        }
        //    }
        //}
        public async Task RemoveTempRequestAsync(Guid GUID)
        //This will remove ALL --> Do not use it until you are completely done with this GUID
        {
            var allTRJs = _context.TempRequestJsons.Where(t => t.GuidID == GUID && t.ApplicationUserID == _userManager.GetUserId(User)).ToList();
            allTRJs.ForEach(tempRequestJson => _context.Remove(tempRequestJson));
            await _context.SaveChangesAsync();
        }
        [Authorize(Roles = "Requests, Operations")]
        public async Task<TermsViewModel> GetTermsViewModelAsync(int vendorID, List<int> requestIds, TempRequestListViewModel tempRequestListViewModel)
        {
            StringWithBool Error = new StringWithBool();
            //var requ = _httpContextAccessor.HttpContext.Session.GetObject<Request>("Request1");
            //List<Request> requests = new List<Request>();

            //var newTRLVM = new TempRequestListViewModel();
            //if (tempRequestListViewModel != null && tempRequestListViewModel.GUID != null)
            //{
            //    var oldTempRequestJson = await GetTempRequestAsync(tempRequestListViewModel.GUID);
            //    //var newTempRequestJson = await CopyToNewCurrentTempRequestAsync(oldTempRequestJson);

            //    newTRLVM.TempRequestViewModels = oldTempRequestJson.DeserializeJson<List<TempRequestViewModel>>();
            //    newTRLVM.GUID = tempRequestListViewModel.GUID;
            //    newTRLVM.RequestIndexObject = tempRequestListViewModel.RequestIndexObject;
            //}
            if (vendorID != 0)
            {
                List<Request> reqsFromDB = new List<Request>();
                if (tempRequestListViewModel.RequestIndexObject.SidebarType == AppUtility.SidebarEnum.Cart)
                {
                    reqsFromDB = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                          .Where(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.AddToCart.ToString()
                          && r.QuoteStatusID == 4)
                          .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
                          .Include(r => r.Product).ThenInclude(r => r.Vendor)
                          .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
                }
                else if (tempRequestListViewModel.RequestIndexObject.SidebarType == AppUtility.SidebarEnum.Orders)
                {
                    reqsFromDB = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                      .Where(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()
                      && r.QuoteStatusID == 4)
                    .Include(r => r.Product).ThenInclude(r => r.Vendor)
                    .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
                }
                tempRequestListViewModel.TempRequestViewModels = new List<TempRequestViewModel>();

                AppUtility.CurrencyEnum CurrencyUsed = AppUtility.CurrencyEnum.None;
                Enum.TryParse(reqsFromDB.FirstOrDefault().Currency, out CurrencyUsed);
                var VendorID = reqsFromDB.FirstOrDefault().Product.VendorID;
                foreach (var req in reqsFromDB)
                {
                    if (req.Currency != CurrencyUsed.ToString())
                    {
                        if (Error.Bool != true)
                        {
                            Error.SetStringAndBool(true, ElixirStrings.ServerDifferentCurrencyErrorMessage);
                        }
                    }
                    if (req.Product.VendorID != vendorID)
                    {
                        Error.SetStringAndBool(true, ElixirStrings.ServerDifferentVendorErrorMessage);
                    }
                    tempRequestListViewModel.TempRequestViewModels.Add(new TempRequestViewModel() { Request = req });
                }
                var updatedTempRequestJson = CreateTempRequestJson(tempRequestListViewModel.GUID, 0);
                await SetTempRequestAsync(updatedTempRequestJson, tempRequestListViewModel, tempRequestListViewModel.RequestIndexObject);

                //tempRequestListViewModel.GUID = Guid.NewGuid();
            }
            else if (requestIds != null && requestIds.Count != 0)
            {
                tempRequestListViewModel.GUID = Guid.NewGuid();
                tempRequestListViewModel.TempRequestViewModels = new List<TempRequestViewModel>();
                var requests = _context.Requests.Where(r => requestIds.Contains(r.RequestID)).Include(r => r.Product).ThenInclude(p => p.Vendor)
                    .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory);
                AppUtility.CurrencyEnum CurrencyUsed = AppUtility.CurrencyEnum.None;
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
                var updatedTempRequestJson = CreateTempRequestJson(tempRequestListViewModel.GUID, 0);
                await SetTempRequestAsync(updatedTempRequestJson, tempRequestListViewModel, tempRequestListViewModel.RequestIndexObject);
            }

            //foreach (var req in requests)
            //{
            //    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + requestNum;
            //    _httpContextAccessor.HttpContext.Session.SetObject(requestName, req);
            //    requestNum++;
            //}


            var termsList = new List<SelectListItem>() { };
            await _context.PaymentStatuses.ForEachAsync(ps =>
            {
                var selected = false;
                if (ps.PaymentStatusID == 2) { selected = true; }
                if (ps.PaymentStatusID != 4 && ps.PaymentStatusID != 7)
                {
                    termsList.Add(new SelectListItem() { Value = ps.PaymentStatusID + "", Text = ps.PaymentStatusDescription, Selected = selected });
                }
            });
            TermsViewModel termsViewModel = new TermsViewModel()
            {
                ParentRequest = new ParentRequest(),
                TermsList = termsList,
                InstallmentDate = DateTime.Now,
                Error = Error
            };
            tempRequestListViewModel.RequestIndexObject.SelectedCurrency = (AppUtility.CurrencyEnum)Enum.Parse(typeof(AppUtility.CurrencyEnum),
                tempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Currency);
            termsViewModel.TempRequestListViewModel = tempRequestListViewModel;

            //await KeepTempRequestJsonCurrentAsOriginal(tempRequestListViewModel.GUID);

            return termsViewModel;
        }

        public int GetNewSequencePosition(Guid GUID)
        {
            return _context.TempRequestJsons.Where(t => t.GuidID == GUID).OrderByDescending(t => t.SequencePosition).FirstOrDefault().SequencePosition + 1;
        }

        public async Task<RedirectAndModel> SaveTermsModalAsync(TermsViewModel termsViewModel, TempRequestListViewModel tempRequestListViewModel)
        {
            var controller = "Requests";
            var needsToBeApproved = false;
            try
            {
                var oldTempRequestJson = await GetTempRequestAsync(tempRequestListViewModel.GUID);
                var sequencePosition = tempRequestListViewModel.SequencePosition + 1;
                var newTempRequestJson = await CopyToNewCurrentTempRequestAsync(oldTempRequestJson, sequencePosition);
                //var newTRLVM = await LoadTempListFromRequestIndexObjectAsync(tempRequestListViewModel.RequestIndexObject);

                var newTRLVM = new TempRequestListViewModel { TempRequestViewModels = newTempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels };
                newTRLVM.GUID = tempRequestListViewModel.GUID;
                newTRLVM.RequestIndexObject = tempRequestListViewModel.RequestIndexObject;
                newTRLVM.SequencePosition = sequencePosition;

                //var requests = new List<Request>();
                //var isRequests = true;
                //var RequestNum = 1;
                //while (isRequests)
                //{
                //    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                //    if (_httpContextAccessor.HttpContext.Session.GetObject<Request>(requestName) != null)
                //    {
                //        requests.Add(_httpContextAccessor.HttpContext.Session.GetObject<Request>(requestName));
                //    }
                //    else
                //    {
                //        isRequests = false;
                //    }
                //    RequestNum++;
                //}

                //RequestNum = 1;
                //var PaymentNum = 1;
                var SaveUsingTempRequest = true;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
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
                        foreach (var tempRequest in newTRLVM.TempRequestViewModels)
                        {
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
                                tempRequest.Request.Product = _context.Products.Where(p => p.ProductID == tempRequest.Request.ProductID).Include(p => p.ProductSubcategory).FirstOrDefault();
                            }

                            if (tempRequest.Request.OrderType == AppUtility.OrderTypeEnum.AlreadyPurchased.ToString() || tempRequest.Request.OrderType == AppUtility.OrderTypeEnum.SaveOperations.ToString() || needsToBeApproved)
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
                                if (tempRequest.Request.OrderType == AppUtility.OrderTypeEnum.SaveOperations.ToString())
                                {
                                    isOperations = true;
                                }
                                tempRequest.Request.Product.SerialNumber = GetSerialNumber(isOperations);
                                await SaveTempRequestAndCommentsAsync(tempRequest);
                            }
                            else if (SaveUsingTempRequest)
                            {
                                tempRequest.Payments = new List<Payment>();
                            }
                            for (int i = 0; i < tempRequest.Request.Installments; i++)
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
                                if (SaveUsingTempRequest)
                                {
                                    tempRequest.Payments.Add(payment);
                                }
                                else
                                {
                                    payment.RequestID = tempRequest.Request.RequestID;
                                    _context.Add(payment);
                                    await _context.SaveChangesAsync();
                                }
                            }

                        }
                        if (SaveUsingTempRequest)
                        {
                            await SetTempRequestAsync(newTempRequestJson, newTRLVM, tempRequestListViewModel.RequestIndexObject);
                            await transaction.CommitAsync();
                            //await KeepTempRequestJsonCurrentAsOriginal(newTRLVM.GUID);
                        }
                        if (!SaveUsingTempRequest)
                        {
                            //foreach (var tempRequest in newTRLVM.TempRequestViewModels)
                            for (int n = 0; n < newTRLVM.TempRequestViewModels.Count; n++)
                            {
                                var additionalRequests = n + 1 < newTRLVM.TempRequestViewModels.Count ? true : false;
                                MoveDocumentsOutOfTempFolder(newTRLVM.TempRequestViewModels[n].Request.RequestID, AppUtility.ParentFolderName.Requests, additionalRequests, newTRLVM.GUID);
                                newTRLVM.TempRequestViewModels[n].Request.Product.Vendor = _context.Vendors.Where(v => v.VendorID == newTRLVM.TempRequestViewModels[n].Request.Product.VendorID).FirstOrDefault();
                                if (!needsToBeApproved)
                                {
                                    RequestNotification requestNotification = new RequestNotification();
                                    requestNotification.RequestID = newTRLVM.TempRequestViewModels[n].Request.RequestID;
                                    requestNotification.IsRead = false;
                                    requestNotification.RequestName = newTRLVM.TempRequestViewModels[n].Request.Product.ProductName;
                                    requestNotification.ApplicationUserID = newTRLVM.TempRequestViewModels[n].Request.ApplicationUserCreatorID;
                                    requestNotification.Description = "item ordered";
                                    requestNotification.NotificationStatusID = 2;
                                    requestNotification.TimeStamp = DateTime.Now;
                                    requestNotification.Controller = "Requests";
                                    requestNotification.Action = "NotificationsView";
                                    requestNotification.OrderDate = DateTime.Now;
                                    requestNotification.Vendor = newTRLVM.TempRequestViewModels[n].Request.Product.Vendor.VendorEnName;
                                    _context.Add(requestNotification);
                                }
                            }
                            MoveDocumentsOutOfTempFolder((int)newTRLVM.TempRequestViewModels[0].Request.ParentRequestID, AppUtility.ParentFolderName.ParentRequest, false, newTRLVM.GUID); //either they all have same parentrequests are already outof the temp folder
                            if (newTRLVM.TempRequestViewModels[0].Request.ParentQuoteID != null)
                            {
                                MoveDocumentsOutOfTempFolder((int)newTRLVM.TempRequestViewModels[0].Request.ParentQuoteID, AppUtility.ParentFolderName.ParentQuote, false, newTRLVM.GUID); //either they all have same parentrequests are already outof the temp folder

                            }
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();

                            await RemoveTempRequestAsync(newTRLVM.GUID);
                            tempRequestListViewModel.RequestIndexObject.GUID = tempRequestListViewModel.GUID;
                            if (!needsToBeApproved)
                            {
                                return new RedirectAndModel() { RedirectToActionResult = new RedirectToActionResult("Index", controller, tempRequestListViewModel.RequestIndexObject) };
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        await RollbackCurrentTempAsync(newTRLVM.GUID);
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
                await RollbackCurrentTempAsync(tempRequestListViewModel.GUID);
                termsViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                //var termsList = new List<SelectListItem>() { };
                //await _context.PaymentStatuses.ForEachAsync(ps => termsList.Add(new SelectListItem() { Value = ps.PaymentStatusID + "", Text = ps.PaymentStatusDescription }));
                //termsViewModel.TermsList = termsList;
                //termsViewModel.TempRequestListViewModel = tempRequestListViewModel;
                return new RedirectAndModel() { RedirectToActionResult = new RedirectToActionResult("", "", ""), TermsViewModel = termsViewModel };
            }
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
        public async Task<IActionResult> _IndexTableWithListTabs(RequestIndexObject requestIndexObject)
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
        public async Task<IActionResult> DeleteModal(int? id, RequestIndexObject requestIndexObject)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
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
                RequestIndexObject = requestIndexObject
            };

            return PartialView(deleteRequestViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteModal(DeleteRequestViewModel deleteRequestViewModel, RequestsSearchViewModel requestsSearchViewModel, SelectedRequestFilters selectedFilters, int numFilters = 0)
        {
            try
            {
                var request = _context.Requests.Where(r => r.RequestID == deleteRequestViewModel.Request.RequestID)
                    .Include(r => r.ParentRequest).Include(r => r.RequestLocationInstances).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory)
                    .ThenInclude(ps => ps.ParentCategory)
                    .FirstOrDefault();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        request.IsDeleted = true;
                        _context.Update(request);
                        await _context.SaveChangesAsync();

                        var parentRequest = _context.ParentRequests.Where(pr => pr.ParentRequestID == request.ParentRequestID).FirstOrDefault();
                        if (parentRequest != null)
                        {
                            parentRequest.Requests = _context.Requests.Where(r => r.ParentRequestID == parentRequest.ParentRequestID && r.IsDeleted != true).ToList();
                            if (parentRequest.Requests.Count() == 0)
                            {
                                parentRequest.IsDeleted = true;
                                var payments = _context.Payments.Where(p => p.RequestID == request.RequestID).ToList();
                                foreach (var payment in payments)
                                {
                                    payment.IsDeleted = true;
                                    _context.Update(payment);
                                    await _context.SaveChangesAsync();
                                }
                                _context.Update(parentRequest);
                                await _context.SaveChangesAsync();
                            }
                        }
                        var parentQuote = _context.ParentQuotes.Where(pr => pr.ParentQuoteID == request.ParentQuoteID).FirstOrDefault();

                        if (parentQuote != null)
                        {
                            parentQuote.Requests = _context.Requests.Where(r => r.ParentQuoteID == parentQuote.ParentQuoteID && r.IsDeleted != true).ToList();
                            //todo figure out the soft delete with child of a parent entity so we could chnage it to 0 or null
                            if (parentQuote.Requests.Count() == 0)
                            {
                                parentQuote.IsDeleted = true;
                                _context.Update(parentQuote);
                                await _context.SaveChangesAsync();
                            }
                        }
                        var productRequests = _context.Requests.Where(r => r.ProductID == request.ProductID).ToList();
                        if (productRequests.Count() == 0)
                        {
                            var product = request.Product;
                            product.IsDeleted = true;
                            _context.Update(product);
                            await _context.SaveChangesAsync();
                        }
                        var requestLocationInstances = request.RequestLocationInstances.ToList();
                        foreach (var requestLocationInstance in requestLocationInstances)
                        {
                            var locationInstance = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstance.LocationInstanceID).FirstOrDefault();
                            locationInstance.IsFull = false;

                            _context.Remove(requestLocationInstance);
                            await _context.SaveChangesAsync();
                            _context.Update(locationInstance);
                            await _context.SaveChangesAsync();
                        }
                        var comments = _context.RequestComments.Where(c => c.ObjectID == request.RequestID).ToList();
                        foreach (var comment in comments)
                        {
                            comment.IsDeleted = true;
                            _context.Update(comment);
                            await _context.SaveChangesAsync();
                        }
                        var notifications = _context.RequestNotifications.Where(rn => rn.RequestID == request.RequestID).ToList();

                        foreach (var notification in notifications)
                        {
                            _context.Remove(notification);
                            await _context.SaveChangesAsync();
                        }
                        //throw new Exception();
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(AppUtility.GetExceptionMessage(e));
                    }

                }
                /*                if (deleteRequestViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.LabManagementQuotes)
                                {
                                    if (deleteRequestViewModel.RequestIndexObject.SidebarType == AppUtility.SidebarEnum.Quotes)
                                    {
                                        return RedirectToAction("LabManageQuotes");
                                    }
                                    else
                                    {
                                        return RedirectToAction("LabManageOrders");
                                    }

                                }
                                else if (deleteRequestViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart)
                                {
                                    return RedirectToAction("Cart");
                                }
                                else if (deleteRequestViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestSummary)
                                {
                                    return RedirectToAction("IndexInventory", deleteRequestViewModel.RequestIndexObject);
                                }
                                else
                                {
                                    return RedirectToAction("Index", deleteRequestViewModel.RequestIndexObject);
                                }*/
            }

            catch (Exception ex)
            {
                deleteRequestViewModel.RequestIndexObject.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
            }
            switch (deleteRequestViewModel.RequestIndexObject.SidebarType)
            {
                case AppUtility.SidebarEnum.Quotes:
                case AppUtility.SidebarEnum.Orders:
                case AppUtility.SidebarEnum.Cart:
                    return PartialView("_IndexTableDataByVendor", await GetIndexViewModelByVendor(deleteRequestViewModel.RequestIndexObject));
                default:
                    return PartialView("_IndexTableData", await GetIndexViewModel(deleteRequestViewModel.RequestIndexObject, selectedFilters: selectedFilters, numFilters: numFilters, requestsSearchViewModel: requestsSearchViewModel));

            }
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
            //RemoveRequestWithCommentsAndEmailSessions();

            TempRequestJson tempRequestJson = CreateTempRequestJson(Guid.NewGuid(), 0);
            TempRequestListViewModel tempRequestListViewModel = new TempRequestListViewModel()
            {
                GUID = tempRequestJson.GuidID,
                SequencePosition = tempRequestJson.SequencePosition,
                TempRequestViewModels = new List<TempRequestViewModel>()
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    _context.Add(tempRequestJson);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(AppUtility.GetExceptionMessage(e));
                }
            }

            requestItemViewModel.TempRequestListViewModel = tempRequestListViewModel;

            TempListWithRequestItem tempViewModel = new TempListWithRequestItem()
            {
                TempRequestListViewModel = tempRequestListViewModel,
                RequestItemViewModel = requestItemViewModel
            };

            //await KeepTempRequestJsonCurrentAsOriginal(tempRequestListViewModel.GUID);
            return View(tempViewModel);
        }

        private TempRequestListViewModel InitializeTempRequestListViewModel()
        {
            return new TempRequestListViewModel()
            {
                TempRequestViewModels = new List<TempRequestViewModel>(),
                GUID = Guid.NewGuid(),
                RequestIndexObject = new RequestIndexObject()
            };
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> AddItemView(AppUtility.OrderTypeEnum OrderType, TempRequestListViewModel tempRequestListViewModel, RequestItemViewModel requestItemViewModel, ReceivedModalVisualViewModel receivedModalVisualViewModel = null)
        {
            requestItemViewModel.TempRequestListViewModel = tempRequestListViewModel;
            try
            {
                var redirectToActionResult = SaveAddItemView(requestItemViewModel, OrderType, receivedModalVisualViewModel).Result;
                return RedirectToAction(redirectToActionResult.ActionName, redirectToActionResult.ControllerName, redirectToActionResult.RouteValues);
            }
            catch (Exception ex)
            {
                requestItemViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                return PartialView("_ErrorMessage", requestItemViewModel.ErrorMessage);
            }
        }

        private static List<IconColumnViewModel> GetIconsByIndividualRequest(int RequestID, List<IconColumnViewModel> iconList, bool needsPlaceholder, FavoriteRequest favoriteRequest = null, Request request = null, ApplicationUser user = null)
        {
            var newIconList = AppUtility.DeepClone(iconList);
            //favorite icon
            var favIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink.Contains("request-favorite"));

            if (favIconIndex != -1 && favoriteRequest != null) //check these checks
            {
                var unLikeIcon = new IconColumnViewModel(" icon-favorite-24px", "var(--order-inv-color);", "request-favorite request-unlike", "Unfavorite");
                newIconList[favIconIndex] = unLikeIcon;
            }
            //for approval icon
            if (request != null)
            {
                var forApprovalIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink.Contains("approve-order"));
                if (request.RequestStatusID != 1 && forApprovalIconIndex != -1)
                {
                    newIconList.RemoveAt(forApprovalIconIndex);
                    needsPlaceholder = true;
                }
                //resend icon
                var resendIcon = new IconColumnViewModel("Resend");
                var placeholder = new IconColumnViewModel("Placeholder");
                if (request.QuoteStatusID == 2)
                {
                    newIconList.Insert(0, resendIcon);
                }
                else if (needsPlaceholder)
                {
                    newIconList.Insert(0, placeholder);
                }
            }
            return newIconList;
        }


        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> CreateItemTabs(int productSubCategoryId, AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest, string itemName = "",
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

            //TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.RequestPageTypeEnum.Request;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;

            return PartialView(requestItemViewModel);
        }
        private async Task<RequestItemViewModel> FillRequestItemViewModel(RequestItemViewModel requestItemViewModel, int categoryTypeId, int productSubcategoryId = 0)
        {
            var productSubcategory = await _context.ProductSubcategories.Where(ps => ps.ProductSubcategoryID == productSubcategoryId).FirstOrDefaultAsync();
            requestItemViewModel = await FillRequestDropdowns(requestItemViewModel, productSubcategory, categoryTypeId);

            if (productSubcategory == null)
            {
                ParentCategory parentCategory = new ParentCategory();
                if (requestItemViewModel.IsProprietary)
                {
                    parentCategory = await _context.ParentCategories.Where(pc => pc.ParentCategoryDescription == AppUtility.ParentCategoryEnum.Samples.ToString()).FirstOrDefaultAsync();
                }

                productSubcategory = new ProductSubcategory()
                {
                    ParentCategory = parentCategory
                };
            }
            else if (productSubcategory.ParentCategory.ParentCategoryDescription == AppUtility.ParentCategoryEnum.Samples.ToString())
            {
                requestItemViewModel.IsProprietary = true;
            }

            requestItemViewModel.Comments = new List<RequestComment>();
            requestItemViewModel.EmailAddresses = new List<string>() { "", "", "", "", "" };
            requestItemViewModel.ModalType = AppUtility.RequestModalType.Create;

            requestItemViewModel.Requests = new List<Request>();
            requestItemViewModel.Requests.Add(new Request());
            requestItemViewModel.Requests.FirstOrDefault().ExchangeRate = GetExchangeRate();
            requestItemViewModel.Requests.FirstOrDefault().Product = new Product();
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
                    locationTypesDepthZero = _context.LocationTypes.Where(lt => lt.Depth == 0),
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
                ParentCategories = _context.ParentCategories.Where(pc => pc.CategoryTypeID == 2).ToList(),
                ProductSubcategories = new List<ProductSubcategory>()
            };
            operationsItemViewModel.Request = new Request() { IncludeVAT = true };
            if (subcategoryID > 0)
            {
                operationsItemViewModel.Request.Product = new Product();
                operationsItemViewModel.Request.Product.ProductSubcategoryID = subcategoryID;
                operationsItemViewModel.Request.Product.ProductSubcategory =
                    _context.ProductSubcategories.Where(ps => ps.ProductSubcategoryID == subcategoryID).FirstOrDefault();
                operationsItemViewModel.ProductSubcategories =
                    _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == operationsItemViewModel.Request.Product.ProductSubcategory.ParentCategoryID).ToList();
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
                var favoriteRequest = _context.FavoriteRequests.Where(fr => fr.RequestID == requestID && fr.ApplicationUserID == userID).FirstOrDefault();
                if (favoriteRequest == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            favoriteRequest = new FavoriteRequest()
                            {
                                RequestID = requestID,
                                ApplicationUserID = userID
                            };
                            _context.Add(favoriteRequest);
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        //throw new Exception(); //check this after!
                        catch (Exception e)
                        {
                            await transaction.RollbackAsync();
                            await Response.WriteAsync(AppUtility.GetExceptionMessage(e));
                        }
                    }
                }
            }
            else if (FavType == "unlike")
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var favoriteRequest = _context.FavoriteRequests
                            .Where(fr => fr.ApplicationUserID == userID)
                            .Where(fr => fr.RequestID == requestID).FirstOrDefault();
                        _context.Remove(favoriteRequest);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    //throw new Exception(); //check this after!
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        await Response.WriteAsync(AppUtility.GetExceptionMessage(e));
                    }
                }
                if (sidebarType == AppUtility.SidebarEnum.Favorites)
                {
                    RequestIndexObject requestIndexObject = new RequestIndexObject()
                    {
                        PageType = AppUtility.PageTypeEnum.RequestCart,
                        SidebarType = sidebarType
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
            //var shareRequestViewModel = new ShareRequestViewModel()
            //{
            //    Request = _context.Requests.Where(r => r.RequestID == requestID).Include(r => r.Product).FirstOrDefault(),
            //    ApplicationUsers = _context.Users
            //                  .Where(u => u.Id != _userManager.GetUserId(User))
            //                  .Select(
            //                      u => new SelectListItem
            //                      {
            //                          Text = u.FirstName + " " + u.LastName,
            //                          Value = u.Id
            //                      }
            //                  ).ToList()
            //};
            var shareViewModel = base.GetShareModalViewModel(ID, ModelsEnum);
            switch (ModelsEnum)
            {
                case AppUtility.ModelsEnum.Request:
                    shareViewModel.ObjectDescription = _context.Requests.Where(r => r.RequestID == ID).Include(r => r.Product).FirstOrDefault().Product.ProductName;
                    break;
                case AppUtility.ModelsEnum.RequestLists:
                    shareViewModel.ObjectDescription = _context.RequestLists.Where(rl => rl.ListID == ID).FirstOrDefault().Title;
                    break;
            }

            return PartialView(shareViewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<bool> ShareModal(ShareModalViewModel shareModalViewModel)
        {
            bool error = false;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var userID in shareModalViewModel.ApplicationUserIDs)
                    {
                        var sharedRequest = _context.ShareRequests.Where(sr => sr.RequestID == shareModalViewModel.ID)
                                               .Where(sr => sr.FromApplicationUserID == _userManager.GetUserId(User))
                                               .Where(sr => sr.ToApplicationUserID == userID).FirstOrDefault();
                        if (sharedRequest == null)
                        {
                            sharedRequest = new ShareRequest()
                            {
                                RequestID = shareModalViewModel.ID,
                                FromApplicationUserID = _userManager.GetUserId(User),
                                ToApplicationUserID = userID,
                                //TimeStamp = DateTime.Now
                            };
                        }
                        else
                        {
                            //sharedRequest.TimeStamp = DateTime.Now;
                        }
                        _context.Update(sharedRequest);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    error = true;
                }
            }
            return error;
        }

        [Authorize(Roles = "Requests")]
        public async Task<bool> RemoveShare(int ID, AppUtility.ModelsEnum ModelsEnum = AppUtility.ModelsEnum.Request)
        {
            bool error = false;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    switch (ModelsEnum)
                    {
                        case AppUtility.ModelsEnum.Request:
                            var shareRequests = _context.ShareRequests.Where(sr => sr.RequestID == ID && sr.ToApplicationUserID == _userManager.GetUserId(User));
                            foreach (var sr in shareRequests)
                            {
                                _context.Remove(sr);
                            }
                            break;
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    error = true;
                    transaction.Rollback();
                }
            };
            return error;
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ItemData(int? id, int? Tab = 0, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, bool isEditable = true)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            List<string> selectedPriceSort = null;
            selectedPriceSort = new List<string>() { AppUtility.PriceSortEnum.Unit.ToString(), AppUtility.PriceSortEnum.TotalVat.ToString() };
            var requestItemViewModel = await editModalViewFunction(id, Tab, SectionType, isEditable, selectedPriceSort);
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
            requestItemViewModel.Vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == categoryTypeId).Count() > 0).ToListAsync();
            requestItemViewModel.SectionType = SectionType;
            var request = _context.Requests.Include(r => r.Product).Include(r => r.Product.Vendor).SingleOrDefault(x => x.RequestID == id);
            requestItemViewModel.Requests = new List<Request>() { request };
            return PartialView(requestItemViewModel);
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> EditModalView(int? id, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, bool isEditable = true, List<string> selectedPriceSort = null, bool isProprietary = false, int? Tab = 0)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            selectedPriceSort = selectedPriceSort.Count == 0 ? new List<string>() { AppUtility.PriceSortEnum.Unit.ToString(), AppUtility.PriceSortEnum.TotalVat.ToString() } : selectedPriceSort;
            var requestItemViewModel = await editModalViewFunction(id, Tab, SectionType, isEditable, selectedPriceSort, isProprietary: isProprietary);
            return PartialView(requestItemViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> EditModalView(RequestItemViewModel requestItemViewModel, ReceivedModalVisualViewModel receivedModalVisualViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    TempData.Keep();
                    var request = requestItemViewModel.Requests.FirstOrDefault();
                    //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
                    request.ParentRequest = null;
                    //requestItemViewModel.Request.ParentQuote.ParentQuoteID = (Int32)requestItemViewModel.Request.ParentQuoteID;
                    var parentQuote = _context.ParentQuotes.Where(pq => pq.ParentQuoteID == request.ParentQuoteID).FirstOrDefault();
                    if (parentQuote != null && request.ParentQuote != null)
                    {

                        parentQuote.QuoteNumber = request.ParentQuote.QuoteNumber;
                        parentQuote.QuoteDate = request.ParentQuote.QuoteDate;
                        request.ParentQuote = parentQuote;
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

                    var product = _context.Products.IgnoreQueryFilters().Include(p => p.Vendor).Include(p => p.ProductSubcategory).FirstOrDefault(v => v.ProductID == request.ProductID);
                    // product.ProductSubcategoryID = requestItemViewModel.Request.Product.ProductSubcategoryID;
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
                        //request.Product.ProductSubcategoryID = request.Product.ProductSubcategory.ProductSubcategoryID;
                        // requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;
                        request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == request.SubProjectID).FirstOrDefault();

                        //_context.Update(requestItemViewModel.Request.Product.SubProject);
                        //_context.Update(requestItemViewModel.Request.Product);
                        /*if (request.ParentQuote != null)
                        {
                            _context.Update(request.ParentQuote);
                            await _context.SaveChangesAsync();
                            request.ParentQuoteID = request.ParentQuote.ParentQuoteID;
                        }*/
                        //_context.Update(request);
                        _context.Entry(request).State = EntityState.Modified;
                        _context.Entry(request.Product).State = EntityState.Modified;
                        if (request.Payments?[0].InvoiceID != null)
                        {
                            _context.Entry(request.Payments[0].Invoice).State = EntityState.Modified; //todo: make a loop
                        }
                        //var entries = _context.ChangeTracker.Entries();
                        await _context.SaveChangesAsync();


                        if (requestItemViewModel.Comments != null)
                        {

                            foreach (var comment in requestItemViewModel.Comments)
                            {
                                if (!String.IsNullOrEmpty(comment.CommentText))
                                {
                                    //save the new comment
                                    comment.ObjectID = request.RequestID;
                                    if (comment.CommentID == 0)
                                    {
                                        comment.CommentTimeStamp = DateTime.Now;
                                        _context.Entry(comment).State = EntityState.Added;
                                    }
                                    else
                                    {
                                        _context.Entry(comment).State = EntityState.Modified;
                                    }
                                    //_context.Update(comment);
                                }
                            }
                            await _context.SaveChangesAsync();
                        }

                        if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                        {
                            var requestLocations = _context.Requests.Where(r => r.RequestID == request.RequestID).Include(r => r.RequestLocationInstances).FirstOrDefault().RequestLocationInstances;
                            foreach (var location in requestLocations)
                            {
                                var locationInstance = _context.LocationInstances.IgnoreQueryFilters().Where(li => li.LocationInstanceID == location.LocationInstanceID).FirstOrDefault();
                                _context.Remove(location);
                                if (locationInstance.LocationTypeID == 103 || locationInstance.LocationTypeID == 205)
                                {
                                    locationInstance.IsFull = false;
                                    _context.Update(locationInstance);
                                }
                                else if (locationInstance.IsEmptyShelf)
                                {
                                    var duplicateLocations = _context.RequestLocationInstances.Where(rli => rli.LocationInstanceID == locationInstance.LocationInstanceID
                                                            && rli.RequestID != request.RequestID).ToList();
                                    if (duplicateLocations.Count() == 0)
                                    {
                                        locationInstance.ContainsItems = false;
                                        _context.Update(locationInstance);
                                    }
                                }
                            }
                            await _context.SaveChangesAsync();
                            await SaveLocations(receivedModalVisualViewModel, request, false);
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
                    else
                    {

                        foreach (var result in results)
                        {
                            requestItemViewModel.ErrorMessage += result.ErrorMessage;
                        }
                        throw new ModelStateInvalidException(requestItemViewModel.ErrorMessage);
                    }
                    //return RedirectToAction("Index");
                    AppUtility.PageTypeEnum requestPageTypeEnum = (AppUtility.PageTypeEnum)requestItemViewModel.PageType;
                    //throw new Exception();
                    await transaction.CommitAsync();
                    requestItemViewModel.Requests[0] = request;
                    return RedirectToAction("Index", new
                    {
                        requestStatusID = requestItemViewModel.RequestStatusID,
                        PageType = requestPageTypeEnum
                    });
                }
                catch (Exception ex)
                {
                    requestItemViewModel.Requests[0] = _context.Requests.Include(r => r.Product)
                    .Include(r => r.ParentQuote)
                    .Include(r => r.ParentRequest)
                    .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.ProductSubcategory.ParentCategory)
                         .Include(r => r.Product.Vendor)
                    .Include(r => r.RequestStatus)
                    .Include(r => r.ApplicationUserCreator)
                    //.Include(r => r.Payments) //do we have to have a separate list of payments to include thefix c inside things (like company account and payment types?)
                    .SingleOrDefault(x => x.RequestID == requestItemViewModel.Requests[0].RequestID);
                    requestItemViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                    await transaction.RollbackAsync();
                    var categoryTypeId = requestItemViewModel.SectionType == AppUtility.MenuItems.Requests ? 1 : 2;
                    var productSubcategory = requestItemViewModel.Requests[0].Product.ProductSubcategory;
                    requestItemViewModel = await FillRequestDropdowns(requestItemViewModel, productSubcategory, categoryTypeId);
                    string requestId = requestItemViewModel.Requests[0].RequestID.ToString();
                    string parentQuoteId = requestItemViewModel.Requests[0].ParentQuoteID.ToString();
                    FillDocumentsInfo(requestItemViewModel, productSubcategory, requestId, parentQuoteId);
                    requestItemViewModel.Comments = await _context.RequestComments.Include(r => r.ApplicationUser).Include(r => r.CommentType).Where(r => r.Request.RequestID == requestItemViewModel.Requests[0].RequestID).ToListAsync();
                    requestItemViewModel.ModalType = AppUtility.RequestModalType.Edit;
                    Response.StatusCode = 50;
                    return PartialView(requestItemViewModel);
                }
            }
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(RequestIndexObject requestIndexObject, int? id, String SectionType = "")
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            /*Object lockObj = new Object();
            lock (lockObj)
            {*/
            //DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests);
            //DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote);
            /*}*/
            //base.RemoveRequestWithCommentsAndEmailSessions();
            TempRequestJson tempRequestJson = CreateTempRequestJson(Guid.NewGuid(), 0);
            TempRequestListViewModel trlvm = new TempRequestListViewModel()
            {
                GUID = tempRequestJson.GuidID,
                RequestIndexObject = requestIndexObject,
                TempRequestViewModels = new List<TempRequestViewModel>()
            };


            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            Request request = _context.Requests
                .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory)
                .Include(r => r.Product.UnitType)
                .Include(r => r.Product.SubUnitType)
                .Include(r => r.Product.SubSubUnitType)
                .SingleOrDefault(x => x.RequestID == id);

            var unittypes = _context.UnitTypes.Where(ut => ut.UnitTypeParentCategory.Where(up => up.ParentCategoryID == request.Product.ProductSubcategory.ParentCategoryID).Count() > 0)
                .Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);

            trlvm.TempRequestViewModels.Add(new TempRequestViewModel() { Request = request });

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                TempRequestListViewModel = trlvm
            };
            requestItemViewModel.Requests = new List<Request>() { request };
            requestItemViewModel.IsReorder = true;
            //var reorderViewModel = new ReorderViewModel() { RequestItemViewModel = requestItemViewModel };

            await SetTempRequestAsync(tempRequestJson, trlvm, requestItemViewModel.TempRequestListViewModel.RequestIndexObject);
            return PartialView(requestItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(RequestItemViewModel requestItemViewModel, TempRequestListViewModel tempRequestListViewModel, AppUtility.OrderTypeEnum OrderTypeEnum/*, bool isCancel = false*/)
        {
            /*if (isCancel)
            {
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests, tempRequestListViewModel.GUID);
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote, tempRequestListViewModel.GUID);
                await RemoveTempRequestAsync(tempRequestListViewModel.GUID);
                return new EmptyResult();
            }*/
            try
            {
                //  ReorderViewModel reorderViewModel = JsonConvert.DeserializeObject<ReorderViewModel>(json);
                //get the old request that we are reordering
                var oldRequest = _context.Requests.Where(r => r.RequestID == requestItemViewModel.Requests.FirstOrDefault().RequestID)
                    .Include(r => r.Product)
                    .ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.Product.Vendor).AsNoTracking().FirstOrDefault();


                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
                //need to include product to check if in budget
                //   reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().Product = oldRequest.Product;
                requestItemViewModel.Requests.FirstOrDefault().RequestID = 0;
                requestItemViewModel.Requests.FirstOrDefault().ApplicationUserCreatorID = currentUser.Id;
                requestItemViewModel.Requests.FirstOrDefault().CreationDate = DateTime.Now;
                requestItemViewModel.Requests.FirstOrDefault().SubProjectID = oldRequest.SubProjectID;
                requestItemViewModel.Requests.FirstOrDefault().URL = oldRequest.URL;
                requestItemViewModel.Requests.FirstOrDefault().Warranty = oldRequest.Warranty;
                requestItemViewModel.Requests.FirstOrDefault().ExchangeRate = oldRequest.ExchangeRate;
                requestItemViewModel.Requests.FirstOrDefault().Currency = oldRequest.Currency;
                requestItemViewModel.Requests.FirstOrDefault().IncludeVAT = oldRequest.IncludeVAT;
                //var entries = _context.ChangeTracker.Entries();
                oldRequest.Product.UnitTypeID = requestItemViewModel.Requests.FirstOrDefault().Product.UnitTypeID;
                oldRequest.Product.SubUnitTypeID = requestItemViewModel.Requests.FirstOrDefault().Product.SubUnitTypeID;
                oldRequest.Product.SubSubUnitTypeID = requestItemViewModel.Requests.FirstOrDefault().Product.SubSubUnitTypeID;
                oldRequest.Product.SubUnit = requestItemViewModel.Requests.FirstOrDefault().Product.SubUnit;
                oldRequest.Product.SubSubUnit = requestItemViewModel.Requests.FirstOrDefault().Product.SubSubUnit;
                requestItemViewModel.Requests.FirstOrDefault().ProductID = oldRequest.ProductID;
                requestItemViewModel.Requests.FirstOrDefault().Product = oldRequest.Product;
                var entries = _context.ChangeTracker.Entries();
                var isInBudget = checkIfInBudget(requestItemViewModel.Requests.FirstOrDefault(), oldRequest.Product);
                var entries2 = _context.ChangeTracker.Entries();

                TempRequestViewModel newTrvm = await AddItemAccordingToOrderType(requestItemViewModel.Requests.FirstOrDefault(), OrderTypeEnum, isInBudget, tempRequestListViewModel);
                if (OrderTypeEnum != AppUtility.OrderTypeEnum.RequestPriceQuote)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            TempRequestJson trj = CreateTempRequestJson(tempRequestListViewModel.GUID, 1);
                            await SetTempRequestAsync(trj,
                            new TempRequestListViewModel() { TempRequestViewModels = new List<TempRequestViewModel>() { newTrvm } },
                            tempRequestListViewModel.RequestIndexObject);

                            await transaction.CommitAsync(); //IF SAVEITEM OR REQUEST ITEM
                            //if (!deserializedTemp.TempRequestViewModels.Any()) //DO WE NEED THIS IF???
                            //{
                            //    MoveDocumentsOutOfTempFolder(reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().RequestID, AppUtility.ParentFolderName.Requests);
                            //    await transaction.CommitAsync();
                            //    base.RemoveRequestWithCommentsAndEmailSessions();
                            //}
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            await RemoveTempRequestAsync(tempRequestListViewModel.GUID);
                            throw new Exception(AppUtility.GetExceptionMessage(ex)); ;
                        }
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
                //should this be added?
                /*//uncurrent the one we're on
                await KeepTempRequestJsonCurrentAsOriginal(tempRequestListViewModel.GUID);*/


                var action = "UploadQuoteModal"; //for order now and add to cart
                if (OrderTypeEnum == AppUtility.OrderTypeEnum.AlreadyPurchased)
                {
                    action = "UploadOrderModal";
                }
                else if (OrderTypeEnum == AppUtility.OrderTypeEnum.RequestPriceQuote)
                {
                    return new EmptyResult();
                }
                tempRequestListViewModel.RequestIndexObject.OrderType = OrderTypeEnum;
                tempRequestListViewModel.RequestIndexObject.IsReorder = true;
                tempRequestListViewModel.RequestIndexObject.GUID = tempRequestListViewModel.GUID;
                //throw new Exception();
                return RedirectToAction(action, "Requests", tempRequestListViewModel.RequestIndexObject);
            }
            catch (Exception ex)
            {
                await RemoveTempRequestAsync(tempRequestListViewModel.GUID);
                requestItemViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                /*var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
                requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");*/
                return PartialView("_ErrorMessage", requestItemViewModel.ErrorMessage);
            }
        }

        private async Task RemoveOldRequestFromInventory(int requestId)
        {

            var productID = _context.Requests.Where(r => r.RequestID == requestId).Select(r => r.ProductID).FirstOrDefault();
            var oldRequest = _context.Requests.Where(r => r.ProductID == productID && r.RequestID != requestId).Where(r => r.IsInInventory).FirstOrDefault();
            if (oldRequest != null)
            {
                oldRequest.IsInInventory = false;
                _context.Update(oldRequest);
                await _context.SaveChangesAsync();
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
            //var allRequests = new List<Request>();
            //var isRequests = true;
            //var RequestNum = 1;
            long lastParentRequestOrderNum = 0;
            //var prs = _context.ParentRequests;
            if (_context.ParentRequests.Any())
            {
                lastParentRequestOrderNum = _context.ParentRequests.IgnoreQueryFilters().OrderByDescending(x => x.OrderNumber).Select(pr => pr.OrderNumber).FirstOrDefault() ?? 0;
            }
            ParentRequest pr = new ParentRequest()
            {
                ApplicationUserID = _userManager.GetUserId(User),
                OrderNumber = lastParentRequestOrderNum + 1,
                OrderDate = DateTime.Now
            };

            TempRequestListViewModel newTRLVM = new TempRequestListViewModel() { RequestIndexObject = requestIndexObject };
            TempRequestJson updatedTempRequestJson = new TempRequestJson();
            var allRequests = new List<Request>();
            if (id != 0) //already has terms, being sent from approve order button -- not in a temprequestjson
            {
                var request = _context.Requests.Where(r => r.RequestID == id).AsNoTracking().FirstOrDefault();
                var parentRequest = _context.ParentRequests.Where(pr => pr.ParentRequestID == request.ParentRequestID).AsNoTracking().FirstOrDefault();
                if (parentRequest != null)
                {
                    pr = parentRequest;
                    pr.OrderNumber = lastParentRequestOrderNum + 1;
                    pr.OrderDate = DateTime.Now;
                }
                request.ParentRequest = pr;
                if (request.Product == null)
                {
                    request.Product = _context.Products.Where(p => p.ProductID == request.ProductID).Include(p => p.Vendor)
                      .Include(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).FirstOrDefault();
                }
                else
                {
                    request.Product.ProductSubcategory.ParentCategory = _context.ParentCategories.Where(pc => pc.ParentCategoryID == request.Product.ProductSubcategory.ParentCategoryID).FirstOrDefault();
                    request.Product.Vendor = _context.Vendors.Where(v => v.VendorID == request.Product.VendorID).FirstOrDefault();
                }
                newTRLVM.TempRequestViewModels = new List<TempRequestViewModel>()
                {
                    new TempRequestViewModel(){
                        Request = request
                    }
                };
                newTRLVM.TempRequestViewModels.ForEach(t => t.Request.ParentRequest = pr);
                updatedTempRequestJson = CreateTempRequestJson(tempRequestListViewModel.GUID, 1);
                await SetTempRequestAsync(updatedTempRequestJson, newTRLVM, tempRequestListViewModel.RequestIndexObject);
                newTRLVM.GUID = updatedTempRequestJson.GuidID;
                newTRLVM.SequencePosition = updatedTempRequestJson.SequencePosition;
                var payments = _context.Payments.Where(p => p.RequestID == id);

                allRequests.Add(request);
            }
            else
            {
                var oldTempRequestJson = await GetTempRequestAsync(tempRequestListViewModel.GUID);
                //var newTempRequestJson = await CopyToNewCurrentTempRequestAsync(oldTempRequestJson);

                newTRLVM.TempRequestViewModels = oldTempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels;
                newTRLVM.GUID = tempRequestListViewModel.GUID;
                newTRLVM.SequencePosition = tempRequestListViewModel.SequencePosition;

                foreach (var tempRequest in newTRLVM.TempRequestViewModels)
                {
                    tempRequest.Request.PaymentStatus = _context.PaymentStatuses.Where(ps => ps.PaymentStatusID == tempRequest.Request.PaymentStatusID).FirstOrDefault();
                    if (tempRequest.Request.ParentRequest != null)
                    {
                        pr.Shipping = tempRequest.Request.ParentRequest.Shipping;
                        pr.NoteToSupplier = tempRequest.Request.ParentRequest.NoteToSupplier;
                    }
                    tempRequest.Request.ParentRequest = pr;
                    if (tempRequest.Request.Product == null)
                    {
                        tempRequest.Request.Product = _context.Products.Where(p => p.ProductID == tempRequest.Request.ProductID).Include(p => p.Vendor)
                          .Include(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).FirstOrDefault();
                    }
                    else
                    {
                        tempRequest.Request.Product.ProductSubcategory = _context.ProductSubcategories.Where(ps => ps.ProductSubcategoryID == tempRequest.Request.Product.ProductSubcategoryID).Include(ps => ps.ParentCategory).FirstOrDefault();
                        tempRequest.Request.Product.Vendor = _context.Vendors.Where(v => v.VendorID == tempRequest.Request.Product.VendorID).FirstOrDefault();
                    }
                    allRequests.Add(tempRequest.Request);
                }

                //updatedTempRequestJson = await CopyToNewCurrentTempRequestAsync(oldTempRequestJson, 3);
                newTRLVM.TempRequestViewModels.ForEach(t => t.Request.ParentRequest = pr);
                await SetTempRequestAsync(oldTempRequestJson, newTRLVM, tempRequestListViewModel.RequestIndexObject);
            }

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
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmEmailModal(ConfirmEmailViewModel confirmEmailViewModel, TempRequestListViewModel tempRequestListViewModel)
        {
            try
            {
                //var isRequests = true;
                //var RequestNum = 1;
                //var PaymentNum = 1;
                //var requests = new List<Request>();
                //var payments = new List<Payment>();


                var oldTempRequestJson = await GetTempRequestAsync(tempRequestListViewModel.GUID);
                var newTempRequestJson = await CopyToNewCurrentTempRequestAsync(oldTempRequestJson, 4);

                var deserializedTempRequestListViewModel = new TempRequestListViewModel()
                {
                    TempRequestViewModels =
                    newTempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels
                };
                //var pr = tempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest; //eventually(when ready to test all cases) put this in instead of next line and put it in for loop below
                deserializedTempRequestListViewModel.TempRequestViewModels.ForEach(t => t.Request.ParentRequest = tempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest);


                //var isEmail = true;
                //var emailNum = 1;
                //var emails = new List<string>();
                //while (isEmail)
                //{
                //    var emailName = AppData.SessionExtensions.SessionNames.Email.ToString() + emailNum;
                //    var email = _httpContextAccessor.HttpContext.Session.GetObject<string>(emailName);
                //    if (email != null)
                //    {
                //        emails.Add(email);
                //    }
                //    else
                //    {
                //        isEmail = false;
                //    }
                //    emailNum++;
                //}

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

                /*string uploadFolder = Path.Combine("wwwroot", AppUtility.ParentFolderName.Requests.ToString());
                string uploadFile = Path.Combine(uploadFolder, "CentarixOrder" + requests.FirstOrDefault().ParentRequest.OrderNumber + ".pdf");
                */


                //instatiate mimemessage
                var message = new MimeMessage();

                //instantiate the body builder
                var builder = new BodyBuilder();

                var userId = deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.ApplicationUserCreatorID ?? _userManager.GetUserId(User); //do we need to do this? (will it ever be null?)
                                                                                                                                                                           //var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
                var currentUser = _context.Users.FirstOrDefault(u => u.Id == userId);
                //var users = _context.Users.ToList();
                //currentUser = _context.Users.Where(u => u.Id == "702fe06c-22e1-4be8-a515-ea89d6e5ee00").FirstOrDefault();
                string ownerEmail = currentUser.Email;
                string ownerUsername = currentUser.FirstName + " " + currentUser.LastName;
                string ownerPassword = currentUser.SecureAppPass;
                deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Product.Vendor = _context.Vendors.Where(v => v.VendorID == deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Product.VendorID).FirstOrDefault();
                string vendorEmail = deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Product.Vendor.OrdersEmail;
                //string vendorEmail = /*firstRequest.Product.Vendor.OrdersEmail;*/ emails.Count() < 1 ? requests.FirstOrDefault().Product.Vendor.OrdersEmail : emails[0];
                string vendorName = deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.Product.Vendor.VendorEnName;

                //add a "From" Email
                message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

                // add a "To" Email
                message.To.Add(new MailboxAddress(vendorName, vendorEmail));

                //add CC's to email
                //TEST THIS STATEMENT IF VENDOR IS MISSING AN ORDERS EMAIL
                if (deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Emails != null)
                {
                    for (int e = 0; e < deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Emails.Count(); e++)
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
                    quoteNumbers = deserializedTempRequestListViewModel.TempRequestViewModels.Select(trvm => trvm.Request.ParentQuote.QuoteNumber).ToList();
                }
                else
                {
                    var parentQuoteIDs = deserializedTempRequestListViewModel.TempRequestViewModels.Select(trvm => trvm.Request.ParentQuoteID);
                    quoteNumbers = _context.ParentQuotes.Where(pq => parentQuoteIDs.Contains(pq.ParentQuoteID)).Select(pq => pq.QuoteNumber).ToList();
                }

                //body
                builder.TextBody = @"Hello," + "\n\n" + "Please see the attached order for quote number(s) " + string.Join(", ", quoteNumbers) +
                    ". \n\nPlease confirm that you received the order. \n\nThank you.\n"
                    + ownerUsername + "\nCentarix";
                builder.Attachments.Add(filePath);

                message.Body = builder.ToMessageBody();

                bool wasSent = false;

                using (var client = new SmtpClient())
                {

                    client.Connect("smtp.gmail.com", 587, false);
                    //var SecureAppPass = _context.Users.Where(u => u.Id == confirmEmail.ParentRequest.ApplicationUserID).FirstOrDefault().SecureAppPass;
                    client.Authenticate(ownerEmail, ownerPassword);// ownerPassword);//
                    client.Timeout = 500000; // 500 seconds

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
                                ViewBag.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                                throw new Exception(AppUtility.GetExceptionMessage(ex));
                            }
                            client.Disconnect(true);

                            if (wasSent)
                            {
                                //foreach (var tempRequest in deserializedTempRequestListViewModel.TempRequestViewModels)
                                for (int tr = 0; tr < deserializedTempRequestListViewModel.TempRequestViewModels.Count(); tr++)
                                {
                                    var tempRequest = deserializedTempRequestListViewModel.TempRequestViewModels[tr];
                                    tempRequest.Request.RequestStatusID = 2;
                                    if (tempRequest.Request.RequestID == 0)
                                    {
                                        if (tempRequest.Request.Product.ProductID == 0)
                                        {
                                            tempRequest.Request.Product.SerialNumber = GetSerialNumber(false);
                                            _context.Entry(tempRequest.Request.Product).State = EntityState.Added;
                                        }
                                        else
                                        {
                                            _context.Entry(tempRequest.Request.Product).State = EntityState.Modified;
                                        }
                                        //tempRequest.Request.ParentRequest = pr;
                                        _context.Entry(tempRequest.Request).State = EntityState.Added;
                                        //tempRequest.Request.ParentRequest.OrderDate = DateTime.Now;
                                        if (tempRequest.Request.ParentQuote.ParentQuoteID == 0)
                                        {
                                            _context.Entry(tempRequest.Request.ParentQuote).State = EntityState.Added;
                                        }
                                        else
                                        {
                                            _context.Entry(tempRequest.Request.ParentQuote).State = EntityState.Modified;

                                        }
                                    }
                                    else
                                    {
                                        _context.Entry(tempRequest.Request).State = EntityState.Modified;
                                    }
                                    await _context.SaveChangesAsync();
                                    //if there are no payments it means that the payments were saved previously
                                    if (tempRequest.Payments == null)
                                    {
                                        //tempRequest.Payments = new List<Payment>();
                                        //foreach(var payment in _context.Payments.Where(p => p.RequestID == tempRequest.Request.RequestID))
                                        //{
                                        //    tempRequest.Payments.Add(payment);
                                        //}
                                    }
                                    else
                                    {
                                        foreach (var p in tempRequest.Payments)
                                        {
                                            //DO WE NEED THIS NEXT LINE HERE???
                                            p.RequestID = tempRequest.Request.RequestID;
                                            _context.Entry(p).State = EntityState.Added;
                                        }
                                    }
                                    await _context.SaveChangesAsync();
                                    if (tempRequest.Comments != null)
                                    {
                                        foreach (var c in tempRequest.Comments)
                                        {
                                            //DO WE NEED THIS NEXT LINE HERE???
                                            c.ObjectID = tempRequest.Request.RequestID;
                                            _context.Add(c);
                                        }
                                        await _context.SaveChangesAsync();
                                    }

                                    if (tempRequest.Request.OrderType == AppUtility.OrderTypeEnum.OrderNow.ToString())
                                    {
                                        var additionalRequests = tr + 1 < deserializedTempRequestListViewModel.TempRequestViewModels.Count() ? true : false;
                                        MoveDocumentsOutOfTempFolder(tempRequest.Request.RequestID, AppUtility.ParentFolderName.Requests, additionalRequests, tempRequestListViewModel.GUID);
                                    }

                                    /*string NewFolder = Path.Combine(uploadFolder, tempRequest.Request.ParentRequestID.ToString());
                                    string folderPath = Path.Combine(NewFolder, AppUtility.FolderNamesEnum.Orders.ToString());
                                    Directory.CreateDirectory(folderPath); //make sure we don't need one above also??

                                    string uniqueFileName = 1 + "OrderEmail.pdf";
                                    string filePath = Path.Combine(folderPath, uniqueFileName);
                                    if (System.IO.File.Exists(filePath))
                                    {
                                        System.IO.File.Delete(filePath);
                                    }

                                    System.IO.File.Copy(uploadFile, filePath); //make sure this works for each of them*/

                                    tempRequest.Request.Product = await _context.Products.Where(p => p.ProductID == tempRequest.Request.ProductID).Include(p => p.Vendor).FirstOrDefaultAsync();
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
                                    requestNotification.OrderDate = DateTime.Now;
                                    requestNotification.Vendor = tempRequest.Request.Product.Vendor.VendorEnName;
                                    _context.Add(requestNotification);
                                    await _context.SaveChangesAsync();
                                }
                                if (deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.OrderType == AppUtility.OrderTypeEnum.OrderNow.ToString())
                                {
                                    MoveDocumentsOutOfTempFolder(deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentQuoteID == null ? 0 : Convert.ToInt32(deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentQuoteID), AppUtility.ParentFolderName.ParentQuote, false, tempRequestListViewModel.GUID);
                                }
                                bool moveOrderDoc = false;
                                if (deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest.ParentRequestID == 0)
                                {
                                    _context.Entry(deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest).State = EntityState.Added;
                                    moveOrderDoc = true;
                                }
                                else //if coming from approve order
                                {
                                    _context.Entry(deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest).State = EntityState.Modified;
                                }
                                await _context.SaveChangesAsync();
                                if (moveOrderDoc)
                                {
                                    MoveDocumentsOutOfTempFolder(deserializedTempRequestListViewModel.TempRequestViewModels[0].Request.ParentRequest.ParentRequestID,
                                         AppUtility.ParentFolderName.ParentRequest, false, tempRequestListViewModel.GUID);     //make sure has an id here.
                                }
                                //throw new Exception();
                                await transaction.CommitAsync();
                                //base.RemoveRequestWithCommentsAndEmailSessions();
                                await RemoveTempRequestAsync(tempRequestListViewModel.GUID);
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            //base.RemoveRequestWithCommentsAndEmailSessions();
                            await RollbackCurrentTempAsync(tempRequestListViewModel.GUID);
                            throw new Exception(AppUtility.GetExceptionMessage(ex));
                        }

                    }

                }
                tempRequestListViewModel.RequestIndexObject.RequestStatusID = 2;
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                await RollbackCurrentTempAsync(tempRequestListViewModel.GUID);
                tempRequestListViewModel.RequestIndexObject.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                await Response.WriteAsync(tempRequestListViewModel.RequestIndexObject.ErrorMessage);
                return new EmptyResult();
            }

        }



        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmQuoteEmailModal(ConfirmEmailViewModel confirmQuoteEmail, RequestIndexObject requestIndexObject)
        {
            try
            {
                List<Request> requests;
                if (confirmQuoteEmail.IsResend)
                {
                    requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.RequestID == confirmQuoteEmail.RequestID)
                        .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                        .Include(r => r.Product.Vendor).Include(r => r.ParentQuote).ToList();
                }
                else if (confirmQuoteEmail.VendorId != null)
                {
                    requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString())
                        .Where(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.QuoteStatusID == 1).Where(r => r.RequestStatusID == 6)
                         .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.Product.Vendor)
                         .Include(r => r.ParentQuote).ToList();
                }
                else
                {
                    requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString())
                        .Where(r => confirmQuoteEmail.Requests.Select(rid => rid.RequestID).Contains(r.RequestID) && (r.QuoteStatusID == 1 || r.QuoteStatusID == 2)).Where(r => r.RequestStatusID == 6)
                         .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.Product.Vendor)
                         .Include(r => r.ParentQuote).ToList();
                }
                if (requests.Count() == 0)
                {
                    requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString())
                        .Where(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.QuoteStatusID == 2).Where(r => r.RequestStatusID == 6)
                         .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                         .Include(r => r.Product.Vendor).Include(r => r.ParentQuote).ToList();
                }
                //base url needs to be declared - perhaps should be getting from js?
                //once deployed need to take base url and put in the parameter for converter.convertHtmlString
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value.ToString()}";

                confirmQuoteEmail.Requests = requests;
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

                    var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
                    //   currentUser = _context.Users.Where(u => u.Id == "702fe06c-22e1-4be8-a515-ea89d6e5ee00").FirstOrDefault();
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
                            throw new Exception(AppUtility.GetExceptionMessage(ex));
                        }

                        client.Disconnect(true);
                        if (wasSent)
                        {
                            foreach (var quote in requests)
                            {
                                quote.QuoteStatusID = 2;
                                //quote.ParentQuote.ApplicationUserID = currentUser.Id;
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
            List<Request> requests = new List<Request>();
            if (isResend)
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.RequestID == id)
               .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
               .ToList();
            }
            else
            {
                if (id != null)
                {
                    requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.Product.VendorID == id && r.QuoteStatusID == 1)
                       .Where(r => r.RequestStatusID == 6).Include(r => r.Product).ThenInclude(p => p.Vendor)
                       .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.ParentQuote).ToList();
                }
                else if (requestIds != null)
                {
                    foreach (var Rid in requestIds)
                    {
                        var request = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.RequestID == Rid && (r.QuoteStatusID == 1 || r.QuoteStatusID == 2))
                           .Where(r => r.RequestStatusID == 6).Include(r => r.Product).ThenInclude(p => p.Vendor)
                           .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.ParentQuote).FirstOrDefault();
                        requests.Add(request);
                    }
                }

            }
            if (requests.Count() == 0)
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.Product.VendorID == id && r.QuoteStatusID == 2)
                    .Where(r => r.RequestStatusID == 6).Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentQuote)
                    .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.ParentQuote).ToList();
            }
            RequestIndexObject requestIndexObject = new RequestIndexObject
            {
                PageType = AppUtility.PageTypeEnum.LabManagementQuotes,
                SidebarType = AppUtility.SidebarEnum.Quotes
            };
            ConfirmEmailViewModel confirmEmail = new ConfirmEmailViewModel
            {
                Requests = requests,
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

            requestsSearchViewModel.ParentCategories = await _context.ParentCategories.Where(pc => categoryID.Contains(pc.CategoryTypeID)).ToListAsync();
            requestsSearchViewModel.ProductSubcategories = await _context.ProductSubcategories.Where(ps => categoryID.Contains(ps.ParentCategory.CategoryTypeID)).ToListAsync();
            requestsSearchViewModel.Vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => categoryID.Contains(vc.CategoryTypeID)).Count() > 0).ToListAsync();
            requestsSearchViewModel.ApplicationUsers = await _context.Users.ToListAsync();
            requestsSearchViewModel.SectionType = SectionType;

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
            //foreach(var li in _context.LocationInstances)
            //{
            //    li.IsFull = false;
            //    _context.Update(li);
            //}
            //_context.SaveChanges();
            var request = _context.Requests.Where(r => r.RequestID == RequestID).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.Product.UnitType)
                    .FirstOrDefault();

            ReceivedLocationViewModel receivedLocationViewModel = new ReceivedLocationViewModel()
            {
                Request = request,
                locationTypesDepthZero = _context.LocationTypes.Where(lt => lt.Depth == 0),
                locationInstancesSelected = new List<LocationInstance>(),
                //ApplicationUsers = await _context.Users.Where(u => !u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null).ToListAsync(),
                RequestIndexObject = requestIndexObject,
                PageRequestStatusID = request.RequestStatusID
            };
            receivedLocationViewModel.locationInstancesSelected.Add(new LocationInstance());
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            receivedLocationViewModel.Request.ApplicationUserReceiverID = currentUser.Id;
            receivedLocationViewModel.Request.ApplicationUserReceiver = currentUser;
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
                locationInstancesDepthZero = _context.LocationInstances.Where(li => li.LocationTypeID == LocationTypeID && !(li is TemporaryLocationInstance))
                .Include(li => li.LocationRoomInstance).Include(li => li.LabPart).OrderBy(li => li.LocationNumber),
                locationTypeNames = new List<string>(),
                locationInstancesSelected = new List<LocationInstance>(),
            };
            bool finished = false;
            int locationTypeIDLoop = LocationTypeID;
            if (LocationTypeID == 500)
            {
                receivedModalSublocationsViewModel.LabPartTypes = _context.LabParts;
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
        public IActionResult ReceivedModalVisual(int LocationInstanceID, int RequestID, bool ShowIcons = false)
        {
            ReceivedModalVisualViewModel receivedModalVisualViewModel = new ReceivedModalVisualViewModel()
            {
                IsEditModalTable = false,
                ShowIcons = ShowIcons
            };

            var parentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == LocationInstanceID).FirstOrDefault();

            var firstChildLI = _context.LocationInstances.Where(li => li.LocationInstanceParentID == parentLocationInstance.LocationInstanceID).FirstOrDefault();
            LocationInstance secondChildLi = null;
            if (firstChildLI != null)
            {
                secondChildLi = _context.LocationInstances.Where(li => li.LocationInstanceParentID == firstChildLI.LocationInstanceID).FirstOrDefault(); //second child is to ensure it doesn't have any box units
            }

            if (secondChildLi != null)
            {
                receivedModalVisualViewModel.DeleteTable = true;
            }
            else
            {
                //if it's an empty shelf- reset the location to the parent location instance id:
                if (/*parentLocationInstance.LocationTypeID == 201 &&*/ parentLocationInstance.IsEmptyShelf)
                {
                    parentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == parentLocationInstance.LocationInstanceParentID).FirstOrDefault();
                    LocationInstanceID = parentLocationInstance.LocationInstanceID;
                }

                receivedModalVisualViewModel.ParentLocationInstance = parentLocationInstance;

                if (receivedModalVisualViewModel.ParentLocationInstance != null)
                {
                    var request = _context.Requests.Where(r => r.RequestID == RequestID).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).FirstOrDefault();



                    receivedModalVisualViewModel.ChildrenLocationInstances =
                        _context.LocationInstances.Where(m => m.LocationInstanceParentID == LocationInstanceID)
                        .Include(m => m.RequestLocationInstances).OrderBy(m => m.LocationNumber).ToList();

                    List<LocationInstancePlace> liPlaces = new List<LocationInstancePlace>();
                    if (request != null)
                    {
                        var requestLocationInstances = request.RequestLocationInstances.ToList();
                        receivedModalVisualViewModel.RequestChildrenLocationInstances =
                                   _context.LocationInstances.OfType<LocationInstance>().Where(m => m.LocationInstanceParentID == parentLocationInstance.LocationInstanceID)
                                   .Include(m => m.RequestLocationInstances)
                                   .Select(li => new RequestChildrenLocationInstances()
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var requestReceived = _context.Requests.Where(r => r.RequestID == receivedLocationViewModel.Request.RequestID)
             .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).AsNoTracking().FirstOrDefault();
                    decimal pricePerUnit = 0;
                    if (receivedLocationViewModel.IsPartial)
                    {
                        requestReceived.RequestID = 0;
                        pricePerUnit = requestReceived.PricePerUnit;
                        requestReceived.Unit = (uint)(requestReceived.Unit - receivedLocationViewModel.AmountArrived);
                        requestReceived.Cost = pricePerUnit * requestReceived.Unit;
                        requestReceived.IsPartial = true;
                        _context.Entry(requestReceived).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        MoveDocumentsOutOfTempFolder(requestReceived.RequestID, AppUtility.ParentFolderName.Requests, receivedLocationViewModel.Request.RequestID, true);

                        await CopyCommentsAsync(receivedLocationViewModel.Request.RequestID, requestReceived.RequestID);
                        await CopyPaymentsAsync(receivedLocationViewModel.Request.RequestID, requestReceived.RequestID);

                        requestReceived = _context.Requests.Where(r => r.RequestID == receivedLocationViewModel.Request.RequestID)
.Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).AsNoTracking().FirstOrDefault();
                        pricePerUnit = requestReceived.PricePerUnit;
                        requestReceived.Unit = (uint)receivedLocationViewModel.AmountArrived;
                        requestReceived.Cost = pricePerUnit * requestReceived.Unit;
                    }
                    if (receivedLocationViewModel.CategoryType == 1)
                    {
                        if (receivedLocationViewModel.TemporaryLocation)
                        {
                            var tempLocationInstance = _context.TemporaryLocationInstances.Where(tli => tli.LocationTypeID == receivedLocationViewModel.LocationTypeID).FirstOrDefault();
                            if (tempLocationInstance == null)
                            {
                                var locationTypeName = _context.LocationTypes.Where(lt => lt.LocationTypeID == receivedLocationViewModel.LocationTypeID).Select(lt => lt.LocationTypeName).FirstOrDefault();
                                tempLocationInstance = new TemporaryLocationInstance()
                                {
                                    LocationTypeID = receivedLocationViewModel.LocationTypeID,
                                    LocationInstanceName = "Temporary " + locationTypeName,
                                    LocationInstanceAbbrev = "Temporary " + locationTypeName
                                };
                                _context.Update(tempLocationInstance);
                                await _context.SaveChangesAsync();
                            }
                            var rli = new RequestLocationInstance()
                            {
                                LocationInstanceID = tempLocationInstance.LocationInstanceID,
                                RequestID = requestReceived.RequestID,
                            };
                            _context.Add(rli);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            await SaveLocations(receivedModalVisualViewModel, requestReceived, false);
                        }
                    }

                    requestReceived.RequestStatusID = 3;
                    requestReceived.IsPartial = false;
                    if (receivedLocationViewModel.Request.ArrivalDate == DateTime.Today)
                    {
                        requestReceived.ArrivalDate = DateTime.Now;
                    }
                    else
                    {
                        requestReceived.ArrivalDate = receivedLocationViewModel.Request.ArrivalDate;
                    }
                    requestReceived.ApplicationUserReceiverID = receivedLocationViewModel.Request.ApplicationUserReceiverID;
                    requestReceived.ApplicationUserReceiver = _context.Users.Where(u => u.Id == receivedLocationViewModel.Request.ApplicationUserReceiverID).FirstOrDefault();

                    requestReceived.NoteForClarifyDelivery = receivedLocationViewModel.Request.NoteForClarifyDelivery;
                    requestReceived.IsClarify = receivedLocationViewModel.Request.IsClarify;
                    requestReceived.IsInInventory = true;
                    if (requestReceived.Product.ProductSubcategory.ParentCategory.ParentCategoryDescriptionEnum == AppUtility.ParentCategoryEnum.ReagentsAndChemicals.ToString())
                    {
                        requestReceived.Batch = receivedLocationViewModel.Request.Batch;
                        requestReceived.BatchExpiration = receivedLocationViewModel.Request.BatchExpiration;
                    }
                    if (requestReceived.PaymentStatusID == 4)
                    {
                        requestReceived.PaymentStatusID = 3;
                    }


                    _context.Update(requestReceived);
                    await _context.SaveChangesAsync();



                    //need to do this function before save changes because if new request it should not have an id yet

                    await RemoveOldRequestFromInventory(requestReceived.RequestID);

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

                    var didntArriveNotification = _context.RequestNotifications.Where(r => r.RequestID == requestReceived.RequestID && r.NotificationStatusID == 1).FirstOrDefault();
                    if (didntArriveNotification != null)
                    {
                        _context.Remove(didntArriveNotification);
                    }
                    //throw new Exception();
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    receivedLocationViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    Response.StatusCode = 500;
                    /*receivedLocationViewModel.locationTypesDepthZero = _context.LocationTypes.Where(lt => lt.Depth == 0);
                    var userid = _userManager.GetUserId(User);
                    receivedLocationViewModel.Request.ApplicationUserReceiver = _context.Users.Where(u => u.Id == userid).FirstOrDefault();
                    receivedLocationViewModel.Request.ApplicationUserReceiverID = userid;
                    receivedLocationViewModel.Request = _context.Requests.Where(r => r.RequestID == receivedLocationViewModel.Request.RequestID).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                    .FirstOrDefault();
                    return PartialView("ReceivedModal", receivedLocationViewModel);*/
                    return PartialView("_ErrorMessage", receivedLocationViewModel.ErrorMessage);
                }

            }

            return PartialView("_IndexTableData", await GetIndexViewModel(receivedLocationViewModel.RequestIndexObject, selectedFilters: selectedFilters, numFilters: numFilters, requestsSearchViewModel: requestsSearchViewModel));


        }



        private async Task CopyCommentsAsync(int OldRequestID, int NewRequestID)
        {
            var comments = _context.RequestComments.Where(c => c.ObjectID == OldRequestID).AsNoTracking();
            foreach (var c in comments)
            {
                c.CommentID = 0;
                c.ObjectID = NewRequestID;
                _context.Entry(c).State = EntityState.Added;
            }
            await _context.SaveChangesAsync();
        }

        private async Task CopyPaymentsAsync(int OldRequestID, int NewRequestID)
        {
            var payments = _context.Payments.Where(p => p.RequestID == OldRequestID).AsNoTracking();
            foreach (var p in payments)
            {
                p.PaymentID = 0;
                p.RequestID = NewRequestID;
                _context.Entry(p).State = EntityState.Added;
            }
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReceivedModalVisual(ReceivedModalVisualViewModel receivedModalVisualViewModel, List<Request> Requests)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                    {
                        var request = Requests.FirstOrDefault();
                        if (request.ArrivalDate == DateTime.Today) //if it's today, add seconds to be now so it shows up on top
                        {
                            request.ArrivalDate = DateTime.Now;
                        }
                        var requestLocations = _context.Requests.Where(r => r.RequestID == request.RequestID).Include(r => r.RequestLocationInstances).FirstOrDefault().RequestLocationInstances;
                        foreach (var location in requestLocations)
                        {
                            _context.Remove(location);
                            MarkLocationAvailable(request.RequestID, location.LocationInstanceID);
                        }
                        receivedModalVisualViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == receivedModalVisualViewModel.ParentLocationInstance.LocationInstanceID).FirstOrDefault();

                        await SaveLocations(receivedModalVisualViewModel, request, false);
                        await transaction.CommitAsync();
                    }

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(AppUtility.GetExceptionMessage(ex));
                }
            }
            //return PartialView(receivedModalVisualViewModel);
            return new EmptyResult();
        }

        /*
         * END RECEIVED MODAL
         */
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public ActionResult DocumentsModal(string id, Guid Guid, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
            AppUtility.ParentFolderName parentFolderName, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
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

        private void MarkLocationAvailable(int requestId, int locationInstanceID)
        {
            var locationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == locationInstanceID).FirstOrDefault();
            if (locationInstance.LocationTypeID == 103 || locationInstance.LocationTypeID == 205)
            {
                locationInstance.IsFull = false;
                _context.Update(locationInstance);
            }
            else if (locationInstance.IsEmptyShelf)
            {
                var duplicateLocations = _context.RequestLocationInstances.Where(rli => rli.LocationInstanceID == locationInstance.LocationInstanceID
                                        && rli.RequestID != requestId).ToList();
                if (duplicateLocations.Count() == 0)
                {
                    locationInstance.ContainsItems = false;
                    _context.Update(locationInstance);
                }
            }
        }


        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public void DocumentsModal(/*[FromBody]*/ DocumentsModalViewModel documentsModalViewModel)
        {
            base.DocumentsModal(documentsModalViewModel);
        }


        [HttpGet]
        public ActionResult DeleteDocumentModal(String FileString, int id, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Requests)
        {

            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            DeleteDocumentsViewModel deleteDocumentsViewModel = new DeleteDocumentsViewModel()
            {
                FileName = FileString,
                ObjectID = id,
                ParentFolderName = parentFolderName,
                FolderName = RequestFolderNameEnum,
                IsEdittable = IsEdittable,
                SectionType = SectionType,
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
            if (VendorID != null && CatalogNumber != null && (ProductID == null && _context.Requests.Where(r => r.Product.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID).Any()))
            {
                return false;
            }
            //validation for the edit
            //var product = _context.Requests.Where(r => r.Product.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID && r.ProductID != ProductID);
            if (ProductID != null && _context.Requests.Where(r => r.Product.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID && r.ProductID != ProductID).Any())
            {
                return false;
            }
            return boolCheck;
        }


        [HttpGet]
        public JsonResult GetSublocationInstancesList(int locationInstanceParentId, int labPartID)
        {
            List<LocationInstance> locationInstanceList = new List<LocationInstance>();
            if (labPartID != 0)
            {
                locationInstanceList = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceParentID == locationInstanceParentId && li.LabPartID == labPartID).Include(li => li.LabPart).OrderBy(li => li.LocationNumber).ToList();

            }
            else
            {
                locationInstanceList = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceParentID == locationInstanceParentId).Include(li => li.LabPart).OrderBy(li => li.LocationNumber).ToList();
            }
            return Json(locationInstanceList);
        }

        [HttpGet]
        [Authorize(Roles = "Requests, Operations")]
        public async Task<IActionResult> Approve(int id, RequestIndexObject requestIndex)
        {
            var request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.ParentQuote).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(px => px.ParentCategory).Include(r => r.Product.Vendor).FirstOrDefault();
            try
            {
                switch (Enum.Parse(typeof(AppUtility.OrderTypeEnum), request.OrderType))
                {
                    case AppUtility.OrderTypeEnum.OrderNow:
                        //var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                        //_httpContextAccessor.HttpContext.Session.SetObject(requestNum, request);
                        TempRequestJson r = CreateTempRequestJson(Guid.NewGuid(), 0);
                        TempRequestListViewModel trlvm = new TempRequestListViewModel()
                        {
                            TempRequestViewModels = new List<TempRequestViewModel>()
                            {
                                new TempRequestViewModel()
                                {
                                    Request = request
                                }
                            }
                        };
                        await SetTempRequestAsync(r, trlvm, requestIndex);
                        return RedirectToAction("ConfirmEmailModal", new { id = id, Guid = r.GuidID });
                        break;
                    case AppUtility.OrderTypeEnum.AlreadyPurchased:
                        break;
                    case AppUtility.OrderTypeEnum.RequestPriceQuote:
                    case AppUtility.OrderTypeEnum.AddToCart:
                        using (var transaction = await _context.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                request.RequestStatusID = 6; //approved
                                _context.Update(request);
                                await _context.SaveChangesAsync();
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
                                await _context.SaveChangesAsync();
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
        public IActionResult EditQuoteDetails(int id, int[] requestIds = null)
        {
            StringWithBool Error = new StringWithBool();
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            if (requestIds != null)
            {
                //user wants to edit only one quote, or for selected requests
                var requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => requestIds.Contains(r.RequestID))
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).ThenInclude(v => v.Country)
                    .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.ParentQuote)
                    .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType).ToList();
                var exchangeRate = GetExchangeRate();

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
            //add one quote for vendor
            //needs testing 
            //not implemented at all on the client side
            //just here for now for future implmentation
            else
            {
                var requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString())
              .Where(r => r.Product.VendorID == id && (r.QuoteStatusID == 2 || r.QuoteStatusID == 1))
              .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(p => p.Product).ThenInclude(p => p.ProductSubcategory)
              .Include(r => r.ParentQuote).Include(r => r.Product.UnitType).Include(r => r.Product.SubSubUnitType).Include(r => r.Product.SubUnitType).ToList();

                return PartialView(requests);
            }
        }
        [HttpPost]
        [Authorize(Roles = "LabManagement")]
        public IActionResult EditQuoteDetails(EditQuoteDetailsViewModel editQuoteDetailsViewModel)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Include(x => x.ParentQuote).Select(r => r);
                    /*var firstRequest = requests.Where(r => r.RequestID == editQuoteDetailsViewModel.Requests[0].RequestID).FirstOrDefault();
                    int? parentQuoteId = firstRequest.ParentQuoteID;*/
                    try
                    {
                        //var quoteDate = editQuoteDetailsViewModel.QuoteDate;
                        //var quoteNumber = editQuoteDetailsViewModel.QuoteNumber;
                        //firstRequest.ParentQuote.QuoteDate = quoteDate;

                        foreach (var quote in editQuoteDetailsViewModel.Requests)
                        {
                            //throw new Exception();
                            var request = requests.Where(r => r.RequestID == quote.RequestID).FirstOrDefault();
                            request.ParentQuote = editQuoteDetailsViewModel.ParentQuote;
                            _context.Entry(request.ParentQuote).State = EntityState.Added;
                            request.QuoteStatusID = 4;
                            request.Cost = quote.Cost;
                            request.Currency = editQuoteDetailsViewModel.Requests[0].Currency;
                            request.ExchangeRate = editQuoteDetailsViewModel.Requests[0].ExchangeRate;
                            request.IncludeVAT = editQuoteDetailsViewModel.Requests[0].IncludeVAT;
                            request.ExpectedSupplyDays = quote.ExpectedSupplyDays;
                            _context.Update(request);
                            _context.SaveChanges();
                        }
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
                        transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.RollbackAsync();
                        DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote, Guid.Empty, (int)requests.FirstOrDefault().ParentQuoteID);
                        throw new Exception(AppUtility.GetExceptionMessage(ex));
                    }
                }

                return RedirectToAction("_IndexTableDataByVendor", new { PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SectionType = AppUtility.MenuItems.LabManagement, SideBarType = AppUtility.SidebarEnum.Quotes });
            }
            catch (Exception ex)
            {
                var previousRequest = _context.Requests.Where(r => r.RequestID == editQuoteDetailsViewModel.Requests.FirstOrDefault().RequestID)
              .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(p => p.Product).ThenInclude(p => p.ProductSubcategory)
              .Include(r => r.ParentQuote).Include(r => r.Product.UnitType).Include(r => r.Product.SubSubUnitType).Include(r => r.Product.SubUnitType).FirstOrDefault();
                var newRequest = editQuoteDetailsViewModel.Requests.FirstOrDefault();
                previousRequest.Cost = newRequest.Cost;
                previousRequest.Currency = newRequest.Currency;
                previousRequest.ExpectedSupplyDays = newRequest.ExpectedSupplyDays;
                editQuoteDetailsViewModel.Requests[0] = previousRequest;
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
                requestNotifications = _context.RequestNotifications.Include(n => n.NotificationStatus).Where(rn => rn.NotificationStatusID == 1).Include(r => r.Request);
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.DidntArrive;
            }
            else
            {
                requestNotifications = _context.RequestNotifications.Include(n => n.NotificationStatus).Where(rn => rn.NotificationStatusID != 1);
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Notifications;
            }
            if (requestID != 0)
            {
                var notification = requestNotifications.Where(rn => rn.NotificationID == requestID).FirstOrDefault();
                notification.IsRead = true;
                _context.Update(notification);
                await _context.SaveChangesAsync();
            }

            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            ApplicationUser currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            var requests = requestNotifications.Where(n => n.ApplicationUserID == currentUser.Id).OrderByDescending(n => n.TimeStamp).ToList();
            return View(requests);
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> Cart(string errorMessage)
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Cart;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            return View(await GetIndexViewModelByVendor(new RequestIndexObject
            {
                SectionType = AppUtility.MenuItems.Requests,
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = AppUtility.SidebarEnum.Cart,
                ErrorMessage = errorMessage
            }));
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
            await RemoveTempRequestAsync(confirmExit.GUID);

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
            var request = _context.Requests
                .Where(r => r.RequestID == id)
                .Include(r => r.ApplicationUserCreator)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).FirstOrDefault();
            return PartialView(request);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> OrderLateModal(Request requestFromView)
        {
            var request = _context.Requests.Where(r => r.RequestID == requestFromView.RequestID).Include(r => r.ApplicationUserCreator).Include(r => r.ParentRequest).Include(r => r.Product)
                .ThenInclude(p => p.Vendor).FirstOrDefault();
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

                }

                client.Disconnect(true);

            }
            //return RedirectToAction("NotificationsView", new { DidntArrive = true });
            return new EmptyResult();
        }


        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AccountingPayments(AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment)
        {
            var payNowCount = await GetPaymentRequests(AppUtility.SidebarEnum.PayNow);
            TempData["PayNowCount"] = payNowCount.Count();
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Accounting;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingPayments;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = accountingPaymentsEnum;
            return View(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.Accounting, PageType = AppUtility.PageTypeEnum.AccountingPayments, SidebarType = accountingPaymentsEnum }));

        }
        private async Task<List<RequestPaymentsViewModel>> GetPaymentRequests(AppUtility.SidebarEnum accountingPaymentsEnum)
        {
            var requests = _context.Requests
                  .Include(r => r.ParentRequest)
                  .Include(r => r.Product).ThenInclude(p => p.Vendor)
                  .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType)
                  .Include(r => r.Product.ProductSubcategory).ThenInclude(pc => pc.ParentCategory).Include(r => r.Payments)
                  .Where(r => r.RequestStatusID != 7 && r.Payments.Where(p => !p.IsPaid).Count() > 0);
            var requestList = new List<RequestPaymentsViewModel>();
            switch (accountingPaymentsEnum)
            {
                case AppUtility.SidebarEnum.MonthlyPayment:
                    requests = requests
                    .Where(r => r.PaymentStatusID == 2 && r.Payments.FirstOrDefault().HasInvoice && r.Payments.FirstOrDefault().IsPaid == false);
                    await requests.ForEachAsync(r => requestList.Add(new RequestPaymentsViewModel { Request = r, Payment = r.Payments.FirstOrDefault() }));
                    break;
                case AppUtility.SidebarEnum.PayNow:
                    requests = requests
                    //.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                    .Where(r => r.PaymentStatusID == 3 && r.Payments.FirstOrDefault().IsPaid == false);
                    await requests.ForEachAsync(r => requestList.Add(new RequestPaymentsViewModel { Request = r, Payment = r.Payments.FirstOrDefault() }));
                    break;
                case AppUtility.SidebarEnum.PayLater:
                    requests = requests
                .Where(r => r.PaymentStatusID == 4 && r.Payments.FirstOrDefault().IsPaid == false);
                    await requests.ForEachAsync(r => requestList.Add(new RequestPaymentsViewModel { Request = r, Payment = r.Payments.FirstOrDefault() }));
                    break;
                case AppUtility.SidebarEnum.Installments:
                    requests = requests
                        .Where(r => r.PaymentStatusID == 5).Where(r => r.Payments.Where(p => p.IsPaid == false && p.PaymentDate < DateTime.Now.AddDays(5)).Count() > 0);
                    foreach (var request in requests)
                    {
                        var currentInstallments = request.Payments.Where(p => p.IsPaid == false && p.PaymentDate < DateTime.Now.AddDays(5)).ToList();
                        requestList.Add(new RequestPaymentsViewModel { Request = request, Payment = currentInstallments.ElementAt(0) });
                        if (currentInstallments.Count() > 0)
                        {
                            for (var i = 1; i < currentInstallments.Count(); i++)
                            {
                                requestList.Add(new RequestPaymentsViewModel { Request = request, Payment = currentInstallments.ElementAt(i) });
                            }
                        }
                    }

                    break;
                case AppUtility.SidebarEnum.StandingOrders:
                    requests = requests
                .Where(r => r.PaymentStatusID == 7).Where(r => r.Payments.Where(p => p.IsPaid == false && p.PaymentDate < DateTime.Now.AddDays(5)).Count() > 0);
                    await requests.ForEachAsync(r => requestList.Add(new RequestPaymentsViewModel { Request = r, Payment = r.Payments.FirstOrDefault() }));
                    break;
                case AppUtility.SidebarEnum.SpecifyPayment:
                    requests = requests
                .Where(r => r.PaymentStatusID == 8 && r.Payments.FirstOrDefault().HasInvoice);
                    await requests.ForEachAsync(r => requestList.Add(new RequestPaymentsViewModel { Request = r, Payment = r.Payments.FirstOrDefault() }));
                    break;
            }
            return requestList;
        }
        private IQueryable<Request> GetPaymentNotificationRequests(AppUtility.SidebarEnum accountingNotificationsEnum)
        {
            var requestsList = _context.Requests
                .Include(r => r.ParentRequest)
                .Include(r => r.Product).ThenInclude(p => p.Vendor)
                .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(pc => pc.ParentCategory)
                .Where(r => r.RequestStatusID != 7);
            switch (accountingNotificationsEnum)
            {
                case AppUtility.SidebarEnum.NoInvoice:
                    requestsList = requestsList.Where(r => r.Payments.FirstOrDefault().HasInvoice == false).Where(r => (r.PaymentStatusID == 2/*+30*/ && r.RequestStatusID == 3) || (r.PaymentStatusID == 3/*pay now*/) || (r.PaymentStatusID == 8/*specify payment*/ && r.RequestStatusID == 3));
                    break;
                case AppUtility.SidebarEnum.DidntArrive:
                    requestsList = requestsList.Where(r => r.RequestStatusID == 2).Where(r => r.ExpectedSupplyDays != null).Where(r => r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0).Date < DateTime.Today);
                    break;
                case AppUtility.SidebarEnum.PartialDelivery:
                    requestsList = requestsList.Where(r => r.RequestStatusID == 2 && r.IsPartial);
                    break;
                case AppUtility.SidebarEnum.ForClarification:
                    requestsList = requestsList.Where(r => r.IsClarify);
                    break;
            }
            return requestsList;
        }
        [HttpGet]
        [Authorize(Roles = " Accounting")]
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
        [Authorize(Roles = " Accounting")]
        public async Task<IActionResult> HandleNotifications(AppUtility.SidebarEnum type, int requestID)
        {
            var request = _context.Requests.Where(r => r.RequestID == requestID).FirstOrDefault();
            switch (type)
            {

                case AppUtility.SidebarEnum.DidntArrive:

                    break;
                case AppUtility.SidebarEnum.PartialDelivery:
                    request.IsPartial = false;
                    break;
                case AppUtility.SidebarEnum.ForClarification:
                    request.IsClarify = false;
                    break;
            }
            _context.Update(request);
            await _context.SaveChangesAsync();
            return RedirectToAction("AccountingNotifications", new { accountingNotificationsEnum = type });
        }

        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AccountingNotifications(AppUtility.SidebarEnum accountingNotificationsEnum = AppUtility.SidebarEnum.NoInvoice)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Accounting;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingNotifications;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = accountingNotificationsEnum;
            return View(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.Accounting, PageType = AppUtility.PageTypeEnum.AccountingNotifications, SidebarType = accountingNotificationsEnum }));

        }

        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> PaymentsPayModal(int? vendorid, int? requestid, int[] requestIds, AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            List<Request> requestsToPay = new List<Request>();
            StringWithBool Error = new StringWithBool();
            var requestsList = new List<Request>();
            GetPaymentRequests(accountingPaymentsEnum).Result.ForEach(rp => requestsList.Add(rp.Request));
            AppUtility.CurrencyEnum CurrencyUsed = AppUtility.CurrencyEnum.None;
            int? VendorID = 0;
            if (vendorid != null)
            {
                var vendorReqList = await requestsList.Where(r => r.Product.VendorID == vendorid).ToListAsync();
                if (vendorReqList.Select(r => r.Currency).Distinct().Count() > 1)
                {
                    Error.SetStringAndBool(true, ElixirStrings.ServerDifferentCurrencyErrorMessage);
                }
                requestsToPay = vendorReqList;
            }
            else if (requestid != null)
            {
                requestsToPay = await requestsList.Where(r => r.RequestID == requestid).ToListAsync();
            }
            else if (requestIds != null)
            {
                bool FirstTime = true;
                foreach (int rId in requestIds)
                {
                    var request = requestsList.Where(r => r.RequestID == rId).FirstOrDefault();
                    requestsToPay.Add(request);
                    if (FirstTime)
                    {
                        Enum.TryParse(request.Currency, out CurrencyUsed);
                        VendorID = request.Product.VendorID;
                        FirstTime = false;
                    }
                    else
                    {
                        if (request.Currency != CurrencyUsed.ToString())
                        {
                            Error.SetStringAndBool(true, ElixirStrings.ServerDifferentCurrencyErrorMessage);
                        }
                        if (request.Product.VendorID != VendorID)
                        {
                            Error.SetStringAndBool(true, ElixirStrings.ServerDifferentVendorErrorMessage);
                        }
                    }
                }
            }

            PaymentsPayModalViewModel paymentsPayModalViewModel = new PaymentsPayModalViewModel()
            {
                Requests = requestsToPay,
                AccountingEnum = accountingPaymentsEnum,
                Payment = new Payment(),
                PaymentTypes = _context.PaymentTypes.Select(pt => pt).ToList(),
                CompanyAccounts = _context.CompanyAccounts.Select(ca => ca).ToList(),
                ShippingToPay = await GetShippingsToPay(requestsToPay),
                Error = Error
            };

            //check if payment status type is installments to show the installments in the view model

            return PartialView(paymentsPayModalViewModel);
        }

        public async Task<List<CheckboxViewModel>> GetShippingsToPay(List<Request> requestsToPay)
        {
            List<CheckboxViewModel> shippings = new List<CheckboxViewModel>();
            foreach (var r in requestsToPay)
            {
                if (r.Payments.FirstOrDefault().ShippingPaidHere && !r.ParentRequest.IsShippingPaid && r.ParentRequest.Shipping > 0)
                {
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (paymentsPayModalViewModel.ShippingToPay != null)
                {
                    try
                    {
                        foreach (var shipping in paymentsPayModalViewModel.ShippingToPay)
                        {
                            var parentRequest = _context.ParentRequests.Where(pr => pr.ParentRequestID == shipping.ID).FirstOrDefault();
                            parentRequest.IsShippingPaid = true;

                            _context.Update(parentRequest);
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw exception here
                    }
                }
                try
                {
                    var paymentsList = _context.Payments.Where(p => p.IsPaid == false);
                    foreach (Request request in paymentsPayModalViewModel.Requests)
                    {
                        var requestToUpdate = _context.Requests.Where(r => r.RequestID == request.RequestID).FirstOrDefault();
                        Payment payment = _context.Payments.Where(p => p.RequestID == request.RequestID).FirstOrDefault();
                        //if (requestToUpdate.PaymentStatusID == 7)
                        //{
                        //    payment = paymentsList.Where(p => p.RequestID == requestToUpdate.RequestID).FirstOrDefault();
                        //    _context.Add(new Payment() { PaymentDate = payment.PaymentDate.AddMonths(1), RequestID = requestToUpdate.RequestID });
                        //}
                        //else if (requestToUpdate.PaymentStatusID == 5)
                        //{
                        //    var payments = paymentsList.Where(p => p.RequestID == requestToUpdate.RequestID);
                        //    var count = payments.Count();

                        //    payment = payments.OrderBy(p => p.PaymentDate).FirstOrDefault();
                        //    if (count <= 1)
                        //    {
                        //        payment.Sum = requestToUpdate.Cost ?? 0;
                        //    }
                        //}
                        //else
                        //{
                        payment.Sum = request.Cost ?? 0;
                        payment.PaymentDate = DateTime.Now.Date;
                        payment.RequestID = requestToUpdate.RequestID;
                        //}
                        payment.Reference = paymentsPayModalViewModel.Payment.Reference;
                        payment.CompanyAccountID = paymentsPayModalViewModel.Payment.CompanyAccountID;
                        payment.PaymentReferenceDate = paymentsPayModalViewModel.Payment.PaymentReferenceDate;
                        payment.PaymentTypeID = paymentsPayModalViewModel.Payment.PaymentTypeID;
                        payment.CreditCardID = paymentsPayModalViewModel.Payment.CreditCardID;
                        payment.CheckNumber = paymentsPayModalViewModel.Payment.CheckNumber;
                        payment.IsPaid = true;


                        _context.Update(payment);
                        _context.Update(requestToUpdate); //don't need this line of code
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Response.StatusCode = 500;
                    /*for (int i = 0; i < paymentsPayModalViewModel.Requests.Count; i++)
                    {
                        paymentsPayModalViewModel.Requests[i] = _context.Requests.Where(r => r.RequestID == paymentsPayModalViewModel.Requests[i].RequestID).Include(r => r.Product)
                            .ThenInclude(p => p.Vendor).FirstOrDefault();
                    }*/
                    paymentsPayModalViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    return PartialView("_ErrorMessage", paymentsPayModalViewModel.ErrorMessage);
                }
            }

            return RedirectToAction("AccountingPayments", new { accountingPaymentsEnum = paymentsPayModalViewModel.AccountingEnum });
        }
        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> PaymentsInvoiceModal(int? vendorid, int? paymentid, AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var payment = _context.Payments.Where(p => p.PaymentID == paymentid).FirstOrDefault();
            var requestToPay = _context.Requests.Where(r => r.RequestID == payment.RequestID).Include(r => r.ParentRequest)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType).Include(r => r.Payments).ToList();

            var paidSum = requestToPay.FirstOrDefault().Payments.Where(p => p.IsPaid).Select(p => p.Sum).Sum();
            var amtLeftToFullPayment = (decimal)requestToPay.FirstOrDefault().Cost - paidSum;
            /*            if (payment.InstallmentNumber == requestToPay.FirstOrDefault().Installments)
            */
            if (payment.Sum > amtLeftToFullPayment)
            {
                payment.Sum = amtLeftToFullPayment;
            }
            PaymentsInvoiceViewModel paymentsInvoiceViewModel = new PaymentsInvoiceViewModel()
            {
                Requests = requestToPay,
                AccountingEnum = accountingPaymentsEnum,
                Payment = payment,
                PaymentTypes = _context.PaymentTypes.Select(pt => pt).ToList(),
                CompanyAccounts = _context.CompanyAccounts.Select(ca => ca).ToList(),
                AmtLeftToPay = amtLeftToFullPayment,
                Invoice = new Invoice()
                {
                    InvoiceDate = DateTime.Today
                },
                ShippingToPay = await GetShippingsToPay(requestToPay)
            };


            return PartialView(paymentsInvoiceViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> PaymentsInvoiceModal(PaymentsInvoiceViewModel paymentsInvoiceViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var payment = _context.Payments.Where(p => p.PaymentID == paymentsInvoiceViewModel.Payment.PaymentID).FirstOrDefault(); ;
                    foreach (Request request in paymentsInvoiceViewModel.Requests)
                    {
                        var requestToUpdate = _context.Requests.Where(r => r.RequestID == request.RequestID).FirstOrDefault();

                        payment.Sum = request.Payments.FirstOrDefault().Sum;

                        //else
                        //{
                        //    payment.Sum = request.Cost ?? 0;
                        //    payment.PaymentDate = DateTime.Now.Date;
                        //    payment.RequestID = requestToUpdate.RequestID;
                        //}
                        payment.Reference = paymentsInvoiceViewModel.Payment.Reference;
                        payment.CompanyAccountID = paymentsInvoiceViewModel.Payment.CompanyAccountID;
                        payment.PaymentReferenceDate = paymentsInvoiceViewModel.Payment.PaymentReferenceDate;
                        payment.PaymentTypeID = paymentsInvoiceViewModel.Payment.PaymentTypeID;
                        payment.CreditCardID = paymentsInvoiceViewModel.Payment.CreditCardID;
                        payment.CheckNumber = paymentsInvoiceViewModel.Payment.CheckNumber;
                        payment.IsPaid = true;
                        payment.HasInvoice = true;
                        payment.Invoice = paymentsInvoiceViewModel.Invoice;

                        _context.Update(payment);
                        _context.Update(requestToUpdate);
                        await _context.SaveChangesAsync();

                        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
                        string requestFolder = Path.Combine(uploadFolder, request.RequestID.ToString());
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
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Response.StatusCode = 500;
                    for (int i = 0; i < paymentsInvoiceViewModel.Requests.Count; i++)
                    {
                        paymentsInvoiceViewModel.Requests[i] = _context.Requests.Where(r => r.RequestID == paymentsInvoiceViewModel.Requests[i].RequestID).Include(r => r.Product)
                            .ThenInclude(p => p.Vendor).FirstOrDefault();
                    }
                    paymentsInvoiceViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    return PartialView(paymentsInvoiceViewModel);
                }
            }

            return RedirectToAction("AccountingPayments", new { accountingPaymentsEnum = paymentsInvoiceViewModel.AccountingEnum });
        }

        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AddInvoiceModal(int? vendorid, int? requestid, int[] requestIds)
        {
            List<Request> Requests = new List<Request>();
            StringWithBool Error = new StringWithBool();
            var queryableRequests = _context.Requests
                .Include(r => r.ParentRequest)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.UnitType).Include(r => r.Product.SubUnitType).Include(r => r.Product.SubSubUnitType).Include(r => r.Payments)
                    .Where(r => r.IsDeleted == false && r.Payments.FirstOrDefault().HasInvoice == false && ((r.PaymentStatusID == 2/*+30*/ && r.RequestStatusID == 3) || (r.PaymentStatusID == 3/*pay now*/) || (r.PaymentStatusID == 8/*specify payment*/ && r.RequestStatusID == 3))).Where(r => r.RequestStatusID != 7);
            if (vendorid != null)
            {
                Requests = queryableRequests
                    .Where(r => r.Payments.FirstOrDefault().HasInvoice == false)
                    .Where(r => r.Product.VendorID == vendorid).ToList();
                if (queryableRequests.Select(r => r.Currency).Distinct().Count() > 1)
                {
                    Error.SetStringAndBool(true, ElixirStrings.ServerDifferentCurrencyErrorMessage);
                }
            }
            else if (requestid != null)
            {
                Requests = queryableRequests.Where(r => r.RequestID == requestid).ToList();
            }
            else if (requestIds != null)
            {
                AppUtility.CurrencyEnum CurrencyUsed = AppUtility.CurrencyEnum.None;
                int? VendorID = null;
                bool FirstTime = true;
                foreach (int rId in requestIds)
                {
                    var request = queryableRequests.Where(r => r.RequestID == rId).Include(r => r.Product).FirstOrDefault();
                    Requests.Add(request);
                    if (FirstTime)
                    {
                        Enum.TryParse(queryableRequests.FirstOrDefault().Currency, out CurrencyUsed);
                        VendorID = request.Product.VendorID;
                        FirstTime = false;
                    }
                    else
                    {
                        if (request.Currency != CurrencyUsed.ToString())
                        {
                            Error.SetStringAndBool(true, ElixirStrings.ServerDifferentCurrencyErrorMessage);
                        }
                        if (request.Product.VendorID != VendorID)
                        {
                            Error.SetStringAndBool(true, ElixirStrings.ServerDifferentVendorErrorMessage);
                        }
                    }
                }
            }
            AddInvoiceViewModel addInvoiceViewModel = new AddInvoiceViewModel()
            {
                Requests = Requests,
                Invoice = new Invoice()
                {
                    InvoiceDate = DateTime.Today
                },
                Guid = Guid.NewGuid(),
                Error = Error
            };
            return PartialView(addInvoiceViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AddInvoiceModal(AddInvoiceViewModel addInvoiceViewModel)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Add(addInvoiceViewModel.Invoice);
                    await _context.SaveChangesAsync();

                    /*                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
                                        string uploadFolderFrom = Path.Combine(uploadFolder, addInvoiceViewModel.Guid.ToString());
                                        string uplaodFolderPathFrom = Path.Combine(uploadFolderFrom, AppUtility.FolderNamesEnum.Invoices.ToString());*/

                    foreach (var request in addInvoiceViewModel.Requests)
                    {
                        var RequestToSave = _context.Requests.Where(r => r.RequestID == request.RequestID).Include(r => r.Payments).FirstOrDefault();
                        RequestToSave.Cost = request.Cost;
                        RequestToSave.Payments.FirstOrDefault().InvoiceID = addInvoiceViewModel.Invoice.InvoiceID;
                        RequestToSave.Payments.FirstOrDefault().HasInvoice = true;
                        _context.Update(RequestToSave);

                        MoveDocumentsOutOfTempFolder(request.RequestID, AppUtility.ParentFolderName.Requests, AppUtility.FolderNamesEnum.Invoices, true, addInvoiceViewModel.Guid);

                        await _context.SaveChangesAsync();

                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    addInvoiceViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    Response.StatusCode = 500;
                    return PartialView("AddInvoiceModal", addInvoiceViewModel);
                }
            }

            return RedirectToAction("AccountingNotifications");
        }

        public async Task<TempRequestListViewModel> LoadTempListFromRequestIndexObjectAsync(RequestIndexObject requestIndexObject)
        {
            var oldJson = _context.TempRequestJsons.Where(trj => trj.GuidID == requestIndexObject.GUID)
                .OrderByDescending(trj => trj.SequencePosition).FirstOrDefault();
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
        public async Task<IActionResult> UploadQuoteModal(RequestIndexObject requestIndexObject)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var uploadQuoteViewModel = new UploadQuoteViewModel() { ParentQuote = new ParentQuote() { ExpirationDate = DateTime.Now } };

            uploadQuoteViewModel.OrderTypeEnum = requestIndexObject.OrderType;
            uploadQuoteViewModel.TempRequestListViewModel = await LoadTempListFromRequestIndexObjectAsync(requestIndexObject);
            //uploadQuoteViewModel.TempRequestListViewModel = new TempRequestListViewModel()
            //{
            //    GUID = requestIndexObject.GUID,
            //    RequestIndexObject = requestIndexObject,
            //    TempRequestViewModels = oldJson.DeserializeJson<List<TempRequestViewModel>>()
            //};
            if (uploadQuoteViewModel.TempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.ProductID != 0)
            {
                foreach (var tempRequestViewModel in uploadQuoteViewModel.TempRequestListViewModel.TempRequestViewModels)
                {
                    var oldQuote = _context.Requests.Where(r => r.ProductID == tempRequestViewModel.Request.ProductID && r.ParentQuote.ExpirationDate >= DateTime.Now.Date).Select(r => r.ParentQuote).OrderByDescending(r => r.QuoteDate).FirstOrDefault();
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
                            uploadQuoteViewModel.FileStrings = new List<String>();
                            foreach (var orderfile in orderfilesfound)
                            {
                                string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                                uploadQuoteViewModel.FileStrings.Add(newFileString);
                            }
                        }

                    }
                    else
                    {
                        oldQuote = new ParentQuote() { ExpirationDate = DateTime.Now };
                    }
                    uploadQuoteViewModel.ParentQuote = oldQuote;


                }

            }

            return PartialView(uploadQuoteViewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _UploadQuoteModal()
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var uploadQuoteViewModel = new UploadQuoteViewModel() { ParentQuote = new ParentQuote() { ExpirationDate = DateTime.Now } };
            //uploadQuoteViewModel.TempRequestListViewModel = new TempRequestListViewModel()
            //{
            //    GUID = requestIndexObject.GUID,
            //    RequestIndexObject = requestIndexObject,
            //    TempRequestViewModels = oldJson.DeserializeJson<List<TempRequestViewModel>>()
            //};


            return PartialView(uploadQuoteViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadQuoteModal(UploadQuoteViewModel uploadQuoteOrderViewModel, TempRequestListViewModel tempRequestListViewModel, bool isCancel = false)
        {
            try
            {

                var oldTempRequestJson = await GetTempRequestAsync(tempRequestListViewModel.GUID);
                var newTempRequestJson = await CopyToNewCurrentTempRequestAsync(oldTempRequestJson, 2);

                var deserializedTempRequestListViewModel = new TempRequestListViewModel()
                {
                    TempRequestViewModels = newTempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels
                };

                foreach (var tempRequestViewModel in deserializedTempRequestListViewModel.TempRequestViewModels)
                {

                    tempRequestViewModel.Request.QuoteStatusID = 4;
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

                if (deserializedTempRequestListViewModel.TempRequestViewModels.FirstOrDefault().Request.OrderType != AppUtility.OrderTypeEnum.AddToCart.ToString())
                {
                    //    var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                    //    _httpContextAccessor.HttpContext.Session.SetObject(requestNum, request);
                    await SetTempRequestAsync(newTempRequestJson, deserializedTempRequestListViewModel, tempRequestListViewModel.RequestIndexObject);
                    //await KeepTempRequestJsonCurrentAsOriginal(newTempRequestJson.GuidID);
                    //tempRequestListViewModel.TempRequestViewModels = deserializedTempRequestListViewModel.TempRequestViewModels;
                    tempRequestListViewModel.RequestIndexObject.GUID = tempRequestListViewModel.GUID;
                    return RedirectToAction("TermsModal", tempRequestListViewModel.RequestIndexObject);
                }
                else
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var tempRequestViewModel in deserializedTempRequestListViewModel.TempRequestViewModels)
                            {
                                if (tempRequestViewModel.Request.ParentQuote.ParentQuoteID == 0)
                                {
                                    _context.Entry(tempRequestViewModel.Request.ParentQuote).State = EntityState.Added;

                                }
                                else
                                {
                                    _context.Entry(tempRequestViewModel.Request.ParentQuote).State = EntityState.Modified;

                                }
                                if (tempRequestViewModel.Request.Product.ProductID == 0)
                                {
                                    tempRequestViewModel.Request.Product.SerialNumber = GetSerialNumber(false);
                                    _context.Entry(tempRequestViewModel.Request.Product).State = EntityState.Added;
                                }
                                else
                                {
                                    _context.Entry(tempRequestViewModel.Request.Product).State = EntityState.Modified;
                                }
                                _context.Entry(tempRequestViewModel.Request).State = EntityState.Added;
                                await _context.SaveChangesAsync();

                                if (tempRequestViewModel.Comments != null)
                                {
                                    foreach (var comment in tempRequestViewModel.Comments)
                                    {
                                        comment.ObjectID = tempRequestViewModel.Request.RequestID;
                                        _context.Add(comment);
                                    }
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
                            await RemoveTempRequestAsync(deserializedTempRequestListViewModel.GUID);
                            //base.RemoveRequestWithCommentsAndEmailSessions();

                            /*var action = "_IndexTableData";
                            if (tempRequestListViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestRequest)
                            {
                                action = "_IndexTableWithCounts";
                                return RedirectToAction(action, "Requests", tempRequestListViewModel.RequestIndexObject);
                            }
                            else if (tempRequestListViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart)
                            {
                                action = "NotificationsView";
                                return RedirectToAction(action, "Requests", tempRequestListViewModel.RequestIndexObject);
                            }
                            return await RedirectRequestsToShared(action, tempRequestListViewModel.RequestIndexObject);*/
                            return new EmptyResult();
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                }

                //var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                //var request = _httpContextAccessor.HttpContext.Session.GetObject<Request>(requestName);
                //request.ParentQuote = uploadQuoteOrderViewModel.ParentQuote;
                //if (uploadQuoteOrderViewModel.ExpectedSupplyDays != null)
                //{
                //    request.ExpectedSupplyDays = uploadQuoteOrderViewModel.ExpectedSupplyDays;
                //}
                //if (request.RequestStatusID == 1)
                //{
                //    TempData["RequestStatus"] = 1;
                //}

                //if ((request.RequestStatusID == 6 || request.RequestStatusID == 1) && request.OrderType != AppUtility.OrderTypeEnum.AddToCart.ToString())
                //{
                //    var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                //    _httpContextAccessor.HttpContext.Session.SetObject(requestNum, request);
                //    return RedirectToAction("TermsModal", uploadQuoteOrderViewModel.RequestIndexObject);
                //}


            }
            catch (Exception ex)
            {
                await RollbackCurrentTempAsync(tempRequestListViewModel.GUID);
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
            var uploadOrderViewModel = new UploadOrderViewModel();

            uploadOrderViewModel.TempRequestListViewModel = await LoadTempListFromRequestIndexObjectAsync(requestIndexObject);

            long lastParentRequestOrderNum = 0;
            var prs = _context.ParentRequests;
            if (_context.ParentRequests.Any())
            {
                lastParentRequestOrderNum = _context.ParentRequests.IgnoreQueryFilters().OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber ?? 0;
            }
            ParentRequest pr = new ParentRequest()
            {
                ApplicationUserID = _userManager.GetUserId(User),
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

            var jsonRequest = _context.TempRequestJsons.Where(t => t.GuidID == requestIndexObject.GUID).OrderByDescending(t => t.SequencePosition).FirstOrDefault();
            var fullRequestJson = new FullRequestJson()
            {
                TempRequestViewModels = uploadOrderViewModel.TempRequestListViewModel.TempRequestViewModels,
                RequestIndexObject = uploadOrderViewModel.TempRequestListViewModel.RequestIndexObject
            };
            jsonRequest.SerializeViewModel(fullRequestJson);
            _context.Update(jsonRequest);
            _context.SaveChanges();

            return PartialView(uploadOrderViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadOrderModal(UploadOrderViewModel uploadOrderViewModel, TempRequestListViewModel tempRequestListViewModel, bool isCancel = false)
        {
            try
            {
                var oldTempRequestJson = await GetTempRequestAsync(tempRequestListViewModel.GUID);
                var newTempRequestJson = await CopyToNewCurrentTempRequestAsync(oldTempRequestJson, 2);
                var deserializedTempRequestListViewModel = new TempRequestListViewModel()
                {
                    TempRequestViewModels = newTempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels
                };
                foreach (var tempRequest in deserializedTempRequestListViewModel.TempRequestViewModels)
                {
                    //if (isCancel)
                    //{
                    //    tempRequest.Request.RequestStatusID = 6;
                    //}
                    //else
                    //{
                    if (uploadOrderViewModel.ExpectedSupplyDays != null)
                    {
                        tempRequest.Request.ExpectedSupplyDays = uploadOrderViewModel.ExpectedSupplyDays;
                    }
                    if (uploadOrderViewModel.ParentRequest.OrderDate == DateTime.Today) //if it's today, add seconds to be now so it shows up on top
                    {
                        uploadOrderViewModel.ParentRequest.OrderDate = DateTime.Now;
                    }
                    tempRequest.Request.ParentRequest = uploadOrderViewModel.ParentRequest;
                    //}
                }
                //throw new Exception();
                await SetTempRequestAsync(newTempRequestJson, deserializedTempRequestListViewModel, tempRequestListViewModel.RequestIndexObject);
                //await KeepTempRequestJsonCurrentAsOriginal(newTempRequestJson.GuidID);
                //do we need this current/original here??
                //var requests = new List<Request>();
                //var isRequests = true;
                //var RequestNum = 1;
                //while (isRequests)
                //{
                //    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                //    var req = _httpContextAccessor.HttpContext.Session.GetObject<Request>(requestName);
                //    if (req != null)
                //    {
                //        if (uploadQuoteOrderViewModel.ExpectedSupplyDays != null)
                //        {
                //            req.ExpectedSupplyDays = uploadQuoteOrderViewModel.ExpectedSupplyDays;
                //        }
                //        requests.Add(req);
                //    }
                //    else
                //    {
                //        isRequests = false;
                //    }
                //    RequestNum++;
                //}
                //RequestNum = 1;
                //foreach (var request in requests)
                //{
                //    request.ParentRequest = uploadQuoteOrderViewModel.ParentRequest;
                //    request.ParentQuote = null;

                //    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;

                //    _httpContextAccessor.HttpContext.Session.SetObject(requestName, request);
                //    RequestNum++;
                //}
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
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> TermsModal(TermsViewModel termsViewModel, TempRequestListViewModel tempRequestListViewModel, bool isCancel = false)
        {

            if (isCancel)
            {
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests, tempRequestListViewModel.GUID);
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote, tempRequestListViewModel.GUID);
                await RemoveTempRequestAsync(tempRequestListViewModel.GUID);
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
            var request = await _context.Requests.Where(r => r.RequestID == requestID).Include(r => r.ApplicationUserCreator).FirstOrDefaultAsync();
            var vendor = await _context.Requests.Where(r => r.RequestID == requestID).Select(r => r.Product.Vendor).FirstOrDefaultAsync();
            var vendorCartTotal = _context.Requests.Where(r => r.Product.VendorID == vendor.VendorID && r.ApplicationUserCreatorID == request.ApplicationUserCreatorID &&
            r.OrderType == AppUtility.OrderTypeEnum.AddToCart.ToString() && r.RequestStatusID != 1)
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                    {
                        var request = _context.Requests.Where(r => r.RequestID == requestId).FirstOrDefault();
                        var requestLocations = _context.Requests.Where(r => r.RequestID == request.RequestID).Include(r => r.RequestLocationInstances).FirstOrDefault().RequestLocationInstances;
                        //archive one location and delete the rest
                        var iterator = requestLocations.GetEnumerator();
                        while (iterator.MoveNext())
                        {
                            var locationToDelete = iterator.Current;
                            _context.Remove(locationToDelete);
                            MarkLocationAvailable(requestId, locationToDelete.LocationInstanceID);
                        }
                        await _context.SaveChangesAsync();
                        await SaveLocations(receivedModalVisualViewModel, request, true);
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(AppUtility.GetExceptionMessage(ex));
                }
            }
            return RedirectToAction("_LocationTab", new { id = requestId });
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> MoveToListModal(int requestID, int prevListID = 0)
        {
            var pageType = AppUtility.PageTypeEnum.RequestCart;
            var userLists = _context.RequestLists.Where(rl => rl.ApplicationUserOwnerID == _userManager.GetUserId(User))
               .OrderBy(rl => rl.DateCreated).ToList();
            var sharedLists =  _context.ShareRequestLists.Where(srl => srl.ToApplicationUserID == _userManager.GetUserId(User) && !srl.ViewOnly).Include(srl => srl.RequestList).OrderBy(srl => srl.TimeStamp).Select(srl => srl.RequestList).ToList();
            sharedLists.ForEach(sl => userLists.Add(sl));
            if (userLists.Count == 0)
            {
                RequestList requestList = new RequestList
                {
                    Title = "List 1",
                    ApplicationUserOwnerID = _userManager.GetUserId(User),
                    RequestListRequests = new List<RequestListRequest>(),
                    DateCreated = DateTime.Now,
                    IsDefault = true
                };
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Entry(requestList).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        userLists.Add(requestList);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(AppUtility.GetExceptionMessage(ex));
                    }
                }
            }
            userLists = userLists.Where(ul => ul.ListID != prevListID).ToList();
            if (prevListID == 0)
            {
                pageType = AppUtility.PageTypeEnum.RequestInventory;
            }
            MoveListViewModel viewModel = new MoveListViewModel()
            {
                Request = _context.Requests.Where(r => r.RequestID == requestID).Include(r => r.Product).FirstOrDefault(),
                PreviousListID = prevListID,
                RequestLists = userLists,
                PageType = pageType
            };
            return PartialView(viewModel);
        }

        public async void MoveLists(int requestToMoveId, int newListID, int prevListID = 0)
        {

            var existingListRequest = _context.RequestListRequests.Where(l => l.RequestID == requestToMoveId && l.ListID == newListID).FirstOrDefault();
            if (existingListRequest != null)
            {
                existingListRequest.TimeStamp = DateTime.Now;
                _context.Update(existingListRequest);
            }
            else
            {
                RequestListRequest requestListRequest = new RequestListRequest()
                {
                    RequestID = requestToMoveId,
                    ListID = newListID,
                    TimeStamp = DateTime.Now
                };
                _context.Entry(requestListRequest).State = EntityState.Added;
            }
            if (prevListID != 0)
            {
                var oldRequestListRequest = _context.RequestListRequests
                    .Where(rlr => rlr.RequestID == requestToMoveId && rlr.ListID == prevListID).FirstOrDefault();
                _context.Remove(oldRequestListRequest);
            }

        }

        [HttpPost]
        [Authorize(Roles = "Requests")]

        public async Task<IActionResult> MoveToListModal(MoveListViewModel moveListViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    MoveLists(moveListViewModel.Request.RequestID, moveListViewModel.NewListID, moveListViewModel.PreviousListID);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(AppUtility.GetExceptionMessage(ex));
                }
            }
            if (moveListViewModel.PageType == AppUtility.PageTypeEnum.RequestCart)
            {
                RequestIndexObject indexObject = new RequestIndexObject()
                {
                    PageType = AppUtility.PageTypeEnum.RequestCart,
                    SidebarType = AppUtility.SidebarEnum.MyLists,
                    ListID = moveListViewModel.PreviousListID
                };
                return RedirectToAction("_IndexTableWithListTabs", indexObject);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> NewListModal(int requestToAddId = 0, int requestPreviousListID = 0)
        {
            NewListViewModel viewModel = new NewListViewModel()
            {
                OwnerID = _userManager.GetUserId(User),
                RequestToAddID = requestToAddId,
                RequestPreviousListID = requestPreviousListID
            };
            return PartialView(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> NewListModal(NewListViewModel newListViewModel)
        {
            var newList = new RequestList();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    newList.ApplicationUserOwnerID = newListViewModel.OwnerID;
                    newList.Title = newListViewModel.ListTitle;
                    newList.DateCreated = DateTime.Now;
                    _context.Entry(newList).State = EntityState.Added;

                    await _context.SaveChangesAsync();

                    if (newListViewModel.RequestToAddID != 0)
                    {
                        MoveLists(newListViewModel.RequestToAddID, newList.ListID, newListViewModel.RequestPreviousListID);

                        await _context.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(AppUtility.GetExceptionMessage(ex));
                }
            }
            RequestIndexObject indexObject = new RequestIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = AppUtility.SidebarEnum.MyLists,
                ListID = newList.ListID
            };
            return RedirectToAction("_IndexTableWithListTabs", indexObject);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteListRequestModal(int requestID, int listID)
        {
            DeleteListRequestViewModel viewModel = new DeleteListRequestViewModel()
            {
                Request = _context.Requests.Where(r => r.RequestID == requestID).Include(r => r.Product).FirstOrDefault(),
                List = _context.RequestLists.Where(l => l.ListID == listID).FirstOrDefault()
            };
            return PartialView(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteListRequestModal(DeleteListRequestViewModel viewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var list = _context.RequestLists.Where(l => l.ListID == viewModel.List.ListID)
                        .Include(l => l.RequestListRequests).FirstOrDefault();
                    var requestListRequest = list.RequestListRequests.Where(rlr => rlr.RequestID == viewModel.Request.RequestID).FirstOrDefault();
                    _context.Remove(requestListRequest);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(AppUtility.GetExceptionMessage(ex));
                }
            }
            RequestIndexObject indexObject = new RequestIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = AppUtility.SidebarEnum.MyLists,
                ListID = viewModel.List.ListID
            };
            return RedirectToAction("_IndexTableWithListTabs", indexObject);
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ListSettingsModal(AppUtility.SidebarEnum SidebarType, int selectedListID=0)
        {
            //var sharedList = new ShareRequestList()
            //{
            //    RequestListID = 27,
            //    FromApplicationUserID = "b96445c0-f6ef-42eb-8638-9a65ab094445",
            //    ToApplicationUserID = "f61f584c-7d1d-4692-801a-82a3069acad5"
            //};
            //_context.Update(sharedList);
            //await _context.SaveChangesAsync();

            var listSettings = GetListSettingsInfo(selectedListID, SidebarType);

            return PartialView(listSettings);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ListSettingsModal(ListSettingsViewModel listSettings, int selectedIndexListID)
        {
            await UpdateListDetails(listSettings);

            RequestIndexObject indexObject = new RequestIndexObject()
            {
                PageType = AppUtility.PageTypeEnum.RequestCart,
                SidebarType = listSettings.SidebarType,
                ListID = selectedIndexListID
                
            };
            return RedirectToAction("_IndexTableWithListTabs", indexObject);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _ListSettings(int listID, AppUtility.SidebarEnum SidebarType)
        {
            var listSettings = GetListSettingsInfo(listID, SidebarType);

            return PartialView(listSettings);
        }

        [Authorize(Roles = "Requests")]
        public async Task<bool> UpdateListDetails(ListSettingsViewModel listSettings)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var viewModelList = listSettings.SidebarType == AppUtility.SidebarEnum.MyLists ? listSettings.SelectedList : listSettings.SelectedSharedList.RequestList;
                    var list = _context.RequestLists.Where(rl => rl.ListID == viewModelList.ListID).FirstOrDefault();
                    list.Title = viewModelList.Title;
                    _context.Update(list);
                    if (listSettings.SharedUsers != null)
                    {
                        foreach (var user in listSettings.SharedUsers)
                        {
                            if (!user.IsRemoved)
                            {
                                if (user.ShareRequestList.ShareID == 0)
                                {
                                    user.ShareRequestList.ToApplicationUser = null;
                                    user.ShareRequestList.RequestListID = listSettings.SelectedList.ListID;
                                    user.ShareRequestList.FromApplicationUserID = _userManager.GetUserId(User);
                                    _context.Add(user.ShareRequestList);
                                }
                                else
                                {
                                    var sharelist = _context.ShareRequestLists.Where(srl => srl.ShareID == user.ShareRequestList.ShareID).FirstOrDefault();
                                    sharelist.ViewOnly = user.ShareRequestList.ViewOnly;
                                    _context.Update(sharelist);
                                }
                            }
                            else
                            {
                                if (user.ShareRequestList.ShareID != 0)
                                {
                                    _context.Remove(user.ShareRequestList);
                                }
                            }

                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(AppUtility.GetExceptionMessage(ex));
                }
            }
            return true;
        }

        [Authorize(Roles = "Requests")]
        private ListSettingsViewModel GetListSettingsInfo(int selectedListID, AppUtility.SidebarEnum sidebarType)
        {

            var viewModel = new ListSettingsViewModel();
            if(sidebarType == AppUtility.SidebarEnum.SharedLists)
            {
                viewModel = GetSharedListSettings(selectedListID);
            }
            else
            {
                viewModel = GetMyListSettings(selectedListID);
            }

            if(viewModel.SelectedList == null && viewModel.SelectedSharedList == null)
            {
                return new ListSettingsViewModel();
            }

           
            viewModel.SharedUsers = _context.ShareRequestLists.Where(l => l.RequestListID == selectedListID).Include(l => l.ToApplicationUser).Select(
                l => new ShareRequestListViewModel
                {
                    ShareRequestList = l
                }).ToList();

            viewModel.ApplicationUsers = GetListUsersDropdown(viewModel);

            return viewModel;
        }

        [Authorize(Roles = "Requests")]
        private ListSettingsViewModel GetSharedListSettings(int selectedListID)
        {
            var userLists = new List<RequestList>();
            var defaultList = _context.ShareRequestLists.Where(srl => srl.RequestListID == selectedListID && srl.ToApplicationUserID == _userManager.GetUserId(User)).Include(srl => srl.RequestList).Include(srl => srl.FromApplicationUser).FirstOrDefault();
            userLists = _context.ShareRequestLists.Where(srl => srl.ToApplicationUserID == _userManager.GetUserId(User)).Include(srl => srl.RequestList).OrderBy(srl => srl.TimeStamp).Select(srl => srl.RequestList).ToList();
            ListSettingsViewModel viewModel = new ListSettingsViewModel()
            {
                RequestLists = userLists,
                SelectedSharedList = defaultList,
                SidebarType = AppUtility.SidebarEnum.SharedLists
            };
            return viewModel;
        }

        [Authorize(Roles = "Requests")]
        private ListSettingsViewModel GetMyListSettings(int selectedListID)
        {
            var userLists = new List<RequestList>();
            var defaultList = _context.RequestLists.Where(rl => rl.ListID == selectedListID).FirstOrDefault();
            userLists = _context.RequestLists.Where(rl => rl.ApplicationUserOwnerID == _userManager.GetUserId(User)).OrderBy(rl => rl.DateCreated).ToList();
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
            var selectedList = listSettings.SidebarType==AppUtility.SidebarEnum.MyLists ? listSettings.SelectedList : listSettings.SelectedSharedList.RequestList;
            listSettings.ApplicationUsers = _context.Users
                              .Where(u => u.Id != _userManager.GetUserId(User)
                              && (!listSettings.SharedUsers.Select(su => su.ShareRequestList.ToApplicationUserID).Contains(u.Id))
                              && u.Id != selectedList.ApplicationUserOwnerID)
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
                    if (listSettings.SharedUsers!= null && listSettings.SharedUsers.Select(su => su.ShareRequestList.ToApplicationUserID).Contains(id))
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
                                    ToApplicationUser = _context.Employees.Where(e => e.Id == id).FirstOrDefault(),
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
            var viewModel = _context.RequestLists.Where(rl => rl.ListID == listID).FirstOrDefault();

            return PartialView(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> DeleteListModal(RequestList deleteList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var list = _context.RequestLists.Where(l => l.ListID == deleteList.ListID)
                        .Include(l => l.RequestListRequests).FirstOrDefault();
                    foreach (var rlr in list.RequestListRequests)
                    {
                        _context.Remove(rlr);
                    }
                    var shareRequestLists = _context.ShareRequestLists.Where(srl => srl.RequestListID == deleteList.ListID).ToList();
                    foreach(var srl in shareRequestLists)
                    {
                        _context.Remove(srl);
                    }
                    _context.Remove(list);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(AppUtility.GetExceptionMessage(ex));
                }
            }

            return RedirectToAction("ListSettingsModal");
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> SaveListModal(int listID)
        {
            return PartialView(listID);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> SaveListModal(ListSettingsViewModel listSettings)
        {
            await UpdateListDetails(listSettings);

            return new EmptyResult();
        }

        //[HttpGet]
        //public async Task<IActionResult> CancelModal(Guid Guid, AppUtility.ModalType ModalType)
        //{
        //    //if (ModalType != AppUtility.ModalType.Reorder) //no point in doing it for reorder....

        //    var oldTempRequestJson = await GetTempRequestAsync(Guid);
        //    var newTempRequestJson = await CopyToNewCurrentTempRequestAsync(oldTempRequestJson);
        //    var deserializedTempRequestListViewModel = new TempRequestListViewModel()
        //    {
        //        TempRequestViewModels = newTempRequestJson.DeserializeJson<FullRequestJson>().TempRequestViewModels
        //    };

        //    switch (ModalType)
        //    {
        //        case AppUtility.ModalType.Reorder:
        //            DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests, Guid);
        //            DeleteTemporaryDocuments(AppUtility.ParentFolderName.ParentQuote, Guid);
        //            await RemoveTempRequestAsync(Guid);
        //            return new EmptyResult();
        //        case AppUtility.ModalType.UploadOrder:
        //            foreach (var tempRequest in deserializedTempRequestListViewModel.TempRequestViewModels)
        //            {
        //                tempRequest.Request.RequestStatusID = 6;
        //            }
        //            break;
        //        case AppUtility.ModalType.UploadQuote:
        //            break;
        //        case AppUtility.ModalType.ConfirmEmail:
        //            break;
        //        case AppUtility.ModalType.Terms:
        //            break;
        //    }
        //    await SetTempRequestAsync(newTempRequestJson, deserializedTempRequestListViewModel);
        //    //await KeepTempRequestJsonCurrentAsOriginal(newTempRequestJson.GuidID);
        //    return new EmptyResult();
        //}


        //public async Task<bool> PopulateProductSerialNumber()
        //{
        //    var products = _context.Products.Select(p => p).Include(p => p.ProductSubcategory.ParentCategory).ToList();
        //    var operationSerialNumber = 0;
        //    var orderSerialNumber = 0;
        //    foreach (var product in products)
        //    {
        //        if (product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
        //        {
        //            product.SerialNumber = "L" + orderSerialNumber;
        //            orderSerialNumber++;
        //        }
        //        else
        //        {
        //            product.SerialNumber = "P" + operationSerialNumber;
        //            operationSerialNumber++;
        //        }
        //        _context.Update(product);
        //        _context.SaveChanges();
        //    }
        //    return true;
        //}

        [HttpGet]
        public async Task UploadRequestsFromExcel()
        {
            var InventoryFileName = @"C:\Users\debbie\OneDrive - Centarix\Desktop\inventoryexcel2.csv";
            var POFileName = @"C:\Users\debbie\OneDrive - Centarix\Desktop\ExcelForTesting\_2019.xlsx";

            var lineNumber = 0;

            var excelRequests = new ExcelQueryFactory(InventoryFileName);
            var excelInvoices = new ExcelQueryFactory(POFileName);
            try
            {
                var requests = from r in excelRequests.Worksheet<UploadExcelModel>("inventory excel") select r;
                var requestsInvoice = from i in excelInvoices.Worksheet<UploadInvoiceExcelModel>("orders") select i;
                var lastSerialNumber = int.Parse(_context.Products.IgnoreQueryFilters().Where(p => p.ProductSubcategory.ParentCategory.CategoryTypeID == 1).OrderBy(p => p).LastOrDefault()?.SerialNumber?.Substring(1) ?? "1");
                var currency = AppUtility.CurrencyEnum.USD;
                long lastParentRequestOrderNum = 1500;
                var requestInvoiceList = requestsInvoice.ToList();
                foreach (var r in requests)
                {
                    lineNumber++;
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var categories = await _context.ProductSubcategories.IgnoreQueryFilters().Include(pc => pc.ParentCategory).Where(ps => ps.ParentCategory.CategoryTypeID == 1).ToListAsync();
                            var requestedBy = _context.Employees.Where(e => e.Email == r.RequstedBy).FirstOrDefault()?.Id;
                            var receivedBy = _context.Employees.Where(e => e.Email == r.ReceivedBy).FirstOrDefault()?.Id;
                            var orderedBy = _context.Employees.Where(e => e.Email == r.OrderedBy).FirstOrDefault()?.Id;
                            var vendorID = _context.Vendors.Where(v => v.VendorEnName == r.VendorName).Select(v => v.VendorID).FirstOrDefault();
                            //check if product exists based on vendor catalog number
                            var productID = _context.Products.Where(p => p.VendorID == vendorID && p.CatalogNumber.ToLower() == r.CatalogNumber.ToLower()).Select(p => p.ProductID).FirstOrDefault();
                            var request = new Request() { };
                            if (vendorID == 0)
                            {

                                WriteErrorToFile("Row " + lineNumber + " did not have a proper vendor");
                                _context.ChangeTracker.Entries()
                                    .Where(e => e.Entity != null).ToList();


                                throw new Exception("failed to find vendor");
                            }
                            if (productID == 0)
                            {
                                var product = new Product()
                                {
                                    ProductName = r.ItemName,
                                    VendorID = vendorID,
                                    ProductSubcategoryID = categories.Where(ps => new string(ps.ProductSubcategoryDescription.ToLower().Where(c => char.IsLetterOrDigit(c)).ToArray()) == new string(r.ProductSubCategoryName.ToLower().Where(c => char.IsLetterOrDigit(c)).ToArray()) && ps.ParentCategory.ParentCategoryDescription.ToLower() == r.ParentCategoryName.ToLower()).Select(ps => ps.ProductSubcategoryID).FirstOrDefault(),
                                    CatalogNumber = r.CatalogNumber,
                                    SerialNumber = "L" + lastSerialNumber++,
                                    ProductCreationDate = DateTime.Now,
                                    UnitTypeID = -1,
                                };
                                if (lineNumber > 548)
                                {
                                    lineNumber = lineNumber;
                                }
                                _context.Entry(product).State = EntityState.Added;
                                await _context.SaveChangesAsync();
                                request.ProductID = product.ProductID;
                            }
                            else
                            {
                                request.ProductID = productID;
                            }

                            var orderType = AppUtility.OrderTypeEnum.ExcelUpload.ToString();


                            var exchangeRate = AppUtility.GetExchangeRateByDate(r.DateOrdered);
                            //cost = cost * exchangeRate; //always from quartzy in dollars        
                            int parentRequestID = 0;
                            if (r.OrderNumber != "")
                            {
                                parentRequestID = _context.ParentRequests.Where(pr => pr.QuartzyOrderNumber == r.OrderNumber).Select(pr => pr.ParentRequestID).FirstOrDefault();

                            }
                            if (parentRequestID != 0)
                            {
                                request.ParentRequestID = parentRequestID;
                            }
                            else
                            {
                                ParentRequest parentRequest = null;
                                if (r.OrderNumber != null && r.OrderNumber.StartsWith("1") && r.OrderNumber.Length == 8)
                                {
                                    parentRequest = new ParentRequest() { QuartzyOrderNumber = r.OrderNumber, OrderNumber = int.Parse(r.OrderNumber.Substring(3)), ApplicationUserID = orderedBy, OrderDate = r.DateOrdered };
                                }
                                else
                                {
                                    parentRequest = new ParentRequest() { QuartzyOrderNumber = r.OrderNumber, OrderNumber = lastParentRequestOrderNum++, ApplicationUserID = orderedBy, OrderDate = r.DateOrdered };
                                }
                                _context.Entry(parentRequest).State = EntityState.Added;
                                await _context.SaveChangesAsync();
                                request.ParentRequestID = parentRequest.ParentRequestID;
                            }
                            request.ApplicationUserCreatorID = requestedBy;
                            request.RequestStatusID = 3;
                            request.ApplicationUserReceiverID = receivedBy;
                            request.ArrivalDate = r.DateReceived;
                            request.Cost = r.TotalPrice * 3.2M;
                            request.Currency = currency.ToString();
                            request.Unit = r.Unit;
                            request.ExchangeRate = 3.2M;
                            request.CreationDate = r.DateRequested;
                            request.ParentQuoteID = null;
                            request.OrderType = orderType;
                            request.IncludeVAT = true;
                            request.URL = r.Url;
                            request.PaymentStatusID = 2;
                            request.Installments = 1;
                            _context.Entry(request).State = EntityState.Added;
                            await _context.SaveChangesAsync();

                            var requestLocationInstance = new RequestLocationInstance() { RequestID = request.RequestID, LocationInstanceID = -1 };
                            _context.Entry(requestLocationInstance).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                            try
                            {
                                try
                                {
                                    if (r.OrderNumber != null && r.OrderNumber != "")
                                    {

                                        var invoiceRow = requestInvoiceList.Where(i => i.OrderNumber == r.OrderNumber && new string(i.CatalogNumber.ToLower().Where(c => char.IsLetterOrDigit(c)).ToArray()) == new string(r.CatalogNumber.ToLower().Where(c => char.IsLetterOrDigit(c)).ToArray()) && i.DocumentNumber != 0 && i.InvoiceNumber != "").FirstOrDefault();
                                        // var invoiceNumbers = invoiceRows.Select(ir => ir.InvoiceNumber).ToList();
                                        if (invoiceRow != null)
                                        {

                                            WriteErrorToFile("this row went in perfectly");
                                            await SetInvoiceAndPaymentsAccordingToResultsFromDB(r, request, invoiceRow);
                                        }


                                        //var invoiceIds= invoices.Select(i=>i.InvoiceID);
                                        //if(invoiceIds.Count()==invoiceNumbers.Count() && invoiceNumbers.Count()>0)
                                        //{
                                        //    foreach(var invoiceID in invoiceIds)
                                        //    {
                                        //        var payment = new Payment() { InvoiceID = invoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
                                        //        _context.Entry(payment).State = EntityState.Added;
                                        //    }

                                        //}
                                        //else if (invoiceIds.Count() != invoiceNumbers.Count() && invoiceNumbers.Count() > 0)
                                        //{
                                        //    foreach (var invoiceID in invoiceIds)
                                        //    {
                                        //        var payment = new Payment() { InvoiceID = invoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
                                        //        _context.Entry(payment).State = EntityState.Added;
                                        //    }
                                        //    var invoiceNumbersNotInDataBaseYet = invoiceRows.Where(i => !invoices.Select(i=>i.InvoiceNumber).Contains(i.InvoiceNumber));
                                        //    foreach(var ie in invoiceNumbersNotInDataBaseYet)
                                        //    {
                                        //        var invoice = new Invoice() {InvoiceNumber = ie.InvoiceNumber, InvoiceDate = ie.InvoiceDate };
                                        //        _context.Entry(invoice).State = EntityState.Added;
                                        //        await _context.SaveChangesAsync();
                                        //        var payment = new Payment() { InvoiceID = invoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID =3 , RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
                                        //        _context.Entry(payment).State = EntityState.Added;

                                        //    }
                                        //    //upload the invoice documents ---
                                        //}
                                        else // no invoice in the excel
                                        {
                                            var invoiceRows = requestInvoiceList.Where(i => i.OrderNumber == r.OrderNumber && i.DocumentNumber > 0 && i.InvoiceNumber != "");
                                            if (invoiceRows.Count() == 0)
                                            {
                                                WriteErrorToFile("There is not matching catalog number and order number to row" + lineNumber + " - added default invoice for requestID: " + request.RequestID);

                                                var emptyInvoice = new Invoice() { InvoiceNumber = "000000000", InvoiceDate = r.DateOrdered };
                                                _context.Entry(emptyInvoice).State = EntityState.Added;
                                                await _context.SaveChangesAsync();
                                                var payment = new Payment() { InvoiceID = emptyInvoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
                                                _context.Entry(payment).State = EntityState.Added;
                                                //no docments either....

                                            }
                                            else if (invoiceRows.Count() == 1)
                                            {

                                                WriteErrorToFile("this row went in okay - there was one row for the po # but not matching catlog number");
                                                await SetInvoiceAndPaymentsAccordingToResultsFromDB(r, request, invoiceRows.FirstOrDefault());
                                            }
                                            else if (invoiceRows.Select(ir => ir.InvoiceNumber).Distinct().Count() < 2 && invoiceRows.Select(ir => ir.DocumentNumber).Distinct().Count() < 2)
                                            {
                                                WriteErrorToFile("this row went in okay - there was one row for the po # but not matching catlog number");

                                                await SetInvoiceAndPaymentsAccordingToResultsFromDB(r, request, invoiceRows.FirstOrDefault());
                                            }
                                            else
                                            {
                                                WriteErrorToFile("There are duplicate matching invoices for " + lineNumber + " - added default invoice for requestID: " + request.RequestID);

                                                var emptyInvoice = new Invoice() { InvoiceNumber = "000000000", InvoiceDate = r.DateOrdered };
                                                _context.Entry(emptyInvoice).State = EntityState.Added;
                                                await _context.SaveChangesAsync();
                                                var payment = new Payment() { InvoiceID = emptyInvoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
                                                _context.Entry(payment).State = EntityState.Added;
                                                //no docments either....
                                            }
                                        }
                                        await _context.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var emptyInvoice = new Invoice() { InvoiceNumber = "000000000", InvoiceDate = r.DateOrdered };
                                        _context.Entry(emptyInvoice).State = EntityState.Added;
                                        await _context.SaveChangesAsync();
                                        var payment = new Payment() { InvoiceID = emptyInvoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
                                        _context.Entry(payment).State = EntityState.Added;
                                        //no docments either....
                                        WriteErrorToFile("This item has no order number" + lineNumber + " - added default invoice for requestID: " + request.RequestID);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteErrorToFile("Row " + lineNumber + " failed to enter database: " + AppUtility.GetExceptionMessage(ex));
                                    _context.ChangeTracker.Entries()
                                        .Where(e => e.Entity != null).ToList()
                                        .ForEach(e => e.State = EntityState.Detached);
                                }

                            }
                            catch (Exception ex)
                            {
                                WriteErrorToFile("Error reading invoice file: " + AppUtility.GetExceptionMessage(ex));
                            }
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            WriteErrorToFile("Row " + lineNumber + " failed to enter database: " + AppUtility.GetExceptionMessage(ex));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorToFile("Error reading requests file: " + AppUtility.GetExceptionMessage(ex));
            }
        }

        private async Task SetInvoiceAndPaymentsAccordingToResultsFromDB(UploadExcelModel r, Request request, UploadInvoiceExcelModel invoiceRow)
        {
            string sourceFile = @"C:\Users\debbie\OneDrive - Centarix\Desktop\DocumentsInvoices\" + invoiceRow.DocumentNumber + ".pdf";

            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
            string requestFolderTo = Path.Combine(uploadFolder, request.RequestID.ToString());
            string uploadFolderPathTo = Path.Combine(requestFolderTo, AppUtility.FolderNamesEnum.Invoices.ToString());

            try
            {
                if (!Directory.Exists(requestFolderTo))
                {
                    Directory.CreateDirectory(requestFolderTo);
                    Directory.CreateDirectory(uploadFolderPathTo);
                }
                else
                {
                    if (!Directory.Exists(uploadFolderPathTo))
                    {
                        Directory.CreateDirectory(uploadFolderPathTo);
                    }
                }
                System.IO.File.Copy(sourceFile, uploadFolderPathTo + @"\" + invoiceRow.DocumentNumber + ".pdf", true);
                WriteErrorToFile("file was addeded for request id:" + request.RequestID);
            }
            catch (IOException iox)
            {
                WriteErrorToFile("error adding file" + request.RequestID);
            }
            var invoiceDB = _context.Invoices.Where(i => i.InvoiceNumber == invoiceRow.InvoiceNumber).AsNoTracking().FirstOrDefault();
            if (invoiceDB != null)
            {
                var payment = new Payment() { InvoiceID = invoiceDB.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
                _context.Entry(payment).State = EntityState.Added;
            }
            else
            {
                var invoice = new Invoice() { InvoiceNumber = invoiceRow.InvoiceNumber, InvoiceDate = invoiceRow.InvoiceDate };
                _context.Entry(invoice).State = EntityState.Added;
                await _context.SaveChangesAsync();
                var payment = new Payment() { InvoiceID = invoice.InvoiceID, HasInvoice = true, IsPaid = true, PaymentTypeID = 3, RequestID = request.RequestID, PaymentDate = r.DateOrdered, CompanyAccountID = 5 };
                _context.Entry(payment).State = EntityState.Added;
            }
        }

        private static void WriteErrorToFile(string message)
        {
            var errorFilePath = "InventoryError.txt";
            if (!System.IO.File.Exists(errorFilePath))
            {
                var errorFile = System.IO.File.Create(errorFilePath);
                errorFile.Close();

            }
            var sw = System.IO.File.AppendText(errorFilePath);
            sw.WriteLine("\n" + message);
            sw.Close();
        }
        public async Task MarkInventory()
        {
            //before running this function, run the following in ssms:
            //update requests set IsInInventory = 'false'

            var requests = _context.Requests.IgnoreQueryFilters().Where(r => !r.IsDeleted).Where(r => r.RequestStatusID == 3 && r.OrderType != AppUtility.OrderTypeEnum.Save.ToString());
            var requestsInInventory = requests.OrderByDescending(r => r.ArrivalDate).ToLookup(r => r.ProductID).Select(e => e.First());

            foreach (var r in requestsInInventory)
            {
                r.IsInInventory = true;
                _context.Update(r);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<String> GetUrlFromUserData(String inputtedUrl)
        {
            return AppUtility.GetUrlFromUserData(inputtedUrl);
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _CommentInfoPartialView(int typeID, int index, AppUtility.CommentModelTypeEnum modelType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return await base._CommentInfoPartialView(typeID, index, modelType);
        }

    }
}
