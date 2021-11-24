using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.CRUD
{
    public class EmployeesProc : ApplicationDbContextProcedure
    {
        public EmployeesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base (context, userManager)
        {

        }

        public IQueryable<Employee> Read()
        {
            return _context.Employees.AsNoTracking().AsQueryable();
        }

        public Employee ReadOneByUserID(string UserID)
        {
            return _context.Employees.Where(u => u.Id == UserID).FirstOrDefault();
        }

        public IQueryable<Employee> GetUsersLoggedInToday()
        {
            return _context.Employees.Where(u => u.LastLogin.Date == DateTime.Today.Date).AsNoTracking().AsQueryable();
        }

        public async Task<StringWithBool> Update(Employee employee)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
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
