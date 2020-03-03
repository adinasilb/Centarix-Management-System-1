using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.Controllers
{
    public class ProductSubcategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductSubcategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductSubcategories
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request)
        {
            TempData["PageType"] = PageType;
            var applicationDbContext = _context.ProductSubcategories.Include(p => p.ParentCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        /*// GET: ProductSubcategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSubcategory = await _context.ProductSubcategories
                .Include(p => p.ParentCategory)
                .FirstOrDefaultAsync(m => m.ProductSubcategoryID == id);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            return View(productSubcategory);
        }

        // GET: ProductSubcategories/Create
        public IActionResult Create()
        {
            ViewData["ParentCategoryID"] = new SelectList(_context.ParentCategories, "ParentCategoryID", "ParentCategoryDescription");
            return View();
        }

        // POST: ProductSubcategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductSubcategoryID,ParentCategoryID,ProductSubcategoryDescription")] ProductSubcategory productSubcategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSubcategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategoryID"] = new SelectList(_context.ParentCategories, "ParentCategoryID", "ParentCategoryDescription", productSubcategory.ParentCategoryID);
            return View(productSubcategory);
        }

        // GET: ProductSubcategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSubcategory = await _context.ProductSubcategories.FindAsync(id);
            if (productSubcategory == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryID"] = new SelectList(_context.ParentCategories, "ParentCategoryID", "ParentCategoryDescription", productSubcategory.ParentCategoryID);
            return View(productSubcategory);
        }

        // POST: ProductSubcategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductSubcategoryID,ParentCategoryID,ProductSubcategoryDescription")] ProductSubcategory productSubcategory)
        {
            if (id != productSubcategory.ProductSubcategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSubcategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSubcategoryExists(productSubcategory.ProductSubcategoryID))
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
            ViewData["ParentCategoryID"] = new SelectList(_context.ParentCategories, "ParentCategoryID", "ParentCategoryDescription", productSubcategory.ParentCategoryID);
            return View(productSubcategory);
        }

        // GET: ProductSubcategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSubcategory = await _context.ProductSubcategories
                .Include(p => p.ParentCategory)
                .FirstOrDefaultAsync(m => m.ProductSubcategoryID == id);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            return View(productSubcategory);
        }

        // POST: ProductSubcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productSubcategory = await _context.ProductSubcategories.FindAsync(id);
            _context.ProductSubcategories.Remove(productSubcategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSubcategoryExists(int id)
        {
            return _context.ProductSubcategories.Any(e => e.ProductSubcategoryID == id);
        }*/
    }
}
