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
using SQLitePCL;

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
                onePageOfProducts = await _context.Calibrations.Include(c => c.Request).ThenInclude(r => r.Product).ThenInclude(p => p.Vendor).Include(c => c.Request.Product.ProductSubcategory).Include(c => c.CalibrationType).ToPagedListAsync(page, 25);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }

            return View(onePageOfProducts);
        }

        // GET: ProductSubcategories
        [Authorize(Roles = "Admin, LabManagment")]
        [HttpGet]

        public async Task<IActionResult> CreateCalibration(int requestid)
        {
            var request = await _context.Requests.Where(r => r.RequestID == requestid).Include(r => r.Product).FirstOrDefaultAsync();
            var calibrations = await _context.Calibrations.Where(c => c.RequestID == requestid).ToListAsync();
            List<ExternalCalibration> externalCalibrations = await _context.ExternalCalibrations.Where(c => c.RequestID == requestid).ToListAsync();
            List<InternalCalibration> internalCalibrations = await _context.InternalCalibrations.Where(c => c.RequestID == requestid).ToListAsync();
            List<Repair> repairs = await _context.Repairs.Where(c => c.RequestID == requestid).ToListAsync();
            CreateCalibrationViewModel createCalibrationViewModel = new CreateCalibrationViewModel
            {
                ProductDescription = request.Product.ProductName,
                PastCalibrations = _context.Calibrations.Where(c => c.RequestID == requestid).Where(c => c.Date < DateTime.Now).Include(c => c.CalibrationType).ToList(),
                Repairs = repairs,
                ExternalCalibrations = externalCalibrations,
                InternalCalibration = internalCalibrations
            };


            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.LabManagementSidebarEnum.Calibrate;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.LabManagementPageTypeEnum.Equipment;
            return View(createCalibrationViewModel);
        }

        [Authorize(Roles = "Admin, LabManagment")]
        [HttpGet]
        public async Task<IActionResult> _Repairs(int RepairIndex)
        {
            //var repair = _context.Repairs.Where(c => c.CalibrationID == calibrationId).FirstOrDefault();
            _RepairsViewModel repairsViewModel = new _RepairsViewModel()
            {
                Repair = new Repair()
                {
                    Date = DateTime.Now
                },
                RepairIndex = RepairIndex + 1,
                IsNew = true
            };

            return PartialView(repairsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, LabManagment")]
        public async Task<IActionResult> SaveRepairs(CreateCalibrationViewModel vm)
        {
            foreach (var repair in vm.Repairs)
            {
                if (repair.CalibrationID > 0) //an old repair
                {
                    var updatedRepair = _context.Repairs.Where(c => c.CalibrationID == repair.CalibrationID).FirstOrDefault();
                    updatedRepair.Date = repair.Date;
                    updatedRepair.Days = repair.Days;
                    updatedRepair.Months = repair.Months;
                    updatedRepair.IsRepeat = repair.IsRepeat;
                    updatedRepair.Description = repair.Description;
                    _context.Update(updatedRepair);
                    await _context.SaveChangesAsync();
                }
                else // a new repair
                {
                    repair.RequestID = vm.RequestID;
                    repair.CalibrationTypeID = 1;
                    _context.Add(repair);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }


        //writing this in a function because it's also called from the RenderPartialAsync in the view
        [Authorize(Roles = "Admin, LabManagment")]
        public static _RepairsViewModel GetRepairsViewModel(int requestId, Repair repair = null)
        {
            _RepairsViewModel repairsViewModel = new _RepairsViewModel()
            {
                RequestID = requestId,
                Repair = new Repair() { Date = DateTime.Now }

            };
            if (repair != null)
            {
                repairsViewModel.Repair = repair;
            }
            return repairsViewModel;
        }

        [Authorize(Roles = "Admin, LabManagment")]
        [HttpGet]
        public async Task<IActionResult> _ExternalCalibration(int requestId, int? CalibrationID = null)
        {
            return PartialView(GetExternalCalibrationViewModel(requestId, CalibrationID));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, LabManagment")]
        public async Task<IActionResult> _ExternalCalibration(_ExternalCalibrationViewModel externalCalibrationViewModel)
        {
            return RedirectToAction("Index");
        }

        //writing this in a function because it's also called from the RenderPartialAsync in the view
        [Authorize(Roles = "Admin, LabManagment")]
        public static _ExternalCalibrationViewModel GetExternalCalibrationViewModel(int requestId, int? calibrationId = null)
        {
            _ExternalCalibrationViewModel externalCalibrationViewModel = new _ExternalCalibrationViewModel();
            if (calibrationId != null)
            {
                externalCalibrationViewModel.RequestID = requestId;
                externalCalibrationViewModel.ExternalCalibration = new ExternalCalibration();
            }
            return externalCalibrationViewModel;
        }

        [Authorize(Roles = "Admin, LabManagment")]
        [HttpGet]
        public async Task<IActionResult> _InternalCalibration(int requestId, int? CalibrationID = null)
        {
            return PartialView(GetInternalCalibrationViewModel(requestId, CalibrationID));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, LabManagment")]
        public async Task<IActionResult> _InternalCalibration(_InternalCalibrationViewModel internalCalibrationViewModel)
        {
            return RedirectToAction("Index");
        }

        //writing this in a function because it's also called from the RenderPartialAsync in the view
        [Authorize(Roles = "Admin, LabManagment")]
        public static _InternalCalibrationViewModel GetInternalCalibrationViewModel(int requestId, int? calibrationId = null)
        {
            _InternalCalibrationViewModel internalCalibrationViewModel = new _InternalCalibrationViewModel();
            if (calibrationId != null)
            {
                internalCalibrationViewModel.RequestID = requestId;
                internalCalibrationViewModel.InternalCalibration = new InternalCalibration();
            }
            return internalCalibrationViewModel;
        }

    }
}
