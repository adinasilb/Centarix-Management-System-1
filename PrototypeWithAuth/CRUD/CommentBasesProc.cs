using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.CRUD
{
    public class CommentBasesProc<T> : ApplicationDbContextProc<T> where T:CommentBase
    {
        public CommentBasesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }


        public virtual async Task<StringWithBool> UpdateAsync(List<T> comments, int objectID, string userID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                if (comments != null)
                {
                    foreach (var c in comments)
                    {
                        if (!c.IsDeleted)
                        {
                            c.ObjectID = objectID;
                            if (c.CommentID == 0)
                            {
                                c.ApplicationUserID = userID;
                                c.CommentTimeStamp = DateTime.Now;
                                _context.Add(c);
                            }
                            else
                            {
                                _context.Update(c);
                            }
                        }
                        else
                        {
                            var commentDB = _context.Set<T>().Where(c => c.CommentID == c.CommentID).FirstOrDefault();
                            if (commentDB != null)
                            {
                                commentDB.IsDeleted = true;
                                _context.Update(commentDB);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }

    }
}
