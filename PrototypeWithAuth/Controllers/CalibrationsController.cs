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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace PrototypeWithAuth.Controllers
{
    public class CalibrationsController : SharedController
    {
        public CalibrationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
            : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
        }

        // GET: ProductSubcategories
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> Index(int? page)
        {

            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Calibrate;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementEquipment;
            //Getting the page that is going to be seen (if no page was specified it will be one)
            int pageNumber = page ?? 1;
            IPagedList<Calibration> onePageOfProducts = Enumerable.Empty<Calibration>().ToPagedList();

            try
            {
                onePageOfProducts = await _context.Calibrations.Include(c => c.Request).ThenInclude(r => r.Product).ThenInclude(p => p.Vendor)
                    .Include(c => c.Request.Product.ProductSubcategory).Include(c => c.CalibrationType)
                    .OrderBy(c => c.Date).ThenBy(c => c.Request.Product.ProductName).ThenBy(c => c.CalibrationName)
                    .ToPagedListAsync(page, 25);
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
        [Authorize(Roles = "LabManagement")]
        [HttpGet]
        public async Task<IActionResult> CreateCalibration(int requestid)
        {
            Request request = await _context.Requests.Where(r => r.RequestID == requestid).Include(r => r.Product).FirstOrDefaultAsync();
            List<Calibration> calibrations = await _context.Calibrations.Where(c => c.RequestID == requestid).ToListAsync();
            List<ExternalCalibration> externalCalibrations = await _context.ExternalCalibrations
                .Where(c => c.RequestID == requestid)
                .Where(c => c.Date > DateTime.Now).ToListAsync();
            List<InternalCalibration> internalCalibrations = await _context.InternalCalibrations
                .Where(c => c.RequestID == requestid)
                .Where(c => c.Date > DateTime.Now).ToListAsync();
            List<Repair> repairs = await _context.Repairs
                .Where(c => c.RequestID == requestid)
                .Where(c => c.Date > DateTime.Now).ToListAsync();
            //delete temp files that may not have been deleted
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
            string requestFolder = Path.Combine(uploadFolder, "0");
           
            Directory.CreateDirectory(requestFolder);
            CreateCalibrationViewModel createCalibrationViewModel = new CreateCalibrationViewModel
            {
                ProductDescription = request.Product.ProductName,
                PastCalibrations = _context.Calibrations.Where(c => c.RequestID == requestid).Where(c => c.Date < DateTime.Now).Include(c => c.CalibrationType).ToList(),
                Repairs = repairs,
                ExternalCalibrations = externalCalibrations,
                InternalCalibration = internalCalibrations,
                RequestID = requestid
            };

            //may be able to do this together - combining the path for the orders folders
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.Requests.ToString());
            string uploadFolder2 = Path.Combine(uploadFolder1, requestid.ToString());
            string uploadFolderWarranties = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.Warranty.ToString());
            string uploadFolderManuals = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.Manual.ToString());
            string uploadFolderPictures = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.Pictures.ToString());
            string uploadFolderMore = Path.Combine(uploadFolder2, AppUtility.FolderNamesEnum.More.ToString());
            //the partial file name that we will search for (1- because we want the first one)
            //creating the directory from the path made earlier

            if (Directory.Exists(uploadFolderManuals))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderManuals);
                //searching for the partial file name in the directory
                FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                createCalibrationViewModel.ManualFileStrings = new List<String>();
                foreach (var orderfile in orderfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                    createCalibrationViewModel.ManualFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderWarranties))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderWarranties);
                FileInfo[] invoicefilesfound = DirectoryToSearch.GetFiles("*.*");
                createCalibrationViewModel.WarrantyFileStrings = new List<string>();
                foreach (var invoicefile in invoicefilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(invoicefile.FullName, 4);
                    createCalibrationViewModel.WarrantyFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderPictures))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderPictures);
                FileInfo[] shipmentfilesfound = DirectoryToSearch.GetFiles("*.*");
                createCalibrationViewModel.PicturesFileStrings = new List<string>();
                foreach (var shipmentfile in shipmentfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(shipmentfile.FullName, 4);
                    createCalibrationViewModel.PicturesFileStrings.Add(newFileString);
                }
            }
            if (Directory.Exists(uploadFolderMore))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolderMore);
                FileInfo[] quotefilesfound = DirectoryToSearch.GetFiles("*.*");
                createCalibrationViewModel.MoreFileStrings = new List<string>();
                foreach (var quotefile in quotefilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(quotefile.FullName, 4);
                    createCalibrationViewModel.MoreFileStrings.Add(newFileString);
                }
            }
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Calibrate;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementEquipment;
            if(AppUtility.IsAjaxRequest(this.Request))
            {
                return PartialView(createCalibrationViewModel);
            }
            return View(createCalibrationViewModel);
        }

        [Authorize(Roles = "LabManagement")]
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
                RepairIndex = RepairIndex,
                IsNew = true
            };

            return PartialView(repairsViewModel);
        }


        [Authorize(Roles = "LabManagement")]
        [HttpGet]
        public async Task<IActionResult> _ExternalCalibration(int ECIndex)
        {
            _ExternalCalibrationViewModel externalCalibrationViewModel = new _ExternalCalibrationViewModel()
            {
                ExternalCalibration = new ExternalCalibration()
                {
                    Date = DateTime.Now
                },
                ExternalCalibrationIndex = ECIndex,
                IsNew = true
            };

            return PartialView(externalCalibrationViewModel);
        }

        [Authorize(Roles = "LabManagement")]
        [HttpGet]
        public async Task<IActionResult> _InternalCalibration(int ICIndex)
        {
            _InternalCalibrationViewModel internalCalibrationViewModel = new _InternalCalibrationViewModel()
            {
                InternalCalibration = new InternalCalibration()
                {
                    Date = DateTime.Now
                },
                InternalCalibrationIndex = ICIndex,
                IsNew = true
            };

            return PartialView(internalCalibrationViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> SaveRepairs(CreateCalibrationViewModel vm)
        {
            foreach (Repair repair in vm.Repairs)
            {
                if (repair.CalibrationID > 0) //an old repair
                {
                    Repair updatedRepair = _context.Repairs.Where(c => c.CalibrationID == repair.CalibrationID).FirstOrDefault();
                    updatedRepair.CalibrationName = repair.CalibrationName;
                    updatedRepair.Date = repair.Date;
                    updatedRepair.IsRepeat = repair.IsRepeat;
                    updatedRepair.IsDeleted = repair.IsDeleted;
                    if (repair.IsRepeat)
                    {
                        updatedRepair.Months = repair.Months;
                        updatedRepair.Days = repair.Days;
                    }
                    else
                    {
                        updatedRepair.Months = 0;
                        updatedRepair.Days = 0;
                    }
                    updatedRepair.Description = repair.Description;
                    _context.Update(updatedRepair);
                    await _context.SaveChangesAsync();
                }
                else if(!repair.IsDeleted) // a new repair that wasn't x'd out
                {
                    repair.RequestID = vm.RequestID;
                    repair.CalibrationTypeID = 2;
                    _context.Add(repair);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> SaveExternalCalibrations(CreateCalibrationViewModel vm)
        {
            foreach (ExternalCalibration externalCalibration in vm.ExternalCalibrations)
            {
                if (externalCalibration.CalibrationID > 0) //an old repair
                {
                    ExternalCalibration updatedEC = _context.ExternalCalibrations.Where(c => c.CalibrationID == externalCalibration.CalibrationID).FirstOrDefault();
                    updatedEC.CalibrationName = externalCalibration.CalibrationName;
                    updatedEC.Date = externalCalibration.Date;
                    updatedEC.IsRepeat = externalCalibration.IsRepeat;
                    updatedEC.IsDeleted = externalCalibration.IsDeleted;
                    if (externalCalibration.IsRepeat)
                    {
                        updatedEC.Months = externalCalibration.Months;
                        updatedEC.Days = externalCalibration.Days;
                    }
                    else
                    {
                        externalCalibration.Months = 0;
                        externalCalibration.Days = 0;
                    }
                    updatedEC.Description = externalCalibration.Description;
                    _context.Update(updatedEC);
                    await _context.SaveChangesAsync();
                }
                else if (!externalCalibration.IsDeleted) // a new repair that wasn't x'd out
                {
                    externalCalibration.RequestID = vm.RequestID;
                    externalCalibration.CalibrationTypeID = 1;
                    _context.Add(externalCalibration);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> SaveInternalCalibrations(CreateCalibrationViewModel vm)
        {
            foreach (InternalCalibration internalCalibration in vm.InternalCalibration)
            {
                if (internalCalibration.CalibrationID > 0) //an old calibration
                {
                    var updatedInternalCalibration = _context.InternalCalibrations.Where(c => c.CalibrationID == internalCalibration.CalibrationID).FirstOrDefault();
                    updatedInternalCalibration.CalibrationName = internalCalibration.CalibrationName;
                    updatedInternalCalibration.Date = internalCalibration.Date;
                    updatedInternalCalibration.IsRepeat = internalCalibration.IsRepeat;
                    updatedInternalCalibration.IsDeleted = internalCalibration.IsDeleted;
                    if (internalCalibration.IsRepeat)
                    {
                        updatedInternalCalibration.Months = internalCalibration.Months;
                        updatedInternalCalibration.Days = internalCalibration.Days;
                    }
                    else
                    {
                        updatedInternalCalibration.Months = 0;
                        updatedInternalCalibration.Days = 0;
                    }
                    updatedInternalCalibration.Description = internalCalibration.Description;
                    _context.Update(updatedInternalCalibration);
                    await _context.SaveChangesAsync();
                }
                else if (!internalCalibration.IsDeleted) // a new calibration that wasn't x'd out
                {
                    internalCalibration.RequestID = vm.RequestID;
                    internalCalibration.CalibrationTypeID = 3;
                    _context.Add(internalCalibration);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }


        //writing this in a function because it's also called from the RenderPartialAsync in the view
        [Authorize(Roles = "LabManagement")]
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

        [HttpPost]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> _ExternalCalibration(_ExternalCalibrationViewModel externalCalibrationViewModel)
        {
            return RedirectToAction("Index");
        }

        //writing this in a function because it's also called from the RenderPartialAsync in the view
        [Authorize(Roles = "LabManagement")]
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

        [HttpPost]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> _InternalCalibration(_InternalCalibrationViewModel internalCalibrationViewModel)
        {
            return RedirectToAction("Index");
        }

        //writing this in a function because it's also called from the RenderPartialAsync in the view
        [Authorize(Roles = "LabManagement")]
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
