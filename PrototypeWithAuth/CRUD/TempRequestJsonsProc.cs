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
                TempRequestJson trj = CreateTempRequestJson(guid, userID, updateSequenceNumber);

                await SetTempRequestAsync(trj, trlvm, requestIndexObject);

            }
            catch (Exception ex)
            {
             
            }

            return new StringWithBool { };
        }



        private TempRequestJson CreateTempRequestJson(Guid guid, string userID, bool updateSequenceNumber) //NEW!! --check after
        {
            var tempRequest = GetTempRequest(guid, userID);
            var sequencePosition = tempRequest.Select(t => t.SequencePosition).FirstOrDefault();
            if(updateSequenceNumber)
            {
                return new TempRequestJson()
                {
                    GuidID = guid,
                    ApplicationUserID = userID,
                    SequencePosition =  sequencePosition== null ? 0 : sequencePosition+1,
                    IsCurrent = true,
                    IsOriginal = sequencePosition==0
                };
            }
            else
            {
                return tempRequest.FirstOrDefault();
            }
          
        }

        private async Task SetTempRequestAsync(TempRequestJson tempRequestJson, TempRequestListViewModel tempRequestListViewModel, RequestIndexObject requestIndexObject)
        {
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


    }
}
