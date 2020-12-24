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
//using Org.BouncyCastle.Asn1.X509;
//using System.Data.Entity.Validation;
//using System.Data.Entity.Infrastructure;

namespace PrototypeWithAuth.Controllers
{
    public class OperationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //take this out?
        private readonly IHostingEnvironment _hostingEnvironment;

        //private readonly IHttpContextAccessor _Context;

        //take this out?
        private readonly List<Request> _cartRequests = new List<Request>();

        private IQueryable<Request> _searchList = Enumerable.Empty<Request>().AsQueryable();
        private ICompositeViewEngine _viewEngine;

        //public MyController(ICompositeViewEngine viewEngine)
        //{
        //    _viewEngine = viewEngine;
        //}
        public OperationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment hostingEnvironment, ICompositeViewEngine viewEngine /*IHttpContextAccessor Context*/)
        {
            //_Context = Context;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            //use the hosting environment for the file uploads
            _hostingEnvironment = hostingEnvironment;
            _viewEngine = viewEngine;
        }

        [HttpGet]
        [Authorize(Roles = "Operations")]
        // GET: Requests
        //IMPORTANT!!! When adding more parameters into the Index Get make sure to add them to the ViewData and follow them through to the Index page
        //ALSO when changing defaults -> change the defaults on the index page for paged list 


        public async Task<IActionResult> Index(int page=0, int RequestStatusID = 1, int subcategoryID = 0, int vendorID = 0,
            string applicationUserID = null, int parentLocationInstanceID = 0,
            RequestsSearchViewModel? requestsSearchViewModel = null, AppUtility.PageTypeEnum PageType= AppUtility.PageTypeEnum.OperationsRequest)
        {

            //instantiate your list of requests to pass into the index
            IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ApplicationUserCreator)
                 .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).Include(r => r.ParentQuote)
                 .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
                 .OrderBy(r => r.CreationDate);

            List<PriceSortViewModel> priceSorts = new List<PriceSortViewModel>();
            Enum.GetValues(typeof(AppUtility.PriceSortEnum)).Cast<AppUtility.PriceSortEnum>().ToList().ForEach(p => priceSorts.Add(new PriceSortViewModel { PriceSortEnum = p, Selected = p == AppUtility.PriceSortEnum.Total ? true : false }));

            int newCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 1, vendorID, subcategoryID, applicationUserID);
            int orderedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 2, vendorID, subcategoryID, applicationUserID);
            int receivedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 3, vendorID, subcategoryID, applicationUserID);
            int approvedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 6, vendorID, subcategoryID, applicationUserID);
            newCount += AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 4, vendorID, subcategoryID, applicationUserID);
            newCount += AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 5, vendorID, subcategoryID, applicationUserID);
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;

            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;//passing in the amounts to display in the top buttons
            TempData["AmountNew"] = newCount;
            TempData["AmountOrdered"] = orderedCount;
            TempData["AmountReceived"] = receivedCount;
            TempData["AmountApproved"] = approvedCount;

            if (ViewBag.ErrorMessage != null)
            {
                ViewBag.ErrorMessage = ViewBag.ErrorMessage;
            }
            var viewmodel = await GetIndexViewModel(page, RequestStatusID, subcategoryID, vendorID, applicationUserID, parentLocationInstanceID,  requestsSearchViewModel, PageType);
            viewmodel.PriceSortEnums = priceSorts;
            viewmodel.currency = AppUtility.CurrencyEnum.NIS;
            return View(viewmodel);

        }
        [Authorize(Roles = "Operations")]
        private async Task<RequestIndexViewModel> GetIndexViewModel(int page = 1, int RequestStatusID = 1, int subcategoryID = 0, int vendorID = 0, string applicationUserID = null, int parentLocationInstanceID = 0,
          RequestsSearchViewModel? requestsSearchViewModel = null, AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.OperationsRequest)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            IQueryable<Request> RequestsPassedIn = Enumerable.Empty<Request>().AsQueryable();

            //instantiate your list of requests to pass into the index
            IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ApplicationUserCreator)
                .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).Include(r => r.ParentQuote)
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
                .OrderBy(r => r.CreationDate);
           
            if (ViewData["ReturnRequests"] != null)
            {
                RequestsPassedIn = TempData["ReturnRequests"] as IQueryable<Request>;
            }
            else if (PageType == AppUtility.PageTypeEnum.OperationsRequest)
            {
                /*
                 * In order to combine all the requests each one needs to be in a separate list
                 * Then you need to union them one at a time into separate variables
                 * you only need the separate union variable in order for the union to work
                 * and the original queries are on separate lists because each is querying the full database with a separate where
                 */
                IQueryable<Request> TempRequestList = Enumerable.Empty<Request>().AsQueryable();
                if (RequestStatusID == 0 || RequestStatusID == 1)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 1);
                    RequestsPassedIn = TempRequestList;
                }

                if (RequestStatusID == 0 || RequestStatusID == 6)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 6);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }

                if (RequestStatusID == 0 || RequestStatusID == 2)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 2);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                    RequestsPassedIn = RequestsPassedIn.OrderByDescending(rpi => rpi.ParentRequest.OrderDate);
                }

                if (RequestStatusID == 0 || RequestStatusID == 3)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 3, 50);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                    RequestsPassedIn = RequestsPassedIn.OrderByDescending(rpi => rpi.ArrivalDate);
                }
                //if the user chooses a new status they want to see this too
                if (RequestStatusID == 0 || RequestStatusID == 4 || RequestStatusID == 1)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 4);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }
                //if the user chooses a new status they want to see this too
                if (RequestStatusID == 0 || RequestStatusID == 5 || RequestStatusID == 1)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 5);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }


            }
            //if it is an inventory page --> get all the requests with received and is inventory request status
            else if (PageType == AppUtility.PageTypeEnum.OperationsInventory)
            {
                //partial and clarify?
                RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3);
                RequestStatusID = 3;
            }
            else
            {
                RequestsPassedIn = fullRequestsList;
            }
            AppUtility.SidebarEnum SidebarTitle = AppUtility.SidebarEnum.LastItem;
            //now that the lists are created sort by vendor or subcategory
            if (vendorID > 0 && requestsSearchViewModel != null)
            {
                SidebarTitle = AppUtility.SidebarEnum.Vendors;
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.VendorID == vendorID);
            }
            else if (subcategoryID > 0 && requestsSearchViewModel != null)
            {
                SidebarTitle = AppUtility.SidebarEnum.Type;
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.ProductSubcategoryID == subcategoryID);
            }
            else if (applicationUserID != null && requestsSearchViewModel != null)
            {
                SidebarTitle = AppUtility.SidebarEnum.Owner;
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.ApplicationUserCreatorID == applicationUserID);
            }
            else if (parentLocationInstanceID > 0 && requestsSearchViewModel != null)
            {
                //  SidebarTitle = AppUtility.OrdersAndInventorySidebarEnum.Vendor;
                LocationInstance rliList = _context.LocationInstances
                    .Include(li => li.AllRequestLocationInstances)
                    .Where(li => li.LocationInstanceID == parentLocationInstanceID).FirstOrDefault();
                RequestsPassedIn = RequestsPassedIn.Where(r => rliList.AllRequestLocationInstances.Select(rli => rli.RequestID).ToList().Contains(r.RequestID));
            }



            RequestIndexViewModel requestIndexViewModel = new RequestIndexViewModel();
            //instantiate your list of requests to pass into the index
            /*int?*/
            requestIndexViewModel.Page = page;
            /*int*/
            requestIndexViewModel.RequestStatusID = RequestStatusID;
            /*int*/
            requestIndexViewModel.SubCategoryID = subcategoryID;
            /*int*/
            requestIndexViewModel.VendorID = vendorID;
            /*string*/
            requestIndexViewModel.ApplicationUserID = applicationUserID;
            /*AppUtility.RequestPageTypeEnum*/
            requestIndexViewModel.OperPageType = PageType;

            /*RequestsSearchViewModel?*/
            //TempData["TempRequestsSearchViewModel"] = requestsSearchViewModel;
            requestIndexViewModel.RequestParentLocationInstanceID = parentLocationInstanceID;
            //use an iqueryable (not ienumerable) until it's passed in so you can include the vendors and subcategories later on
            var onePageOfProducts = Enumerable.Empty<Request>().ToPagedList();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = SidebarTitle;
            try
            {
                onePageOfProducts = await RequestsPassedIn.Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.ParentRequest)
                    .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                    .ToPagedListAsync(page == 0 ? 1 : page, 25);

                onePageOfProducts.OrderByDescending(opop => opop.ArrivalDate).Where(opop => opop.RequestStatusID == 5); // display by arrivaldate if recieved
                onePageOfProducts.Where(opop => opop.RequestStatusID == 2).OrderByDescending(opop => opop.ParentRequest.OrderDate); // display by orderdate if ordered
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                ViewBag.ErrorMessage = ex.Message;
                // Redirect("~/Views/Shared/RequestError.cshtml");
            }
            requestIndexViewModel.PagedList = onePageOfProducts;
            return requestIndexViewModel;
        }
        [HttpGet]
        [Authorize(Roles = "Operations")] //TODO: Does this also 
        public async Task<IActionResult> _IndexTable(int page, int RequestStatusID = 1, int subcategoryID = 0, int vendorID = 0, string applicationUserID = null, int parentLocationInstanceID = 0,
             AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests,
                  RequestsSearchViewModel? requestsSearchViewModel = null, List<String> selectedPriceSort = null, AppUtility.CurrencyEnum selectedCurrency = AppUtility.CurrencyEnum.NIS)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;

            RequestIndexViewModel viewModel = await GetIndexViewModel(page, RequestStatusID, subcategoryID, vendorID, applicationUserID, parentLocationInstanceID,
           requestsSearchViewModel);
            viewModel.currency = selectedCurrency;
            viewModel.PriceSortEnumsList = selectedPriceSort;
            return PartialView(viewModel);
        }
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> CreateModalView()
        {
            var parentcategories = await _context.ParentCategories.Where(pr => pr.CategoryTypeID == 2).ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(pr => pr.ParentCategory.CategoryTypeID == 2).ToListAsync();
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 2).Count() > 0).ToListAsync();
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts,
                CommentTypes = commentTypes,
                Comments = new List<Comment>(),
            };
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
            requestItemViewModel.Request = new Request();
            requestItemViewModel.Request.Product = new Product();
            requestItemViewModel.Request.Product = new Product();
            requestItemViewModel.Request.ParentQuote = new ParentQuote();
            requestItemViewModel.Request.ExchangeRate =  _context.ExchangeRates.FirstOrDefault().LatestExchangeRate;

            requestItemViewModel.Request.ParentQuote.QuoteDate = DateTime.Now;
            requestItemViewModel.Request.CreationDate = DateTime.Now;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsRequest;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
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
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> CreateModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //why do we need this here?
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.Include(ps => ps.ParentCategory).FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

            //in case we need to return to the modal view
            requestItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 2).Count() > 0).ToListAsync();
            requestItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();
            //formatting the select list of the unit types

            //declared outside the if b/c it's used farther down too 
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            requestItemViewModel.Request.ApplicationUserCreatorID = currentUser.Id;
            requestItemViewModel.Request.ApplicationUserCreator = currentUser;
            requestItemViewModel.Request.CreationDate = DateTime.Now;
            requestItemViewModel.Request.ParentQuote.ApplicationUser = currentUser;
            requestItemViewModel.Request.ParentQuote.QuoteDate = DateTime.Now;

            //all new ones will be "new" until actually ordered after the confirm email
            requestItemViewModel.Request.RequestStatusID = 1;

            //in case we need to redirect to action
            //TempData["ModalView"] = true;
            //todo why is this here?
            //todo terms and installements and paid
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
                //CREATE MODAL - may need to take this out? shouldn't it always create a new product??
                //requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;

                //if it is out of the budget get sent to get approved automatically and user is not in role admin !User.IsInRole("Admin")
                if (/*!User.IsInRole("Admin") &&*/ OrderType.Equals("Ask For Permission") || !checkIfInBudget(requestItemViewModel.Request))
                {
                    try
                    {
                        requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                        requestItemViewModel.Request.RequestStatusID = 1; //new request
                        _context.Update(requestItemViewModel.Request);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                        TempData["InnerMessage"] = ex.InnerException;
                        return View("~/Views/Shared/RequestError.cshtml");
                    }
                }
                else
                {
                    if (OrderType.Equals("Add To Cart"))
                    {
                        try
                        {
                            requestItemViewModel.Request.RequestStatusID = 6; //approved
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = ex.Message;
                            TempData["InnerMessage"] = ex.InnerException;
                            return View("~/Views/Shared/RequestError.cshtml");
                        }
                    }
                    try
                    {
                        if (OrderType.Equals("Without Order"))
                        {
                            requestItemViewModel.Request.ParentRequest = new ParentRequest();
                            int lastParentRequestOrderNum = 0;
                            requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
                            if (_context.ParentRequests.Any())
                            {
                                lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber ?? 0;
                            }
                            requestItemViewModel.Request.ParentRequest.OrderNumber = lastParentRequestOrderNum + 1;
                            requestItemViewModel.Request.ParentRequest.OrderDate = DateTime.Now;
                            requestItemViewModel.Request.ParentRequest.WithoutOrder = true;
                            requestItemViewModel.Request.RequestStatusID = 2;
                            requestItemViewModel.RequestStatusID = 2;
                            requestItemViewModel.Request.ParentQuote = null;
                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                        }
                        else if (OrderType.Equals("Order"))
                        {
                            requestItemViewModel.Request.RequestStatusID = 1; //new request
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            requestItemViewModel.RequestStatusID = 1;
                            requestItemViewModel.Request.ParentRequest = new ParentRequest();
                            requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;

                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                            TempData["OpenTermsModal"] = "Single";
                            //TempData["OpenConfirmEmailModal"] = true; //now we want it to go to the terms instead
                            TempData["RequestID"] = requestItemViewModel.Request.RequestID;
                        }
                        else
                        {
                            requestItemViewModel.Request.RequestStatusID = 1; //needs approvall
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                        TempData["InnerMessage"] = ex.InnerException;
                        return View("~/Views/Shared/RequestError.cshtml");
                    }
                }
                try
                {

                    try
                    {
                        foreach (var comment in requestItemViewModel.Comments)
                        {
                            if (comment.CommentText.Length != 0)
                            {
                                //save the new comment
                                comment.ApplicationUserID = currentUser.Id;
                                comment.CommentTimeStamp = DateTime.Now; //check if we actually need this line
                                comment.RequestID = requestItemViewModel.Request.RequestID;
                                _context.Add(comment);
                            }


                        }
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        //do something here. comment didn't save
                    }

                    //rename temp folder to the request id
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    string requestFolderFrom = Path.Combine(uploadFolder, "0");
                    string requestFolderTo = Path.Combine(uploadFolder, requestItemViewModel.Request.RequestID.ToString());
                    Directory.Move(requestFolderFrom, requestFolderTo);

                    return RedirectToAction("Index", new
                    {
                        page = requestItemViewModel.Page,
                        requestStatusID = requestItemViewModel.Request.RequestStatusID,
                        PageType = AppUtility.PageTypeEnum.RequestRequest
                    });
                }
                catch (DbUpdateException ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    TempData["InnerMessage"] = ex.InnerException.ToString();
                    return View("~/Views/Shared/RequestError.cshtml");
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.Message.ToString();
                    TempData["InnerMessage"] = ex.InnerException.ToString();
                    return View("~/Views/Shared/RequestError.cshtml");
                }
            }
            else
            {
                TempData["InnerMessage"] = "The request model failed to validate. Please ensure that all fields were filled in correctly";
                return View("~/Views/Shared/RequestError.cshtml");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> DetailsModalView(int? id, bool NewRequestFromProduct = false)
        {
            //string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }

            var parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 2).ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 2).ToListAsync();
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 2).Count() > 0).ToListAsync();
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts
            };

            if (id == 0)
            {
                return RedirectToAction("CreateModalView");
            }

            else
            {

                requestItemViewModel.Request = _context.Requests.Include(r => r.Product)
                    .Include(r => r.ParentRequest)
                    .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .Include(r => r.RequestStatus)
                    .Include(r => r.ApplicationUserCreator)
                    .Include(r => r.ParentQuote)
                    //.Include(r => r.Payments) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .SingleOrDefault(x => x.RequestID == id);

                //check if this works once there are commments
                //var comments = Enumerable.Empty<Comment>();
                //comments = _context.Comments
                //    .Include(c => c.ApplicationUser)
                //    .Where(c => c.Request.RequestID == id);
                ////needs to be instantiated here so it doesn't throw an error if nothing is in it
                ///*
                // *I think it should be an ienumerable and look like
                // *requestItemViewModel.Comments = new Enumerable.Empty<Comment>(); 
                // *ike before but it's not recognizing the syntax
                //*/
                //requestItemViewModel.OldComments = comments.ToList();

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

        [HttpGet]
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> EditModalView(int? id, bool NewRequestFromProduct = false)
        {
            return await editModalViewFunction(id);
        }

        [HttpGet]
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> EditModalViewPartial(int? id, int? Tab)
        {
            return await editModalViewFunction(id, Tab);
        }
        public async Task<IActionResult> editModalViewFunction(int? id, int? Tab = 0)
        {
            string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }

            var parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 2).ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 2).ToListAsync();
            var vendors = await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == 2).Count() > 0).ToListAsync();
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();
            List<AppUtility.CommentTypeEnum> commentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts,
                Tab = Tab ?? 0,
                Comments = await _context.Comments
                .Include(r => r.ApplicationUser)
                .Where(r => r.Request.RequestID == id).ToListAsync(),
                CommentTypes = commentTypes
            };

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
                .SingleOrDefault(r => r.RequestID == id);

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
            //    requestItemViewModel.Debt = requestItemViewModel.Request.Cost;
            //}

            //setting the lists of companyaccounts by payment type id (so easy filtering on the frontend)

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
        //[Authorize(Roles = "Admin, Operation")]
        //public async Task<IActionResult> EditSummaryModalView(int? id, bool NewRequestFromProduct = false)
        //{

        //    //not imlemented yet
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Operations")]
        public async Task<IActionResult> EditModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
            requestItemViewModel.Request.ParentRequest = null;
            // requestItemViewModel.Request.ParentQuote.ParentQuoteID = (Int32)requestItemViewModel.Request.ParentQuoteID;
            //  var parentQuote = _context.ParentQuotes.Where(pq => pq.ParentQuoteID == requestItemViewModel.Request.ParentQuoteID).FirstOrDefault();
            // parentQuote.QuoteNumber = requestItemViewModel.Request.ParentQuote.QuoteNumber;
            // parentQuote.QuoteDate = requestItemViewModel.Request.ParentQuote.QuoteDate;
            // requestItemViewModel.Request.ParentQuote = parentQuote;

            var product = _context.Products.Include(p => p.Vendor).Include(p => p.ProductSubcategory).FirstOrDefault(v => v.ProductID == requestItemViewModel.Request.ProductID);
            product.ProductSubcategoryID = requestItemViewModel.Request.Product.ProductSubcategoryID;
            product.VendorID = requestItemViewModel.Request.Product.VendorID;
            product.ProductName = requestItemViewModel.Request.Product.ProductName;
            //in case we need to return to the modal view
            requestItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();

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

                // requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;
                try
                {
                    //_context.Update(requestItemViewModel.Request.Product.SubProject);
                    //_context.Update(requestItemViewModel.Request.Product);
                    requestItemViewModel.Request.Product = product;
                    _context.Update(requestItemViewModel.Request);
                    await _context.SaveChangesAsync();


                    try
                    {
                        foreach (var comment in requestItemViewModel.Comments)
                        {
                            if (!String.IsNullOrEmpty(comment.CommentText))
                            {
                                //save the new comment
                                comment.ApplicationUserID = currentUser.Id;
                                comment.CommentTimeStamp = DateTime.Now; //check if we actually need this line
                                comment.RequestID = requestItemViewModel.Request.RequestID;
                                _context.Update(comment);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        //Tell the user that the comment didn't save here
                    }


                    //Saving the Payments - each one should come in with a 1) date 2) companyAccountID
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
            //AppUtility.RequestPageTypeEnum requestPageTypeEnum = (AppUtility.RequestPageTypeEnum)requestItemViewModel.PageType;
            return RedirectToAction("Index", new
            {
                page = requestItemViewModel.Page,
                requestStatusID = requestItemViewModel.RequestStatusID,
                subcategoryID = requestItemViewModel.SubCategoryID,
                vendorID = requestItemViewModel.VendorID,
                applicationUserID = requestItemViewModel.ApplicationUserID,
                //PageType = requestPageTypeEnum
            });
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Operations")]
        public IActionResult Order(int id)
        {
            var request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(px => px.ParentCategory).FirstOrDefault();
            try
            {
                request.RequestStatusID = 2; //ordered
                ParentRequest parentRequest = new ParentRequest();
                parentRequest.ApplicationUserID = _userManager.GetUserId(User);
                int lastParentRequestOrderNum = 0;
                if (_context.ParentRequests.Any())
                {
                    lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber.Value;
                }
                parentRequest.OrderDate = DateTime.Now;
                parentRequest.OrderNumber = lastParentRequestOrderNum;
                request.ParentRequest = parentRequest;
                _context.Update(request);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }
            return RedirectToAction("Index", "Operations", new
            {
                requestStatusID = 2,
                PageType = AppUtility.RequestPageTypeEnum.Request
            });
        }


        private bool checkIfInBudget(Request request)
        {
            DateTime firstOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
            {//operational
                var pricePerUnit = request.Cost;
                if (pricePerUnit > request.ApplicationUserCreator.OperationUnitLimit)
                {
                    return false;
                }
                if (request.Cost > request.ApplicationUserCreator.OperationOrderLimit)
                {
                    return false;
                }

                var monthsSpending = _context.Requests
                    .Where(r => request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
                    .Where(r => r.ApplicationUserCreatorID == request.ApplicationUserCreatorID)
                    .Where(r => r.ParentRequest.OrderDate >= firstOfMonth)
                    .Sum(r => r.Cost);
                if (monthsSpending + request.Cost > request.ApplicationUserCreator.OperationMonthlyLimit)
                {
                    return false;
                }
                return true;
            }
            else
            {
                //probably will never happen should not have any lab here
                return false; //not any type of operation and therefore cannot be ordered without being approved
            }
        }
    }

}
