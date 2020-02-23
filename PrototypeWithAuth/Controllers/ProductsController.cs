using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int ? subcategoryID, int ? vendorID)
        {
            if (vendorID != null)
            {
                var products = _context.Products
                    .OrderByDescending(p => p.ProductID)
                    .Where(p => p.VendorID == vendorID);
                return View(await products.Include(p => p.ProductSubcategory).Include(v => v.Vendor).ToListAsync());
            }
            else if (subcategoryID != null)
            {
                var products = _context.Products
                    .OrderByDescending(p => p.ProductID)
                    .Where(p => p.ProductSubcategoryID == subcategoryID);
                return View(await products.Include(p => p.ProductSubcategory).Include(v => v.Vendor).ToListAsync());
            }
            else
            {
                return View(await _context.Products.Include(p => p.ProductSubcategory).Include(v => v.Vendor).ToListAsync());
            }
            //return View(await _context.Products.Include(p => p.ProductSubcategory)./*Include(p => p.ProductSubcategory.ParentCategory).*/Include(v => v.Vendor).ToListAsync());
        
        }
        
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.ProductSubcategory)
                .Include(v =>v.Vendor)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /*
         * START ADINA'S COPY
         */
        public async Task<IActionResult> IndexCopy(int? subcategoryID, int? vendorID)
        {
            if (vendorID != null)
            {
                var products = _context.Products
                    .OrderByDescending(p => p.ProductID)
                    .Where(p => p.VendorID == vendorID);
                return View(await products.Include(p => p.ProductSubcategory).Include(v => v.Vendor).ToListAsync());
            }
            else if (subcategoryID != null)
            {
                var products = _context.Products
                    .OrderByDescending(p => p.ProductID)
                    .Where(p => p.ProductSubcategoryID == subcategoryID);
                return View(await products.Include(p => p.ProductSubcategory).Include(v => v.Vendor).ToListAsync());
            }
            else
            {
                return View(await _context.Products.Include(p => p.ProductSubcategory).Include(v => v.Vendor).ToListAsync());
            }
            //return View(await _context.Products.Include(p => p.ProductSubcategory)./*Include(p => p.ProductSubcategory.ParentCategory).*/Include(v => v.Vendor).ToListAsync());

        }


        public async Task<IActionResult> DetailsCopy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.ProductSubcategory)
                .Include(v => v.Vendor)
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            return PartialView(product);
        }

        /*
        * END ADINA'S COPY
        */

        // GET: Products/Create
        public IActionResult Create(int ? parentCategoryID)
        {
            var parentCategories = _context.ParentCategories.ToList();
            var productSubcategories1 = _context.ProductSubcategories.ToList();

            var viewModel = new CreateProductViewModel
            {
                ParentCategories = parentCategories,
                ProductSubcategories = productSubcategories1
            };
            return View(viewModel);

           
        }
        [HttpGet] //send a json to that the subcategory list is filered
        public JsonResult GetSubCategoryList(int ParentCategoryId)
        {
            var subCategoryList = _context.ProductSubcategories.Where(c => c.ParentCategoryID == ParentCategoryId).ToList();
            return Json(subCategoryList);

        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,VendorID,ProductSubcategoryID,ParentCategoryID,LocationID,QuantityPerUnit,UnitsInStock,UnitsInOrder,ReorderLevel,ProductComment,ProductMedia")]Product product)
        {
            
           // if (ModelState.IsValid)
          //  {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           // }
            //return View(viewModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,VendorID,ProductSubcategoryID,LocationID,QuantityPerUnit,UnitsInStock,UnitsInOrder,ReorderLevel,ProductComment,ProductMedia")] Product product)
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
                    if (!ProductExists(product.ProductID))
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
            return View(product);
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

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return PartialView(product);
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
                    if (!ProductExists(product.ProductID))
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
            return PartialView(product);
        }

        /*
         * END MODAL VIEW COPY
         */

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
