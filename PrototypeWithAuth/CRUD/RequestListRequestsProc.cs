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
    public class RequestListRequestsProc : ApplicationDbContextProc<RequestListRequest>
    {
        public RequestListRequestsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }


        public void DeleteByRequestListWithoutSaveChanges(RequestList list)
        {
            foreach (var rlr in list.RequestListRequests)
            {
                _context.Remove(rlr);
            }
        }

        public async Task<StringWithBool> DeleteByListIDAndRequestIDsAsync(int listID, int requestID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                
                    try
                    {
                        await DeleteByListIDAndRequestIDsWithoutTransactionAsync(listID, requestID);
                        await transaction.CommitAsync();
                        ReturnVal.SetStringAndBool(true, null);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(AppUtility.GetExceptionMessage(ex));
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }

        public async Task DeleteByListIDAndRequestIDsWithoutTransactionAsync(int listID, int requestID)
        {
            var list = _context.RequestLists.Where(l => l.ListID == listID)
                .Include(l => l.RequestListRequests).FirstOrDefault();
            var requestListRequest = list.RequestListRequests.Where(rlr => rlr.RequestID == requestID).FirstOrDefault();
            try
            {
                _context.Remove(requestListRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(AppUtility.GetExceptionMessage(ex));
            }
        }

        public async Task MoveListWithoutTransactionAsync(int requestToMoveId, int newListID, int prevListID = 0)
        {
            var existingListRequest = _context.RequestListRequests.Where(l => l.RequestID == requestToMoveId && l.ListID == newListID).FirstOrDefault();
            if (existingListRequest != null)
            {
                existingListRequest.TimeStamp = DateTime.Now;
                _context.Update(existingListRequest);
            }
            else
            {
                RequestListRequest requestListRequest = new RequestListRequest()
                {
                    RequestID = requestToMoveId,
                    ListID = newListID,
                    TimeStamp = DateTime.Now
                };
                _context.Entry(requestListRequest).State = EntityState.Added;
            }
            if (prevListID != 0)
            {
                await this.DeleteByListIDAndRequestIDsWithoutTransactionAsync(prevListID, requestToMoveId);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<StringWithBool> MoveList(int requestToMoveId, int newListID, int prevListID = 0)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {

                    try
                    {
                        await MoveListWithoutTransactionAsync(requestToMoveId, newListID, prevListID);

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(AppUtility.GetExceptionMessage(ex));
                    }
                    ReturnVal.SetStringAndBool(true, null);
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
