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
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Data.Migrations;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class VendorsController : SharedController
    {
        private CRUD.VendorProc _vendor;
        private CRUD.CategoryTypeProc _categoryType;
        private CRUD.CountryProc _country;
        private CRUD.VendorContact _vendorContact;
        private CRUD.VendorCommentProc _vendorComment;
        public VendorsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)

        {
            _vendor = new CRUD.VendorProc(context, userManager);
            _categoryType = new CRUD.CategoryTypeProc(context, userManager);
            _country = new CRUD.CountryProc(context, userManager);
            _vendorContact = new CRUD.VendorContact(context, userManager);
            _vendorComment = new CRUD.VendorCommentProc(context, userManager);
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
                //ParentCategories = _context.ParentCategories.ToList(),
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
            foreach (var country in _country.Read())
            {
                createSupplierViewModel.Countries.Add(new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() });
            }
            createSupplierViewModel.VendorContacts = vendorContacts;
            createSupplierViewModel.VendorComments = vendorComments;
            createSupplierViewModel.SectionType = SectionType; //TODO: take this out when all the views are combined
            createSupplierViewModel.CategoryTypes = _categoryType.Read();

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
            var userid = _userManager.GetUserAsync(User).Result.Id;
            StringWithBool vendorCreated = await _vendor.Create(createSupplierViewModel, ModelState, userid);
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
                createSupplierViewModel.ErrorMessage += vendorCreated.String;
                createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
                createSupplierViewModel.CategoryTypes = _categoryType.Read();
                createSupplierViewModel.Countries = _country.ReadAsSelectList();

                return View("Create", createSupplierViewModel);

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
        public async Task<IActionResult> _VendorHeader(int id, AppUtility.MenuItems SectionType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var createSupplierViewModel = new CreateSupplierViewModel()
            {
                Vendor = _vendor.ReadByVendorID(id)
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
            createSupplierViewModel.Countries = _country.ReadAsSelectList();
            createSupplierViewModel.Vendor = _vendor.ReadByVendorID(Convert.ToInt32(id));
            createSupplierViewModel.SectionType = SectionType;
            createSupplierViewModel.CategoryTypes = _categoryType.Read();
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
            createSupplierViewModel.VendorContacts = _vendorContact.ReadAsVendorContactWithDeleteByVendorID(Convert.ToInt32(id));
            createSupplierViewModel.VendorComments = _vendorComment.ReadByVendorID(Convert.ToInt32(id));

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
            var vendorUpdated = await _vendor.Update(createSupplierViewModel, ModelState);
            if (vendorUpdated.Bool)
            {
                return RedirectToAction(nameof(IndexForPayment), new { SectionType = createSupplierViewModel.SectionType });
            }
            else
            {
                createSupplierViewModel.ErrorMessage += vendorUpdated.String;
                createSupplierViewModel.CategoryTypes = _categoryType.Read();
                createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
                createSupplierViewModel.VendorComments = _vendorComment.ReadByVendorID(createSupplierViewModel.Vendor.VendorID);
                Response.StatusCode = 550;
                return PartialView("Edit", createSupplierViewModel);
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
