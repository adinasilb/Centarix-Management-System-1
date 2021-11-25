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
    public class ApplicationUsersProc : ApplicationDbContextProc
    {
        public ApplicationUsersProc (ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base (context, userManager)
        {

        }

        public async Task<Employee> ReadEmployeeByID(string UserID)
        {
            return await _context.Users.OfType<Employee>().Where(u => u.Id == UserID).Include(u => u.SalariedEmployee)
                .AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<StringWithBool> UpdateEmployee(Employee employee)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
                ReturnVal.Bool = true;
            }
            catch (Exception ex)
            {
                ReturnVal.Bool = false;
                ReturnVal.String = AppUtility.GetExceptionMessage(ex);
            }
            return ReturnVal;
        }
    }
}
