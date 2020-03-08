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
            TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.Type;
            var applicationDbContext = _context.ProductSubcategories.Include(p => p.ParentCategory);
            return View(await applicationDbContext.ToListAsync());
        }
        
        [HttpGet] //send a json to that the subcategory list is filered
        public JsonResult GetSubCategoryList(int ParentCategoryId)
        {
            var subCategoryList = _context.ProductSubcategories.Where(c => c.ParentCategoryID == ParentCategoryId).ToList();
            return Json(subCategoryList);

        }

    }
}
