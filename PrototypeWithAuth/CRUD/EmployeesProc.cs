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
    public class EmployeesProc :ApplicationDbContextProc<Employee>
    {
        public EmployeesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }
      
     
        public async Task<double> GetOffDaysByYear(Employee User, int OffDayTypeID, int thisYear)
        {
            double offDaysLeft = 0;
            var year = AppUtility.YearStartedTimeKeeper;
         
            while (year <= thisYear)
            {
                double vacationDays = 0;
                double sickDays = 0;
                double offDaysTaken = _employeeHoursProc.ReadOffDaysByYear(year, OffDayTypeID, User.Id).Count();
                if (User.EmployeeStatusID == 1 && OffDayTypeID == 2)
                {
                    var vacationHours =await _employeeHoursProc.ReadPartialOffDayHoursByYearAsync(year, 2, User.Id);
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
    }
}
