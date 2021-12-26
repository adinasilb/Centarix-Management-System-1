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


        public StringWithBool DeleteByRequestListWithoutSaveChanges(RequestList list)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                foreach (var rlr in list.RequestListRequests)
                {
                    _context.Remove(rlr);
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
         
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
                        await DeleteByListIDAndRequestIDsWithoutSaveChangesAsync(listID, requestID);
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

        public async Task<StringWithBool> DeleteByListIDAndRequestIDsWithoutSaveChangesAsync(int listID, int requestID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {

                var list = _context.RequestLists.Where(l => l.ListID == listID)
                    .Include(l => l.RequestListRequests).FirstOrDefault();
                var requestListRequest = list.RequestListRequests.Where(rlr => rlr.RequestID == requestID).FirstOrDefault();
                try
                {
                    _context.Remove(requestListRequest);
                    await _context.SaveChangesAsync();
                    ReturnVal.SetStringAndBool(true, null);
                }
                catch (Exception ex)
                {
                    throw new Exception(AppUtility.GetExceptionMessage(ex));
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }

        public async Task<StringWithBool> MoveListWithoutSaveChanges(int requestToMoveId, int newListID, int prevListID = 0)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
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
                    await _requestListRequestsProc.DeleteByListIDAndRequestIDsWithoutSaveChangesAsync(prevListID, requestToMoveId);
                }
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

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
                        await MoveListWithoutSaveChanges(requestToMoveId, newListID, prevListID);
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
