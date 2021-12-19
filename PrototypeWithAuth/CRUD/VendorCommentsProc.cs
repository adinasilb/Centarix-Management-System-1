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
    public class VendorCommentsProc : ApplicationDbContextProc<VendorComment>
    {
        public VendorCommentsProc(ApplicationDbContext context, bool FromBase = false) : base (context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public IQueryable<VendorComment> Read()
        {
            return _context.VendorComments.AsNoTracking().AsQueryable();
        }

     
        public override async Task<StringWithBool> UpdateAsync<VendorComment>(List<VendorComment> comments, int vendorID, string userID)
        {
            return await base.UpdateAsync(comments, vendorID, userID);
        }
    }
}
