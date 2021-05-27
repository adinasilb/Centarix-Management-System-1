using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class TimekeeperController : SharedController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TimekeeperController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager):base(context)
        {            
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public IActionResult ReportHours(string errorMessage = null)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimeKeeperReport;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ReportHours;
            var userid = _userManager.GetUserId(User);
            var todaysEntry = _context.EmployeeHours.Where(eh => eh.Date.Date == DateTime.Today.Date && eh.EmployeeID == userid).FirstOrDefault();
            EntryExitViewModel entryExitViewModel = new EntryExitViewModel();
            if (todaysEntry == null || todaysEntry.Entry1 == null)
            {
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Entry1;
            }
            else if (todaysEntry.Exit1 == null)
            {
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit1;
                entryExitViewModel.Entry = todaysEntry.Entry1;
            }
            else if (todaysEntry.Entry2 == null)
            {
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Entry2;
            }
            else if (todaysEntry.Exit2 == null)
            {
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit2;
                entryExitViewModel.Entry = todaysEntry.Entry2;
            }
            else
            {
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.None;
            }
            if(errorMessage != null)
            {
                entryExitViewModel.ErrorMessage += (errorMessage ?? "");
                Response.StatusCode = 550;
                return PartialView(entryExitViewModel);
            }
            var notifications = _context.TimekeeperNotifications.Where(n => n.ApplicationUserID == userid).Include(n=> n.EmployeeHours).OrderByDescending(n => n.EmployeeHours.Date).Take(25).ToList();
            entryExitViewModel.TimekeeperNotifications = notifications;
            return View(entryExitViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> ReportHours(EntryExitViewModel entryExitViewModel)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimeKeeperReport;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ReportHours;
            var currentClickButton = entryExitViewModel.EntryExitEnum;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userid = _userManager.GetUserId(User);
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
                    // throw new Exception();
                    //var notifications = _context.TimekeeperNotifications.Where(n => n.ApplicationUserID == userid).Include(n => n.EmployeeHours).OrderByDescending(n => n.EmployeeHours.Date).Take(25).ToList();
                    //entryExitViewModel.TimekeeperNotifications = notifications;
                    await transaction.CommitAsync();
                    return RedirectToAction();
                    //return PartialView(entryExitViewModel);
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    return RedirectToAction("ReportHours", new { errorMessage = AppUtility.GetExceptionMessage(ex) });

                }
            }
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SummaryDaysOff()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryDaysOff;
            var userid = _userManager.GetUserId(User);
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).Include(u => u.SalariedEmployee).FirstOrDefault();
            if (user != null)
            {
                ReportDaysViewModel reportDaysViewModel = GetSummaryDaysOffModel(userid, user, DateTime.Now.Year);
                return View(reportDaysViewModel);

            }

            return RedirectToAction("ReportHours");
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _SummaryDaysOff(int year)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryDaysOff;
            var userid = _userManager.GetUserId(User);
            var user = _context.Employees.Where(u => u.Id == userid).Include(u=>u.SalariedEmployee).FirstOrDefault();
            if (user != null)
            {
                ReportDaysViewModel reportDaysViewModel = GetSummaryDaysOffModel(userid, user, year);

                return PartialView(reportDaysViewModel);
            }

            return RedirectToAction("ReportHours");
        }
        private ReportDaysViewModel GetSummaryDaysOffModel(string userid, Employee user, int year)
        {
            int month = DateTime.Now.Month;
            var daysOffViewModel = new ReportDaysViewModel
            {
                VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).OrderByDescending(eh => eh.Date),
                SickDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).OrderByDescending(eh => eh.Date),
                SpecialDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 4 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).OrderByDescending(eh => eh.Date),
                SelectedYear = year
            };
            var sickDaysVacationDaysLeft = getVacationSickDaysLeft(user, year);
            daysOffViewModel.VacationDays = sickDaysVacationDaysLeft.TotalVacationDays;
            daysOffViewModel.SickDays = sickDaysVacationDaysLeft.TotalSickDays;
            daysOffViewModel.VacationDaysTakenCount = sickDaysVacationDaysLeft.VacationDaysTaken;
            daysOffViewModel.SickDaysTakenCount = sickDaysVacationDaysLeft.SickDaysTaken;
            return daysOffViewModel;
        }

        private DaysOffViewModel getYearsVacationSickDays (Employee user, int year, DaysOffViewModel pastYearViewModel)
        {
            double vacationDays = 0;
            double sickDays = 0;
            double vacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).Count();
            double sickDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).Count();
            if (user.EmployeeStatusID == 1)
            {
                var vacationHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay), 2);

                var sickHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                sickDaysTaken = Math.Round(sickDaysTaken + (sickHours / user.SalariedEmployee.HoursPerDay), 2);
            }

            var unpaidLeaveTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 5 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).Count();
            if (year == user.StartedWorking.Year)
            {
                int month = year == DateTime.Now.Year ? (DateTime.Now.Month - user.StartedWorking.Month + 1) : (12 - user.StartedWorking.Month + 1);
                vacationDays = user.VacationDaysPerMonth * month;
                sickDays = user.SickDaysPerMonth * month;
            }
            else if (year == AppUtility.YearStartedTimeKeeper && year == DateTime.Now.Year)
            {
                int month = DateTime.Now.Month;
                vacationDays = (user.VacationDaysPerMonth * month) + user.RollOverVacationDays;
                sickDays = (user.SickDaysPerMonth * month) + user.RollOverSickDays;
            }
            else if (year == AppUtility.YearStartedTimeKeeper)
            {
                int month = DateTime.Now.Month;
                vacationDays = user.VacationDays + user.RollOverVacationDays;
                sickDays = user.SickDays + user.RollOverSickDays;
            }
            else if (year == DateTime.Now.Year)
            {
                int month = DateTime.Now.Month;
                vacationDays = (user.VacationDaysPerMonth * month) + pastYearViewModel.VacationDaysLeft;
                sickDays = (user.SickDaysPerMonth * month) + pastYearViewModel.SickDaysLeft;
            }
            else
            {
                vacationDays = user.VacationDays + pastYearViewModel.VacationDaysLeft;
                sickDays = user.SickDays + pastYearViewModel.SickDaysLeft;
            }
            DaysOffViewModel summaryOfDaysOff = new DaysOffViewModel
            {
                Year = year,
                TotalVacationDays = vacationDays,
                VacationDaysTaken = vacationDaysTaken,
                SickDaysTaken = sickDaysTaken,
                TotalSickDays = sickDays,
                UnpaidLeaveTaken = unpaidLeaveTaken
            };

            return summaryOfDaysOff;
        }
        private DaysOffViewModel getVacationSickDaysLeft(Employee user, int SelectedYear)
        {
            int year = AppUtility.YearStartedTimeKeeper;
            if (user.StartedWorking.Year > AppUtility.YearStartedTimeKeeper)
            {
                year = user.StartedWorking.Year;
            }

            DaysOffViewModel pastYearViewModel = new DaysOffViewModel();

            while (year <= SelectedYear)
            {
                var summaryOfDaysOff = getYearsVacationSickDays(user, year, pastYearViewModel);
                year = year + 1;
                pastYearViewModel = summaryOfDaysOff;
            }
            return pastYearViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> HoursPage(int month = 0, int year = 0, string userId = null, AppUtility.PageTypeEnum pageType = AppUtility.PageTypeEnum.TimekeeperSummary)
        {
            if (userId == null)
            {
                userId = _userManager.GetUserId(User);
            }
            var user = _context.Employees.Where(u => u.Id == userId).Include(e => e.SalariedEmployee).FirstOrDefault();
            SummaryHoursViewModel summaryHoursViewModel = base.SummaryHoursFunction(month, year, user);
            summaryHoursViewModel.PageType = pageType;
            return PartialView(summaryHoursViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SummaryHours(int? Month, int? Year, string errorMessage = null)
        {
            var year = Year ?? DateTime.Now.Year;
            var month = Month ?? DateTime.Now.Month;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryHours;
            var userid = _userManager.GetUserId(User);
            var user = _context.Employees.Where(u => u.Id == userid).Include(e => e.SalariedEmployee).FirstOrDefault();
            //int month = Month?.Month ?? DateTime.Now.Month;
            return PartialView(base.SummaryHoursFunction(month, year, user, errorMessage));
        }     

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _EmployeeHoursAwaitingApproval(int ehaaID)
        {
            var ehaa = _context.EmployeeHoursAwaitingApprovals
                .Include(ehaa => ehaa.EmployeeHours).Include(ehaa => ehaa.PartialOffDayType)
                .Where(ehaa => ehaa.EmployeeHoursAwaitingApprovalID == ehaaID).FirstOrDefault();
            return PartialView(ehaa);
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> ReportDaysOff()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimeKeeperReport;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ReportDaysOff;
            return View(ReportDaysOffFunction());
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _ReportDaysOff(string errorMessage = null)
        {
            return PartialView(ReportDaysOffFunction(errorMessage));
        }

        [Authorize(Roles = "TimeKeeper")]
        private SummaryOfDaysOffViewModel ReportDaysOffFunction(string errorMessage = "")
        {
            var userid = _userManager.GetUserId(User);
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).Include(u=>u.SalariedEmployee).FirstOrDefault(); //TODO: make sure this is only employees


            if (user != null)
            {
                List<DaysOffViewModel> daysOffByYear = new List<DaysOffViewModel>();
                SummaryOfDaysOffViewModel summaryOfDaysOffViewModel = new SummaryOfDaysOffViewModel();
                int year = AppUtility.YearStartedTimeKeeper;
                if (user.StartedWorking.Year > AppUtility.YearStartedTimeKeeper)
                {
                    year = user.StartedWorking.Year;
                }
                var thisYear = DateTime.Now.Year;

                DaysOffViewModel pastYearViewModel = new DaysOffViewModel();

                while (year <= thisYear)
                {
                    var summaryOfDaysOff = getYearsVacationSickDays(user, year, pastYearViewModel);
                    if (year == DateTime.Now.Year)
                    {
                        summaryOfDaysOffViewModel.VacationDaysLeft = summaryOfDaysOff.VacationDaysLeft;
                        summaryOfDaysOffViewModel.SickDaysLeft = summaryOfDaysOff.SickDaysLeft;
                    }
                    daysOffByYear.Add(summaryOfDaysOff);
                    year = year + 1;
                    pastYearViewModel = summaryOfDaysOff;
                  
                }
                summaryOfDaysOffViewModel.DaysOffs = daysOffByYear.OrderByDescending(d=>d.Year);
                summaryOfDaysOffViewModel.TotalVacationDaysPerYear = user.VacationDays;
                summaryOfDaysOffViewModel.TotalSickDaysPerYear = user.SickDays;
                summaryOfDaysOffViewModel.BonusSickDays = user.BonusSickDays;
                summaryOfDaysOffViewModel.BonusVacationDays = user.BonusVacationDays;
                summaryOfDaysOffViewModel.ErrorMessage += errorMessage;
                summaryOfDaysOffViewModel.Employee = user;
                return summaryOfDaysOffViewModel;
            }

            return null;
        }


        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> UpdateHours(DateTime chosenDate, String PageType, bool isWorkFromHome=false)
        {
            return PartialView(await GetUpdateHoursViewModel(chosenDate, PageType, isWorkFromHome));
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _UpdateHours(DateTime chosenDate, String PageType, bool isWorkFromHome = false)
        {            
            return PartialView(await GetUpdateHoursViewModel(chosenDate, PageType, isWorkFromHome));
        }

        private async Task<UpdateHoursViewModel> GetUpdateHoursViewModel(DateTime chosenDate, String PageType, bool isWorkFromHome = false)
        {
            if (chosenDate == new DateTime())
            {
                chosenDate = DateTime.Today;
            }
            var userID = _userManager.GetUserId(User);
            var user = await _context.Employees.Where(u => u.Id == userID).FirstOrDefaultAsync();
            var employeeHour = _context.EmployeeHours.Where(eh => eh.EmployeeID == userID && eh.Date.Date == chosenDate.Date).Include(eh => eh.OffDayType).Include(e => e.Employee).FirstOrDefault();
            if (employeeHour == null)
            {
                employeeHour = new EmployeeHours { EmployeeID = userID, Date = chosenDate, Employee = user };
            }
            employeeHour.EmployeeHoursStatusEntry1 = _context.EmployeeHoursStatuses.Where(ehs => ehs.EmployeeHoursStatusID == employeeHour.EmployeeHoursStatusEntry1ID).FirstOrDefault();
            employeeHour.EmployeeHoursStatusEntry2 = _context.EmployeeHoursStatuses.Where(ehs => ehs.EmployeeHoursStatusID == employeeHour.EmployeeHoursStatusEntry2ID).FirstOrDefault();
            UpdateHoursViewModel updateHoursViewModel = new UpdateHoursViewModel() { EmployeeHour = employeeHour, PageType = PageType };
            if (employeeHour.Entry1 == null && employeeHour.TotalHours == null  && !isWorkFromHome)
            {
                updateHoursViewModel.IsForgotToReport = true;
            }
            if (!isWorkFromHome)
            {
                updateHoursViewModel.AutoFillEntry1Type = 2;
            }
            else
            {
                updateHoursViewModel.AutoFillEntry1Type = 1;
            }
            updateHoursViewModel.PartialOffDayTypes = _context.OffDayTypes.Where(od => od.OffDayTypeID == 1 /*Sick Day*/ || od.OffDayTypeID == 2 /*Vacation Day*/);
            return updateHoursViewModel;
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> UpdateHours(UpdateHoursViewModel updateHoursViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date).FirstOrDefault();

                    var eh = _context.EmployeeHours.Where(eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date).FirstOrDefault();

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
                    ehaa.PartialOffDayHours = updateHoursViewModel.EmployeeHour.PartialOffDayHours;
                    ehaa.IsDenied = false;
                    //mark as forgot to report if bool is true and not work from home
                    if (updateHoursViewModel.IsForgotToReport && updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry1ID != 1)
                    {
                        if (eh != null)
                        {
                            if (eh.IsBonus)
                            {
                                ehaa.IsBonus = true;
                                ehaa.OffDayTypeID = eh.OffDayTypeID;
                            }
                            if(eh.OffDayTypeID ==4)
                            {
                                ehaa.OffDayTypeID = eh.OffDayTypeID;
                            }
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

                    var notifications = _context.TimekeeperNotifications.Where(n => n.EmployeeHoursID == updateHoursViewModel.EmployeeHour.EmployeeHoursID).ToList();
                    foreach(var notification in notifications)
                    {
                        _context.Remove(notification);
                        await _context.SaveChangesAsync();
                    }
                    
                    //throw new Exception();
                    await transaction.CommitAsync();
                    if(updateHoursViewModel.PageType == null || updateHoursViewModel.PageType == "ReportHours")
                    {
                        return RedirectToAction("ReportHours");
                    }
                    return RedirectToAction("SummaryHours", new { Month = Month, Year = Year});
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    updateHoursViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                    updateHoursViewModel.PartialOffDayTypes = _context.OffDayTypes.Where(od => od.OffDayTypeID == 1 /*Sick Day*/ || od.OffDayTypeID == 2 /*Vacation Day*/);
                    var userID = _userManager.GetUserId(User);
                    var user = await _context.Employees.Where(u => u.Id == userID).FirstOrDefaultAsync();
                    updateHoursViewModel.EmployeeHour.Employee = user;
                    var offDayType = await _context.OffDayTypes.Where(odt => odt.OffDayTypeID == updateHoursViewModel.EmployeeHour.OffDayTypeID).FirstOrDefaultAsync();
                    updateHoursViewModel.EmployeeHour.OffDayType = offDayType;
                  
                    Response.StatusCode = 550;
                    return PartialView("UpdateHours", updateHoursViewModel);

                }
            }

            
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> OffDayModal(AppUtility.PageTypeEnum PageType, AppUtility.OffDayTypeEnum OffDayType)
        {
            OffDayViewModel offDayViewModel = new OffDayViewModel()
            {
                OffDayType = OffDayType,
                PageType = PageType
            };
            return PartialView(offDayViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> OffDayModal(OffDayViewModel offDayViewModel)
        {
            string errorMessage = "";
            try
            {
                await SaveOffDay(offDayViewModel.FromDate, offDayViewModel.ToDate, offDayViewModel.OffDayType);
            }
            catch(Exception ex)
            {
                errorMessage = AppUtility.GetExceptionMessage(ex);
            }            
            return RedirectToAction("_ReportDaysOff", new { errorMessage });
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> OffDayConfirmModal(AppUtility.PageTypeEnum PageType, DateTime date, AppUtility.OffDayTypeEnum OffDayType)
        {
            OffDayViewModel offDayViewModel = new OffDayViewModel()
            {
                PageType = PageType,
                OffDayType = OffDayType,
                FromDate = date
            };
            return PartialView(offDayViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> ExitModal()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userID = _userManager.GetUserId(User);
                    var todaysEntry = _context.EmployeeHours.Where(eh => eh.EmployeeID == userID && eh.Date.Date == DateTime.Now.Date).FirstOrDefault();
                    if (todaysEntry.Exit1 == null)
                    {
                        todaysEntry.Exit1 = DateTime.Now;
                        _context.EmployeeHours.Update(todaysEntry);
                        _context.SaveChanges();
                    }

                    else if (todaysEntry.Exit2 == null)
                    {
                        todaysEntry.Exit2 = DateTime.Now;
                        _context.EmployeeHours.Update(todaysEntry);
                        _context.SaveChanges();
                    }
                    //throw new Exception();
                    await transaction.CommitAsync();
                    return PartialView(todaysEntry);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    string errorMessage = AppUtility.GetExceptionMessage(ex);
                    return RedirectToAction("ReportHours", new { errorMessage });
                }
            }
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> OffDayConfirmModal(OffDayViewModel offDayViewModel)
        {
            try
            {
                await SaveOffDay(offDayViewModel.FromDate, new DateTime(), offDayViewModel.OffDayType);
            }
            catch(Exception ex)
            {
                offDayViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
            }
           
            return RedirectToAction("SummaryHours", new { Month = offDayViewModel.Month, Year = offDayViewModel.FromDate.Year, errorMessage = offDayViewModel.ErrorMessage});
        }

        private async Task SaveOffDay(DateTime dateFrom, DateTime dateTo, AppUtility.OffDayTypeEnum offDayType)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userID = _userManager.GetUserId(User);
                    var companyDaysOff = new List<DateTime>();
                    bool alreadyOffDay = false;
                    var offDayTypeID = _context.OffDayTypes.Where(odt => odt.Description == AppUtility.GetDisplayNameOfEnumValue(offDayType.ToString())).Select(odt => odt.OffDayTypeID).FirstOrDefault();
                    EmployeeHours employeeHour = null;
                    var user = _context.Employees.Include(eh => eh.SalariedEmployee).Where(e => e.Id == userID).FirstOrDefault();
                    if (dateTo == new DateTime()) //just one date
                    {
                        companyDaysOff = _context.CompanyDayOffs.Select(cdo => cdo.Date.Date).Where(d => d.Date == dateFrom).ToList();
                        if (dateFrom.DayOfWeek != DayOfWeek.Friday && dateFrom.DayOfWeek != DayOfWeek.Saturday && !companyDaysOff.Contains(dateFrom.Date))
                        {
                            var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == userID && eh.Date.Date == dateFrom).FirstOrDefault();
                       
                            employeeHour = _context.EmployeeHours.Where(eh => eh.Date.Date == dateFrom.Date && eh.EmployeeID == userID).FirstOrDefault();
                            if (offDayTypeID == 4 && employeeHour?.OffDayTypeID != 4)
                            {
                                //employeeHour.Employee = user;
                                //employeeHour.Employee.SpecialDays -= 1;
                                user.SpecialDays -= 1;
                            }
                            else if(employeeHour?.OffDayTypeID == 4 && offDayTypeID != 4)
                            {
                                user.SpecialDays += 1;
                            }
                            if (employeeHour == null)
                            {
                                employeeHour = new EmployeeHours
                                {
                                    EmployeeID = userID,
                                    Date = dateFrom,
                                    OffDayTypeID = offDayTypeID,
                                    OffDayType = _context.OffDayTypes.Where(odt => odt.OffDayTypeID == offDayTypeID).FirstOrDefault()
                                };
                            }
                            else if (employeeHour.Entry1 == null && employeeHour.Entry2 == null && employeeHour.TotalHours == null)
                            {
                                if (employeeHour.OffDayTypeID == offDayTypeID)
                                {
                                    alreadyOffDay = true;
                                }
                                else if (employeeHour.OffDayTypeID != null)
                                {
                                    RemoveEmployeeBonusDay(employeeHour, user);
                                }
                                else
                                {
                                    RemoveNotifications(employeeHour.EmployeeHoursID);
                                }
                                employeeHour.OffDayTypeID = offDayTypeID;
                                
                                employeeHour.IsBonus = false;
                                employeeHour.OffDayType = _context.OffDayTypes.Where(odt => odt.OffDayTypeID == offDayTypeID).FirstOrDefault();
                            }
                            if (!alreadyOffDay)
                            {
                                if(user.BonusSickDays>=1 || user.BonusVacationDays>=1)
                                {
                                    var vacationLeftCount = base.GetUsersOffDaysLeft(user, offDayTypeID, dateFrom.Year);
                                    if (dateFrom.Year != dateTo.Year && dateTo.Year != 1)
                                    {
                                        vacationLeftCount += base.GetUsersOffDaysLeft(user, offDayTypeID, dateTo.Year);
                                    }
                                    if (vacationLeftCount < 1)
                                    {
                                        TakeBonusDay(user, offDayTypeID, employeeHour);
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
                                    _context.Update(employeeHour);
                                    _context.SaveChanges();

                                }
                              
                            }
                        }
                        //throw new Exception();
                        await transaction.CommitAsync();              
                    }
                    else
                    {
                        var employeeHours = _context.EmployeeHours.Where(eh => (eh.Date.Date >= dateFrom && eh.Date.Date <= dateTo) && eh.EmployeeID == userID);
                        companyDaysOff = _context.CompanyDayOffs.Select(cdo => cdo.Date.Date).Where(d => d.Date >= dateFrom && d.Date <= dateTo).ToList();
                        while (dateFrom <= dateTo)
                        {
                           if (dateFrom.DayOfWeek != DayOfWeek.Friday && dateFrom.DayOfWeek != DayOfWeek.Saturday && !companyDaysOff.Contains(dateFrom.Date))
                            {
                               var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == userID && eh.Date.Date == dateFrom).FirstOrDefault();
                                if (employeeHours.Count() > 0)
                                {
                                    employeeHour = employeeHours.Where(eh => eh.Date == dateFrom).FirstOrDefault();
                                    if (offDayTypeID == 4 && employeeHour?.OffDayTypeID != 4)
                                    {
                                        //employeeHour.Employee = user;
                                        //employeeHour.Employee.SpecialDays -= 1;
                                        user.SpecialDays -= 1;
                                    }
                                    else if (employeeHour?.OffDayTypeID == 4 && offDayTypeID != 4)
                                    {
                                        user.SpecialDays += 1;
                                    }
                                    if (employeeHour == null)
                                    {
                                        employeeHour = new EmployeeHours
                                        {
                                            EmployeeID = userID,
                                            Date = dateFrom,
                                            OffDayTypeID = offDayTypeID
                                        };
                                    }
                                    else if (employeeHour.Entry1 == null && employeeHour.Entry2 == null && employeeHour.TotalHours == null)
                                    {
                                        if (employeeHour.OffDayTypeID == offDayTypeID)
                                        {
                                            alreadyOffDay = true;
                                        }
                                        else if (employeeHour.OffDayTypeID != null)
                                        {
                                            RemoveEmployeeBonusDay(employeeHour, user);
                                        }
                                        else
                                        {
                                            RemoveNotifications(employeeHour.EmployeeHoursID);
                                        }
                                        employeeHour.OffDayTypeID = offDayTypeID;
                                        employeeHour.IsBonus = false;
                                    }
                                }
                                else
                                {
                                    employeeHour = new EmployeeHours
                                    {
                                        EmployeeID = userID,
                                        OffDayTypeID = offDayTypeID,
                                        Date = dateFrom
                                    };
                                }
                                
                                if (!alreadyOffDay)
                                {
                                    if(user.BonusSickDays >=1 || user.BonusVacationDays>=1)
                                    {
                                        var vacationLeftCount = base.GetUsersOffDaysLeft(user, offDayTypeID, dateFrom.Year);
                                        if (dateFrom.Year != dateTo.Year && dateTo.Year != 1)
                                        {
                                            vacationLeftCount += base.GetUsersOffDaysLeft(user, offDayTypeID, dateTo.Year);
                                        }
                                        if (vacationLeftCount < 1)
                                        {
                                            TakeBonusDay(user, offDayTypeID, employeeHour);
                                        }
                                        _context.Update(employeeHour);
                                        if (ehaa != null)
                                        {
                                            _context.Remove(ehaa);
                                        }
                                    }
                                    else
                                    {
                                        _context.Update(employeeHour);
                                        _context.SaveChanges();
                                    }
                                    
                                }
                            }
                            dateFrom = dateFrom.AddDays(1);
                            _context.SaveChanges();
                        }
                        //throw new Exception();
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        private void RemoveNotifications(int employeeHoursID)
        {
            var notifications = _context.TimekeeperNotifications.Where(n => n.EmployeeHoursID == employeeHoursID).ToList();
            foreach (TimekeeperNotification n in notifications)
            {
                _context.Remove(n);
                _context.SaveChanges();
            }
        }
        private void RemoveEmployeeBonusDay(EmployeeHours employeeHour, Employee user)
        {
            if (employeeHour.OffDayTypeID == 2 && employeeHour.IsBonus)
            {
                user.BonusVacationDays += 1;
            }
            else if(employeeHour.IsBonus)
            {
                user.BonusSickDays += 1;
            }
            _context.Update(user);
        }
        private void TakeBonusDay(Employee user, int offDayTypeID, EmployeeHours employeeHour)
        {
            if (offDayTypeID == 2)
            {
                if (user.BonusVacationDays >= 1)
                {
                    employeeHour.IsBonus = true;
                    user.BonusVacationDays -= 1;
                    _context.Update(user);
                }
            }
            else
            {
                if (user.BonusSickDays >= 1)
                {
                    employeeHour.IsBonus = true;
                    user.BonusSickDays -= 1;
                    _context.Update(user);
                }
            }
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> DeleteHourModal(int? id, AppUtility.MenuItems sectionType)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Employee Hour not found (no id). Unable to delete.";
                return NotFound();
            }
            var employeeHour = await _context.EmployeeHours.Where(eh => eh.EmployeeHoursID == id).Include(eh => eh.OffDayType).FirstOrDefaultAsync();
            if (employeeHour == null)
            {
                ViewBag.ErrorMessage = "Employee Hour not found. Unable to delete";
                return NotFound();
            }
            var ehaa = await _context.EmployeeHoursAwaitingApprovals.Where(ehaa => ehaa.EmployeeHoursID == id).Include(ehaa => ehaa.PartialOffDayType).FirstOrDefaultAsync();
            DeleteHourViewModel deleteHourViewModel = new DeleteHourViewModel()
            {
                EmployeeHour = employeeHour,
                SectionType = sectionType,
                Ehaa = ehaa
            };

            return PartialView(deleteHourViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> DeleteHourModal(DeleteHourViewModel deleteHourViewModel) //remove ehaa too
        {
            try
            {
                var employeeHoursID = deleteHourViewModel.EmployeeHour.EmployeeHoursID;
                var notifications = _context.TimekeeperNotifications.Where(n => n.EmployeeHoursID == employeeHoursID).ToList();
                var dayoff = _context.CompanyDayOffs.Where(cdo => cdo.Date.Date == deleteHourViewModel.EmployeeHour.Date).FirstOrDefault();
                var anotherEmployeeHourWithSameDate = _context.EmployeeHours.Where(eh => eh.Date.Date == deleteHourViewModel.EmployeeHour.Date.Date && eh.EmployeeID == deleteHourViewModel.EmployeeHour.EmployeeID && eh.EmployeeHoursID != deleteHourViewModel.EmployeeHour.EmployeeHoursID).FirstOrDefault();
                var employeeHour = _context.EmployeeHours.Where(eh => eh.EmployeeHoursID == deleteHourViewModel.EmployeeHour.EmployeeHoursID).AsNoTracking().FirstOrDefault();
                EmployeeHours newEmployeeHour = null;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    { 
                        if (anotherEmployeeHourWithSameDate == null ) {
                            if (employeeHour.OffDayTypeID == 4)
                            {
                                var employee = _context.Employees.Where(e => e.Id == employeeHour.EmployeeID).FirstOrDefault();
                                employee.SpecialDays += 1;
                                _context.Update(employee);
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

                            if(notifications.Count() == 0) //might need to change this if if notifications starts working differently
                            {
                                TimekeeperNotification newNotification = new TimekeeperNotification()
                                {
                                    EmployeeHoursID = employeeHoursID,
                                    IsRead = false,
                                    ApplicationUserID = newEmployeeHour.EmployeeID,
                                    Description = "no hours reported for " + newEmployeeHour.Date.ToString("dd/MM/yyyy"),
                                    NotificationStatusID = 5,
                                    TimeStamp = DateTime.Now,
                                    Controller = "Timekeeper",
                                    Action = "SummaryHours"
                                };
                                _context.Add(newNotification);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            foreach (TimekeeperNotification n in notifications)
                            {
                                _context.Remove(n);
                                await _context.SaveChangesAsync();
                            }

                            _context.Remove(employeeHour);
                            await _context.SaveChangesAsync();
                                                        
                        }
                        var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(ehaa => ehaa.EmployeeHoursID == employeeHoursID).FirstOrDefault();
                        if (ehaa != null)
                        {
                            _context.Remove(ehaa);
                            await _context.SaveChangesAsync();
                        }
                        
                            //throw new Exception();
                            await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
                return RedirectToAction("SummaryHours", 
                    new { Month = deleteHourViewModel.EmployeeHour.Date.Month, Year = deleteHourViewModel.EmployeeHour.Date.Year });
            }
            catch (Exception ex)
            {
                //deleteHourViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                return RedirectToAction("SummaryHours", 
                    new { Month = deleteHourViewModel.EmployeeHour.Date.Month, 
                        Year = deleteHourViewModel.EmployeeHour.Date.Year, errorMessage = AppUtility.GetExceptionMessage(ex) });
            }
        }
    }
}

