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
    public class ShareBasesProc<T> : ApplicationDbContextProc<T> where T:ShareBase
    {
        public ShareBasesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }



        public virtual async Task<StringWithBool> DeleteAsync(int objectID, string userID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var shareRequests = _context.Set<T>().Where(sr => sr.ObjectID == objectID && sr.ToApplicationUserID == userID).AsEnumerable();
                    foreach (var sr in shareRequests)
                    {
                        _context.Remove(sr);
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {                    
                    transaction.Rollback();
                    ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
                }
            };
            return ReturnVal;

        }

        protected async Task<StringWithBool> UpdateAsync(int objectID, string currentUserID, List<string> userIDs, T sharedObject)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var userID in userIDs)
                    {
                        var sharedObjectDB = _context.Set<T>().Where(sr => sr.ObjectID == objectID)
                                               .Where(sr => sr.FromApplicationUserID == currentUserID)
                                               .Where(sr => sr.ToApplicationUserID == userID).FirstOrDefault();
                        if (sharedObjectDB == null)
                        {
                            sharedObject.ObjectID = objectID;
                            sharedObject.FromApplicationUserID = currentUserID;
                            sharedObject.ToApplicationUserID = userID;
                        }
                        else
                        {
                            //sharedRequest.TimeStamp = DateTime.Now;
                        }
                        _context.Update(sharedObject);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
                }
            }
            return ReturnVal;

        }
    }
}
