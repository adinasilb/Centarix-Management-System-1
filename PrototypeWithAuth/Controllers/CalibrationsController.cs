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
using PrototypeWithAuth.ViewModels;
using X.PagedList;

namespace PrototypeWithAuth.Controllers
{
    public class CalibrationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalibrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductSubcategories
        [Authorize(Roles = "Admin, LabManagment")]
        public async Task<IActionResult> Index(int? page)
        {

            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.LabManagementSidebarEnum.Calibrate;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.LabManagementPageTypeEnum.Equipment;
            //Getting the page that is going to be seen (if no page was specified it will be one)
            var pageNumber = page ?? 1;
            var onePageOfProducts = Enumerable.Empty<Calibration>().ToPagedList();

            try
            {
               onePageOfProducts = await _context.Calibrations.Include(c => c.Request).ThenInclude(r => r.Product).ThenInclude(p => p.Vendor).Include(c => c.Request.Product.ProductSubcategory).ToPagedListAsync(page, 25);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }

            return View(onePageOfProducts);
        }

    }
}
