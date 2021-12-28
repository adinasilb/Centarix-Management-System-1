using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.CRUD
{
    public class ProductCommentsProc : CommentBasesProc<ProductComment>
    {
        public ProductCommentsProc(ApplicationDbContext context, bool FromBase = false) : base(context, FromBase)
        {
        }


        public override async Task UpdateAsync(List<ProductComment> comments, int vendorID, string userID)
        {
            await base.UpdateAsync(comments, vendorID, userID);
        }
    }
}
