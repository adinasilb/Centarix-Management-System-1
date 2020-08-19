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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, int categoryType=1)
        {
            TempData["PageType"] = PageType;
            TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.Type;
            var applicationDbContext = _context.ProductSubcategories.Include(p => p.ParentCategory).ThenInclude(pc => pc.CategoryType)
                .Where(ps => ps.ParentCategory.CategoryTypeID == categoryType);
            return View(await applicationDbContext.ToListAsync());
        }
        
        [HttpGet] //send a json to that the subcategory list is filtered
        public JsonResult GetSubCategoryList(int? ParentCategoryId)
        {
            var subCategoryList = _context.ProductSubcategories.ToList();
            if (ParentCategoryId != null)
            {
                subCategoryList = _context.ProductSubcategories.Where(c => c.ParentCategoryID == ParentCategoryId).ToList();
            }
            return Json(subCategoryList);
        }

    }
}
