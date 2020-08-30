using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

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

        public IActionResult ReportHours()
        {
            TempData["PageType"] = AppUtility.TimeKeeperPageTypeEnum.Report;
            TempData["SideBar"] = AppUtility.TimeKeeperSidebarEnum.ReportHours;
            return View();

        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, TimeKeeper")]
        public bool MarkEntryExit(bool isEntry)
        {
            var userid = _userManager.GetUserId(User);
            var todaysEntry = _context.EmployeeHours.Where(eh => eh.Entry1 == DateTime.Today && eh.EmployeeID == userid).FirstOrDefault();
            if(todaysEntry == null)
            {
                if (isEntry)
                {
                    EmployeeHours todaysHours = new EmployeeHours { EmployeeID = userid, Entry1 = DateTime.Now };
                    _context.EmployeeHours.Add(todaysHours);
                    _context.SaveChanges();
                }
                else
                {
                    return false; //not a valid entry
                }
              
            }
            else
            {
                if (isEntry)
                {
                    if (todaysEntry.Entry2 == null)
                    {
                        todaysEntry.Entry2 = DateTime.Now;
                    }
                    else
                    {
                        return false;//no more entries
                    }
                    
                }
                else
                {
                    if (todaysEntry.Exit1 == null)
                    {
                        todaysEntry.Exit1 = DateTime.Now;
                    }
                    else if (todaysEntry.Exit2==null)
                    {
                        todaysEntry.Exit2 = DateTime.Now;
                    }
                    else
                    {
                        return false;//not more entries left - really not a valid entry
                    }
                }
                _context.EmployeeHours.Update(todaysEntry);
                _context.SaveChanges();

            }

            return true;
        }

    }
}
