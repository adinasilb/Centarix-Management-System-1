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
using PrototypeWithAuth.AppData;
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
            //debbie added in
            var currentUserID = _userManager.GetUserId(User);
            var user = _context.Users.Where(u => u.Id == currentUserID).FirstOrDefault();
            if(user.LastLogin != DateTime.Today)
            {
                var lateOrders = _context.Requests.Where(r=>r.ApplicationUserCreatorID==currentUserID).Where(r => r.RequestStatusID == 2)
                    .Where(r => r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0) >= user.LastLogin && r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0) < DateTime.Today)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.ParentRequest);
                foreach(var request in lateOrders)
                {
                    RequestNotification requestNotification = new RequestNotification();
                    requestNotification.RequestID = request.RequestID;
                    requestNotification.IsRead = false;
                    requestNotification.RequestName = request.Product.ProductName;
                    requestNotification.ApplicationUserID = request.ApplicationUserCreatorID;
                    requestNotification.Description = "should have arrived " + request.ParentRequest.OrderDate.AddDays(request.ExpectedSupplyDays ?? 0).ToString("dd/MM/yyyy");
                    requestNotification.NotificationStatusID = 1;
                    requestNotification.TimeStamp = DateTime.Now;
                    requestNotification.Controller = "Requests";
                    requestNotification.Action = "NotificationsView";
                    requestNotification.OrderDate = request.ParentRequest.OrderDate;
                    requestNotification.Vendor = request.Product.Vendor.VendorEnName;
                    _context.Update(requestNotification);            
                }
                _context.SaveChanges();
                user.LastLogin = DateTime.Today;
                _context.Update(user);
                _context.SaveChanges();
            }
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
            var currentUserID = _userManager.GetUserId(User);
            DateTime lastReadNotfication = _context.Users.FirstOrDefault(u => u.Id ==currentUserID).DateLastReadNotifications;
            int count = _context.RequestNotifications.Where(n => n.TimeStamp > lastReadNotfication & n.ApplicationUserID==currentUserID).Count();
            return count;
        }
        [HttpGet]
        public JsonResult GetLatestNotifications()
        {
            ApplicationUser currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            //todo: figure out exactly which notfications to show
            var rnotification = _context.RequestNotifications.Include(n => n.NotificationStatus).Where(n => n.ApplicationUserID == currentUser.Id && n.TimeStamp > DateTime.Now.AddDays(-5))
             
             .Select(n => new
             {
                 id = n.NotificationID,
                 timeStamp = n.TimeStamp,
                 description = n.Description,
                 requestName = n.RequestName,
                 icon = n.NotificationStatus.Icon,
                 color = n.NotificationStatus.Color,
                 controller = n.Controller,
                 action = n.Action,
                 isRead = n.IsRead
             }).OrderByDescending(n => n.timeStamp).ToList();

            //var notificationsCombined = notification.Concat(rnotification).OrderByDescending(n=>n.timeStamp).ToList();
            return Json(rnotification.Take(4));
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
