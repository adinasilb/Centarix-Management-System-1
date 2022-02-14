using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
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
            using (var transaction = _applicationDbContextTransaction.Transaction)
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
                        _globalInfosProc.UpdateWithoutSaving(lastLoginUpdate);
                    }
                    if (lastUpdateToTimekeeperNotifications == null)
                    {
                        lastUpdateToTimekeeperNotifications = new GlobalInfo { GlobalInfoType = AppUtility.GlobalInfoType.TimekeeperNotificationUpdated.ToString(), Date = lastLoginUpdate.Date};
                        updateTimekeeperNotifications = (_globalInfosProc.UpdateWithoutSaving(lastUpdateToTimekeeperNotifications)).Bool;
                    }
                    if (lastUpdateToBirthdayNotifications == null)
                    {
                        lastUpdateToBirthdayNotifications = new GlobalInfo { GlobalInfoType = AppUtility.GlobalInfoType.BirthdayNotificationUpdated.ToString(), Date = lastLoginUpdate.Date };
                        updateBirthdayNotifications =  (_globalInfosProc.UpdateWithoutSaving(lastUpdateToBirthdayNotifications)).Bool;
                    }

                    if (lastUpdateToBirthdayNotifications.Date.Date < DateTime.Today || lastUpdateToTimekeeperNotifications.Date.Date < DateTime.Today)
                    {
                        
                        DateTime birthdaysNextDay = lastUpdateToBirthdayNotifications.Date.AddDays(1);
                        var existingBirthdays = new List<Employee>();
                        var timekeeperInfoUpdated = new StringWithBool();

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
                                timekeeperInfoUpdated = await fillInTimekeeperMissingDays(employee, lastUpdateToTimekeeperNotifications.Date);
                                if (timekeeperInfoUpdated.Bool)
                                {
                                    timekeeperInfoUpdated = fillInTimekeeperNotifications(employee, lastUpdateToTimekeeperNotifications.Date);
                                }
                                if(!timekeeperInfoUpdated.Bool)
                                {
                                    updateTimekeeperNotifications = false;
                                    viewModel.ErrorMessage += timekeeperInfoUpdated.String;
                                }
                            }
                            if (updateBirthdayNotifications && existingBirthdays.Count > 0)
                            {
                                updateBirthdayNotifications = fillInBirthdayNotifications(employee, existingBirthdays).Bool;
                            }
                        }
                        if (updateTimekeeperNotifications)
                        {
                            lastUpdateToTimekeeperNotifications.Date = DateTime.Now;
                            _globalInfosProc.UpdateWithoutSaving(lastUpdateToTimekeeperNotifications);
                        }
                        else
                        {
                            var timekeeperentries = _applicationDbContextEntries.Entries.Where(e => e.Entity.GetType() == new TimekeeperNotification().GetType() ||
                            e.Entity.Equals(lastUpdateToTimekeeperNotifications) || e.Entity.GetType() == new EmployeeHours().GetType());
                            foreach (var te in timekeeperentries)
                            {
                                te.State = EntityState.Detached;
                            }
                        }
                        if (updateBirthdayNotifications)
                        {
                            lastUpdateToBirthdayNotifications.Date = DateTime.Now;
                            _globalInfosProc.UpdateWithoutSaving(lastUpdateToBirthdayNotifications);

                        }
                        else
                        {
                            var birthdayentries = _applicationDbContextEntries.Entries.Where(e => e.Entity.GetType() == new EmployeeInfoNotification().GetType()
                            );
                            foreach (var be in birthdayentries)
                            {
                                be.State = EntityState.Detached;
                            }
                        }
                    }
                    await _globalInfosProc.SaveDbChangesAsync();
                    await transaction.CommitAsync();
               }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    viewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                }
            }
            using (var transaction2 = _applicationDbContextTransaction.Transaction)
            {
                var orderLatesUpdated = new StringWithBool();
                try
                {
                    if (user.LastLogin.Date < DateTime.Today)
                    {
                        orderLatesUpdated = await fillInOrderLate(user);
                        if (orderLatesUpdated.Bool)
                        {
                            _employeesProc.UpdateLastLoginWithoutSaving(user.Id);
                            await _employeesProc.SaveDbChangesAsync();
                        }
                        else
                        {
                            viewModel.ErrorMessage += orderLatesUpdated.String;
                        }
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
            var latestRate = await _globalInfosProc.ReadOneAsync(new List<Expression<Func<GlobalInfo, bool>>> { gi => gi.GlobalInfoType == AppUtility.GlobalInfoType.ExchangeRate.ToString() });

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
                    var exchangeRateUpdated = await _globalInfosProc.UpdateAsync(latestRate);
                    if (!exchangeRateUpdated.Bool)
                    {
                        viewModel.ErrorMessage += exchangeRateUpdated.String;
                    }
                }
                else
                {
                    var message = new MimeMessage();

                    //instantiate the body builder
                    var builder = new BodyBuilder();                                                                                                                
                    var currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Email == "elixir@centarix.com" });
                    string ownerEmail = currentUser.Email;
                    string ownerUsername = currentUser.FirstName + " " + currentUser.LastName;
                    string ownerPassword = currentUser.SecureAppPass;
                    string recipientEmail = "logistic@centarix.com";
                    string recipientName = "Logistics Centarix";
                    //add a "From" Email
                    message.From.Add(new MailboxAddress(ownerUsername, ownerEmail));

                    // add a "To" Email
                    message.To.Add(new MailboxAddress(recipientName, recipientEmail));

                    //subject
                    message.Subject = "Error with Exchange Rate";

                    //body
                    builder.TextBody = @"There has been an error in accessing today's Exchange Rate." + "\n" + "Please forward this email to a company software developer";

                    message.Body = builder.ToMessageBody();

                    using (var client = new SmtpClient())
                    {

                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate(ownerEmail, ownerPassword);
                        client.Timeout = 500000; // 500 seconds
                        try
                        {
                            client.Send(message);
                            client.Disconnect(true);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            return View(viewModel);
        }
        public async Task<IActionResult> _MenuButtons()
        {
            var user = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == _userManager.GetUserId(User) });
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
        public async Task<int> GetNotificationCount()
        {
            var currentUserID = _userManager.GetUserId(User);
            DateTime lastReadNotfication = (await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == _userManager.GetUserId(User) })).DateLastReadNotifications;
            int count1 = _requestNotificationsProc.Read(new List<Expression<Func<RequestNotification, bool>>> {n => n.TimeStamp > lastReadNotfication && n.ApplicationUserID == currentUserID}).Count();
            int count2 = _employeeInfoNotificationsProc.Read(new List<Expression<Func<EmployeeInfoNotification, bool>>> { n => n.TimeStamp > lastReadNotfication && n.ApplicationUserID == currentUserID }).Count();
            return count1 + count2;
        }
        [HttpGet]
        public async Task<JsonResult> GetLatestNotifications()
        {
            ApplicationUser currentUser = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { u => u.Id == _userManager.GetUserId(User) });
            //todo: figure out exactly which notfications to show
            var rnotification = _requestNotificationsProc.Read(new List<Expression<Func<RequestNotification, bool>>> { n => n.ApplicationUserID == currentUser.Id },
                new List<ComplexIncludes<RequestNotification, ModelBase>>
                {
                    new ComplexIncludes<RequestNotification, ModelBase>{ Include = n => n.NotificationStatus}
                })
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

            var enotification = _employeeInfoNotificationsProc.Read(new List<Expression<Func<EmployeeInfoNotification, bool>>> { n => n.ApplicationUserID == currentUser.Id },
                new List<ComplexIncludes<EmployeeInfoNotification, ModelBase>>
                {
                    new ComplexIncludes<EmployeeInfoNotification, ModelBase>{ Include = n => n.NotificationStatus}
                })

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
        public async Task<bool> UpdateLastReadNotifications()
        {
            var returnVal = await _employeesProc.UpdateLastReadNotification(_userManager.GetUserId(User));
            return returnVal.Bool;
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

        private async Task<StringWithBool> fillInOrderLate(ApplicationUser user)
        {
            var ReturnVal = new StringWithBool();

            var lateOrders = await  _requestsProc.Read(new List<Expression<Func<Request, bool>>> { r => r.ApplicationUserCreatorID == user.Id, r => r.RequestStatusID == 2, r => r.ExpectedSupplyDays != null,
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
                    }).ToListAsync();
            ReturnVal = await _requestNotificationsProc.CreateManyOrderLateWithoutSaveAsync(lateOrders);

            return ReturnVal;
        }
        private StringWithBool fillInTimekeeperNotifications(ApplicationUser user, DateTime lastUpdate)
        {
            var ReturnVal = new StringWithBool() {Bool = true };

            var eh = _employeeHoursProc.Read(new List<Expression<Func<EmployeeHours, bool>>> {
                r => r.EmployeeID == user.Id,
                r => (r.Entry1 != null && r.Exit1 == null) || (r.Entry1 == null && r.Exit1 == null && r.OffDayType == null && r.TotalHours == null) || (r.Entry2 != null && r.Exit2 == null),
                r => r.Date.Date >=lastUpdate.Date && r.Date.Date < DateTime.Today,
                r => r.CompanyDayOffID==null,
                r => r.EmployeeHoursAwaitingApproval == null
            });
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
                _timekeeperNotificationsProc.CreateWithoutSaveChanges(timekeeperNotification);
            }
            return ReturnVal;
        }

        private StringWithBool fillInBirthdayNotifications(Employee user, List<Employee> userBirthdays)
        {
            var ReturnVal = new StringWithBool() { Bool = true };
            
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
                _employeeInfoNotificationsProc.CreateWithoutSaveChanges(BirthdayNotification);
            }
            return ReturnVal;
        }


        public async Task<IActionResult> WebCam()
        {
            return null;
        }

     
    }
}
