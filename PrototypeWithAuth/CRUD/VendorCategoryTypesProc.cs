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


        private async Task RemoveAsync(List<VendorCategoryType> items)
        {
            try
            {
                foreach(var item in items)
                {
                    _context.Remove(item);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateAsync(List<int> newVendorCategoryTypes, int vendorID)
        {
            try
            {
                var oldVendorCategoryTypes = await Read(new List<Expression<Func<VendorCategoryType, bool>>> { v => v.VendorID == vendorID } ).ToListAsync();

                await RemoveAsync(oldVendorCategoryTypes);
                await CreateAsync(newVendorCategoryTypes, vendorID);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update vendor category types - " + AppUtility.GetExceptionMessage(ex));
            }         

        }

        private async Task CreateAsync(List<int> types, int vendorID)
        {
            try
            {
                foreach(var t in types)
                {
                    _context.Entry(new VendorCategoryType {CategoryTypeID = t, VendorID = vendorID }).State = EntityState.Added;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
