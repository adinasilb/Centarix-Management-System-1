﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace PrototypeWithAuth.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index(int? subcategoryID, int? vendorID, AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.None)
        {
            //instantiate your list of requests to pass into the index
            var requests1 = _context.Requests;
            //use an iqueryable (not ienumerable) until its passed in so you can include the vendors and subcategories later on
            IQueryable<Request> requests4 = null;
            //use an enum to determine which page type you are using and fill the data accordingly, 
            //also pass the data through tempdata to the page so you can 
            TempData["PageType"] = PageType;
            //if it is a request page --> get all the requests with a new or ordered request status
            if (PageType == AppUtility.RequestPageTypeEnum.Request)
            {
                //get all the requests that go on the request page
                //do i have to include requeststatus somewhere here? or is it ok that i included it before?
                var requests2 = requests1.Where(r => r.RequestStatus.RequestStatusDescription == "New");
                var requests3 = requests1.Where(r => r.RequestStatus.RequestStatusDescription == "Ordered");
                //partial and clarify?
                requests4 = requests2.Concat(requests3);
            }
            //if it is an inventory page --> get all the requests with received and is inventory request status
            else if (PageType == AppUtility.RequestPageTypeEnum.Inventory)
            {
                //partial and clarify?
                requests4 = requests1.Where(r => r.RequestStatus.RequestStatusDescription == "RecievedAndIsInventory");
            }

            //now that the lists are created sort by vendor or subcategory
            if (vendorID != null)
            {
                requests4 = requests4
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.VendorID == vendorID);
                //return View(await requests4.Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).ToListAsync());
            }
            else if (subcategoryID != null)
            {
                requests4 = requests4
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.ProductSubcategoryID == subcategoryID);
                //return View(await requests4.Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).ToListAsync());
            }
            else
            {
                requests4 = requests1;
            }
            //else
            //{
            //    var applicationDbContext = _context.Requests.Include(r => r.ApplicationUser).Include(r => r.Product).Include(r => r.RequestStatus).Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor);
            //    return View(await applicationDbContext.ToListAsync());
            //    //return View(await _context.Requests.Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).ToListAsync());
            //}
            return View(await requests4.Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).Include(r => r.RequestStatus).ToListAsync());
            //var applicationDbContext = _context.Requests.Include(r => r.ApplicationUser).Include(r => r.Product).Include(r => r.RequestStatus);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.ApplicationUser)
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
            var requeststatutes = _context.RequestStatuses.ToList();

            var viewModel = new AddNewItemViewModel
            {
                ParentCategories = parentCategories,
                ProductSubcategories = productSubcategories,
                Vendors = vendors,
                RequestStatuses = requeststatutes,
                Request = new Request()
            };

            viewModel.Request.Product = new Product(); // have to instantaiate the product from the requests, because the viewModel relies on request.product to create the new product

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
        public async Task<IActionResult> Create(AddNewItemViewModel AddNewItemViewModelObj)
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
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", AddNewItemViewModelObj.Request.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", AddNewItemViewModelObj.Request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", AddNewItemViewModelObj.Request.RequestStatusID);

            //inserting the vendor from the vendor id, the subcategory from the subcategory id and the application user from the application user id to test for the viewmodel validation
            AddNewItemViewModelObj.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == AddNewItemViewModelObj.Request.Product.VendorID);
            AddNewItemViewModelObj.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == AddNewItemViewModelObj.Request.Product.ProductSubcategoryID);
            AddNewItemViewModelObj.Request.ApplicationUser = _context.Users.FirstOrDefault(u => u.Id == AddNewItemViewModelObj.Request.ApplicationUserID);

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
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", request.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", request.RequestStatusID);
            return View(request);
        }

        //POST: Requests/Edit/5
        //This is not being used right now --> delete 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddNewItemViewModel addNewItemViewModel)
        {
            //same logic as create controller
            addNewItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            addNewItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            addNewItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            addNewItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();

            addNewItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == addNewItemViewModel.Request.Product.VendorID);
            addNewItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == addNewItemViewModel.Request.Product.ProductSubcategoryID);
            addNewItemViewModel.Request.ApplicationUser = _context.Users.FirstOrDefault(u => u.Id == addNewItemViewModel.Request.ApplicationUserID);

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
                .Include(r => r.ApplicationUser)
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

            AddNewItemViewModel addNewItemViewModel = new AddNewItemViewModel
            {
                Request = _context.Requests.Include(r => r.Product)
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
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", addNewItemViewModel.Request.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", addNewItemViewModel.Request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", addNewItemViewModel.Request.RequestStatusID);
            return PartialView(addNewItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModalView(AddNewItemViewModel addNewItemViewModel)
        {
            //same logic as create controller
            addNewItemViewModel.ParentCategories = await _context.ParentCategories.ToListAsync();
            addNewItemViewModel.ProductSubcategories = await _context.ProductSubcategories.ToListAsync();
            addNewItemViewModel.Vendors = await _context.Vendors.ToListAsync();
            addNewItemViewModel.RequestStatuses = await _context.RequestStatuses.ToListAsync();

            addNewItemViewModel.Request.Product.Vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == addNewItemViewModel.Request.Product.VendorID);
            addNewItemViewModel.Request.Product.ProductSubcategory = _context.ProductSubcategories.FirstOrDefault(ps => ps.ProductSubcategoryID == addNewItemViewModel.Request.Product.ProductSubcategoryID);
            addNewItemViewModel.Request.ApplicationUser = _context.Users.FirstOrDefault(u => u.Id == addNewItemViewModel.Request.ApplicationUserID);

            //in case we need to redirect to action
            TempData["ModalView"] = true;
            TempData["RequestID"] = addNewItemViewModel.Request.RequestID;

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

        //Get the Json of the vendor business id from the vendor id so JS (site.js) can auto load the form-control with the newly requested vendor
        [HttpGet] //send a json to that the subcategory list is filered
        public JsonResult GetVendorBusinessID(int VendorID)
        {
            Vendor vendor = _context.Vendors.SingleOrDefault(v => v.VendorID == VendorID);
            return Json(vendor);
        }

    }
}
