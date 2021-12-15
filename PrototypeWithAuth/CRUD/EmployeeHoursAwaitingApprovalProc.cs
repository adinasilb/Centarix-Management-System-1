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
    public class EmployeeHoursAwaitingApprovalProc : ApplicationDbContextProc<EmployeeHoursAwaitingApproval>
    {
        public EmployeeHoursAwaitingApprovalProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }


        public async Task<StringWithBool> DeleteAsync(int ID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var ehaa = await _employeeHoursAwaitingApprovalProc.ReadOneAsync( new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { ehaa => ehaa.EmployeeHoursID == ID });
                if (ehaa != null)
                {
                    _context.Remove(ehaa);
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
