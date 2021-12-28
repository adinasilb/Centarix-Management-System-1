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
    public class VendorsProc :  ApplicationDbContextProc<Vendor>
    {
        public VendorsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }


        public async Task<StringWithBool> UpdateAsync(CreateSupplierViewModel createSupplierViewModel, string UserID)
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
                            _context.Entry(createSupplierViewModel.Vendor).State = EntityState.Added;
                        }
                        else
                        {
                            _context.Entry(createSupplierViewModel.Vendor).State = EntityState.Modified;
                        }
                        await _context.SaveChangesAsync();

                        await _vendorCategoryTypesProc.UpdateAsync(createSupplierViewModel.VendorCategoryTypes, createSupplierViewModel.Vendor.VendorID);
                        await _vendorContactsProc.UpdateAsync(createSupplierViewModel.VendorContacts, createSupplierViewModel.Vendor.VendorID);

                        await _vendorCommentsProc.UpdateAsync(createSupplierViewModel.Comments, createSupplierViewModel.Vendor.VendorID, UserID);
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
       
    }




}
