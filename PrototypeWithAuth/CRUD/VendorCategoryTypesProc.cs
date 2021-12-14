using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class VendorCategoryTypesProc : ApplicationDbContextProc<VendorCategoryType>
    {
        public VendorCategoryTypesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> CreateWithoutSavingAsync(int VendorID, int CategoryTypeID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                VendorCategoryType vct = new VendorCategoryType()
                {
                    CategoryTypeID = CategoryTypeID,
                    VendorID = VendorID
                };
                _context.Add(vct);
                ReturnVal.Bool = true;
            }
            catch (Exception ex)
            {
                ReturnVal.Bool = false;
                ReturnVal.String = AppUtility.GetExceptionMessage(ex);
            }
            return ReturnVal;
        }


        private async Task<StringWithBool> RemoveAsync(List<VendorCategoryType> items)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                foreach(var item in items)
                {
                    _context.Remove(item);
                }
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public async Task<StringWithBool> UpdateAsync(List<int> newVendorCategoryTypes, int vendorID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var vendor = await _vendorsProc.ReadOneAsync(new List<Expression<Func<Vendor, bool>>> { v => v.VendorID == vendorID },
                         new List<ComplexIncludes<Vendor, ModelBase>> { new ComplexIncludes<Vendor, ModelBase> { Include= v => v.VendorCategoryTypes } });

                await _vendorCategoryTypesProc.RemoveAsync(vendor.VendorCategoryTypes);
                await _vendorCategoryTypesProc.CreateAsync(newVendorCategoryTypes, vendor.VendorID);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;          

        }

        private async Task<StringWithBool> CreateAsync(List<int> types, int vendorID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                foreach(var t in types)
                {
                    _context.Entry(new VendorCategoryType {CategoryTypeID = t, VendorID = vendorID }).State = EntityState.Added;
                }
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

    }
}
