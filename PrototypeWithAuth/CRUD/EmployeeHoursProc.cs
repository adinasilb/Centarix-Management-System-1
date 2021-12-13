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
    public class EmployeeHoursProc : ApplicationDbContextProc<EmployeeHours>
    {
        public EmployeeHoursProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }
        public async Task<EmployeeHours> ReadOneByPKAsync(int? EHID, List<Expression<Func<EmployeeHours, object>>> includes = null)
        {
            var employeehours = _context.EmployeeHours.Where(eh => eh.EmployeeHoursID == EHID).Take(1);
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    employeehours = employeehours.Include(t);
                }
            }
            return await employeehours.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<StringWithBool> Update(EmployeeHours EmployeeHour)
        {
            var ReturnVal = new StringWithBool();
            try
            {
                _context.Update(EmployeeHour);
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public async Task<EmployeeHours> ReadOneByDateAndUserIDAsync(DateTime dateTime, string UserID, List<Expression<Func<EmployeeHours, object>>> includes = null)
        {
            var employeehours = _context.EmployeeHours.Where(eh => eh.Date.Date == dateTime.Date && eh.EmployeeID == UserID).Take(1);

            if (includes != null)
            {
                foreach (var t in includes)
                {
                    employeehours = employeehours.Include(t);
                }
            }
            return await employeehours.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<EmployeeHours> ReadDoubledAsync(DateTime dateTime, string UserID, int EHID)
        {
            return await _context.EmployeeHours.Where(eh => eh.Date.Date == dateTime.Date && eh.EmployeeID == UserID
                    && eh.EmployeeHoursID != EHID).AsNoTracking().FirstOrDefaultAsync();
        }

        public IQueryable<EmployeeHours> ReadOffDaysByYearOffDayTypeIDAndUserID(int year, int offDayTypeID, string userId)
        {
            return _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userId)
                .Where(eh => eh.OffDayTypeID == offDayTypeID && eh.Date <= DateTime.Now.Date).AsNoTracking();
        }
        public IQueryable<EmployeeHours> ReadPartialOffDaysByYearOffDayTypeIDAndUserID(int year, int partialOffDayTypeID, string userId)
        {
            return _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userId)
                .Where(eh => eh.OffDayTypeID == partialOffDayTypeID && eh.Date <= DateTime.Now.Date).AsNoTracking();
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
            var notifications = _timekeeperNotificationsProc.Read(new List<Expression<Func<TimekeeperNotification, bool>>> { n => n.EmployeeHoursID == employeeHoursID });
            var dayoff = await _companyDaysOffProc.ReadOneByDateAsync(deleteHourViewModel.EmployeeHour.Date);
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
                            var employee = await _employeesProc.ReadEmployeeByIDAsync(employeeHour.EmployeeID);
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
                        await _timekeeperNotificationsProc.DeleteByEHIDAsync(employeeHoursID);
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
                    var user = await _employeesProc.ReadEmployeeByIDAsync(UserID);

                    if (DateTo == new DateTime()) //just one date
                    {
                        var newone = await _companyDaysOffProc.ReadOneByDateAsync(DateFrom);
                        if (newone != null) { companyDaysOff.Add(newone.Date); }
                        if (DateFrom.DayOfWeek != DayOfWeek.Friday && DateFrom.DayOfWeek != DayOfWeek.Saturday && !(companyDaysOff == null))
                        {
                            var ehaa = await _employeeHoursAwaitingApprovalProc.ReadOneByUserIDAndDateAsync(UserID, DateFrom);
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
                            await _employeesProc.UpdateAsync(user);
                            if (employeeHour == null)
                            {
                                employeeHour = new EmployeeHours
                                {
                                    EmployeeID = UserID,
                                    Date = DateFrom,
                                    OffDayTypeID = OffDayTypeID,
                                    OffDayType = await _offDayTypesProc.ReadOneByPKAsync(OffDayTypeID)
                                };
                            }
                            else if (employeeHour.Entry1 == null && employeeHour.Entry2 == null && employeeHour.TotalHours == null)
                            {
                                if (employeeHour.OffDayTypeID == OffDayTypeID)
                                {
                                    alreadyOffDay = true;
                                }
                                else
                                {
                                    await _timekeeperNotificationsProc.DeleteByEHIDAsync(employeeHour.EmployeeHoursID);
                                }
                                employeeHour.OffDayTypeID = OffDayTypeID;
                                employeeHour.OffDayType = await _offDayTypesProc.ReadOneByPKAsync(OffDayTypeID);
                            }
                            if (!alreadyOffDay)
                            {
                                 var changeTracker = _context.ChangeTracker.Entries();
                                    _context.Update(employeeHour);
                                    _context.SaveChanges();
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
                                    await _employeesProc.UpdateAsync(user);
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
                                        else
                                        {
                                            await _timekeeperNotificationsProc.DeleteByEHIDAsync(employeeHour.EmployeeHoursID);
                                        }
                                        employeeHour.OffDayTypeID = OffDayTypeID;
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
                                    var changeTracker = _context.ChangeTracker.Entries();
                                    _context.Update(employeeHour);
                                    _context.SaveChanges();
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

        public async Task<StringWithBool> UpdateHoursAsync(UpdateHoursViewModel updateHoursViewModel)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var ehaa = await _employeeHoursAwaitingApprovalProc.ReadOneByUserIDAndDateAsync(updateHoursViewModel.EmployeeHour.EmployeeID, updateHoursViewModel.EmployeeHour.Date.Date);


                    var eh = await ReadOneByDateAndUserIDAsync(updateHoursViewModel.EmployeeHour.Date.Date, updateHoursViewModel.EmployeeHour.EmployeeID);


                    var updateHoursDate = updateHoursViewModel.EmployeeHour.Date;

                    if (ehaa == null)
                    {
                        ehaa = new EmployeeHoursAwaitingApproval();
                    }

                    ehaa.EmployeeID = updateHoursViewModel.EmployeeHour.EmployeeID;

                    if (updateHoursViewModel.EmployeeHour.Entry1 != null)
                    {
                        ehaa.Entry1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry1?.Minute ?? 0, 0);
                    }
                    else
                    {
                        ehaa.Entry1 = null;
                    }
                    if (updateHoursViewModel.EmployeeHour.Entry2 != null)
                    {
                        ehaa.Entry2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry2?.Minute ?? 0, 0);
                    }
                    else
                    {
                        ehaa.Entry2 = null;
                    }
                    if (updateHoursViewModel.EmployeeHour.Exit1 != null)
                    {
                        ehaa.Exit1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit1?.Minute ?? 0, 0);
                    }
                    else
                    {
                        ehaa.Exit1 = null;
                    }
                    if (updateHoursViewModel.EmployeeHour.Exit2 != null)
                    {
                        ehaa.Exit2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit2?.Minute ?? 0, 0);
                    }
                    else
                    {
                        ehaa.Exit2 = null;
                    }
                    ehaa.TotalHours = updateHoursViewModel.EmployeeHour.TotalHours;
                    ehaa.Date = updateHoursViewModel.EmployeeHour.Date;
                    ehaa.EmployeeHoursStatusEntry1ID = updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry1ID;
                    ehaa.EmployeeHoursStatusEntry2ID = updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry2ID;
                    ehaa.PartialOffDayTypeID = updateHoursViewModel.EmployeeHour.PartialOffDayTypeID;
                    if (updateHoursViewModel.EmployeeHour.PartialOffDayTypeID != null && updateHoursViewModel.EmployeeHour.PartialOffDayHours == null)
                    {
                        var employee = await _employeesProc.ReadEmployeeByIDAsync(updateHoursViewModel.EmployeeHour.EmployeeID,
                            new List<Expression<Func<Employee, object>>> { eh => eh.SalariedEmployee });
                        var employeeTime = employee.SalariedEmployee.HoursPerDay;

                        var offDayHours = TimeSpan.FromHours(employeeTime) - updateHoursViewModel.EmployeeHour.TotalHours;
                        if (offDayHours > TimeSpan.Zero)
                        {
                            ehaa.PartialOffDayHours = offDayHours;
                        }
                        else
                        {
                            ehaa.PartialOffDayTypeID = null;
                        }
                    }
                    else
                    {
                        ehaa.PartialOffDayHours = updateHoursViewModel.EmployeeHour.PartialOffDayHours;
                    }
                    ehaa.IsDenied = false;
                    //mark as forgot to report if bool is true and not work from home
                    if (updateHoursViewModel.IsForgotToReport && updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry1ID != 1)
                    {
                        if (eh != null)
                        {                           
                            if (eh.OffDayTypeID == null)
                            {
                                ehaa.EmployeeHoursStatusEntry1ID = 3;
                            }
                        }
                    }
                    if (eh == null)
                    {
                        updateHoursViewModel.EmployeeHour = new EmployeeHours() { Date = updateHoursViewModel.EmployeeHour.Date, EmployeeID = updateHoursViewModel.EmployeeHour.EmployeeID };
                        _context.Update(updateHoursViewModel.EmployeeHour);
                        await _context.SaveChangesAsync();
                    }

                    var employeeHoursID = updateHoursViewModel.EmployeeHour.EmployeeHoursID;
                    ehaa.EmployeeHoursID = employeeHoursID;
                    int Month = ehaa.Date.Month;
                    int Year = ehaa.Date.Year;
                    _context.Update(ehaa);
                    await _context.SaveChangesAsync();

                    var notifications = _timekeeperNotificationsProc.Read(new List<Expression<Func<TimekeeperNotification, bool>>> { n => n.EmployeeHoursID == updateHoursViewModel.EmployeeHour.EmployeeHoursID });

                    foreach (var notification in notifications)
                    {
                        _context.Remove(notification);
                    }
                    await _context.SaveChangesAsync();

                    //throw new Exception();
                    await transaction.CommitAsync();
                    ReturnVal.SetStringAndBool(true, null);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));


                }
            }
            return ReturnVal;
        }
    }
}
