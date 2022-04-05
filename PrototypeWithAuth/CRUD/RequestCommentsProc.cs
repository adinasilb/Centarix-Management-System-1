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


        public override async Task UpdateWithoutTransactionAsync(List<RequestComment> comments, int objectID, string userID)
        {
            await base.UpdateWithoutTransactionAsync(comments, objectID, userID);
        }

        public async Task CopyCommentsAsync(int OldRequestID, int NewRequestID)
        {
            var comments = _context.RequestComments.Where(c => c.ObjectID == OldRequestID).AsNoTracking();
            foreach (var c in comments)
            {
                c.CommentID = 0;
                c.ObjectID = NewRequestID;
                _context.Entry(c).State = EntityState.Added;
            }
            await _context.SaveChangesAsync();
        }


    }
}
