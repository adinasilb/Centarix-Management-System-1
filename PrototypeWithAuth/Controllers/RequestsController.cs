using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Linq.Expressions;

namespace PrototypeWithAuth.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //take this out?
        private readonly IHostingEnvironment _hostingEnvironment;
        //take this out?
        private readonly List<Request> _cartRequests = new List<Request>();

        private IQueryable<Request> _searchList = Enumerable.Empty<Request>().AsQueryable();

        public RequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            //use the hosting environment for the file uploads
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        // GET: Requests
        //IMPORTANT!!! When adding more parameters into the Index Get make sure to add them to the ViewData and follow them through to the Index page
        public async Task<IActionResult> Index(int? page, int RequestStatusID = 1, int subcategoryID = 0, int vendorID = 0, string applicationUserID = null, AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, RequestsSearchViewModel? requestsSearchViewModel = null)
        {
            //instantiate your list of requests to pass into the index
            IQueryable<Request> fullRequestsList = _context.Requests.Include(r => r.ParentRequest);

            TempData["RequestStatusID"] = RequestStatusID;
            var SidebarTitle = AppUtility.RequestSidebarEnum.None;

            int newCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 1, vendorID, subcategoryID, applicationUserID);
            int orderedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 2, vendorID, subcategoryID, applicationUserID);
            int receivedCount = AppUtility.GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(fullRequestsList, 3, vendorID, subcategoryID, applicationUserID);
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
                if (RequestStatusID == 0 || RequestStatusID == 2)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 2);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                }
                if (RequestStatusID == 0 || RequestStatusID == 3)
                {
                    TempRequestList = AppUtility.GetRequestsListFromRequestStatusID(fullRequestsList, 3, 50);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
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
                    .Where(r => r.ParentRequest.ApplicationUserID == applicationUserID);
                //pass the subcategoryID into the temp data to use if you'd like to sort from there
                SidebarTitle = AppUtility.RequestSidebarEnum.Owner;
                TempData["ApplicationUserID"] = applicationUserID;
            }

            //passing in the amounts to display in the top buttons
            TempData["AmountNew"] = newCount;
            TempData["AmountOrdered"] = orderedCount;
            TempData["AmountReceived"] = receivedCount;

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

            //Getting the page that is going to be seen (if no page was specified it will be one)
            var pageNumber = page ?? 1;
            var onePageOfProducts = Enumerable.Empty<Request>().ToPagedList();
            try
            {
                onePageOfProducts = await RequestsPassedIn.Include(r => r.ParentRequest).Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).Include(r => r.RequestStatus).ToPagedListAsync(pageNumber, 25);
            }
            catch (Exception ex)
            {
                //do something here 
            }
            return View(onePageOfProducts);
        }




        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.ParentRequest.ApplicationUser)
                .Include(r => r.Product)
                .Include(r => r.RequestStatus)
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        //public IActionResult Create()
        //{
        //    ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id");
        //    ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName");
        //    ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID");
        //    return View();
        //}

        // GET: Requests/Create
        public async Task<IActionResult> CreateAsync() //need to correct to be for request
        {
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName");
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID");
            var parentCategories = _context.ParentCategories.ToList();
            var productSubcategories = _context.ProductSubcategories.ToList();
            var vendors = _context.Vendors.ToList();
            var requeststatuses = _context.RequestStatuses.ToList();

            RequestItemViewModel viewModel = new RequestItemViewModel
            {
                //pass the lists into the viewmodel to use on the front end
                ParentCategories = parentCategories,
                ProductSubcategories = productSubcategories,
                Vendors = vendors,
                RequestStatuses = requeststatuses,
                //instantiate a new request
                Request = new Request()
            };

            viewModel.Request.Product = new Product(); // have to instantaiate the product from the requests, because the viewModel relies on request.product to create the new product
            viewModel.Request.ParentRequest = new ParentRequest();

            //gets the user
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            //passes the user into the view model in the correct place
            viewModel.Request.ParentRequest.ApplicationUserID = user.Id;

            //adding a tempdata so the testing in the requestnavview layout page will work (better to create a base controller for temp data and store it there)
            //https://stackoverflow.com/questions/37267586/how-to-check-condition-if-temp-data-has-value-in-every-controller
            //if putting it here should get from somewhere and not just make it up...
            TempData["PageType"] = AppUtility.RequestPageTypeEnum.Request;
            return View(viewModel);
        }

        // POST: Requests/Create/ 
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Adina's code for creating and binding product model with request model in a single view, check that all errors are handled for.

        [HttpPost]
        [ValidateAntiForgeryToken]

        /*
         * ParentRequest looks like it's working
         * Create the cart and then check
         */

        public async Task<IActionResult> Create(RequestItemViewModel requestItemViewModel)
        {
            // view data is placed in the beginning in order to redirect when errors are caught, so need to have the info saved before handling the error
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", requestItemViewModel.Request.ParentRequest.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", requestItemViewModel.Request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", requestItemViewModel.Request.RequestStatusID);

            //inserting the vendor from the vendor id, the subcategory from the subcategory id and the application user from the application user id to test for the viewmodel validation
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);
            requestItemViewModel.Request.ParentRequest.ApplicationUser = _context.Users.FirstOrDefault(u => u.Id == requestItemViewModel.Request.ParentRequest.ApplicationUserID);

            //using the dataannotations validator to test the updated object because modelstate.isvalid only looks at the stack trace that was passed in 
            var context = new ValidationContext(requestItemViewModel.Request, null, null);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(requestItemViewModel.Request, context, results, true))
            {
                _context.Add(requestItemViewModel.Request.ParentRequest);
                _context.Add(requestItemViewModel.Request);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(requestItemViewModel);
        }


        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", request.ParentRequest.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", request.RequestStatusID);
            return View(request);
        }

        //POST: Requests/Edit/5
        //This is not being used right now --> delete 
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(RequestItemViewModel addNewItemViewModel)
        //{
        //    //same logic as create controller
        //    addNewItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
        //    addNewItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
        //    addNewItemViewModel.Vendors = await _context.Vendors.ToListAsync();
        //    addNewItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();

        //    addNewItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == addNewItemViewModel.Request.Product.VendorID);
        //    addNewItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == addNewItemViewModel.Request.Product.ProductSubcategoryID);
        //    addNewItemViewModel.Request.ParentRequest.ApplicationUser = _context.Users.FirstOrDefault(u => u.Id == addNewItemViewModel.Request.ParentRequest.ApplicationUserID);

        //    var context = new ValidationContext(addNewItemViewModel.Request, null, null);
        //    var results = new List<ValidationResult>();
        //    if (Validator.TryValidateObject(addNewItemViewModel.Request, context, results, true))
        //    {
        //        try
        //        {
        //            _context.Update(addNewItemViewModel.Request.Product);
        //            _context.Update(addNewItemViewModel.Request);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            return await ModalView(addNewItemViewModel.Request.RequestID);
        //        }
        //    }
        //    else
        //    {
        //        return await ModalView(addNewItemViewModel.Request.RequestID);
        //    }
        //    return RedirectToAction("Index");
        //}


        // GET: Requests/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var request = await _context.Requests
        //        .Include(r => r.ParentRequest.ApplicationUser)
        //        .Include(r => r.Product)
        //        .Include(r => r.RequestStatus)
        //        .FirstOrDefaultAsync(m => m.RequestID == id);
        //    if (request == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(request);
        //}

        //// POST: Requests/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var request = await _context.Requests.FindAsync(id);
        //    _context.Requests.Remove(request);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool RequestExists(int id)
        //{
        //    return _context.Requests.Any(e => e.RequestID == id);
        //}

        /*
         * START MODAL VIEW COPY
         */
        public async Task<IActionResult> CreateModalView()
        {
            var unitTypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            RequestItemViewModel requestItemViewModel = new RequestItemViewModel
            {
                ParentCategories = await _context.ParentCategories.ToListAsync(),
                ProductSubcategories = await _context.ProductSubcategories.ToListAsync(),
                Vendors = await _context.Vendors.ToListAsync(),
                RequestStatuses = await _context.RequestStatuses.ToListAsync(),
                PaymentTypes = await _context.PaymentTypes.ToListAsync(),
                CompanyAccounts = await _context.CompanyAccounts.ToListAsync(),
                UnitTypeList = new SelectList(unitTypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription")
            };

            requestItemViewModel.Request = new Request();
            requestItemViewModel.Request.ParentRequest = new ParentRequest();
            requestItemViewModel.Request.RequestStatus = new RequestStatus();
            requestItemViewModel.Request.ParentRequest.ApplicationUser = new ApplicationUser();

            //if you are creating a new one set the dates to today to prevent problems in the front end
            //in the future use jquery datepicker (For smooth ui on the front end across all browsers)
            //(already imported it)
            requestItemViewModel.Request.ParentRequest.OrderDate = DateTime.Now;
            requestItemViewModel.Request.ParentRequest.InvoiceDate = DateTime.Now;

            return View(requestItemViewModel);
        }
        public async Task<IActionResult> DetailsModalView(int? id, bool NewRequestFromProduct = false)
        {
            //string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }

            var parentcategories = await _context.ParentCategories.ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();
            var requeststatuses = await _context.RequestStatuses.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                RequestStatuses = requeststatuses,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts
            };

            if (id == 0)
            {
                return RedirectToAction("CreateModalView");
            }
            else if (NewRequestFromProduct)
            {


                requestItemViewModel.Request = new Request();
                requestItemViewModel.Request.ParentRequest = new ParentRequest();
                requestItemViewModel.Request.RequestStatus = new RequestStatus();
                requestItemViewModel.Request.ParentRequest.ApplicationUser = new ApplicationUser();

                var request = _context.Requests
                    .Include(r => r.Product)
                    .SingleOrDefault(x => x.RequestID == id);
                requestItemViewModel.Request.ProductID = request.ProductID;
                requestItemViewModel.Request.Product = request.Product;

                var paymentsList = _context.Payments
                    .Include(p => p.CompanyAccount) //check if it works without this
                    .Include(p => p.CompanyAccount.PaymentType)
                    .Where(p => p.ParentRequestID == request.ParentRequest.ParentRequestID);
                requestItemViewModel.OldPayments = paymentsList;

            }
            else
            {

                requestItemViewModel.Request = _context.Requests.Include(r => r.Product)
                    .Include(r => r.ParentRequest)
                    .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .Include(r => r.RequestStatus)
                    .Include(r => r.ParentRequest.ApplicationUser)
                    .Include(r => r.ParentRequest.Payments) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .SingleOrDefault(x => x.RequestID == id);

                //check if this works once there are commments
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
                string uploadFolder3 = Path.Combine(uploadFolder2, "Orders");
                //the partial file name that we will search for (1- because we want the first one)
                //creating the directory from the path made earlier

                if (Directory.Exists(uploadFolder3))
                {
                    DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolder3);
                    //searching for the partial file name in the directory
                    FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                    //checking if there were any files found before looping through them (to prevent an error)
                    requestItemViewModel.OrderFileStrings = new List<string>();
                    if (orderfilesfound[0].Exists)
                    {
                        //getting the file from the FileInfo[]
                        foreach (FileInfo file in orderfilesfound)
                        {
                            requestItemViewModel.OrderFileStrings.Add(file.FullName.ToString());
                        }
                    }
                }

                if (requestItemViewModel.Request == null)
                {
                    return NotFound();
                }
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //initializing the boolean here
            //b/c need to check if the requestID is 0 but then pass in the new request ID
            bool WithOrder = false;

            //why do we need this here?
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

            //in case we need to return to the modal view
            requestItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            requestItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();
            //formatting the select list of the unit types
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");

            //declared outside the if b/c it's used farther down too (for parent request the new comment too)
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            ////checks if it's a new request -- CREATE MODAL So should always go into here
            //if (requestItemViewModel.Request.ParentRequestID == 0)
            //{
            //use application user of whoever signed in
            /*Right now since it's a new parent request for each request then it gives a new Application UserID but in the future
            *when we implement parent requests then there will be more logic
            */
            requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
            //}

            //can we combine this with the one above?
            //if it's a new request need to put in a request status --CREATE MODAL so should always go here
            if (requestItemViewModel.Request.RequestStatusID == null)
            {
                //all new ones will be "new" until actually ordered after the confirm email
                requestItemViewModel.Request.RequestStatusID = 1;
                //if it's less than 5500 shekel OR the user is an admin it will be ordered
                if ((requestItemViewModel.Request.Cost < 5500 || User.IsInRole("Admin")) && OrderType.Equals("Order"))
                {
                    if (OrderType.Equals("Order"))
                    {
                        WithOrder = true;
                    }
                }
            }
            //in case we need to redirect to action
            //TempData["ModalView"] = true;
            //why is this here?
            TempData["RequestID"] = requestItemViewModel.Request.RequestID;

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
                try
                {
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
                        foreach (IFormFile orderFile in requestItemViewModel.OrderFiles)
                            foreach (IFormFile orderfile in requestItemViewModel.OrderFiles)
                            {
                                //create file
                                string folderPath = Path.Combine(requestFolder, "Orders");
                                Directory.CreateDirectory(folderPath);
                                string uniqueFileName = x + orderfile.FileName;
                                string filePath = Path.Combine(folderPath, uniqueFileName);
                                orderfile.CopyTo(new FileStream(filePath, FileMode.Create));
                                x++;
                            }
                    }
                    //test that this works
                    if (WithOrder)
                    {
                        return RedirectToAction("ConfirmEmailModal", new { id = requestItemViewModel.Request.RequestID });
                    }
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.InnerException.ToString();
                    return View(requestItemViewModel);
                }
            }
            else
            {
                return View(requestItemViewModel);
            }

            //insert code here
            return RedirectToAction("Index");
            //return RedirectToAction("Index", new
            //{
            //    page = requestItemViewModel.Page,
            //    requestStatusID = requestItemViewModel.RequestStatusID,
            //    subcategoryID = requestItemViewModel.SubCategoryID,
            //    vendorID = requestItemViewModel.VendorID,
            //    applicationUserID = requestItemViewModel.ApplicationUserID,
            //    PageType = requestItemViewModel.PageType
            //});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //initializing the boolean here
            //b/c need to check if the requestID is 0 but then pass in the new request ID
            bool WithOrder = false;

            //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
            requestItemViewModel.Request.ParentRequest.ParentRequestID = requestItemViewModel.Request.ParentRequestID;
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

            //in case we need to return to the modal view
            requestItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            requestItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");

            //declared outside the if b/c it's used farther down to (for parent request the new comment too)
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            //checks if it's a new request
            if (requestItemViewModel.Request.ParentRequestID == 0)
            {
                //use application user of whoever signed in
                requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
            }

            //can we combine this with the one above?
            //if it's a new request need to put in a request status
            if (requestItemViewModel.Request.RequestStatusID == null)
            {
                //all new ones will be "new" until actually ordered after the confirm email
                requestItemViewModel.Request.RequestStatusID = 1;
                //if it's less than 5500 shekel OR the user is an admin it will be ordered
                if ((requestItemViewModel.Request.Cost < 5500 || User.IsInRole("Admin")) && OrderType.Equals("Order"))
                {
                    WithOrder = true;
                }
            }
            //in case we need to redirect to action
            //TempData["ModalView"] = true;
            TempData["RequestID"] = requestItemViewModel.Request.RequestID;

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
                try
                {
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
                        foreach (IFormFile orderFile in requestItemViewModel.OrderFiles)
                            foreach (IFormFile orderfile in requestItemViewModel.OrderFiles)
                            {
                                //create file
                                string folderPath = Path.Combine(requestFolder, "Orders");
                                Directory.CreateDirectory(folderPath);
                                string uniqueFileName = x + orderfile.FileName;
                                string filePath = Path.Combine(folderPath, uniqueFileName);
                                orderfile.CopyTo(new FileStream(filePath, FileMode.Create));
                                x++;
                            }
                    }
                    //test that this works
                    if (WithOrder)
                    {
                        return RedirectToAction("ConfirmEmailModal", new { id = requestItemViewModel.Request.RequestID });
                    }
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.InnerException.ToString();
                    return View(requestItemViewModel);
                }
            }
            else
            {
                return View(requestItemViewModel);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(RequestItemViewModel requestItemViewModel)
        {
            return RedirectToAction("Cart");
        }

        public ActionResult DownloadPDF(string filename)
        {
            return File(filename, "application/pdf");
        }


        public async Task<IActionResult> EditModalView(int? id, bool NewRequestFromProduct = false)
        {
            string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }

            var parentcategories = await _context.ParentCategories.ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();
            var requeststatuses = await _context.RequestStatuses.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                RequestStatuses = requeststatuses,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts
            };

            if (id == 0)
            {
                return RedirectToAction("CreateModalView");
            }
            else if (NewRequestFromProduct)
            {
                ModalViewType = "Create"; //?

                requestItemViewModel.Request = new Request();
                requestItemViewModel.Request.ParentRequest = new ParentRequest();
                requestItemViewModel.Request.RequestStatus = new RequestStatus();
                requestItemViewModel.Request.ParentRequest.ApplicationUser = new ApplicationUser();

                var request = _context.Requests
                    .Include(r => r.Product)
                    .SingleOrDefault(x => x.RequestID == id);
                requestItemViewModel.Request.ProductID = request.ProductID;
                requestItemViewModel.Request.Product = request.Product;

                var paymentsList = _context.Payments
                    .Include(p => p.CompanyAccount) //check if it works without this
                    .Include(p => p.CompanyAccount.PaymentType)
                    .Where(p => p.ParentRequestID == request.ParentRequest.ParentRequestID);
                requestItemViewModel.OldPayments = paymentsList;

            }
            else
            {
                ModalViewType = "Edit";

                requestItemViewModel.Request = _context.Requests.Include(r => r.Product)
                    .Include(r => r.ParentRequest)
                    .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .Include(r => r.RequestStatus)
                    .Include(r => r.ParentRequest.ApplicationUser)
                    .Include(r => r.ParentRequest.Payments) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .SingleOrDefault(x => x.RequestID == id);

                //check if this works once there are commments
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
                string uploadFolder3 = Path.Combine(uploadFolder2, "Orders");
                //the partial file name that we will search for (1- because we want the first one)
                //creating the directory from the path made earlier

                if (Directory.Exists(uploadFolder3))
                {
                    DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolder3);
                    //searching for the partial file name in the directory
                    FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                    //checking if there were any files found before looping through them (to prevent an error)
                    requestItemViewModel.OrderFileStrings = new List<string>();
                    if (orderfilesfound[0].Exists)
                    {
                        //getting the file from the FileInfo[]
                        foreach (FileInfo file in orderfilesfound)
                        {
                            requestItemViewModel.OrderFileStrings.Add(file.FullName.ToString());
                        }
                    }
                }

                if (requestItemViewModel.Request == null)
                {
                    return NotFound();
                }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //initializing the boolean here
            //b/c need to check if the requestID is 0 but then pass in the new request ID
            bool WithOrder = false;

            //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
            requestItemViewModel.Request.ParentRequest.ParentRequestID = requestItemViewModel.Request.ParentRequestID;
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

            //in case we need to return to the modal view
            requestItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            requestItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");

            //declared outside the if b/c it's used farther down to (for parent request the new comment too)
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            //checks if it's a new request
            if (requestItemViewModel.Request.ParentRequestID == 0)
            {
                //use application user of whoever signed in
                requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
            }

            //can we combine this with the one above?
            //if it's a new request need to put in a request status
            if (requestItemViewModel.Request.RequestStatusID == null)
            {
                //all new ones will be "new" until actually ordered after the confirm email
                requestItemViewModel.Request.RequestStatusID = 1;
                //if it's less than 5500 shekel OR the user is an admin it will be ordered
                if ((requestItemViewModel.Request.Cost < 5500 || User.IsInRole("Admin")) && OrderType.Equals("Order"))
                {
                    WithOrder = true;
                }
            }
            //in case we need to redirect to action
            //TempData["ModalView"] = true;
            TempData["RequestID"] = requestItemViewModel.Request.RequestID;

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
                try
                {
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
                        foreach (IFormFile orderFile in requestItemViewModel.OrderFiles)
                            foreach (IFormFile orderfile in requestItemViewModel.OrderFiles)
                            {
                                //create file
                                string folderPath = Path.Combine(requestFolder, "Orders");
                                Directory.CreateDirectory(folderPath);
                                string uniqueFileName = x + orderfile.FileName;
                                string filePath = Path.Combine(folderPath, uniqueFileName);
                                orderfile.CopyTo(new FileStream(filePath, FileMode.Create));
                                x++;
                            }
                    }
                    //test that this works
                    if (WithOrder)
                    {
                        return RedirectToAction("ConfirmEmailModal", new { id = requestItemViewModel.Request.RequestID });
                    }
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.InnerException.ToString();
                    return View(requestItemViewModel);
                }
            }
            else
            {
                return View(requestItemViewModel);
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

        public async Task<IActionResult> ReOrderModalView(int? id, bool NewRequestFromProduct = false)
        {
            string ModalViewType = "";
            if (id == null)
            {
                return NotFound();
            }

            var parentcategories = await _context.ParentCategories.ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();
            var requeststatuses = await _context.RequestStatuses.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            var paymenttypes = await _context.PaymentTypes.ToListAsync();
            var companyaccounts = await _context.CompanyAccounts.ToListAsync();

            RequestItemViewModel requestItemViewModel = new RequestItemViewModel()
            {
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors,
                RequestStatuses = requeststatuses,
                UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription"),
                PaymentTypes = paymenttypes,
                CompanyAccounts = companyaccounts
            };

            if (id == 0)
            {
                return RedirectToAction("CreateModalView");
            }
            else if (NewRequestFromProduct)
            {
                ModalViewType = "Create"; //?

                requestItemViewModel.Request = new Request();
                requestItemViewModel.Request.ParentRequest = new ParentRequest();
                requestItemViewModel.Request.RequestStatus = new RequestStatus();
                requestItemViewModel.Request.ParentRequest.ApplicationUser = new ApplicationUser();

                var request = _context.Requests
                    .Include(r => r.Product)
                    .SingleOrDefault(x => x.RequestID == id);
                requestItemViewModel.Request.ProductID = request.ProductID;
                requestItemViewModel.Request.Product = request.Product;

                var paymentsList = _context.Payments
                    .Include(p => p.CompanyAccount) //check if it works without this
                    .Include(p => p.CompanyAccount.PaymentType)
                    .Where(p => p.ParentRequestID == request.ParentRequest.ParentRequestID);
                requestItemViewModel.OldPayments = paymentsList;

            }
            else
            {
                ModalViewType = "Edit";

                requestItemViewModel.Request = _context.Requests.Include(r => r.Product)
                    .Include(r => r.ParentRequest)
                    .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .Include(r => r.RequestStatus)
                    .Include(r => r.ParentRequest.ApplicationUser)
                    .Include(r => r.ParentRequest.Payments) //do we have to have a separate list of payments to include the inside things (like company account and payment types?)
                    .SingleOrDefault(x => x.RequestID == id);

                //check if this works once there are commments
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
                string uploadFolder3 = Path.Combine(uploadFolder2, "Orders");
                //the partial file name that we will search for (1- because we want the first one)
                //creating the directory from the path made earlier

                if (Directory.Exists(uploadFolder3))
                {
                    DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolder3);
                    //searching for the partial file name in the directory
                    FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                    //checking if there were any files found before looping through them (to prevent an error)
                    requestItemViewModel.OrderFileStrings = new List<string>();
                    if (orderfilesfound[0].Exists)
                    {
                        //getting the file from the FileInfo[]
                        foreach (FileInfo file in orderfilesfound)
                        {
                            requestItemViewModel.OrderFileStrings.Add(file.FullName.ToString());
                        }
                    }
                }

                if (requestItemViewModel.Request == null)
                {
                    return NotFound();
                }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReOrderModalView(RequestItemViewModel requestItemViewModel, string OrderType)
        {
            //initializing the boolean here
            //b/c need to check if the requestID is 0 but then pass in the new request ID
            bool WithOrder = false;

            //fill the request.parentrequestid with the request.parentrequets.parentrequestid (otherwise it creates a new not used parent request)
            requestItemViewModel.Request.ParentRequest.ParentRequestID = requestItemViewModel.Request.ParentRequestID;
            requestItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == requestItemViewModel.Request.Product.VendorID);
            requestItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == requestItemViewModel.Request.Product.ProductSubcategoryID);

            //in case we need to return to the modal view
            requestItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            requestItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            requestItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            requestItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();
            //redo the unit types when seeded
            var unittypes = _context.UnitTypes.Include(u => u.UnitParentType).OrderBy(u => u.UnitParentType.UnitParentTypeID).ThenBy(u => u.UnitTypeDescription);
            requestItemViewModel.UnitTypeList = new SelectList(unittypes, "UnitTypeID", "UnitTypeDescription", null, "UnitParentType.UnitParentTypeDescription");

            //declared outside the if b/c it's used farther down to (for parent request the new comment too)
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));

            //checks if it's a new request
            if (requestItemViewModel.Request.ParentRequestID == 0)
            {
                //use application user of whoever signed in
                requestItemViewModel.Request.ParentRequest.ApplicationUserID = currentUser.Id;
            }

            //can we combine this with the one above?
            //if it's a new request need to put in a request status
            if (requestItemViewModel.Request.RequestStatusID == null)
            {
                //all new ones will be "new" until actually ordered after the confirm email
                requestItemViewModel.Request.RequestStatusID = 1;
                //if it's less than 5500 shekel OR the user is an admin it will be ordered
                if ((requestItemViewModel.Request.Cost < 5500 || User.IsInRole("Admin")) && OrderType.Equals("Order"))
                {
                    WithOrder = true;
                }
            }
            //in case we need to redirect to action
            //TempData["ModalView"] = true;
            TempData["RequestID"] = requestItemViewModel.Request.RequestID;

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
                try
                {
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
                        foreach (IFormFile orderFile in requestItemViewModel.OrderFiles)
                            foreach (IFormFile orderfile in requestItemViewModel.OrderFiles)
                            {
                                //create file
                                string folderPath = Path.Combine(requestFolder, "Orders");
                                Directory.CreateDirectory(folderPath);
                                string uniqueFileName = x + orderfile.FileName;
                                string filePath = Path.Combine(folderPath, uniqueFileName);
                                orderfile.CopyTo(new FileStream(filePath, FileMode.Create));
                                x++;
                            }
                    }
                    //test that this works
                    if (WithOrder)
                    {
                        return RedirectToAction("ConfirmEmailModal", new { id = requestItemViewModel.Request.RequestID });
                    }
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError();
                    ViewData["ModalViewType"] = "Create";
                    TempData["ErrorMessage"] = ex.InnerException.ToString();
                    return View(requestItemViewModel);
                }
            }
            else
            {
                return View(requestItemViewModel);
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

        /*
         * END MODAL VIEW COPY
         */

        /*
         * BEGIN SEND EMAIL
         */
        [HttpGet]
        public async Task<IActionResult> ConfirmEmailModal(int? id, bool IsBeingApproved = false)
        {
            Request request = await _context.Requests.Where(r => r.RequestID == id).Include(r => r.Product).ThenInclude(r => r.Vendor).Include(r => r.ParentRequest).ThenInclude(r => r.ApplicationUser).FirstOrDefaultAsync();
            if (IsBeingApproved)
            {
                TempData["IsBeingApproved"] = true;
            }
            else
            {
                TempData["IsBeingApproved"] = false;
            }

            string path = "wwwroot//OrderPDFs//" + request.RequestID + ".pdf";
            //if (File.Exists(path))
            //{
            //    File.Delete(path);
            //}
            FileStream fs = new FileStream(path, FileMode.Create);
            PdfWriter writer = new PdfWriter(fs, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
            PdfDocument pdfDocument = new PdfDocument(writer);
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            document.Add(new Paragraph("Vendor: " + request.Product.Vendor.VendorEnName));
            document.Add(new Paragraph("City" + request.Product.Vendor.VendorCity));
            document.Close();


            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmailModal(Request requestThatIsApproved)
        {

            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "OrderPDFs");
            string uploadFile = Path.Combine(uploadFolder, requestThatIsApproved.RequestID.ToString() + ".pdf");

            if (System.IO.File.Exists(uploadFile))
            {
                //instatiate mimemessage
                var message = new MimeMessage();

                //instantiate the body builder
                var builder = new BodyBuilder();



                var request = _context.Requests.Where(r => r.RequestID == requestThatIsApproved.RequestID).Include(r => r.ParentRequest).ThenInclude(r=>r.ApplicationUser).Include(r=>r.Product).ThenInclude(r=>r.Vendor).FirstOrDefault();
                string ownerEmail = request.ParentRequest.ApplicationUser.Email;
                string ownerUsername = request.ParentRequest.ApplicationUser.FirstName + " " + request.ParentRequest.ApplicationUser.LastName;
                string ownerPassword = request.ParentRequest.ApplicationUser.PasswordHash;
                string vendorEmail = request.Product.Vendor.OrderEmail;
                string vendorName = request.Product.Vendor.VendorEnName;

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
                    client.Authenticate(ownerEmail, "FakeUser@123"); // set up two step authentication and get app password
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
                        request.RequestStatusID = 2;
                        await _context.SaveChangesAsync();
                    }
                    
                }
                return RedirectToAction("Index");
            }

            else
            {
                return RedirectToAction("Error");
            }


        }


        /*
         * END SEND EMAIL
         */

        /*
         * BEGIN SEARCH
         */
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            TempData["PageType"] = AppUtility.RequestPageTypeEnum.Search;

            RequestsSearchViewModel requestsSearchViewModel = new RequestsSearchViewModel
            {
                ParentCategories = await _context.ParentCategories.ToListAsync(),
                Vendors = await _context.Vendors.ToListAsync(),
                Request = new Request(),
                //check if we need this here
            };

            requestsSearchViewModel.Request.ParentRequest = new ParentRequest();

            return View(requestsSearchViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Search(RequestsSearchViewModel requestsSearchViewModel, int? page)
        {
            IQueryable<Request> requestsSearched = _context.Requests.AsQueryable();
            if (requestsSearchViewModel.Request.Product.ProductName != null)
            {
                requestsSearched = requestsSearched.Where(r => r.Product.ProductName == requestsSearchViewModel.Request.Product.ProductName);
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
                requestsSearched = requestsSearched.Where(r => r.ParentRequest.OrderNumber == requestsSearchViewModel.Request.ParentRequest.OrderNumber);
            }
            if (requestsSearchViewModel.Request.ParentRequest.OrderDate != DateTime.MinValue) //should this be datetime.min?
            {
                requestsSearched = requestsSearched.Where(r => r.ParentRequest.OrderDate == requestsSearchViewModel.Request.ParentRequest.OrderDate);
            }
            if (requestsSearchViewModel.Request.ParentRequest.InvoiceNumber != null)
            {
                requestsSearched = requestsSearched.Where(r => r.ParentRequest.InvoiceNumber == requestsSearchViewModel.Request.ParentRequest.InvoiceNumber);
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
                onePageOfProducts = await requestsSearched.Include(r => r.ParentRequest).Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).Include(r => r.RequestStatus).ToPagedListAsync(pageNumber, 25);
            }
            catch (Exception ex)
            {
                //do something here 
            }

            TempData["Search"] = "True";

            return View("Index", onePageOfProducts);
        }


        /*
         * END SEARCH
         */

        /*
         * BEGIN CART
         */
        public async Task<IActionResult> Cart()
        {
            TempData["PageType"] = AppUtility.RequestPageTypeEnum.Cart;
            return View();
        }
        /*
         * END CART
         */


        /*
         * START RECEIVED MODAL
         */

        [HttpGet]
        public async Task<IActionResult> ReceivedModal()
        {
            ReceivedLocationViewModel receivedLocationViewModel = new ReceivedLocationViewModel()
            {
                locationTypesDepthZero = _context.LocationTypes.Where(lt => lt.Depth == 0),
                locationInstancesSelected = new List<LocationInstance>()
            };
            receivedLocationViewModel.locationInstancesSelected.Add(new LocationInstance());

            return View(receivedLocationViewModel);
        }

        [HttpGet]
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


        /*
         * END RECEIVED MODAL
         */


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
        public JsonResult GetCompanyAccountList(int PaymentTypeID)
        {
            var companyAccountList = _context.CompanyAccounts.Where(c => c.PaymentTypeID == PaymentTypeID).ToList();
            return Json(companyAccountList);
        }


    }
}
