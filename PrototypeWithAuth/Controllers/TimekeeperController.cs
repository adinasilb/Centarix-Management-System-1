using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class TimekeeperController : SharedController
    {
        private CRUD.EmployeeHoursProc _employeeHoursProc;
        private CRUD.TimekeeperNotificationsProc _timekeeperNotificationsProc;
        private CRUD.ApplicationUsersProc _applicationUsersProc;
        private CRUD.EmployeeHoursAwaitingApprovalProc _employeeHoursAwaitingApprovalProc;
        private CRUD.EmployeeHoursStatuesProc _employeeHoursStatuesProc;
        private CRUD.OffDayTypesProc _offDayTypesProc;
        private CRUD.CompanyDaysOffProc _companyDaysOffProc;
        public TimekeeperController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
            _employeeHoursProc = new CRUD.EmployeeHoursProc(context, userManager);
            _timekeeperNotificationsProc = new CRUD.TimekeeperNotificationsProc(context, userManager);
            _applicationUsersProc = new CRUD.ApplicationUsersProc(context, userManager);
            _employeeHoursAwaitingApprovalProc = new CRUD.EmployeeHoursAwaitingApprovalProc(context, userManager);
            _employeeHoursStatuesProc = new CRUD.EmployeeHoursStatuesProc(context, userManager);
            _offDayTypesProc = new CRUD.OffDayTypesProc(context, userManager);
            _companyDaysOffProc = new CRUD.CompanyDaysOffProc(context, userManager);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public IActionResult ReportHours(string errorMessage = null)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimeKeeperReport;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ReportHours;
            var userid = _userManager.GetUserId(User);
            var todaysEntry = _employeeHoursProc.ReadOneByDateAndUserID(DateTime.Today, userid).FirstOrDefault();
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
            else if (todaysEntry.Entry2 == null)
            {
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
            var notifications = _timekeeperNotificationsProc.ReadByUserID(userid).OrderByDescending(n => n.EmployeeHours.Date).Take(20).ToList();

            entryExitViewModel.TimekeeperNotifications = notifications;
            if (errorMessage != null)
            {
                entryExitViewModel.ErrorMessage += errorMessage;
                Response.StatusCode = 500;
                //return PartialView(entryExitViewModel);
            }
            return View(entryExitViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> ReportHours(EntryExitViewModel entryExitViewModel)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimeKeeperReport;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ReportHours;
            var currentClickButton = entryExitViewModel.EntryExitEnum;

            var updateEmployeeHours = await _employeeHoursProc.ReportHours(entryExitViewModel, _userManager.GetUserId(User));

            if (updateEmployeeHours.Bool)
            {
                return RedirectToAction();
            }
            else
            {
                return RedirectToAction("ReportHours", new { errorMessage = updateEmployeeHours.String });
            }

        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SummaryDaysOff()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryDaysOff;


            var userid = _userManager.GetUserId(User);
            var user = await _applicationUsersProc.ReadEmployeeByID(userid);
            if (user != null)
            {
                ReportDaysViewModel reportDaysViewModel = GetSummaryDaysOffModel(userid, user, DateTime.Now.Year);
                return View(reportDaysViewModel);

            }

            return RedirectToAction("ReportHours");
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _SummaryDaysOff(int year)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryDaysOff;

            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var userid = _userManager.GetUserId(User);
            var user = await _applicationUsersProc.ReadEmployeeByID(userid);
            if (user != null)
            {
                ReportDaysViewModel reportDaysViewModel = GetSummaryDaysOffModel(userid, user, year);

                return PartialView(reportDaysViewModel);
            }

            return RedirectToAction("ReportHours");
        }
        private ReportDaysViewModel GetSummaryDaysOffModel(string userid, Employee user, int year)
        {
            int month = DateTime.Now.Month;
            var daysOffViewModel = new ReportDaysViewModel
            {
                VacationDaysTaken = _employeeHoursProc.ReadOffDaysByYearOffDayTypeIDAndUserID(2, year, userid).OrderByDescending(eh => eh.Date),
                SickDaysTaken = _employeeHoursProc.ReadOffDaysByYearOffDayTypeIDAndUserID(1, year, userid).OrderByDescending(eh => eh.Date).OrderByDescending(eh => eh.Date),
                SpecialDaysTaken = _employeeHoursProc.ReadOffDaysByYearOffDayTypeIDAndUserID(4, year, userid).OrderByDescending(eh => eh.Date).OrderByDescending(eh => eh.Date),
                SelectedYear = year
            };
            var sickDaysVacationDaysLeft = getVacationSickDaysLeft(user, year);
            daysOffViewModel.VacationDays = sickDaysVacationDaysLeft.TotalVacationDays;
            daysOffViewModel.SickDays = sickDaysVacationDaysLeft.TotalSickDays;
            daysOffViewModel.VacationDaysTakenCount = sickDaysVacationDaysLeft.VacationDaysTaken;
            daysOffViewModel.SickDaysTakenCount = sickDaysVacationDaysLeft.SickDaysTaken;
            return daysOffViewModel;
        }

        private DaysOffViewModel getYearsVacationSickDays(Employee user, int year, DaysOffViewModel pastYearViewModel)
        {
            double vacationDays = 0;
            double sickDays = 0;
            double vacationDaysTaken = _employeeHoursProc.ReadOffDaysByYearOffDayTypeIDAndUserID(2, year, user.Id).Count();
            double sickDaysTaken = _employeeHoursProc.ReadOffDaysByYearOffDayTypeIDAndUserID(1, year, user.Id).Count();
            if (user.EmployeeStatusID == 1)
            {
                var vacationHours =
                    _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id &&
                    eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date)
                    .Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours)
                    .ToList().Sum(p => p);
                vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay), 2);

                var sickHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                sickDaysTaken = Math.Round(sickDaysTaken + (sickHours / user.SalariedEmployee.HoursPerDay), 2);
            }

            var unpaidLeaveTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 5 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).Count();
            if (year == user.StartedWorking.Year)
            {
                int month = year == DateTime.Now.Year ? (DateTime.Now.Month - user.StartedWorking.Month + 1) : (12 - user.StartedWorking.Month + 1);
                vacationDays = user.VacationDaysPerMonth * month;
                sickDays = user.SickDaysPerMonth * month;
            }
            else if (year == AppUtility.YearStartedTimeKeeper && year == DateTime.Now.Year)
            {
                int month = DateTime.Now.Month;
                vacationDays = (user.VacationDaysPerMonth * month) + user.RollOverVacationDays;
                sickDays = (user.SickDaysPerMonth * month) + user.RollOverSickDays;
            }
            else if (year == AppUtility.YearStartedTimeKeeper)
            {
                int month = DateTime.Now.Month;
                vacationDays = user.VacationDays + user.RollOverVacationDays;
                sickDays = user.SickDays + user.RollOverSickDays;
            }
            else if (year == DateTime.Now.Year)
            {
                int month = DateTime.Now.Month;
                vacationDays = (user.VacationDaysPerMonth * month) + pastYearViewModel.VacationDaysLeft;
                sickDays = (user.SickDaysPerMonth * month) + pastYearViewModel.SickDaysLeft;
            }
            else
            {
                vacationDays = user.VacationDays + pastYearViewModel.VacationDaysLeft;
                sickDays = user.SickDays + pastYearViewModel.SickDaysLeft;
            }
            DaysOffViewModel summaryOfDaysOff = new DaysOffViewModel
            {
                Year = year,
                TotalVacationDays = vacationDays,
                VacationDaysTaken = vacationDaysTaken,
                SickDaysTaken = sickDaysTaken,
                TotalSickDays = sickDays,
                UnpaidLeaveTaken = unpaidLeaveTaken
            };

            return summaryOfDaysOff;
        }
        private DaysOffViewModel getVacationSickDaysLeft(Employee user, int SelectedYear)
        {
            int year = AppUtility.YearStartedTimeKeeper;
            if (user.StartedWorking.Year > AppUtility.YearStartedTimeKeeper)
            {
                year = user.StartedWorking.Year;
            }

            DaysOffViewModel pastYearViewModel = new DaysOffViewModel();

            while (year <= SelectedYear)
            {
                var summaryOfDaysOff = getYearsVacationSickDays(user, year, pastYearViewModel);
                year = year + 1;
                pastYearViewModel = summaryOfDaysOff;
            }
            return pastYearViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> HoursPage(int month = 0, int year = 0, string userId = null, AppUtility.PageTypeEnum pageType = AppUtility.PageTypeEnum.TimekeeperSummary)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            if (userId == null)
            {
                userId = _userManager.GetUserId(User);
            }
            var user = await _applicationUsersProc.ReadEmployeeByID(userId);
            SummaryHoursViewModel summaryHoursViewModel = base.SummaryHoursFunction(month, year, user);
            summaryHoursViewModel.PageType = pageType;
            return PartialView(summaryHoursViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SummaryHours(int? Month, int? Year, string errorMessage = null)
        {
            var year = Year ?? DateTime.Now.Year;
            var month = Month ?? DateTime.Now.Month;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryHours;
            var userid = _userManager.GetUserId(User);
            var user = await _applicationUsersProc.ReadEmployeeByID(userid);
            //int month = Month?.Month ?? DateTime.Now.Month;
            return PartialView(base.SummaryHoursFunction(month, year, user, errorMessage));
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _EmployeeHoursAwaitingApproval(int ehaaID)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var ehaa = await _employeeHoursAwaitingApprovalProc.ReadByPK(ehaaID).Include(ehaa => ehaa.EmployeeHours).Include(ehaa => ehaa.PartialOffDayType).FirstOrDefaultAsync();

            return PartialView(ehaa);
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> ReportDaysOff()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimeKeeperReport;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ReportDaysOff;
            return View(ReportDaysOffFunction());
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _ReportDaysOff(string errorMessage = null)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return PartialView(ReportDaysOffFunction(errorMessage));
        }

        [Authorize(Roles = "TimeKeeper")]
        private async Task<SummaryOfDaysOffViewModel> ReportDaysOffFunction(string errorMessage = "")
        {
            var userid = _userManager.GetUserId(User);
            var user = await _applicationUsersProc.ReadEmployeeByID(userid);

            if (user != null)
            {
                List<DaysOffViewModel> daysOffByYear = new List<DaysOffViewModel>();
                SummaryOfDaysOffViewModel summaryOfDaysOffViewModel = new SummaryOfDaysOffViewModel();
                int year = AppUtility.YearStartedTimeKeeper;
                if (user.StartedWorking.Year > AppUtility.YearStartedTimeKeeper)
                {
                    year = user.StartedWorking.Year;
                }
                var thisYear = DateTime.Now.Year;

                DaysOffViewModel pastYearViewModel = new DaysOffViewModel();

                while (year <= thisYear)
                {
                    var summaryOfDaysOff = getYearsVacationSickDays(user, year, pastYearViewModel);
                    if (year == DateTime.Now.Year)
                    {
                        summaryOfDaysOffViewModel.VacationDaysLeft = summaryOfDaysOff.VacationDaysLeft;
                        summaryOfDaysOffViewModel.SickDaysLeft = summaryOfDaysOff.SickDaysLeft;
                    }
                    daysOffByYear.Add(summaryOfDaysOff);
                    year = year + 1;
                    pastYearViewModel = summaryOfDaysOff;

                }
                summaryOfDaysOffViewModel.DaysOffs = daysOffByYear.OrderByDescending(d => d.Year);
                summaryOfDaysOffViewModel.TotalVacationDaysPerYear = user.VacationDays;
                summaryOfDaysOffViewModel.TotalSickDaysPerYear = user.SickDays;
                summaryOfDaysOffViewModel.BonusSickDays = user.BonusSickDays;
                summaryOfDaysOffViewModel.BonusVacationDays = user.BonusVacationDays;
                summaryOfDaysOffViewModel.ErrorMessage += errorMessage;
                summaryOfDaysOffViewModel.Employee = user;
                return summaryOfDaysOffViewModel;
            }

            return null;
        }


        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> UpdateHours(DateTime chosenDate, String PageType, bool isWorkFromHome = false)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return PartialView(await GetUpdateHoursViewModel(chosenDate, PageType, isWorkFromHome));
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _UpdateHours(DateTime chosenDate, String PageType, bool isWorkFromHome = false)
        {
            return PartialView(await GetUpdateHoursViewModel(chosenDate, PageType, isWorkFromHome));
        }

        private async Task<UpdateHoursViewModel> GetUpdateHoursViewModel(DateTime chosenDate, String PageType, bool isWorkFromHome = false)
        {
            if (chosenDate == new DateTime())
            {
                chosenDate = DateTime.Today;
            }
            var userID = _userManager.GetUserId(User);
            var user = await _applicationUsersProc.ReadEmployeeByID(userID);
            var employeeHour = _employeeHoursProc.ReadOneByDateAndUserID(chosenDate.Date, userID).Include(e => e.Employee).FirstOrDefault();
            if (employeeHour == null)
            {
                employeeHour = new EmployeeHours { EmployeeID = userID, Date = chosenDate, Employee = user };
            }
            employeeHour.EmployeeHoursStatusEntry1 = _employeeHoursStatuesProc.ReadOneByPK(employeeHour.EmployeeHoursStatusEntry1ID);
            employeeHour.EmployeeHoursStatusEntry2 = _employeeHoursStatuesProc.ReadOneByPK(employeeHour.EmployeeHoursStatusEntry2ID);
            UpdateHoursViewModel updateHoursViewModel = new UpdateHoursViewModel() { EmployeeHour = employeeHour, PageType = PageType };
            if (employeeHour.Entry1 == null && employeeHour.TotalHours == null && !isWorkFromHome)
            {
                updateHoursViewModel.IsForgotToReport = true;
            }
            if (!isWorkFromHome)
            {
                updateHoursViewModel.AutoFillEntry1Type = 2;
            }
            else
            {
                updateHoursViewModel.AutoFillEntry1Type = 1;
            }
            updateHoursViewModel.PartialOffDayTypes = _offDayTypesProc.ReadManyByPKS(new List<int>() { 1, 2 });
            return updateHoursViewModel;
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> UpdateHours(UpdateHoursViewModel updateHoursViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var ehaa = _context.EmployeeHoursAwaitingApprovals.Where(eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date).FirstOrDefault();

                    var eh = _context.EmployeeHours.Where(eh => eh.EmployeeID == updateHoursViewModel.EmployeeHour.EmployeeID && eh.Date.Date == updateHoursViewModel.EmployeeHour.Date.Date).FirstOrDefault();

                    var updateHoursDate = updateHoursViewModel.EmployeeHour.Date;

                    if (ehaa == null)
                    {
                        ehaa = new EmployeeHoursAwaitingApproval();
                    }

                    ehaa.EmployeeID = updateHoursViewModel.EmployeeHour.EmployeeID;

                    if (updateHoursViewModel.EmployeeHour.Entry1 != null)
                    {
                        ehaa.Entry1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry1?.Minute ?? 0, 0);
                    }
                    else
                    {
                        ehaa.Entry1 = null;
                    }
                    if (updateHoursViewModel.EmployeeHour.Entry2 != null)
                    {
                        ehaa.Entry2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Entry2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Entry2?.Minute ?? 0, 0);
                    }
                    else
                    {
                        ehaa.Entry2 = null;
                    }
                    if (updateHoursViewModel.EmployeeHour.Exit1 != null)
                    {
                        ehaa.Exit1 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit1?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit1?.Minute ?? 0, 0);
                    }
                    else
                    {
                        ehaa.Exit1 = null;
                    }
                    if (updateHoursViewModel.EmployeeHour.Exit2 != null)
                    {
                        ehaa.Exit2 = new DateTime(updateHoursDate.Year, updateHoursDate.Month, updateHoursDate.Day, updateHoursViewModel.EmployeeHour.Exit2?.Hour ?? 0, updateHoursViewModel.EmployeeHour.Exit2?.Minute ?? 0, 0);
                    }
                    else
                    {
                        ehaa.Exit2 = null;
                    }
                    ehaa.TotalHours = updateHoursViewModel.EmployeeHour.TotalHours;
                    ehaa.Date = updateHoursViewModel.EmployeeHour.Date;
                    ehaa.EmployeeHoursStatusEntry1ID = updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry1ID;
                    ehaa.EmployeeHoursStatusEntry2ID = updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry2ID;
                    ehaa.PartialOffDayTypeID = updateHoursViewModel.EmployeeHour.PartialOffDayTypeID;
                    if (updateHoursViewModel.EmployeeHour.PartialOffDayTypeID != null && updateHoursViewModel.EmployeeHour.PartialOffDayHours == null)
                    {
                        var employeeTime = _context.Employees.Include(e => e.SalariedEmployee).Where(e => e.Id == updateHoursViewModel.EmployeeHour.EmployeeID).FirstOrDefault().SalariedEmployee.HoursPerDay;
                        var offDayHours = TimeSpan.FromHours(employeeTime) - updateHoursViewModel.EmployeeHour.TotalHours;
                        if (offDayHours > TimeSpan.Zero)
                        {
                            ehaa.PartialOffDayHours = offDayHours;
                        }
                        else
                        {
                            ehaa.PartialOffDayTypeID = null;
                        }
                    }
                    else
                    {
                        ehaa.PartialOffDayHours = updateHoursViewModel.EmployeeHour.PartialOffDayHours;
                    }
                    ehaa.IsDenied = false;
                    //mark as forgot to report if bool is true and not work from home
                    if (updateHoursViewModel.IsForgotToReport && updateHoursViewModel.EmployeeHour.EmployeeHoursStatusEntry1ID != 1)
                    {
                        if (eh != null)
                        {
                            if (eh.IsBonus)
                            {
                                ehaa.IsBonus = true;
                                ehaa.OffDayTypeID = eh.OffDayTypeID;
                            }
                            //if(eh.OffDayTypeID ==4)
                            //{
                            //    ehaa.OffDayTypeID = eh.OffDayTypeID;
                            //}
                            if (eh.OffDayTypeID == null)
                            {
                                ehaa.EmployeeHoursStatusEntry1ID = 3;
                            }
                        }
                    }
                    if (eh == null)
                    {
                        updateHoursViewModel.EmployeeHour = new EmployeeHours() { Date = updateHoursViewModel.EmployeeHour.Date, EmployeeID = updateHoursViewModel.EmployeeHour.EmployeeID };
                        _context.Update(updateHoursViewModel.EmployeeHour);
                        await _context.SaveChangesAsync();
                    }

                    var employeeHoursID = updateHoursViewModel.EmployeeHour.EmployeeHoursID;
                    ehaa.EmployeeHoursID = employeeHoursID;
                    int Month = ehaa.Date.Month;
                    int Year = ehaa.Date.Year;
                    _context.Update(ehaa);
                    await _context.SaveChangesAsync();

                    var notifications = _context.TimekeeperNotifications.Where(n => n.EmployeeHoursID == updateHoursViewModel.EmployeeHour.EmployeeHoursID).ToList();
                    foreach (var notification in notifications)
                    {
                        _context.Remove(notification);
                        await _context.SaveChangesAsync();
                    }

                    //throw new Exception();
                    await transaction.CommitAsync();
                    if (updateHoursViewModel.PageType == null || updateHoursViewModel.PageType == "ReportHours")
                    {
                        return RedirectToAction("ReportHours");
                    }
                    return RedirectToAction("SummaryHours", new { Month = Month, Year = Year });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    updateHoursViewModel.ErrorMessage += AppUtility.GetExceptionMessage(ex);
                    /*updateHoursViewModel.PartialOffDayTypes = _context.OffDayTypes.Where(od => od.OffDayTypeID == 1 *//*Sick Day*//* || od.OffDayTypeID == 2 *//*Vacation Day*//*);
                    var userID = _userManager.GetUserId(User);
                    var user = await _context.Employees.Where(u => u.Id == userID).FirstOrDefaultAsync();
                    updateHoursViewModel.EmployeeHour.Employee = user;
                    var offDayType = await _context.OffDayTypes.Where(odt => odt.OffDayTypeID == updateHoursViewModel.EmployeeHour.OffDayTypeID).FirstOrDefaultAsync();
                    updateHoursViewModel.EmployeeHour.OffDayType = offDayType;
                  */
                    Response.StatusCode = 500;
                    return PartialView("_ErrorMessage", updateHoursViewModel.ErrorMessage);

                }
            }


        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> OffDayModal(AppUtility.PageTypeEnum PageType, AppUtility.OffDayTypeEnum OffDayType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            OffDayViewModel offDayViewModel = new OffDayViewModel()
            {
                OffDayType = OffDayType,
                PageType = PageType
            };
            return PartialView(offDayViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> OffDayModal(OffDayViewModel offDayViewModel)
        {
            string errorMessage = "";
            var userId = _userManager.GetUserId(User);
            var offDayTypeID = _offDayTypesProc.ReadOneByOffDayTypeEnum(offDayViewModel.OffDayType).OffDayTypeID;
            var success = await _employeeHoursProc.SaveOffDay(offDayViewModel.FromDate, offDayViewModel.ToDate, offDayTypeID, userId);
            if (!success.Bool) {
                errorMessage = success.String;
            }
            return RedirectToAction("_ReportDaysOff", new { errorMessage });
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> OffDayConfirmModal(AppUtility.PageTypeEnum PageType, DateTime date, AppUtility.OffDayTypeEnum OffDayType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            OffDayViewModel offDayViewModel = new OffDayViewModel()
            {
                PageType = PageType,
                OffDayType = OffDayType,
                FromDate = date
            };
            return PartialView(offDayViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> ExitModal()
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
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
                    //throw new Exception();
                    await transaction.CommitAsync();
                    return PartialView(todaysEntry);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    string errorMessage = AppUtility.GetExceptionMessage(ex);
                    return RedirectToAction("ReportHours", new { errorMessage });
                }
            }
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> OffDayConfirmModal(OffDayViewModel offDayViewModel)
        {
            int offdayid = _offDayTypesProc.ReadOneByOffDayTypeEnum(offDayViewModel.OffDayType).OffDayTypeID;
            string userid = _userManager.GetUserId(User);
            var success = await _employeeHoursProc.SaveOffDay(offDayViewModel.FromDate, offDayViewModel.ToDate, offdayid, userid);
            if (!success.Bool)
            {
                offDayViewModel.ErrorMessage += success.String;
            }
            return RedirectToAction("SummaryHours", new { Month = offDayViewModel.Month, Year = offDayViewModel.FromDate.Year, errorMessage = offDayViewModel.ErrorMessage });
        }

        //private async Task SaveOffDay(DateTime dateFrom, DateTime dateTo, AppUtility.OffDayTypeEnum offDayType)
        //{
        //    var
        //    var success = _employeeHoursProc.SaveOffDay(dateFrom, dateTo, )
        //}

        //private async Task RemoveNotifications(int employeeHoursID)
        //{
        //    var success = await _timekeeperNotificationsProc.DeleteByEHID(employeeHoursID);
        //}
        //private void RemoveEmployeeBonusDay(EmployeeHours employeeHour, Employee user)
        //{
        //    if (employeeHour.OffDayTypeID == 2 && employeeHour.IsBonus)
        //    {
        //        user.BonusVacationDays += 1;
        //    }
        //    else if (employeeHour.IsBonus)
        //    {
        //        user.BonusSickDays += 1;
        //    }
        //    var success = _applicationUsersProc.UpdateEmployee(user);
        //}
        private void TakeBonusDay(Employee user, int offDayTypeID, EmployeeHours employeeHour)
        {

        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> DeleteHourModal(int? id, AppUtility.MenuItems sectionType)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            if (id == null)
            {
                ViewBag.ErrorMessage = "Employee Hour not found (no id). Unable to delete.";
                return NotFound();
            }
            var employeeHour = await _employeeHoursProc.ReadOneByPK(id);
            if (employeeHour == null)
            {
                ViewBag.ErrorMessage = "Employee Hour not found. Unable to delete";
                return NotFound();
            }
            var ehaa = await _employeeHoursAwaitingApprovalProc.ReadByPK(id).Include(ehaa => ehaa.EmployeeHours).Include(ehaa => ehaa.PartialOffDayType).FirstOrDefaultAsync();
            DeleteHourViewModel deleteHourViewModel = new DeleteHourViewModel()
            {
                EmployeeHour = employeeHour,
                SectionType = sectionType,
                Ehaa = ehaa
            };

            return PartialView(deleteHourViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> DeleteHourModal(DeleteHourViewModel deleteHourViewModel) //remove ehaa too
        {
            var success = await _employeeHoursProc.Delete(deleteHourViewModel);
            if (success.Bool)
            {
                return RedirectToAction("SummaryHours",
                    new { Month = deleteHourViewModel.EmployeeHour.Date.Month, Year = deleteHourViewModel.EmployeeHour.Date.Year });
            }
            else
            {
                //deleteHourViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                Response.StatusCode = 500;
                return RedirectToAction("SummaryHours",
                    new
                    {
                        Month = deleteHourViewModel.EmployeeHour.Date.Month,
                        Year = deleteHourViewModel.EmployeeHour.Date.Year,
                        errorMessage = success.String
                    }); ;
            }
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> DeleteNotification(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Timekeeper Notification not found (no id). Unable to delete.";
                return NotFound();
            }

            var success = await _timekeeperNotificationsProc.DeleteByPK(id);
            if (success.Bool)
            {
                return RedirectToAction("ReportHours");
            }
            else
            {
                Response.StatusCode = 500;
                return PartialView("_ErrorMessage", success.String);
            }

        }
    }
}

