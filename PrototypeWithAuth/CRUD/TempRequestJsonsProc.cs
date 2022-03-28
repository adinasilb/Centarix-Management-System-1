using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class TempRequestJsonsProc : ApplicationDbContextProc<TempRequestJson>
    {
        public TempRequestJsonsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }

        public async Task<StringWithBool> UpdateAsync(Guid guid, RequestIndexObject requestIndexObject, TempRequestListViewModel trlvm, string userID, bool updateSequenceNumber)
        {
            try
            {
                using (var saveItemTransaction = _context.Database.BeginTransaction())
                {
                    await UpdateWithoutTransactionAsync(guid, requestIndexObject, trlvm, userID, updateSequenceNumber);
                    await saveItemTransaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
               
            }
            return new StringWithBool { };
        }

        public async Task<StringWithBool> UpdateWithoutTransactionAsync(Guid guid, RequestIndexObject requestIndexObject, TempRequestListViewModel trlvm, string userID, bool updateSequenceNumber)
        {
            try
            {
                TempRequestJson trj = await CreateTempRequestJson(guid, userID, updateSequenceNumber, trlvm, requestIndexObject);

                await SetTempRequestAsync(trj, trlvm, requestIndexObject);

            }
            catch (Exception ex)
            {
             
            }

            return new StringWithBool { };
        }



        private async Task<TempRequestJson> CreateTempRequestJson(Guid guid, string userID, bool updateSequenceNumber, TempRequestListViewModel trlvm, RequestIndexObject requestIndexObject)
        {
            var tempRequest = GetTempRequest(guid, userID);
            if(tempRequest.Count()==0)
            {

                var baseTrj = new TempRequestJson()
                {
                    GuidID = guid,
                    ApplicationUserID = userID,
                    SequencePosition = 0,
                    IsCurrent = false,
                    IsOriginal = true
                };
                await SetTempRequestAsync(baseTrj, trlvm, requestIndexObject);
            }
            var sequencePosition = tempRequest.Select(t => t.SequencePosition).FirstOrDefault();
            if(updateSequenceNumber)
            {
                return new TempRequestJson()
                {
                    GuidID = guid,
                    ApplicationUserID = userID,
                    SequencePosition = sequencePosition+1,
                    IsCurrent = true,
                    IsOriginal = false
                };
            }
            else
            {
                return tempRequest.FirstOrDefault();
            }
          
        }

        private async Task SetTempRequestAsync(TempRequestJson tempRequestJson, TempRequestListViewModel tempRequestListViewModel, RequestIndexObject requestIndexObject)
        {
            //tempRequestListViewModel.SequencePosition = tempRequestJson.SequencePosition;
            var fullRequestJson = new FullRequestJson()
            {
                TempRequestViewModels = tempRequestListViewModel.TempRequestViewModels,
                RequestIndexObject = requestIndexObject
            };
            tempRequestJson.SerializeViewModel(fullRequestJson);

            _context.Update(tempRequestJson);
            await _context.SaveChangesAsync();
        }


        public async Task<TempRequestJson> RollbackAsync(Guid GUID, int sequencePosition)
        //remove the one that is current if it is NOT the original
        {
            var current = await ReadOneAsync( new List<Expression<Func<TempRequestJson, bool>>> { t => t.GuidID == GUID && t.SequencePosition == sequencePosition });
            var tempRequestJson = _context.ChangeTracker.Entries<TempRequestJson>().FirstOrDefault();
            if(tempRequestJson !=null)
            {
                tempRequestJson.State = EntityState.Detached;
            };
            _context.Remove(current);
            await _context.SaveChangesAsync();
            var oneStepBack = await ReadOneAsync( new List<Expression<Func<TempRequestJson, bool>>> { t => t.GuidID == GUID && t.SequencePosition == sequencePosition - 1 });
            return oneStepBack;
        }

        public async Task<StringWithBool> RemoveAllAsync(Guid GUID, String userID)
        //This will remove ALL --> Do not use it until you are completely done with this GUID
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var allTRJs = _context.TempRequestJsons.Where(t => t.GuidID == GUID && t.ApplicationUserID == userID).ToList();
                allTRJs.ForEach(tempRequestJson => _context.Remove(tempRequestJson));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;           
        }


        public IQueryable<TempRequestJson> GetTempRequest(Guid cookieID, string userID)
        {
            return _context.TempRequestJsons
                .Where(t => t.GuidID == cookieID && t.ApplicationUserID == userID)
                .OrderByDescending(t => t.SequencePosition)
                .Take(1);
        }

        public IQueryable<TempRequestJson> GetTempRequest(Guid cookieID, string userID, int sequencePosition)
        {
            return _context.TempRequestJsons
                .Where(t => t.GuidID == cookieID && t.ApplicationUserID == userID && t.SequencePosition==sequencePosition).Take(1);
        }
    }
}
