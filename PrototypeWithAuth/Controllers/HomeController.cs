using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using SQLitePCL;

namespace PrototypeWithAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //Adina added in 3 lines
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("IndexAdmin");
            }
            IEnumerable<Menu> menu = _context.Menus.Select(x => x);
            return View(menu);
        }

        //Adina added in
        [Authorize(Roles = "Admin")]
        public IActionResult IndexAdmin()
        {
            IEnumerable<Menu> menu = _context.Menus.Select(x => x);
            return View(menu);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public int GetNotficationCount()
        {
            DateTime lastReadNotfication = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User)).DateLastReadNotifications;
            int count = _context.Notifications.Where(n => n.TimeStamp > lastReadNotfication).Count();
            return count;
        }
        [HttpGet]
        public JsonResult GetLatestNotifications()
        {
            ApplicationUser currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            //todo: figure out exactly which notfications to show
            var notification = _context.Notifications.Where(n => !(n is RequestNotification)).Where(n => n.ApplicationUserID == currentUser.Id && n.TimeStamp > DateTime.Now.AddDays(-5))
                .Select(n => new
                {
                    timeStamp = n.TimeStamp,
                    description = n.Description,
                    requestName = ""
                });
            var rnotification = _context.Notifications.OfType<RequestNotification>().Where(n => n.ApplicationUserID == currentUser.Id && n.TimeStamp > DateTime.Now.AddDays(-5))
             .Select(n => new
             {
                 timeStamp = n.TimeStamp,
                 description = n.Description,
                 requestName = n.RequestName
             });

            var notificationsCombined = notification.Concat(rnotification).ToList();
            return Json(notificationsCombined);
        }
        [HttpPost]
        public bool UpdateLastReadNotifications()
        {
            ApplicationUser currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            DateTime lastReadNotfication = currentUser.DateLastReadNotifications;
            currentUser.DateLastReadNotifications = DateTime.Now;
            _context.Update(currentUser);
            _context.SaveChanges();          
            return true;
        }
    }
}
