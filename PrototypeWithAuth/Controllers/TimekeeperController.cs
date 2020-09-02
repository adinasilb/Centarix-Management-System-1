using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Utilities;
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
            if(todaysEntry == null)
            {
                return View(AppUtility.EntryExitEnum.Entry);
            }
            else if (todaysEntry.Exit1 == null)
            {
                return View(AppUtility.EntryExitEnum.Exit);
            }
            else if(todaysEntry.Entry2 == null){

                return View(AppUtility.EntryExitEnum.Entry);
            }
            else if (todaysEntry.Exit2 == null)
            {
                return View(AppUtility.EntryExitEnum.Exit);
            }
            else
            {
                return View(AppUtility.EntryExitEnum.None);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public IActionResult ReportHours(AppUtility.EntryExitEnum entryExitEnum)
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.ReportHours;
            var userid = _userManager.GetUserId(User);
            var todaysEntry = _context.EmployeeHours.Where(eh => eh.Date.Date == DateTime.Today.Date && eh.EmployeeID == userid).FirstOrDefault();
            if (todaysEntry == null)
            {
                EmployeeHours todaysHours = new EmployeeHours { EmployeeID = userid, Entry1 = DateTime.Now, Date = DateTime.Now };
                _context.EmployeeHours.Add(todaysHours);
                _context.SaveChanges();
                return View(AppUtility.EntryExitEnum.Exit);
            }
            else if (todaysEntry.Exit1 == null)
            {
                todaysEntry.Exit1 = DateTime.Now;
                _context.EmployeeHours.Update(todaysEntry);
                _context.SaveChanges();
                return View(AppUtility.EntryExitEnum.Entry);
            }
            else if (todaysEntry.Entry2 == null)
            {
                todaysEntry.Entry2 = DateTime.Now;
                _context.EmployeeHours.Update(todaysEntry);
                _context.SaveChanges();
                return View(AppUtility.EntryExitEnum.Exit);
               
            }
            else if (todaysEntry.Exit2 == null)
            {
                todaysEntry.Exit2 = DateTime.Now;
                _context.EmployeeHours.Update(todaysEntry);
                _context.SaveChanges();
                return View(AppUtility.EntryExitEnum.None);
            }
            else
            {
                return View(AppUtility.EntryExitEnum.None);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> Days()
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
                VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.Date >= startOfThisYear).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 2),
                SickDaysTaken = _context.EmployeeHours.Where(eh=>eh.Date>=startOfThisYear).Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 1)
            };
            return View(reportDaysViewModel);
           
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
        public async Task<IActionResult> Hours()
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
            var hours = _context.EmployeeHours.Where(eh => eh.EmployeeID == userid).Where(eh => eh.Date.Month == monthDate.Month).ToList();
            return hours;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> DaysOff(int month = 0)
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.DaysOff;
            return View();

        }

        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> ReportHoursFromHomeModal(string userID)
        {
            EmployeeHoursAwaitingApproval employeeHour = new EmployeeHoursAwaitingApproval { EmployeeID = userID, Date = DateTime.Now};
            return PartialView(employeeHour);

        }

        [HttpPost]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> ReportHoursFromHomeModal(EmployeeHoursAwaitingApproval employeeHours)
        {
            employeeHours.EmployeeHoursStatusID = 1;
            _context.Add(employeeHours);
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
                employeeHour = new EmployeeHours { EmployeeID = userID, Date = DateTime.Now, EmployeeHoursStatusID=3 };
            }

            return PartialView(employeeHour);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> UpdateHours(EmployeeHours employeeHours)
        {
            EmployeeHoursAwaitingApproval employeeHoursAwaitingApproval = new EmployeeHoursAwaitingApproval
            {
                EmployeeID = employeeHours.EmployeeID,
                EmployeeHoursID = employeeHours.EmployeeHoursID,
                EmployeeHoursStatusID = employeeHours.EmployeeHoursStatusID ?? 2,
                Entry1 = employeeHours.Entry1,
                Entry2 = employeeHours.Entry2,
                Exit1 = employeeHours.Exit1,
                Exit2 = employeeHours.Exit2,
                OffDayTypeID = employeeHours.OffDayTypeID
            };
            _context.Update(employeeHoursAwaitingApproval);
            await _context.SaveChangesAsync();
            return Redirect("ReportHours");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> Vacation(string userID)
        {
            EmployeeHours employeeHour = new EmployeeHours { EmployeeID = _userManager.GetUserId(User), Date = DateTime.Now };
            return PartialView(employeeHour);
        }

    }
}
