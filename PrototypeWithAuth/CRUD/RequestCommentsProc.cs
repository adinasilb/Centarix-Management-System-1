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
    public class RequestCommentsProc : CommentBasesProc<RequestComment>
    {
        public RequestCommentsProc(ApplicationDbContext context, bool FromBase = false) : base(context, FromBase)
        {
        }


        public override async Task<StringWithBool> UpdateAsync(List<RequestComment> comments, int vendorID, string userID)
        {
            return await base.UpdateAsync(comments, vendorID, userID);
        }
    }
}
