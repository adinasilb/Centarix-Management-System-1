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
    public class ParentQuotesProc : ApplicationDbContextProc<ParentQuote>
    {
        public ParentQuotesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }

        public async Task<StringWithBool> DeleteAsync(ParentQuote parentQuote)
        {

            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                parentQuote.Requests = _requestsProc.Read( new List<Expression<Func<Request, bool>>> { r => r.ParentQuoteID == parentQuote.ParentQuoteID && r.IsDeleted != true }).ToList();
                //todo figure out the soft delete with child of a parent entity so we could chnage it to 0 or null
                if (parentQuote.Requests.Count() == 0)
                {
                    parentQuote.IsDeleted = true;
                    _context.Update(parentQuote);
                    await _context.SaveChangesAsync();
                }

                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }
    }
}
