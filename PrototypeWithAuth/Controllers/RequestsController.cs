using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index(int? subcategoryID, int? vendorID)
        {
            if (vendorID != null)
            {
                var requests = _context.Requests
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.VendorID == vendorID);
                return View(await requests.Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).ToListAsync());
            }
            else if (subcategoryID != null)
            {
                var requests = _context.Requests
                    .OrderByDescending(r => r.ProductID)
                    .Where(r => r.Product.ProductSubcategoryID == subcategoryID);
                return View(await requests.Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Requests.Include(r => r.ApplicationUser).Include(r => r.Product).Include(r => r.RequestStatus).Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor);
                return View(await applicationDbContext.ToListAsync());
                //return View(await _context.Requests.Include(r => r.Product.ProductSubcategory).Include(r => r.Product.Vendor).ToListAsync());
            }

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

            var viewModel = new AddNewItemViewModel
            {
                ParentCategories = parentCategories,
                ProductSubcategories = productSubcategories,
                Vendors = vendors,
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
            AddNewItemViewModelObj.ParentCategories = parentCategories;
            AddNewItemViewModelObj.ProductSubcategories = productSubcategories;
            AddNewItemViewModelObj.Vendors = vendors;

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
                catch(Exception ex)
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
