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
    public class TimekeeperController : ControllerBase
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
        public IActionResult ReportHours()
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
            return View(entryExitViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public IActionResult ReportHours(EntryExitViewModel entryExitViewModel)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimeKeeperReport;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ReportHours;


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
                _context.SaveChanges();
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit1;
                entryExitViewModel.Entry = todaysEntry.Entry1;
            }
            else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Exit1)
            {
                todaysEntry.Exit1 = DateTime.Now;
                _context.EmployeeHours.Update(todaysEntry);
                _context.SaveChanges();
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Entry2;
            }
            else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Entry2)
            {
                todaysEntry.Entry2 = DateTime.Now;
                _context.EmployeeHours.Update(todaysEntry);
                _context.SaveChanges();
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit2;
                entryExitViewModel.Entry = todaysEntry.Entry2;

            }
            else if (entryExitViewModel.EntryExitEnum == AppUtility.EntryExitEnum.Exit2)
            {
                todaysEntry.Exit2 = DateTime.Now;
                _context.EmployeeHours.Update(todaysEntry);
                _context.SaveChanges();
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.None;
            }
            else
            {
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.None;
            }
            return PartialView(entryExitViewModel);
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
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).Include(u=>u.SalariedEmployee).FirstOrDefault();
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
                VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).OrderByDescending(eh => eh.Date),
                SickDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).OrderByDescending(eh => eh.Date),
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
                double vacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Count();
                if (user.EmployeeStatusID == 1)
                {
                    var vacationHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                    vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay),2);
                }
                var sickDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).Count();
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
        public async Task<IActionResult> SummaryHours(DateTime? Month)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryHours;
            var userid = _userManager.GetUserId(User);
            var user = _context.Employees.Where(u => u.Id == userid).Include(e => e.SalariedEmployee).FirstOrDefault();
            return PartialView(base.SummaryHoursFunction(DateTime.Now.Month, DateTime.Now.Year, user));
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
        public async Task<IActionResult> _ReportDaysOff()
        {
            return PartialView(ReportDaysOffFunction());
        }

        [Authorize(Roles = "TimeKeeper")]
        private SummaryOfDaysOffViewModel ReportDaysOffFunction()
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
                    double vacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Count();
                    if(user.EmployeeStatusID ==  1)
                    {
                        var vacationHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                        vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay),2);
                    }                  
                    var sickDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).Count();
                    if(year==AppUtility.YearStartedTimeKeeper & year==thisYear)
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
              
                return summaryOfDaysOffViewModel;
            }

            return null;
        }


        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> UpdateHours(DateTime chosenDate, String PageType, bool isWorkFromHome=false)
        {
            if (chosenDate == new DateTime())
            {
                chosenDate = DateTime.Today;
            }
            var userID = _userManager.GetUserId(User);
            var employeeHour = _context.EmployeeHours.Where(eh => eh.EmployeeID == userID && eh.Date.Date == chosenDate.Date).Include(eh=>eh.OffDayType).Include(e=>e.Employee).FirstOrDefault();
            if (employeeHour == null)
            {
                employeeHour = new EmployeeHours { EmployeeID = userID, Date = chosenDate };
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
            updateHoursViewModel.PartialOffDayTypes = _context.PartialOffDayTypes;
            return PartialView(updateHoursViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> UpdateHours(UpdateHoursViewModel updateHoursViewModel)
        {
            var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date).FirstOrDefault();

            var eh = _context.EmployeeHours.Where(eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date).FirstOrDefault();
           
            var updateHoursDate = updateHoursViewModel.EmployeeHour.Date;
            if (eh == null)
            {
                _context.Update(new EmployeeHours () { Date = updateHoursViewModel.EmployeeHour.Date, EmployeeID = updateHoursViewModel.EmployeeHour.EmployeeID });
                await _context.SaveChangesAsync();
            }
            var employeeHoursID = updateHoursViewModel.EmployeeHour.EmployeeHoursID;
            if (ehaa == null)
            {
                ehaa = new EmployeeHoursAwaitingApproval();
            }

            ehaa.EmployeeID = updateHoursViewModel.EmployeeHour.EmployeeID;
            ehaa.EmployeeHoursID = employeeHoursID;
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
            ehaa.OffDayTypeID = null;
            ehaa.Date = updateHoursViewModel.EmployeeHour.Date;
            ehaa.EmployeeHoursStatusEntry1ID = updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry1ID;
            ehaa.EmployeeHoursStatusEntry2ID = updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry2ID;
            ehaa.PartialOffDayTypeID = updateHoursViewModel.EmployeeHour.PartialOffDayTypeID;
            ehaa.PartialOffDayHours = updateHoursViewModel.EmployeeHour.PartialOffDayHours;
            
            //mark as forgot to report if bool is true and not work from home
            if (updateHoursViewModel.IsForgotToReport && updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry1ID!=1)
            {
                if (eh != null)
                {
                    if (eh.OffDayTypeID == null)
                    {
                        ehaa.EmployeeHoursStatusEntry1ID = 3;
                    }
                }
                  
            }
            DateTime Month = ehaa.Date;

            _context.Update(ehaa);
            _context.SaveChanges();


            return RedirectToAction(updateHoursViewModel.PageType ?? "ReportHours", new { Month = Month });
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> Vacation(String PageType)
        {
            return PartialView("Vacation", PageType);
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SickDay(String PageType, DateTime? date)
        {
            return PartialView("SickDay", new SickDayViewModel { PageType = PageType, SelectedDate = date ?? DateTime.Now });
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SickDayConfirmModal(String PageType, DateTime? date)
        {            
            return PartialView("SickDayConfirmModal", new SickDayViewModel { PageType = PageType, SelectedDate = date ?? DateTime.Now });
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> VacationDayConfirmModal(String PageType, DateTime? date)
        {
            return PartialView("VacationDayConfirmModal", new SickDayViewModel { PageType = PageType, SelectedDate = date ?? DateTime.Now });
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> ExitModal()
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
            return PartialView(todaysEntry);
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public IActionResult SaveVacation(DateTime dateFrom, DateTime dateTo, String PageType)
        {
            SaveOffDay(dateFrom, dateTo, 2);
            if (PageType.Equals("ReportDaysOff"))
            {
                PageType = "_" + PageType;
            }
            return RedirectToAction(PageType);
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public IActionResult SaveSick(DateTime dateFrom, DateTime dateTo, int? month, String PageType = "")
        {
            SaveOffDay(dateFrom, dateTo, 1);
            if (PageType.Equals("ReportDaysOff"))
            {
                PageType = "_" + PageType;
            }
            return RedirectToAction(PageType, new { Month = new DateTime(DateTime.Now.Year, month ?? DateTime.Now.Month, 1) });
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public IActionResult SickDayConfirmModal(DateTime dateFrom, String PageType, int? month)
        {
            SaveOffDay(dateFrom, new DateTime(), 1);
            return RedirectToAction(PageType, new { Month = new DateTime(DateTime.Now.Year, month ?? DateTime.Now.Month, 1) });
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public IActionResult VacationDayConfirmModal(DateTime dateFrom, String PageType, int? month)
        {
            SaveOffDay(dateFrom, new DateTime(), 2);
            return RedirectToAction(PageType, new { Month = new DateTime(DateTime.Now.Year, month ?? DateTime.Now.Month, 1) });
        }
        private bool SaveOffDay(DateTime dateFrom, DateTime dateTo, int offDayTypeID)
        {
            var userID = _userManager.GetUserId(User);
            var companyDaysOff = new List<DateTime>();
            EmployeeHours employeeHour = null;

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
                        employeeHour.OffDayTypeID = offDayTypeID;
                        employeeHour.OffDayType = _context.OffDayTypes.Where(odt => odt.OffDayTypeID == offDayTypeID).FirstOrDefault();
                    }

                    _context.Update(employeeHour);
                    _context.SaveChanges();
                    if (ehaa != null)
                    {
                        _context.Remove(ehaa);
                        _context.SaveChanges();
                    }
                }
                return true;
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
                                employeeHour.OffDayTypeID = offDayTypeID;
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
                        _context.Update(employeeHour);
                        if (ehaa != null)
                        {
                            _context.Remove(ehaa);
                        }

                    }
                    dateFrom = dateFrom.AddDays(1);
                }
                _context.SaveChanges();
                return true;
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

