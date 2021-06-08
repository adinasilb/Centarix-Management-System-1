using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;


namespace PrototypeWithAuth.Controllers
{
    public class HomeController : SharedController
    {
        private readonly UrlEncoder _urlEncoder;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, UrlEncoder urlEncoder)
            : base(context, userManager, hostingEnvironment)
        {
            _urlEncoder = urlEncoder;
        }

        public async Task<IActionResult> Index()
        {
            var user = _context.Employees.Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var usersLoggedIn = _context.Employees.Where(u => u.LastLogin.Date == DateTime.Today.Date).Count();
            var users = _context.Employees.ToList();
            var lastUpdateToTimekeeper = _context.GlobalInfos.Where(gi => gi.GlobalInfoType == AppUtility.GlobalInfoType.LoginUpdates.ToString()).FirstOrDefault();
            if(lastUpdateToTimekeeper==null)
            {
                lastUpdateToTimekeeper = new GlobalInfo { GlobalInfoType = AppUtility.GlobalInfoType.LoginUpdates.ToString(), Date = DateTime.Now.AddDays(-1) };
                _context.Update(lastUpdateToTimekeeper);
                await _context.SaveChangesAsync();
            }
            else if(lastUpdateToTimekeeper.Date.Date < DateTime.Today)
            {
                foreach (Employee employee in users)
                {
                    var userRoles = await _userManager.GetRolesAsync(employee);
                    if (userRoles.Contains("TimeKeeper") && employee.EmployeeStatusID != 4) //if employee statuses updated, function needs to be changed
                    {
                        await fillInTimekeeperMissingDays(employee, lastUpdateToTimekeeper.Date);
                        fillInTimekeeperNotifications(employee, lastUpdateToTimekeeper.Date);
                    }
                }
                lastUpdateToTimekeeper.Date = DateTime.Now;
                _context.Update(lastUpdateToTimekeeper);
                await _context.SaveChangesAsync();
            }
  
            if (user.LastLogin.Date < DateTime.Today)
            {
                fillInOrderLate(user);
                user.LastLogin = DateTime.Now;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            IEnumerable<Menu> menu = null;
            //if (rolesList.Contains(AppUtility.RoleItems.CEO.ToString()) || rolesList.Contains(AppUtility.RoleItems.Admin.ToString()))
            //{
            //    menu = _context.Menus.Select(x => x);
            //}
            //else
            //{
                menu =CreateMainMenu.GetMainMenu().Where(m => rolesList.Contains(m.MenuDescription));
            //}

            //update latest exchange rate if need be
            var latestRate = _context.GlobalInfos.Where(gi => gi.GlobalInfoType == AppUtility.GlobalInfoType.ExchangeRate.ToString()).FirstOrDefault();

          
            if (latestRate == null)
            {
                latestRate = new GlobalInfo();
                latestRate.GlobalInfoType = AppUtility.GlobalInfoType.ExchangeRate.ToString();
            }
            var updateDate = latestRate.Date;
            if (updateDate.Date != DateTime.Today)
            {
                latestRate.Date = DateTime.Now;
                latestRate.Description = AppUtility.GetExchangeRateFromApi().ToString();
                _context.Update(latestRate);
                await _context.SaveChangesAsync();
            }
            return View(menu);
        }
        public async Task<IActionResult> _MenuButtons()
        {
            var user = await _context.Users.Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefaultAsync();
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            IEnumerable<Menu> menu = null;
            //if (rolesList.Contains(AppUtility.RoleItems.CEO.ToString()) || rolesList.Contains(AppUtility.RoleItems.Admin.ToString()))
            //{
            //    menu = _context.Menus.Select(x => x);
            //}
            //else
            //{
                menu = CreateMainMenu.GetMainMenu().Where(m => rolesList.Contains(m.MenuDescription));
            //}

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
        public int GetNotificationCount()
        {
            var currentUserID = _userManager.GetUserId(User);
            DateTime lastReadNotfication = _context.Users.FirstOrDefault(u => u.Id == currentUserID).DateLastReadNotifications;
            int count1 = _context.RequestNotifications.Where(n => n.TimeStamp > lastReadNotfication && n.ApplicationUserID == currentUserID).Count();
            /*var tk = _context.TimekeeperNotifications.Where(n => n.TimeStamp > lastReadNotfication && n.ApplicationUserID == currentUserID);
            int count2 = tk.Count();
            return count1 + count2;*/
            return count1;
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
                 status = n.NotificationStatusID,
                 controller = n.Controller,
                 action = n.Action,
                 isRead = n.IsRead
             });
            //todo: figure out how to filter out - maybe only select those that are from less then 10 days ago
            /* var tnotification = _context.TimekeeperNotifications.Where(n => n.ApplicationUserID == currentUser.Id).Include(n => n.NotificationStatus)

                .Select(n => new
                {
                    id = n.NotificationID,
                    timeStamp = n.TimeStamp,
                    description = n.Description,
                    requestName = "",
                    icon = n.NotificationStatus.Icon,
                    color = n.NotificationStatus.Color,
                    status= n.NotificationStatusID,
                    controller = n.Controller,
                    action = n.Action,
                    isRead = n.IsRead
                });*/
            //var notificationsCombined = notification.Concat(rnotification).OrderByDescending(n=>n.timeStamp).ToList();
            //return Json(tnotification.Concat(rnotification).OrderByDescending(n => n.timeStamp).ToList().Take(4));
            return Json(rnotification.OrderByDescending(n => n.timeStamp).ToList().Take(4));
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

        private async Task fillInTimekeeperMissingDays(Employee user, DateTime lastUpdate)
        {
            DateTime nextDay = lastUpdate.AddDays(1);
            var year = nextDay.Year;
            var companyDaysOff = await _context.CompanyDayOffs.Where(d => d.Date.Year == year).ToListAsync();
            
            while (nextDay.Date <= DateTime.Today)
            {
                if (year != nextDay.Year)
                {
                    year = nextDay.Year;
                    companyDaysOff = await _context.CompanyDayOffs.Where(d => d.Date.Year == year).ToListAsync();
                }
                if (nextDay.DayOfWeek != DayOfWeek.Friday && nextDay.DayOfWeek != DayOfWeek.Saturday)
                {
                    var existentHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Date == nextDay.Date).FirstOrDefault();
                    var dayoff = companyDaysOff.Where(cdo => cdo.Date.Date == nextDay.Date).FirstOrDefault();
                    if (dayoff != null)
                    {
                        if (existentHours == null)
                        {
                            existentHours = new EmployeeHours
                            {
                                EmployeeID = user.Id,
                                Date = nextDay.Date
                            };

                        }
                        existentHours.OffDayTypeID = null;
                        existentHours.PartialOffDayTypeID = null;
                        existentHours.PartialOffDayHours = null;
                        existentHours.CompanyDayOffID = dayoff.CompanyDayOffID;
                        _context.Update(existentHours);
                    }
                    else
                    {
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

                }
                nextDay = nextDay.AddDays(1);
            }
            await _context.SaveChangesAsync();
        }

        private void fillInOrderLate(ApplicationUser user)
        
        {
            if (user.LastLogin.Date != DateTime.Now.Date)
            {

                var lateOrders = _context.Requests.Where(r => r.ApplicationUserCreatorID == user.Id).Where(r => r.RequestStatusID == 2).Where(r => r.ExpectedSupplyDays != null)
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
        private void fillInTimekeeperNotifications(ApplicationUser user, DateTime lastUpdate)
        {

                var eh = _context.EmployeeHours.Where(r => r.EmployeeID == user.Id).Where(r => (r.Entry1 != null && r.Exit1 == null) || (r.Entry1 == null && r.Exit1 == null && r.OffDayType == null && r.TotalHours == null) || (r.Entry2 != null && r.Exit2 == null))
                    .Where(r => r.Date.Date >=lastUpdate.Date && r.Date.Date < DateTime.Today).Where(r => r.CompanyDayOffID==null)
                    .Where(r => r.EmployeeHoursAwaitingApproval == null);
                foreach (var e in eh)
                {
                    TimekeeperNotification timekeeperNotification = new TimekeeperNotification();
                    timekeeperNotification.EmployeeHoursID = e.EmployeeHoursID;
                    timekeeperNotification.IsRead = false;
                    timekeeperNotification.ApplicationUserID = e.EmployeeID;
                    timekeeperNotification.Description = "no hours reported for " + e.Date.ToString("dd/MM/yyyy");
                    timekeeperNotification.NotificationStatusID = 5;
                    timekeeperNotification.TimeStamp = DateTime.Now;
                    timekeeperNotification.Controller = "Timekeeper";
                    timekeeperNotification.Action = "SummaryHours";
                    _context.Update(timekeeperNotification);
                }
                _context.SaveChanges();

        }


        public async Task<IActionResult> WebCam()
        {
            return null;
        }
        

    }
}
