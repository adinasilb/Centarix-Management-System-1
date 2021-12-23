using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;


namespace PrototypeWithAuth.Controllers
{
    public class HomeController : SharedController
    {
        private readonly UrlEncoder _urlEncoder;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, UrlEncoder urlEncoder, ICompositeViewEngine viewEngine, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
            _urlEncoder = urlEncoder;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _employeesProc.ReadOneAsync( new List<System.Linq.Expressions.Expression<Func<Employee, bool>>> { e => e.Id==_userManager.GetUserId(User) }, 
                new List<ComplexIncludes<Employee, ModelBase>> { new ComplexIncludes<Employee, ModelBase> { Include = e => e.SalariedEmployee } });
            var usersLoggedIn = _employeesProc.ReadOne(new List<System.Linq.Expressions.Expression<Func<Employee, bool>>> { u => u.LastLogin.Date == DateTime.Today.Date }).Count();
            var users =  _employeesProc.Read().ToList();
            HomePageViewModel viewModel = new HomePageViewModel();

            //RecurringJob.AddOrUpdate("DailyNotifications", () => DailyNotificationUpdate(users), Cron.Daily);
            using (var transaction = _applicationContextTransaction.Transaction)
            {
                try
                {
                    var lastUpdateToTimekeeperNotifications = await _globalInfosProc.ReadOneAsync(new List<Expression<Func<GlobalInfo, bool>>> { gi => gi.GlobalInfoType == AppUtility.GlobalInfoType.TimekeeperNotificationUpdated.ToString() });
                    var lastUpdateToBirthdayNotifications = await _globalInfosProc.ReadOneAsync(new List<Expression<Func<GlobalInfo, bool>>> { gi => gi.GlobalInfoType == AppUtility.GlobalInfoType.BirthdayNotificationUpdated.ToString() });
                    var lastLoginUpdate = await _globalInfosProc.ReadOneAsync(new List<Expression<Func<GlobalInfo, bool>>> { gi => gi.GlobalInfoType == AppUtility.GlobalInfoType.LoginUpdates.ToString() });

                    bool updateTimekeeperNotifications = true;
                    bool updateBirthdayNotifications = true;

                    if(lastLoginUpdate == null)
                    {
                        lastLoginUpdate = new GlobalInfo { GlobalInfoType = AppUtility.GlobalInfoType.LoginUpdates.ToString(), Date = DateTime.Now.AddDays(-1) };
                        await _globalInfosProc.UpdateAsyncWithoutSaving(lastLoginUpdate);
                    }
                    if (lastUpdateToTimekeeperNotifications == null)
                    {
                        lastUpdateToTimekeeperNotifications = new GlobalInfo { GlobalInfoType = AppUtility.GlobalInfoType.TimekeeperNotificationUpdated.ToString(), Date = lastLoginUpdate.Date};
                        updateTimekeeperNotifications = (await _globalInfosProc.UpdateAsyncWithoutSaving(lastUpdateToTimekeeperNotifications)).Bool;
                    }
                    if (lastUpdateToBirthdayNotifications == null)
                    {
                        lastUpdateToBirthdayNotifications = new GlobalInfo { GlobalInfoType = AppUtility.GlobalInfoType.BirthdayNotificationUpdated.ToString(), Date = lastLoginUpdate.Date };
                        updateBirthdayNotifications =  (await _globalInfosProc.UpdateAsyncWithoutSaving(lastUpdateToBirthdayNotifications)).Bool;
                    }

                    else if (lastUpdateToBirthdayNotifications.Date.Date < DateTime.Today || lastUpdateToTimekeeperNotifications.Date.Date < DateTime.Today)
                    {
                        var entries = _context.ChangeTracker.Entries().Where(e => e.Entity.GetType() == new GlobalInfo().GetType()).ToList();
                        entries.ForEach(e => e.State = EntityState.Detached);
                        _context.ChangeTracker.Entries();
                        await transaction.CommitAsync();
                        DateTime birthdaysNextDay = lastUpdateToBirthdayNotifications.Date.AddDays(1);
                        var existingBirthdays = new List<Employee>();

                        while (birthdaysNextDay.Date <= DateTime.Today)
                        {
                            users.Where(r => r.DOB.AddYears(birthdaysNextDay.Year - r.DOB.Year) == birthdaysNextDay.Date).ToList().ForEach(e => existingBirthdays.Add(e));
                            birthdaysNextDay = birthdaysNextDay.AddDays(1);
                        }
                        foreach (Employee employee in users)
                        {
                            var userRoles = await _userManager.GetRolesAsync(employee);
                            if (updateTimekeeperNotifications && userRoles.Contains("TimeKeeper") && employee.EmployeeStatusID != 4) //if employee statuses updated, function needs to be changed
                            {
                                var timekeeperInfoUpdated = await fillInTimekeeperMissingDays(employee, lastUpdateToTimekeeperNotifications.Date);
                                if (timekeeperInfoUpdated.Bool)
                                {
                                    timekeeperInfoUpdated = await fillInTimekeeperNotifications(employee, lastUpdateToTimekeeperNotifications.Date);
                                }
                                if(!timekeeperInfoUpdated.Bool)
                                {
                                    updateTimekeeperNotifications = false;
                                    viewModel.ErrorMessage += timekeeperInfoUpdated.String;
                                    var timekeeperentries = _context.ChangeTracker.Entries().Where(e => e.Entity.GetType() == new TimekeeperNotification().GetType());
                                }
                            }
                            if (updateBirthdayNotifications && existingBirthdays.Count > 0)
                            {
                                fillInBirthdayNotifications(employee, existingBirthdays);
                            }
                        }
                        lastUpdateToTimekeeperNotifications.Date = DateTime.Now;
                        lastUpdateToBirthdayNotifications.Date = DateTime.Now;
                        //_context.Update(lastUpdateToNotifications);
                        await _context.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    viewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                }
            }
            using (var transaction2 = _context.Database.BeginTransaction())
            {
                try
                {
                    if (user.LastLogin.Date < DateTime.Today)
                    {
                        fillInOrderLate(user);
                        user.LastLogin = DateTime.Now;
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    await transaction2.CommitAsync();
                }
                catch(Exception ex)
                {
                    await transaction2.RollbackAsync();
                    viewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                }
            }
        
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
            viewModel.Menus = menu;
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
                decimal currentRate = AppUtility.GetExchangeRateFromApi();
                if (currentRate != 0)
                {
                    latestRate.Date = DateTime.Now;
                    latestRate.Description = currentRate.ToString();
                    _context.Update(latestRate);
                    await _context.SaveChangesAsync();
                }
            }
            return View(viewModel);
        }
        public async Task DailyNotificationUpdate(List<Employee> users)
        {

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
            int count2 = _context.EmployeeInfoNotifications.Where(n => n.TimeStamp > lastReadNotfication && n.ApplicationUserID == currentUserID).Count();
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
                 date = n.NotificationDate,
                 description = n.Description,
                 requestName = n.RequestName,
                 icon = n.NotificationStatus.Icon,
                 color = n.NotificationStatus.Color,
                 status = n.NotificationStatusID,
                 controller = n.Controller,
                 action = n.Action,
                 isRead = n.IsRead,
                 timestamp = n.TimeStamp
             });

