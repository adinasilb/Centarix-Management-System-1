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

        public async Task<StringWithBool> UpdateAsync(EmployeeHours EmployeeHour)
        {
            var ReturnVal = new StringWithBool();
            try
            {
                _context.Update(EmployeeHour);;
                await _context.SaveChangesAsync();
               
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }
        public StringWithBool UpdateWithoutSaving(EmployeeHours EmployeeHour)
        {
            var ReturnVal = new StringWithBool();
            try
            {
                _context.Update(EmployeeHour);
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public IQueryable<EmployeeHours> ReadOffDaysByYear(int year, int offDayTypeID, string userId)
        {
            return Read(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.Date.Year == year, eh => eh.EmployeeID == userId, eh => eh.OffDayTypeID == offDayTypeID && eh.Date <= DateTime.Now.Date });
        }

        public async Task<double> ReadPartialOffDayHoursByYearAsync(int year, int partialOffDayTypeID, string UserID)
        {
            var offdays = Read(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.Date.Year == year, eh => eh.EmployeeID == UserID, eh => eh.OffDayTypeID == partialOffDayTypeID && eh.Date <= DateTime.Now.Date });
            var list = await offdays.Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToListAsync();
            return list.Sum(p => p);
        }

        public async Task<StringWithBool> ReportHoursAsync(EntryExitViewModel entryExitViewModel, string userid)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var todaysEntry = await ReadOneAsync(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.Date.Date == DateTime.Today.Date && eh.EmployeeID == userid }, new List<ComplexIncludes<EmployeeHours, ModelBase>> { new ComplexIncludes<EmployeeHours, ModelBase> { Include=eh => eh.OffDayType } });
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
                        _context.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        //entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit1;
                        //entryExitViewModel.Entry = todaysEntry.Entry1;
                    }
                    else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Exit1)
                    {
                        todaysEntry.Exit1 = DateTime.Now;
                        _context.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        //entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Entry2;
                    }
                    else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Entry2)
                    {
                        todaysEntry.Entry2 = DateTime.Now;
                        _context.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        //entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit2;
                        //entryExitViewModel.Entry = todaysEntry.Entry2;

                    }
                    else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Exit2)
                    {
                        todaysEntry.Exit2 = DateTime.Now;
                        _context.Update(todaysEntry);
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
            var notifications = await _timekeeperNotificationsProc.Read(new List<Expression<Func<TimekeeperNotification, bool>>> { n => n.EmployeeHoursID == employeeHoursID }).ToListAsync();
            var dayoff = await _companyDaysOffProc.ReadOneAsync( new List<Expression<Func<CompanyDayOff, bool>>> { co => co.Date.Date == deleteHourViewModel.EmployeeHour.Date.Date });
            var anotherEmployeeHourWithSameDate = await ReadOneAsync( new List<Expression<Func<EmployeeHours, bool>>>{eh => eh.Date.Date == deleteHourViewModel.EmployeeHour.Date.Date && eh.EmployeeID == deleteHourViewModel.EmployeeHour.EmployeeID
                    && eh.EmployeeHoursID !=  deleteHourViewModel.EmployeeHour.EmployeeHoursID });
            var employeeHour = await ReadOneAsync(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.EmployeeHoursID ==  deleteHourViewModel.EmployeeHour.EmployeeHoursID }, new List<ComplexIncludes<EmployeeHours, ModelBase>> { new ComplexIncludes<EmployeeHours, ModelBase> { Include= eh => eh.OffDayType } });
            var ehaaOnEmployeeHour = await _employeeHoursAwaitingApprovalProc.ReadOneAsync(new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { eh => eh.EmployeeHoursID == deleteHourViewModel.EmployeeHour.EmployeeHoursID });
            EmployeeHours newEmployeeHour = null;
            var entries = _context.ChangeTracker.Entries();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if(notifications.Count > 0)
                    {
                        await _timekeeperNotificationsProc.DeleteByEHIDAsync(employeeHoursID);
                    }
                    
                    if (ehaaOnEmployeeHour != null)
                    {
                        await _employeeHoursAwaitingApprovalProc.DeleteByEHIDAsync(employeeHoursID);
                    }

                    if (anotherEmployeeHourWithSameDate == null)
                    {
                        if (employeeHour.OffDayTypeID == 4)
                        {
                            //var employee = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id==employeeHour.EmployeeID });
                            //employee.SpecialDays += 1;
                            await _employeesProc.AddSpecialDays(employeeHour.EmployeeID, 1);
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
                        await _timekeeperNotificationsProc.CreateWithoutTransactionAsync(newNotification);
                        
                    }
                    else
                    {
                        _context.Remove(employeeHour);
                        await _context.SaveChangesAsync();
                    }
                    
                    

                    await transaction.CommitAsync();
                    ReturnVal.SetStringAndBool(true, null);

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ReturnVal.SetStringAndBool(false, "Failed to delete hours - "+AppUtility.GetExceptionMessage(e));
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
                    //var user = await _employeesProc.ReadOneAsync( new List<Expression<Func<Employee, bool>>> { e => e.Id== UserID });
                    var specialDays = 0;
                    if (DateTo == new DateTime()) //just one date
                    {
                        var newone = await _companyDaysOffProc.ReadOneAsync(new List<Expression<Func<CompanyDayOff, bool>>> { companyDaysOff => companyDaysOff.Date.Date == DateFrom.Date });
                        if (newone != null) { companyDaysOff.Add(newone.Date); }
                        if (DateFrom.DayOfWeek != DayOfWeek.Friday && DateFrom.DayOfWeek != DayOfWeek.Saturday && !(companyDaysOff == null))
                        {
                            var ehaa = await _employeeHoursAwaitingApprovalProc.ReadOneAsync(new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { eh => eh.EmployeeID == UserID && eh.Date.Date == DateFrom.Date });
                            employeeHour = await _employeeHoursProc.ReadOneAsync(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.EmployeeID == UserID && eh.Date.Date == DateFrom.Date });

                            if (OffDayTypeID == 4 && employeeHour?.OffDayTypeID != 4)
                            {
                                specialDays+=-1;
                            }
                            else if (employeeHour?.OffDayTypeID == 4 && OffDayTypeID != 4)
                            {
                                specialDays+=1;
                            }
                            if (employeeHour == null)
                            {
                                employeeHour = new EmployeeHours
                                {
                                    EmployeeID = UserID,
                                    Date = DateFrom,
                                    OffDayTypeID = OffDayTypeID,
                                    OffDayType = await _offDayTypesProc.ReadOneAsync(new List<Expression<Func<OffDayType, bool>>> { od => od.OffDayTypeID == OffDayTypeID })
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
                                employeeHour.OffDayType = await _offDayTypesProc.ReadOneAsync(new List<Expression<Func<OffDayType, bool>>> { od => od.OffDayTypeID == OffDayTypeID });
                            }
                            if (!alreadyOffDay)
                            {
                                var changeTracker = _context.ChangeTracker.Entries();
                                _context.Update(employeeHour);
                                _context.SaveChanges();
                            }
                        }
                        if (specialDays!=0)
                        {
                            await _employeesProc.AddSpecialDays(UserID, specialDays);
                        }

                        await transaction.CommitAsync();
                    }
                    else
                    {
                        var employeeHours = Read(new List<Expression<Func<EmployeeHours, bool>>> { eh => (eh.Date.Date >= DateFrom.Date && eh.Date.Date <= DateTo.Date) && eh.EmployeeID == UserID});
                        companyDaysOff = await _companyDaysOffProc.Read( new List<Expression<Func<CompanyDayOff, bool>>> { d => d.Date >= DateFrom && d.Date <= DateTo }).Select(cdf => cdf.Date).ToListAsync();

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
                                        specialDays+=-1;
                                    }
                                    else if (employeeHour?.OffDayTypeID == 4 && OffDayTypeID != 4)
                                    {
                                        specialDays+=1;
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
                        if (specialDays!=0)
                        {
                            await _employeesProc.AddSpecialDays(UserID, specialDays);
                        }
                        await transaction.CommitAsync();
                    }

                    ReturnVal.SetStringAndBool(true, null);
                }
                catch (Exception ex)
                {
                    ReturnVal.SetStringAndBool(false, "Failed to save off day - " +AppUtility.GetExceptionMessage(ex));
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
                    var ehaa = await _employeeHoursAwaitingApprovalProc.ReadOneAsync( new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date});


                    var eh = await ReadOneAsync( new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date, eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID });

                    
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
                        var employee = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id==updateHoursViewModel.EmployeeHour.EmployeeID },
                new List<ComplexIncludes<Employee, ModelBase>> { new ComplexIncludes<Employee, ModelBase> { Include = e => e.SalariedEmployee } });
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

        public async Task<StringWithBool> UpdateApprovedHoursAsync(int approvalHoursID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    EmployeeHours employeeHours = new EmployeeHours();
                    EmployeeHoursAwaitingApproval employeeHoursBeingApproved = await _employeeHoursAwaitingApprovalProc.ReadOneAsync(new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { ehaa => ehaa.EmployeeHoursAwaitingApprovalID == approvalHoursID },
                new List<ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase>> {
                    new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include = ehaa => ehaa.EmployeeHours}
                });
                    employeeHours = await _employeeHoursProc.ReadOneAsync(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.EmployeeHoursID == employeeHoursBeingApproved.EmployeeHoursID });
                    var user = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id == employeeHoursBeingApproved.EmployeeID },
                        new List<ComplexIncludes<Employee, ModelBase>> {
                            new ComplexIncludes<Employee, ModelBase> { Include = e => e.SalariedEmployee}
                        });
                    if (employeeHoursBeingApproved.OffDayTypeID != null)
                    {
                        employeeHoursBeingApproved.OffDayTypeID = null;
                    }
                    else if (employeeHoursBeingApproved.EmployeeHours.OffDayTypeID == 4)
                    {
                        await _employeesProc.AddSpecialDays(user.Id, 1);
                    }
                    if (employeeHours == null)
                    {
                        employeeHours = new EmployeeHours
                        {
                            Entry1 = employeeHoursBeingApproved.Entry1,
                            Entry2 = employeeHoursBeingApproved.Entry2,
                            Exit1 = employeeHoursBeingApproved.Exit1,
                            Exit2 = employeeHoursBeingApproved.Exit2,
                            TotalHours = employeeHoursBeingApproved.TotalHours,
                            EmployeeHoursStatusEntry1ID = employeeHoursBeingApproved.EmployeeHoursStatusEntry1ID,
                            EmployeeHoursStatusEntry2ID = employeeHoursBeingApproved.EmployeeHoursStatusEntry2ID,
                            EmployeeID = employeeHoursBeingApproved.EmployeeID,
                            Date = employeeHoursBeingApproved.Date,
                            EmployeeHoursID = employeeHoursBeingApproved.EmployeeHoursID,
                            PartialOffDayTypeID = employeeHoursBeingApproved.PartialOffDayTypeID,
                            PartialOffDayHours = employeeHoursBeingApproved.PartialOffDayHours,
                            OffDayTypeID = employeeHoursBeingApproved.OffDayTypeID
                        };
                    }
                    else
                    {
                        employeeHours.Entry1 = employeeHoursBeingApproved.Entry1;
                        employeeHours.Entry2 = employeeHoursBeingApproved.Entry2;
                        employeeHours.Exit1 = employeeHoursBeingApproved.Exit1;
                        employeeHours.Exit2 = employeeHoursBeingApproved.Exit2;
                        employeeHours.TotalHours = employeeHoursBeingApproved.TotalHours;
                        employeeHours.EmployeeHoursStatusEntry1ID = employeeHoursBeingApproved.EmployeeHoursStatusEntry1ID;
                        employeeHours.EmployeeHoursStatusEntry2ID = employeeHoursBeingApproved.EmployeeHoursStatusEntry2ID;
                        employeeHours.EmployeeID = employeeHoursBeingApproved.EmployeeID;
                        employeeHours.Date = employeeHoursBeingApproved.Date;
                        employeeHours.EmployeeHoursID = employeeHoursBeingApproved.EmployeeHoursID;
                        employeeHours.PartialOffDayTypeID = employeeHoursBeingApproved.PartialOffDayTypeID;
                        employeeHours.PartialOffDayHours = employeeHoursBeingApproved.PartialOffDayHours;
                        employeeHours.OffDayTypeID = employeeHoursBeingApproved.OffDayTypeID;
                    }
                    _context.Update(employeeHours);
                    await _context.SaveChangesAsync();
                    await _employeeHoursAwaitingApprovalProc.DeleteAsync(employeeHoursBeingApproved.EmployeeHoursAwaitingApprovalID);
                    
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
