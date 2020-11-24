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
        public async Task<IActionResult> Index(String PageType = "", int categoryType=1)
        {
            if(PageType.Equals( AppUtility.LabManagementPageTypeEnum.Equipment.ToString()))
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.LabManagementSidebarEnum.EquipmentCategories;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.LabManagementPageTypeEnum.Equipment;
                var equipmentCategories = _context.ProductSubcategories.Include(p => p.ParentCategory).ThenInclude(pc => pc.CategoryType)
              .Where(ps => ps.ParentCategoryID ==5);
                return View(await equipmentCategories.ToListAsync());
            }
            else if (categoryType == 1)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
                TempData["SidebarTitle"] = AppUtility.OrdersAndInventorySidebarEnum.Type;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            }
            else if (categoryType == 2)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.OperationsSidebarEnum.Type;
                if (PageType.ToString() == AppUtility.RequestPageTypeEnum.Inventory.ToString())
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.OperationsPageTypeEnum.InventoryOperations;
                }
                else if (PageType.ToString() == AppUtility.RequestPageTypeEnum.Request.ToString())
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
