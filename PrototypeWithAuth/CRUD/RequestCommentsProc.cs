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
    public class RequestCommentsProc : ApplicationDbContextProc<RequestComment>
    {
        public RequestCommentsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }


        public override async Task<StringWithBool> UpdateAsync<RequestComment>(List<RequestComment> comments, int vendorID, string userID)
        {
            return await base.UpdateAsync(comments, vendorID, userID);
        }
    }
}
