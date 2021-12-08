using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Extensions;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.Exceptions;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Data.Migrations;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class VendorsController : SharedController
    {

        public VendorsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)

        {

        }
        // GET: Vendors
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> Index(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            var categoryType = 1;
            if (SectionType == AppUtility.MenuItems.Operations)
            {
                categoryType = 2;
            }
            if (categoryType == 1)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            }
            else
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
            }
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Vendors;
            TempData["CategoryType"] = categoryType;

            var vendors = _vendor.ReadByCategoryTypeID(categoryType);
            return View(vendors);
        }

        // GET: Vendors
        [Authorize(Roles = "Accounting,  LabManagement")]
        public async Task<IActionResult> IndexForPayment(AppUtility.MenuItems SectionType = AppUtility.MenuItems.LabManagement)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.AllSuppliers;
            if (SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSuppliers;
            }
            else
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingSuppliers;
            }


            return View(_vendor.Read());

        }
        // GET: Vendors
        [Authorize(Roles = "Accounting,  LabManagement")]
        public async Task<IActionResult> _IndexForPayment(AppUtility.MenuItems SectionType = AppUtility.MenuItems.LabManagement)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.AllSuppliers;
            if (SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSuppliers;
            }
            else
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingSuppliers;
            }

            return PartialView(_vendor.Read());

        }


        // GET: Vendors/Search
        [HttpGet]
        [Authorize(Roles = "Accounting, LabManagement")]
        public IActionResult Search(AppUtility.MenuItems SectionType)
        {

            VendorSearchViewModel vendorSearchViewModel = new VendorSearchViewModel
            {
                CategoryTypes = _categoryType.Read(),
                Vendors = _vendor.Read(),
                Countries = _country.Read(),
                SectionType = SectionType
            };

            //tempdata page type for active tab link
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Search;
            if (SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSuppliers;
            }
            else if (SectionType == AppUtility.MenuItems.Accounting)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingSuppliers;
            }
            //return View(vendorSearchViewModel);
            if (AppUtility.IsAjaxRequest(this.Request))
                return PartialView(vendorSearchViewModel);
            else
                return View(vendorSearchViewModel);

        }
        // Post: Vendors/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> Search(VendorSearchViewModel vendorSearchViewModel)

        {
            var listfilteredVendors = _vendor.Read(vendorSearchViewModel);

            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = vendorSearchViewModel.SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Search;
            if (vendorSearchViewModel.SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSuppliers;
            }
            else if (vendorSearchViewModel.SectionType == AppUtility.MenuItems.Accounting)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingSuppliers;
            }
            return View("IndexForPayment", listfilteredVendors);
        }


        [HttpGet]
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> SearchByVendorNameAndCompanyID(string vendorName, string companyID)

        {
            IQueryable<Vendor> filteredVendors = _context.Vendors.AsQueryable();
            var listfilteredVendors = await filteredVendors
            .Where(fv => (String.IsNullOrEmpty(vendorName) || fv.VendorEnName.ToLower().Contains(vendorName.ToLower()))
                &&
             (String.IsNullOrEmpty(companyID) || fv.VendorBuisnessID.ToLower().Contains(companyID.ToLower()))).ToListAsync();
            return PartialView("_IndexForPayment", listfilteredVendors);
        }


        // GET: Vendors/Create
        [Authorize(Roles = "LabManagement, Accounting")]
        public async Task<IActionResult> Create(AppUtility.MenuItems SectionType)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            //tempdata page type for active tab link
            if (SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSuppliers;
            }
            else if (SectionType == AppUtility.MenuItems.Accounting)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingSuppliers;
            }
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.NewSupplier;

            CreateSupplierViewModel createSupplierViewModel = await GetCreateSupplierViewModel(SectionType);
            //only allowed to have 10 contacts

            createSupplierViewModel.ModalType = AppUtility.VendorModalType.Create;
            return View(createSupplierViewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> _CommentInfoPartialView(int typeID, int index)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return await base._CommentInfoPartialView(typeID, index);
        }
        //[HttpGet]
        //public IActionResult AddContact()
        //{
        //    //tempdata page type for active tab link
        //    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PaymentPageTypeEnum.SuppliersAC;
        //    VendorContact vendorContact = new VendorContact();
        //    return PartialView(vendorContact);
        //}

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> Create(CreateSupplierViewModel createSupplierViewModel)
        {
            var userid = _userManager.GetUserAsync(User).Result.Id;
            StringWithBool vendorCreated = await _vendor.CreateAsync(createSupplierViewModel, ModelState, userid);
            if (!vendorCreated.Bool)
            {
                return RedirectToAction(nameof(IndexForPayment), new { SectionType = createSupplierViewModel.SectionType });
            }
            else
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = createSupplierViewModel.SectionType;
                //tempdata page type for active tab link
                if (createSupplierViewModel.SectionType == AppUtility.MenuItems.LabManagement)
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSuppliers;

                }
                else if (createSupplierViewModel.SectionType == AppUtility.MenuItems.Accounting)
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.AccountingSuppliers;
                }
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.NewSupplier;
                createSupplierViewModel.ErrorMessage += vendorCreated.String;
                createSupplierViewModel = await GetCreateSupplierViewModel(createSupplierViewModel.SectionType, 0, createSupplierViewModel.Tab);
                createSupplierViewModel.ModalType = AppUtility.VendorModalType.Create;

                return View(createSupplierViewModel);

            }
        }

        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> Edit(int id, AppUtility.MenuItems SectionType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            CreateSupplierViewModel createSupplierViewModel = await GetCreateSupplierViewModel(SectionType, id);
            createSupplierViewModel.ModalType = AppUtility.VendorModalType.Edit;
            return PartialView(createSupplierViewModel);
        }
        // GET: Vendors/Edit/5
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> VendorData(int id, AppUtility.MenuItems SectionType, int Tab)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            CreateSupplierViewModel createSupplierViewModel = await GetCreateSupplierViewModel(SectionType, id, Tab);
            createSupplierViewModel.ModalType = AppUtility.VendorModalType.Edit;
            return PartialView(createSupplierViewModel);
        }

        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> _VendorHeader(int id, AppUtility.MenuItems SectionType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var createSupplierViewModel = new CreateSupplierViewModel()
            {
                Vendor = await _vendor.ReadByVendorIDAsync(id)
            };
            return PartialView(createSupplierViewModel);
        }
        
        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> Edit(CreateSupplierViewModel createSupplierViewModel)
        {
            //PROC START
            var vendorUpdated = await _vendor.Update(createSupplierViewModel, ModelState);
            if (vendorUpdated.Bool)
            {
                return RedirectToAction(nameof(IndexForPayment), new { SectionType = createSupplierViewModel.SectionType });
            }
            else
            {
                createSupplierViewModel = await GetCreateSupplierViewModel(createSupplierViewModel.SectionType, createSupplierViewModel.Vendor.VendorID, createSupplierViewModel.Tab);
                createSupplierViewModel.ErrorMessage += vendorUpdated.String;
                createSupplierViewModel.ModalType = AppUtility.VendorModalType.Edit;

                Response.StatusCode = 550;
                return PartialView("Edit", createSupplierViewModel);
            }
            //PROC END
        }

        // GET: Vendors/Delete/5
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = _vendor.ReadByVendorIDAsync(Convert.ToInt32(id));
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accounting, LabManagement")]
        public IActionResult DeleteConfirmed(int id)
        {
            //PROC START
            _vendor.Delete(id);
            //var vendor = _context.Vendors.Find(id);
            //var contacts = _context.VendorContacts.Where(x => x.VendorID == id);
            //using (var transaction = new TransactionScope())
            //{
            //    foreach (var contact in contacts)
            //    {
            //        //don't actually remove set a boolean
            //        _context.Remove(contact);
            //        _context.SaveChanges();
            //    }
            //    transaction.Complete();
            //}
            //var comments = _context.VendorComments.Where(x => x.VendorID == id);
            //using (var transaction = new TransactionScope())
            //{
            //    foreach (var comment in comments)
            //    {
            //        _context.Remove(comment);
            //        _context.SaveChanges();
            //    }
            //    transaction.Complete();
            //}
            //_context.Vendors.Remove(vendor);
            //_context.SaveChanges();
            //PROC END
            //VENDO REDO START
            var vendor = _context.Vendors.Find(id);
            var contacts = _context.VendorContacts.Where(x => x.VendorID == id);
            using (var transaction = new TransactionScope())
            {
                foreach (var contact in contacts)
                {
                    //don't actually remove set a boolean
                    _context.Remove(contact);
                    _context.SaveChanges();
                }
                transaction.Complete();
            }
            var comments = _context.VendorComments.Where(x => x.ObjectID == id);
            using (var transaction = new TransactionScope())
            {
                foreach (var comment in comments)
                {
                    _context.Remove(comment);
                    _context.SaveChanges();
                }
                transaction.Complete();
            }
            _context.Vendors.Remove(vendor);
            _context.SaveChanges();
            //VENDOR REDO END
            return RedirectToAction(nameof(IndexForPayment));
        }

        [Authorize(Roles = "Accounting")]
        private async Task<bool> VendorExists(int id)
        {
            return await _vendor.ReadByVendorIDAsync(id) != null ? true : false;
        }

        //Get the Json of the vendor business id from the vendor id so JS (site.js) can auto load the form-control with the newly requested vendor
        [HttpGet]
        public async  Task<JsonResult> GetVendorBusinessID(int VendorID)
        {
            Vendor vendor = await  _vendor.ReadByVendorIDAsync(VendorID);
            return Json(vendor);
        }
      
        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> ContactInfoPartial(int index)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            VendorContact contact = new VendorContact();
            AddContactViewModel addContactViewModel = new AddContactViewModel { VendorContact = contact, Index = index };
            return PartialView(addContactViewModel);
        }

        [HttpPost]
        public bool CheckUniqueCompanyIDAndCountry(string CompanyID, int CountryID, int? VendorID = null)
        {
            //validation for the create
            if (CompanyID != null && CountryID != null && VendorID == null && 
                _vendor.ReadByVendorBusinessIDCountryID(CompanyID, CountryID) != null)
            {
                return false;
            }
            //validation for the edit
            if (VendorID != null && 
                _vendor.ReadByVendorBusinessIDCountryIDVendorID(CompanyID, CountryID, Convert.ToInt32(VendorID)) != null)
            {
                return false;
            }
            return true;
        }

        public async Task<string> GetVendorCountryCurrencyIDAsync(int VendorID)
        {
            var vendor = await _vendor.ReadByVendorIDAsync(VendorID, new List<System.Linq.Expressions.Expression<Func<Vendor, object>>> { v => v.Country });
            return vendor.Country.CurrencyID.ToString();
        }
    }

}