            var enotification = _context.EmployeeInfoNotifications.Include(n => n.NotificationStatus).Where(n => n.ApplicationUserID == currentUser.Id)

            .Select(n => new
            {
                id = n.NotificationID,
                date = n.NotificationDate,
                description = n.Description,
                requestName = "",
                icon = n.NotificationStatus.Icon,
                color = n.NotificationStatus.Color,
                status = n.NotificationStatusID,
                controller = n.Controller,
                action = n.Action,
                isRead = n.IsRead,
                timestamp = n.TimeStamp
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
            var notificationsCombined = rnotification.Concat(enotification).OrderByDescending(n => n.timestamp).ToList().Take(4);
            //return Json(tnotification.Concat(rnotification).OrderByDescending(n => n.timeStamp).ToList().Take(4));
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

        [HttpGet]
        public IActionResult Login2FA()
        {
            TwoFactorAuthenticationViewModel twoFactorAuthenticationViewModel = new TwoFactorAuthenticationViewModel { };
            return View(twoFactorAuthenticationViewModel);
        }

        private async Task<StringWithBool> fillInTimekeeperMissingDays(Employee user, DateTime lastUpdate)
        {
            var missingDaysFilled = new StringWithBool();
            try
            {
                DateTime nextDay = lastUpdate.AddDays(1);
                var year = nextDay.Year;
                var companyDaysOff = _companyDaysOffProc.Read(new List<Expression<Func<CompanyDayOff, bool>>> { d => d.Date.Year == year });

                while (nextDay.Date <= DateTime.Today)
                {
                    if (year != nextDay.Year)
                    {
                        year = nextDay.Year;
                        companyDaysOff = _companyDaysOffProc.Read(new List<Expression<Func<CompanyDayOff, bool>>> { d => d.Date.Year == year });
                    }
                    if (nextDay.DayOfWeek != DayOfWeek.Friday && nextDay.DayOfWeek != DayOfWeek.Saturday)
                    {
                        var existentHours = await _employeeHoursProc.ReadOneAsync(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.EmployeeID == user.Id && eh.Date.Date == nextDay.Date });
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
                            missingDaysFilled = _employeeHoursProc.UpdateWithoutSaving(existentHours);
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
                                missingDaysFilled = _employeeHoursProc.UpdateWithoutSaving(employeeHours);
                            }
                        }
                    }
                    nextDay = nextDay.AddDays(1);
                }
                missingDaysFilled.SetStringAndBool(true, null);
            }
            catch(Exception ex)
            {
                missingDaysFilled.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return missingDaysFilled;
        }

