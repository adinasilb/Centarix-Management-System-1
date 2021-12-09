using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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
    public class VendorsProc : ApplicationDbContextProc
    {
        public VendorsProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
         }

        public IQueryable<Vendor> Read(List<Expression<Func<Vendor, object>>> includes = null)
        {
            var vendors = _context.Vendors.AsQueryable();
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    vendors = vendors.Include(t);
                }
            }
            return vendors.AsNoTracking().AsQueryable();
        }

        public IQueryable<Vendor> ReadByCategoryTypeID(int CategoryTypeID)
        {
            return _context.Vendors
                .Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == CategoryTypeID).Count() > 0)
                .AsNoTracking().AsQueryable();
        }

        public async Task<Vendor> ReadByVendorIDAsync(int VendorID, List<Expression<Func<Vendor, object>>> includes = null)
        {
            var vendor = _context.Vendors
                .Where(v => v.VendorID == VendorID).Take(1);
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    vendor = vendor.Include(t);
                }
            }
            return await vendor.AsNoTracking().FirstOrDefaultAsync();
        }

        public Vendor ReadByVendorBusinessIDCountryID(string VendorBusinessID, int CountryID)
        {
            return _context.Vendors.Where(v => v.VendorBuisnessID.Equals(VendorBusinessID) && v.CountryID == CountryID)
                .AsNoTracking().FirstOrDefault();
        }
        public Vendor ReadByVendorBusinessIDCountryIDVendorID(string VendorBusinessID, int CountryID, int VendorID)
        {
            return _context.Vendors.Where(v => v.VendorBuisnessID.Equals(VendorBusinessID) && v.CountryID == CountryID && v.VendorID != VendorID)
                .AsNoTracking().FirstOrDefault();
        }

        public IEnumerable<Vendor> Read(VendorSearchViewModel vendorSearchViewModel)
        {
            IEnumerable<Models.Vendor> filteredVendors = Read(new List<Expression<Func<Vendor, object>>> { v=>v.VendorCategoryTypes});
            List<int> orderedVendorCategoryTypes = new List<int>();
            if (vendorSearchViewModel.VendorCategoryTypes != null)
            {
                orderedVendorCategoryTypes = vendorSearchViewModel.VendorCategoryTypes.OrderBy(e => e).ToList();
            }
            IEnumerable<Vendor> listfilteredVendors = filteredVendors
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
            (String.IsNullOrEmpty(vendorSearchViewModel.VendorRoutingNum) || fv.VendorRoutingNum.ToLower().Contains(vendorSearchViewModel.VendorRoutingNum.ToLower()))).AsEnumerable();

            //listfilteredVendors = listfilteredVendors.Where(fv => (orderedVendorCategoryTypes == null || orderedVendorCategoryTypes.SequenceEqual(fv.VendorCategoryTypes.OrderBy( ct => ct.CategoryTypeID).Select(ct => ct.CategoryTypeID)))).ToList(); //if choose lab, will not show vendors that have both lab and operations
            listfilteredVendors = listfilteredVendors.Where(fv => (orderedVendorCategoryTypes == null || orderedVendorCategoryTypes.All(fv.VendorCategoryTypes.Select(ct => ct.CategoryTypeID).Contains))).AsEnumerable(); //if choose lab, will include vendors that have both lab and operations
            return listfilteredVendors;
        }

        public async Task<StringWithBool> UpdateAsync(CreateSupplierViewModel createSupplierViewModel, ModelStateDictionary ModelState, string UserID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var context = new ValidationContext(createSupplierViewModel.Vendor, null, null);
                    var results = new List<ValidationResult>();

                    if (Validator.TryValidateObject(createSupplierViewModel.Vendor, context, results, true))
                    {
                        if (createSupplierViewModel.Vendor.VendorID==0)
                        {
                            _context.Add(createSupplierViewModel.Vendor);
                        }
                        else
                        {
                            _context.Update(createSupplierViewModel.Vendor);                    
                        }
                        await _context.SaveChangesAsync();
                        var vendor = await
                            _context.Vendors.Where(v => v.VendorID == createSupplierViewModel.Vendor.VendorID).Include(v => v.VendorCategoryTypes).FirstOrDefaultAsync();
                        if (vendor.VendorCategoryTypes.Count() > 0)
                        {
                            foreach (var type in createSupplierViewModel.Vendor.VendorCategoryTypes)
                            {
                                //need proc?
                                _context.Remove(type);
                            }
                        }
                        foreach (var type in createSupplierViewModel.VendorCategoryTypes)
                        {
                            await _vendorCategoryTypesProc.CreateWithoutSavingAsync(createSupplierViewModel.Vendor.VendorID, type);
                        }
                       
                        foreach (var vendorContact in createSupplierViewModel.VendorContacts)
                        {
                            if (vendorContact.Delete && vendorContact.VendorContact.VendorContactID != 0)
                            {
                                var dvc = _context.VendorContacts.Where(vc => vc.VendorContactID == vendorContact.VendorContact.VendorContactID).FirstOrDefault();
                                //need proc?
                                _context.Remove(dvc);
                            }
                            else if (!vendorContact.Delete)
                            {
                                vendorContact.VendorContact.VendorID = createSupplierViewModel.Vendor.VendorID;
                                //need proc?
                                _context.Update(vendorContact.VendorContact);
                            }

                        }
                        if (createSupplierViewModel.Comments != null)
                        {
                                foreach (var vendorComment in createSupplierViewModel.Comments)
                                {
                                    if (!vendorComment.IsDeleted)
                                    {
                                        vendorComment.ObjectID = createSupplierViewModel.Vendor.VendorID;
                                        if (vendorComment.CommentID == 0)
                                        {
                                            vendorComment.ApplicationUserID = UserID;
                                            vendorComment.CommentTimeStamp = DateTime.Now;
                                            //need proc?
                                            _context.Add(vendorComment);
                                        }
                                        else
                                        {    //need proc?
                                            _context.Update(vendorComment);
                                        }
                                    }
                                    else
                                    {
                                        var vendorCommentDB = _context.VendorComments.Where(c => c.CommentID == vendorComment.CommentID).FirstOrDefault();
                                        if (vendorCommentDB != null)
                                        {
                                            vendorCommentDB.IsDeleted = true;
                                            //need proc?
                                            _context.Update(vendorCommentDB);
                                        }
                                    }
                                }                            
                        }
                        await _context.SaveChangesAsync();
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

        public async Task<StringWithBool> DeleteAsync(int VendorID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            if (_context.Products.Where(p => p.VendorID == VendorID).Any())
            {
                ReturnVal.Bool = false;
            }
            else
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var vendorComment in _context.VendorComments.Where(vc => vc.ObjectID == VendorID))
                        {
                            //need proc?
                            _context.Remove(vendorComment);
                        }
                        await _context.SaveChangesAsync();
                        try
                        {
                            foreach (var vendorContact in _context.VendorContacts.Where(vc => vc.VendorID == VendorID))
                            {
                                //need proc?
                                _context.Remove(vendorContact);
                            }
                            await _context.SaveChangesAsync();
                            try
                            {
                                foreach (var vendorCT in _context.Vendors.Where(vc => vc.VendorID == VendorID).Select(v => v.VendorCategoryTypes))
                                {
                                    //need proc?
                                    _context.Remove(vendorCT);
                                }
                                await _context.SaveChangesAsync();
                                try
                                {
                                    var vendor = await ReadByVendorIDAsync(VendorID);
                                    _context.Remove(vendor);
                                    await _context.SaveChangesAsync();
                                    await transaction.CommitAsync();
                                }
                                catch (Exception ex)
                                {
                                    ReturnVal.Bool = false;
                                    ReturnVal.String = "Could not delete vendor";
                                }
                            }
                            catch (Exception ex)
                            {
                                ReturnVal.Bool = false;
                                ReturnVal.String = "Could not delete vendor category types";
                            }
                        }
                        catch (Exception ex)
                        {
                            ReturnVal.Bool = false;
                            ReturnVal.String = "Could not delete vendor contacts";
                        }
                    }
                    catch (Exception ex)
                    {
                        ReturnVal.Bool = false;
                        ReturnVal.String = "Could not delete vendor comments";
                    }
                }
            }
            return ReturnVal;
        }
    }
}
