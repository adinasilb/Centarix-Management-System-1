using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;


namespace PrototypeWithAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UrlEncoder _urlEncoder;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, UrlEncoder urlEncoder)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _urlEncoder = urlEncoder;
        }

        public async Task<IActionResult> Index()
        {
            var user = _context.Users.Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefault();

            if (user.LastLogin.Date < DateTime.Today)
            {
                fillInOrderLate(user);
                fillInTimekeeperMissingDays(user);
                fillInTimekeeperNotifications(user);
                user.LastLogin = DateTime.Now;
                _context.Update(user);
                _context.SaveChanges();
            }
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            IEnumerable<Menu> menu = null;
            if (rolesList.Contains(AppUtility.RoleItems.CEO.ToString()) || rolesList.Contains(AppUtility.RoleItems.Admin.ToString()))
            {
                menu = _context.Menus.Select(x => x);
            }
            else
            {
                menu = _context.Menus.Where(m => rolesList.Contains(m.MenuDescription));
            }


            return View(menu);
        }
        public async Task<IActionResult> _MenuButtons()
        {
            var user = await _context.Users.Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefaultAsync();
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            IEnumerable<Menu> menu = null;
            if (rolesList.Contains(AppUtility.RoleItems.CEO.ToString()) || rolesList.Contains(AppUtility.RoleItems.Admin.ToString()))
            {
                menu = _context.Menus.Select(x => x);
            }
            else
            {
                menu = _context.Menus.Where(m=> rolesList.Contains(m.MenuDescription));
            }

            return PartialView(menu);
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
            DateTime lastReadNotfication = _context.Users.FirstOrDefault(u => u.Id == currentUserID).DateLastReadNotifications;
            int count1 = _context.RequestNotifications.Where(n => n.TimeStamp > lastReadNotfication && n.ApplicationUserID == currentUserID).Count();
            var tk = _context.TimekeeperNotifications.Where(n => n.TimeStamp > lastReadNotfication && n.ApplicationUserID == currentUserID);
            int count2 = tk.Count();
            return count1 + count2;
        }
        [HttpGet]
        public JsonResult GetLatestNotifications()
        {
            ApplicationUser currentUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            //todo: figure out exactly which notfications to show
            var rnotification = _context.RequestNotifications.Include(n => n.NotificationStatus).Where(n => n.ApplicationUserID == currentUser.Id)

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
             });
            //todo: figure out how to filter out - maybe only select those that are from less then 10 days ago
            var tnotification = _context.TimekeeperNotifications.Where(n => n.ApplicationUserID == currentUser.Id).Include(n => n.NotificationStatus)

               .Select(n => new
               {
                   id = n.NotificationID,
                   timeStamp = n.TimeStamp,
                   description = n.Description,
                   requestName = "",
                   icon = n.NotificationStatus.Icon,
                   color = n.NotificationStatus.Color,
                   controller = n.Controller,
                   action = n.Action,
                   isRead = n.IsRead
               });
            //var notificationsCombined = notification.Concat(rnotification).OrderByDescending(n=>n.timeStamp).ToList();
            return Json(tnotification.Concat(rnotification).OrderByDescending(n => n.timeStamp).ToList().Take(4));
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

        [HttpGet]
        public IActionResult Login2FA()
        {
            TwoFactorAuthenticationViewModel twoFactorAuthenticationViewModel = new TwoFactorAuthenticationViewModel { };
            return View(twoFactorAuthenticationViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login2FA(TwoFactorAuthenticationViewModel twoFactorAuthenticationViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            // Strip spaces and hypens
            var verificationCode = twoFactorAuthenticationViewModel.TwoFACode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid authentication code.");
                return View(twoFactorAuthenticationViewModel);
            }

            //await _userManager.SetTwoFactorEnabledAsync(user, true);
            return RedirectToAction("Index");
        }
        private void fillInTimekeeperMissingDays(ApplicationUser user)
        {
            DateTime nextDay = user.LastLogin.AddDays(1);
            var year = nextDay.Year;
            if (user.LastLogin == new DateTime())
            {
                return;
            }
            var companyDaysOff = _context.CompanyDayOffs.Select(cdo => cdo.Date.Date).Where(d => d.Date.Year == year).ToList();

            while (nextDay.Date <= DateTime.Today)
            {
                if( year != nextDay.Year)
                {
                    year = nextDay.Year;
                    companyDaysOff = _context.CompanyDayOffs.Select(cdo => cdo.Date.Date).Where(d => d.Date.Year == year).ToList();
                }
                if (nextDay.DayOfWeek != DayOfWeek.Friday && nextDay.DayOfWeek != DayOfWeek.Saturday && !companyDaysOff.Contains(nextDay.Date))
                {
                    var existentHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Date == nextDay.Date).FirstOrDefault();
                    if (existentHours == null)
                    {
                        EmployeeHours employeeHours = new EmployeeHours
                        {
                            EmployeeID = user.Id,
                            Date = nextDay.Date
                        };
                        _context.Update(employeeHours);
                    }
                }
                nextDay = nextDay.AddDays(1);
            }
            _context.SaveChanges();
        }

        private void fillInOrderLate(ApplicationUser user)
        {
            if (user.LastLogin.Date != DateTime.Now.Date)
            {

                var lateOrders = _context.Requests.Where(r => r.ApplicationUserCreatorID == user.Id).Where(r => r.RequestStatusID == 2)
                    .Where(r => r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0).Date >= user.LastLogin.Date && r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0).Date < DateTime.Today)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.ParentRequest);
                foreach (var request in lateOrders)
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

            }
        }
        private void fillInTimekeeperNotifications(ApplicationUser user)
        {
            if (user.LastLogin == new DateTime())
            {
                return;
            }
            if (user.LastLogin.Date != DateTime.Now.Date)
            {
                var eh = _context.EmployeeHours.Where(r => r.EmployeeID == user.Id).Where(r => (r.Entry1 != null && r.Exit1 == null) || (r.Entry1 == null && r.Exit1 == null && r.OffDayType == null) || (r.Entry2 != null && r.Exit2 == null))
                    .Where(r => r.Date.Date >= user.LastLogin.Date && r.Date.Date < DateTime.Today);
                foreach (var e in eh)
                {
                    TimekeeperNotification timekeeperNotification = new TimekeeperNotification();
                    timekeeperNotification.EmployeeHoursID = e.EmployeeHoursID;
                    timekeeperNotification.IsRead = false;
                    timekeeperNotification.ApplicationUserID = e.EmployeeID;
                    timekeeperNotification.Description = "update hours for " + e.Date.ToString("dd/MM/yyyy");
                    timekeeperNotification.NotificationStatusID = 5;
                    timekeeperNotification.TimeStamp = DateTime.Now;
                    timekeeperNotification.Controller = "Timekeeper";
                    timekeeperNotification.Action = "SummaryHours";
                    _context.Update(timekeeperNotification);
                }
                _context.SaveChanges();

            }
        }


        public async Task<IActionResult> WebCam()
        {
            return null;
        }
        

    }
}
