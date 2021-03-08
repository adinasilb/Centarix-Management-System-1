using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Extensions;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public VendorsController(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }






        // GET: Vendors
        [Authorize(Roles = "Requests")]
        public async Task<IActionResult> Index(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest, int categoryType = 1, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
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
                ParentCategories = _context.ParentCategories.ToList(),
                Vendors = _context.Vendors.ToList(),
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
            IQueryable<Vendor> filteredVendors = _context.Vendors.AsQueryable();
            filteredVendors = filteredVendors
                .Where(fv => (String.IsNullOrEmpty(vendorSearchViewModel.VendorEnName) || fv.VendorEnName.Contains(vendorSearchViewModel.VendorEnName))
                 &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorHeName) || fv.VendorHeName.Contains(vendorSearchViewModel.VendorHeName))
             &&
             (String.IsNullOrEmpty(vendorSearchViewModel.VendorBuisnessID) || fv.VendorBuisnessID.Contains(vendorSearchViewModel.VendorBuisnessID))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.InfoEmail) || fv.InfoEmail.Contains(vendorSearchViewModel.InfoEmail))
            &&
            (String.IsNullOrEmpty(vendorSearchViewModel.OrdersEmail) || fv.OrdersEmail.Contains(vendorSearchViewModel.OrdersEmail))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorTelephone) || fv.VendorTelephone.Contains(vendorSearchViewModel.VendorTelephone))
              &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorCellPhone) || fv.VendorCellPhone.Contains(vendorSearchViewModel.VendorCellPhone))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorFax) || fv.VendorFax.Contains(vendorSearchViewModel.VendorFax))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorCity) || fv.VendorCity.Contains(vendorSearchViewModel.VendorCity))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorStreet) || fv.VendorStreet.Contains(vendorSearchViewModel.VendorStreet))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorZip) || fv.VendorZip.Contains(vendorSearchViewModel.VendorZip))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorWebsite) || fv.VendorWebsite.Contains(vendorSearchViewModel.VendorWebsite))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorBank) || fv.VendorBank.Contains(vendorSearchViewModel.VendorBank))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorBankBranch) || fv.VendorBankBranch.Contains(vendorSearchViewModel.VendorBankBranch))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorAccountNum) || fv.VendorAccountNum.Contains(vendorSearchViewModel.VendorAccountNum))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorSwift) || fv.VendorSwift.Contains(vendorSearchViewModel.VendorSwift))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorBIC) || fv.VendorBIC.Contains(vendorSearchViewModel.VendorBIC))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorGoldAccount) || fv.VendorGoldAccount.Contains(vendorSearchViewModel.VendorGoldAccount)));


            if (vendorSearchViewModel.SelectedParentCategoryID > 0)
            {
                foreach (var v in filteredVendors)
                {

                    bool vendorContainsParentCategory = false;
                    foreach (var p in _context.Products.Include(p => p.ProductSubcategory))
                    {
                        //var psc = _context.ProductSubcategories.Where(psc => psc.ProductSubcategoryID == p.ProductSubcategoryID) //need to instatntaite subcategories before refrenceing to parent
                        if ((p.ProductSubcategory.ParentCategoryID == vendorSearchViewModel.SelectedParentCategoryID) && (p.VendorID == v.VendorID))
                        {
                            vendorContainsParentCategory = true;

                        }
                    }
                    if (!vendorContainsParentCategory)
                    {
                        filteredVendors = filteredVendors.Where(fv => fv.VendorID != v.VendorID);

                    }
                }
            }
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
            return View("IndexForPayment", filteredVendors.ToList());
        }



        //string? enName, string? heName, string? bizID, string? contactPerson, string? contactEmail,
        //string? orderEmail, string? phoneNum1, string? phoneNum2, string? faxNum, string? city, string? street, string? zip,
        //string? website, string? bank, string? bankBranch, string? bankAccount, string? swift, string? BIC, string? goldAccount




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
                    createSupplierViewModel.ErrorMessage += ex.Message;
                    createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
                    createSupplierViewModel.CategoryTypes = _context.CategoryTypes.ToList();
                    return View("Create", createSupplierViewModel);
                }
            }

        }

        public async Task<IActionResult> Edit(int? id, AppUtility.MenuItems SectionType)
        {
            return await editFunction(id, SectionType);
        }
        // GET: Vendors/Edit/5
        [Authorize(Roles = "Accounting, LabManagement")]
        public async Task<IActionResult> EditPartial(int? id, AppUtility.MenuItems SectionType, int? Tab)
        {
            return await editFunction(id, SectionType, Tab);
        }
        public async Task<IActionResult> editFunction(int? id, AppUtility.MenuItems SectionType, int? Tab = 0)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateSupplierViewModel createSupplierViewModel = new CreateSupplierViewModel();
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
            createSupplierViewModel.VendorContacts =  _context.VendorContacts.Where(c => c.VendorID == id).ToList()
                .Select(vc => new VendorContactWithDeleteViewModel()
                {
                    VendorContact = vc,
                    Delete = false
                }).ToList();
            createSupplierViewModel.VendorComments = await _context.VendorComments.Where(c => c.VendorID == id).ToListAsync();
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
                foreach(var vc in createSupplierViewModel.VendorContacts.Where(vc => vc.Delete))
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
                        vendorComment.VendorID = createSupplierViewModel.Vendor.VendorID;
                        vendorComment.ApplicationUserID = _userManager.GetUserId(User);
                        vendorComment.CommentTimeStamp = DateTime.Now;
                        _context.Update(vendorComment);

                    }
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(IndexForPayment), new { SectionType = createSupplierViewModel.SectionType });
            }


            return View(createSupplierViewModel);
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
            VendorComment comment = new VendorComment();
            comment.CommentType = type;
            AddCommentViewModel addCommentViewModel = new AddCommentViewModel { VendorComment = comment, Index = index};
            return PartialView(addCommentViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "LabManagement")]
        public async Task<IActionResult> ContactInfoPartial(int index)
        {
            VendorContact contact = new VendorContact();
            AddContactViewModel addContactViewModel = new AddContactViewModel { VendorContact = contact, Index = index };
            return PartialView(addContactViewModel);
        }
    }

}
