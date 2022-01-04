using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class FavoriteRequestsProc : ApplicationDbContextProc<FavoriteRequest>
    {
        public FavoriteRequestsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                this.InstantiateProcs();
            }
        }


        public async Task<StringWithBool> CreateAsync( int requestID, string userID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var favoriteRequest = new FavoriteRequest()
                    {
                        RequestID = requestID,
                        ApplicationUserID = userID
                    };
                    _context.Add(favoriteRequest);
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

        public async Task<StringWithBool> DeleteAsync(int requestID, string userID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var favoriteRequest = _context.FavoriteRequests
                           .Where(fr => fr.ApplicationUserID == userID)
                           .Where(fr => fr.RequestID == requestID).FirstOrDefault();
                    _context.Remove(favoriteRequest);
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
