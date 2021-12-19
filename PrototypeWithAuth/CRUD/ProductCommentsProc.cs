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
    public class ProductCommentsProc : ApplicationDbContextProc<ProductComment>
    {
        public ProductCommentsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }


        public override async Task<StringWithBool> UpdateAsync<ProductComment>(List<ProductComment> comments, int vendorID, string userID)
        {
            return await base.UpdateAsync(comments, vendorID, userID);
        }
    }
}
