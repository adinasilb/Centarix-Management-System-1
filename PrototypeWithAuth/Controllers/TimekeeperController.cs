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
        [Authorize(Roles = "Admin, TimeKeeper")]
        public IActionResult ReportHours()
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.ReportHours;
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
            else if(todaysEntry.Entry2 == null){
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
        [Authorize(Roles = "Admin, TimeKeeper")]
        public IActionResult ReportHours(EntryExitViewModel entryExitViewModel)
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.ReportHours;
            var userid = _userManager.GetUserId(User);
            var todaysEntry = _context.EmployeeHours.Where(eh => eh.Date.Date == DateTime.Today.Date && eh.EmployeeID == userid).FirstOrDefault();
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
            return View(entryExitViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> SummaryDaysOff()
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.Days;
            var userid = _userManager.GetUserId(User);
            var user = _context.Users.OfType<Employee>().Where(u=>u.Id==userid).FirstOrDefault();
            var daysWorking = (DateTime.Today - user.StartedWorking).TotalDays;
            int totalYears = (int)(daysWorking / 365);
            var startOfThisYear = user.StartedWorking.AddYears(totalYears);
            ReportDaysViewModel reportDaysViewModel = new ReportDaysViewModel
            {
                VacationDays = user.VacationDays,
                VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.Date >= startOfThisYear).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date).OrderByDescending(eh => eh.Date),
                SickDaysTaken = _context.EmployeeHours.Where(eh => eh.Date >= startOfThisYear).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).OrderByDescending(eh => eh.Date),
                VacationDaysLeft = getVacationDaysLeft(user)
            };
            return View(reportDaysViewModel);
           
        }

        private int getVacationDaysLeft(Employee user)
        {
            int year = DateTime.Now.Year;
            int vacationLeft = 0;

            while (year >= user.StartedWorking.Year)
            {
                int vacationDays = 0;
                double vacationDaysPerMonth = user.VacationDays / 12;
                if (year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    double vacationDaysBeforeRound = vacationDaysPerMonth * month;
                    vacationDays = (int)Math.Ceiling(vacationDaysBeforeRound);
                }
                else if (year == user.StartedWorking.Year)
                {
                    int month = 12 - user.StartedWorking.Month + 1;
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
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> HoursPage(int month = 0)
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.Hours;
            var hours =GetHours( new DateTime(DateTime.Now.Year, month, DateTime.Now.Day));
            return PartialView(hours);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> SummaryHours()
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.Hours;
            var hours = GetHours(DateTime.Now);
            return PartialView(hours);
        }

        private List<EmployeeHours> GetHours(DateTime monthDate)
        {
            var userid = _userManager.GetUserId(User);
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).FirstOrDefault();
            var hours = _context.EmployeeHours.Include(eh=>eh.OffDayType).Where(eh => eh.EmployeeID == userid).Where(eh => eh.Date.Month == monthDate.Month && eh.Date.Date<=DateTime.Now.Date)
                .OrderByDescending(eh=>eh.Date).ToList();
            return hours;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> ReportDaysOff()
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.DaysOff;
            var userid = _userManager.GetUserId(User);
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).FirstOrDefault();

            List<SummaryOfDaysOffViewModel> daysOffByYear = new List<SummaryOfDaysOffViewModel>();
            int year = DateTime.Now.Year;
            while(year >= user.StartedWorking.Year)
            {
                int vacationDays = 0;
                double vacationDaysPerMonth = user.VacationDays/12;
                if (year == DateTime.Now.Year )
                {
                    int month = DateTime.Now.Month;    
                    double vacationDaysBeforeRound = vacationDaysPerMonth * month;
                    vacationDays = (int)Math.Ceiling(vacationDaysBeforeRound);
                }
                else if(year == user.StartedWorking.Year)
                {
                    int month = 12 - user.StartedWorking.Month+1;
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
            return View(daysOffByYear);

        }

        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> ReportHoursFromHomeModal(DateTime chosenDate)
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

            return PartialView(employeeHoursAwaitingApproval);
           

        }

        [HttpPost]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public IActionResult ReportHoursFromHomeModal(EmployeeHoursAwaitingApproval employeeHoursAwaitingApproval)
        {
            var userID = _userManager.GetUserId(User);
            var employeeHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == userID && eh.Date.Date == employeeHoursAwaitingApproval.Date.Date).FirstOrDefault();
            if (employeeHours != null)
            {
                employeeHoursAwaitingApproval.EmployeeHoursID = employeeHours.EmployeeHoursID;
                employeeHoursAwaitingApproval.OffDayTypeID = employeeHours.OffDayTypeID;
            }
            var awaitingApprovalID = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == userID && eh.Date.Date == employeeHoursAwaitingApproval.Date.Date).Select(e => e.EmployeeHoursAwaitingApprovalID).FirstOrDefault();
            if(awaitingApprovalID != null)
            {
                employeeHoursAwaitingApproval.EmployeeHoursAwaitingApprovalID = awaitingApprovalID;
            }
            employeeHoursAwaitingApproval.EmployeeHoursStatusID = 1;
            _context.Update(employeeHoursAwaitingApproval);
            _context.SaveChanges();
            return Redirect("ReportHours");

        }
        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> UpdateHours(DateTime chosenDate)
        {
            if(chosenDate == new DateTime())
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

            return PartialView(employeeHour);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> UpdateHours(EmployeeHours employeeHours)
        {
            var awaitingApproval = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == employeeHours.EmployeeID && eh.Date.Date == employeeHours.Date.Date).FirstOrDefault();
            int? employeeHoursID = null;
            if(employeeHours.EmployeeHoursID!=0)
            {
                employeeHoursID = employeeHours.EmployeeHoursID;
            }
            EmployeeHoursAwaitingApproval employeeHoursAwaitingApproval = new EmployeeHoursAwaitingApproval();
            if (awaitingApproval == null)
            {
                employeeHoursAwaitingApproval.EmployeeID = employeeHours.EmployeeID;
                employeeHoursAwaitingApproval.EmployeeHoursID = employeeHoursID;
                employeeHoursAwaitingApproval.Entry1 = employeeHours.Entry1;
                employeeHoursAwaitingApproval.Entry2 = employeeHours.Entry2;
                employeeHoursAwaitingApproval.Exit1 = employeeHours.Exit1;
                employeeHoursAwaitingApproval.Exit2 = employeeHours.Exit2;
                employeeHoursAwaitingApproval.TotalHours = employeeHours.TotalHours;
                employeeHoursAwaitingApproval.OffDayTypeID = employeeHours.OffDayTypeID;
                employeeHoursAwaitingApproval.Date = employeeHours.Date;
                employeeHoursAwaitingApproval.EmployeeHoursStatusID = employeeHours.EmployeeHoursStatusID;
            }
            else
            {
                awaitingApproval.Entry1 = employeeHours.Entry1;
                awaitingApproval.Exit1 = employeeHours.Entry2;
                awaitingApproval.Entry2 = employeeHours.Exit1;
                awaitingApproval.Exit2 = employeeHours.Exit2;
                awaitingApproval.TotalHours = employeeHours.TotalHours;
                awaitingApproval.OffDayTypeID = employeeHours.OffDayTypeID;
                employeeHoursAwaitingApproval = awaitingApproval;
            }
           
            _context.Update(employeeHoursAwaitingApproval);
            _context.SaveChanges();
            return Redirect("ReportHours");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> Vacation()
        {
            return PartialView();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> SickDay()
        {
            return PartialView();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
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
        [Authorize(Roles = "Admin, TimeKeeper")]
        public bool SaveVacation(DateTime dateFrom, DateTime dateTo)
        {
            return SaveOffDay(dateFrom, dateTo, 2);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public bool SaveSick(DateTime dateFrom, DateTime dateTo)
        {
            return SaveOffDay(dateFrom, dateTo, 1);
        }

        private bool SaveOffDay(DateTime dateFrom, DateTime dateTo, int offDayTypeID)
        {
            var userID = _userManager.GetUserId(User);
            EmployeeHours employeeHour = null;
            if (dateTo == new DateTime())
            {
                employeeHour = _context.EmployeeHours.Where(eh => eh.Date.Date == dateFrom.Date && eh.EmployeeID == userID).FirstOrDefault();
                if (employeeHour == null)
                {
                    employeeHour = new EmployeeHours
                    {
                        EmployeeID = userID,
                        Date = dateFrom
                    };
                }
                employeeHour.OffDayTypeID = offDayTypeID;
                _context.Update(employeeHour);
                _context.SaveChanges();
                return true;
            }
            else
            {
                var employeeHours = _context.EmployeeHours.Where(eh => (eh.Date.Date >= dateFrom && eh.Date.Date <= dateTo) && eh.EmployeeID == userID);

                while (dateFrom <= dateTo)
                {

                    if (employeeHours.Count() > 0)
                    {
                        employeeHour = employeeHours.Where(eh => eh.Date == dateFrom).FirstOrDefault();
                        if (employeeHour == null)
                        {
                            employeeHour = new EmployeeHours
                            {
                                EmployeeID = userID,
                                Date = dateFrom
                            };
                        }
                        employeeHour.OffDayTypeID = offDayTypeID;
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
                    dateFrom = dateFrom.AddDays(1);
                }
                _context.SaveChanges();
                return true;
            }
        }
    }
}
