using System;
using System.Collections.Generic;
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
            return View(await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == categoryType).Count() > 0).ToListAsync());
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


            return View(await _context.Vendors.ToListAsync());

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

            return PartialView(await _context.Vendors.ToListAsync());

        }


        // GET: Vendors/Search
        [HttpGet]
        [Authorize(Roles = "Accounting, LabManagement")]
        public IActionResult Search(AppUtility.MenuItems SectionType)
        {

            VendorSearchViewModel vendorSearchViewModel = new VendorSearchViewModel
            {
                //ParentCategories = _context.ParentCategories.ToList(),
                CategoryTypes = _context.CategoryTypes.ToList(),
                Vendors = _context.Vendors.ToList(),
                Countries = _context.Countries.ToList(),
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
            IQueryable<Vendor> filteredVendors = _context.Vendors.Include(v => v.VendorCategoryTypes).AsQueryable();
            List<int> orderedVendorCategoryTypes = null;
            if (vendorSearchViewModel.VendorCategoryTypes != null)
            {
                orderedVendorCategoryTypes = vendorSearchViewModel.VendorCategoryTypes.OrderBy(e => e).ToList();
            }
            var listfilteredVendors = filteredVendors
                .Where(fv => (String.IsNullOrEmpty(vendorSearchViewModel.VendorEnName) || fv.VendorEnName.ToLower().Contains(vendorSearchViewModel.VendorEnName.ToLower()))
                 &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorHeName) || fv.VendorHeName.ToLower().Contains(vendorSearchViewModel.VendorHeName.ToLower()))
             &&
             (String.IsNullOrEmpty(vendorSearchViewModel.VendorBuisnessID) || fv.VendorBuisnessID.ToLower().Contains(vendorSearchViewModel.VendorBuisnessID.ToLower()))
             &&
             (String.IsNullOrEmpty(vendorSearchViewModel.CountryID.ToString()) || fv.CountryID.ToString().ToLower().Equals(vendorSearchViewModel.CountryID.ToLower()))
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
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorRoutingNum) || fv.VendorRoutingNum.ToLower().Contains(vendorSearchViewModel.VendorRoutingNum.ToLower()))).ToList();

            //listfilteredVendors = listfilteredVendors.Where(fv => (orderedVendorCategoryTypes == null || orderedVendorCategoryTypes.SequenceEqual(fv.VendorCategoryTypes.OrderBy( ct => ct.CategoryTypeID).Select(ct => ct.CategoryTypeID)))).ToList(); //if choose lab, will not show vendors that have both lab and operations
            listfilteredVendors = listfilteredVendors.Where(fv => (orderedVendorCategoryTypes == null || orderedVendorCategoryTypes.All(fv.VendorCategoryTypes.Select(ct => ct.CategoryTypeID).Contains))).ToList(); //if choose lab, will include vendors that have both lab and operations

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



        //string? enName, string? heName, string? bizID, string? contactPerson, string? contactEmail,
        //string? orderEmail, string? phoneNum1, string? phoneNum2, string? faxNum, string? city, string? street, string? zip,
        //string? website, string? bank, string? bankBranch, string? bankAccount, string? swift, string? BIC, string? goldAccount string? routingNum




        // GET: Vendors/Create
        [Authorize(Roles = "LabManagement, Accounting")]
        public IActionResult Create(AppUtility.MenuItems SectionType)
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
            CreateSupplierViewModel createSupplierViewModel = new CreateSupplierViewModel();
            createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
            List<VendorContactWithDeleteViewModel> vendorContacts = new List<VendorContactWithDeleteViewModel>();
            List<VendorComment> vendorComments = new List<VendorComment>();
            //only allowed to have 10 contacts
            //have to hard coded becasuse did not know how to render dynamic partial views
            createSupplierViewModel.Countries = new List<SelectListItem>();
            foreach (var country in _context.Countries)
            {
                createSupplierViewModel.Countries.Add(new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() });
            }
            createSupplierViewModel.VendorContacts = vendorContacts;
            createSupplierViewModel.VendorComments = vendorComments;
            createSupplierViewModel.SectionType = SectionType; //TODO: take this out when all the views are combined
            createSupplierViewModel.CategoryTypes = _context.CategoryTypes.ToList();

            return View(createSupplierViewModel);
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var ms in ModelState.ToArray())
                    {
                        if (ms.Key.StartsWith("VendorContact"))
                        {
                            ModelState.Remove(ms.Key);
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        _context.Add(createSupplierViewModel.Vendor);
                        _context.SaveChanges();
                        foreach (var type in createSupplierViewModel.VendorCategoryTypes)
                        {
                            _context.Add(new VendorCategoryType { VendorID = createSupplierViewModel.Vendor.VendorID, CategoryTypeID = type });
                        }
                        _context.SaveChanges();
                        //delete contacts that need to be deleted
                        foreach (var vc in createSupplierViewModel.VendorContacts.Where(vc => vc.Delete))
                        {
                            if (vc.VendorContact.VendorContactID != 0) //only will delete if it's a previously loaded ones
                            {
                                var dvc = _context.VendorContacts.Where(vc => vc.VendorContactID == vc.VendorContactID).FirstOrDefault();
                                _context.Remove(dvc);
                                await _context.SaveChangesAsync();
                            }
                        }


                        //update and add contacts
                        foreach (var vendorContact in createSupplierViewModel.VendorContacts.Where(vc => !vc.Delete))
                        {
                            vendorContact.VendorContact.VendorID = createSupplierViewModel.Vendor.VendorID;
                            _context.Update(vendorContact.VendorContact);

                        }
                        await _context.SaveChangesAsync();
                        var userid = _userManager.GetUserAsync(User).Result.Id;
                        if (createSupplierViewModel.VendorComments != null)
                        {
                            foreach (var vendorComment in createSupplierViewModel.VendorComments)
                            {
                                vendorComment.VendorID = createSupplierViewModel.Vendor.VendorID;
                                vendorComment.ApplicationUserID = userid;
                                vendorComment.CommentTimeStamp = DateTime.Now;
                                _context.Add(vendorComment);

                            }
                        }
                        await _context.SaveChangesAsync();
                        //throw new Exception();
                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(IndexForPayment), new { SectionType = createSupplierViewModel.SectionType });

                    }
                    else
                    {
                        throw new ModelStateInvalidException();
                    }


                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
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
                    createSupplierViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                    createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
                    createSupplierViewModel.CategoryTypes = _context.CategoryTypes.ToList();
                    return View("Create", createSupplierViewModel);
                }
            }

        }

        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> Edit(int? id, AppUtility.MenuItems SectionType)
        {
            return await editFunction(id, SectionType);
        }
        // GET: Vendors/Edit/5
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> EditPartial(int? id, AppUtility.MenuItems SectionType, int? Tab)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return await editFunction(id, SectionType, Tab);
        }

        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> _VendorHeader(int? id, AppUtility.MenuItems SectionType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var createSupplierViewModel = new CreateSupplierViewModel()
            {
                Vendor = await _context.Vendors.Include(v => v.VendorCategoryTypes).Where(v => v.VendorID == id).FirstOrDefaultAsync()
            };
            return PartialView(createSupplierViewModel);
        }
        public async Task<IActionResult> editFunction(int? id, AppUtility.MenuItems SectionType, int? Tab = 0)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateSupplierViewModel createSupplierViewModel = new CreateSupplierViewModel();
            createSupplierViewModel.Countries = new List<SelectListItem>();
            foreach (var country in _context.Countries)
            {
                createSupplierViewModel.Countries.Add(new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() });
            }
            createSupplierViewModel.Vendor = await _context.Vendors.Include(v => v.VendorCategoryTypes).Where(v => v.VendorID == id).FirstOrDefaultAsync();
            createSupplierViewModel.SectionType = SectionType;
            createSupplierViewModel.CategoryTypes = _context.CategoryTypes.ToList();
            createSupplierViewModel.VendorCategoryTypes = createSupplierViewModel.Vendor.VendorCategoryTypes.Select(vc => vc.CategoryTypeID).ToList();
            createSupplierViewModel.Tab = Tab ?? 0;
            //var count = createSupplierViewModel.Vendor.VendorCategoryTypes.Count();
            //if (count == 2)
            //{

            //}
            // createSupplierViewModel.CategoryTypeID = createSupplierViewModel.Vendor.VendorCategoryTypes
            if (createSupplierViewModel.Vendor == null)
            {
                return NotFound();
            }
            createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
            createSupplierViewModel.VendorContacts = _context.VendorContacts.Where(c => c.VendorID == id).ToList()
                .Select(vc => new VendorContactWithDeleteViewModel()
                {
                    VendorContact = vc,
                    Delete = false
                }).ToList();
            createSupplierViewModel.VendorComments = await _context.VendorComments.Where(c => c.VendorID == id).Include(r => r.ApplicationUser).ToListAsync();
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.AllSuppliers;
            //tempdata page type for active tab link
            if (SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSuppliers;
            }
            else if (SectionType == AppUtility.MenuItems.Accounting)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.LabManagementSuppliers;
            }
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //ModelState.Remove()
                    foreach (var ms in ModelState.ToArray())
                    {
                        if (ms.Key.StartsWith("VendorContact"))
                        {
                            ModelState.Remove(ms.Key);
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        _context.Update(createSupplierViewModel.Vendor);
                        _context.SaveChanges();
                        var vendor = await _context.Vendors.Where(v => v.VendorID == createSupplierViewModel.Vendor.VendorID).Include(v => v.VendorCategoryTypes).FirstOrDefaultAsync();
                        if (vendor.VendorCategoryTypes.Count() > 0)
                        {
                            foreach (var type in createSupplierViewModel.Vendor.VendorCategoryTypes)
                            {
                                _context.Remove(type);
                            }
                        }

                        foreach (var type in createSupplierViewModel.VendorCategoryTypes)
                        {
                            _context.Add(new VendorCategoryType { VendorID = createSupplierViewModel.Vendor.VendorID, CategoryTypeID = type });
                        }
                        //delete contacts that need to be deleted
                        foreach (var vc in createSupplierViewModel.VendorContacts.Where(vc => vc.Delete))
                        {
                            if (vc.VendorContact.VendorContactID != 0) //only will delete if it's a previously loaded ones
                            {
                                var dvc = _context.VendorContacts.Where(vc => vc.VendorContactID == vc.VendorContactID).FirstOrDefault();
                                _context.Remove(dvc);
                                await _context.SaveChangesAsync();
                            }
                        }


                        //update and add contacts
                        foreach (var vendorContact in createSupplierViewModel.VendorContacts.Where(vc => !vc.Delete))
                        {
                            vendorContact.VendorContact.VendorID = createSupplierViewModel.Vendor.VendorID;
                            _context.Update(vendorContact.VendorContact);

                        }
                        if (createSupplierViewModel.VendorComments != null)
                        {
                            foreach (var vendorComment in createSupplierViewModel.VendorComments)
                            {
                                if (!String.IsNullOrEmpty(vendorComment.CommentText))
                                {
                                    vendorComment.VendorID = createSupplierViewModel.Vendor.VendorID;
                                    if (vendorComment.VendorCommentID == 0)
                                    {
                                        vendorComment.CommentTimeStamp = DateTime.Now;
                                    }
                                    _context.Update(vendorComment);
                                }

                            }
                        }
                        _context.SaveChanges();
                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(IndexForPayment), new { SectionType = createSupplierViewModel.SectionType });
                    }
                    else
                    {
                        throw new ModelStateInvalidException();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    createSupplierViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                    createSupplierViewModel.CategoryTypes = _context.CategoryTypes.ToList();
                    createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
                    createSupplierViewModel.VendorComments = await _context.VendorComments.Where(c => c.VendorID == createSupplierViewModel.Vendor.VendorID).ToListAsync();
                    Response.StatusCode = 550;
                    return PartialView("Edit", createSupplierViewModel);

                }
            }
        }

        // GET: Vendors/Delete/5
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors
                .FirstOrDefaultAsync(m => m.VendorID == id);
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
            var comments = _context.VendorComments.Where(x => x.VendorID == id);
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
            return RedirectToAction(nameof(IndexForPayment));
        }

        [Authorize(Roles = "Accounting")]
        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.VendorID == id);
        }

        //Get the Json of the vendor business id from the vendor id so JS (site.js) can auto load the form-control with the newly requested vendor
        [HttpGet]
        public JsonResult GetVendorBusinessID(int VendorID)
        {
            Vendor vendor = _context.Vendors.SingleOrDefault(v => v.VendorID == VendorID);
            return Json(vendor);
        }
        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> CommentInfoPartialView(String type, int index)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            VendorComment comment = new VendorComment();
            comment.CommentType = type;
            comment.ApplicationUser = _userManager.GetUserAsync(User).Result;
            comment.ApplicationUserID = comment.ApplicationUser.Id;
            AddCommentViewModel addCommentViewModel = new AddCommentViewModel { VendorComment = comment, Index = index };
            return PartialView(addCommentViewModel);
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
            if (CompanyID != null && CountryID != null && (VendorID == null && _context.Vendors.Where(v => v.VendorBuisnessID.Equals(CompanyID) && v.CountryID.Equals(CountryID)).Any()))
            {
                return false;
            }
            //validation for the edit
            if (VendorID != null && _context.Vendors.Where(v => v.VendorBuisnessID.Equals(CompanyID) && v.CountryID.Equals(CountryID) && v.VendorID != VendorID).Any())
            {
                return false;
            }
            return true;
        }

        public string GetVendorCountryCurrencyID(int VendorID)
        {
            return _context.Vendors.Include(v => v.Country).Where(v => v.VendorID == VendorID).Select(v => v.Country.CurrencyID).FirstOrDefault().ToString();
        }
    }

}
