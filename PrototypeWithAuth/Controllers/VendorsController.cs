using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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

            var vendors = _vendorsProc.Read(new List<Expression<Func<Vendor, bool>>> {
                 v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == categoryType).Count() > 0
            });
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


            return View(_vendorsProc.Read().OrderBy(v=>v.VendorEnName));

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

            return PartialView(_vendorsProc.Read().OrderBy(v => v.VendorEnName));

        }


        // GET: Vendors/Search
        [HttpGet]
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> Search(AppUtility.MenuItems SectionType)
        {

            VendorSearchViewModel vendorSearchViewModel = new VendorSearchViewModel
            {
                CategoryTypes = _categoryType.Read(),
                Vendors = _vendorsProc.Read(),
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
            List<Expression<Func<Vendor, bool>>> wheres = new List<Expression<Func<Vendor, bool>>>();
            List<int> orderedVendorCategoryTypes = new List<int>();
            if (vendorSearchViewModel.VendorCategoryTypes != null)
            {
                orderedVendorCategoryTypes = vendorSearchViewModel.VendorCategoryTypes.OrderBy(e => e).ToList();
            }
            wheres.Add(fv => (String.IsNullOrEmpty(vendorSearchViewModel.VendorEnName) || fv.VendorEnName.ToLower().Contains(vendorSearchViewModel.VendorEnName.ToLower()))
                &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorHeName) || fv.VendorHeName.ToLower().Contains(vendorSearchViewModel.VendorHeName.ToLower()))
             &&
             (String.IsNullOrEmpty(vendorSearchViewModel.VendorBuisnessID) || fv.VendorBuisnessID.ToLower().Contains(vendorSearchViewModel.VendorBuisnessID.ToLower()))
             &&
             (vendorSearchViewModel.CountryID == null || fv.CountryID.ToString().ToLower().Equals(vendorSearchViewModel.CountryID.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorCity) || fv.VendorCity.ToLower().Contains(vendorSearchViewModel.VendorCity.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorStreet) || fv.VendorStreet.ToLower().Contains(vendorSearchViewModel.VendorStreet.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorZip) || fv.VendorZip.ToLower().Contains(vendorSearchViewModel.VendorZip.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorTelephone) || fv.VendorTelephone.Contains(vendorSearchViewModel.VendorTelephone))
              &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorCellPhone) || fv.VendorCellPhone.Contains(vendorSearchViewModel.VendorCellPhone))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorFax) || fv.VendorFax.Contains(vendorSearchViewModel.VendorFax))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.OrdersEmail) || fv.OrdersEmail.ToLower().Contains(vendorSearchViewModel.OrdersEmail.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.InfoEmail) || fv.InfoEmail.ToLower().Contains(vendorSearchViewModel.InfoEmail.ToLower()))
            &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorWebsite) || fv.VendorWebsite.ToLower().Contains(vendorSearchViewModel.VendorWebsite.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorBank) || fv.VendorBank.ToLower().Contains(vendorSearchViewModel.VendorBank.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorBankBranch) || fv.VendorBankBranch.ToLower().Contains(vendorSearchViewModel.VendorBankBranch.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorAccountNum) || fv.VendorAccountNum.ToLower().Contains(vendorSearchViewModel.VendorAccountNum.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorSwift) || fv.VendorSwift.ToLower().Contains(vendorSearchViewModel.VendorSwift.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorBIC) || fv.VendorBIC.ToLower().Contains(vendorSearchViewModel.VendorBIC.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorGoldAccount) || fv.VendorGoldAccount.ToLower().Contains(vendorSearchViewModel.VendorGoldAccount.ToLower()))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorRoutingNum) || fv.VendorRoutingNum.ToLower().Contains(vendorSearchViewModel.VendorRoutingNum.ToLower())));

            foreach (var ovct in orderedVendorCategoryTypes)
            {
                wheres.Add(fv => fv.VendorCategoryTypes.Select(ct => ct.CategoryTypeID).Contains(ovct));
            }
            
            var listfilteredVendors = _vendorsProc.Read(wheres, new List<ComplexIncludes<Vendor, ModelBase>> { new ComplexIncludes<Vendor, ModelBase> { Include = v=>v.VendorCategoryTypes} }).AsEnumerable();

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
            List<Expression<Func<Vendor, bool>>> wheres = new List<Expression<Func<Vendor, bool>>>();
            IQueryable<Vendor> filteredVendors = _vendorsProc.Read();

            wheres.Add(fv => (String.IsNullOrEmpty(vendorName) || fv.VendorEnName.ToLower().Contains(vendorName.ToLower()))
                &&
             (String.IsNullOrEmpty(companyID) || fv.VendorBuisnessID.ToLower().Contains(companyID.ToLower())));
            var listfilteredVendors = _vendorsProc.Read(wheres, new List<ComplexIncludes<Vendor, ModelBase>> { new ComplexIncludes<Vendor, ModelBase> { Include = v => v.VendorCategoryTypes } });
            return PartialView("_IndexForPayment", listfilteredVendors.OrderBy(v=>v.VendorEnName));
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
            StringWithBool vendorCreated = await _vendorsProc.UpdateAsync(createSupplierViewModel, userid);
            if (vendorCreated.Bool)
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
                createSupplierViewModel = await GetCreateSupplierViewModel(createSupplierViewModel.SectionType, 0, createSupplierViewModel.Tab);
                createSupplierViewModel.ModalType = AppUtility.VendorModalType.Create;
                createSupplierViewModel.ErrorMessage += vendorCreated.String;

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
                Vendor = await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == id })
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
            var vendorUpdated = await _vendorsProc.UpdateAsync(createSupplierViewModel, _userManager.GetUserId(User));
            if (vendorUpdated.Bool)
            {
                ControllerContext.ModelState.Clear();
                createSupplierViewModel = await GetCreateSupplierViewModel(createSupplierViewModel.SectionType, createSupplierViewModel.Vendor.VendorID, createSupplierViewModel.Tab);
                createSupplierViewModel.ModalType = AppUtility.VendorModalType.Edit;
                return PartialView("VendorData", createSupplierViewModel);
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
      
        [Authorize(Roles = "Accounting")]
        private async Task<bool> VendorExists(int id)
        {
            return await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == id }) != null ? true : false;
        }

        [HttpGet]
        public async  Task<JsonResult> GetVendorBusinessID(int VendorID)
        {
            Vendor vendor = await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == VendorID });
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
        public async Task<bool> CheckUniqueCompanyIDAndCountry(string CompanyID, int? CountryID, int? VendorID = null)
        {
            Vendor vendor = null; 
            if (CompanyID != null && CountryID != null && VendorID == null)
            {
                vendor =await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorBuisnessID.Equals(CompanyID) && v.CountryID == CountryID });
                if(vendor !=null)
                {
                    return false; 
                }                
            }
            if (VendorID != null)
            {
                vendor = await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorBuisnessID.Equals(CompanyID) && v.CountryID == CountryID && v.VendorID !=VendorID });
                if (vendor !=null)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<string> GetVendorCountryCurrencyIDAsync(int VendorID)
        {
            var vendor = await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == VendorID },
                new List<ComplexIncludes<Vendor, ModelBase>> {
                    new ComplexIncludes<Vendor, ModelBase>{ Include = v => v.Country }
                }
            );
            return vendor.Country.CurrencyID.ToString();
        }
    }

}
