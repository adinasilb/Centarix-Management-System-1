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
    public class VendorCommentsProc : CommentBasesProc<VendorComment>
    {
        public VendorCommentsProc(ApplicationDbContext context, bool FromBase = false) : base (context, FromBase)
        {

        }


     
        public override async Task UpdateAsync(List<VendorComment> comments, int vendorID, string userID)
        {
            await base.UpdateAsync(comments, vendorID, userID);
        }
    }
}
