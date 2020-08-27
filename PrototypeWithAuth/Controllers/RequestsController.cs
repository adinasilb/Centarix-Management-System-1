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
//using Org.BouncyCastle.Asn1.X509;
//using System.Data.Entity.Validation;
//using System.Data.Entity.Infrastructure;

namespace PrototypeWithAuth.Controllers
{
    public class RequestsController : Controller
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
        public RequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        // GET: Requests
        //IMPORTANT!!! When adding more parameters into the Index Get make sure to add them to the ViewData and follow them through to the Index page
        //ALSO when changing defaults -> change the defaults on the index page for paged list 


        public async Task<IActionResult> Index(int? page, int RequestStatusID = 1, int subcategoryID = 0, int vendorID = 0, string applicationUserID = null, int parentLocationInstanceID = 0, AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, RequestsSearchViewModel? requestsSearchViewModel = null)
        {

            //instantiate your list of requests to pass into the index
            IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ApplicationUserCreator)
                .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).Include(r => r.ParentQuote)
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .OrderBy(r => r.CreationDate);
            //.Include(r=>r.UnitType).ThenInclude(ut => ut.UnitTypeDescription).Include(r=>r.SubUnitType).ThenInclude(sut => sut.UnitTypeDescription).Include(r=>r.SubSubUnitType).ThenInclude(ssut =>ssut.UnitTypeDescription); //inorder to display types of units

            TempData["RequestStatusID"] = RequestStatusID;
            var SidebarTitle = AppUtility.RequestSidebarEnum.None;

            int newCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 1, vendorID, subcategoryID, applicationUserID);
            int orderedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 2, vendorID, subcategoryID, applicationUserID);
            int receivedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 3, vendorID, subcategoryID, applicationUserID);
            int approvedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 6, vendorID, subcategoryID, applicationUserID);
            newCount += AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 4, vendorID, subcategoryID, applicationUserID);
            newCount += AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 5, vendorID, subcategoryID, applicationUserID);

            //use an iqueryable (not ienumerable) until it's passed in so you can include the vendors and subcategories later on
            IQueryable<Request> RequestsPassedIn = Enumerable.Empty<Request>().AsQueryable();
            //use an enum to determine which page type you are using and fill the data accordingly, 
            //also pass the data through tempdata to the page so you can 
            TempData["PageType"] = PageType;
            //instantiating the ints to keep track of the amounts- will then pass into tempdata to use on the frontend
            //if it is a request page --> get all the requests with a new or ordered request status

            if (ViewData["ReturnRequests"] != null)
            {
                RequestsPassedIn = TempData["ReturnRequests"] as IQueryable<Request>;
            }
            else if (PageType == AppUtility.RequestPageTypeEnum.Request)
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
            else if (PageType == AppUtility.RequestPageTypeEnum.Inventory)
            {
                //partial and clarify?
                RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3);
            }
            else if (PageType == AppUtility.RequestPageTypeEnum.Summary)
            {
                RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusID == 3).Include(r => r.Product.ProductSubcategory)
                     .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).ToList().GroupBy(r => r.ProductID).Select(e => e.First()).AsQueryable();
            }
            else
            {
                RequestsPassedIn = fullRequestsList;
            }

            //now that the lists are created sort by vendor or subcategory
            if (vendorID > 0 && requestsSearchViewModel != null)
            {
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.VendorID == vendorID);
                //pass the vendorID into the temp data to use if you'd like to sort from there
                SidebarTitle = AppUtility.RequestSidebarEnum.Vendor;
                TempData["VendorID"] = vendorID;
            }
            else if (subcategoryID > 0 && requestsSearchViewModel != null)
            {
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.ProductSubcategoryID == subcategoryID);
                //pass the subcategoryID into the temp data to use if you'd like to sort from there
                SidebarTitle = AppUtility.RequestSidebarEnum.Type;
                TempData["SubcategoryID"] = subcategoryID;
            }
            else if (applicationUserID != null && requestsSearchViewModel != null)
            {
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.ApplicationUserCreatorID == applicationUserID);
                //pass the subcategoryID into the temp data to use if you'd like to sort from there
                SidebarTitle = AppUtility.RequestSidebarEnum.Owner;
                TempData["ApplicationUserID"] = applicationUserID;
            }
            else if (parentLocationInstanceID > 0 && requestsSearchViewModel != null)
            {
                LocationInstance rliList = _context.LocationInstances
                    .Include(li => li.AllRequestLocationInstances)
                    .Where(li => li.LocationInstanceID == parentLocationInstanceID).FirstOrDefault();
                RequestsPassedIn = RequestsPassedIn.Where(r => rliList.AllRequestLocationInstances.Select(rli => rli.RequestID).ToList().Contains(r.RequestID));
                //RequestsPassedIn = RequestsPassedIn.Except(RequestsPassedIn.Where(r => rliList.RequestLocationInstances.se))
            }
            else
            {
                SidebarTitle = AppUtility.RequestSidebarEnum.LastItem;
            }


            //passing in the amounts to display in the top buttons
            TempData["AmountNew"] = newCount;
            TempData["AmountOrdered"] = orderedCount;
            TempData["AmountReceived"] = receivedCount;
            TempData["AmountApproved"] = approvedCount;

            TempData["SidebarTitle"] = SidebarTitle;


            //TRY USING TEMP DATA TO REMEMBER WHERE THE PAGE IS

            /*int?*/
            TempData["TempPage"] = page;
            /*int*/
            TempData["TempRequestStatusID"] = RequestStatusID;
            /*int*/
            TempData["TempSubcategoryID"] = subcategoryID;
            /*int*/
            TempData["TempVendorID"] = vendorID;
            /*string*/
            TempData["TempApplicationUserID"] = applicationUserID;
            /*AppUtility.RequestPageTypeEnum*/
            TempData["TempPageType"] = (int)PageType;
            /*RequestsSearchViewModel?*/
            //TempData["TempRequestsSearchViewModel"] = requestsSearchViewModel;
            TempData["ParentLocationInstanceID"] = parentLocationInstanceID;
            //Getting the page that is going to be seen (if no page was specified it will be one)
            var pageNumber = page ?? 1;
            var onePageOfProducts = Enumerable.Empty<Request>().ToPagedList();

            try
            {
                onePageOfProducts = await RequestsPassedIn.Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.Vendor).Include(r => r.RequestStatus).Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                    .ToPagedListAsync(pageNumber, 25);

                onePageOfProducts.OrderByDescending(opop => opop.ArrivalDate).Where(opop => opop.RequestStatusID == 5); // display by arrivaldate if recieved
                onePageOfProducts.Where(opop => opop.RequestStatusID == 2).OrderByDescending(opop => opop.ParentRequest.OrderDate); // display by orderdate if ordered
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }

            return View(onePageOfProducts);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> DeleteModal(int? id, bool isQuote = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
               .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).Include(r => r.Product.Vendor)
               .FirstOrDefaultAsync(m => m.RequestID == id);

            if (request == null)
            {
                return NotFound();
            }

            DeleteRequestViewModel deleteRequestViewModel = new DeleteRequestViewModel()
            {
                Request = request,
                IsReorder = isQuote
            };

            return View(deleteRequestViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> DeleteModal(DeleteRequestViewModel deleteRequestViewModel)
        {
            var request = _context.Requests.Where(r => r.RequestID == deleteRequestViewModel.Request.RequestID)
                .Include(r => r.RequestLocationInstances).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory)
                .ThenInclude(ps => ps.ParentCategory)
                .FirstOrDefault();
            request.IsDeleted = true;
            _context.Update(request);
            _context.SaveChanges();
            var parentRequest = _context.ParentRequests.Where(pr => pr.ParentRequestID == request.ParentRequestID).FirstOrDefault();
            if (parentRequest != null)
            {
                //todo figure out the soft delete with child of a parent entity so we could chnage it to 0 or null
                if (parentRequest.Requests.Count() <= 1)
                {
                    parentRequest.IsDeleted = true;
                    _context.Update(parentRequest);
                    await _context.SaveChangesAsync();
                }

            }
            var parentQuote = _context.ParentQuotes.Where(pr => pr.ParentQuoteID == request.ParentQuoteID).FirstOrDefault();
            if (parentQuote != null)
            {
                //todo figure out the soft delete with child of a parent entity so we could chnage it to 0 or null
                if (parentQuote.Requests.Count() <= 1)
                {
                    parentQuote.IsDeleted = true;
                    _context.Update(parentQuote);
                    await _context.SaveChangesAsync();
                }

            }
            foreach (var requestLocationInstance in request.RequestLocationInstances)
            {
                requestLocationInstance.IsDeleted = true;
                var locationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstance.LocationInstanceID).FirstOrDefault();
                locationInstance.IsFull = false;
                _context.Update(requestLocationInstance);
                _context.SaveChanges();
                _context.Update(locationInstance);
                _context.SaveChanges();
            }

            if (deleteRequestViewModel.IsReorder)
            {
                Reorder quote = (Reorder)request;
                if (quote.ParentQuote?.QuoteStatusID == 4)
                {
                    return RedirectToAction("LabManageOrders", new
                    {
                        RequestsByVendor = _context.Requests.OfType<Reorder>().Where(r => r.ParentQuote.QuoteStatusID == 4 && r.RequestStatusID == 6)
                  .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                  .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                  .Include(r => r.ApplicationUserCreator)
                  .ToLookup(r => r.Product.Vendor)
                    });
                }
                return RedirectToAction("LabManageQuotes", new
                {
                    RequestsByVendor = _context.Requests.OfType<Reorder>().Where(r => r.ParentQuote.QuoteStatusID == 3)
                  .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                  .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                  .Include(r => r.ApplicationUserCreator).Include(r => r.ParentQuote)
                  .ToLookup(r => r.Product.Vendor)
                });

            }
            else
            {
                if (request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                {
                    return RedirectToAction("Index", new
                    {
                        requestStatusID = request.RequestStatusID,
                        PageType = AppUtility.RequestPageTypeEnum.Request
                    });
                }
                else
                {
                    // AppUtility.RequestPageTypeEnum requestPageTypeEnum = (AppUtility.RequestPageTypeEnum)deleteRequestViewModel.PageType;
                    return RedirectToAction("Index", "Operations", new
                    {
                        requestStatusID = request.RequestStatusID,
                        PageType = AppUtility.RequestPageTypeEnum.Request
                    });
                }

            }

        }

        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> CreateModalView()
        {
            var parentcategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 1).ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(ps => ps.ParentCategory.CategoryTypeID == 1).ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var subprojects = await _context.SubProjects.ToListAsync();

            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var unittypeslookup = _context.UnitTypes.Include(u => u.UnitParentType).ToLookup(u => u.UnitParentType);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                Projects = projects,
                SubProjects = subprojects,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                UnitTypes = unittypeslookup,
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts
            };

            requestItemViewModel.Request = new Request();
            requestItemViewModel.Request.Product = new Product();
            requestItemViewModel.Request.ParentQuote = new ParentQuote();
            requestItemViewModel.Request.SubProject = new SubProject();

            requestItemViewModel.Request.ParentQuote.QuoteDate = DateTime.Now;
            requestItemViewModel.Request.CreationDate = DateTime.Now;

            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string requestFolder = Path.Combine(uploadFolder, "0");
            if (Directory.Exists(requestFolder))
            {
                Directory.Delete(requestFolder);
            }
            Directory.CreateDirectory(requestFolder);

            TempData["PageType"] = AppUtility.RequestPageTypeEnum.Request;
            TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.AddItem;
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> CreateModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //why do we need this here?
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.Include(ps => ps.ParentCategory).FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);
            //in case we need to return to the modal view
            requestItemViewModel.ParentCategories = await _context.ParentCategories.Where(pc => pc.CategoryTypeID == 1).ToListAsync();
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
            requestItemViewModel.Request.ParentQuote.QuoteDate = DateTime.Now;
            requestItemViewModel.Request.CreationDate = DateTime.Now;

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
                if (!User.IsInRole("Admin") && (OrderType.Equals("Ask For Permission") || !checkIfInBudget(requestItemViewModel.Request)))
                {
                    requestItemViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == requestItemViewModel.Request.SubProjectID).FirstOrDefault(); //Why do we need this here?
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
                    requestItemViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == requestItemViewModel.Request.SubProjectID).FirstOrDefault(); //Why do we need this here?
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
                        requestItemViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == requestItemViewModel.Request.SubProjectID).FirstOrDefault(); //Why do we need this here?
                        if (OrderType.Equals("Without Order"))
                        {
                            requestItemViewModel.Request.ParentRequest = new ParentRequest();
                            int lastParentRequestOrderNum = 0;
                            requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
                            if (_context.ParentRequests.Any())
                            {
                                lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber.Value;
                            }
                            requestItemViewModel.Request.ParentRequest.OrderNumber = lastParentRequestOrderNum + 1;
                            requestItemViewModel.Request.ParentRequest.OrderDate = DateTime.Now;
                            requestItemViewModel.Request.ParentRequest.WithoutOrder = true;
                            requestItemViewModel.Request.RequestStatusID = 2;
                            requestItemViewModel.RequestStatusID = 2;
                            requestItemViewModel.Request.ParentQuote = null;
                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                            RequestNotification requestNotification = new RequestNotification();
                            requestNotification.RequestID = requestItemViewModel.Request.RequestID;
                            requestNotification.IsRead = false;
                            requestNotification.RequestName = requestItemViewModel.Request.Product.ProductName;
                            requestNotification.ApplicationUserID = requestItemViewModel.Request.ApplicationUserCreatorID;
                            requestNotification.Description = "item ordered";
                            requestNotification.NotificationStatusID = 2;
                            requestNotification.TimeStamp = DateTime.Now;
                            requestNotification.Controller = "Requests";
                            requestNotification.Action = "NotificationsView";
                            requestNotification.OrderDate = DateTime.Now;
                            requestNotification.Vendor = requestItemViewModel.Request.Product.Vendor.VendorEnName;
                            _context.Update(requestNotification);
                            _context.SaveChanges();
                        }
                        else if (OrderType.Equals("Order"))
                        {
                            requestItemViewModel.Request.RequestStatusID = 1; //new request
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            requestItemViewModel.RequestStatusID = 1;

                            requestItemViewModel.Request.ParentRequest = new ParentRequest();
                            int lastParentRequestOrderNum = 0;
                            requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
                            if (_context.ParentRequests.Any())
                            {
                                lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber.Value;
                            }

                            _context.Add(requestItemViewModel.Request);
                            _context.SaveChanges();

                            TempData["OpenTermsModal"] = "Single";
                            //TempData["OpenConfirmEmailModal"] = true; //now we want it to go to the terms instead
                            TempData["RequestID"] = requestItemViewModel.Request.RequestID;
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
                    //var subprojectid = requestItemViewModel.Request.Product.SubProjectID;
                    //var subproject = requestItemViewModel.Request.Product.SubProject;
                    requestItemViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == requestItemViewModel.Request.SubProjectID).FirstOrDefault(); //Why do we need this here?
                    if (!String.IsNullOrEmpty(requestItemViewModel.NewComment.CommentText))
                    {
                        try
                        {
                            //save the new comment
                            requestItemViewModel.NewComment.ApplicationUserID = currentUser.Id;
                            requestItemViewModel.NewComment.CommentTimeStamp = DateTime.Now; //check if we actually need this line
                            requestItemViewModel.NewComment.RequestID = requestItemViewModel.Request.RequestID;
                            _context.Add(requestItemViewModel.NewComment);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            //do something here. comment didn't save
                        }
                    }

                    //rename temp folder to the request id
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    string requestFolderFrom = Path.Combine(uploadFolder, "0");
                    string requestFolderTo = Path.Combine(uploadFolder, requestItemViewModel.Request.RequestID.ToString());
                    Directory.Move(requestFolderFrom, requestFolderTo);
                    //check if there are any files to upload first
                    //save the files
                    //string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    //string requestFolder = Path.Combine(uploadFolder, requestItemViewModel.Request.RequestID.ToString());
                    //Directory.CreateDirectory(requestFolder);
                    //if (requestItemViewModel.OrderFiles != null) //test for more than one???
                    //{
                    //    var x = 1;
                    //    foreach (IFormFile orderfile in requestItemViewModel.OrderFiles)
                    //    {
                    //        //create file
                    //        string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Orders.ToString());
                    //        Directory.CreateDirectory(folderPath);
                    //        string uniqueFileName = x + orderfile.FileName;
                    //        string filePath = Path.Combine(folderPath, uniqueFileName);
                    //        orderfile.CopyTo(new FileStream(filePath, FileMode.Create));
                    //        x++;
                    //    }
                    //}
                    //if (requestItemViewModel.InvoiceFiles != null) //test for more than one???
                    //{
                    //    var x = 1;
                    //    foreach (IFormFile invoiceFile in requestItemViewModel.InvoiceFiles)
                    //    {
                    //        //create file
                    //        string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Invoices.ToString());
                    //        Directory.CreateDirectory(folderPath);
                    //        string uniqueFileName = x + invoiceFile.FileName;
                    //        string filePath = Path.Combine(folderPath, uniqueFileName);
                    //        invoiceFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    //        x++;
                    //    }
                    //}
                    //if (requestItemViewModel.ShipmentFiles != null) //test for more than one???
                    //{
                    //    var x = 1;
                    //    foreach (IFormFile shipmentFile in requestItemViewModel.ShipmentFiles)
                    //    {
                    //        //create file
                    //        string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Shipments.ToString());
                    //        Directory.CreateDirectory(folderPath);
                    //        string uniqueFileName = x + shipmentFile.FileName;
                    //        string filePath = Path.Combine(folderPath, uniqueFileName);
                    //        shipmentFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    //        x++;
                    //    }
                    //}
                    //if (requestItemViewModel.QuoteFiles != null) //test for more than one???
                    //{
                    //    var x = 1;
                    //    foreach (IFormFile quoteFile in requestItemViewModel.QuoteFiles)
                    //    {
                    //        //create file
                    //        string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Quotes.ToString());
                    //        Directory.CreateDirectory(folderPath);
                    //        string uniqueFileName = x + quoteFile.FileName;
                    //        string filePath = Path.Combine(folderPath, uniqueFileName);
                    //        quoteFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    //        x++;
                    //    }
                    //}
                    //if (requestItemViewModel.InfoFiles != null) //test for more than one???
                    //{
                    //    var x = 1;
                    //    foreach (IFormFile infoFile in requestItemViewModel.InfoFiles)
                    //    {
                    //        //create file
                    //        string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Info.ToString());
                    //        Directory.CreateDirectory(folderPath);
                    //        string uniqueFileName = x + infoFile.FileName;
                    //        string filePath = Path.Combine(folderPath, uniqueFileName);
                    //        infoFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    //        x++;
                    //    }
                    //}
                    //if (requestItemViewModel.PictureFiles != null) //test for more than one???
                    //{
                    //    var x = 1;
                    //    foreach (IFormFile pictureFile in requestItemViewModel.PictureFiles)
                    //    {
                    //        //create file
                    //        string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Pictures.ToString());
                    //        Directory.CreateDirectory(folderPath);
                    //        string uniqueFileName = x + pictureFile.FileName;
                    //        string filePath = Path.Combine(folderPath, uniqueFileName);
                    //        pictureFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    //        x++;
                    //    }
                    //}

                    return RedirectToAction("Index", new
                    {
                        //page = requestItemViewModel.Page, //don't need this here b/c create is not a modal anymore
                        requestStatusID = requestItemViewModel.Request.RequestStatusID,
                        PageType = AppUtility.RequestPageTypeEnum.Request
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
                TempData["InnerMessage"] = "The request model failed to validate. Please ensure that all fields were filled in correctly";
                return View("~/Views/Shared/RequestError.cshtml");
            }
        }


        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> DetailsModalView(int? id, bool NewRequestFromProduct = false)
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

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Projects = projects,
                SubProjects = subprojects,
                Vendors = vendors,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
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
                    .Include(r => r.SubProject)
                    .ThenInclude(sp => sp.Project)
                    .SingleOrDefault(x => x.RequestID == id);

                //check if this works once there are commments
                var comments = Enumerable.Empty<Comment>();
                comments = _context.Comments
                    .Include(c => c.ApplicationUser)
                    .Where(c => c.Request.RequestID == id);
                //needs to be instantiated here so it doesn't throw an error if nothing is in it
                /*
                 *I think it should be an ienumerable and look like
                 *requestItemViewModel.Comments = new Enumerable.Empty<Comment>(); 
                 *ike before but it's not recognizing the syntax
                */
                requestItemViewModel.OldComments = comments.ToList();

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
                        string newFileString = AppUtility.GetLastFourFiles(orderfile.FullName);
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
                        string newFileString = AppUtility.GetLastFourFiles(invoicefile.FullName);
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
                        string newFileString = AppUtility.GetLastFourFiles(shipmentfile.FullName);
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
                        string newFileString = AppUtility.GetLastFourFiles(quotefile.FullName);
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
                        string newFileString = AppUtility.GetLastFourFiles(infofile.FullName);
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
                        string newFileString = AppUtility.GetLastFourFiles(picturefile.FullName);
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
                        string newFileString = AppUtility.GetLastFourFiles(returnfile.FullName);
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
                        string newFileString = AppUtility.GetLastFourFiles(creditfile.FullName);
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


                //locations:
                //get the list of requestLocationInstances in this request
                //can't look for _context.RequestLocationInstances b/c it's a join table and doesn't have a dbset
                var request1 = _context.Requests.Where(r => r.RequestID == id).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).FirstOrDefault();
                var requestLocationInstances = request1.RequestLocationInstances.ToList();
                //if it has => (which it should once its in a details view)
                if (requestLocationInstances.Any())
                {
                    //get the parent location instances of the first one
                    //can do this now b/c can only be in one box - later on will have to be a list or s/t b/c will have more boxes
                    //int? locationInstanceParentID = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstanceID).FirstOrDefault().LocationInstanceParentID;
                    requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                    //requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                    //need to test b/c the model is int? which is nullable
                    if (requestItemViewModel.ParentLocationInstance != null)
                    {
                        //inserting list of childrenslocationinstances to show on the frontend
                        requestItemViewModel.ChildrenLocationInstances = _context.LocationInstances
                            .Where(li => li.LocationInstanceParentID == requestItemViewModel.ParentLocationInstance.LocationInstanceID)
                            .Include(li => li.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();
                        //var x = 0; //place in cli
                        //requestItemViewModel.ChildrenLocationInstancesRequests = new List<Request>();
                        //foreach (var cli in requestItemViewModel.ChildrenLocationInstances)
                        //{
                        //    var req = _context.Requests
                        //        .Include(r => r.RequestLocationInstances.Select(rli => rli.LocationInstanceID == cli.LocationInstanceID)).Include(r => r.Product)
                        //        .FirstOrDefault();
                        //    if (req != null)
                        //    {
                        //        requestItemViewModel.ChildrenLocationInstancesRequests.Add(req);
                        //    }
                        //}

                    }
                }

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

        [Authorize(Roles = "Admin, OrdersAndInventory")]
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

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Projects = projects,
                SubProjects = subprojects,
                Vendors = vendors,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
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
                //check if this works once there are commments
                var comments = Enumerable.Empty<Comment>();
                comments = _context.Comments
                    .Include(c => c.ApplicationUser)
                    .Where(c => c.Request.RequestID == id);
                //needs to be instantiated here so it doesn't throw an error if nothing is in it
                /*
                 *I think it should be an ienumerable and look like
                 *requestItemViewModel.Comments = new Enumerable.Empty<Comment>(); 
                 *ike before but it's not recognizing the syntax
                */
                requestItemViewModel.OldComments = comments.ToList();

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
                        string newFileString = AppUtility.GetLastFourFiles(infofile.FullName);
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
                        string newFileString = AppUtility.GetLastFourFiles(picturefile.FullName);
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


                //locations:
                //get the list of requestLocationInstances in this request
                //can't look for _context.RequestLocationInstances b/c it's a join table and doesn't have a dbset
                var request1 = _context.Requests.Where(r => r.RequestID == id).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).FirstOrDefault();
                var requestLocationInstances = request1.RequestLocationInstances.ToList();
                //if it has => (which it should once its in a details view)
                if (requestLocationInstances.Any())
                {
                    //get the parent location instances of the first one
                    //can do this now b/c can only be in one box - later on will have to be a list or s/t b/c will have more boxes
                    //int? locationInstanceParentID = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstanceID).FirstOrDefault().LocationInstanceParentID;
                    requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                    //requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                    //need to test b/c the model is int? which is nullable
                    if (requestItemViewModel.ParentLocationInstance != null)
                    {
                        //inserting list of childrenslocationinstances to show on the frontend
                        requestItemViewModel.ChildrenLocationInstances = _context.LocationInstances
                            .Where(li => li.LocationInstanceParentID == requestItemViewModel.ParentLocationInstance.LocationInstanceID)
                            .Include(li => li.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();
                        //var x = 0; //place in cli
                        //requestItemViewModel.ChildrenLocationInstancesRequests = new List<Request>();
                        //foreach (var cli in requestItemViewModel.ChildrenLocationInstances)
                        //{
                        //    var req = _context.Requests
                        //        .Include(r => r.RequestLocationInstances.Select(rli => rli.LocationInstanceID == cli.LocationInstanceID)).Include(r => r.Product)
                        //        .FirstOrDefault();
                        //    if (req != null)
                        //    {
                        //        requestItemViewModel.ChildrenLocationInstancesRequests.Add(req);
                        //    }
                        //}

                    }
                }

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

        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public ActionResult DownloadPDF(string filename)
        {
            //string filename = orderFileInfo.FullName.ToString();
            string concatShortFilename = "inline; filename=" +
                filename.Substring(filename.LastIndexOf("\\") + 2); //follow through with this
            Response.Headers.Add("Content-Disposition", concatShortFilename);
            return File(filename, "application/pdf");
        }


        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> EditModalView(int? id, bool NewRequestFromProduct = false)
        {
            string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }

            var parentcategories = await _context.ParentCategories.ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                Projects = projects,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts
            };

            ModalViewType = "Edit";

            requestItemViewModel.Request = _context.Requests.Include(r => r.Product)
                .Include(r => r.ParentQuote)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product.ProductSubcategory)
                .Include(r => r.Product.ProductSubcategory.ParentCategory)
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

            var comments = Enumerable.Empty<Comment>();
            comments = _context.Comments
                .Include(r => r.ApplicationUser)
                .Where(r => r.Request.RequestID == id);
            //needs to be instantiated here so it doesn't throw an error if nothing is in it
            /*
             *I think it should be an ienumerable and look like
             *requestItemViewModel.Comments = new Enumerable.Empty<Comment>(); 
             *ike before but it's not recognizing the syntax
            */
            requestItemViewModel.OldComments = comments.ToList();

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
                    string newFileString = AppUtility.GetLastFourFiles(orderfile.FullName);
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
                    string newFileString = AppUtility.GetLastFourFiles(invoicefile.FullName);
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
                    string newFileString = AppUtility.GetLastFourFiles(shipmentfile.FullName);
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
                    string newFileString = AppUtility.GetLastFourFiles(quotefile.FullName);
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
                    string newFileString = AppUtility.GetLastFourFiles(infofile.FullName);
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
                    string newFileString = AppUtility.GetLastFourFiles(picturefile.FullName);
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
                    string newFileString = AppUtility.GetLastFourFiles(returnfile.FullName);
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
                    string newFileString = AppUtility.GetLastFourFiles(creditfile.FullName);
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
            //can't look for _context.RequestLocationInstances b/c it's a join table and doesn't have a dbset
            var request1 = _context.Requests.Where(r => r.RequestID == id).Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).FirstOrDefault();
            var requestLocationInstances = request1.RequestLocationInstances.ToList();
            //if it has => (which it should once its in a details view)
            if (requestLocationInstances.Any())
            {
                //get the parent location instances of the first one
                //can do this now b/c can only be in one box - later on will have to be a list or s/t b/c will have more boxes
                //int? locationInstanceParentID = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstanceID).FirstOrDefault().LocationInstanceParentID;
                requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                //requestItemViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == requestLocationInstances[0].LocationInstance.LocationInstanceParentID).FirstOrDefault();
                //need to test b/c the model is int? which is nullable
                if (requestItemViewModel.ParentLocationInstance != null)
                {
                    //inserting list of childrenslocationinstances to show on the frontend
                    requestItemViewModel.ChildrenLocationInstances = _context.LocationInstances
                        .Where(li => li.LocationInstanceParentID == requestItemViewModel.ParentLocationInstance.LocationInstanceID)
                        .Include(li => li.RequestLocationInstances).ThenInclude(rli => rli.Request).ThenInclude(r => r.Product).ToList();
                    //var x = 0; //place in cli
                    //requestItemViewModel.ChildrenLocationInstancesRequests = new List<Request>();
                    //foreach (var cli in requestItemViewModel.ChildrenLocationInstances)
                    //{
                    //    var req = _context.Requests
                    //        .Include(r => r.RequestLocationInstances.Select(rli => rli.LocationInstanceID == cli.LocationInstanceID)).Include(r => r.Product)
                    //        .FirstOrDefault();
                    //    if (req != null)
                    //    {
                    //        requestItemViewModel.ChildrenLocationInstancesRequests.Add(req);
                    //    }
                    //}

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
            if (AppUtility.IsAjaxRequest(this.Request))
            {
                return PartialView(requestItemViewModel);
            }
            else
            {
                return View(requestItemViewModel);
            }
        }

        //[Authorize(Roles = "Admin, OrdersAndInventory")]
        //public async Task<IActionResult> EditSummaryModalView(int? id, bool NewRequestFromProduct = false)
        //{

        //    //not imlemented yet
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> EditModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
            requestItemViewModel.Request.ParentRequest = null;
            requestItemViewModel.Request.ParentQuote.ParentQuoteID = (Int32)requestItemViewModel.Request.ParentQuoteID;
            var parentQuote = _context.ParentQuotes.Where(pq => pq.ParentQuoteID == requestItemViewModel.Request.ParentQuoteID).FirstOrDefault();
            parentQuote.QuoteNumber = requestItemViewModel.Request.ParentQuote.QuoteNumber;
            parentQuote.QuoteDate = requestItemViewModel.Request.ParentQuote.QuoteDate;
            requestItemViewModel.Request.ParentQuote = parentQuote;
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

            //in case we need to return to the modal view
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
                requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;
                requestItemViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == requestItemViewModel.Request.SubProjectID).FirstOrDefault();
                try
                {
                    //_context.Update(requestItemViewModel.Request.Product.SubProject);
                    //_context.Update(requestItemViewModel.Request.Product);
                    _context.Update(requestItemViewModel.Request);
                    await _context.SaveChangesAsync();

                    if (!String.IsNullOrEmpty(requestItemViewModel.NewComment.CommentText))
                    {
                        try
                        {
                            //save the new comment
                            requestItemViewModel.NewComment.ApplicationUserID = currentUser.Id;
                            requestItemViewModel.NewComment.CommentTimeStamp = DateTime.Now; //check if we actually need this line
                            requestItemViewModel.NewComment.RequestID = requestItemViewModel.Request.RequestID;
                            _context.Add(requestItemViewModel.NewComment);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            //Tell the user that the comment didn't save here
                        }
                    }

                    //check if there are any files to upload first
                    //save the files
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
                    string requestFolder = Path.Combine(uploadFolder, requestItemViewModel.Request.RequestID.ToString());
                    Directory.CreateDirectory(requestFolder);
                    if (requestItemViewModel.OrderFiles != null) //test for more than one???
                    {
                        var x = 1;
                        foreach (IFormFile orderfile in requestItemViewModel.OrderFiles)
                        {
                            //create file
                            string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Orders.ToString());
                            Directory.CreateDirectory(folderPath);
                            string uniqueFileName = x + orderfile.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            orderfile.CopyTo(new FileStream(filePath, FileMode.Create));
                            x++;
                        }
                    }
                    if (requestItemViewModel.InvoiceFiles != null) //test for more than one???
                    {
                        var x = 1;
                        foreach (IFormFile invoiceFile in requestItemViewModel.InvoiceFiles)
                        {
                            //create file
                            string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Invoices.ToString());
                            Directory.CreateDirectory(folderPath);
                            string uniqueFileName = x + invoiceFile.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            invoiceFile.CopyTo(new FileStream(filePath, FileMode.Create));
                            x++;
                        }
                    }
                    if (requestItemViewModel.ShipmentFiles != null) //test for more than one???
                    {
                        var x = 1;
                        foreach (IFormFile shipmentFile in requestItemViewModel.ShipmentFiles)
                        {
                            //create file
                            string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Shipments.ToString());
                            Directory.CreateDirectory(folderPath);
                            string uniqueFileName = x + shipmentFile.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            shipmentFile.CopyTo(new FileStream(filePath, FileMode.Create));
                            x++;
                        }
                    }
                    if (requestItemViewModel.QuoteFiles != null) //test for more than one???
                    {
                        var x = 1;
                        foreach (IFormFile quoteFile in requestItemViewModel.QuoteFiles)
                        {
                            //create file
                            string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Quotes.ToString());
                            Directory.CreateDirectory(folderPath);
                            string uniqueFileName = x + quoteFile.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            quoteFile.CopyTo(new FileStream(filePath, FileMode.Create));
                            x++;
                        }
                    }
                    if (requestItemViewModel.InfoFiles != null) //test for more than one???
                    {
                        var x = 1;
                        foreach (IFormFile infoFile in requestItemViewModel.InfoFiles)
                        {
                            //create file
                            string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Info.ToString());
                            Directory.CreateDirectory(folderPath);
                            string uniqueFileName = x + infoFile.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            infoFile.CopyTo(new FileStream(filePath, FileMode.Create));
                            x++;
                        }
                    }
                    if (requestItemViewModel.PictureFiles != null) //test for more than one???
                    {
                        var x = 1;
                        foreach (IFormFile pictureFile in requestItemViewModel.PictureFiles)
                        {
                            //create file
                            string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Pictures.ToString());
                            Directory.CreateDirectory(folderPath);
                            string uniqueFileName = x + pictureFile.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            pictureFile.CopyTo(new FileStream(filePath, FileMode.Create));
                            x++;
                        }
                    }
                    if (requestItemViewModel.ReturnFiles != null) //test for more than one???
                    {
                        var x = 1;
                        foreach (IFormFile returnFile in requestItemViewModel.ReturnFiles)
                        {
                            //create file
                            string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Returns.ToString());
                            Directory.CreateDirectory(folderPath);
                            string uniqueFileName = x + returnFile.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            returnFile.CopyTo(new FileStream(filePath, FileMode.Create));
                            x++;
                        }
                    }
                    if (requestItemViewModel.CreditFiles != null) //test for more than one???
                    {
                        var x = 1;
                        foreach (IFormFile creditFile in requestItemViewModel.CreditFiles)
                        {
                            //create file
                            string folderPath = Path.Combine(requestFolder, AppUtility.RequestFolderNamesEnum.Credits.ToString());
                            Directory.CreateDirectory(folderPath);
                            string uniqueFileName = x + creditFile.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            creditFile.CopyTo(new FileStream(filePath, FileMode.Create));
                            x++;
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
            AppUtility.RequestPageTypeEnum requestPageTypeEnum = (AppUtility.RequestPageTypeEnum)requestItemViewModel.PageType;
            return RedirectToAction("Index", new
            {
                page = requestItemViewModel.Page,
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

        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> ReOrderFloatModalView(int? id, bool NewRequestFromProduct = false)
        {
            var parentcategories = await _context.ParentCategories.ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            var subprojects = await _context.SubProjects.ToListAsync();

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
                Request = request
            };

            //initiating the  following models so that we can use them in an asp-for in the view 
            return PartialView(requestItemViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> ReOrderFloatModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //get the old request that we are reordering
            var oldRequest = _context.Requests.Where(r => r.RequestID == requestItemViewModel.Request.RequestID)
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
            reorderRequest.Cost = requestItemViewModel.Request.Cost;
            reorderRequest.Currency = oldRequest.Currency;
            reorderRequest.CatalogNumber = oldRequest.CatalogNumber;
            reorderRequest.RequestStatusID = 1; //waiting approval status of new
            reorderRequest.UnitTypeID = requestItemViewModel.Request.UnitTypeID;
            reorderRequest.Unit = requestItemViewModel.Request.Unit;
            reorderRequest.SubSubUnit = requestItemViewModel.Request.SubSubUnit;
            reorderRequest.SubUnit = requestItemViewModel.Request.SubUnit;
            reorderRequest.SubUnitTypeID = requestItemViewModel.Request.SubUnitTypeID;
            reorderRequest.SubSubUnitTypeID = requestItemViewModel.Request.SubSubUnitTypeID;
            reorderRequest.UnitsOrdered = oldRequest.UnitsOrdered;
            reorderRequest.UnitsInStock = oldRequest.UnitsInStock;
            reorderRequest.Quantity = oldRequest.Quantity;
            reorderRequest.VAT = requestItemViewModel.Request.VAT;
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

                    await populateRequestItemViewModel(requestItemViewModel, oldRequest);
                    return PartialView(requestItemViewModel);
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.InnerException.ToString();

                    await populateRequestItemViewModel(requestItemViewModel, oldRequest);
                    return PartialView(requestItemViewModel);
                }
            }
            else
            {
                //in case we need to redirect to action
                //TempData["ModalView"] = true;
                TempData["RequestID"] = requestItemViewModel.Request.RequestID;

                await populateRequestItemViewModel(requestItemViewModel, oldRequest);
                return PartialView(requestItemViewModel);
            }
            return RedirectToAction("Index", new
            {
                page = requestItemViewModel.Page,
                requestStatusID = 1,
                PageType = AppUtility.RequestPageTypeEnum.Request
            });
        }

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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> TermsModal(int id, bool isSingleRequest = false, bool IsCart = false) //either it'll be a request or parentrequest and then it'll send it to all the requests in that parent request
        {
            //TODO: add temp data memory here 
            int lastParentRequestOrderNum = 0;
            if (_context.ParentRequests.Any())
            {
                lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber.Value;
            }
            ParentRequest pr = new ParentRequest()
            {
                ApplicationUserID = _userManager.GetUserId(User),
                OrderNumber = lastParentRequestOrderNum + 1,
                OrderDate = DateTime.Now
            };
            _context.Add(pr);
            await _context.SaveChangesAsync();
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
            if (isSingleRequest)
            {
                var request = _context.Requests.Where(r => r.RequestID == id).FirstOrDefault();
                request.ParentRequestID = termsViewModel.ParentRequest.ParentRequestID;
                _context.Update(request);
                await _context.SaveChangesAsync();
            }
            else if (IsCart)
            {
                //termsViewModel.ParentRequest = new ParentRequest();
                var requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                        .Where(r => r.Product.VendorID == id && r.RequestStatusID == 6 && !(r is Reorder))
                        .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
                              .Include(r => r.Product).ThenInclude(r => r.Vendor).ToListAsync();
                foreach (Request req in requests)
                {
                    req.ParentRequestID = termsViewModel.ParentRequest.ParentRequestID;
                    _context.Update(req);
                    await _context.SaveChangesAsync();
                }
            }
            return PartialView(termsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> TermsModal(TermsViewModel termsViewModel)
        {
            _context.Update(termsViewModel.ParentRequest);
            await _context.SaveChangesAsync();

            var requests = _context.Requests.Where(r => r.ParentRequestID == termsViewModel.ParentRequest.ParentRequestID);

            if (termsViewModel.NewPayments != null)
            {
                foreach (var payment in termsViewModel.NewPayments)
                {
                    payment.CompanyAccount = _context.CompanyAccounts.Where(ca => ca.CompanyAccountID == payment.CompanyAccountID).FirstOrDefault();
                    payment.ParentRequestID = termsViewModel.ParentRequest.ParentRequestID;
                    _context.Add(payment);
                }
                await _context.SaveChangesAsync();
            };

            if (requests != null)
            {
                foreach (var request in requests)
                {
                    if (termsViewModel.Paid)
                    {
                        request.PaymentStatusID = 6;
                    }
                    else if (termsViewModel.Terms == "0")
                    {
                        request.PaymentStatusID = 3;
                    }
                    else if (termsViewModel.Terms == "15" || termsViewModel.Terms == "30" || termsViewModel.Terms == "45")
                    {
                        request.PaymentStatusID = 4;
                    }
                    else if (termsViewModel.Installments > 0) //again : should we check if it needs more than 1?
                    {
                        request.PaymentStatusID = 5;
                        //the payments don't go here otherwise it would add for every request (needs to be added just once for the parent request)
                    }
                    else
                    {
                        request.PaymentStatusID = 2;
                    }
                    _context.Update(request);
                }
                await _context.SaveChangesAsync();
            }
            TempData["ParentRequestConfirmEmail"] = true;
            TempData["ParentRequestID"] = termsViewModel.ParentRequest.ParentRequestID;
            return RedirectToAction("Index"); //todo: put in tempdata memory here
            //return RedirectToAction("ConfirmEmailModal", new { id = termsViewModel.ParentRequest.ParentRequestID });
        }

        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> ConfirmEmailModal(int id)
        {
            //List<Request> requests = null;
            //if (isSingleOrder)
            //{
            //    requests = await _context.Requests.Where(r => r.RequestID == id)
            //   .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
            //}
            //else
            //{
            //    if (cart)
            //    {
            //        requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
            //            .Where(r => r.Product.VendorID == id && r.RequestStatusID == 6 && !(r is Reorder))
            //            .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
            //                 .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
            //                 .Include(r => r.Product).ThenInclude(r => r.Vendor).ToListAsync();
            //    }
            //    else
            //    {
            //        requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1).Where(r => r.Product.VendorID == id && r.RequestStatusID == 6 && !(r is Reorder))
            //                  .Include(r => r.Product).ThenInclude(r => r.Vendor).ToListAsync();
            //    }

            //}
            //ParentRequest parentRequest = new ParentRequest();
            //foreach (var request in parentRequest.Requests)
            //{
            //    request.ParentRequest = parentRequest;
            //    int lastParentRequestOrderNum = 0;
            //    request.ParentRequest.ApplicationUserID = _userManager.GetUserId(User);
            //    if (_context.ParentRequests.Any())
            //    {
            //        lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber.Value;
            //    }
            //    request.ParentRequest.OrderNumber = lastParentRequestOrderNum + 1;
            //    request.ParentRequest.OrderDate = DateTime.Now;
            //}

            ConfirmEmailViewModel confirm = new ConfirmEmailViewModel
            {
                ParentRequest = _context.ParentRequests.Where(pr => pr.ParentRequestID == id)
                    .Include(pr => pr.Requests).ThenInclude(r => r.Product).ThenInclude(p => p.Vendor)
                    .Include(pr => pr.Requests).ThenInclude(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                    .FirstOrDefault(),
                //Requests = parentRequest.Requests.ToList(),
                VendorId = id,
                RequestID = id,
                //IsSingleOrder = isSingleOrder,
                //Cart = cart
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
            foreach (var request in confirm.ParentRequest.Requests)
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
            TempData["ParentRequestConfirmEmail"] = null;

            return PartialView(confirm);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> ConfirmEmailModal(ConfirmEmailViewModel confirmEmail)
        {
            //List<Request> requests = null;
            //if (confirmEmail.IsSingleOrder)
            //{
            //    requests = await _context.Requests.Where(r => r.RequestID == confirmEmail.RequestID)
            //   .Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
            //   .Include(r => r.ApplicationUserCreator).ToListAsync();
            //}
            //else
            //{
            //    if (confirmEmail.Cart)
            //    {
            //        requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
            //            .Where(r => r.Product.VendorID == confirmEmail.VendorID && r.RequestStatusID == 6 && !(r is Reorder))
            //            .Where(r => r.ApplicationUserCreatorID == _userManager.GetUserId(User))
            //                  .Include(r => r.Product).ThenInclude(r => r.Vendor)
            //                  .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ToListAsync();
            //    }
            //    else
            //    {
            //        requests = await _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1).Where(r => r.Product.VendorID == confirmEmail.VendorID && r.RequestStatusID == 6 && !(r is Reorder))
            //                  .Include(r => r.Product).ThenInclude(r => r.Vendor).ToListAsync();
            //    }
            //}

            var firstRequest = _context.Requests.Where(r => r.ParentRequestID == confirmEmail.ParentRequest.ParentRequestID)
                .Include(r => r.Product).ThenInclude(p => p.Vendor)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).FirstOrDefault();

            string uploadFolder1 = Path.Combine("~", "files");
            string uploadFolder = Path.Combine("wwwroot", "files");
            string uploadFolder2 = Path.Combine(uploadFolder, firstRequest.RequestID.ToString());
            string uploadFolder3 = Path.Combine(uploadFolder2, "Orders");
            string uploadFile = Path.Combine(uploadFolder3, "OrderPDF.pdf");

            if (System.IO.File.Exists(uploadFile))
            {
                //instatiate mimemessage
                var message = new MimeMessage();

                //instantiate the body builder
                var builder = new BodyBuilder();


                //var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
                var currentUser = _context.Users.Where(u => u.Id == "702fe06c-22e1-4be8-a515-ea89d6e5ee00").FirstOrDefault();
                string ownerEmail = currentUser.Email;
                string ownerUsername = currentUser.FirstName + " " + currentUser.LastName;
                string ownerPassword = currentUser.SecureAppPass;
                string vendorEmail = firstRequest.Product.Vendor.OrdersEmail;
                string vendorName = firstRequest.Product.Vendor.VendorEnName;

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
                    //var SecureAppPass = _context.Users.Where(u => u.Id == confirmEmail.ParentRequest.ApplicationUserID).FirstOrDefault().SecureAppPass;
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
                        foreach (var request in _context.Requests.Where(r => r.ParentRequestID == confirmEmail.ParentRequest.ParentRequestID).Include(r=>r.Product).ThenInclude(p=>p.Vendor))
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
                            _context.SaveChanges();

                        }
                        await _context.SaveChangesAsync();
                        //foreach (var request in requests)
                        //{
                        //    ParentRequest parentRequest = new ParentRequest();
                        //    request.ParentRequest = parentRequest;
                        //    int lastParentRequestOrderNum = 0;
                        //    request.ParentRequest.ApplicationUserID = currentUser.Id;
                        //    if (_context.ParentRequests.Any())
                        //    {
                        //        lastParentRequestOrderNum = _context.ParentRequests.OrderByDescending(x => x.OrderNumber).FirstOrDefault().OrderNumber.Value;
                        //    }
                        //    request.ParentRequest.OrderNumber = lastParentRequestOrderNum + 1;
                        //    request.ParentRequest.OrderDate = DateTime.Now;
                        //    requests.FirstOrDefault().RequestStatusID = 2;
                        //    _context.Update(request);
                        //    await _context.SaveChangesAsync();
                        //}
                     
                    }

                }

                AppUtility.RequestPageTypeEnum requestPageTypeEnum = (AppUtility.RequestPageTypeEnum)confirmEmail.PageType;
                if (firstRequest.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                {
                    TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.LastItem;
                    //return RedirectToAction("Index", new
                    //{
                    //    page = confirmEmail.Page,
                    //    requestStatusID = 2,
                    //    PageType = AppUtility.RequestPageTypeEnum.Request
                    //});
                    return RedirectToAction("Index"); //temp: todo: must add Tempdata
                }
                else
                {
                    TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.LastItem;
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


        /*
         * END SEND EMAIL
         */

        [HttpPost]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
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
                          .Include(r => r.Product).ThenInclude(r => r.Vendor).ToList();
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

        [Authorize(Roles = "Admin, OrdersAndInventory")]
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
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
        [Authorize(Roles = "Admin, LabManagement")]
        public async Task<IActionResult> LabManageQuotes()
        {
            LabManageQuotesViewModel labManageQuotesViewModel = new LabManageQuotesViewModel();
            labManageQuotesViewModel.RequestsByVendor = _context.Requests.OfType<Reorder>().Where(r => r.ParentQuote.QuoteStatusID == 1 || r.ParentQuote.QuoteStatusID == 2)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                .Include(r => r.ParentQuote).Include(r => r.ApplicationUserCreator)
                .ToLookup(r => r.Product.Vendor);
            TempData["PageType"] = AppUtility.LabManagementPageTypeEnum.Quotes;
            TempData["SideBarPageType"] = AppUtility.LabManagementSidebarEnum.Quotes;
            return View(labManageQuotesViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, LabManagement")]
        public async Task<IActionResult> LabManageOrders()
        {
            LabManageQuotesViewModel labManageQuotesViewModel = new LabManageQuotesViewModel();
            labManageQuotesViewModel.RequestsByVendor = _context.Requests.OfType<Reorder>().Where(r => r.ParentQuote.QuoteStatusID == 4 && r.RequestStatusID == 6)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.Product.ProductSubcategory)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType).Include(r => r.ApplicationUserCreator)
                .ToLookup(r => r.Product.Vendor);
            TempData["PageType"] = AppUtility.LabManagementPageTypeEnum.Quotes;
            TempData["SideBarPageType"] = AppUtility.LabManagementSidebarEnum.Orders;
            return View(labManageQuotesViewModel);
        }

        /*
         * BEGIN SEARCH
         */
        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> Search(AppUtility.MenuItems SectionType)
        {
            TempData["PageType"] = AppUtility.RequestPageTypeEnum.Search;
            TempData["SectionType"] = SectionType;
            RequestsSearchViewModel requestsSearchViewModel = new RequestsSearchViewModel
            {
                ParentCategories = await _context.ParentCategories.ToListAsync(),
                ProductSubcategories = await _context.ProductSubcategories.ToListAsync(),
                Projects = await _context.Projects.ToListAsync(),
                SubProjects = await _context.SubProjects.ToListAsync(),
                Vendors = await _context.Vendors.ToListAsync(),
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
        [Authorize(Roles = "Admin, OrdersAndInventory, Operations")]
        public async Task<IActionResult> Search(RequestsSearchViewModel requestsSearchViewModel, int? page)
        {
            var categoryType = requestsSearchViewModel.SectionType == AppUtility.MenuItems.Operation ? 2 : 1;
            int RSRecieved = 0;
            int RSOrdered = 0;
            int RSNew = 0;
            IQueryable<Request> requestsSearched = _context.Requests.AsQueryable().Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType);

            //convert the bools into thier corresponding IDs
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



            if (requestsSearchViewModel.Request.Product.ProductName != null)
            {
                requestsSearched = requestsSearched.Where(r => r.Product.ProductName.Contains(requestsSearchViewModel.Request.Product.ProductName));
            }
            if (requestsSearchViewModel.Request.Product.ProductSubcategory.ParentCategoryID != 0)
            {
                requestsSearched = requestsSearched.Where(r => r.Product.ProductSubcategory.ParentCategoryID == requestsSearchViewModel.Request.Product.ProductSubcategory.ParentCategoryID);
            }
            if (requestsSearchViewModel.Request.Product.ProductSubcategoryID != 0)
            {
                requestsSearched = requestsSearched.Where(r => r.Product.ProductSubcategoryID == requestsSearchViewModel.Request.Product.ProductSubcategoryID);
            }
            //check for project
            //check for sub project
            if (requestsSearchViewModel.Request.Product.VendorID != 0)
            {
                requestsSearched = requestsSearched.Where(r => r.Product.VendorID == requestsSearchViewModel.Request.Product.VendorID);
            }
            if (requestsSearchViewModel.Request.ParentRequest.OrderNumber != null)
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
            if (requestsSearchViewModel.Request.ExpectedSupplyDays != 0)//should this be on the parent request
            {
                requestsSearched = requestsSearched.Where(r => r.ExpectedSupplyDays == requestsSearchViewModel.Request.ExpectedSupplyDays);
            }

            //not sure what the to date and the from date are on????

            bool IsRequest = true;
            bool IsInventory = false;
            bool IsAll = false;

            //also need to get the list smaller to just request or inventory

            var PageType = AppUtility.RequestPageTypeEnum.None;
            if (IsRequest)
            {
                TempData["PageType"] = AppUtility.RequestPageTypeEnum.Request;
            }
            else if (IsInventory)
            {
                TempData["PageType"] = AppUtility.RequestPageTypeEnum.Inventory;
            }
            else if (IsAll)
            {
                TempData["PageType"] = AppUtility.RequestPageTypeEnum.Request;
            }

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
            if (requestsSearchViewModel.SectionType == AppUtility.MenuItems.OrdersAndInventory)
            {
                return View("Index", onePageOfProducts);
            }
            else if (requestsSearchViewModel.SectionType == AppUtility.MenuItems.LabManagement)
            {
                return RedirectToAction("IndexForLabManage", "Vendors", onePageOfProducts);
            }
            else if (requestsSearchViewModel.SectionType == AppUtility.MenuItems.Operation)
            {
                return RedirectToAction("Index", "Operations", onePageOfProducts);
            }
            return View("Index", onePageOfProducts);

        }


        /*
         * END SEARCH
         */



        /*
         * START RECEIVED MODAL
         */

        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> ReceivedModal(int RequestID)
        {
            //foreach(var li in _context.LocationInstances)
            //{
            //    li.IsFull = false;
            //    _context.Update(li);
            //}
            //_context.SaveChanges();

            ReceivedLocationViewModel receivedLocationViewModel = new ReceivedLocationViewModel()
            {
                Request = _context.Requests.Where(r => r.RequestID == RequestID).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory)
                    .FirstOrDefault(),
                locationTypesDepthZero = _context.LocationTypes.Where(lt => lt.Depth == 0),
                locationInstancesSelected = new List<LocationInstance>(),
                ApplicationUsers = await _context.Users.Where(u => !u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null).ToListAsync()
            };
            receivedLocationViewModel.locationInstancesSelected.Add(new LocationInstance());
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            receivedLocationViewModel.Request.ApplicationUserReceiverID = currentUser.Id;
            receivedLocationViewModel.Request.ArrivalDate = DateTime.Today;
            receivedLocationViewModel.CategoryType = receivedLocationViewModel.Request.Product.ProductSubcategory.ParentCategory.CategoryTypeID;

            return View(receivedLocationViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public IActionResult ReceivedModalVisual(int LocationInstanceID)
        {
            ReceivedModalVisualViewModel receivedModalVisualViewModel = new ReceivedModalVisualViewModel()
            {
                ParentLocationInstance = _context.LocationInstances.Where(m => m.LocationInstanceID == LocationInstanceID).FirstOrDefault()
            };

            if (receivedModalVisualViewModel.ParentLocationInstance != null)
            {
                receivedModalVisualViewModel.ChildrenLocationInstances =
                    _context.LocationInstances.Where(m => m.LocationInstanceParentID == LocationInstanceID)
                    .Include(m => m.RequestLocationInstances).ToList();

                //return NotFound();
            }
            return PartialView(receivedModalVisualViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> ReceivedModal(ReceivedLocationViewModel receivedLocationViewModel, ReceivedModalSublocationsViewModel receivedModalSublocationsViewModel, ReceivedModalVisualViewModel receivedModalVisualViewModel)
        {
            var requestReceived = _context.Requests.Where(r => r.RequestID == receivedLocationViewModel.Request.RequestID)
                .Include(r => r.Product).ThenInclude(p=>p.Vendor).FirstOrDefault();

            bool hasLocationInstances = false;
            if (receivedLocationViewModel.CategoryType == 1)
            {
                foreach (LocationInstance locationInstance in receivedModalVisualViewModel.ChildrenLocationInstances)
                {
                    bool flag = false;
                    LocationInstance parentLocationInstance = locationInstance;
                    while (!flag)
                    {
                        var pli = _context.LocationInstances.Where(li => li.LocationInstanceID == parentLocationInstance.LocationInstanceParentID).FirstOrDefault();
                        if (pli != null)
                        {
                            parentLocationInstance = pli;
                        }
                        else
                        {
                            flag = true;
                        }
                    }

                    var tempLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == locationInstance.LocationInstanceID).FirstOrDefault();
                    if (!tempLocationInstance.IsFull && locationInstance.IsFull)//only putting in the locationInstance.IsFull if it's false b/c sometimes it doesn't pass in the true value so we can end up taking things out by mistake
                    {
                        tempLocationInstance.IsFull = locationInstance.IsFull;
                        _context.Update(tempLocationInstance);
                        //coule be later on we'll want to save here too

                        //this only works because we're using a one to many relationship with request and locationinstance instead of a many to many
                        var requestLocationInstances = _context.LocationInstances
                            .Where(li => li.LocationInstanceID == locationInstance.LocationInstanceID)
                            .FirstOrDefault().RequestLocationInstances;

                        //if it doesn't have any requestlocationinstances
                        //WHY DO WE NEED THIS??????
                        if (requestLocationInstances == null)
                        {
                            RequestLocationInstance requestLocationInstance = new RequestLocationInstance()
                            {
                                RequestID = receivedLocationViewModel.Request.RequestID,
                                LocationInstanceID = locationInstance.LocationInstanceID,
                                ParentLocationInstanceID = parentLocationInstance.LocationInstanceID
                            };
                            _context.Add(requestLocationInstance);
                            hasLocationInstances = true;
                        }
                        _context.SaveChanges();
                    }
                }
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
                //receivedLocationViewModel.Request.Product = _context.Products.Where(p => p.ProductID == receivedLocationViewModel.Request.ProductID).FirstOrDefault();
                //receivedLocationViewModel.Request.ParentRequest = _context.ParentRequests.Where(pr => pr.ParentRequestID == receivedLocationViewModel.Request.ParentRequestID).FirstOrDefault();
                //receivedLocationViewModel.Request.ApplicationUserReceiver = _context.Users.Where(u => u.Id == receivedLocationViewModel.Request.ApplicationUserReceiverID).FirstOrDefault();
                //receivedLocationViewModel.Request.RequestStatus = _context.RequestStatuses.Where(rs => rs.RequestStatusID == receivedLocationViewModel.Request.RequestStatusID).FirstOrDefault();
                //receivedLocationViewModel.Request.SubProject = _context.SubProjects.Where(sp => sp.SubProjectID == receivedLocationViewModel.Request.SubProjectID).FirstOrDefault();
                requestReceived.ArrivalDate = receivedLocationViewModel.Request.ArrivalDate;
                requestReceived.ApplicationUserReceiverID = receivedLocationViewModel.Request.ApplicationUserReceiverID;
                requestReceived.ApplicationUserReceiver = _context.Users.Where(u => u.Id == receivedLocationViewModel.Request.ApplicationUserReceiverID).FirstOrDefault();
                _context.Update(requestReceived);
                await _context.SaveChangesAsync();

                RequestNotification requestNotification = new RequestNotification();
                requestNotification.RequestID = requestReceived.RequestID;
                requestNotification.IsRead = false;
                requestNotification.ApplicationUserID = receivedLocationViewModel.Request.ApplicationUserCreatorID;
                requestNotification.RequestName = receivedLocationViewModel.Request.Product.ProductName;
                requestNotification.NotificationStatusID = 4;
                requestNotification.Description = "received by "+ receivedLocationViewModel.Request.ApplicationUserReceiver.FirstName;
                requestNotification.TimeStamp = DateTime.Now;
                requestNotification.Controller = "Requests";
                requestNotification.Action = "NotificatonsView";
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
            if (receivedLocationViewModel.CategoryType == 1)
            {
                return RedirectToAction("Index", new
                {
                    page = receivedLocationViewModel.Page,
                    requestStatusID = receivedLocationViewModel.RequestStatusID,
                    subcategoryID = receivedLocationViewModel.SubCategoryID,
                    vendorID = receivedLocationViewModel.VendorID,
                    applicationUserID = receivedLocationViewModel.ApplicationUserID
                });
            }
            else
            {
                return RedirectToAction("Index", "Operations", new
                {
                    page = receivedLocationViewModel.Page,
                    requestStatusID = receivedLocationViewModel.RequestStatusID,
                    subcategoryID = receivedLocationViewModel.SubCategoryID,
                    vendorID = receivedLocationViewModel.VendorID,
                    applicationUserID = receivedLocationViewModel.ApplicationUserID
                });
            }

        }


        /*
         * END RECEIVED MODAL
         */
        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public ActionResult DocumentsModal(int id, AppUtility.RequestFolderNamesEnum RequestFolderNameEnum, bool IsEdittable)
        {
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                Request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.Product).FirstOrDefault(),
                RequestFolderName = RequestFolderNameEnum,
                IsEdittable = IsEdittable
                //Files = new List<FileInfo>()
            };

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
                    string newFileString = AppUtility.GetLastFourFiles(docfile.FullName);
                    documentsModalViewModel.FileStrings.Add(newFileString);
                    //documentsModalViewModel.Files.Add(docfile);
                }
            }

            return PartialView(documentsModalViewModel);
        }


        [HttpPost]
        public void DocumentsModal(/*[FromBody]*/ DocumentsModalViewModel documentsModalViewModel)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            string requestFolder = Path.Combine(uploadFolder, documentsModalViewModel.Request.RequestID.ToString());
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
        public ActionResult DeleteDocumentModal(String FileString, int id, AppUtility.RequestFolderNamesEnum RequestFolderNameEnum)
        {
            DeleteDocumentsViewModel deleteDocumentsViewModel = new DeleteDocumentsViewModel()
            {
                FileName = FileString,
                RequestID = id,
                FolderName = RequestFolderNameEnum
            };
            return View(deleteDocumentsViewModel);
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
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
            var subprojectList = _context.SubProjects.Where(sp => sp.ProjectID == ProjectID).ToList();
            return Json(subprojectList);
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public IActionResult ApproveReorder(int id)
        {
            var request = _context.Requests.OfType<Reorder>().Where(r => r.RequestID == id).Include(x => x.ParentQuote).Include(r=>r.Product).ThenInclude(p=>p.Vendor).FirstOrDefault();
            try
            {
                request.RequestStatusID = 6; //approved
                request.ParentQuote.QuoteStatusID = 1; //awaiting quote request
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
            AppUtility.RequestPageTypeEnum requestPageTypeEnum = AppUtility.RequestPageTypeEnum.Request;

            return RedirectToAction("Index", new
            {
                requestStatusID = 6,
                PageType = requestPageTypeEnum
            });
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, OrdersAndInventory, Operation")]
        public IActionResult Approve(int id)
        {
            var request = _context.Requests.Where(r => r.RequestID == id).Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(px => px.ParentCategory).Include(r => r.Product.Vendor).FirstOrDefault();
            try
            {
                request.RequestStatusID = 6; //approved
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
            AppUtility.RequestPageTypeEnum requestPageTypeEnum = AppUtility.RequestPageTypeEnum.Request;
            if (request.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
            {
                return RedirectToAction("Index", new
                {
                    requestStatusID = 6,
                    PageType = requestPageTypeEnum
                });
            }
            else
            {
                return RedirectToAction("Index", "Operations", new
                {
                    requestStatusID = 6,
                    PageType = requestPageTypeEnum
                });
            }


        }

        [HttpGet]
        [Authorize(Roles = "Admin, LabManagement")]
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
        [Authorize(Roles = "Admin, LabManagement")]
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
                    request.ParentQuote.QuoteNumber = quoteNumber;
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> NotificationsView(int id=0)
        {
            if (id != 0)
            {
                var notification = _context.RequestNotifications.Where(rn => rn.NotificationID == id).FirstOrDefault();
                notification.IsRead = true;
                _context.Update(notification);
                await _context.SaveChangesAsync();
            }
            TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.Notifications;
            TempData["PageType"] = AppUtility.RequestPageTypeEnum.Cart;
            ApplicationUser currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            var requests = _context.RequestNotifications.Include(n => n.NotificationStatus).Where(n => n.ApplicationUserID == currentUser.Id).OrderByDescending(n=>n.TimeStamp).ToList();
            return View(requests);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> Cart()
        {
            TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.Cart;
            TempData["PageType"] = AppUtility.RequestPageTypeEnum.Cart;
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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> OrderLateModal(int id)
        {
            var request = _context.Requests
                .Where(r => r.RequestID == id)
                .Include(r=>r.ApplicationUserCreator)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product).ThenInclude(p => p.Vendor).FirstOrDefault();
            return PartialView(request);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> OrderLateModal(Request request)
        {
            request = _context.Requests.Where(r => r.RequestID == request.RequestID).Include(r => r.ParentRequest).ThenInclude(pr => pr.ApplicationUser).Include(r => r.Product).ThenInclude(p => p.Vendor).FirstOrDefault();
            //instatiate mimemessage
            var message = new MimeMessage();

            //instantiate the body builder
            var builder = new BodyBuilder();


            string ownerEmail = request.ApplicationUserCreator.Email;
            string ownerUsername = request.ParentRequest.ApplicationUser.FirstName + " " + request.ParentRequest.ApplicationUser.LastName;
            string ownerPassword = request.ParentRequest.ApplicationUser.SecureAppPass;
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
                    $"{request.ParentRequest.ApplicationUser.FirstName} { request.ParentRequest.ApplicationUser.FirstName}\n" +
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
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> AccountingPayments(AppUtility.AccountingPaymentsEnum accountingPaymentsEnum)
        {
            TempData["Action"] = accountingPaymentsEnum;
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Payments;
            var requestsList = _context.Requests
                .Include(r => r.ParentRequest)
                .Include(r => r.Product).ThenInclude(p => p.Vendor)
                .Include(r => r.UnitType).Include(r => r.SubUnitType).Include(r => r.SubSubUnitType)
                .Include(r => r.Product.ProductSubcategory).ThenInclude(pc => pc.ParentCategory)
                .Where(r => r.ParentRequest.WithoutOrder == false)
                .Where(r => r.IsDeleted == false);
            switch (accountingPaymentsEnum)
            {
                case AppUtility.AccountingPaymentsEnum.MonthlyPayment:
                    requestsList = requestsList
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .Where(r => r.PaymentStatusID == 1);
                    break;
                case AppUtility.AccountingPaymentsEnum.PayNow:
                    requestsList = requestsList
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .Where(r => r.PaymentStatusID == 3);
                    break;
                case AppUtility.AccountingPaymentsEnum.PayLater:
                    requestsList = requestsList
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .Where(r => r.PaymentStatusID == 4);
                    break;
                case AppUtility.AccountingPaymentsEnum.Installments:
                    requestsList = requestsList
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
                .Where(r => r.PaymentStatusID == 5);
                    break;
                case AppUtility.AccountingPaymentsEnum.StandingOrders:
                    requestsList = requestsList
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2);
                    break;
            }
            AccountingPaymentsViewModel accountingPaymentsViewModel = new AccountingPaymentsViewModel()
            {
                AccountingPaymentsEnum = accountingPaymentsEnum,
                Requests = requestsList.ToLookup(r => r.Product.Vendor)
            };
            return View(accountingPaymentsViewModel);
        }

        //[HttpGet]
        //[Authorize(Roles = "Admin, Accounting")]
        //public async Task<IActionResult> PaymentsPayModal(int? vendorid, int? paymentstatusid/*, List<int>? requestIds*/)
        //{
        //    List<Request> requestsToPay = new List<Request>();

        //    if (vendorid != null && paymentstatusid != null)
        //    {
        //        requestsToPay = _context.Requests
        //        .Include(r => r.Product).ThenInclude(p => p.Vendor)
        //        .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1)
        //        .Where(r => r.Product.VendorID == vendorid)
        //        .Where(r => r.PaymentStatusID == paymentstatusid).ToList();
        //    }

        //    PaymentsPayModalViewModel paymentsPayModalViewModel = new PaymentsPayModalViewModel()
        //    {
        //        Requests = requestsToPay
        //    };

        //    //check if payment status type is installments to show the installments in the view model

        //    return PartialView(paymentsPayModalViewModel);
        //}


        /*
         * 
         */


    }


}
