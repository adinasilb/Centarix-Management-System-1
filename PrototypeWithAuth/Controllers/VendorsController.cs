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
        private CRUD.VendorsProc _vendor;
        private CRUD.CategoryTypesProc _categoryType;
        private CRUD.CountriesProc _country;
        private CRUD.VendorContactsProc _vendorContact;
        private CRUD.VendorCommentsProc _vendorComment;
        public VendorsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)

        {
            _vendor = new CRUD.VendorsProc(context, userManager);
            _categoryType = new CRUD.CategoryTypesProc(context, userManager);
            _country = new CRUD.CountriesProc(context, userManager);
            _vendorContact = new CRUD.VendorContactsProc(context, userManager);
            _vendorComment = new CRUD.VendorCommentsProc(context, userManager);
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
            //have to hard coded becasuse did not know how to render dynamic partial views
            //PROC START
            createSupplierViewModel.Countries = new List<SelectListItem>();
            foreach (var country in _country.Read())
            {
                createSupplierViewModel.Countries.Add(new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() });
            }


            createSupplierViewModel.VendorContacts = vendorContacts;
            createSupplierViewModel.VendorComments = vendorComments;
            createSupplierViewModel.SectionType = SectionType; //TODO: take this out when all the views are combined
            createSupplierViewModel.CategoryTypes = _categoryType.Read();
            //PROC END

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
            //PROC START
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
                createSupplierViewModel.CommentTypes = Enum.GetValues(typeof(AppUtility.CommentTypeEnum)).Cast<AppUtility.CommentTypeEnum>().ToList();
                createSupplierViewModel.CategoryTypes = _categoryType.Read();
                createSupplierViewModel.Countries = _country.ReadAsSelectList();

                return View("Create", createSupplierViewModel);

            }
            //PROC END 
            //VENDOR REDO START
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
                    var context = new ValidationContext(createSupplierViewModel.Vendor, null, null);
                    var results = new List<ValidationResult>();

                    if (Validator.TryValidateObject(createSupplierViewModel.Vendor, context, results, true))
                    {
                        _context.Add(createSupplierViewModel.Vendor);
                        _context.SaveChanges();
                        foreach (var type in createSupplierViewModel.VendorCategoryTypes)
                        {
                            _context.Add(new VendorCategoryType { VendorID = createSupplierViewModel.Vendor.VendorID, CategoryTypeID = type });
                        }
                        _context.SaveChanges();

                        //add contacts
                        foreach (var vendorContact in createSupplierViewModel.VendorContacts.Where(vc => !vc.Delete))
                        {
                            vendorContact.VendorContact.VendorID = createSupplierViewModel.Vendor.VendorID;
                            _context.Update(vendorContact.VendorContact);

                        }
                        await _context.SaveChangesAsync();
                        var userid = _userManager.GetUserId(User);
                        if (createSupplierViewModel.Comments != null)
                        {
                            foreach (var vendorComment in createSupplierViewModel.Comments)
                            {
                                if (!vendorComment.IsDeleted)
                                {
                                    vendorComment.ObjectID = createSupplierViewModel.Vendor.VendorID;
                                    vendorComment.ApplicationUserID = userid;
                                    vendorComment.CommentTimeStamp = DateTime.Now;
                                    _context.Add(vendorComment);
                                }
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
                    createSupplierViewModel = await GetCreateSupplierViewModel(createSupplierViewModel.SectionType, 0, createSupplierViewModel.Tab);
                    createSupplierViewModel.ModalType = AppUtility.VendorModalType.Create;

                    return View(createSupplierViewModel);
                }
            }
            //VENDOR REDO END
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
        public async Task<IActionResult> editFunction(int? id, AppUtility.MenuItems SectionType, int? Tab = 0)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateSupplierViewModel createSupplierViewModel = new CreateSupplierViewModel();
            createSupplierViewModel.Countries = _country.ReadAsSelectList();
            createSupplierViewModel.Vendor = await _vendor.ReadByVendorIDAsync(Convert.ToInt32(id), new List<System.Linq.Expressions.Expression<Func<Vendor, object>>> { v=>v.VendorCategoryTypes});
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
            createSupplierViewModel.VendorComments = await _vendorComment.ReadByVendorID(Convert.ToInt32(id), new List<System.Linq.Expressions.Expression<Func<VendorComment, object>>> { c=>c.ApplicationUser}).ToListAsync();

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
            //PROC START
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
                createSupplierViewModel.VendorComments = await _vendorComment.ReadByVendorID(createSupplierViewModel.Vendor.VendorID, new List<System.Linq.Expressions.Expression<Func<VendorComment, object>>> { c=>c.ApplicationUser }).ToListAsync();
                Response.StatusCode = 550;
                return PartialView("Edit", createSupplierViewModel);
            }
            //PROC END
            //VENDOR REDO END
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
                    var context = new ValidationContext(createSupplierViewModel.Vendor, null, null);
                    var results = new List<ValidationResult>();

                    if (Validator.TryValidateObject(createSupplierViewModel.Vendor, context, results, true))
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

                        //update,add, and delete contacts
                        foreach (var vendorContact in createSupplierViewModel.VendorContacts)
                        {
                            if (vendorContact.Delete && vendorContact.VendorContact.VendorContactID != 0)
                            {
                                var dvc = _context.VendorContacts.Where(vc => vc.VendorContactID == vendorContact.VendorContact.VendorContactID).FirstOrDefault();
                                _context.Remove(dvc);
                            }
                            else if (!vendorContact.Delete)
                            {
                                vendorContact.VendorContact.VendorID = createSupplierViewModel.Vendor.VendorID;
                                _context.Update(vendorContact.VendorContact);
                            }

                        }
                        await _context.SaveChangesAsync();
                        if (createSupplierViewModel.Comments != null)
                        {
                            foreach (var vendorComment in createSupplierViewModel.Comments)
                            {
                                if (!vendorComment.IsDeleted)
                                {
                                    vendorComment.ObjectID = createSupplierViewModel.Vendor.VendorID;
                                    if (vendorComment.CommentID == 0)
                                    {
                                        vendorComment.CommentTimeStamp = DateTime.Now;
                                        _context.Entry(vendorComment).State = EntityState.Added;
                                    }
                                    else
                                    {
                                        _context.Entry(vendorComment).State = EntityState.Modified;
                                    }
                                }
                                else
                                {
                                    var vendorCommentDB = _context.VendorComments.Where(c => c.CommentID == vendorComment.CommentID).FirstOrDefault();
                                    if (vendorCommentDB != null)
                                    {
                                        vendorCommentDB.IsDeleted = true;
                                        _context.Entry(vendorCommentDB).State = EntityState.Modified;
                                    }
                                }
                            }
                        }
                        await _context.SaveChangesAsync();
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
                    createSupplierViewModel = await GetCreateSupplierViewModel(createSupplierViewModel.SectionType, createSupplierViewModel.Vendor.VendorID, createSupplierViewModel.Tab);
                    createSupplierViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                    createSupplierViewModel.ModalType = AppUtility.VendorModalType.Edit;

                    Response.StatusCode = 550;
                    return PartialView("Edit", createSupplierViewModel);

                }
            }
            //VENDOR REDO END
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
