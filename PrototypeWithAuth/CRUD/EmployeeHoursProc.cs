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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class EmployeeHoursProc : ApplicationDbContextProc
    {
        public EmployeeHoursProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }
        public async Task<EmployeeHours> ReadOneByPKAsync(int? EHID, List<Expression<Func<EmployeeHours, object>>> includes = null)
        {
            var employeehours = _context.EmployeeHours.Where(eh => eh.EmployeeHoursID == EHID).Take(1);
            if (includes !=null)
            {
                foreach (var t in includes)
                {
                    employeehours =employeehours.Include(t);
                }
            }          
            return await employeehours.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<EmployeeHours> ReadOneByDateAndUserIDAsync(DateTime dateTime, string UserID, List<Expression<Func<EmployeeHours, object>>> includes = null)
        {
            var employeehours = _context.EmployeeHours.Where(eh => eh.Date.Date == dateTime.Date && eh.EmployeeID == UserID).Take(1);

            if (includes !=null)
            {
                foreach (var t in includes)
                {
                    employeehours =employeehours.Include(t);
                }
            }
            return await employeehours.AsNoTracking().FirstOrDefaultAsync();
        }

        public async  Task<EmployeeHours> ReadDoubledAsync(DateTime dateTime, string UserID, int EHID)
        {
            return await _context.EmployeeHours.Where(eh => eh.Date.Date == dateTime.Date && eh.EmployeeID == UserID
                    && eh.EmployeeHoursID != EHID).AsNoTracking().FirstOrDefaultAsync();
        }

        public IQueryable<EmployeeHours> ReadOffDaysByYearOffDayTypeIDAndUserID(int year, int offDayTypeID, string userId)
        {
            return _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userId)
                .Where(eh => eh.OffDayTypeID == offDayTypeID && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).AsNoTracking();
        }
        public IQueryable<EmployeeHours> ReadPartialOffDaysByYearOffDayTypeIDAndUserID(int year, int partialOffDayTypeID, string userId)
        {
            return _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userId)
                .Where(eh => eh.OffDayTypeID == partialOffDayTypeID && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).AsNoTracking();
        }

        public async Task<double> ReadPartialOffDayHoursAsync(int year, int partialOffDayTypeID, string UserID)
        {
            var offdays = ReadPartialOffDaysByYearOffDayTypeIDAndUserID(year, partialOffDayTypeID, UserID);
            var list = await offdays.Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToListAsync();
            return list.Sum(p => p);
        }

        public IQueryable<EmployeeHours> ReadByDateSpanAndUserID(DateTime DateTo, DateTime DateFrom, string UserID)
        {
            return _context.EmployeeHours.Where(eh => (eh.Date.Date >= DateFrom.Date && eh.Date.Date <= DateTo.Date) && eh.EmployeeID == UserID);
        }

        public async Task<StringWithBool> ReportHoursAsync(EntryExitViewModel entryExitViewModel, string userid)
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


        public async Task<StringWithBool> DeleteAsync(DeleteHourViewModel deleteHourViewModel)
        {
            StringWithBool ReturnVal = new StringWithBool();

            var employeeHoursID = deleteHourViewModel.EmployeeHour.EmployeeHoursID;
            var notifications = _timekeeperNotificationsProc.ReadByEHID(employeeHoursID);
            var dayoff = _companyDaysOffProc.ReadOneByDate(deleteHourViewModel.EmployeeHour.Date);
            var anotherEmployeeHourWithSameDate = await ReadDoubledAsync(deleteHourViewModel.EmployeeHour.Date, deleteHourViewModel.EmployeeHour.EmployeeID, deleteHourViewModel.EmployeeHour.EmployeeHoursID);
            var employeeHour = await ReadOneByPKAsync(deleteHourViewModel.EmployeeHour.EmployeeHoursID, new List<Expression<Func<EmployeeHours, object>>> { eh => eh.OffDayType });
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
                            await _employeesProc.UpdateAsync(employee);
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
                            await _timekeeperNotificationsProc.CreateAsync(newNotification);
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

        public async Task<StringWithBool> SaveOffDayAsync(DateTime DateFrom, DateTime DateTo, int OffDayTypeID, string UserID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var companyDaysOff = new List<DateTime>();
                    bool alreadyOffDay = false;
                    EmployeeHours employeeHour = null;
                    var user = _employeesProc.ReadOneByUserID(UserID);

                    if (DateTo == new DateTime()) //just one date
                    {
                        var newone = _companyDaysOffProc.ReadOneByDate(DateFrom);
                        if (newone != null) { companyDaysOff.Add(newone.Date); }
                        if (DateFrom.DayOfWeek != DayOfWeek.Friday && DateFrom.DayOfWeek != DayOfWeek.Saturday && !(companyDaysOff == null))
                        {
                            var ehaa = _employeeHoursAwaitingApprovalProc.ReadOneByUserIDAndDate(UserID, DateFrom).FirstOrDefault();
                            employeeHour = await ReadOneByDateAndUserIDAsync(DateFrom, UserID);
                            if (OffDayTypeID == 4 && employeeHour?.OffDayTypeID != 4)
                            {
                                //employeeHour.Employee = user;
                                //employeeHour.Employee.SpecialDays -= 1;
                                user.SpecialDays -= 1;
                            }
                            else if (employeeHour?.OffDayTypeID == 4 && OffDayTypeID != 4)
                            {
                                user.SpecialDays += 1;
                            }
                            if (employeeHour == null)
                            {
                                employeeHour = new EmployeeHours
                                {
                                    EmployeeID = UserID,
                                    Date = DateFrom,
                                    OffDayTypeID = OffDayTypeID,
                                    OffDayType = _offDayTypesProc.ReadOneByPK(OffDayTypeID)
                                };
                            }
                            else if (employeeHour.Entry1 == null && employeeHour.Entry2 == null && employeeHour.TotalHours == null)
                            {
                                if (employeeHour.OffDayTypeID == OffDayTypeID)
                                {
                                    alreadyOffDay = true;
                                }
                                else if (employeeHour.OffDayTypeID != null)
                                {
                                    await _employeesProc.RemoveEmployeeBonusDay(employeeHour, user);
                                }
                                else
                                {
                                    _timekeeperNotificationsProc.DeleteWithoutSaving(new List<TimekeeperNotification>() { _timekeeperNotificationsProc.ReadByPK(employeeHour.EmployeeHoursID) });
                                    _context.SaveChanges();
                                    //RemoveNotifications(employeeHour.EmployeeHoursID);
                                }
                                employeeHour.OffDayTypeID = OffDayTypeID;

                                employeeHour.IsBonus = false;
                                employeeHour.OffDayType = _offDayTypesProc.ReadOneByPK(OffDayTypeID);
                            }
                            if (!alreadyOffDay)
                            {
                                if (user.BonusSickDays >= 1 || user.BonusVacationDays >= 1)
                                {
                                    var vacationLeftCount = await _employeesProc.GetDaysOffCountByUserOffTypeIDYearAsync(user, OffDayTypeID, DateFrom.Year);
                                    if (DateFrom.Year != DateTo.Year && DateTo.Year != 1)
                                    {
                                        vacationLeftCount += await _employeesProc.GetDaysOffCountByUserOffTypeIDYearAsync(user, OffDayTypeID, DateFrom.Year);
                                    }
                                    if (vacationLeftCount < 1)
                                    {
                                        _employeesProc.TakeBonusDay(user, OffDayTypeID, employeeHour);
                                    }
                                    _context.Update(employeeHour);
                                    _context.SaveChanges();
                                    if (ehaa != null)
                                    {
                                        _context.Remove(ehaa);
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    var changeTracker = _context.ChangeTracker.Entries();
                                    _context.Update(employeeHour);
                                    _context.SaveChanges();
                                }
                            }
                        }
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        var employeeHours = this.ReadByDateSpanAndUserID(DateTo, DateFrom, UserID);
                        companyDaysOff = _companyDaysOffProc.ReadByDateSpan(DateFrom, DateTo).Select(cdf => cdf.Date).ToList();


                        while (DateFrom <= DateTo)
                        {
                            if (DateFrom.DayOfWeek != DayOfWeek.Friday && DateFrom.DayOfWeek != DayOfWeek.Saturday && !companyDaysOff.Contains(DateFrom.Date))
                            {
                                var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == UserID && eh.Date.Date == DateFrom).FirstOrDefault();
                                if (employeeHours.Count() > 0)
                                {
                                    employeeHour = employeeHours.Where(eh => eh.Date == DateFrom).FirstOrDefault();
                                    if (OffDayTypeID == 4 && employeeHour?.OffDayTypeID != 4)
                                    {
                                        //employeeHour.Employee = user;
                                        //employeeHour.Employee.SpecialDays -= 1;
                                        user.SpecialDays -= 1;
                                    }
                                    else if (employeeHour?.OffDayTypeID == 4 && OffDayTypeID != 4)
                                    {
                                        user.SpecialDays += 1;
                                    }
                                    if (employeeHour == null)
                                    {
                                        employeeHour = new EmployeeHours
                                        {
                                            EmployeeID = UserID,
                                            Date = DateFrom,
                                            OffDayTypeID = OffDayTypeID
                                        };
                                    }
                                    else if (employeeHour.Entry1 == null && employeeHour.Entry2 == null && employeeHour.TotalHours == null)
                                    {
                                        if (employeeHour.OffDayTypeID == OffDayTypeID)
                                        {
                                            alreadyOffDay = true;
                                        }
                                        else if (employeeHour.OffDayTypeID != null)
                                        {
                                            await _employeesProc.RemoveEmployeeBonusDay(employeeHour, user);
                                        }
                                        else
                                        {
                                            _timekeeperNotificationsProc.DeleteWithoutSaving(new List<TimekeeperNotification>() { _timekeeperNotificationsProc.ReadByPK(employeeHour.EmployeeHoursID) }) ;
                                            _context.SaveChanges();
                                            //_timekeeperNotificationsProc.DeleteByEHID(employeeHour.EmployeeHoursID);
                                        }
                                        employeeHour.OffDayTypeID = OffDayTypeID;
                                        employeeHour.IsBonus = false;
                                    }
                                }
                                else
                                {
                                    employeeHour = new EmployeeHours
                                    {
                                        EmployeeID = UserID,
                                        OffDayTypeID = OffDayTypeID,
                                        Date = DateFrom
                                    };
                                }

                                if (!alreadyOffDay)
                                {
                                    if (user.BonusSickDays >= 1 || user.BonusVacationDays >= 1)
                                    {
                                        var vacationLeftCount = await _employeesProc.GetDaysOffCountByUserOffTypeIDYearAsync(user, OffDayTypeID, DateFrom.Year);
                                        if (DateFrom.Year != DateTo.Year && DateTo.Year != 1)
                                        {
                                            vacationLeftCount += await _employeesProc.GetDaysOffCountByUserOffTypeIDYearAsync(user, OffDayTypeID, DateTo.Year);
                                        }
                                        if (vacationLeftCount < 1)
                                        {
                                            await _employeesProc.TakeBonusDay(user, OffDayTypeID, employeeHour);
                                        }
                                        _context.Update(employeeHour);
                                        if (ehaa != null)
                                        {
                                            _context.Remove(ehaa);
                                        }
                                    }
                                    else
                                    {
                                        var changeTracker = _context.ChangeTracker.Entries();
                                        _context.Update(employeeHour);
                                        _context.SaveChanges();
                                    }

                                }
                            }
                            DateFrom = DateFrom.AddDays(1);
                            _context.SaveChanges();
                        }
                        //throw new Exception();
                        await transaction.CommitAsync();
                    }

                    ReturnVal.SetStringAndBool(true, null);
                }
                catch (Exception ex)
                {
                    ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
                }
            }
            return ReturnVal;
        }
    }
}
