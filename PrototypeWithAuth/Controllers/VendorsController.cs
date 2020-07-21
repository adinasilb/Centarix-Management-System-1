using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNet.Identity;
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
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request)
        {
            TempData["PageType"] = PageType;
            TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.Vendor;
            return View(await _context.Vendors.ToListAsync());
        }

        // GET: Vendors
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> IndexForPayment()
        {
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;

            return View(await _context.Vendors.ToListAsync());

        }
        [Authorize(Roles = "Admin, LabManagement")]
        public async Task<IActionResult> IndexForLabManage()
        {
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;

            return View(await _context.Vendors.ToListAsync());

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
        public async Task<IActionResult> DetailsForPayment(int? id) // should not have to repeat this code - but for now just doing things quick
        {
            if (id == null)
            {
                return NotFound();
            }
            VendorDetailsViewModel vendorDetailsViewModel = new VendorDetailsViewModel
            {
                ParentCategories = _context.ParentCategories.ToList(),
                Products = _context.Products.Where(p => p.VendorID == id).Include(p =>p.ProductSubcategory).ThenInclude(p => p.ParentCategory),
                Vendors = _context.Vendors,
                Vendor = await _context.Vendors.FirstOrDefaultAsync(m => m.VendorID == id),
            // Requests = _context.Requests.ToList(),
            //check if we need this here
        };
            

            
            if (vendorDetailsViewModel.Vendor == null)
            {
                return NotFound();
            }

            //tempdata page type for active tab link
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;

            return View(vendorDetailsViewModel);
        }

     
        // GET: Vendors/Search
        [HttpGet]
        [Authorize(Roles = "Admin, Accounting")]
        public IActionResult Search()
        {
            VendorSearchViewModel vendorSearchViewModel = new VendorSearchViewModel
            {
                ParentCategories = _context.ParentCategories.ToList(),
                Vendors = _context.Vendors.ToList(),
                Vendor = new Vendor()
  
            };

            //tempdata page type for active tab link
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;

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
                .Where(fv => (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorEnName) || fv.VendorEnName.Contains(vendorSearchViewModel.Vendor.VendorEnName))
                 &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorHeName) || fv.VendorHeName.Contains(vendorSearchViewModel.Vendor.VendorHeName))
             &&
             (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorBuisnessID) || fv.VendorBuisnessID.Contains(vendorSearchViewModel.Vendor.VendorBuisnessID))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.InfoEmail) || fv.InfoEmail.Contains(vendorSearchViewModel.Vendor.InfoEmail))
            &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.OrdersEmail) || fv.OrdersEmail.Contains(vendorSearchViewModel.Vendor.OrdersEmail))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorTelephone) || fv.VendorTelephone.Contains(vendorSearchViewModel.Vendor.VendorTelephone))
              &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorCellPhone) || fv.VendorCellPhone.Contains(vendorSearchViewModel.Vendor.VendorCellPhone))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorFax) || fv.VendorFax.Contains(vendorSearchViewModel.Vendor.VendorFax))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorCity) || fv.VendorCity.Contains(vendorSearchViewModel.Vendor.VendorCity))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorStreet) || fv.VendorStreet.Contains(vendorSearchViewModel.Vendor.VendorStreet))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorZip) || fv.VendorZip.Contains(vendorSearchViewModel.Vendor.VendorZip))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorWebsite) || fv.VendorWebsite.Contains(vendorSearchViewModel.Vendor.VendorWebsite))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorBank) || fv.VendorBank.Contains(vendorSearchViewModel.Vendor.VendorBank))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorBankBranch) || fv.VendorBankBranch.Contains(vendorSearchViewModel.Vendor.VendorBankBranch))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorAccountNum) || fv.VendorAccountNum.Contains(vendorSearchViewModel.Vendor.VendorAccountNum))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorSwift) || fv.VendorSwift.Contains(vendorSearchViewModel.Vendor.VendorSwift))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorBIC) || fv.VendorBIC.Contains(vendorSearchViewModel.Vendor.VendorBIC))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorGoldAccount) || fv.VendorGoldAccount.Contains(vendorSearchViewModel.Vendor.VendorGoldAccount)));


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

            return View("IndexForPayment", filteredVendors.ToList());
        }



        //string? enName, string? heName, string? bizID, string? contactPerson, string? contactEmail,
        //string? orderEmail, string? phoneNum1, string? phoneNum2, string? faxNum, string? city, string? street, string? zip,
        //string? website, string? bank, string? bankBranch, string? bankAccount, string? swift, string? BIC, string? goldAccount




        // GET: Vendors/Create
        [Authorize(Roles = "Admin, Accounting")]
        public IActionResult Create()
        {
            //tempdata page type for active tab link
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;
            
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
            return View(createSupplierViewModel);
        }

        //[HttpGet]
        //public IActionResult AddContact()
        //{
        //    //tempdata page type for active tab link
        //    TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;
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
                if (!createSupplierViewModel.VendorComments[i].IsActive)
                {
                    ModelState.Remove($"VendorComments[{i}].VendorComment.CommentText");           
                }
                else
                {
                    vendorComments.Add(createSupplierViewModel.VendorComments[i]);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(createSupplierViewModel.Vendor);
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
                return RedirectToAction(nameof(IndexForPayment));
            }


            return View(createSupplierViewModel);
        }

        // GET: Vendors/Edit/5
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }


            //tempdata page type for active tab link
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;

            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<IActionResult> Edit(int id, [Bind("VendorID,VendorEnName,VendorHeName,VendorBuisnessID,ContactPerson,ContactEmail,OrderEmail,VendorContactPhone1,VendorContactPhone2,VendorFax,VendorCity,VendorStreet,VendorZip,VendorWebsite,VendorBank,VendorBankBranch,VendorAccountNum,VendorSwift,VendorBIC,VendorGoldAccount")] Vendor vendor)
        {
            if (id != vendor.VendorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorExists(vendor.VendorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexForPayment));
            }
            return View(vendor);
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
