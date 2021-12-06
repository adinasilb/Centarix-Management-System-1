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
    public class EmployeesProc : ApplicationDbContextProc
    {
        public EmployeesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
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

        public async Task<StringWithBool> RemoveEmployeeBonusDay(EmployeeHours employeeHour, Employee user)
        {
            if (employeeHour.OffDayTypeID == 2 && employeeHour.IsBonus)
            {
                user.BonusVacationDays += 1;
            }
            else if (employeeHour.IsBonus)
            {
                user.BonusSickDays += 1;
            }
            var success = await _applicationUsersProc.UpdateEmployee(user);
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
                    success = await _applicationUsersProc.UpdateEmployee(user);
                }
            }
            else
            {
                if (user.BonusSickDays >= 1)
                {
                    employeeHour.IsBonus = true;
                    user.BonusSickDays -= 1;
                    success = await _applicationUsersProc.UpdateEmployee(user);
                }
            }
            return success;
        }

        public double GetDaysOffCountByUserOffTypeIDYear(Employee User, int OffDayTypeID, int thisYear)
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
                    var vacationHours = _employeeHoursProc.ReadPartialOffDayHours(year, 2, User.Id);
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
            var success = await _applicationUsersProc.UpdateEmployee(user);
            return success;
        }
    }
}
