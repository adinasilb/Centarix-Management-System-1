using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class CategoryBasesProc<T1> : ApplicationDbContextProc<T1> where T1:CategoryBase
    {
        public CategoryBasesProc(ApplicationDbContext context, bool FromBase = false): base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> UpdateWithoutTransaction(T1 Category, List<CustomField> Details, List<CustomField> Price, List<CustomField> Documents, List<CustomField> Received)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                ListDictionary json = new ListDictionary();
                json.Add("Details", Details);
                json.Add("Price", Price);
                _context.Entry(Category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

    }
}
