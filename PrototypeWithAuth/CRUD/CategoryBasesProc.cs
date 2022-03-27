using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                json.Add(AppUtility.CategorySettingType.Details.ToString(), Details);
                json.Add(AppUtility.CategorySettingType.Price.ToString(), Price);
                json.Add(AppUtility.CategorySettingType.Documents.ToString(), Documents);
                json.Add(AppUtility.CategorySettingType.Received.ToString(), Received);
                Category.CategoryJson = JsonConvert.SerializeObject(json);
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
