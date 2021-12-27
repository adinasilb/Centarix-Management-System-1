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



        public virtual async Task<StringWithBool> DeleteAsync(int objectID, string userID, AppUtility.ModelsEnum modelsEnum)
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
    }
}
