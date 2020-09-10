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
        [Authorize(Roles = "Admin, Operation")]
        // GET: Requests
        //IMPORTANT!!! When adding more parameters into the Index Get make sure to add them to the ViewData and follow them through to the Index page
        //ALSO when changing defaults -> change the defaults on the index page for paged list 


        public async Task<IActionResult> Index(int? page, int RequestStatusID = 1, int subcategoryID = 0, int vendorID = 0, string applicationUserID = null, int parentLocationInstanceID = 0, AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, RequestsSearchViewModel? requestsSearchViewModel = null)
        {

            //instantiate your list of requests to pass into the index
            IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ApplicationUserCreator)
                .Include(r => r.RequestLocationInstances).ThenInclude(rli => rli.LocationInstance).Include(r=>r.ParentQuote)
                .Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2)
                .OrderBy(r => r.CreationDate);
            //.Include(r=>r.UnitType).ThenInclude(ut => ut.UnitTypeDescription).Include(r=>r.SubUnitType).ThenInclude(sut => sut.UnitTypeDescription).Include(r=>r.SubSubUnitType).ThenInclude(ssut =>ssut.UnitTypeDescription); //inorder to display types of units

            TempData["RequestStatusID"] = RequestStatusID;
            var SidebarTitle = AppUtility.OrdersAndInventorySidebarEnum.None;

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
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
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
                SidebarTitle = AppUtility.OrdersAndInventorySidebarEnum.Vendor;
                TempData["VendorID"] = vendorID;
            }
            else if (subcategoryID > 0 && requestsSearchViewModel != null)
            {
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.ProductSubcategoryID == subcategoryID);
                //pass the subcategoryID into the temp data to use if you'd like to sort from there
                SidebarTitle = AppUtility.OrdersAndInventorySidebarEnum.Type;
                TempData["SubcategoryID"] = subcategoryID;
            }
            else if (applicationUserID != null && requestsSearchViewModel != null)
            {
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.ApplicationUserCreatorID == applicationUserID);
                //pass the subcategoryID into the temp data to use if you'd like to sort from there
                SidebarTitle = AppUtility.OrdersAndInventorySidebarEnum.Owner;
                TempData["ApplicationUserID"] = applicationUserID;
            }
            else
            {
                SidebarTitle = AppUtility.OrdersAndInventorySidebarEnum.LastItem;
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

                onePageOfProducts.Where(opop => opop.RequestStatusID == 5).OrderByDescending(opop => opop.ArrivalDate); // display by arrivaldate if recieved
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

        [Authorize(Roles = "Admin, Operation")]
        public async Task<IActionResult> CreateModalView()
        {
            var parentcategories = await _context.ParentCategories.Where(pr => pr.CategoryTypeID == 2).ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.Where(pr => pr.ParentCategory.CategoryTypeID == 2).ToListAsync();
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

            requestItemViewModel.Request = new Request();
            requestItemViewModel.Request.Product = new Product();
            requestItemViewModel.Request.Product = new Product();
            requestItemViewModel.Request.ParentQuote = new ParentQuote();

            requestItemViewModel.Request.ParentQuote.QuoteDate = DateTime.Now;
            requestItemViewModel.Request.CreationDate = DateTime.Now;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.RequestPageTypeEnum.Request;
            TempData["SidebarTitle"] = AppUtility.OrdersAndInventorySidebarEnum.AddItem;
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
        [Authorize(Roles = "Admin, Operation")]
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
                if (!User.IsInRole("Admin") && (OrderType.Equals("Ask For Permission") || !checkIfInBudget(requestItemViewModel.Request)))
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
                        }
                        else if (OrderType.Equals("Order"))
                        {
                            requestItemViewModel.Request.RequestStatusID = 1; //new request
                            requestItemViewModel.Request.ParentQuote.QuoteStatusID = 4;
                            requestItemViewModel.RequestStatusID = 1;
                            _context.Update(requestItemViewModel.Request);
                            _context.SaveChanges();
                            TempData["OpenConfirmEmailModal"] = true;
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

                    return RedirectToAction("Index", new
                    {
                        page = requestItemViewModel.Page,
                        requestStatusID = requestItemViewModel.Request.RequestStatusID,
                        PageType = AppUtility.RequestPageTypeEnum.Request
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
        [Authorize(Roles = "Admin, Operation")]
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
        [Authorize(Roles = "Admin, Operation")]
        public async Task<IActionResult> EditModalView(int? id, bool NewRequestFromProduct = false)
        {
            string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }

            var parentcategories = await _context.ParentCategories.Where(pc=>pc.CategoryTypeID==2).ToListAsync();
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

            ModalViewType = "Edit";

            requestItemViewModel.Request = _context.Requests.Include(r => r.Product)
                .Include(r=>r.ParentQuote)
                .Include(r => r.ParentRequest)
                .Include(r => r.Product.ProductSubcategory)
                .Include(r => r.Product.ProductSubcategory.ParentCategory)
                .Include(r => r.RequestStatus)
                .Include(r => r.ApplicationUserCreator)
                //.Include(r => r.Payments) //do we have to have a separate list of payments to include thefix c inside things (like company account and payment types?)
                .SingleOrDefault(r => r.RequestID == id);

            //load the correct list of subprojects
            
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

        //[Authorize(Roles = "Admin, Operation")]
        //public async Task<IActionResult> EditSummaryModalView(int? id, bool NewRequestFromProduct = false)
        //{
           
        //    //not imlemented yet
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Operation")]
        public async Task<IActionResult> EditModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
            requestItemViewModel.Request.ParentRequest = null;         
           // requestItemViewModel.Request.ParentQuote.ParentQuoteID = (Int32)requestItemViewModel.Request.ParentQuoteID;
          //  var parentQuote = _context.ParentQuotes.Where(pq => pq.ParentQuoteID == requestItemViewModel.Request.ParentQuoteID).FirstOrDefault();
           // parentQuote.QuoteNumber = requestItemViewModel.Request.ParentQuote.QuoteNumber;
           // parentQuote.QuoteDate = requestItemViewModel.Request.ParentQuote.QuoteDate;
           // requestItemViewModel.Request.ParentQuote = parentQuote;
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

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
           
                requestItemViewModel.Request.Product.ProductID = requestItemViewModel.Request.ProductID;
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

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,  Operation")]
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
                if (request.Cost > request.ApplicationUserCreator.OperaitonOrderLimit)
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
