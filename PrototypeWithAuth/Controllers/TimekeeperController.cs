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
                        entryExitViewModel.OffDayRemoved = todaysEntry.OffDayType.Description;
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
                        entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit1;
                        entryExitViewModel.Entry = todaysEntry.Entry1;
                    }
                    else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Exit1)
                    {
                        todaysEntry.Exit1 = DateTime.Now;
                        _context.EmployeeHours.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Entry2;
                    }
                    else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Entry2)
                    {
                        todaysEntry.Entry2 = DateTime.Now;
                        _context.EmployeeHours.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit2;
                        entryExitViewModel.Entry = todaysEntry.Entry2;

                    }
                    else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Exit2)
                    {
                        todaysEntry.Exit2 = DateTime.Now;
                        _context.EmployeeHours.Update(todaysEntry);
                        await _context.SaveChangesAsync();
                        entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.None;
                    }
                    else
                    {
                        entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.None;
                    }
                   // throw new Exception();
                    await transaction.CommitAsync();
                    return PartialView(entryExitViewModel);
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    entryExitViewModel.ErrorMessage += ex;
                    entryExitViewModel.EntryExitEnum = currentClickButton;
                    return PartialView(entryExitViewModel);

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
            double vacationDays =  user.VacationDaysPerMonth * month;
            double sickDays = user.SickDays * month;
            var daysOffViewModel = new ReportDaysViewModel
            {
                VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).OrderByDescending(eh => eh.Date),
                SickDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).OrderByDescending(eh => eh.Date),
                SelectedYear = year
            };
            var sickDaysVacationDaysLeft = getVacationSickDaysLeft(user, year);
            daysOffViewModel.VacationDays = sickDaysVacationDaysLeft.TotalVacationDays;
            daysOffViewModel.SickDays = sickDaysVacationDaysLeft.TotalSickDays;
            daysOffViewModel.VacationDaysTakenCount = sickDaysVacationDaysLeft.VacationDaysTaken;
            return daysOffViewModel;
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
                double vacationDays = 0;
                double sickDays = 0;
                double vacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).Count();
                if (user.EmployeeStatusID == 1)
                {
                    var vacationHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.IsBonus==false).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                    vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay),2);
                }
                var sickDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).Count();
                if (year == AppUtility.YearStartedTimeKeeper & year == SelectedYear)
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
                else if (year == user.StartedWorking.Year)
                {
                    int month = 12 - user.StartedWorking.Month + 1;
                    vacationDays = user.VacationDaysPerMonth * month;
                    sickDays = user.SickDays * month;
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
                    TotalSickDays = sickDays
                };
                year = year + 1;
                pastYearViewModel = summaryOfDaysOff;
            }
            return pastYearViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> HoursPage(int month = 0, int year = 0, AppUtility.PageTypeEnum pageType = AppUtility.PageTypeEnum.TimekeeperSummary)
        {
            var userid = _userManager.GetUserId(User);
            var user = _context.Employees.Where(u => u.Id == userid).Include(e => e.SalariedEmployee).FirstOrDefault();
            SummaryHoursViewModel summaryHoursViewModel = base.SummaryHoursFunction(month, year, user);
            summaryHoursViewModel.PageType = pageType;
            return PartialView(summaryHoursViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SummaryHours(DateTime? Month, string errorMessage = null)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryHours;
            var userid = _userManager.GetUserId(User);
            var user = _context.Employees.Where(u => u.Id == userid).Include(e => e.SalariedEmployee).FirstOrDefault();
            int month = Month?.Month ?? DateTime.Now.Month;
            return PartialView(base.SummaryHoursFunction(month, DateTime.Now.Year, user, errorMessage));
        }     

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _EmployeeHoursAwaitingApproval(int ehaaID)
        {
            var ehaa = _context.EmployeeHoursAwaitingApprovals
                .Include(ehaa => ehaa.EmployeeHours)
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
                    double vacationDays = 0;
                    double sickDays = 0;
                    double vacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date &&eh.IsBonus==false).Count();
                    if(user.EmployeeStatusID ==  1)
                    {
                        var vacationHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                        vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay),2);
                    }                  
                    var sickDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date && eh.IsBonus==false).Count();
                    if(year==AppUtility.YearStartedTimeKeeper && year==thisYear)
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
                    else if (year == user.StartedWorking.Year)
                    {
                        int month = 12 - user.StartedWorking.Month + 1;
                         vacationDays = user.VacationDaysPerMonth * month;
                         sickDays = user.SickDays * month;
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
                        TotalSickDays = sickDays
                    };
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
            if (employeeHour.Entry1 == null && employeeHour.TotalHours == null && !isWorkFromHome)
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
            updateHoursViewModel.PartialOffDayTypes = _context.OffDayTypes;
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
                    //throw new Exception();
                    await transaction.CommitAsync();
                    return RedirectToAction(updateHoursViewModel.PageType ?? "ReportHours", new { Month = Month, Year = Year});
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    updateHoursViewModel.ErrorMessage += ex.Message;
                    updateHoursViewModel.PartialOffDayTypes = _context.OffDayTypes;
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
                errorMessage = ex.Message;
            }
            var view = "";
            //if (offDayViewModel.PageType.Equals(AppUtility.PageTypeEnum.TimeKeeperReport))
            //{
                view = "_ReportDaysOff";
            //}
            //else if (offDayViewModel.PageType.Equals(AppUtility.PageTypeEnum.TimekeeperSummary))
            //{
            //    view = "SummaryHours";
            //}
            return RedirectToAction(view, new { errorMessage });
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
                    string errorMessage = ex.Message;
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
                offDayViewModel.ErrorMessage += ex.Message;
            }
            var view = "";
            //if (offDayViewModel.PageType.Equals(AppUtility.PageTypeEnum.TimeKeeperReport))
            //{
            //    view = "_ReportDaysOff";
            //}
            //else if (offDayViewModel.PageType.Equals(AppUtility.PageTypeEnum.TimekeeperSummary))
            //{
                view = "SummaryHours";
            //}

            return RedirectToAction(view, new { Month = new DateTime(DateTime.Now.Year, offDayViewModel.Month ?? DateTime.Now.Month, 1), errorMessage = offDayViewModel.ErrorMessage});
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
                    if (dateTo == new DateTime())
                    {
                        if (dateFrom.DayOfWeek != DayOfWeek.Friday && dateFrom.DayOfWeek != DayOfWeek.Saturday && !companyDaysOff.Contains(dateFrom.Date))
                        {
                            var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == userID && eh.Date.Date == dateFrom).FirstOrDefault();
                            companyDaysOff = _context.CompanyDayOffs.Select(cdo => cdo.Date.Date).Where(d => d.Date == dateFrom).ToList();
                            employeeHour = _context.EmployeeHours.Where(eh => eh.Date.Date == dateFrom.Date && eh.EmployeeID == userID).FirstOrDefault();
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
                                employeeHour.OffDayTypeID = offDayTypeID;
                                employeeHour.IsBonus = false;
                                employeeHour.OffDayType = _context.OffDayTypes.Where(odt => odt.OffDayTypeID == offDayTypeID).FirstOrDefault();
                            }
                            if (!alreadyOffDay)
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
        public async Task<IActionResult> Documents()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Documents;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> CompanyAbsences()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.CompanyAbsences;
            return View();
        }


    }
}

