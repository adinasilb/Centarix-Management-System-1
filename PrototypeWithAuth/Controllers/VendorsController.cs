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
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Data.Migrations;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class VendorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public VendorsController(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }






        // GET: Vendors
        [Authorize(Roles = "Admin, OrdersAndInventory")]
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, int categoryType = 1)
        {
            if (categoryType == 1)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.OrdersAndInventory;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
                TempData["SidebarTitle"] = AppUtility.OrdersAndInventorySidebarEnum.Vendor;
            }
            else
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operation;
                if (PageType == AppUtility.RequestPageTypeEnum.Request || PageType.ToString() == AppUtility.OperationsPageTypeEnum.RequestOperations.ToString())
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.OperationsPageTypeEnum.RequestOperations.ToString();
                }
                else if (PageType == AppUtility.RequestPageTypeEnum.Inventory || PageType.ToString() == AppUtility.OperationsPageTypeEnum.InventoryOperations.ToString())
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.OperationsPageTypeEnum.InventoryOperations.ToString();
                }
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.OperationsSidebarEnum.Vendors;
            }
            TempData["CategoryType"] = categoryType;
            return View(await _context.Vendors.Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == categoryType).Count() > 0).ToListAsync());
        }

        // GET: Vendors
        [Authorize(Roles = "Admin, Accounting,  LabManagement")]
        public async Task<IActionResult> IndexForPayment(AppUtility.MenuItems SectionType)
        {
            TempData["SectionType"] = SectionType;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            if (SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.LabManagementSidebarEnum.AllSuppliers;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.LabManagementPageTypeEnum.Suppliers;
            }
            else
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PaymentPageTypeEnum.SuppliersAC;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.AccountingSidebarEnum.AllSuppliersAC;
            }
            TempData["Action"] = AppUtility.SuppliersEnum.All;
            return View(await _context.Vendors.ToListAsync());

        }

        [Authorize(Roles = "Admin, LabManagement")]
        public async Task<IActionResult> IndexForLabManage()
        {
            //TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PaymentPageTypeEnum.SuppliersAC;
            //TempData["Action"] = AppUtility.SuppliersEnum.All;
            //TempData["Action"] = AppUtility.LabManagementSidebarEnum.AllSuppliers;
            //return View(await _context.Vendors.ToListAsync());
            return RedirectToAction("IndexForPayment", new { SectionType = AppUtility.MenuItems.LabManagement });
        }

        // GET: Vendors/Details/5
        [Authorize(Roles = "Admin")] //don't know where this goes to so can't have a role
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VendorDetailsViewModel vendorDetailsViewModel = new VendorDetailsViewModel
            {
                ParentCategories = _context.ParentCategories.ToList(),
                Products = _context.Products.Where(p => p.VendorID == id).Include(p => p.ProductSubcategory).ThenInclude(p => p.ParentCategory),
                Vendors = _context.Vendors,
                Vendor = await _context.Vendors.FirstOrDefaultAsync(m => m.VendorID == id),
                // Requests = _context.Requests.ToList(),
                //check if we need this here
            };

            if (vendorDetailsViewModel.Vendor == null)
            {
                return NotFound();
            }

            return View(vendorDetailsViewModel);
        }
        // GET: Vendors/Details/5
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> DetailsForPayment(int? id, AppUtility.MenuItems SectionType) // should not have to repeat this code - but for now just doing things quick
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateSupplierViewModel createSupplierViewModel = new CreateSupplierViewModel();

            createSupplierViewModel.Vendor = await _context.Vendors.Include(v=>v.VendorCategoryTypes).Where(v=>v.VendorID == id).FirstOrDefaultAsync();
            createSupplierViewModel.SectionType = SectionType;
            createSupplierViewModel.CategoryTypes = _context.CategoryTypes.ToList();
            createSupplierViewModel.VendorCategoryTypes = createSupplierViewModel.Vendor.VendorCategoryTypes.Select(vc => vc.CategoryTypeID).ToList();
            if (createSupplierViewModel.Vendor == null)
            {
                return NotFound();
            }
            createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
            List<AddContactViewModel> vendorContacts = new List<AddContactViewModel>();
            List<AddCommentViewModel> vendorComments = new List<AddCommentViewModel>();
            _context.VendorContacts.Where(c => c.VendorID == id).ToList().ForEach(c => vendorContacts.Add(new AddContactViewModel { VendorContact = c, IsActive = true }));
            _context.VendorComments.Where(c => c.VendorID == id).ToList().ForEach(c => vendorComments.Add(new AddCommentViewModel { VendorComment = c, IsActive = true }));
            createSupplierViewModel.VendorContacts = vendorContacts;
            createSupplierViewModel.VendorComments = vendorComments;

            //tempdata page type for active tab link
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PaymentPageTypeEnum.SuppliersAC;           
            return View(createSupplierViewModel);
        }

     
        // GET: Vendors/Search
        [HttpGet]
        [Authorize(Roles = "Admin, Accounting, LabManagement")]
        public IActionResult Search(AppUtility.MenuItems SectionType)
        {
     
            VendorSearchViewModel vendorSearchViewModel = new VendorSearchViewModel
            {
                ParentCategories = _context.ParentCategories.ToList(),
                Vendors = _context.Vendors.ToList(),
                SectionType = SectionType
            };

            //tempdata page type for active tab link
            if (SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.LabManagement;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.LabManagementPageTypeEnum.Suppliers;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.LabManagementSidebarEnum.SearchSupplier;
            }
            else
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PaymentPageTypeEnum.SuppliersAC;
                TempData["Action"] = AppUtility.SuppliersEnum.Search;
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
        [Authorize(Roles = "Admin, Accounting")]
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
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PaymentPageTypeEnum.SuppliersAC;
            TempData["SectionType"] = vendorSearchViewModel.SectionType;
            return View("IndexForPayment", filteredVendors.ToList());
        }



        //string? enName, string? heName, string? bizID, string? contactPerson, string? contactEmail,
        //string? orderEmail, string? phoneNum1, string? phoneNum2, string? faxNum, string? city, string? street, string? zip,
        //string? website, string? bank, string? bankBranch, string? bankAccount, string? swift, string? BIC, string? goldAccount




        // GET: Vendors/Create
        [Authorize(Roles = "Admin, LabManagement, Accounting")]
        public IActionResult Create(AppUtility.MenuItems SectionType)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = SectionType;
            //tempdata page type for active tab link
            if (SectionType == AppUtility.MenuItems.LabManagement)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.LabManagementPageTypeEnum.Suppliers;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.LabManagementSidebarEnum.NewSupplier;
            }
            else if (SectionType == AppUtility.MenuItems.Accounting)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PaymentPageTypeEnum.SuppliersAC;
                TempData["Action"] = AppUtility.SuppliersEnum.NewSupplier;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.AccountingSidebarEnum.NewSupplierAC;
            }
            CreateSupplierViewModel createSupplierViewModel = new CreateSupplierViewModel();
            createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
            List<AddContactViewModel> vendorContacts = new List<AddContactViewModel>();
            List<AddCommentViewModel> vendorComments = new List<AddCommentViewModel>();
            //only allowed to have 10 contacts
            //have to hard coded becasuse did not know how to render dynamic partial views
            for (int i = 0; i < 10; i++)
            {
                vendorContacts.Add(new AddContactViewModel());
                vendorComments.Add(new AddCommentViewModel());
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
        [Authorize(Roles = "Admin, Accounting")] 
        public IActionResult Create(CreateSupplierViewModel createSupplierViewModel)
        {
            List<AddCommentViewModel> vendorComments = new List<AddCommentViewModel>();
            List<AddContactViewModel> vendorContacts = new List<AddContactViewModel>();
            //loop throught the bedor contact to see which contact are filled in
            for (int i=0; i< createSupplierViewModel.VendorContacts.Count();i++)
            {
                if (!createSupplierViewModel.VendorContacts[i].IsActive)
                {
                    ModelState.Remove($"VendorContacts[{i}].VendorContact.VendorContactName");
                    ModelState.Remove($"VendorContacts[{i}].VendorContact.VendorContactEmail");
                    ModelState.Remove($"VendorContacts[{i}].VendorContact.VendorContactPhone");
                }
                else
                {
                    vendorContacts.Add(createSupplierViewModel.VendorContacts[i]);
                }
            }
            for (int i = 0; i < createSupplierViewModel.VendorComments.Count(); i++)
            {
                if (createSupplierViewModel.VendorComments[i].IsActive)
                {
                    vendorComments.Add(createSupplierViewModel.VendorComments[i]);
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
                foreach (var vendorContact in vendorContacts)
                {
                    vendorContact.VendorContact.VendorID = createSupplierViewModel.Vendor.VendorID;
                    _context.Add(vendorContact.VendorContact);
                    _context.SaveChanges();
                }
                var userid = _userManager.GetUserAsync(User).Result.Id;
                foreach (var vendorComment in vendorComments)
                {
                    vendorComment.VendorComment.VendorID = createSupplierViewModel.Vendor.VendorID;
                    vendorComment.VendorComment.ApplicationUserID = userid;
                    vendorComment.VendorComment.CommentTimeStamp = DateTime.Now;
                    _context.Add(vendorComment.VendorComment);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(IndexForPayment), new { SectionType = createSupplierViewModel.SectionType });
            }


            return View(createSupplierViewModel);
        }

        // GET: Vendors/Edit/5
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Edit(int? id, AppUtility.MenuItems SectionType)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateSupplierViewModel createSupplierViewModel = new CreateSupplierViewModel();
            createSupplierViewModel.Vendor = await _context.Vendors.Include(v=>v.VendorCategoryTypes).Where(v=>v.VendorID == id).FirstOrDefaultAsync();
            createSupplierViewModel.SectionType = SectionType;
            createSupplierViewModel.CategoryTypes = _context.CategoryTypes.ToList();
            createSupplierViewModel.VendorCategoryTypes = createSupplierViewModel.Vendor.VendorCategoryTypes.Select(vc=>vc.CategoryTypeID).ToList();
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
            List<AddContactViewModel> vendorContacts = new List<AddContactViewModel>();
            List<AddCommentViewModel> vendorComments = new List<AddCommentViewModel>();
            _context.VendorContacts.Where(c => c.VendorID == id).ToList().ForEach(c => vendorContacts.Add(new AddContactViewModel { VendorContact = c, IsActive = true }));
            _context.VendorComments.Where(c => c.VendorID == id).ToList().ForEach(c => vendorComments.Add(new AddCommentViewModel { VendorComment = c, IsActive = true }));
           int count = vendorContacts.Count;
            for (int i = count-1; i < 10; i++)
            {
                vendorContacts.Add(new AddContactViewModel());
                vendorComments.Add(new AddCommentViewModel());
            }
            createSupplierViewModel.VendorContacts = vendorContacts;
            createSupplierViewModel.VendorComments = vendorComments;
      
            //tempdata page type for active tab link
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PaymentPageTypeEnum.SuppliersAC;

            return View(createSupplierViewModel);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Edit(CreateSupplierViewModel createSupplierViewModel)
        {
            List<AddContactViewModel> vendorContacts = new List<AddContactViewModel>();
            List<AddCommentViewModel> vendorComments = new List<AddCommentViewModel>();
            for (int i = 0; i < createSupplierViewModel.VendorContacts.Count(); i++)
            {
                if (!createSupplierViewModel.VendorContacts[i].IsActive)
                {
                    ModelState.Remove($"VendorContacts[{i}].VendorContact.VendorContactName");
                    ModelState.Remove($"VendorContacts[{i}].VendorContact.VendorContactEmail");
                    ModelState.Remove($"VendorContacts[{i}].VendorContact.VendorContactPhone");
                }
                else
                {
                    vendorContacts.Add(createSupplierViewModel.VendorContacts[i]);
                }
            }
            for (int i = 0; i < createSupplierViewModel.VendorComments.Count(); i++)
            {
                if (createSupplierViewModel.VendorComments[i].IsActive)
                {
                    vendorComments.Add(createSupplierViewModel.VendorComments[i]);
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
                _context.SaveChanges();
                foreach (var type in createSupplierViewModel.VendorCategoryTypes)
                {
                    _context.Add(new VendorCategoryType { VendorID = createSupplierViewModel.Vendor.VendorID, CategoryTypeID = type });
                }
                _context.SaveChanges();
                foreach (var vendorContact in vendorContacts)
                {
                    vendorContact.VendorContact.VendorID = createSupplierViewModel.Vendor.VendorID;
                    _context.Update(vendorContact.VendorContact);
                    _context.SaveChanges();
                }
                foreach (var vendorComment in vendorComments)
                {
                    vendorComment.VendorComment.VendorID = createSupplierViewModel.Vendor.VendorID;
                    _context.Update(vendorComment.VendorComment);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(IndexForPayment), new { SectionType = createSupplierViewModel.SectionType});
            }


            return View(createSupplierViewModel);
        }

        // GET: Vendors/Delete/5
        [Authorize(Roles = "Admin, Accounting")]
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
        [Authorize(Roles = "Admin, Accounting")]
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

        [Authorize(Roles = "Admin, Accounting")]
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
    }
}