        private void fillInOrderLate(ApplicationUser user)

        {
                var lateOrders = _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.ApplicationUserCreatorID == user.Id, r => r.RequestStatusID == 2, r => r.ExpectedSupplyDays != null,
                    r => r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0).Date >= user.LastLogin.Date && r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0).Date < DateTime.Today},
                    new List<ComplexIncludes<Request, ModelBase>> {
                        new ComplexIncludes<Request, ModelBase> {
                            Include = r => r.Product,
                            ThenInclude = new ComplexIncludes<ModelBase, ModelBase>
                            {
                                Include = p => ((Product)p).Vendor
                            }
                        },
                        new ComplexIncludes<Request, ModelBase> {
                            Include = r => r.ParentRequest,
                        }
                    });
                foreach (var request in lateOrders)
                {
                    RequestNotification requestNotification = new RequestNotification();
                    requestNotification.RequestID = request.RequestID;
                    requestNotification.IsRead = false;
                    requestNotification.RequestName = request.Product.ProductName;
                    requestNotification.ApplicationUserID = request.ApplicationUserCreatorID;
                    requestNotification.Description = "should have arrived " + request.ParentRequest.OrderDate.AddDays(request.ExpectedSupplyDays ?? 0).GetElixirDateFormat();
                    requestNotification.NotificationStatusID = 1;
                    requestNotification.NotificationDate = DateTime.Now;
                    requestNotification.Controller = "Requests";
                    requestNotification.Action = "NotificationsView";
                    requestNotification.OrderDate = request.ParentRequest.OrderDate;
                    requestNotification.Vendor = request.Product.Vendor.VendorEnName;
                    _requestNotificationsProc.CreateWithoutSaveChanges(requestNotification);
                }
                _context.SaveChanges();
        }
        private async Task<StringWithBool> fillInTimekeeperNotifications(ApplicationUser user, DateTime lastUpdate)
        {
            var ReturnVal = new StringWithBool();
                var eh = _context.EmployeeHours.Where(r => r.EmployeeID == user.Id).Where(r => (r.Entry1 != null && r.Exit1 == null) || (r.Entry1 == null && r.Exit1 == null && r.OffDayType == null && r.TotalHours == null) || (r.Entry2 != null && r.Exit2 == null))
                    .Where(r => r.Date.Date >=lastUpdate.Date && r.Date.Date < DateTime.Today).Where(r => r.CompanyDayOffID==null)
                    .Where(r => r.EmployeeHoursAwaitingApproval == null);
                foreach (var e in eh)
                {
                    TimekeeperNotification timekeeperNotification = new TimekeeperNotification();
                    timekeeperNotification.EmployeeHoursID = e.EmployeeHoursID;
                    timekeeperNotification.IsRead = false;
                    timekeeperNotification.ApplicationUserID = e.EmployeeID;
                    timekeeperNotification.Description = "no hours reported for " + e.Date.GetElixirDateFormat();
                    timekeeperNotification.NotificationStatusID = 5;
                    timekeeperNotification.NotificationDate = DateTime.Now;
                    timekeeperNotification.Controller = "Timekeeper";
                    timekeeperNotification.Action = "SummaryHours";
                    _context.Update(timekeeperNotification);
                }
                _context.SaveChanges();
            return ReturnVal;
        }

        private async Task<StringWithBool> fillInBirthdayNotifications(Employee user, List<Employee> userBirthdays)
        {
            var ReturnVal = new StringWithBool();
            foreach (var e in userBirthdays)
            {
                EmployeeInfoNotification BirthdayNotification = new EmployeeInfoNotification();
                BirthdayNotification.IsRead = false;
                BirthdayNotification.ApplicationUserID = user.Id;
                BirthdayNotification.EmployeeID = e.Id;
                BirthdayNotification.Description = "Happy Birthday to " + e.FirstName + " " + e.LastName;
                BirthdayNotification.NotificationStatusID = 6;
                BirthdayNotification.NotificationDate = new DateTime(DateTime.Today.Year, e.DOB.Month, e.DOB.Day);
                BirthdayNotification.Controller = "";
                BirthdayNotification.Action = "";
                _context.Update(BirthdayNotification);
            }
            _context.SaveChanges();

            return ReturnVal;
        }


        public async Task<IActionResult> WebCam()
        {
            return null;
        }


    }
}
