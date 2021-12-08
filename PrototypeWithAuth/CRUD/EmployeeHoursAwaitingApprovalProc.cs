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
    public class EmployeeHoursAwaitingApprovalProc : ApplicationDbContextProc
    {
        public EmployeeHoursAwaitingApprovalProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<EmployeeHoursAwaitingApproval> ReadByPKAsync(int? ID, List<Expression<Func<EmployeeHoursAwaitingApproval, object>>> includes = null)
        {
            var employeehours = _context.EmployeeHoursAwaitingApprovals
                .Where(ehaa => ehaa.EmployeeHoursAwaitingApprovalID == ID).Take(1);
            if (includes !=null)
            {
                foreach (var t in includes)
                {
                    employeehours =employeehours.Include(t);
                }
            }
            return await employeehours.AsNoTracking().FirstOrDefaultAsync(); 
        }

        public async Task<EmployeeHoursAwaitingApproval> ReadOneByUserIDAndDateAsync(string UserID, DateTime date, List<Expression<Func<EmployeeHoursAwaitingApproval, object>>> includes = null)
        {

            var employeehours = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == UserID && eh.Date.Date == date.Date)
                .Take(1);
            if (includes !=null)
            {
                foreach (var t in includes)
                {
                    employeehours =employeehours.Include(t);
                }
            }
            return await employeehours.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<StringWithBool> Delete(int ID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(ehaa => ehaa.EmployeeHoursID == ID).FirstOrDefault();
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
