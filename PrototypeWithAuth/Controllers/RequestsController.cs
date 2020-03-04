using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using X.PagedList;

namespace PrototypeWithAuth.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int _amountNew = 0; private int _amountOrdered = 0; private int _amountReceived = 0;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index(int? subcategoryID, int? vendorID, int? RequestStatusID, int? page, AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request)
        {
            //instantiate your list of requests to pass into the index
            IQueryable<Request> fullRequestsList = _context.Requests;
            //use an iqueryable (not ienumerable) until it's passed in so you can include the vendors and subcategories later on
            IQueryable<Request> RequestsPassedIn = Enumerable.Empty<Request>().AsQueryable();
            //use an enum to determine which page type you are using and fill the data accordingly, 
            //also pass the data through tempdata to the page so you can 
            TempData["PageType"] = PageType;
            //instantiating the ints to keep track of the amounts- will then pass into tempdata to use on the frontend
            //if it is a request page --> get all the requests with a new or ordered request status
            if (PageType == AppUtility.RequestPageTypeEnum.Request)
            {
                /*
                 * In order to combine all the requests each one needs to be in a separate list
                 * Then you need to union them one at a time into separate variables
                 * you only need the separate union variable in order for the union to work
                 * and the original queries are on separate lists because each is querying the full database with a separate where
                 */

                IQueryable<Request> TempRequestList = Enumerable.Empty<Request>().AsQueryable();
                if (RequestStatusID == null || RequestStatusID == 1)
                {
                    TempRequestList = fullRequestsList.Where(r => r.RequestStatusID == 1);
                    RequestsPassedIn = TempRequestList;
                    _amountNew += TempRequestList.Count();
                }
                if (RequestStatusID == null || RequestStatusID == 2)
                {
                    TempRequestList = fullRequestsList.Where(r => r.RequestStatusID == 2);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                    _amountOrdered += TempRequestList.Count();
                }
                if (RequestStatusID == null || RequestStatusID == 3)
                {
                    TempRequestList = fullRequestsList.Where(r => r.RequestStatusID == 3).Take(50);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                    _amountReceived += TempRequestList.Count();
                }
                if (RequestStatusID == null || RequestStatusID == 4)
                {
                    TempRequestList = fullRequestsList.Where(r => r.RequestStatusID == 4);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                    _amountOrdered += TempRequestList.Count();
                }
                if (RequestStatusID == null || RequestStatusID == 5)
                {
                    TempRequestList = fullRequestsList.Where(r => r.RequestStatusID == 5);
                    RequestsPassedIn = AppUtility.CombineTwoRequestsLists(RequestsPassedIn, TempRequestList);
                    _amountOrdered += TempRequestList.Count();
                }

            }
            //if it is an inventory page --> get all the requests with received and is inventory request status
            else if (PageType == AppUtility.RequestPageTypeEnum.Inventory)
            {
                //partial and clarify?
                RequestsPassedIn = fullRequestsList.Where(r => r.RequestStatus.RequestStatusDescription == "RecievedAndIsInventory");
            }
            else
            {
                RequestsPassedIn = fullRequestsList;
            }

            //now that the lists are created sort by vendor or subcategory
            if (vendorID > 0)
            {
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.VendorID == vendorID);
                //pass the vendorID into the temp data to use if you'd like to sort from there
                TempData["VendorID"] = vendorID;
            }
            else if (subcategoryID > 0)
            {
                RequestsPassedIn = RequestsPassedIn
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.ProductSubcategoryID == subcategoryID);
                //pass the subcategoryID into the temp data to use if you'd like to sort from there
                TempData["SubcategoryID"] = subcategoryID;
            }

            //passing in the amounts to display in the top buttons
            TempData["AmountNew"] = _amountNew;
            TempData["AmountOrdered"] = _amountOrdered;
            TempData["AmountReceived"] = _amountReceived;

            //Getting the page that is going to be seen (if no page was specified it will be one
            var pageNumber = page ?? 1;
            var onePageOfProducts = RequestsPassedIn.Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).Include(r => r.RequestStatus).ToPagedList(pageNumber, 4);

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
        public IActionResult Create() //need to correct to be for request
        {
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName");
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID");
            var parentCategories = _context.ParentCategories.ToList();
            var productSubcategories = _context.ProductSubcategories.ToList();
            var vendors = _context.Vendors.ToList();
            var requeststatuses = _context.RequestStatuses.ToList();

            var viewModel = new RequestItemViewModel
            {
                ParentCategories = parentCategories,
                ProductSubcategories = productSubcategories,
                Vendors = vendors,
                RequestStatuses = requeststatuses,
                Request = new Request()
            };

            viewModel.Request.Product = new Product(); // have to instantaiate the product from the requests, because the viewModel relies on request.product to create the new product
            viewModel.Request.ParentRequest = new ParentRequest();

            //adding a tempdata so the testing in the requestnavview layout page will work (better to create a base controller for temp data and store it there)
            //https://stackoverflow.com/questions/37267586/how-to-check-condition-if-temp-data-has-value-in-every-controller
            //if putting it here should get from somewhere and not just make it up...
            TempData["PageType"] = AppUtility.RequestPageTypeEnum.Request;
            return View(viewModel);
        }

        [HttpGet] //send a json to that the subcategory list is filered
        public JsonResult GetSubCategoryList(int ParentCategoryId)
        {
            var subCategoryList = _context.ProductSubcategories.Where(c => c.ParentCategoryID == ParentCategoryId).ToList();
            return Json(subCategoryList);

        }


        // POST: Requests/Create/ 
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Adina's code for creating and binding product model with request model in a single view, check that all errors are handled for.

        [HttpPost]
        [ValidateAntiForgeryToken]

        //CREATE PARENTREQUEST!!!!!!!!!



        public async Task<IActionResult> Create(RequestItemViewModel AddNewItemViewModelObj)
        {
            //insert the lists of parentcategories, productcategories, and vendors into the view model object for validation purposes and to return the same view if needed
            var parentCategories = _context.ParentCategories.ToList();
            var productSubcategories = _context.ProductSubcategories.ToList();
            var vendors = _context.Vendors.ToList();
            var requeststatuses = _context.RequestStatuses.ToList();
            AddNewItemViewModelObj.ParentCategories = parentCategories;
            AddNewItemViewModelObj.ProductSubcategories = productSubcategories;
            AddNewItemViewModelObj.Vendors = vendors;
            AddNewItemViewModelObj.RequestStatuses = requeststatuses;

            // view data is placed in the beginning in order to redirect when errors are caught, so need to have the info saved before handling the error
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", AddNewItemViewModelObj.Request.ParentRequest.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", AddNewItemViewModelObj.Request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", AddNewItemViewModelObj.Request.RequestStatusID);

            //inserting the vendor from the vendor id, the subcategory from the subcategory id and the application user from the application user id to test for the viewmodel validation
            AddNewItemViewModelObj.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == AddNewItemViewModelObj.Request.Product.VendorID);
            AddNewItemViewModelObj.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == AddNewItemViewModelObj.Request.Product.ProductSubcategoryID);
            AddNewItemViewModelObj.Request.ParentRequest.ApplicationUser = _context.Users.FirstOrDefault(u => u.Id == AddNewItemViewModelObj.Request.ParentRequest.ApplicationUserID);

            //using the dataannotations validator to test the updated object because modelstate.isvalid only looks at the stack trace that was passed in 
            var context = new ValidationContext(AddNewItemViewModelObj.Request, null, null);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(AddNewItemViewModelObj.Request, context, results, true))
            {
                _context.Add(AddNewItemViewModelObj.Request);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(AddNewItemViewModelObj);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RequestItemViewModel addNewItemViewModel)
        {
            //same logic as create controller
            addNewItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            addNewItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            addNewItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            addNewItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();

            addNewItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == addNewItemViewModel.Request.Product.VendorID);
            addNewItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == addNewItemViewModel.Request.Product.ProductSubcategoryID);
            addNewItemViewModel.Request.ParentRequest.ApplicationUser = _context.Users.FirstOrDefault(u => u.Id == addNewItemViewModel.Request.ParentRequest.ApplicationUserID);

            var context = new ValidationContext(addNewItemViewModel.Request, null, null);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(addNewItemViewModel.Request, context, results, true))
            {
                try
                {
                    _context.Update(addNewItemViewModel.Request.Product);
                    _context.Update(addNewItemViewModel.Request);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return await ModalView(addNewItemViewModel.Request.RequestID);
                }
            }
            else
            {
                return await ModalView(addNewItemViewModel.Request.RequestID);
            }
            return RedirectToAction("Index");
        }


        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.RequestID == id);
        }

        /*
         * START MODAL VIEW COPY
         */
        public async Task<IActionResult> ModalView(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else if (id == 0)
            {

            }
            else
            {

            }
            var parentcategories = await _context.ParentCategories.ToListAsync();
            var productsubactegories = await _context.ProductSubcategories.ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();

            RequestItemViewModel addNewItemViewModel = new RequestItemViewModel
            {
                Request = _context.Requests.Include(r => r.Product)
                    //.Include(r => r.RequestStatus) =>not needed as of now
                    .Include(r => r.Product.ProductSubcategory)
                    .Include(r => r.Product.ProductSubcategory.ParentCategory)
                    .SingleOrDefault(x => x.RequestID == id),
                ParentCategories = parentcategories,
                ProductSubcategories = productsubactegories,
                Vendors = vendors
            };
            if (addNewItemViewModel.Request == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", addNewItemViewModel.Request.ParentRequest.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", addNewItemViewModel.Request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", addNewItemViewModel.Request.RequestStatusID);
            return PartialView(addNewItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModalView(RequestItemViewModel addNewItemViewModel)
        {
            //same logic as create controller
            addNewItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            addNewItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            addNewItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            addNewItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();

            addNewItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == addNewItemViewModel.Request.Product.VendorID);
            addNewItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == addNewItemViewModel.Request.Product.ProductSubcategoryID);
            addNewItemViewModel.Request.ParentRequest.ApplicationUser = _context.Users.FirstOrDefault(u => u.Id == addNewItemViewModel.Request.ParentRequest.ApplicationUserID);

            //in case we need to redirect to action
            TempData["ModalView"] = true;
            TempData["RequestID"] = addNewItemViewModel.Request.RequestID;

            var orderDocs = addNewItemViewModel.OrderDocs;
            var invoiceDocs = addNewItemViewModel.InvoiceDocs;
            var shipmentDocs = addNewItemViewModel.ShipmentDocs;
            var quoteDocs = addNewItemViewModel.QuoteDocs;
            var infoDocs = addNewItemViewModel.InfoDocs;
            var picturesDocs = addNewItemViewModel.PicturesDocs;
            var returnDocs = addNewItemViewModel.ReturnDocs;
            var creditDocs = addNewItemViewModel.CreditDocs; 

            int countFileUploads = 0;
            //saving the files (will return request to take off the files or get better ones if doesn't go through)
            long size = orderDocs.Sum(f => f.Length);
            //full path to file in temp location
            var filePath = Path.GetTempFileName();
            foreach (var formFile in orderDocs)
            { 
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        try
                        {
                            await formFile.CopyToAsync(stream);
                            countFileUploads += 1;
                        }
                        catch
                        { //catch exception here }
                        }
                    }
                }
            }

            if (countFileUploads < size)
            {
                //return to the modal view with error
            }

            var context = new ValidationContext(addNewItemViewModel.Request, null, null);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(addNewItemViewModel.Request, context, results, true))
            {
                /*
                 * the viewmodel loads the request.product with a primary key of 0
                 * so if you don't insert the request.productid into the request.product.product id
                 * it will create a new one instead of updating the existing one
                 */
                addNewItemViewModel.Request.Product.ProductID = addNewItemViewModel.Request.ProductID;
                try
                {
                    _context.Update(addNewItemViewModel.Request.Product);
                    _context.Update(addNewItemViewModel.Request);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.ToString();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(addNewItemViewModel);
            }
            return RedirectToAction("Index");
        }

        /*
         * END MODAL VIEW COPY
         */



    }
}
