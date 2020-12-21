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
    public class TimekeeperController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TimekeeperController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
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
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.TimeKeeperSidebarEnum.ReportHours;
            var userid = _userManager.GetUserId(User);
            var todaysEntry = _context.EmployeeHours.Where(eh => eh.Date.Date == DateTime.Today.Date && eh.EmployeeID == userid).FirstOrDefault();
            EntryExitViewModel entryExitViewModel = new EntryExitViewModel();
            if (todaysEntry == null || todaysEntry.Entry1 == null)
            {
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Entry1;
            }
            else if (todaysEntry.Exit1 == null )
            {
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Exit1;
                entryExitViewModel.Entry = todaysEntry.Entry1;
            }
            else if (todaysEntry.Entry2 == null )
            {
                entryExitViewModel.EntryExitEnum = AppUtility.EntryExitEnum.Entry2;
            }
            else if (todaysEntry.Exit2 == null )
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
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.TimeKeeperSidebarEnum.ReportHours;


            var userid = _userManager.GetUserId(User);
            var todaysEntry = _context.EmployeeHours.Where(eh => eh.Date.Date == DateTime.Today.Date && eh.EmployeeID == userid).FirstOrDefault();
            if (todaysEntry != null)
            {
                todaysEntry.OffDayTypeID = null;
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
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.TimeKeeperPageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.TimeKeeperSidebarEnum.SummaryDaysOff;
            var userid = _userManager.GetUserId(User);
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).FirstOrDefault();
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
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.TimeKeeperPageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.TimeKeeperSidebarEnum.SummaryDaysOff;
            var userid = _userManager.GetUserId(User);
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).FirstOrDefault();
            if (user != null)
            {
                ReportDaysViewModel reportDaysViewModel = GetSummaryDaysOffModel(userid, user, year);
                return PartialView(reportDaysViewModel);
            }

            return RedirectToAction("ReportHours");
        }
        private ReportDaysViewModel GetSummaryDaysOffModel(string userid, Employee user, int year)
        {
            return new ReportDaysViewModel
            {
                VacationDays = user.VacationDays,
                VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).OrderByDescending(eh => eh.Date),
                SickDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).OrderByDescending(eh => eh.Date),
                VacationDaysLeft = getVacationDaysLeft(user),
                SelectedYear = year
            };
        }

        private int getVacationDaysLeft(Employee user)
        {
            int year = DateTime.Now.Year;
            int vacationLeft = 0;

            while (year >= user.StartedWorking.Year)
            {
                int vacationDays = 0;
                double vacationDaysPerMonth = user.VacationDays / 12.0;
                if (year == user.StartedWorking.Year)
                {
                    int month = 12 - user.StartedWorking.Month + 1; //includes this month, even though month is not finished yet
                    double vacationDaysBeforeRound = vacationDaysPerMonth * month;
                    vacationDays = (int)Math.Ceiling(vacationDaysBeforeRound);
                }
                else if (year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    double vacationDaysBeforeRound = vacationDaysPerMonth * month;
                    vacationDays = (int)Math.Ceiling(vacationDaysBeforeRound);
                }
                else
                {
                    vacationDays = user.VacationDays;
                }
                SummaryOfDaysOffViewModel summaryOfDaysOff = new SummaryOfDaysOffViewModel
                {
                    Year = year,
                    TotalVacationDays = vacationDays,
                    VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Count()

                };

                year -= 1;
                vacationLeft += summaryOfDaysOff.VacationDaysLeft;
            }
            return vacationLeft;
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> HoursPage(int month = 0, int year = 0)
        {
            var userid = _userManager.GetUserId(User);
            var user = _context.Employees.Where(u => u.Id == userid).Include(e => e.SalariedEmployee).FirstOrDefault();


            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.TimeKeeperPageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.TimeKeeperSidebarEnum.SummaryHours;
            var hours = GetHours(new DateTime(year, month, DateTime.Now.Day));
            var CurMonth = new DateTime(year, month, DateTime.Now.Day);
            double? totalhours;
            if (user.EmployeeStatusID != 1)
            {
                totalhours = null;
            }
            else
            {
                var vacationSickCount= _context.EmployeeHours.Where(eh => eh.Date.Month == month && eh.Date.Year == year && (eh.OffDayTypeID==2||eh.OffDayTypeID==1) &&eh.Date<=DateTime.Now.Date).Count();
                totalhours = AppUtility.GetTotalWorkingDaysThisMonth(new DateTime(year, month, 1), _context.CompanyDayOffs, vacationSickCount) * user.SalariedEmployee.HoursPerDay;
            }
            SummaryHoursViewModel summaryHoursViewModel = new SummaryHoursViewModel()
            {
                EmployeeHours = hours,
                CurrentMonth = CurMonth,
                TotalHoursInMonth = totalhours,
                SelectedYear = year
            };
            return PartialView(summaryHoursViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SummaryHours(DateTime? Month)
        {
            var userid = _userManager.GetUserId(User);
            var user = _context.Employees.Where(u => u.Id == userid).Include(e=>e.SalariedEmployee).FirstOrDefault();
            
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.TimeKeeperPageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.TimeKeeperSidebarEnum.SummaryHours;
            var hours = GetHours(Month??DateTime.Now);
            var CurMonth = Month ?? DateTime.Today;
            double? totalhours;
            if (user.EmployeeStatusID!= 1)
            {
                totalhours = null;
            }
            else
            {
                var month = (Month?.Month ?? DateTime.Now.Month);
                var vacationSickCount = _context.EmployeeHours.Where(eh => eh.Date.Month == month && eh.Date.Year == DateTime.Now.Year && (eh.OffDayTypeID == 2 || eh.OffDayTypeID == 1) && eh.Date <= DateTime.Now.Date).Count();
                totalhours =AppUtility.GetTotalWorkingDaysThisMonth(new DateTime(DateTime.Now.Year, (Month?.Month ?? DateTime.Now.Month), 1), _context.CompanyDayOffs, vacationSickCount) * user.SalariedEmployee.HoursPerDay;
            }
            SummaryHoursViewModel summaryHoursViewModel = new SummaryHoursViewModel()
            {
                EmployeeHours = hours,
                CurrentMonth = CurMonth,
                TotalHoursInMonth = totalhours,
                SelectedYear = DateTime.Now.Year

            };
            return PartialView(summaryHoursViewModel);
    }

    private List<EmployeeHours> GetHours(DateTime monthDate)
    {
        var userid = _userManager.GetUserId(User);
        var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).FirstOrDefault();
        var hours = _context.EmployeeHours.Include(eh => eh.OffDayType).Include(eh=>eh.EmployeeHoursStatus).Include(eh=>eh.CompanyDayOff).ThenInclude(cdo=>cdo.CompanyDayOffType).Where(eh => eh.EmployeeID == userid).Where(eh => eh.Date.Month == monthDate.Month && eh.Date.Year == monthDate.Year && eh.Date.Date <= DateTime.Now.Date)
            .OrderByDescending(eh => eh.Date).ToList();
        return hours;
    }

    [HttpGet]
    [Authorize(Roles = "TimeKeeper")]
    public async Task<IActionResult> ReportDaysOff()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.TimeKeeperSidebarEnum.ReportDaysOff;
            return View(ReportDaysOffFunction());
        }
    [HttpGet]
    [Authorize(Roles = "TimeKeeper")]
    public async Task<IActionResult> _ReportDaysOff()
    {
        return PartialView(ReportDaysOffFunction());
    }

    private List<SummaryOfDaysOffViewModel> ReportDaysOffFunction()
    {
        var userid = _userManager.GetUserId(User);
        var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).FirstOrDefault(); //TODO: make sure this is only employees


        if (user != null)
        {
            List<SummaryOfDaysOffViewModel> daysOffByYear = new List<SummaryOfDaysOffViewModel>();
            int year = DateTime.Now.Year;
            while (year >= user.StartedWorking.Year)
            {
                int vacationDays = 0;
                double vacationDaysPerMonth = user.VacationDays / 12.0;
                if (year == user.StartedWorking.Year)
                {
                    int month = 12 - user.StartedWorking.Month + 1;
                    double vacationDaysBeforeRound = vacationDaysPerMonth * month;
                    vacationDays = (int)Math.Ceiling(vacationDaysBeforeRound);
                }
                else if (year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    double vacationDaysBeforeRound = vacationDaysPerMonth * month;
                    vacationDays = (int)Math.Ceiling(vacationDaysBeforeRound);
                }
                else
                {
                    vacationDays = 16;
                }
                SummaryOfDaysOffViewModel summaryOfDaysOff = new SummaryOfDaysOffViewModel
                {
                    Year = year,
                    TotalVacationDays = vacationDays,
                    VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Count(),
                    SickDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).Count()
                };
                daysOffByYear.Add(summaryOfDaysOff);
                year = year - 1;
            }

            return  daysOffByYear;
        }

        return null;
    }

        [HttpGet]
    [Authorize(Roles = "TimeKeeper")]
    public async Task<IActionResult> ReportHoursFromHomeModal(DateTime chosenDate, String PageType)
        {
        if (chosenDate == new DateTime())
        {
            chosenDate = DateTime.Today;
        }
        var userID = _userManager.GetUserId(User);
        EmployeeHoursAwaitingApproval employeeHoursAwaitingApproval = new EmployeeHoursAwaitingApproval();
        var employeeHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == userID && eh.Date.Date == chosenDate.Date).FirstOrDefault();
        if (employeeHours == null)
        {
            employeeHoursAwaitingApproval.EmployeeID = userID;
            employeeHoursAwaitingApproval.Date = chosenDate;
        }
        else
        {
            employeeHoursAwaitingApproval.EmployeeID = userID;
            employeeHoursAwaitingApproval.Date = chosenDate;
            employeeHoursAwaitingApproval.Entry1 = employeeHours.Entry1;
            employeeHoursAwaitingApproval.Exit1 = employeeHours.Entry2;
            employeeHoursAwaitingApproval.Entry2 = employeeHours.Exit1;
            employeeHoursAwaitingApproval.Exit2 = employeeHours.Exit2;
            employeeHoursAwaitingApproval.TotalHours = employeeHours.TotalHours;
        }
            ReportHoursFromHomeViewModel reportHoursFromHomeViewModel = new ReportHoursFromHomeViewModel() { EmployeeHour = employeeHoursAwaitingApproval, PageType = PageType };
            return PartialView(reportHoursFromHomeViewModel);


    }

    [HttpPost]
    [Authorize(Roles = "TimeKeeper")]
    public IActionResult ReportHoursFromHomeModal(ReportHoursFromHomeViewModel reportHoursFromHomeViewModel)
    {
        var userID = _userManager.GetUserId(User);
        var employeeHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == userID && eh.Date.Date == reportHoursFromHomeViewModel.EmployeeHour.Date.Date).FirstOrDefault();
        if (employeeHours != null)
        {
                reportHoursFromHomeViewModel.EmployeeHour.EmployeeHoursID = employeeHours.EmployeeHoursID;
                reportHoursFromHomeViewModel.EmployeeHour.OffDayTypeID = employeeHours.OffDayTypeID;
        }
        var awaitingApprovalID = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == userID && eh.Date.Date == reportHoursFromHomeViewModel.EmployeeHour.Date.Date).Select(e => e.EmployeeHoursAwaitingApprovalID).FirstOrDefault();
        if (awaitingApprovalID != null)
        {
                reportHoursFromHomeViewModel.EmployeeHour.EmployeeHoursAwaitingApprovalID = awaitingApprovalID;
        }
            reportHoursFromHomeViewModel.EmployeeHour.EmployeeHoursStatusID = 1;
            DateTime Month = reportHoursFromHomeViewModel.EmployeeHour.Date;
            _context.Update(reportHoursFromHomeViewModel.EmployeeHour);
          _context.SaveChanges();
            return RedirectToAction(reportHoursFromHomeViewModel.PageType, new { Month = Month });

        }
    [HttpGet]
    [Authorize(Roles = "TimeKeeper")]
    public async Task<IActionResult> UpdateHours(DateTime chosenDate, String PageType)
    {
        if (chosenDate == new DateTime())
        {
            chosenDate = DateTime.Today;
        }
        var userID = _userManager.GetUserId(User);
        var employeeHour = _context.EmployeeHours.Where(eh => eh.EmployeeID == userID && eh.Date.Date == chosenDate.Date).FirstOrDefault();
        if (employeeHour == null)
        {
            employeeHour = new EmployeeHours { EmployeeID = userID, Date = chosenDate, EmployeeHoursStatusID = 3 };
        }
        else
        {
            employeeHour.EmployeeHoursStatusID = 2;
        }
            UpdateHoursViewModel updateHoursViewModel = new UpdateHoursViewModel() { EmployeeHour = employeeHour, PageType = PageType };
        return PartialView(updateHoursViewModel);
    }

    [HttpPost]
    [Authorize(Roles = "TimeKeeper")]
    public async Task<IActionResult> UpdateHours(UpdateHoursViewModel updateHoursViewModel)
    {
        var awaitingApproval = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date).FirstOrDefault();
        var updateHoursDate = updateHoursViewModel.EmployeeHour.Date;
            int? employeeHoursID = null;
        if (updateHoursViewModel.EmployeeHour.EmployeeHoursID != 0)
        {
            employeeHoursID = updateHoursViewModel.EmployeeHour.EmployeeHoursID;
        }
        EmployeeHoursAwaitingApproval employeeHoursAwaitingApproval = new EmployeeHoursAwaitingApproval();
        if (awaitingApproval == null)
        {
            employeeHoursAwaitingApproval.EmployeeID = updateHoursViewModel.EmployeeHour.EmployeeID;
            employeeHoursAwaitingApproval.EmployeeHoursID = employeeHoursID;
            if(updateHoursViewModel.EmployeeHour.Entry1!=null)
                employeeHoursAwaitingApproval.Entry1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry1?.Minute ?? 0, 0);
            if (updateHoursViewModel.EmployeeHour.Entry2 != null)
                employeeHoursAwaitingApproval.Entry2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry2?.Minute ?? 0, 0);
            if (updateHoursViewModel.EmployeeHour.Exit1 != null) 
                employeeHoursAwaitingApproval.Exit1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit1?.Minute ?? 0, 0);
            if (updateHoursViewModel.EmployeeHour.Exit2 != null)
                employeeHoursAwaitingApproval.Exit2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit2?.Minute ?? 0, 0);
            employeeHoursAwaitingApproval.TotalHours = updateHoursViewModel.EmployeeHour.TotalHours;
            employeeHoursAwaitingApproval.OffDayTypeID = null;
            employeeHoursAwaitingApproval.Date = updateHoursViewModel.EmployeeHour.Date;
            employeeHoursAwaitingApproval.EmployeeHoursStatusID = updateHoursViewModel.EmployeeHour.EmployeeHoursStatusID;
        }
        else
        {
            if (updateHoursViewModel.EmployeeHour.Entry1 != null)
               employeeHoursAwaitingApproval.Entry1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry1?.Minute ?? 0, 0);
            if (updateHoursViewModel.EmployeeHour.Entry2 != null) 
               employeeHoursAwaitingApproval.Entry2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry2?.Minute ?? 0, 0);
            if (updateHoursViewModel.EmployeeHour.Exit1 != null)
                employeeHoursAwaitingApproval.Exit1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit1?.Minute ?? 0, 0);
            if (updateHoursViewModel.EmployeeHour.Exit2 != null)
                employeeHoursAwaitingApproval.Exit2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit2?.Minute ?? 0, 0);
            awaitingApproval.TotalHours = updateHoursViewModel.EmployeeHour.TotalHours;
            awaitingApproval.OffDayTypeID = null;
            employeeHoursAwaitingApproval = awaitingApproval;
        }
        DateTime Month = employeeHoursAwaitingApproval.Date;

        _context.Update(employeeHoursAwaitingApproval);
        _context.SaveChanges();


        return RedirectToAction(updateHoursViewModel.PageType??"ReportHours", new { Month = Month });
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
        return PartialView("SickDay",  new SickDayViewModel { PageType=PageType, SelectedDate=date??DateTime.Now });
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
    public IActionResult SaveVacation(DateTime dateFrom, DateTime dateTo, String PageType )
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
        return RedirectToAction(PageType,  new { Month = new DateTime(DateTime.Now.Year, month??DateTime.Now.Month, 1 )});
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
                companyDaysOff = _context.CompanyDayOffs.Select(cdo => cdo.Date.Date).Where(d => d.Date == dateFrom).ToList();
                employeeHour = _context.EmployeeHours.Where(eh => eh.Date.Date == dateFrom.Date && eh.EmployeeID == userID).FirstOrDefault();
                if (employeeHour == null)
                {
                    employeeHour = new EmployeeHours
                    {
                        EmployeeID = userID,
                        Date = dateFrom,
                        OffDayTypeID = offDayTypeID
                    };
                }
                 else if(employeeHour.Entry1 ==null && employeeHour.Entry2 == null && employeeHour.TotalHours == null)
                {
                    employeeHour.OffDayTypeID = offDayTypeID;
                }
             
                _context.Update(employeeHour);
                _context.SaveChanges();
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
        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.TimeKeeperPageTypeEnum.TimekeeperSummary;
        TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.TimeKeeperSidebarEnum.Documents;
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "TimeKeeper")]
    public async Task<IActionResult> CompanyAbsences()
    {
        TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.TimeKeeperPageTypeEnum.TimekeeperSummary;
        TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.TimeKeeperSidebarEnum.CompanyAbsences;
        return View();
    }


  }
}

