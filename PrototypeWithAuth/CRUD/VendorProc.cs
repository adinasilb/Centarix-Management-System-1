using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.Exceptions;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
namespace PrototypeWithAuth.CRUD
{
    public class VendorProc : ApplicationDbContextProcedure
    {
        public VendorProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        public IQueryable<Models.Vendor> Read()
        {
            return _context.Vendors
                .Include(v => v.VendorCategoryTypes)
                .AsNoTracking().AsQueryable();
        }

        public IQueryable<Models.Vendor> ReadByCategoryTypeID(int CategoryTypeID)
        {
            return _context.Vendors
                .Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == CategoryTypeID).Count() > 0)
                .AsNoTracking().AsQueryable();
        }

        public Models.Vendor ReadByVendorID(int VendorID)
        {
            return _context.Vendors
                .Where(v => v.VendorID == VendorID).Include(v => v.VendorCategoryTypes)
                .AsNoTracking().FirstOrDefault();
        }

        public IQueryable<Models.Vendor> Read(VendorSearchViewModel vendorSearchViewModel)
        {
            IQueryable<Models.Vendor> filteredVendors = this.Read();
            List<int> orderedVendorCategoryTypes = new List<int>();
            if (vendorSearchViewModel.VendorCategoryTypes != null)
            {
                orderedVendorCategoryTypes = vendorSearchViewModel.VendorCategoryTypes.OrderBy(e => e).ToList();
            }
            IQueryable<Models.Vendor> listfilteredVendors = filteredVendors
            .Where(fv => (String.IsNullOrEmpty(vendorSearchViewModel.VendorEnName) || fv.VendorEnName.ToLower().Contains(vendorSearchViewModel.VendorEnName.ToLower()))
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
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorRoutingNum) || fv.VendorRoutingNum.ToLower().Contains(vendorSearchViewModel.VendorRoutingNum.ToLower()))).AsQueryable();

            //listfilteredVendors = listfilteredVendors.Where(fv => (orderedVendorCategoryTypes == null || orderedVendorCategoryTypes.SequenceEqual(fv.VendorCategoryTypes.OrderBy( ct => ct.CategoryTypeID).Select(ct => ct.CategoryTypeID)))).ToList(); //if choose lab, will not show vendors that have both lab and operations
            listfilteredVendors = listfilteredVendors.Where(fv => (orderedVendorCategoryTypes == null || orderedVendorCategoryTypes.All(fv.VendorCategoryTypes.Select(ct => ct.CategoryTypeID).Contains))).AsQueryable(); //if choose lab, will include vendors that have both lab and operations
            return listfilteredVendors;
        }

        public async Task<StringWithBool> Create(CreateSupplierViewModel createSupplierViewModel, ModelStateDictionary ModelState, string UserID)
        {
            StringWithBool stringWithBool = new StringWithBool();
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
                        //var userid = _userManager.GetUserAsync(User).Result.Id;
                        if (createSupplierViewModel.VendorComments != null)
                        {
                            foreach (var vendorComment in createSupplierViewModel.VendorComments)
                            {
                                vendorComment.VendorID = createSupplierViewModel.Vendor.VendorID;
                                vendorComment.ApplicationUserID = UserID;
                                vendorComment.CommentTimeStamp = DateTime.Now;
                                _context.Add(vendorComment);

                            }
                        }
                        await _context.SaveChangesAsync();
                        //throw new Exception();
                        await transaction.CommitAsync();

                        //
                    }
                    else
                    {
                        stringWithBool.Bool = true;
                        stringWithBool.String = "Model State Invalid";
                        throw new ModelStateInvalidException();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    stringWithBool.Bool = true;
                    stringWithBool.String = AppUtility.GetExceptionMessage(ex);

                }
            }
            return stringWithBool;
        }

        public async Task<StringWithBool> Update(CreateSupplierViewModel createSupplierViewModel, ModelStateDictionary ModelState)
        {
            StringWithBool ReturnVal = new StringWithBool();
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
                        var vendor = await
                            _context.Vendors.Where(v => v.VendorID == createSupplierViewModel.Vendor.VendorID).Include(v => v.VendorCategoryTypes).FirstOrDefaultAsync();
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
                        ReturnVal.Bool = true;
                    }
                    else
                    {
                        ReturnVal.Bool = false;
                        ReturnVal.String = "Model State Invalid Exception";
                        throw new ModelStateInvalidException();
                    }
                }
                catch (Exception ex)
                {
                    ReturnVal.Bool = false;
                    ReturnVal.String = AppUtility.GetExceptionMessage(ex);
                    await transaction.RollbackAsync();
                }
            }
            return ReturnVal;
        }

        public async Task<StringWithBool> Delete(int VendorID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            if ()
            return ReturnVal;
        }
    }
}
