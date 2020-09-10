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
using Abp.Extensions;

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
        [Authorize(Roles = "Admin, OrdersAndInventory, Operation")]
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, int categoryType=1)
        {
            if (categoryType == 1)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.OrdersAndInventory;
                TempData["SidebarTitle"] = AppUtility.OrdersAndInventorySidebarEnum.Type;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            }
            else if (categoryType == 2)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operation;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.OperationsSidebarEnum.Type;
                if (PageType == AppUtility.RequestPageTypeEnum.Inventory)
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.OperationsPageTypeEnum.InventoryOperations;
                }
                else if (PageType == AppUtility.RequestPageTypeEnum.Request)
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.OperationsPageTypeEnum.RequestOperations;

                }
            }
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
