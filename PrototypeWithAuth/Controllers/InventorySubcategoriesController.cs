using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.Controllers
{
    public class InventorySubcategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventorySubcategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InventorySubcategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.InventorySubcategories.ToListAsync());
        }

        // GET: InventorySubcategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventorySubcategory = await _context.InventorySubcategories
                .FirstOrDefaultAsync(m => m.InventorySubcategoryID == id);
            if (inventorySubcategory == null)
            {
                return NotFound();
            }

            return View(inventorySubcategory);
        }

        // GET: InventorySubcategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InventorySubcategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InventorySubcategoryID,InventorySubcategoryDescription")] InventorySubcategory inventorySubcategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventorySubcategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventorySubcategory);
        }

        // GET: InventorySubcategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventorySubcategory = await _context.InventorySubcategories.FindAsync(id);
            if (inventorySubcategory == null)
            {
                return NotFound();
            }
            return View(inventorySubcategory);
        }

        // POST: InventorySubcategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventorySubcategoryID,InventorySubcategoryDescription")] InventorySubcategory inventorySubcategory)
        {
            if (id != inventorySubcategory.InventorySubcategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventorySubcategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventorySubcategoryExists(inventorySubcategory.InventorySubcategoryID))
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
            return View(inventorySubcategory);
        }

        // GET: InventorySubcategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventorySubcategory = await _context.InventorySubcategories
                .FirstOrDefaultAsync(m => m.InventorySubcategoryID == id);
            if (inventorySubcategory == null)
            {
                return NotFound();
            }

            return View(inventorySubcategory);
        }

        // POST: InventorySubcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventorySubcategory = await _context.InventorySubcategories.FindAsync(id);
            _context.InventorySubcategories.Remove(inventorySubcategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventorySubcategoryExists(int id)
        {
            return _context.InventorySubcategories.Any(e => e.InventorySubcategoryID == id);
        }
    }
}
