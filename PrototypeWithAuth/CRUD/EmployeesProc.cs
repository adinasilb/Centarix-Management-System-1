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
    public class EmployeesProc : ApplicationUsersProc
    {
        public EmployeesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public IQueryable<Employee> Read(List<Expression<Func<Employee, object>>> includes = null)
        {
            var employees = _context.Employees.AsQueryable();
            if (includes !=null)
            {
                foreach (var t in includes)
                {
                    employees = employees.Include(t);
                }
            }
            return _context.Employees.AsNoTracking();
        }

        public async Task<Employee> ReadEmployeeByIDAsync(string UserID, List<Expression<Func<Employee, object>>> includes = null)
        {
            var employee = _context.Employees.Where(e => e.Id == UserID).Take(1);
            if (includes !=null)
            {
                foreach (var t in includes)
                {
                    employee =employee.Include(t);
                }
            }
            return await employee.AsNoTracking().FirstOrDefaultAsync();
        }

        public IQueryable<Employee> GetUsersLoggedInToday()
        {
            return _context.Employees.Where(u => u.LastLogin.Date == DateTime.Today.Date).AsNoTracking().AsQueryable();
        }
     
        public async Task<StringWithBool> UpdateAsync(Employee employee)
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

        public async Task<StringWithBool> RemoveEmployeeBonusDayAsync(EmployeeHours employeeHour, Employee user)
        {
            if (employeeHour.OffDayTypeID == 2 && employeeHour.IsBonus)
            {
                user.BonusVacationDays += 1;
            }
            else if (employeeHour.IsBonus)
            {
                user.BonusSickDays += 1;
            }
            var success = await UpdateAsync(user);
            return success;
        }

        public async Task<StringWithBool> TakeBonusDay(Employee user, int OffDayTypeID, EmployeeHours employeeHour)
        {
            StringWithBool success = new StringWithBool();
            if (OffDayTypeID == 2)
            {
                if (user.BonusVacationDays >= 1)
                {
                    employeeHour.IsBonus = true;
                    user.BonusVacationDays -= 1;
                    success = await UpdateAsync(user);
                }
            }
            else
            {
                if (user.BonusSickDays >= 1)
                {
                    employeeHour.IsBonus = true;
                    user.BonusSickDays -= 1;
                    success = await UpdateAsync(user);
                }
            }
            return success;
        }

        public async Task<double> GetDaysOffCountByUserOffTypeIDYearAsync(Employee User, int OffDayTypeID, int thisYear)
        {
            double offDaysLeft = 0;
            var year = AppUtility.YearStartedTimeKeeper;

            while (year <= thisYear)
            {
                double vacationDays = 0;
                double sickDays = 0;
                double offDaysTaken = _employeeHoursProc.ReadOffDaysByYearOffDayTypeIDAndUserID(year, OffDayTypeID, User.Id).Count();
                if (User.EmployeeStatusID == 1 && OffDayTypeID == 2)
                {
                    var vacationHours =await _employeeHoursProc.ReadPartialOffDayHoursAsync(year, 2, User.Id);
                    offDaysTaken = Math.Round(offDaysTaken + (vacationHours / User.SalariedEmployee.HoursPerDay), 2);
                }
                if (year == AppUtility.YearStartedTimeKeeper && year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = (User.VacationDaysPerMonth * month) + User.RollOverVacationDays;
                    sickDays = (User.SickDaysPerMonth * month) + User.RollOverSickDays;
                }
                else if (year == AppUtility.YearStartedTimeKeeper)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = User.VacationDays + User.RollOverVacationDays;
                    sickDays = User.SickDays + User.RollOverSickDays;
                }
                else if (year == User.StartedWorking.Year)
                {
                    int month = 12 - User.StartedWorking.Month + 1;
                    vacationDays = User.VacationDaysPerMonth * month;
                    sickDays = User.SickDays * month;
                }
                else if (year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = (User.VacationDaysPerMonth * month);
                    sickDays = (User.SickDaysPerMonth * month);
                }
                else
                {
                    vacationDays = User.VacationDays;
                    sickDays = User.SickDays;
                }
                if (OffDayTypeID == 2)
                {
                    offDaysLeft += vacationDays - offDaysTaken;
                }
                else
                {
                    offDaysLeft += sickDays - offDaysTaken;
                }
                year = year + 1;
            }

            return offDaysLeft;
        }

        public async Task<StringWithBool> RemoveBonusDay(EmployeeHours employeeHour, Employee user)
        {
            if (employeeHour.OffDayTypeID == 2 && employeeHour.IsBonus)
            {
                user.BonusVacationDays += 1;
            }
            else if (employeeHour.IsBonus)
            {
                user.BonusSickDays += 1;
            }
            var success = await UpdateAsync(user);
            return success;
        }
    }
}
