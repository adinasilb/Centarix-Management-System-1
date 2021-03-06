using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class TimekeeperController : SharedController
    {
        public TimekeeperController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
           : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
          }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> ReportHours(string errorMessage = null)
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimeKeeperReport;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ReportHours;
            var userid = _userManager.GetUserId(User);
            var todaysEntry = await _employeeHoursProc.ReadOneAsync(new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.Date.Date == DateTime.Today, eh => eh.Employee.Id==userid });
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
            var notifications = _timekeeperNotificationsProc.Read(new List<Expression<Func<TimekeeperNotification, bool>>> { n => n.ApplicationUserID == userid },
            new List<ComplexIncludes<TimekeeperNotification, ModelBase>> { new ComplexIncludes<TimekeeperNotification, ModelBase>{ Include = tn => tn.EmployeeHours } }).OrderByDescending(n => n.EmployeeHours.Date).Take(20).ToList();
            
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

            var updateEmployeeHours = await _employeeHoursProc.ReportHoursAsync(entryExitViewModel, _userManager.GetUserId(User));

            return RedirectToAction("ReportHours", new { errorMessage = updateEmployeeHours.String });

        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> SummaryDaysOff()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimekeeperSummary;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.SummaryDaysOff;


            var userid = _userManager.GetUserId(User);
            var user = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id==userid },
                new List<ComplexIncludes<Employee, ModelBase>>{ new ComplexIncludes<Employee, ModelBase> { Include= e => e.SalariedEmployee } });
            if (user != null)
            {
                ReportDaysViewModel reportDaysViewModel = await GetSummaryDaysOffModel(userid, user, DateTime.Now.Year);
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
            var user = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id==userid },  
                new List<ComplexIncludes<Employee, ModelBase>>{ new ComplexIncludes<Employee, ModelBase> { Include = e => e.SalariedEmployee } });
            if (user != null)
            {
                ReportDaysViewModel reportDaysViewModel = await GetSummaryDaysOffModel(userid, user, year);

                return PartialView(reportDaysViewModel);
            }

            return RedirectToAction("ReportHours");
        }
        private async Task<ReportDaysViewModel> GetSummaryDaysOffModel(string userid, Employee user, int year)
        {
            int month = DateTime.Now.Month;
            var daysOffViewModel = new ReportDaysViewModel
            {
                VacationDaysTaken = _employeeHoursProc.ReadOffDaysByYear(year, 2, userid).OrderByDescending(eh => eh.Date),
                SickDaysTaken = _employeeHoursProc.ReadOffDaysByYear(year, 1, userid).OrderByDescending(eh => eh.Date).OrderByDescending(eh => eh.Date),
                SpecialDaysTaken = _employeeHoursProc.ReadOffDaysByYear(year, 4, userid).OrderByDescending(eh => eh.Date).OrderByDescending(eh => eh.Date),
                SelectedYear = year
            };
            var sickDaysVacationDaysLeft = await getVacationSickDaysLeft(user, year);
            daysOffViewModel.VacationDays = sickDaysVacationDaysLeft.TotalVacationDays;
            daysOffViewModel.SickDays = sickDaysVacationDaysLeft.TotalSickDays;
            daysOffViewModel.VacationDaysTakenCount = sickDaysVacationDaysLeft.VacationDaysTaken;
            daysOffViewModel.SickDaysTakenCount = sickDaysVacationDaysLeft.SickDaysTaken;
            return daysOffViewModel;
        }

        private async Task<DaysOffViewModel> getYearsVacationSickDays(Employee user, int year, DaysOffViewModel pastYearViewModel)
        {
            double vacationDays = 0;
            double sickDays = 0;
            double vacationDaysTaken = _employeeHoursProc.ReadOffDaysByYear(year, 2, user.Id).Count();
            double sickDaysTaken = _employeeHoursProc.ReadOffDaysByYear(year, 1, user.Id).Count();
            if (user.EmployeeStatusID == 1)
            {
                var vacationHours = await _employeeHoursProc.ReadPartialOffDayHoursByYearAsync(year, 2, user.Id);
                vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay), 2);

                var sickHours = await _employeeHoursProc.ReadPartialOffDayHoursByYearAsync(year, 1, user.Id);
                sickDaysTaken = Math.Round(sickDaysTaken + (sickHours / user.SalariedEmployee.HoursPerDay), 2);
            }

            var unpaidLeaveTaken = Convert.ToInt32(_employeeHoursProc.ReadOffDaysByYear(year, 5, user.Id).Count());
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
        private async Task<DaysOffViewModel> getVacationSickDaysLeft(Employee user, int SelectedYear)
        {
            int year = AppUtility.YearStartedTimeKeeper;
            if (user.StartedWorking.Year > AppUtility.YearStartedTimeKeeper)
            {
                year = user.StartedWorking.Year;
            }

            DaysOffViewModel pastYearViewModel = new DaysOffViewModel();

            while (year <= SelectedYear)
            {
                var summaryOfDaysOff = await getYearsVacationSickDays(user, year, pastYearViewModel);
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
            var user = await _employeesProc.ReadOneAsync( new List<Expression<Func<Employee, bool>>> { e => e.Id==userId },
               new List<ComplexIncludes<Employee, ModelBase>> { new ComplexIncludes<Employee, ModelBase> { Include = e => e.SalariedEmployee } });
            SummaryHoursViewModel summaryHoursViewModel = await base.SummaryHoursFunctionAsync(month, year, user);
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
            var user = await _employeesProc.ReadOneAsync( new List<Expression<Func<Employee, bool>>> { e => e.Id== userid },
                 new List<ComplexIncludes<Employee, ModelBase>> { new ComplexIncludes<Employee, ModelBase> { Include= e => e.SalariedEmployee } });
            //int month = Month?.Month ?? DateTime.Now.Month;
            return PartialView(await base.SummaryHoursFunctionAsync(month, year, user, errorMessage));
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _EmployeeHoursAwaitingApproval(int ehaaID)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var ehaa = await _employeeHoursAwaitingApprovalProc.ReadOneAsync(new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { ehaa => ehaa.EmployeeHoursAwaitingApprovalID == ehaaID },
                new List<ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase>> { new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include =ehaa => ehaa.EmployeeHours },
                    new  ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase>{ Include = ehaa => ehaa.PartialOffDayType } });
             
            return PartialView(ehaa);
        }

        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> ReportDaysOff()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.TimeKeeper;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.TimeKeeperReport;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.ReportDaysOff;
            return View(await ReportDaysOffFunctionAsync());
        }
        [HttpGet]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> _ReportDaysOff(string errorMessage = null)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return PartialView(await ReportDaysOffFunctionAsync(errorMessage));
        }

        [Authorize(Roles = "TimeKeeper")]
        private async Task<SummaryOfDaysOffViewModel> ReportDaysOffFunctionAsync(string errorMessage = "")
        {
            var userid = _userManager.GetUserId(User);
            var user = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id== userid },
                 new List<ComplexIncludes<Employee, ModelBase>> { new ComplexIncludes<Employee, ModelBase> { Include= e => e.SalariedEmployee } });

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
                    var summaryOfDaysOff = await getYearsVacationSickDays(user, year, pastYearViewModel);
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
            var user = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id== userID });
            var employeeHour = await _employeeHoursProc.ReadOneAsync( new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.Date.Date== chosenDate.Date, eh => eh.Employee.Id == userID }, new List<ComplexIncludes<EmployeeHours, ModelBase>>{ new ComplexIncludes<EmployeeHours, ModelBase> { Include = e => e.Employee }, new ComplexIncludes<EmployeeHours, ModelBase>{ Include = e => e.OffDayType } });
            if (employeeHour == null)
            {
                employeeHour = new EmployeeHours { EmployeeID = userID, Date = chosenDate, Employee = user };
            }
            employeeHour.EmployeeHoursStatusEntry1 =await _employeeHoursStatuesProc.ReadOneAsync( new List<Expression<Func<EmployeeHoursStatus, bool>>> { ehs => ehs.EmployeeHoursStatusID == employeeHour.EmployeeHoursStatusEntry1ID });
            employeeHour.EmployeeHoursStatusEntry2 = await _employeeHoursStatuesProc.ReadOneAsync( new List<Expression<Func<EmployeeHoursStatus, bool>>> { ehs => ehs.EmployeeHoursStatusID == employeeHour.EmployeeHoursStatusEntry2ID });
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
            updateHoursViewModel.PartialOffDayTypes = _offDayTypesProc.Read( new List<Expression<Func<OffDayType, bool>>> { od => new List<int>() { 1, 2 }.Contains(od.OffDayTypeID) });
            return updateHoursViewModel;
        }

        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> UpdateHours(UpdateHoursViewModel updateHoursViewModel)
        {
            var success = await _employeeHoursProc.UpdateHoursAsync(updateHoursViewModel);
            if (success.Bool)
            {
                if (updateHoursViewModel.PageType == null || updateHoursViewModel.PageType == "ReportHours")
                {
                    return RedirectToAction("ReportHours");
                }
                return RedirectToAction("SummaryHours", new { Month = updateHoursViewModel.EmployeeHour.Date.Month, Year = updateHoursViewModel.EmployeeHour.Date.Year });
            }
            else
            {
                Response.StatusCode = 500;
                return PartialView("_ErrorMessage", success.String /*updateHoursViewModel.ErrorMessage*/);
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
            var offDayType = await _offDayTypesProc.ReadOneAsync( new List<Expression<Func<OffDayType, bool>>> { odt => odt.Description == AppUtility.GetDisplayNameOfEnumValue(offDayViewModel.OffDayType.ToString())});
            var offDayTypeID = offDayType.OffDayTypeID;
            var success = await _employeeHoursProc.SaveOffDayAsync(offDayViewModel.FromDate, offDayViewModel.ToDate, offDayTypeID, userId);
            if (!success.Bool)
            {
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

            var userID = _userManager.GetUserId(User);
            var todaysEntry = await _employeeHoursProc.ReadOneAsync( new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.Date.Date == DateTime.Now.Date && eh.EmployeeID == userID });
            var success = new StringWithBool();
            using (var transaction = _applicationDbContextTransaction.Transaction)
            {
                try
                {
                    if (todaysEntry.Exit1 == null)
                    {
                        todaysEntry.Exit1 = DateTime.Now;
                        success = await _employeeHoursProc.UpdateAsync(todaysEntry);
                    }

                    else if (todaysEntry.Exit2 == null)
                    {
                        todaysEntry.Exit2 = DateTime.Now;
                        success = await _employeeHoursProc.UpdateAsync(todaysEntry);
                    }
                    await transaction.CommitAsync();
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    success.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
                }
            }
            
            if (success.Bool)
            {
                return PartialView(todaysEntry);
            }
            else
            {
                string errorMessage = success.String;
                return RedirectToAction("ReportHours", new { errorMessage });
            }
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> OffDayConfirmModal(OffDayViewModel offDayViewModel)
        {
            var offday = await _offDayTypesProc.ReadOneAsync(new List<Expression<Func<OffDayType, bool>>> { odt => odt.Description == AppUtility.GetDisplayNameOfEnumValue(offDayViewModel.OffDayType.ToString()) });
            int offdayid = offday.OffDayTypeID;
            string userid = _userManager.GetUserId(User);
            var success = await _employeeHoursProc.SaveOffDayAsync(offDayViewModel.FromDate, offDayViewModel.ToDate, offdayid, userid);
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
            var employeeHour = await _employeeHoursProc.ReadOneAsync( new List<Expression<Func<EmployeeHours, bool>>> { eh => eh.EmployeeHoursID ==  id }, new List<ComplexIncludes<EmployeeHours, ModelBase>>{
                new ComplexIncludes<EmployeeHours, ModelBase>{ Include = eh => eh.PartialOffDayType }, new ComplexIncludes<EmployeeHours, ModelBase>{Include = eh => eh.OffDayType },
                    new ComplexIncludes<EmployeeHours, ModelBase> {Include = eh=>eh.EmployeeHoursAwaitingApproval }, new ComplexIncludes<EmployeeHours, ModelBase>{ Include = eh=>eh.EmployeeHoursAwaitingApproval.PartialOffDayType }});
            if (employeeHour == null)
            {
                ViewBag.ErrorMessage = "Employee Hour not found. Unable to delete";
                return NotFound();
            }
            var ehaa = employeeHour.EmployeeHoursAwaitingApproval;
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
            var success = await _employeeHoursProc.DeleteAsync(deleteHourViewModel);
            if (!success.Bool)
            {
                Response.StatusCode = 500;
            }
            return RedirectToAction("SummaryHours",
                   new { Month = deleteHourViewModel.EmployeeHour.Date.Month, Year = deleteHourViewModel.EmployeeHour.Date.Year, errorMessage = success.String});
        }
        [HttpPost]
        [Authorize(Roles = "TimeKeeper")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var success = await _timekeeperNotificationsProc.DeleteByPKAsync(id);

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

