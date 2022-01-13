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
    public class ShareBasesProc<T> : ApplicationDbContextProc<T> where T : ShareBase
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

        protected async Task<StringWithBool> UpdateAsync(int objectID, string currentUserID, List<string> userIDs, T shareObject)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    List<ModelAndState> sharedObjects = new List<ModelAndState>();
                    foreach (var userID in userIDs)
                    {
                        var sharedObjectDB = _context.Set<T>().Where(sr => sr.ObjectID == objectID)
                                               .Where(sr => sr.FromApplicationUserID == currentUserID)
                                               .Where(sr => sr.ToApplicationUserID == userID).FirstOrDefault();
                        if (sharedObjectDB == null)
                        {
                            sharedObjectDB = AppUtility.DeepClone<T>(shareObject);
                            sharedObjectDB.ObjectID = objectID;
                            sharedObjectDB.FromApplicationUserID = currentUserID;
                            sharedObjectDB.ToApplicationUserID = userID;
                            _context.Entry(sharedObjectDB).State = EntityState.Added;
                        }
                        else
                        {
                            sharedObjectDB.TimeStamp = DateTime.Now;
                            _context.Entry(sharedObjectDB).State = EntityState.Modified;
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    ReturnVal.SetStringAndBool(true, null);
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
