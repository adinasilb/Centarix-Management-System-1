using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class VendorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorsController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: Vendors
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request)
        {
            TempData["PageType"] = PageType;
            TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.Vendor;
            return View(await _context.Vendors.ToListAsync());
        }

        // GET: Vendors
        public async Task<IActionResult> IndexForPayment(/*string? filteredVendors*/)
        {
            //if (filteredVendors != null)
            //{
            //    List<VendorListViewModel> vendorSearchViewModel = JsonConvert.DeserializeObject<List<VendorListViewModel>>(filteredVendors);
            //    return View(filteredVendors.ToList());


            //}
            //else
            //{
            //tempdata page type for active tab link
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;

            return View(await _context.Vendors.ToListAsync());
            //}
        }

        // GET: Vendors/Details/5
        public async Task<IActionResult> Details(int? id)
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
        // GET: Vendors/Details/5
        public async Task<IActionResult> DetailsForPayment(int? id) // should not have to repeat this code - but for now just doing things quick
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

            //tempdata page type for active tab link
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;

            return View(vendor);
        }

        //public async Task<IActionResult> SearchVendor(VendorSearchViewModel vendorSearchViewModel)
        //{
        //    IQueryable<Vendor> vendorsSearched = _context.Vendors.AsQueryable();
        //    if (vendorSearchViewModel.Vendor.VendorEnName != null)
        //    {
        //        vendorsSearched = vendorsSearched.Where(v => v.VendorEnName == vendorSearchViewModel.Vendor.VendorEnName);
        //    }
        //    if (vendorSearchViewModel.Vendor.VendorHeName != null)
        //    {
        //        vendorsSearched = vendorsSearched.Where(v => v.VendorHeName == vendorSearchViewModel.Vendor.VendorHeName);
        //    }
        //    if (vendorSearchViewModel.Vendor.VendorBuisnessID != null)
        //    {
        //        vendorsSearched = vendorsSearched.Where(v => v.VendorBuisnessID == vendorSearchViewModel.Vendor.VendorBuisnessID);
        //    }
        //    if (vendorSearchViewModel.Vendor.VendorContactPhone1 != null)
        //    {
        //        vendorsSearched = vendorsSearched.Where(v => v.VendorContactPhone1 == vendorSearchViewModel.Vendor.VendorContactPhone1);
        //    }
        //    return View(vendorsSearched.ToListAsync());
        //}
        // GET: Vendors/Search
        [HttpGet]
        public IActionResult Search()
        {
            VendorSearchViewModel vendorSearchViewModel = new VendorSearchViewModel
            {
                ParentCategories = _context.ParentCategories.ToList(),
                Vendors = _context.Vendors.ToList(),
                Vendor = new Vendor()
                // Requests = _context.Requests.ToList(),
                //check if we need this here
            };

            //tempdata page type for active tab link
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;

            //return View(vendorSearchViewModel);
            return PartialView(vendorSearchViewModel);
        }
        // Post: Vendors/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.ContactPerson) || fv.ContactPerson.Contains(vendorSearchViewModel.Vendor.ContactPerson))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.ContactEmail) || fv.ContactEmail.Contains(vendorSearchViewModel.Vendor.ContactEmail))
            &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.OrderEmail) || fv.OrderEmail.Contains(vendorSearchViewModel.Vendor.OrderEmail))
             &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorContactPhone1) || fv.VendorContactPhone1.Contains(vendorSearchViewModel.Vendor.VendorContactPhone1))
              &&
            (String.IsNullOrEmpty(vendorSearchViewModel.Vendor.VendorContactPhone2) || fv.VendorContactPhone2.Contains(vendorSearchViewModel.Vendor.VendorContactPhone2))
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
        public IActionResult Create()
        {
            //tempdata page type for active tab link
            TempData["PageType"] = AppUtility.PaymentPageTypeEnum.Suppliers;

            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendorID,VendorEnName,VendorHeName,VendorBuisnessID,ContactPerson,ContactEmail,OrderEmail,VendorContactPhone1,VendorContactPhone2,VendorFax,VendorCity,VendorStreet,VendorZip,VendorWebsite,VendorBank,VendorBankBranch,VendorAccountNum,VendorSwift,VendorBIC,VendorGoldAccount")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexForPayment));
            }


            return View(vendor);
        }

        // GET: Vendors/Edit/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexForPayment));
        }

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
