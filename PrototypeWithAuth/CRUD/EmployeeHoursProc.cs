using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class EmployeeHoursProc : ApplicationDbContextProcedure
    {
        public EmployeeHoursProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {

        }

        public EmployeeHours ReadOneByDateAndUserID(DateTime dateTime, string UserID)
        {
            return _context.EmployeeHours.Where(eh => eh.Date.Date == dateTime.Date && eh.EmployeeID == UserID).FirstOrDefault();
        }

        public async Task<StringWithBool> ReportHours(EntryExitViewModel entryExitViewModel, string userid)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var todaysEntry = _context.EmployeeHours
                        .Include(eh => eh.OffDayType)
                        .Where(eh => eh.Date.Date == DateTime.Today.Date && eh.EmployeeID == userid).FirstOrDefault();
                    if (todaysEntry != null && todaysEntry.OffDayTypeID != null)
                    {
                        todaysEntry.OffDayTypeID = null;
                        //entryExitViewModel.OffDayRemoved = todaysEntry.OffDayType.Description;
                    }
                    if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Entry1)
                    {
                        if (todaysEntry == null)
                        {
                            EmployeeHours todaysHours = new EmployeeHours { EmployeeID = userid, Entry1 = DateTime.Now, Date = DateTime.Now };
                            todaysEntry = todaysHours;
                        }
                        else
                        {
                            todaysEntry.Entry1 = DateTime.Now;

                        }
                        _context.EmployeeHours.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        //entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit1;
                        //entryExitViewModel.Entry = todaysEntry.Entry1;
                    }
                    else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Exit1)
                    {
                        todaysEntry.Exit1 = DateTime.Now;
                        _context.EmployeeHours.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        //entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Entry2;
                    }
                    else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Entry2)
                    {
                        todaysEntry.Entry2 = DateTime.Now;
                        _context.EmployeeHours.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        //entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit2;
                        //entryExitViewModel.Entry = todaysEntry.Entry2;

                    }
                    else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Exit2)
                    {
                        todaysEntry.Exit2 = DateTime.Now;
                        _context.EmployeeHours.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        //entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.None;
                    }
                    else
                    {
                        //entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.None;
                    }
                    await transaction.CommitAsync();
                    ReturnVal.Bool = true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ReturnVal.String = AppUtility.GetExceptionMessage(ex);
                }
            }
            return ReturnVal;
        }
    }
}
