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

        public RequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment hostingEnvironment, ICompositeViewEngine viewEngine /*IHttpContextAccessor Context*/, IHttpContextAccessor httpContextAccessor) : base(context, hostingEnvironment: hostingEnvironment, userManager: userManager)
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
            SetViewModelProprietaryCounts(requestIndexObject, viewmodel);
            //viewmodel.InventoryFilterViewModel = GetInventoryFilterViewModel();

            if (ViewBag.ErrorMessage != null)
            {
                ViewBag.ErrorMessage = ViewBag.ErrorMessage;
            }

            return View(viewmodel);
        }



        [Authorize(Roles = "Requests, LabManagement, Operations")]
        private async Task<RequestIndexPartialViewModelByVendor> GetIndexViewModelByVendor(RequestIndexObject requestIndexObject)
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
            var favoriteIcon = new IconColumnViewModel(" icon-favorite_border-24px", "#5F79E2", "request-favorite", "Favorite");
            var popoverMoreIcon = new IconColumnViewModel("More", "icon-more_vert-24px", "black", "More");
            var popoverPartialClarifyIcon = new IconColumnViewModel("PartialClarify");
            string checkboxString = "Checkbox";
            string buttonText = "";
            var defaultImage = "/images/css/CategoryImages/placeholder.png";
            switch (requestIndexObject.PageType)
            {
                case AppUtility.PageTypeEnum.LabManagementQuotes:
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.Orders:
                            var ordersRequests = _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1).Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.ParentQuote.QuoteStatusID == 4 && r.RequestStatusID == 6)
                                     .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                                     .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.ApplicationUserCreator);

                            iconList.Add(deleteIcon);
                            viewModelByVendor.RequestsByVendor = ordersRequests.OrderByDescending(r => r.CreationDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
                            {
                                TotalCost = (r.Cost ?? 0) + r.VAT,
                                ExchangeRate = r.ExchangeRate,
                                Vendor = r.Product.Vendor,
                                ButtonClasses = " load-terms-modal lab-man-background-color ",
                                ButtonText = "Order",
                                Columns = new List<RequestIndexPartialColumnViewModel>()
                                {
                                     new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                     new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                     new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                     new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                                     new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                     new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                     new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                     new RequestIndexPartialColumnViewModel()
                                     {
                                         Title = "", Width=10, Icons = GetIconsByIndividualRequest(r.RequestID, iconList, false), AjaxID = r.RequestID
                                     }
                                }
                            }).ToLookup(c => c.Vendor);
                            break;
                        case AppUtility.SidebarEnum.Quotes:
                            var quoteRequests = _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1).Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => (r.ParentQuote.QuoteStatusID == 1 || r.ParentQuote.QuoteStatusID == 2) && r.RequestStatusID == 6)
            .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
            .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
            .Include(r => r.ParentQuote).Include(r => r.ApplicationUserCreator);
                            //iconList.Add(resendIcon);
                            iconList.Add(editQuoteDetailsIcon);
                            iconList.Add(deleteIcon);
                            viewModelByVendor.RequestsByVendor = quoteRequests.OrderByDescending(r => r.CreationDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
                            {
                                TotalCost = (r.Cost ?? 0) + r.VAT,
                                ExchangeRate = r.ExchangeRate,
                                Vendor = r.Product.Vendor,
                                ButtonClasses = " confirm-quote lab-man-background-color ",
                                ButtonText = "Ask For Quote",
                                Columns = new List<RequestIndexPartialColumnViewModel>()
                                {
                                     new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                     new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                     new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                     new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                                     new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                     new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                     new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                     new RequestIndexPartialColumnViewModel()
                                     {
                                         Title = "", Width=15, Icons = GetIconsByIndividualRequest(r.RequestID, iconList, true), AjaxID = r.RequestID
                                     }
                                }
                            }).ToLookup(c => c.Vendor);
                            break;
                    }
                    break;
                case AppUtility.PageTypeEnum.AccountingNotifications:

                    var accountingNotificationsList = GetPaymentNotificationRequests(requestIndexObject.SidebarType);
                    iconList.Add(popoverPartialClarifyIcon);
                    string value = "";
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.DidntArrive:
                            checkboxString = "";
                            break;
                        case AppUtility.SidebarEnum.PartialDelivery:
                            value = "PartialClarify";
                            checkboxString = "";
                            break;
                        case AppUtility.SidebarEnum.ForClarification:
                            value = "PartialClarify";
                            checkboxString = "";
                            break;
                        case AppUtility.SidebarEnum.NoInvoice:
                            iconList.Add(addInvoiceIcon);
                            buttonText = "Add To All";
                            break;
                    }
                    viewModelByVendor.RequestsByVendor = accountingNotificationsList.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel()
                    {
                        TotalCost = (r.Cost ?? 0) + r.VAT,
                        ExchangeRate = r.ExchangeRate,
                        Vendor = r.Product.Vendor,
                        ButtonClasses = " invoice-add-all accounting-background-color ",
                        ButtonText = buttonText,
                        Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                            new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Value = new List<string>() { checkboxString }, AjaxID = r.RequestID },
                            new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = r.Product.ProductSubcategory.ImageURL == null ? defaultImage : r.Product.ProductSubcategory.ImageURL },
                            new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = new List<string>() { r.Product.ProductName }, AjaxLink = "load-product-details-summary", AjaxID = r.RequestID },
                            new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                            new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType) },
                            new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price },
                            new RequestIndexPartialColumnViewModel() { Title = "Date", Width = 12, Value = new List<string>() { r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                            new RequestIndexPartialColumnViewModel()
                            {
                                Title = "", Width = 10, Icons = iconList, AjaxID = r.RequestID, Note = AppUtility.GetNote(requestIndexObject.SidebarType, r)
                            }
                        }
                    }).ToLookup(c => c.Vendor);

                    break;
                case AppUtility.PageTypeEnum.AccountingPayments:

                    var paymentList = GetPaymentRequests(requestIndexObject.SidebarType);
                    switch (requestIndexObject.SidebarType)
                    {
                        case AppUtility.SidebarEnum.Installments:
                            payNowIcon = new IconColumnViewModel(" icon-monetization_on-24px green-overlay ", "", "pay-invoice-one", "Pay");
                            checkboxString = "";
                            iconList.Add(payNowIcon);
                            iconList.Add(popoverMoreIcon);
                            var usedPaymentsList = new List<Payment>();
                            var currentPayment = new Payment();
                            viewModelByVendor.RequestsByVendor = paymentList.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel()
                            {
                                TotalCost = (r.Cost ?? 0) + r.VAT,
                                ExchangeRate = r.ExchangeRate,
                                Vendor = r.Product.Vendor,
                                ButtonText = "",
                                Columns = new List<RequestIndexPartialColumnViewModel>()
                                {
                                    new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Value = new List<string>() {checkboxString} , AjaxID = r.RequestID},
                                    new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = r.Product.ProductSubcategory.ImageURL == null ? defaultImage : r.Product.ProductSubcategory.ImageURL },
                                    new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = new List<string>() { r.Product.ProductName }, AjaxLink = "load-product-details-summary", AjaxID = r.RequestID },
                                    new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                                    new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType) },
                                    new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price },
                                    new RequestIndexPartialColumnViewModel() { Title = "Payment Date", Width = 12, Value = new List<string>() { AppUtility.GetCurrentInstallment(usedPaymentsList, r, out currentPayment).PaymentDate.ToString("dd'/'MM'/'yyyy") } },
                                    new RequestIndexPartialColumnViewModel()
                                    {
                                        Title = "", Width = 10, Icons = iconList, AjaxID = currentPayment.PaymentID
                                    }
                                }
                            }).ToLookup(c => c.Vendor);
                            break;
                        default:
                            iconList.Add(payNowIcon);
                            iconList.Add(popoverMoreIcon);
                            viewModelByVendor.RequestsByVendor = paymentList.OrderByDescending(r => r.ParentRequest.OrderDate).Select(r => new RequestIndexPartialRowViewModel()
                            {
                                TotalCost = (r.Cost ?? 0) + r.VAT,
                                ExchangeRate = r.ExchangeRate,
                                Vendor = r.Product.Vendor,
                                ButtonClasses = " payments-pay-now accounting-background-color ",
                                ButtonText = "Pay All",
                                Columns = new List<RequestIndexPartialColumnViewModel>()
                                {
                                    new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Value = new List<string>() {checkboxString} , AjaxID = r.RequestID},
                                    new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = r.Product.ProductSubcategory.ImageURL == null ? defaultImage : r.Product.ProductSubcategory.ImageURL },
                                    new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width = 15, Value = new List<string>() { r.Product.ProductName }, AjaxLink = "load-product-details-summary", AjaxID = r.RequestID },
                                    new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                                    new RequestIndexPartialColumnViewModel() { Title = "Amount", Width = 10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType) },
                                    new RequestIndexPartialColumnViewModel() { Title = "Price", Width = 10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum = AppUtility.FilterEnum.Price },
                                    new RequestIndexPartialColumnViewModel() { Title = "Date", Width = 12, Value = new List<string>() { r.ParentRequest.OrderDate.ToString("dd'/'MM'/'yyyy") } },
                                    new RequestIndexPartialColumnViewModel()
                                    {
                                        Title = "", Width = 10, Icons = iconList, AjaxID = r.RequestID
                                    }
                                }
                            }).ToLookup(c => c.Vendor);
                            buttonText = "Pay All";
                            break;
                    }


                    break;
                case AppUtility.PageTypeEnum.RequestCart:
                    var cartRequests = _context.Requests
              .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
              .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
              .Include(r => r.ApplicationUserCreator)
              .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
              .Where(r => r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.AddToCart.ToString())
              .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1);

                    iconList.Add(deleteIcon);
                    viewModelByVendor.RequestsByVendor = cartRequests.OrderByDescending(r => r.CreationDate).Select(r => new RequestIndexPartialRowViewModel()
                    {

                        Vendor = r.Product.Vendor,
                        TotalCost = (r.Cost ?? 0) + r.VAT,
                        ExchangeRate = r.ExchangeRate,
                        ButtonClasses = " load-terms-modal order-inv-background-color ",
                        ButtonText = "Order",
                        Columns = new List<RequestIndexPartialColumnViewModel>()
                        {
                                new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                                new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r,  requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                new RequestIndexPartialColumnViewModel()
                                {
                                    Title = "", Width=10, Icons = iconList, AjaxID = r.RequestID,
                                },

                        }
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

        private static async Task<IPagedList<RequestIndexPartialRowViewModel>> GetForApprovalRows(RequestIndexObject requestIndexObject, IPagedList<RequestIndexPartialRowViewModel> onePageOfProducts, IQueryable<Request> RequestPassedInWithInclude, List<IconColumnViewModel> iconList, string defaultImage)
        {
            onePageOfProducts = await RequestPassedInWithInclude.OrderByDescending(r => r.CreationDate).ToList().Select(r => new RequestIndexPartialRowViewModel()
            {
                Columns = new List<RequestIndexPartialColumnViewModel>()
                            {
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Image = r.Product.ProductSubcategory.ImageURL==null?defaultImage: r.Product.ProductSubcategory.ImageURL},
                                 new RequestIndexPartialColumnViewModel() { Title = "Item Name", Width=15, Value = new List<string>(){ r.Product.ProductName}, AjaxLink = "load-product-details", AjaxID=r.RequestID},
                                 new RequestIndexPartialColumnViewModel() { Title = "Vendor", Width=10, Value = new List<string>(){ r.Product.Vendor.VendorEnName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Amount", Width=10, Value = AppUtility.GetAmountColumn(r, r.UnitType, r.SubUnitType, r.SubSubUnitType)},
                                 new RequestIndexPartialColumnViewModel() { Title = "Category", Width=11, Value = AppUtility.GetCategoryColumn(requestIndexObject.CategorySelected, requestIndexObject.SubcategorySelected, r.Product.ProductSubcategory), FilterEnum=AppUtility.FilterEnum.Category },
                                 new RequestIndexPartialColumnViewModel() { Title = "Owner", Width=12, Value = new List<string>(){r.ApplicationUserCreator.FirstName + " " + r.ApplicationUserCreator.LastName} },
                                 new RequestIndexPartialColumnViewModel() { Title = "Price", Width=10, Value = AppUtility.GetPriceColumn(requestIndexObject.SelectedPriceSort, r, requestIndexObject.SelectedCurrency), FilterEnum=AppUtility.FilterEnum.Price},
                                 new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width=12, Value = new List<string>(){ r.CreationDate.ToString("dd'/'MM'/'yyyy") } },
                                 new RequestIndexPartialColumnViewModel() { Title = "", Width=10, Icons = iconList,  AjaxID = r.RequestID },
                                 new RequestIndexPartialColumnViewModel() { Width=0, AjaxLink = " d-none order-type"+r.RequestID, AjaxID = (int)Enum.Parse(typeof(AppUtility.OrderTypeEnum),r.OrderType), Value = new List<string>(){r.OrderType.ToString()} }
                            }
            }).ToPagedListAsync(requestIndexObject.PageNumber == 0 ? 1 : requestIndexObject.PageNumber, 25);
            return onePageOfProducts;
        }




        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AccountingGeneral(RequestIndexObject requestIndexObject)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = requestIndexObject.PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = requestIndexObject.SidebarType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = requestIndexObject.SectionType;
            RequestIndexPartialViewModel requestIndexPartialViewModel = await GetIndexViewModel(requestIndexObject, Years: new List<int>() { DateTime.Now.Year });
            AccountingGeneralViewModel viewModel = new AccountingGeneralViewModel() { RequestIndexPartialViewModel = requestIndexPartialViewModel };
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
            SetViewModelProprietaryCounts(requestIndexObject, viewModel);
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
            SetViewModelProprietaryCounts(requestIndexObject, viewModel);
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
            RequestIndexPartialViewModel viewModel = await base.GetSharedRequestIndexObjectAsync();
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
            SetViewModelProprietaryCounts(requestIndexObject, viewModel);
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
        public async Task<IActionResult> DeleteModal(DeleteRequestViewModel deleteRequestViewModel)
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
                        foreach (var requestLocationInstance in request.RequestLocationInstances)
                        {
                            requestLocationInstance.IsDeleted = true;
                            var locationInstance = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstance.LocationInstanceID).FirstOrDefault();
                            locationInstance.IsFull = false;

                            _context.Update(requestLocationInstance);
                            await _context.SaveChangesAsync();
                            _context.Update(locationInstance);
                            await _context.SaveChangesAsync();
                        }
                        var comments = _context.Comments.Where(c => c.RequestID == request.RequestID);
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
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
                if (deleteRequestViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.LabManagementQuotes)
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
                }
            }
            catch (Exception ex)
            {
                deleteRequestViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                if (deleteRequestViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.LabManagementQuotes)
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
                else
                {
                    return RedirectToAction("Index", deleteRequestViewModel.RequestIndexObject);
                }
            }

        }

        [HttpGet]
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

            requestItemViewModel.PageType = PageType;
            requestItemViewModel.SectionType = SectionType;
            RemoveRequestWithCommentsAndEmailSessions();
            return View(requestItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> AddItemView(RequestItemViewModel requestItemViewModel, AppUtility.OrderTypeEnum OrderType, ReceivedModalVisualViewModel receivedModalVisualViewModel = null)
        {
            var redirectToActionResult = base.SaveAddItemView(requestItemViewModel, OrderType, receivedModalVisualViewModel).Result;
            var x = 1;
            return RedirectToAction(redirectToActionResult.ActionName, redirectToActionResult.ControllerName, redirectToActionResult.RouteValues);
        }


        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> CreateItemTabs(int productSubCategoryId, AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest, string itemName = "", bool isRequestQuote = false)
        {
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

            requestItemViewModel.SectionType = sectionType;
            requestItemViewModel.PageType = PageType;
            requestItemViewModel.Requests.FirstOrDefault().Product.ProductName = itemName;
            requestItemViewModel.IsRequestQuote = isRequestQuote;

            //TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.RequestPageTypeEnum.Request;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;

            return PartialView(requestItemViewModel);
        }
        private async Task<RequestItemViewModel> FillRequestDropdowns(RequestItemViewModel requestItemViewModel, ProductSubcategory productSubcategory, int categoryTypeId)
        {
            var parentcategories = new List<ParentCategory>();
            var productsubcategories = new List<ProductSubcategory>();
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            if (productSubcategory != null)
            {
                if (categoryTypeId == 1)
                {
                    parentcategories = await _context.ParentCategories.Where(pc => pc.ParentCategoryID == productSubcategory.ParentCategoryID).ToListAsync();
                }
                else
                {
                    parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 2).ToListAsync();
                }
                productsubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == productSubcategory.ParentCategoryID).ToListAsync();
                unittypes = _context.UnitTypes.Where(ut => ut.UnitTypeParentCategory.Where(up => up.ParentCategoryID == productSubcategory.ParentCategoryID).Count() > 0).Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            }
            else
            {
                if (requestItemViewModel.IsProprietary)
                {
                    var proprietarycategory = await _context.ParentCategories.Where(pc => pc.ParentCategoryDescription == AppUtility.ParentCategoryEnum.Samples.ToString()).FirstOrDefaultAsync();
                    productsubcategories = await _context.ProductSubcategories.Where(ps => ps.ParentCategoryID == proprietarycategory.ParentCategoryID).ToListAsync();
                }
                else
                {
                    parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == categoryTypeId && !pc.IsProprietary).ToListAsync();
                }
            }
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == categoryTypeId).Count() > 0).ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var subprojects = await _context.SubProjects.ToListAsync();
            var unittypeslookup = unittypes.ToLookup(u => u.UnitParentType);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();

            requestItemViewModel.ParentCategories = parentcategories;
            requestItemViewModel.ProductSubcategories = productsubcategories;
            requestItemViewModel.Vendors = vendors;
            requestItemViewModel.Projects = projects;
            requestItemViewModel.SubProjects = subprojects;

            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");
            requestItemViewModel.UnitTypes = unittypeslookup;
            requestItemViewModel.CommentTypes = commentTypes;
            requestItemViewModel.PaymentTypes = paymenttypes;
            requestItemViewModel.CompanyAccounts = companyaccounts;
            return requestItemViewModel;
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

            requestItemViewModel.Comments = new List<Comment>();
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
            FillDocumentsInfo(requestItemViewModel, "", productSubcategory);
            /* if (productSubcategory.ProductSubcategoryDescription == "Blood" || productSubcategory.ProductSubcategoryDescription == "Serum")
             {
                 GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.S, "");

             }
             if (productSubcategory.ProductSubcategoryDescription != "Blood" && productSubcategory.ProductSubcategoryDescription != "Serum"
                 && productSubcategory.ProductSubcategoryDescription != "Cells")
             {
                 GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Info, "");

             }
             if (productSubcategory.ProductSubcategoryDescription != "Blood" && productSubcategory.ProductSubcategoryDescription != "Serum"
                 && productSubcategory.ProductSubcategoryDescription != "Cells" && productSubcategory.ProductSubcategoryDescription != "Probes")
             {
                 GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Map, "");

             }
         }
         else if(requestItemViewModel.ParentCategories.FirstOrDefault().CategoryTypeID==2)
         {
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Orders, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Invoices, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Details, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Quotes, "");
         }
         else
         {
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Orders, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Invoices, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Shipments, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Quotes, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Info, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Pictures, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Returns, "");
             GetExistingFileStrings(requestItemViewModel, AppUtility.FolderNamesEnum.Credits, "");
         }
    */
            DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests);
            return requestItemViewModel;
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _PartialItemOperationsTab(int index, int subcategoryID = 0)
        {
            var operationsItemViewModel = new OperationsItemViewModel()
            {
                RequestIndex = index,
                ModalType = AppUtility.RequestModalType.Create,
                ParentCategories = _context.ParentCategories.Where(pc => pc.CategoryTypeID == 2).ToList(),
                ProductSubcategories = new List<ProductSubcategory>()
            };
            if (subcategoryID > 0)
            {
                operationsItemViewModel.Request = new Request();
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
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var favoriteRequest = new FavoriteRequest()
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
                    return RedirectToAction("_IndexTable", requestIndexObject);
                }
            }
            return new EmptyResult();
        }

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ShareModal(int ID, AppUtility.ModelsEnum ModelsEnum)
        {
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
                                TimeStamp = DateTime.Now
                            };
                        }
                        else
                        {
                            sharedRequest.TimeStamp = DateTime.Now;
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
            var requestItemViewModel = await editModalViewFunction(id, Tab, SectionType, isEditable);
            return PartialView(requestItemViewModel);
        }


        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> EditModalView(int? id, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, bool isEditable = true, List<string> selectedPriceSort = null, bool isProprietary = false)
        {
            selectedPriceSort = selectedPriceSort.Count == 0 ? new List<string>() { AppUtility.PriceSortEnum.Unit.ToString(), AppUtility.PriceSortEnum.TotalVat.ToString() } : selectedPriceSort;
            var requestItemViewModel = await editModalViewFunction(id, 0, SectionType, isEditable, selectedPriceSort, isProprietary: isProprietary);
            return PartialView(requestItemViewModel);
        }

        [Authorize(Roles = "Requests")]
        public async Task<RequestItemViewModel> editModalViewFunction(int? id, int? Tab = 0, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests,
            bool isEditable = true, List<string> selectedPriceSort = null, string selectedCurrency = null, bool isProprietary = false)
        {

            var categoryType = 1;
            if (SectionType == AppUtility.MenuItems.Operations)
            {
                categoryType = 2;
            }

            string ModalViewType = "";
            if (id == null)
            {
                return null;
            }

            var productId = _context.Requests.Where(r => r.RequestID == id).Select(r => r.ProductID).FirstOrDefault();

            var request = _context.Requests.Include(r => r.Product)
                .Include(r => r.ParentQuote)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product.ProductSubcategory)
                .Include(r => r.Product.ProductSubcategory.ParentCategory)
                .Include(r => r.Product.Vendor)
                .Include(r => r.RequestStatus)
                .Include(r => r.ApplicationUserCreator).Include(r => r.PaymentStatus)
                .Include(r => r.Payments).ThenInclude(p => p.CompanyAccount)
                .Include(r => r.Payments).ThenInclude(p => p.CreditCard)
                .Include(r => r.Payments).ThenInclude(p => p.Invoice)
                .Include(r => r.ApplicationUserReceiver)
                //.Include(r => r.Payments) //do we have to have a separate list of payments to include thefix c inside things (like company account and payment types?)
                .SingleOrDefault(x => x.RequestID == id);

            var requestsByProduct = _context.Requests.Where(r => r.ProductID == productId && (r.RequestStatusID == 3))
                 .Include(r => r.Product.ProductSubcategory).Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .Include(r => r.ApplicationUserCreator) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .Include(r => r.ParentRequest)
                    .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType).Include(r => r.ApplicationUserReceiver)
                    .OrderByDescending(r => r.CreationDate)
                    .ToList();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel();
            await FillRequestDropdowns(requestItemViewModel, request.Product.ProductSubcategory, categoryType);

            requestItemViewModel.Tab = Tab ?? 0;
            requestItemViewModel.Comments = await _context.Comments
                .Include(r => r.ApplicationUser)
                .Where(r => r.Request.RequestID == id).ToListAsync();
            requestItemViewModel.SectionType = SectionType;
            requestItemViewModel.RequestsByProduct = requestsByProduct;
            requestItemViewModel.Requests = new List<Request>();
            if (isEditable)
            {
                requestItemViewModel.ModalType = AppUtility.RequestModalType.Edit;
            }
            else
            {
                requestItemViewModel.ModalType = AppUtility.RequestModalType.Summary;
            }

            ModalViewType = "Edit";
            requestItemViewModel.Requests.Add(request);

            //load the correct list of subprojects
            //var subprojects = await _context.SubProjects
            //    .Where(sp => sp.ProjectID == requestItemViewModel.Request.SubProject.ProjectID)
            //    .ToListAsync();
            //requestItemViewModel.SubProjects = subprojects;
            //may be able to do this together - combining the path for the orders folders
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
            string uploadFolder2 = Path.Combine(uploadFolder1, requestItemViewModel.Requests.FirstOrDefault().RequestID.ToString());
            requestItemViewModel.DocumentsInfo = new List<DocumentFolder>();

            //the partial file name that we will search for (1- because we want the first one)
            //creating the directory from the path made earlier
            var productSubcategory = requestItemViewModel.Requests.FirstOrDefault().Product.ProductSubcategory;

            FillDocumentsInfo(requestItemViewModel, uploadFolder2, productSubcategory);

            //locations:
            //get the list of requestLocationInstances in this request
            //can't look for _context.RequestLocationInstances b/c it's a join table and doesn't have a dbset
            var request1 = _context.Requests.Where(r => r.RequestID == id).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).FirstOrDefault();
            var requestLocationInstances = request1.RequestLocationInstances.ToList();
            //if it has => (which it should once its in a details view)
            requestItemViewModel.LocationInstances = new List<LocationInstance>();
            requestLocationInstances.ForEach(rli => requestItemViewModel.LocationInstances.Add(rli.LocationInstance));
            if (request1.RequestStatusID == 3 || request1.RequestStatusID == 5 || request1.RequestStatusID == 4 || request1.RequestStatusID == 7)
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
                    //int? locationInstanceParentID = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstanceID).FirstOrDefault().LocationInstanceParentID;
                    LocationInstance parentLocationInstance = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).Include(li => li.LocationType).FirstOrDefault();
                    //requestItemViewModel.ParentLocationInstance = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                    //need to test b/c the model is int? which is nullable

                    if (parentLocationInstance == null)
                    {
                        var locationType = _context.TemporaryLocationInstances.Where(l => l.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceID).Select(li => li.LocationType).FirstOrDefault();
                        requestItemViewModel.ReceivedLocationViewModel = receivedLocationViewModel;
                        ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel = new ReceivedModalSublocationsViewModel()
                        {
                            locationInstancesDepthZero = new List<LocationInstance>(),
                            locationTypeNames = new List<string>(),
                            locationInstancesSelected = _context.LocationInstances.Where(l => l.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceID).ToList()

                        };
                        requestItemViewModel.ReceivedModalSublocationsViewModel = receivedModalSublocationsViewModel;
                        requestItemViewModel.ChildrenLocationInstances = new List<List<LocationInstance>>();
                        requestItemViewModel.ChildrenLocationInstances.Add(receivedModalSublocationsViewModel.locationInstancesSelected);

                        requestItemViewModel.ParentDepthZeroOfSelected = locationType;
                        //requestItemViewModel.ReceivedModalVisualViewModel = receivedModalVisualViewModel;
                    }
                    else
                    {
                        var locationType = parentLocationInstance.LocationType;
                        while (locationType.Depth != 0)
                        {
                            locationType = _context.LocationTypes.Where(l => l.LocationTypeID == locationType.LocationTypeParentID).FirstOrDefault();
                        }

                        receivedLocationViewModel.locationInstancesSelected.Add(parentLocationInstance);
                        requestItemViewModel.ParentDepthZeroOfSelected = locationType;
                        requestItemViewModel.ReceivedLocationViewModel = receivedLocationViewModel;

                        ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel = new ReceivedModalSublocationsViewModel()
                        {
                            locationInstancesDepthZero = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationTypeID == locationType.LocationTypeID),
                            locationTypeNames = new List<string>(),
                            locationInstancesSelected = new List<LocationInstance>()
                        };
                        bool finished = false;
                        int locationTypeIDLoop = locationType.LocationTypeID;
                        var parent = parentLocationInstance;
                        receivedModalSublocationsViewModel.locationInstancesSelected.Add(parent);
                        requestItemViewModel.ChildrenLocationInstances = new List<List<LocationInstance>>();
                        requestItemViewModel.ChildrenLocationInstances.Add(_context.LocationInstances.OfType<LocationInstance>().Where(l => l.LocationInstanceParentID == parent.LocationInstanceParentID).ToList());
                        while (parent.LocationInstanceParentID != null)
                        {
                            parent = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceID == parent.LocationInstanceParentID).FirstOrDefault();
                            requestItemViewModel.ChildrenLocationInstances.Add(_context.LocationInstances.OfType<LocationInstance>().Where(l => l.LocationInstanceParentID == parent.LocationInstanceParentID).ToList());
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
                            ParentLocationInstance = _context.LocationInstances.OfType<LocationInstance>().Where(m => m.LocationInstanceID == parentLocationInstance.LocationInstanceID).FirstOrDefault()
                        };

                        if (receivedModalVisualViewModel.ParentLocationInstance != null)
                        {
                            receivedModalVisualViewModel.RequestChildrenLocationInstances =
                                _context.LocationInstances.OfType<LocationInstance>().Where(m => m.LocationInstanceParentID == parentLocationInstance.LocationInstanceID)
                                .Include(m => m.RequestLocationInstances)
                                .Select(li => new RequestChildrenLocationInstances()
                                {
                                    LocationInstance = li,
                                    IsThisRequest = li.RequestLocationInstances.Select(rli => rli.RequestID).Where(i => i == id).Any()
                                }).OrderBy(m => m.LocationInstance.LocationNumber).ToList();

                            List<LocationInstancePlace> liPlaces = new List<LocationInstancePlace>();
                            foreach (var cli in receivedModalVisualViewModel.RequestChildrenLocationInstances)
                            {
                                liPlaces.Add(new LocationInstancePlace()
                                {
                                    LocationInstanceId = cli.LocationInstance.LocationInstanceID,
                                    Placed = cli.IsThisRequest
                                });
                            }
                            receivedModalVisualViewModel.LocationInstancePlaces = liPlaces;
                            //return NotFound();
                        }
                        requestItemViewModel.ReceivedModalVisualViewModel = receivedModalVisualViewModel;
                    }

                }


            }
            if (selectedPriceSort != null && !isProprietary)
            {
                requestItemViewModel.PricePopoverViewModel = new PricePopoverViewModel();
                List<PriceSortViewModel> priceSorts = new List<PriceSortViewModel>();
                Enum.GetValues(typeof(AppUtility.PriceSortEnum)).Cast<AppUtility.PriceSortEnum>().ToList().ForEach(p => priceSorts.Add(new PriceSortViewModel { PriceSortEnum = p, Selected = selectedPriceSort.Contains(p.ToString()) }));
                requestItemViewModel.PricePopoverViewModel.PriceSortEnums = priceSorts;
                requestItemViewModel.PricePopoverViewModel.SelectedCurrency = (AppUtility.CurrencyEnum)Enum.Parse(typeof(AppUtility.CurrencyEnum), selectedCurrency ?? requestItemViewModel.Requests[0].Currency);
                requestItemViewModel.PricePopoverViewModel.PopoverSource = 2;
            }
            requestItemViewModel.IsProprietary = isProprietary;

            if (requestItemViewModel.Requests.FirstOrDefault() == null)
            {
                TempData["InnerMessage"] = "The request sent in was null";
            }

            ViewData["ModalViewType"] = ModalViewType;
            //ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", addNewItemViewModel.Request.ParentRequest.ApplicationUserID);
            //ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", addNewItemViewModel.Request.ProductID);
            //ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", addNewItemViewModel.Request.RequestStatusID);
            return requestItemViewModel;


        }

        private void FillDocumentsInfo(RequestItemViewModel requestItemViewModel, string uploadFolder, ProductSubcategory productSubcategory)
        {
            requestItemViewModel.DocumentsInfo = new List<DocumentFolder>();

            if (productSubcategory.ParentCategory.IsProprietary)
            {
                if (productSubcategory.ProductSubcategoryDescription == "Blood" || productSubcategory.ProductSubcategoryDescription == "Serum")
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.S, uploadFolder);

                }
                if (productSubcategory.ProductSubcategoryDescription != "Blood" && productSubcategory.ProductSubcategoryDescription != "Serum"
                    && productSubcategory.ProductSubcategoryDescription != "Cells")
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, uploadFolder);

                }
                if (productSubcategory.ProductSubcategoryDescription != "Blood" && productSubcategory.ProductSubcategoryDescription != "Serum"
                    && productSubcategory.ProductSubcategoryDescription != "Cells" && productSubcategory.ProductSubcategoryDescription != "Probes")
                {
                    GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Map, uploadFolder);

                }
            }
            else if (requestItemViewModel.ParentCategories.FirstOrDefault().CategoryTypeID == 2)
            {
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Orders, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Invoices, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Details, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Quotes, uploadFolder);
            }
            else
            {
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Quotes, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Orders, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Invoices, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Shipments, uploadFolder);

                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Info, uploadFolder);
                GetExistingFileStrings(requestItemViewModel.DocumentsInfo, AppUtility.FolderNamesEnum.Pictures, uploadFolder);
                //GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Returns, uploadFolder);
                //GetExistingFileStrings(requestItemViewModel, AppUtility.RequestFolderNamesEnum.Credits, uploadFolder);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                    var product = _context.Products.Include(p => p.Vendor).Include(p => p.ProductSubcategory).FirstOrDefault(v => v.ProductID == request.ProductID);
                    // product.ProductSubcategoryID = requestItemViewModel.Request.Product.ProductSubcategoryID;
                    product.VendorID = request.Product.VendorID;
                    product.CatalogNumber = request.Product.CatalogNumber;
                    //in case we need to return to the modal view
                    product.ProductName = request.Product.ProductName;
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
                        request.Product.ProductSubcategoryID = request.Product.ProductSubcategory.ProductSubcategoryID;
                        // requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;
                        request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == request.SubProjectID).FirstOrDefault();

                        //_context.Update(requestItemViewModel.Request.Product.SubProject);
                        //_context.Update(requestItemViewModel.Request.Product);
                        if (request.ParentQuote != null)
                        {
                            _context.Update(request.ParentQuote);
                            await _context.SaveChangesAsync();
                            request.ParentQuoteID = request.ParentQuote.ParentQuoteID;
                        }
                        _context.Update(request);
                        await _context.SaveChangesAsync();


                        if (requestItemViewModel.Comments != null)
                        {

                            foreach (var comment in requestItemViewModel.Comments)
                            {
                                if (!String.IsNullOrEmpty(comment.CommentText))
                                {
                                    //save the new comment
                                    comment.ApplicationUserID = currentUser.Id;
                                    comment.CommentTimeStamp = DateTime.Now;
                                    comment.RequestID = request.RequestID;
                                    _context.Update(comment);
                                }
                            }
                            await _context.SaveChangesAsync();
                        }

                        if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                        {
                            var requestLocations = _context.Requests.Where(r => r.RequestID == request.RequestID).Include(r => r.RequestLocationInstances).FirstOrDefault().RequestLocationInstances;
                            var isTemporary = false;
                            foreach (var location in requestLocations)
                            {
                                var locationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == location.LocationInstanceID).FirstOrDefault();
                                if (locationInstance is TemporaryLocationInstance)
                                {
                                    isTemporary = true;
                                }
                                else
                                {
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
                            }
                            if (!isTemporary)
                            {
                                await base.SaveLocations(receivedModalVisualViewModel, request);
                            }
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
                    string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
                    string uploadFolder2 = Path.Combine(uploadFolder1, requestItemViewModel.Requests[0].RequestID.ToString());
                    FillDocumentsInfo(requestItemViewModel, uploadFolder2, productSubcategory);
                    requestItemViewModel.Comments = await _context.Comments.Include(r => r.ApplicationUser).Where(r => r.Request.RequestID == requestItemViewModel.Requests[0].RequestID).ToListAsync();
                    requestItemViewModel.ModalType = AppUtility.RequestModalType.Edit;
                    Response.StatusCode = 550;
                    return PartialView(requestItemViewModel);
                }
            }
        }

        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(RequestIndexObject requestIndexObject, int? id, String SectionType = "")
        {
            DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests);
            base.RemoveRequestWithCommentsAndEmailSessions();
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            Request request = _context.Requests
                .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory)
                .Include(r => r.UnitType)
                .Include(r => r.SubUnitType)
                .Include(r => r.SubSubUnitType)
                .SingleOrDefault(x => x.RequestID == id);

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
            };
            requestItemViewModel.Requests = new List<Request>() { request };
            var reorderViewModel = new ReorderViewModel() { RequestIndexObject = requestIndexObject, RequestItemViewModel = requestItemViewModel };
            return PartialView(reorderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReOrderFloatModalView(ReorderViewModel reorderViewModel, AppUtility.OrderTypeEnum OrderTypeEnum, bool isCancel = false)
        {
            if (isCancel)
            {
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests);
                return PartialView("Default");
            }
            else
            {
                try
                {
                    //  ReorderViewModel reorderViewModel = JsonConvert.DeserializeObject<ReorderViewModel>(json);
                    //get the old request that we are reordering
                    var oldRequest = _context.Requests.Where(r => r.RequestID == reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().RequestID)
                        .Include(r => r.Product)
                        .ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.Product.Vendor).FirstOrDefault();


                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
                    //need to include product to check if in budget
                    //   reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().Product = oldRequest.Product;

                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().RequestID = 0;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().ProductID = oldRequest.ProductID;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().ApplicationUserCreatorID = currentUser.Id;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().CreationDate = DateTime.Now;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().SubProjectID = oldRequest.SubProjectID;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().Product = oldRequest.Product;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().Product.SerialNumber = oldRequest.Product.SerialNumber;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().URL = oldRequest.URL;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().Warranty = oldRequest.Warranty;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().ExchangeRate = oldRequest.ExchangeRate;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().Currency = oldRequest.Currency;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().IncludeVAT = oldRequest.IncludeVAT;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().UnitTypeID = oldRequest.UnitTypeID;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().SubUnitTypeID = oldRequest.SubUnitTypeID;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().SubSubUnitTypeID = oldRequest.SubSubUnitTypeID;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().SubUnit = oldRequest.SubUnit;
                    reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().SubSubUnit = oldRequest.SubSubUnit;
                    var isInBudget = checkIfInBudget(reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault(), oldRequest.Product);
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {

                            await AddItemAccordingToOrderType(reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault(), OrderTypeEnum, isInBudget);
                            var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                            var isSavedUsingSession = HttpContext.Session.GetObject<Request>(requestName) != null;
                            if (!isSavedUsingSession)
                            {
                                MoveDocumentsOutOfTempFolder(reorderViewModel.RequestItemViewModel.Requests.FirstOrDefault().RequestID, AppUtility.ParentFolderName.Requests);
                                await transaction.CommitAsync();
                                base.RemoveRequestWithCommentsAndEmailSessions();
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            base.RemoveRequestWithCommentsAndEmailSessions();
                            throw ex;
                        }
                    }

                    var action = reorderViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestSummary ? "IndexInventory" : "Index";
                    switch (OrderTypeEnum)
                    {
                        case AppUtility.OrderTypeEnum.AlreadyPurchased:
                            action = "UploadOrderModal";
                            break;
                        case AppUtility.OrderTypeEnum.OrderNow:
                        case AppUtility.OrderTypeEnum.AddToCart:
                            action = "UploadQuoteModal";
                            break;
                    }
                    reorderViewModel.RequestIndexObject.OrderType = OrderTypeEnum;
                    reorderViewModel.RequestIndexObject.IsReorder = true;
                    return RedirectToAction(action, "Requests", reorderViewModel.RequestIndexObject);
                }
                catch (Exception ex)
                {
                    reorderViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    Response.StatusCode = 500;
                    var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
                    reorderViewModel.RequestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");
                    return PartialView("ReOrderFloatModalView", reorderViewModel);
                }
            }
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
            //var prs = _context.ParentRequests;
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
                    request.PaymentStatus = _context.PaymentStatuses.Where(ps => ps.PaymentStatusID == request.PaymentStatusID).FirstOrDefault();
                    if (request.ParentRequest != null)
                    {
                        pr.Shipping = request.ParentRequest.Shipping;
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
            string renderedView = await RenderPartialViewToString("OrderEmailView", confirm);
            //instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            PdfDocument doc = new PdfDocument();
            // create a new pdf document converting an url
            doc = converter.ConvertHtmlString(renderedView, baseUrl);

            //save this as orderform
            string path1 = Path.Combine("wwwroot", AppUtility.ParentFolderName.Requests.ToString());
            string fileName = Path.Combine(path1, "OrderForm.pdf");
            doc.Save(fileName);
            doc.Close();
            confirm.RequestIndexObject = requestIndexObject;
            return PartialView(confirm);
        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmEmailModal(ConfirmEmailViewModel confirmEmailViewModel)
        {
            try
            {
                string uploadFolder = Path.Combine("wwwroot", AppUtility.ParentFolderName.Requests.ToString());
                string uploadFile = Path.Combine(uploadFolder, "OrderForm.pdf");

                var isRequests = true;
                var RequestNum = 1;
                var PaymentNum = 1;
                var requests = new List<Request>();
                var payments = new List<Payment>();
                while (isRequests)
                {
                    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;

                    if (HttpContext.Session.GetObject<Request>(requestName) != null)
                    {
                        var request = HttpContext.Session.GetObject<Request>(requestName);
                        requests.Add(request);
                        for (int i = 0; i < request.Installments; i++)
                        {
                            var paymentName = AppData.SessionExtensions.SessionNames.Payment.ToString() + (PaymentNum);
                            var payment = HttpContext.Session.GetObject<Payment>(paymentName);
                            payment.Request = request;
                            payments.Add(payment);
                            PaymentNum++;
                        }
                        //if (request.PaymentStatusID == 7)
                        //{
                        //    var paymentName = AppData.SessionExtensions.SessionNames.Payment.ToString() + 1;
                        //    var payment = HttpContext.Session.GetObject<Payment>(paymentName);
                        //    payment.Request = request;
                        //    payments.Add(payment);
                        //}
                    }
                    else
                    {
                        isRequests = false;
                    }
                    RequestNum++;
                }
                var action = "Index";
                if (confirmEmailViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestSummary)
                {
                    action = "IndexInventory";
                }
                else if (requests.FirstOrDefault().OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString())
                {
                    action = "LabManageOrders";
                }
                else if (requests.FirstOrDefault().OrderType == AppUtility.OrderTypeEnum.AddToCart.ToString())
                {
                    action = "Cart";
                }
                var isEmail = true;
                var emailNum = 1;
                var emails = new List<string>();
                while (isEmail)
                {
                    var emailName = AppData.SessionExtensions.SessionNames.Email.ToString() + emailNum;
                    var email = HttpContext.Session.GetObject<string>(emailName);
                    if (email != null)
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

                    var userId = requests.FirstOrDefault().ApplicationUserCreatorID ?? _userManager.GetUserId(User); //do we need to do this? (will it ever be null?)
                                                                                                                     //var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
                    var currentUser = _context.Users.FirstOrDefault(u => u.Id == userId);
                    //var users = _context.Users.ToList();
                    //currentUser = _context.Users.Where(u => u.Id == "702fe06c-22e1-4be8-a515-ea89d6e5ee00").FirstOrDefault();
                    string ownerEmail = currentUser.Email;
                    string ownerUsername = currentUser.FirstName + " " + currentUser.LastName;
                    string ownerPassword = currentUser.SecureAppPass;
                    string vendorEmail = /*firstRequest.Product.Vendor.OrdersEmail;*/ emails.Count() < 1 ? requests.FirstOrDefault().Product.Vendor.OrdersEmail : emails[0];
                    string vendorName = requests.FirstOrDefault().Product.Vendor.VendorEnName;

                    //add a "From" Email
                    message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

                    // add a "To" Email

                    message.To.Add(new MailboxAddress(vendorName, vendorEmail));
                    if (emails.Count >= 2)
                    {
                        message.Cc.Add(new MailboxAddress(emails[1]));
                    }
                    if (emails.Count >= 3)
                    {
                        message.Cc.Add(new MailboxAddress(emails[2]));
                    }
                    if (emails.Count >= 4)
                    {
                        message.Cc.Add(new MailboxAddress(emails[3]));
                    }
                    if (emails.Count >= 5)
                    {
                        message.Cc.Add(new MailboxAddress(emails[4]));
                    }
                    //add CC's to email





                    //subject
                    message.Subject = "Order from Centarix to " + vendorName;

                    //body
                    builder.TextBody = @"Hello," + "\n\n" + "Please see the attached order. \n\nThank you.\n"
                        + ownerUsername + "\nCentarix";
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
                                    throw ex;
                                }
                                client.Disconnect(true);

                                if (wasSent)
                                {
                                    foreach (var r in requests)
                                    {
                                        r.RequestStatusID = 2;
                                        if (r.RequestID == 0)
                                        {
                                            if (r.Product.ProductID == 0)
                                            {
                                                _context.Entry(r.Product).State = EntityState.Added;
                                            }
                                            _context.Entry(r).State = EntityState.Added;
                                            _context.Entry(r.ParentRequest).State = EntityState.Added;
                                            _context.Entry(r.ParentQuote).State = EntityState.Added;
                                        }
                                        else
                                        {
                                            _context.Entry(r).State = EntityState.Modified;
                                            _context.Entry(r.ParentRequest).State = EntityState.Added;
                                        }
                                        await _context.SaveChangesAsync();
                                    }

                                    foreach (var p in payments)
                                    {
                                        _context.Entry(p).State = EntityState.Added;
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
                                        n++;
                                    } while (commentExists);
                                    await _context.SaveChangesAsync();
                                    if (requests.FirstOrDefault().OrderType == AppUtility.OrderTypeEnum.OrderNow.ToString())
                                    {
                                        MoveDocumentsOutOfTempFolder(requests.FirstOrDefault().RequestID, AppUtility.ParentFolderName.Requests);
                                    }

                                    //save the document
                                    foreach (var request in requests)
                                    {

                                        string NewFolder = Path.Combine(uploadFolder, request.RequestID.ToString());
                                        string folderPath = Path.Combine(NewFolder, AppUtility.FolderNamesEnum.Orders.ToString());
                                        Directory.CreateDirectory(folderPath); //make sure we don't need one above also??

                                        string uniqueFileName = 1 + "OrderEmail.pdf";
                                        string filePath = Path.Combine(folderPath, uniqueFileName);
                                        if (System.IO.File.Exists(filePath))
                                        {
                                            System.IO.File.Delete(filePath);
                                        }

                                        System.IO.File.Copy(uploadFile, filePath); //make sure this works for each of them

                                        request.Product = await _context.Products.Where(p => p.ProductID == request.ProductID).Include(p => p.Vendor).FirstOrDefaultAsync();
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
                                        _context.Add(requestNotification);
                                    }
                                    await _context.SaveChangesAsync();
                                }
                                //throw new Exception();
                                await transaction.CommitAsync();
                                base.RemoveRequestWithCommentsAndEmailSessions();

                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                base.RemoveRequestWithCommentsAndEmailSessions();
                                throw ex;
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
            catch (Exception ex)
            {
                confirmEmailViewModel.RequestIndexObject.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                if (confirmEmailViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.LabManagementQuotes)
                {
                    if (confirmEmailViewModel.RequestIndexObject.SidebarType == AppUtility.SidebarEnum.Quotes)
                    {
                        return RedirectToAction("LabManageQuotes");
                    }
                    else
                    {
                        return RedirectToAction("LabManageOrders");
                    }

                }
                else if (confirmEmailViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart)
                {
                    return RedirectToAction("Cart");
                }
                else
                {
                    return RedirectToAction("Index", confirmEmailViewModel.RequestIndexObject);
                }
            }

        }


        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ConfirmQuoteEmailModal(ConfirmEmailViewModel confirmQuoteEmail)
        {
            List<Request> requests;
            if (confirmQuoteEmail.IsResend)
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.RequestID == confirmQuoteEmail.RequestID)
                    .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                    .Include(r => r.Product.Vendor).Include(r => r.ParentQuote).ToList();
            }
            else
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString())
                    .Where(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.ParentQuote.QuoteStatusID == 1).Where(r => r.RequestStatusID == 6)
                     .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.Product.Vendor)
                     .Include(r => r.ParentQuote).ToList();
            }
            if (requests.Count() == 0)
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString())
                    .Where(r => r.Product.VendorID == confirmQuoteEmail.VendorId && r.ParentQuote.QuoteStatusID == 2).Where(r => r.RequestStatusID == 6)
                     .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                     .Include(r => r.Product.Vendor).Include(r => r.ParentQuote).ToList();
            }
            //base url needs to be declared - perhaps should be getting from js?
            //once deployed need to take base url and put in the parameter for converter.convertHtmlString
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value.ToString()}";

            confirmQuoteEmail.Requests = requests;
            //render the purchase order view into a string using a the confirmEmailViewModel
            string renderedView = await RenderPartialViewToString("OrderEmailView", confirmQuoteEmail);
            //instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            PdfDocument doc = new PdfDocument();
            // create a new pdf document converting an url
            doc = converter.ConvertHtmlString(renderedView, baseUrl);

            /*foreach (var request in requests) //this seems to be doing the exact same thing for each request -> overwriting previously saved one. what should be happening here???
            {*/
                //creating the path for the file to be saved
                string path1 = Path.Combine("wwwroot", AppUtility.ParentFolderName.Requests.ToString());
                string uniqueFileName = "QuotePDF.pdf";
                string filePath = Path.Combine(path1, uniqueFileName);
                // save pdf document
                doc.Save(filePath);
            //}
            // close pdf document
            doc.Close();


            string uploadFolder = Path.Combine("wwwroot", AppUtility.ParentFolderName.Requests.ToString());
            string uploadFile = Path.Combine(uploadFolder, "QuotePDF.pdf");

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
                builder.TextBody = @"Hello," + "\n\n" + "Please send a price quote for the items listed in the attached pdf. \n\nThank you.\n"
                        + ownerUsername + "\nCentarix"; ;
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
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.RequestID == id)
               .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
               .ToList();
            }
            else
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.Product.VendorID == id && r.ParentQuote.QuoteStatusID == 1)
                    .Where(r => r.RequestStatusID == 6).Include(r => r.Product).ThenInclude(p => p.Vendor)
                    .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.ParentQuote).ToList();
            }
            if (requests.Count() == 0)
            {
                requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.Product.VendorID == id && r.ParentQuote.QuoteStatusID == 2)
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
                RequestIndexObject = requestIndexObject
            };

            return PartialView(confirmEmail);
        }


        /*LABMANAGEMENT*/
        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> LabManageQuotes()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementQuotes;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Quotes;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            return View(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.LabManagement, PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SidebarType = AppUtility.SidebarEnum.Quotes }));
        }

        public async Task<IActionResult> _LabManageQuotes(RequestIndexPartialViewModelByVendor labManageQuotesViewModel)
        {
            return PartialView(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.LabManagement, PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SidebarType = AppUtility.SidebarEnum.Quotes }));
        }

        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> LabManageOrders()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementQuotes;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Orders;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            return View(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.LabManagement, PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SidebarType = AppUtility.SidebarEnum.Orders }));
        }
        public async Task<IActionResult> _LabManageOrders(RequestIndexPartialViewModelByVendor labManageQuotesViewModel)
        {
            return PartialView(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.LabManagement, PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SidebarType = AppUtility.SidebarEnum.Orders }));
        }

        public async Task<IActionResult> _IndexTableDataByVendor(RequestIndexObject requestIndexObject)
        {
            return PartialView(await GetIndexViewModelByVendor(requestIndexObject));
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

        //[HttpPost]
        //[Authorize(Roles = "Admin, Requests, Operations")]
        //public async Task<IActionResult> Search(RequestsSearchViewModel requestsSearchViewModel, int? page)
        //{
        //    var categoryType = requestsSearchViewModel.SectionType == AppUtility.MenuItems.Operations ? 2 : 1;
        //    int RSRecieved = 0;
        //    int RSOrdered = 0;
        //    int RSNew = 0;
        //    IQueryable<Request> requestsSearched = _context.Requests.AsQueryable().Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType);

        //    //convert the bools into their corresponding IDs
        //    if (requestsSearchViewModel.Inventory)
        //    {
        //        RSRecieved = 3;
        //    }
        //    if (requestsSearchViewModel.Ordered)
        //    {
        //        RSOrdered = 2;
        //    }
        //    if (requestsSearchViewModel.ForApproval)
        //    {
        //        RSNew = 1;
        //    }
        //    if (requestsSearchViewModel.Inventory || requestsSearchViewModel.Ordered || requestsSearchViewModel.ForApproval) //if any of the checkboxes were selected then filter accordingly
        //    {
        //        requestsSearched = requestsSearched.Where(rs => rs.RequestStatusID == RSRecieved || rs.RequestStatusID == RSOrdered || rs.RequestStatusID == RSNew);
        //    }


        //    requestsSearchViewModel.Request.Product.ProductSubcategory = await _context.ProductSubcategories.Include(ps => ps.ParentCategory).Where(ps => ps.ProductSubcategoryID == requestsSearchViewModel.Request.Product.ProductSubcategoryID).FirstOrDefaultAsync();
        //    if (requestsSearchViewModel.Request.Product.ProductName != null)
        //    {
        //        requestsSearched = requestsSearched.Where(r => r.Product.ProductName.Contains(requestsSearchViewModel.Request.Product.ProductName));
        //    }
        //    if (requestsSearchViewModel.Request.Product?.ProductSubcategory?.ParentCategoryID != 0 && requestsSearchViewModel.Request.Product?.ProductSubcategory?.ParentCategoryID != null)
        //    {
        //        requestsSearched = requestsSearched.Where(r => r.Product.ProductSubcategory.ParentCategoryID == requestsSearchViewModel.Request.Product.ProductSubcategory.ParentCategoryID);
        //    }
        //    if (requestsSearchViewModel.Request.Product?.ProductSubcategoryID != 0 && requestsSearchViewModel.Request.Product?.ProductSubcategoryID != null)
        //    {
        //        requestsSearched = requestsSearched.Where(r => r.Product.ProductSubcategoryID == requestsSearchViewModel.Request.Product.ProductSubcategoryID);
        //    }
        //    //check for project
        //    //check for sub project
        //    if (requestsSearchViewModel.Request.Product?.VendorID != 0 && requestsSearchViewModel.Request.Product?.VendorID != null)
        //    {
        //        requestsSearched = requestsSearched.Where(r => r.Product.VendorID == requestsSearchViewModel.Request.Product.VendorID);
        //    }
        //    if (requestsSearchViewModel.Request.ParentRequest?.OrderNumber != null)
        //    {
        //        requestsSearched = requestsSearched.Where(r => r.ParentRequest.OrderNumber.ToString().Contains(requestsSearchViewModel.Request.ParentRequest.OrderNumber.ToString()));
        //    }
        //    if (requestsSearchViewModel.Request.ParentRequest.OrderDate != DateTime.MinValue) //should this be datetime.min?
        //    {
        //        requestsSearched = requestsSearched.Where(r => r.ParentRequest.OrderDate == requestsSearchViewModel.Request.ParentRequest.OrderDate);
        //    }
        //    if (requestsSearchViewModel.Request.Invoice.InvoiceNumber != null)
        //    {
        //        requestsSearched = requestsSearched.Where(r => r.Invoice.InvoiceNumber.Contains(requestsSearchViewModel.Request.Invoice.InvoiceNumber));
        //    }
        //    if (requestsSearchViewModel.Request.Invoice.InvoiceDate != DateTime.MinValue) //should this be datetime.min?
        //    {
        //        requestsSearched = requestsSearched.Where(r => r.Invoice.InvoiceDate == requestsSearchViewModel.Request.Invoice.InvoiceDate);
        //    }
        //    if (requestsSearchViewModel.Request.ExpectedSupplyDays != null)//should this be on the parent request
        //    {
        //        requestsSearched = requestsSearched.Where(r => r.ExpectedSupplyDays == requestsSearchViewModel.Request.ExpectedSupplyDays);
        //    }

        //    //not sure what the to date and the from date are on????

        //    bool IsRequest = true;
        //    bool IsInventory = false;
        //    bool IsAll = false;
        //    if (requestsSearchViewModel.Inventory)
        //    {
        //        IsRequest = false;
        //        IsInventory = true;
        //    }
        //    else
        //    {
        //        foreach (Request r in requestsSearched)
        //        {
        //            if (r.RequestStatusID != 6)
        //            {
        //                break;
        //            }
        //        }
        //        IsRequest = false;
        //        IsInventory = true;
        //    }

        //    //also need to get the list smaller to just request or inventory

        //    var PageType = AppUtility.PageTypeEnum.None;
        //    if (IsRequest)
        //    {
        //        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestRequest;
        //    }
        //    else if (IsInventory)
        //    {
        //        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestInventory;
        //    }
        //    else if (IsAll)
        //    {
        //        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestRequest;
        //    }
        //    TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;

        //    //ViewData["ReturnRequests"] = requestsSearched;


        //    //Getting the page that is going to be seen (if no page was specified it will be one)
        //    var pageNumber = page ?? 1;
        //    var onePageOfProducts = Enumerable.Empty<Request>().ToPagedList();
        //    try
        //    {
        //        onePageOfProducts = await requestsSearched.Include(r => r.ParentRequest).Include(r => r.Product.ProductSubcategory)
        //            .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).ToPagedListAsync(pageNumber, 25);
        //        //onePageOfProducts;


        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = ex.Message;
        //        TempData["InnerMessage"] = ex.InnerException;
        //        return View("~/Views/Shared/RequestError.cshtml");
        //    }

        //    TempData["Search"] = "True";
        //    if (requestsSearchViewModel.SectionType == AppUtility.MenuItems.Requests)
        //    {
        //        TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
        //        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestRequest;
        //        TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
        //        return RedirectToAction("Index", new { pagetype = TempData[AppUtility.TempDataTypes.PageType.ToString()], vendorID = requestsSearchViewModel.Request.Product.VendorID, subcategoryID = requestsSearchViewModel.Request.Product.ProductSubcategoryID, requestsSearchViewModel = onePageOfProducts });
        //    }
        //    else if (requestsSearchViewModel.SectionType == AppUtility.MenuItems.LabManagement)
        //    {
        //        TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
        //        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSearch;
        //        TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Search;
        //        return RedirectToAction("IndexForLabManage", "Vendors", onePageOfProducts);
        //    }
        //    else if (requestsSearchViewModel.SectionType == AppUtility.MenuItems.Operations)
        //    {
        //        TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
        //        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsInventory;
        //        TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;
        //        return RedirectToAction("Index", "Operations", new { vendorID = requestsSearchViewModel.Request.Product.VendorID, subcategoryID = requestsSearchViewModel.Request.Product.ProductSubcategoryID, requestsSearchViewModel = onePageOfProducts });
        //    }

        //    return RedirectToAction("Index", new { pagetype = TempData[AppUtility.TempDataTypes.PageType.ToString()], vendorID = requestsSearchViewModel.Request.Product.VendorID, subcategoryID = requestsSearchViewModel.Request.Product.ProductSubcategoryID, requestsSearchViewModel = onePageOfProducts });

        //}


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
                    receivedModalVisualViewModel.ChildrenLocationInstances =
                        _context.LocationInstances.Where(m => m.LocationInstanceParentID == LocationInstanceID)
                        .Include(m => m.RequestLocationInstances).OrderBy(m => m.LocationNumber).ToList();


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
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> ReceivedModal(ReceivedLocationViewModel receivedLocationViewModel, ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel, ReceivedModalVisualViewModel receivedModalVisualViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var requestReceived = _context.Requests.Where(r => r.RequestID == receivedLocationViewModel.Request.RequestID)
             .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).FirstOrDefault();
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
                            await SaveLocations(receivedModalVisualViewModel, requestReceived);
                        }
                    }
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

                    requestReceived.ArrivalDate = receivedLocationViewModel.Request.ArrivalDate;
                    requestReceived.ApplicationUserReceiverID = receivedLocationViewModel.Request.ApplicationUserReceiverID;
                    requestReceived.ApplicationUserReceiver = _context.Users.Where(u => u.Id == receivedLocationViewModel.Request.ApplicationUserReceiverID).FirstOrDefault();
                    requestReceived.NoteForPartialDelivery = receivedLocationViewModel.Request.NoteForPartialDelivery;
                    requestReceived.IsPartial = receivedLocationViewModel.Request.IsPartial;
                    requestReceived.NoteForClarifyDelivery = receivedLocationViewModel.Request.NoteForClarifyDelivery;
                    requestReceived.IsClarify = receivedLocationViewModel.Request.IsClarify;
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
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    receivedLocationViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    Response.StatusCode = 500;
                    receivedLocationViewModel.locationTypesDepthZero = _context.LocationTypes.Where(lt => lt.Depth == 0);
                    var userid = _userManager.GetUserId(User);
                    receivedLocationViewModel.Request.ApplicationUserReceiver = _context.Users.Where(u => u.Id == userid).FirstOrDefault();
                    receivedLocationViewModel.Request.ApplicationUserReceiverID = userid;
                    receivedLocationViewModel.Request = _context.Requests.Where(r => r.RequestID == receivedLocationViewModel.Request.RequestID).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                    .FirstOrDefault();
                    return PartialView("ReceivedModal", receivedLocationViewModel);
                }

            }

            return RedirectToAction("_IndexTableWithCounts", receivedLocationViewModel.RequestIndexObject);

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
                    var request = Requests.FirstOrDefault();

                    var requestLocations = _context.Requests.Where(r => r.RequestID == request.RequestID).Include(r => r.RequestLocationInstances).FirstOrDefault().RequestLocationInstances;
                    var isTemporary = false;
                    foreach (var location in requestLocations)
                    {
                        var locationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == location.LocationInstanceID).FirstOrDefault();
                        if (locationInstance is TemporaryLocationInstance)
                        {
                            isTemporary = true;
                            receivedModalVisualViewModel.DeleteTable = true;
                        }
                        else
                        {
                            _context.Remove(location);
                            receivedModalVisualViewModel.ParentLocationInstance =
                        _context.LocationInstances.Where(li => li.LocationInstanceID == receivedModalVisualViewModel.ParentLocationInstance.LocationInstanceID).FirstOrDefault();
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
                    }
                    if (!isTemporary)
                    {
                        await SaveLocations(receivedModalVisualViewModel, request);
                        await transaction.CommitAsync();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return PartialView(receivedModalVisualViewModel);
        }

        /*
         * END RECEIVED MODAL
         */
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public ActionResult DocumentsModal(int? id, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
            AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                FolderName = RequestFolderNameEnum,
                IsEdittable = IsEdittable,
                ParentFolderName = AppUtility.ParentFolderName.Requests,
                ObjectID = id ?? 0,
                SectionType = SectionType,
                ShowSwitch = showSwitch
            };

            FillDocumentsViewModel(documentsModalViewModel);
            return PartialView(documentsModalViewModel);
        }


        [HttpPost]
        public void DocumentsModal(/*[FromBody]*/ DocumentsModalViewModel documentsModalViewModel)
        {
            base.DocumentsModal(documentsModalViewModel);
        }

        [HttpGet]
        public ActionResult DeleteDocumentModal(String FileString, int id, AppUtility.FolderNamesEnum RequestFolderNameEnum, bool IsEdittable, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Requests)
        {
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
            var product = _context.Requests.Where(r => r.Product.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID && r.ProductID != ProductID);
            if (ProductID != null && _context.Requests.Where(r => r.Product.CatalogNumber == CatalogNumber && r.Product.VendorID == VendorID && r.ProductID != ProductID).Any())
            {
                return false;
            }
            return boolCheck;
        }


        [HttpGet]
        public JsonResult GetSublocationInstancesList(int locationInstanceParentId)
        {
            var locationInstanceList = _context.LocationInstances.OfType<LocationInstance>().Where(li => li.LocationInstanceParentID == locationInstanceParentId).Include(li => li.LabPart).OrderBy(li => li.LocationNumber).ToList();
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
                        var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                        HttpContext.Session.SetObject(requestNum, request);
                        return RedirectToAction("TermsModal", requestIndex);
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
                                await transaction.CommitAsync();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                await Response.WriteAsync(ex.Message);
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
                var requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Where(r => r.RequestID == requestID)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.ParentQuote)
                    .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).ToList();
                foreach (var request in requests)
                {
                    request.ExchangeRate = GetExchangeRate();
                }
                EditQuoteDetailsViewModel editQuoteDetailsViewModel = new EditQuoteDetailsViewModel()
                {
                    Requests = requests,
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
                var requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString())
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
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var requests = _context.Requests.Where(r => r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString()).Include(x => x.ParentQuote).Select(r => r);
                        //var quoteDate = editQuoteDetailsViewModel.QuoteDate;
                        var quoteNumber = editQuoteDetailsViewModel.QuoteNumber;
                        foreach (var quote in editQuoteDetailsViewModel.Requests)
                        {
                            //throw new Exception();
                            var request = requests.Where(r => r.RequestID == quote.RequestID).FirstOrDefault();

                            request.ParentQuote.QuoteStatusID = 4;
                            //request.ParentQuote.QuoteDate = quoteDate;
                            request.ParentQuote.QuoteNumber = quoteNumber.ToString();
                            if (quote.IncludeVAT)
                            {
                                quote.Cost = quote.Cost / (decimal)1.17;
                            }
                            request.Cost = quote.Cost;
                            request.Currency = quote.Currency;
                            request.IncludeVAT = quote.IncludeVAT;
                            request.ExpectedSupplyDays = quote.ExpectedSupplyDays;
                            _context.Update(request);
                            _context.SaveChanges();
                            //save file
                            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
                            string requestFolder = Path.Combine(uploadFolder, quote.RequestID.ToString());
                            string folderPath = Path.Combine(requestFolder, AppUtility.FolderNamesEnum.Quotes.ToString());
                            Directory.CreateDirectory(folderPath);
                            string uniqueFileName = 1 + editQuoteDetailsViewModel.QuoteFileUpload.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            editQuoteDetailsViewModel.QuoteFileUpload.CopyTo(new FileStream(filePath, FileMode.Create));
                        }
                        transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.RollbackAsync();
                        editQuoteDetailsViewModel.Requests.ForEach(r => DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests, r.RequestID));
                        throw ex;
                    }
                }

                return RedirectToAction("_IndexTableDataByVendor", new { PageType = AppUtility.PageTypeEnum.LabManagementQuotes, SectionType = AppUtility.MenuItems.LabManagement, SideBarType = AppUtility.SidebarEnum.Quotes });
            }
            catch (Exception ex)
            {
                var previousRequest = _context.Requests.Where(r => r.RequestID == editQuoteDetailsViewModel.Requests.FirstOrDefault().RequestID)
              .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(p => p.Product).ThenInclude(p => p.ProductSubcategory)
              .Include(r => r.ParentQuote).Include(r => r.UnitType).Include(r => r.SubSubUnitType).Include(r => r.SubUnitType).FirstOrDefault();
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
        public async Task<IActionResult> Cart()
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Cart;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.RequestCart;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            return View(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.Requests, PageType = AppUtility.PageTypeEnum.RequestCart, SidebarType = AppUtility.SidebarEnum.Cart }));
        }


        [HttpGet]
        [Authorize(Roles = "Requests, Users, Biomarkers, Accounting, Admin, Reports, Timekeeper, Operations, Protocols, Income, Operation, Expenses, LabManagement")]
        public async Task<IActionResult> ConfirmEdit(AppUtility.MenuItems MenuItem = AppUtility.MenuItems.Requests)
        {
            return PartialView(MenuItem);
        }

        [HttpGet]
        [Authorize(Roles = "Requests, Users, Biomarkers, Accounting, Admin, Reports, Timekeeper, Operations, Protocols, Income, Operation, Expenses, LabManagement")]
        public async Task<IActionResult> ConfirmExit(AppUtility.MenuItems MenuItem = AppUtility.MenuItems.Requests, string url = "")
        {
            ConfirmExitViewModel confirmExit = new ConfirmExitViewModel()
            {
                SectionType = MenuItem,
                URL = url
            };
            return PartialView(confirmExit);
        }
        [HttpPost]
        [Authorize(Roles = "Requests, Users, Biomarkers, Accounting, Admin, Reports, Timekeeper, Operations, Protocols, Income, Operation, Expenses, LabManagement")]
        public async Task<IActionResult> ConfirmExit(ConfirmExitViewModel confirmExit)
        {
            DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests);
            RemoveRequestWithCommentsAndEmailSessions();

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
                return PartialView("Default");
            }
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
            message.Subject = "Please update on delayed supply for Centarix's Order #" + request.ParentRequest.OrderNumber;

            //body
            builder.TextBody = $"The order number {request.ParentRequest.OrderNumber} for {request.Product.ProductName}" +
                $" which was scheduled to arrive on {request.ParentRequest.OrderDate.AddDays((double)request.ExpectedSupplyDays).ToString("dd/MM/yyyy")}, " +
                $"has not arrived yet. \n" +
                    $"Please update us on the matter.\n" +
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
            return RedirectToAction("NotificationsView", new { DidntArrive = true });
        }


        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> AccountingPayments(AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment)
        {

            TempData["PayNowCount"] = GetPaymentRequests(AppUtility.SidebarEnum.PayNow).ToList().Count();
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Accounting;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingPayments;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = accountingPaymentsEnum;
            return View(await GetIndexViewModelByVendor(new RequestIndexObject { SectionType = AppUtility.MenuItems.Accounting, PageType = AppUtility.PageTypeEnum.AccountingPayments, SidebarType = accountingPaymentsEnum }));

        }

        private IQueryable<Request> GetPaymentRequests(AppUtility.SidebarEnum accountingPaymentsEnum)
        {
            var requests = _context.Requests
                .Include(r => r.ParentRequest)
                .Include(r => r.Product).ThenInclude(p => p.Vendor)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(pc => pc.ParentCategory).Include(r => r.Payments)
                .Where(r => r.RequestStatusID != 7 && r.Payments.Where(p => !p.IsPaid).Count() > 0);
            switch (accountingPaymentsEnum)
            {
                case AppUtility.SidebarEnum.MonthlyPayment:
                    requests = requests
                    .Where(r => r.PaymentStatusID == 2 && r.Payments.FirstOrDefault().HasInvoice);
                    break;
                case AppUtility.SidebarEnum.PayNow:
                    requests = requests
                    //.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                    .Where(r => r.PaymentStatusID == 3);
                    break;
                case AppUtility.SidebarEnum.PayLater:
                    requests = requests
                .Where(r => r.PaymentStatusID == 4);
                    break;
                case AppUtility.SidebarEnum.Installments:
                    requests = requests
                        .Where(r => r.PaymentStatusID == 5).Where(r => r.Payments.Where(p => p.IsPaid == false && p.PaymentDate < DateTime.Now.AddDays(5)).Count() > 0);
                    var requestList = requests.ToList();
                    foreach (var request in requests)
                    {
                        var currentInstallments = request.Payments.Where(p => p.IsPaid == false && p.PaymentDate < DateTime.Now.AddDays(5));
                        if (currentInstallments.Count() > 0)
                        {
                            for (var i = 1; i < currentInstallments.Count(); i++)
                            {
                                requestList.Add(request);
                            }
                            requests = requestList.AsQueryable();
                        }
                    }

                    break;
                //case AppUtility.SidebarEnum.StandingOrders:
                //    requestsList = requestsList
                //.Where(r => r.PaymentStatusID == 7).Where(r => r.Payments.Where(p => p.IsPaid == false && p.PaymentDate < DateTime.Now.AddDays(5)).Count() > 0);
                //    break;
                case AppUtility.SidebarEnum.SpecifyPayment:
                    requests = requests
                .Where(r => r.PaymentStatusID == 8 && r.Payments.FirstOrDefault().HasInvoice);
                    break;
            }
            return requests;
        }
        private IQueryable<Request> GetPaymentNotificationRequests(AppUtility.SidebarEnum accountingNotificationsEnum)
        {
            var requestsList = _context.Requests
                .Include(r => r.ParentRequest)
                .Include(r => r.Product).ThenInclude(p => p.Vendor)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
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
                    requestsList = requestsList.Where(r => r.IsPartial);
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
            List<Request> requestsToPay = new List<Request>();
            var requestsList = GetPaymentRequests(accountingPaymentsEnum);

            if (vendorid != null)
            {
                requestsToPay = await requestsList.Where(r => r.Product.VendorID == vendorid).ToListAsync();
            }
            else if (requestid != null)
            {
                requestsToPay = await requestsList.Where(r => r.RequestID == requestid).ToListAsync();
            }
            else if (requestIds != null)

            {
                foreach (int rId in requestIds)
                {
                    requestsToPay.Add(requestsList.Where(r => r.RequestID == rId).FirstOrDefault());
                }
            }
            PaymentsPayModalViewModel paymentsPayModalViewModel = new PaymentsPayModalViewModel()
            {
                Requests = requestsToPay,
                AccountingEnum = accountingPaymentsEnum,
                Payment = new Payment(),
                PaymentTypes = _context.PaymentTypes.Select(pt => pt).ToList(),
                CompanyAccounts = _context.CompanyAccounts.Select(ca => ca).ToList()
            };

            //check if payment status type is installments to show the installments in the view model

            return PartialView(paymentsPayModalViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> PaymentsPayModal(PaymentsPayModalViewModel paymentsPayModalViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
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
                        _context.Update(requestToUpdate);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Response.StatusCode = 500;
                    for (int i = 0; i < paymentsPayModalViewModel.Requests.Count; i++)
                    {
                        paymentsPayModalViewModel.Requests[i] = _context.Requests.Where(r => r.RequestID == paymentsPayModalViewModel.Requests[i].RequestID).Include(r => r.Product)
                            .ThenInclude(p => p.Vendor).FirstOrDefault();
                    }
                    paymentsPayModalViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    return PartialView(paymentsPayModalViewModel);
                }
            }

            return RedirectToAction("AccountingPayments", new { accountingPaymentsEnum = paymentsPayModalViewModel.AccountingEnum });
        }
        [HttpGet]
        [Authorize(Roles = "Accounting")]
        public async Task<IActionResult> PaymentsInvoiceModal(int? vendorid, int? paymentid, AppUtility.SidebarEnum accountingPaymentsEnum = AppUtility.SidebarEnum.MonthlyPayment)
        {
            var payment = _context.Payments.Where(p => p.PaymentID == paymentid).FirstOrDefault();
            var requestToPay = _context.Requests.Where(r => r.RequestID == payment.RequestID).Include(r => r.ParentRequest)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.Payments).ToList();
            if (payment.InstallmentNumber == requestToPay.FirstOrDefault().Installments)
            {
                var paidSum = requestToPay.FirstOrDefault().Payments.Where(p => p.IsPaid).Select(p => p.Sum).Sum();
                payment.Sum = (decimal)requestToPay.FirstOrDefault().Cost - paidSum;
            }
            PaymentsInvoiceViewModel paymentsInvoiceViewModel = new PaymentsInvoiceViewModel()
            {
                Requests = requestToPay,
                AccountingEnum = accountingPaymentsEnum,
                Payment = payment,
                PaymentTypes = _context.PaymentTypes.Select(pt => pt).ToList(),
                CompanyAccounts = _context.CompanyAccounts.Select(ca => ca).ToList(),
                Invoice = new Invoice()
                {
                    InvoiceDate = DateTime.Today
                }
            };

            //check if payment status type is installments to show the installments in the view model

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
                        if (requestToUpdate.PaymentStatusID == 5)
                        {
                            payment.Sum = request.Payments.FirstOrDefault().Sum;
                        }
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
            var queryableRequests = _context.Requests
                .Include(r => r.ParentRequest)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.Payments)
                    .Where(r => r.IsDeleted == false && r.Payments.FirstOrDefault().HasInvoice == false && ((r.PaymentStatusID == 2/*+30*/ && r.RequestStatusID == 3) || (r.PaymentStatusID == 3/*pay now*/) || (r.PaymentStatusID == 8/*specify payment*/ && r.RequestStatusID == 3))).Where(r => r.RequestStatusID != 7);
            if (vendorid != null)
            {
                Requests = queryableRequests
                    .Where(r => r.Payments.FirstOrDefault().HasInvoice == false)
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Add(addInvoiceViewModel.Invoice);
                    await _context.SaveChangesAsync();


                    foreach (var request in addInvoiceViewModel.Requests)
                    {
                        var RequestToSave = _context.Requests.Where(r => r.RequestID == request.RequestID).Include(r => r.Payments).FirstOrDefault();
                        RequestToSave.Cost = request.Cost;
                        RequestToSave.Payments.FirstOrDefault().InvoiceID = addInvoiceViewModel.Invoice.InvoiceID;
                        RequestToSave.Payments.FirstOrDefault().HasInvoice = true;
                        if (RequestToSave.IncludeVAT)
                        {
                            RequestToSave.Cost = RequestToSave.Cost / (decimal)1.17;
                        }

                        _context.Update(RequestToSave);

                        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
                        string requestFolder = Path.Combine(uploadFolder, request.RequestID.ToString());
                        Directory.CreateDirectory(requestFolder);
                        if (addInvoiceViewModel.InvoiceImage != null)
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
                            string uniqueFileName = x + addInvoiceViewModel.InvoiceImage.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            FileStream filestream = new FileStream(filePath, FileMode.Create);
                            addInvoiceViewModel.InvoiceImage.CopyTo(filestream);
                            filestream.Close();
                        }
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
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadQuoteModal(AppUtility.OrderTypeEnum OrderType)
        {
            RequestIndexObject requestIndexObject = new RequestIndexObject();
            var uploadQuoteViewModel = new UploadQuoteViewModel();

            uploadQuoteViewModel.OrderTypeEnum = OrderType;
            uploadQuoteViewModel.RequestIndexObject = requestIndexObject;
            uploadQuoteViewModel.IsReorder = requestIndexObject.IsReorder;

            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
            string uploadFolder2 = Path.Combine(uploadFolder1, "0");
            string uploadFolderQuotes = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.Quotes.ToString());

            uploadQuoteViewModel = new UploadQuoteViewModel();
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

            return PartialView(uploadQuoteViewModel);

        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> UploadQuoteModal(UploadQuoteViewModel uploadQuoteOrderViewModel, bool isCancel = false)
        {
            if (isCancel)
            {
                RemoveRequestWithCommentsAndEmailSessions();
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests);
                return PartialView("Default");
            }
            try
            {

                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                var request = HttpContext.Session.GetObject<Request>(requestName);
                uploadQuoteOrderViewModel.ParentQuote.QuoteStatusID = 4;
                request.ParentQuote = uploadQuoteOrderViewModel.ParentQuote;
                request.ExpectedSupplyDays = uploadQuoteOrderViewModel.ExpectedSupplyDays;
                if (request.RequestStatusID == 1)
                {
                    TempData["RequestStatus"] = 1;
                }

                if (request.RequestStatusID == 6 && request.OrderType != AppUtility.OrderTypeEnum.AddToCart.ToString())
                {
                    var requestNum = AppData.SessionExtensions.SessionNames.Request.ToString() + 1;
                    HttpContext.Session.SetObject(requestNum, request);
                    return RedirectToAction("TermsModal", uploadQuoteOrderViewModel.RequestIndexObject);
                }
                else
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (uploadQuoteOrderViewModel.ExpectedSupplyDays != null)
                            {
                                request.ExpectedSupplyDays = uploadQuoteOrderViewModel.ExpectedSupplyDays;
                            }
                            _context.Entry(request.ParentQuote).State = EntityState.Added;
                            if (request.Product.ProductID == 0)
                            {
                                _context.Entry(request.Product).State = EntityState.Added;
                            }
                            _context.Entry(request).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                            await SaveCommentsFromSession(request);
                            //rename temp folder to the request id
                            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
                            string requestFolderFrom = Path.Combine(uploadFolder, "0");
                            string requestFolderTo = Path.Combine(uploadFolder, request.RequestID.ToString());
                            if (Directory.Exists(requestFolderTo))
                            {
                                Directory.Delete(requestFolderTo);
                            }
                            Directory.Move(requestFolderFrom, requestFolderTo);

                            try
                            {
                                await transaction.CommitAsync();
                            }
                            catch (Exception ex)
                            {
                                Directory.Move(requestFolderTo, requestFolderFrom);
                                throw ex;
                            }
                            base.RemoveRequestWithCommentsAndEmailSessions();
                            var action = "_IndexTableData";
                            if (uploadQuoteOrderViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestRequest)
                            {
                                action = "_IndexTableWithCounts";
                            }
                            else if (uploadQuoteOrderViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart)
                            {
                                action = "NotificationsView";
                            }

                            return RedirectToAction(action, uploadQuoteOrderViewModel.RequestIndexObject);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                uploadQuoteOrderViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                return PartialView("UploadQuoteModal", uploadQuoteOrderViewModel);
            }
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
            var UploadQuoteViewModel = new UploadOrderViewModel() { ParentRequest = pr, RequestIndexObject = requestIndexObject, IsReorder = requestIndexObject.IsReorder };

            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
            string uploadFolder2 = Path.Combine(uploadFolder1, "0");
            string uploadFolderOrders = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.Orders.ToString());

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
        public async Task<IActionResult> UploadOrderModal(UploadOrderViewModel uploadQuoteOrderViewModel, bool isCancel = false)
        {
            if (isCancel)
            {
                RemoveRequestWithCommentsAndEmailSessions();
                DeleteTemporaryDocuments(AppUtility.ParentFolderName.Requests);
                return PartialView("Default");
            }
            try
            {
                var requests = new List<Request>();
                var isRequests = true;
                var RequestNum = 1;
                while (isRequests)
                {
                    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                    var req = HttpContext.Session.GetObject<Request>(requestName);
                    if (req != null)
                    {
                        req.ExpectedSupplyDays = uploadQuoteOrderViewModel.ExpectedSupplyDays;
                        requests.Add(req);
                    }
                    else
                    {
                        isRequests = false;
                    }
                    RequestNum++;
                }
                RequestNum = 1;
                foreach (var request in requests)
                {
                    request.ParentRequest = uploadQuoteOrderViewModel.ParentRequest;
                    request.ParentQuote = null;

                    var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;

                    HttpContext.Session.SetObject(requestName, request);
                    RequestNum++;
                }
                string action;
                if (uploadQuoteOrderViewModel.RequestIndexObject.OrderType == AppUtility.OrderTypeEnum.AlreadyPurchased || uploadQuoteOrderViewModel.RequestIndexObject.OrderType == AppUtility.OrderTypeEnum.SaveOperations)
                {
                    action = "TermsModal";
                }
                else if (uploadQuoteOrderViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestRequest)
                {
                    action = "_IndexTableWithCounts";
                }
                else if (uploadQuoteOrderViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestCart)
                {
                    action = "NotificationsView";
                }
                else
                {
                    action = "_IndexTableData";
                }
                Response.StatusCode = 200;
                return RedirectToAction(action, uploadQuoteOrderViewModel.RequestIndexObject);
            }
            catch (Exception ex)
            {
                uploadQuoteOrderViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                return PartialView("UploadOrderModal", uploadQuoteOrderViewModel);
            }
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
                        n++;
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

        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> TermsModal(int vendorID, RequestIndexObject requestIndexObject) //either it'll be a request or parentrequest and then it'll send it to all the requests in that parent request
        {
            var requ = HttpContext.Session.GetObject<Request>("Request1");
            List<Request> requests = new List<Request>();
            if (vendorID != 0)
            {
                if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Cart)
                {
                    requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
          .Where(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.AddToCart.ToString() && r.ParentQuote.QuoteStatusID == 4)
          .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
                .Include(r => r.Product).ThenInclude(r => r.Vendor)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
                }
                else if (requestIndexObject.SidebarType == AppUtility.SidebarEnum.Orders)
                {
                    requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
          .Where(r => r.Product.VendorID == vendorID && r.RequestStatusID == 6 && r.OrderType == AppUtility.OrderTypeEnum.RequestPriceQuote.ToString() && r.ParentQuote.QuoteStatusID == 4)
                .Include(r => r.Product).ThenInclude(r => r.Vendor)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
                }
                RemoveRequestWithCommentsAndEmailSessions();
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
            var termsList = new List<SelectListItem>() { };
            await _context.PaymentStatuses.ForEachAsync(ps =>
            {
                if (ps.PaymentStatusID != 7)//don't have standing orders as an option
                {
                    termsList.Add(new SelectListItem() { Value = ps.PaymentStatusID + "", Text = ps.PaymentStatusDescription });
                }
            });
            TermsViewModel termsViewModel = new TermsViewModel()
            {
                ParentRequest = new ParentRequest(),
                TermsList = termsList,
                InstallmentDate = DateTime.Now
            };
            requestIndexObject.SelectedCurrency = (AppUtility.CurrencyEnum)Enum.Parse(typeof(AppUtility.CurrencyEnum), requests[0].Currency);
            termsViewModel.RequestIndexObject = requestIndexObject;
            return PartialView(termsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> TermsModal(TermsViewModel termsViewModel)
        {
            try
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

                RequestNum = 1;
                var PaymentNum = 1;
                var SaveUsingSessions = true;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var req in requests)
                        {
                            if (req.Product == null)
                            {
                                req.Product = _context.Products.Where(p => p.ProductID == req.ProductID).Include(p => p.ProductSubcategory).FirstOrDefault();
                            }

                            if (req.OrderType == AppUtility.OrderTypeEnum.AlreadyPurchased.ToString() || req.OrderType == AppUtility.OrderTypeEnum.SaveOperations.ToString())
                            {
                                SaveUsingSessions = false;
                            }
                            if (req.ParentRequest == null)
                            {
                                req.ParentRequest = termsViewModel.ParentRequest;
                            }
                            req.ParentRequest.Shipping = termsViewModel.ParentRequest.Shipping;
                            req.PaymentStatusID = termsViewModel.SelectedTerm;
                            req.Installments = (uint)termsViewModel.Installments;
                            if (termsViewModel.Installments == 0)
                            {
                                req.Installments = 1;
                            }
                            if (SaveUsingSessions)
                            {
                                var requestName = AppData.SessionExtensions.SessionNames.Request.ToString() + RequestNum;
                                HttpContext.Session.SetObject(requestName, req);
                            }
                            else
                            {

                                //if (req.PaymentStatusID == 7)
                                //{
                                //    req.RequestStatusID = 3;
                                //    req.ApplicationUserReceiverID = _userManager.GetUserId(User);
                                //    req.ArrivalDate = DateTime.Now;
                                //}
                                if (req.Product.ProductID == 0)
                                {
                                    _context.Entry(req.Product).State = EntityState.Added;
                                }
                                else
                                {
                                    _context.Entry(req.Product).State = EntityState.Unchanged;
                                }
                                _context.Entry(req).State = EntityState.Added;
                                _context.Entry(req.ParentRequest).State = EntityState.Added;


                                await _context.SaveChangesAsync();
                            }
                            for (int i = 0; i < req.Installments; i++)
                            {
                                var payment = new Payment() { InstallmentNumber = i + 1 };
                                if (req.PaymentStatusID == 5)
                                {
                                    payment.PaymentDate = termsViewModel.InstallmentDate.AddMonths(i);
                                    payment.Sum = ((req.Cost ?? 0) / (req.Installments ?? 0));
                                }
                                else
                                {
                                    payment.PaymentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                    payment.Sum = req.Cost ?? 0;
                                }
                                if (SaveUsingSessions)
                                {
                                    var paymentName = AppData.SessionExtensions.SessionNames.Payment.ToString() + (PaymentNum);
                                    HttpContext.Session.SetObject(paymentName, payment);
                                }
                                else
                                {
                                    payment.RequestID = req.RequestID;
                                    _context.Add(payment);
                                    await _context.SaveChangesAsync();
                                }
                                PaymentNum++;
                            }
                            RequestNum++;
                        }
                        if (!SaveUsingSessions)
                        {

                            int i = 1;
                            var additionalRequests = false;
                            foreach (var request in requests)
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
                                    n++;
                                } while (commentExists);
                                await _context.SaveChangesAsync();
                                if (i < requests.Count)
                                {
                                    additionalRequests = true;
                                }
                                else
                                {
                                    additionalRequests = false;
                                }
                                MoveDocumentsOutOfTempFolder(request.RequestID, AppUtility.ParentFolderName.Requests, additionalRequests);
                                request.Product.Vendor = _context.Vendors.Where(v => v.VendorID == request.Product.VendorID).FirstOrDefault();
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
                                _context.Add(requestNotification);
                                i++;
                            }
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                            var action = "Index";
                            if (termsViewModel.RequestIndexObject.PageType == AppUtility.PageTypeEnum.RequestSummary)
                            {
                                action = "IndexInventory";
                            }
                            return RedirectToAction(action, termsViewModel.RequestIndexObject);
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }
                return RedirectToAction("ConfirmEmailModal", termsViewModel.RequestIndexObject);
            }
            catch (Exception ex)
            {
                termsViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                var termsList = new List<SelectListItem>() { };
                await _context.PaymentStatuses.ForEachAsync(ps => termsList.Add(new SelectListItem() { Value = ps.PaymentStatusID + "", Text = ps.PaymentStatusDescription }));
                termsViewModel.TermsList = termsList;
                return PartialView("TermsModal", termsViewModel);
            }
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
        public IActionResult _InventoryFilterResults(SelectedFilters selectedFilters, int numFilters, AppUtility.MenuItems sectionType, bool isProprietary)
        {
            InventoryFilterViewModel inventoryFilterViewModel = GetInventoryFilterViewModel(selectedFilters, numFilters, sectionType, isProprietary);
            return PartialView(inventoryFilterViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _CartTotalModal(int requestID, AppUtility.MenuItems sectionType = AppUtility.MenuItems.Requests)
        {
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
            var requestItemViewModel = await editModalViewFunction(id, 0, SectionType, false);
            requestItemViewModel.IsHistory = true;
            return PartialView(requestItemViewModel);
        }
        [Authorize(Roles = "Requests")]
        [HttpPost]
        public async Task<IActionResult> _HistoryTab(int? id, List<string> selectedPriceSort, string selectedCurrency, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            var requestItemViewModel = await editModalViewFunction(id, 0, SectionType, false, selectedPriceSort, selectedCurrency);
            return PartialView(requestItemViewModel);
        }
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

    }
}
