using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class EmployeeHoursAwaitingApprovalProc : ApplicationDbContextProc
    {
        public EmployeeHoursAwaitingApprovalProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<EmployeeHoursAwaitingApproval> ReadByPK(int? ID)
        {
            return await _context.EmployeeHoursAwaitingApprovals
                .Include(ehaa => ehaa.EmployeeHours).Include(ehaa => ehaa.PartialOffDayType)
                .Where(ehaa => ehaa.EmployeeHoursAwaitingApprovalID == ID).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<EmployeeHoursAwaitingApproval> ReadOneByUserIDAndDate(string UserID, DateTime date)
        {
            return await _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == UserID && eh.Date.Date == date.Date)
                .AsNoTracking().FirstOrDefaultAsync();
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
