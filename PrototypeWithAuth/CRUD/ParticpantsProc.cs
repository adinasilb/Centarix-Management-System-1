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
    public class ParticipantsProc : ApplicationDbContextProc<Participant>
    {
        public ParticipantsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> UpdateAsync(Participant Participant)
        {
            StringWithBool ReturnVal = new StringWithBool();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Update(Participant);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    ReturnVal.SetStringAndBool(true, null);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
                    throw ex;
                }
            }

            return ReturnVal;
        }

        public async Task<StringWithBool> CreateAsync(Participant participant)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    participant.ParticipantStatusID = 1;
                    _context.Add(participant);
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
