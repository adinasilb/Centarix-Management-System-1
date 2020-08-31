using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.CryptoPro;
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
            var todaysEntry = _context.EmployeeHours.Where(eh => eh.Entry1.Date == DateTime.Today.Date && eh.EmployeeID == userid).FirstOrDefault();
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
            ReportDaysViewModel reportDaysViewModel = new ReportDaysViewModel
            {
                VacationDays = user.VacationDays,
                VacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 2),
                SickDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == userid).Where(eh => eh.OffDayTypeID == 1)
            };
            return View(reportDaysViewModel);
           
        }

        [HttpGet]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public async Task<IActionResult> Hours()
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.Days;
            var userid = _userManager.GetUserId(User);
            var user = _context.Users.OfType<Employee>().Where(u => u.Id == userid).FirstOrDefault();
            var hours = _context.EmployeeHours.Where(eh => eh.EmployeeID == userid).Where(eh => eh.Date.Month == DateTime.Today.Month).ToList();
            return View(hours);

        }
    }
}
