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

        public IQueryable<VendorComment> ReadByVendorID(int VendorID, List<Expression<Func<VendorComment, object>>> includes = null)
        {
            var comments = _context.VendorComments
                .Where(vc => vc.ObjectID == VendorID);
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    comments = comments.Include(t);
                }
            }
            return comments.AsNoTracking().AsQueryable();
        }

        public async Task<StringWithBool> UpdateAsync(List<VendorComment> comments, int vendorID, string userID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                if (comments != null)
                {
                    foreach (var vendorComment in comments)
                    {
                        if (!vendorComment.IsDeleted)
                        {
                            vendorComment.ObjectID = vendorID;
                            if (vendorComment.CommentID == 0)
                            {
                                vendorComment.ApplicationUserID = userID;
                                vendorComment.CommentTimeStamp = DateTime.Now;
                                _context.Add(vendorComment);
                            }
                            else
                            {
                                _context.Update(vendorComment);
                            }
                        }
                        else
                        {
                            var vendorCommentDB = _context.VendorComments.Where(c => c.CommentID == vendorComment.CommentID).FirstOrDefault();
                            if (vendorCommentDB != null)
                            {
                                vendorCommentDB.IsDeleted = true;
                                _context.Update(vendorCommentDB);
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
