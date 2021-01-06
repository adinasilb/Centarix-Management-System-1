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
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryDaysOff;
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
            int month = DateTime.Now.Month;
            double vacationDays =  user.VacationDaysPerMonth * month;
            double sickDays = user.SickDays * month;
            
            var daysOffViewModel = new ReportDaysViewModel
            {
                VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).OrderByDescending(eh => eh.Date),
                SickDaysTaken = _context.EmployeeHours.Where(eh => eh.Date.Year == year).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).OrderByDescending(eh => eh.Date),
                SelectedYear = year
            };
            if (year < user.StartedWorking.Year)
            {
                return daysOffViewModel;
            }
            if (year == DateTime.Now.Year && user.StartedWorking.Year!=year)
            {
                daysOffViewModel.VacationDays = (user.VacationDaysPerMonth * month) + user.RollOverVacationDays;
                daysOffViewModel.SickDays = (user.SickDaysPerMonth * month) + user.RollOverSickDays;
            }
            else if(user.StartedWorking.Year == year)
            {
                daysOffViewModel.VacationDays = user.VacationDaysPerMonth * (13 - user.StartedWorking.Month);
                daysOffViewModel.SickDays = user.SickDaysPerMonth * (13 - user.StartedWorking.Month);
            }
            else
            {
                daysOffViewModel.VacationDays = user.VacationDays;
                daysOffViewModel.SickDays = user.SickDays;
            }
            return daysOffViewModel;
        }

        //private double getVacationDaysLeft(Employee user)
        //{
        //    int year = DateTime.Now.Year;
        //    double vacationLeft = 0;

        //    while (year >= user.StartedWorking.Year)
        //    {
        //        double vacationDays = 0;
        //        double vacationDaysPerMonth = user.VacationDays / 12.0;
        //        if (year == user.StartedWorking.Year)
        //        {
        //            int month = 12 - user.StartedWorking.Month + 1; //includes this month, even though month is not finished yet
        //            double vacationDaysBeforeRound = vacationDaysPerMonth * month;
        //            vacationDays = (int)Math.Ceiling(vacationDaysBeforeRound);
        //        }
        //        else if (year == DateTime.Now.Year)
        //        {
        //            int month = DateTime.Now.Month;
        //            double vacationDaysBeforeRound = vacationDaysPerMonth * month;
        //            vacationDays = (int)Math.Ceiling(vacationDaysBeforeRound);
        //        }
        //        else
        //        {
        //            vacationDays = user.VacationDays;
        //        }
        //        DaysOffViewModel summaryOfDaysOff = new DaysOffViewModel
        //        {
        //            Year = year,
        //            TotalVacationDays = vacationDays,
        //            VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Count()

        //        };

        //        year -= 1;
        //        vacationLeft += summaryOfDaysOff.VacationDaysLeft ;
        //    }
        //    return vacationLeft;
        //}

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> HoursPage(int month = 0, int year = 0)
        {
            SummaryHoursViewModel summaryHoursViewModel = SummaryHoursFunction(month, year);
            return PartialView(summaryHoursViewModel);
        }

        private SummaryHoursViewModel SummaryHoursFunction(int month, int year)
        {
            var userid = _userManager.GetUserId(User);
            var user = _context.Employees.Where(u => u.Id == userid).Include(e => e.SalariedEmployee).FirstOrDefault();

            var hours = GetHours(new DateTime(year, month, DateTime.Now.Day));
            var CurMonth = new DateTime(year, month, DateTime.Now.Day);
            double? totalhours;
            if (user.EmployeeStatusID != 1)
            {
                totalhours = null;
            }
            else
            {
                var vacationSickCount = _context.EmployeeHours.Where(eh => eh.Date.Month == month && eh.Date.Year == year && (eh.OffDayTypeID == 2 || eh.OffDayTypeID == 1) && eh.Date <= DateTime.Now.Date).Count();
                totalhours = AppUtility.GetTotalWorkingDaysThisMonth(new DateTime(year, month, 1), _context.CompanyDayOffs, vacationSickCount) * user.SalariedEmployee.HoursPerDay;
            }
            SummaryHoursViewModel summaryHoursViewModel = new SummaryHoursViewModel()
            {
                EmployeeHours = hours,
                CurrentMonth = CurMonth,
                TotalHoursInMonth = totalhours,
                SelectedYear = year,
                TotalHolidaysInMonth = _context.CompanyDayOffs.Where(cdo=> cdo.Date.Year==year && cdo.Date.Month == month ).Count()
            };
            return summaryHoursViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SummaryHours(DateTime? Month)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryHours;
           
            return PartialView(SummaryHoursFunction(DateTime.Now.Month, DateTime.Now.Year));
        }

        private List<EmployeeHoursAndAwaitingApprovalViewModel> GetHours(DateTime monthDate)
        {
            var userid = _userManager.GetUserId(User);
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).FirstOrDefault();
            var hours = _context.EmployeeHours.Include(eh => eh.OffDayType).Include(eh => eh.EmployeeHoursStatusEntry1).Include(eh => eh.CompanyDayOff).ThenInclude(cdo => cdo.CompanyDayOffType).Where(eh => eh.EmployeeID == userid).Where(eh => eh.Date.Month == monthDate.Month && eh.Date.Year == monthDate.Year && eh.Date.Date <= DateTime.Now.Date)
                .OrderByDescending(eh => eh.Date).ToList();

            List<EmployeeHoursAndAwaitingApprovalViewModel> hoursList = new List<EmployeeHoursAndAwaitingApprovalViewModel>();
            foreach (var hour in hours)
            {
                var ehaaavm = new EmployeeHoursAndAwaitingApprovalViewModel()
                {
                    EmployeeHours = hour
                };
                if (_context.EmployeeHoursAwaitingApprovals.Where(ehaa => ehaa.EmployeeID == hour.EmployeeID).Where(ehaa => ehaa.Date == hour.Date).Any())
                {
                    ehaaavm.EmployeeHoursAwaitingApproval = _context.EmployeeHoursAwaitingApprovals
                        .Where(ehaa => ehaa.EmployeeID == hour.EmployeeID).Where(ehaa => ehaa.Date == hour.Date).FirstOrDefault();
                }
                hoursList.Add(ehaaavm);
            }
            return hoursList;
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
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).FirstOrDefault(); //TODO: make sure this is only employees


            if (user != null)
            {
                List<DaysOffViewModel> daysOffByYear = new List<DaysOffViewModel>();
                SummaryOfDaysOffViewModel summaryOfDaysOffViewModel = new SummaryOfDaysOffViewModel();
                int year = DateTime.Now.Year;
                var startYear = AppUtility.YearStartedTimeKeeper;
                if (user.StartedWorking.Year > startYear)
                {
                    startYear = user.StartedWorking.Year;
                }
                while (year >= startYear)
                {
                    double vacationDays = 0;
                    double sickDays = 0;
                    var vacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).Count();
                    var sickDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).Count();
                    if (year == user.StartedWorking.Year)
                    {
                        int month = 12 - user.StartedWorking.Month + 1;
                         vacationDays = user.VacationDaysPerMonth * month;
                         sickDays = user.SickDays * month;
                    }
                    else if (year == DateTime.Now.Year)
                    {
                        int month = DateTime.Now.Month;
                        vacationDays = (user.VacationDaysPerMonth * month)+user.RollOverVacationDays;
                        sickDays = (user.SickDaysPerMonth * month)+user.RollOverSickDays;
                        summaryOfDaysOffViewModel.VacationDaysLeft = vacationDays - vacationDaysTaken;
                        summaryOfDaysOffViewModel.SickDaysLeft = sickDays - sickDaysTaken;
                    }
                    else
                    {
                        vacationDays = user.VacationDays;
                        sickDays = user.SickDays;
                    }
                    DaysOffViewModel summaryOfDaysOff = new DaysOffViewModel
                    {
                        Year = year,
                        TotalVacationDays = vacationDays,
                        VacationDaysTaken = vacationDaysTaken,
                        SickDaysTaken = sickDaysTaken,
                        TotalSickDays = sickDays
                    };
                    daysOffByYear.Add(summaryOfDaysOff);
                    year = year - 1;
                }
                summaryOfDaysOffViewModel.DaysOffs = daysOffByYear;
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
            var employeeHour = _context.EmployeeHours.Where(eh => eh.EmployeeID == userID && eh.Date.Date == chosenDate.Date).Include(eh=>eh.OffDayType).FirstOrDefault();
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
            return PartialView(updateHoursViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> UpdateHours(UpdateHoursViewModel updateHoursViewModel)
        {
            var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date).FirstOrDefault();
            //var ehaa = new EmployeeHoursAwaitingApproval()
            //{
            //    EmployeeHoursAwaitingApprovalID = awaitingApproval.EmployeeHoursAwaitingApprovalID
            //};
            var eh = _context.EmployeeHours.Where(eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date).FirstOrDefault();
           
            var updateHoursDate = updateHoursViewModel.EmployeeHour.Date;
            if (eh == null)
            {
                _context.Update(updateHoursViewModel.EmployeeHour);
                await _context.SaveChangesAsync();
            }
            var employeeHoursID = updateHoursViewModel.EmployeeHour.EmployeeHoursID;
            if (ehaa == null)
            {
                ehaa = new EmployeeHoursAwaitingApproval();
            }
            //if (awaitingApproval == null)
            //{
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
            ehaa.IsDenied = false;
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
            //}
            //else
            //{
            //    if (updateHoursViewModel.EmployeeHour.Entry1 != null)
            //        employeeHoursAwaitingApproval.Entry1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry1?.Minute ?? 0, 0);
            //    if (updateHoursViewModel.EmployeeHour.Entry2 != null)
            //        employeeHoursAwaitingApproval.Entry2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry2?.Minute ?? 0, 0);
            //    if (updateHoursViewModel.EmployeeHour.Exit1 != null)
            //        employeeHoursAwaitingApproval.Exit1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit1?.Minute ?? 0, 0);
            //    if (updateHoursViewModel.EmployeeHour.Exit2 != null)
            //        employeeHoursAwaitingApproval.Exit2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit2?.Minute ?? 0, 0);
            //    awaitingApproval.TotalHours = updateHoursViewModel.EmployeeHour.TotalHours;
            //    awaitingApproval.OffDayTypeID = null;
            //    employeeHoursAwaitingApproval = awaitingApproval;
            //}
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
            return PartialView("SickDayConfirmModal", new SickDayViewModel { PageType = PageType, SelectedDate = date ?? DateTime.Now, NeedsDoctorsNote=checkIfMoreThan2ConsecutiveDays(date) });
        }
        private bool checkIfMoreThan2ConsecutiveDays(DateTime? date)
        {
           // var daysWithinRange = _context.EmployeeHours.Where(eh => eh.Date < date?.AddDays(3) && date > eh.Date.AddDays(-3));

            return true;
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
                    else if (employeeHour.Entry1 == null && employeeHour.Entry2 == null && employeeHour.TotalHours == null)
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

