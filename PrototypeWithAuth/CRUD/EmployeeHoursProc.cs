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
        public async Task<EmployeeHours> ReadOneByPK(int? EHID)
        {
            return await _context.EmployeeHours.Where(eh => eh.EmployeeHoursID == EHID)
                .Include(eh => eh.OffDayType).FirstOrDefaultAsync();
        }

        public EmployeeHours ReadOneByDateAndUserID(DateTime dateTime, string UserID)
        {
            return _context.EmployeeHours.Where(eh => eh.Date.Date == dateTime.Date && eh.EmployeeID == UserID).FirstOrDefault();
        }

        public EmployeeHours ReadDoubled(DateTime dateTime, string UserID, int EHID)
        {
            return _context.EmployeeHours.Where(eh => eh.Date.Date == dateTime.Date && eh.EmployeeID == UserID
                    && eh.EmployeeHoursID != EHID).FirstOrDefault();
        }

        public IQueryable<EmployeeHours> ReadOffDaysByYearOffDayTypeIDAndUserID(int year, int offDayTypeID, string userId)
        {
            return _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userId)
                .Where(eh => eh.OffDayTypeID == offDayTypeID && eh.Date <= DateTime.Now.Date && eh.IsBonus == false);
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


        public async Task<StringWithBool> Delete(DeleteHourViewModel deleteHourViewModel)
        {
            StringWithBool ReturnVal = new StringWithBool();

            var employeeHoursID = deleteHourViewModel.EmployeeHour.EmployeeHoursID;
            var notifications = _timekeeperNotificationsProc.ReadByEHID(employeeHoursID);
            var dayoff = _companyDaysOffProc.ReadOneByDate(deleteHourViewModel.EmployeeHour.Date);
            var anotherEmployeeHourWithSameDate = this.ReadDoubled(deleteHourViewModel.EmployeeHour.Date, deleteHourViewModel.EmployeeHour.EmployeeID, deleteHourViewModel.EmployeeHour.EmployeeHoursID);
            var employeeHour = await this.ReadOneByPK(deleteHourViewModel.EmployeeHour.EmployeeHoursID);

            EmployeeHours newEmployeeHour = null;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (anotherEmployeeHourWithSameDate == null)
                    {
                        if (employeeHour.OffDayTypeID == 4)
                        {
                            var employee = _employeesProc.ReadOneByUserID(employeeHour.EmployeeID);
                            employee.SpecialDays += 1;
                            _employeesProc.Update(employee);
                        }

                        newEmployeeHour = new EmployeeHours()
                        {
                            EmployeeHoursID = employeeHoursID,
                            Date = deleteHourViewModel.EmployeeHour.Date,
                            EmployeeID = deleteHourViewModel.EmployeeHour.EmployeeID,
                            CompanyDayOffID = dayoff?.CompanyDayOffID

                        };

                        _context.Entry(newEmployeeHour).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        if (notifications.Count() == 0) //might need to change this if if notifications starts working differently
                        {
                            TimekeeperNotification newNotification = new TimekeeperNotification()
                            {
                                EmployeeHoursID = employeeHoursID,
                                IsRead = false,
                                ApplicationUserID = newEmployeeHour.EmployeeID,
                                Description = "no hours reported for " + newEmployeeHour.Date.GetElixirDateFormat(),
                                NotificationStatusID = 5,
                                TimeStamp = DateTime.Now,
                                Controller = "Timekeeper",
                                Action = "SummaryHours"
                            };
                            await _timekeeperNotificationsProc.Create(newNotification);
                        }
                    }
                    else
                    {
                        _timekeeperNotificationsProc.DeleteWithoutSaving(notifications);
                        await _context.SaveChangesAsync();

                        _context.Remove(employeeHour);
                        await _context.SaveChangesAsync();

                    }
                    await _employeeHoursAwaitingApprovalProc.Delete(employeeHoursID);

                    await transaction.CommitAsync();
                    ReturnVal.SetStringAndBool(true, null);

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(e));
                    //throw e;
                }
                return ReturnVal;
            }
        }
    }
}
