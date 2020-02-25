using System;
using System.Collections.Generic;
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

        // GET: Products/Create
        public IActionResult Create(int? parentCategoryID) //need to correct to be for request
        {
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName");
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID");
            var parentCategories = _context.ParentCategories.ToList();
            var productSubcategories1 = _context.ProductSubcategories.ToList();

            var viewModel = new AddNewItemViewModel
            {
                ParentCategories = parentCategories,
                ProductSubcategories = productSubcategories1
            };
            return View(viewModel);


        }


        [HttpGet] //send a json to that the subcategory list is filtered
        public JsonResult GetSubCategoryList(int ParentCategoryId)
        {
            var subCategoryList = _context.ProductSubcategories.Where(c => c.ParentCategoryID == ParentCategoryId).ToList();
            return Json(subCategoryList);

        }


        // POST: Requests/Create/ 
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Adina's code fore createing and binding product model with request model in a single view, check that all errors are handled for.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddNewItemViewModel AddNewItemViewModelObj)
        {
            var parentCategories = _context.ParentCategories.ToList();
            var productSubcategories = _context.ProductSubcategories.ToList();
            AddNewItemViewModelObj.ParentCategories = parentCategories;
            AddNewItemViewModelObj.ProductSubcategories = productSubcategories;

            // view data is placed in th ebeginning in order to redirect when errrs are caught, so need to have the info saved before handling the error
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", AddNewItemViewModelObj.Request.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", AddNewItemViewModelObj.Request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", AddNewItemViewModelObj.Request.RequestStatusID);


            Product product = new Product
            {
                ProductID = AddNewItemViewModelObj.Product.ProductID,
                ProductName = AddNewItemViewModelObj.Product.ProductName,
                VendorID = AppUtility.ReplaceIntValueIfNull(AddNewItemViewModelObj.Product.VendorID),
                ProductSubcategoryID = AddNewItemViewModelObj.Product.ProductSubcategoryID,
                LocationID = AddNewItemViewModelObj.Product.LocationID,
                Handeling = AppUtility.ReplaceStringValueIfNull(AddNewItemViewModelObj.Product.Handeling),
                QuantityPerUnit = AppUtility.ReplaceIntValueIfNull(AddNewItemViewModelObj.Product.QuantityPerUnit),
                UnitsInStock = AppUtility.ReplaceIntValueIfNull(AddNewItemViewModelObj.Product.UnitsInStock),
                UnitsInOrder = AppUtility.ReplaceIntValueIfNull(AddNewItemViewModelObj.Product.UnitsInOrder),
                ReorderLevel = AppUtility.ReplaceIntValueIfNull(AddNewItemViewModelObj.Product.ReorderLevel),
                ProductComment = AppUtility.ReplaceStringValueIfNull(AddNewItemViewModelObj.Product.ProductComment),
                ProductMedia = AppUtility.ReplaceStringValueIfNull(AddNewItemViewModelObj.Product.ProductMedia)
            };
            try
            {
                _context.Add(product);
                _context.SaveChanges();
                Request request = new Request
                {
                    RequestID = AddNewItemViewModelObj.Request.RequestID,
                    ProductID = product.ProductID,
                    LocationID = AddNewItemViewModelObj.Product.LocationID,
                    RequestStatusID = AddNewItemViewModelObj.Request.RequestStatusID,
                    AmountWithInLocation = AddNewItemViewModelObj.Request.AmountWithInLocation,
                    AmountWithOutLocation = AddNewItemViewModelObj.Request.AmountWithOutLocation,
                    ApplicationUserID = AddNewItemViewModelObj.Request.ApplicationUserID,
                    OrderDate = AddNewItemViewModelObj.Request.OrderDate,
                    OrderNumber = AddNewItemViewModelObj.Request.OrderNumber,
                    Quantity = AddNewItemViewModelObj.Request.Quantity,
                    Cost = AddNewItemViewModelObj.Request.Cost,
                    WithOrder = AddNewItemViewModelObj.Request.WithOrder,
                    InvoiceNumber = AddNewItemViewModelObj.Request.InvoiceNumber,
                    CatalogNumber = AddNewItemViewModelObj.Request.CatalogNumber,
                    SerialNumber = AddNewItemViewModelObj.Request.SerialNumber,
                    URL = AddNewItemViewModelObj.Request.URL,
                };
                try
                {
                    _context.Add(request);
                    await _context.SaveChangesAsync();
                }
                catch //(Exception ex)
                {
                    _context.Remove(product);
                    await _context.SaveChangesAsync();
                    return View(AddNewItemViewModelObj);
                }
                if (request != null)
                {
                    return RedirectToAction("Index");
                }
                return View(AddNewItemViewModelObj);
            }
            catch (Exception ex)
            {
                return View(AddNewItemViewModelObj);
            }

        }

        /* Working Creating Product and Model
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create(AddNewItemViewModel AddNewItemViewModelObj)
    //// "RequestID,ProductID,LocationID,RequestStatusID,AmountWithInLocation,AmountWithOutLocation,ApplicationUserID,OrderDate,OrderNumber,Quantity,Cost,WithOrder,InvoiceNumber,CatalogNumber,SerialNumber,URL")]
    //{
    //    var parentCategories = _context.ParentCategories.ToList();
    //    var productSubcategories = _context.ProductSubcategories.ToList();
    //    AddNewItemViewModelObj.ParentCategories = parentCategories;
    //    AddNewItemViewModelObj.ProductSubcategories = productSubcategories;


    //    ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", AddNewItemViewModelObj.Request.ApplicationUserID);
    //    ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", AddNewItemViewModelObj.Request.ProductID);
    //    ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", AddNewItemViewModelObj.Request.RequestStatusID);

    //    Product product = new Product
    //    {
    //        ProductID = AddNewItemViewModelObj.Product.ProductID,
    //        ProductName = AddNewItemViewModelObj.Product.ProductName,
    //        VendorID = /*AddNewItemViewModelObj.Product.VendorID*/
        //_context.Vendors.ToList()[0].VendorID,
        //        ProductSubcategoryID = AddNewItemViewModelObj.Product.ProductSubcategoryID,
        //        LocationID = AddNewItemViewModelObj.Product.LocationID,
        //        Handeling = AddNewItemViewModelObj.Product.Handeling,
        //        QuantityPerUnit = /*AddNewItemViewModelObj.Product.QuantityPerUnit*/0,
        //        UnitsInStock = /*AddNewItemViewModelObj.Product.UnitsInStock*/0,
        //        UnitsInOrder = /*AddNewItemViewModelObj.Product.UnitsInOrder*/0,
        //        ReorderLevel = /*AddNewItemViewModelObj.Product.ReorderLevel*/0,
        //        ProductComment = /*AddNewItemViewModelObj.Product.ProductComment*/"comment...",
        //        ProductMedia = /*AddNewItemViewModelObj.Product.ProductMedia*/"media..."
        //    };
        //    try
        //    {
        //        _context.Add(product);
        //        _context.SaveChanges();
        //    }
        //    catch(Exception)
        //    {
        //        return View(AddNewItemViewModelObj.Request);
        //    }
        //    //AddNewItemViewModelObj.Product = product;
        //    //AddNewItemViewModelObj.Request.ProductID = product.ProductID;


        //    if (product != null)
        //    {
        //        Request request = new Request
        //        {
        //            RequestID = AddNewItemViewModelObj.Request.RequestID,
        //            ProductID = product.ProductID,
        //            LocationID = product.LocationID,
        //            RequestStatusID = AddNewItemViewModelObj.Request.RequestStatusID,
        //            AmountWithInLocation = AddNewItemViewModelObj.Request.AmountWithInLocation,
        //            AmountWithOutLocation = AddNewItemViewModelObj.Request.AmountWithOutLocation,
        //            ApplicationUserID = AddNewItemViewModelObj.Request.ApplicationUserID,
        //            OrderDate = AddNewItemViewModelObj.Request.OrderDate,
        //            OrderNumber = AddNewItemViewModelObj.Request.OrderNumber,
        //            Quantity = AddNewItemViewModelObj.Request.Quantity,
        //            Cost = AddNewItemViewModelObj.Request.Cost,
        //            WithOrder = true /*AddNewItemViewModelObj.Request.WithOrder*/,
        //            InvoiceNumber = AddNewItemViewModelObj.Request.InvoiceNumber,
        //            CatalogNumber = AddNewItemViewModelObj.Request.CatalogNumber,
        //            SerialNumber = AddNewItemViewModelObj.Request.SerialNumber,
        //            URL = AddNewItemViewModelObj.Request.URL
        //        };

        //        try
        //        {
        //            _context.Add(request);
        //            _context.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            return View(AddNewItemViewModelObj);
        //        }

        //        if (request != null)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }

        //    //if (ModelState.IsValid)
        //    //{
        //    //    //await _context.SaveChangesAsync();
        //    //    _context.Add(AddNewItemViewModelObj.Request);
        //    //    await _context.SaveChangesAsync();
        //    //    return RedirectToAction(nameof(Index));
        //    //}

        //    return View(AddNewItemViewModelObj);
        //}



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

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,ProductID,LocationID,RequestStatusID,AmountWithInLocation,AmountWithOutLocation,ApplicationUserID,OrderDate,OrderNumber,Quantity,Cost,WithOrder,InvoiceNumber,CatalogNumber,SerialNumber,URL")] Request request)
        {
            if (id != request.RequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.RequestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", request.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", request.RequestStatusID);
            return View(request);
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

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", request.ApplicationUserID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName", request.ProductID);
            ViewData["RequestStatusID"] = new SelectList(_context.RequestStatuses, "RequestStatusID", "RequestStatusID", request.RequestStatusID);
            return PartialView(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModalView(int id, [Bind("ProductID,ProductName,VendorID,ProductSubcategoryID,LocationID,QuantityPerUnit,UnitsInStock,UnitsInOrder,ReorderLevel,ProductComment,ProductMedia")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //Get ProductExists to work
                    /*if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }*/
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return PartialView(product);
        }

        /*
         * END MODAL VIEW COPY
         */
    }
}
