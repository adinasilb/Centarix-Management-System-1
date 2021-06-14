using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Authorization;
using PrototypeWithAuth.Models;
using Microsoft.AspNetCore.Identity;
using Abp.Threading.Extensions;
using PrototypeWithAuth.ViewModels;
using static PrototypeWithAuth.AppData.AppUtility;

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrototypeWithAuth.Controllers
{
    public class ApplicationUsersController : SharedController
    {

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
            : base(context, userManager, hostingEnvironment)
        {
        }
        // GET: /<controller>/
        [HttpGet]

        [Authorize(Roles = "Requests, Operations")]
        public async Task<IActionResult> Index(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            if (await base.IsAuthorizedAsync(SectionType))
            {

                var categoryType = 1;
                if (SectionType == AppUtility.MenuItems.Operations)
                {
                    categoryType = 2;
                }
                TempData["CategoryType"] = categoryType == 1 ? AppUtility.CategoryTypeEnum.Lab : AppUtility.CategoryTypeEnum.Operations;
                if (categoryType == 1)
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
                    TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Owner;
                    TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
                }
                if (categoryType == 2)
                {
                    TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
                    TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Owner;
                    if (PageType == AppUtility.PageTypeEnum.RequestRequest || PageType == AppUtility.PageTypeEnum.OperationsRequest)
                    {
                        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsRequest;
                    }
                    else if (PageType == AppUtility.PageTypeEnum.RequestInventory || PageType == AppUtility.PageTypeEnum.OperationsInventory)
                    {
                        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsInventory;
                    }
                }
                return View(await _context.Employees.Where(u => !u.IsSuspended).ToListAsync());

            }
            else
            {
                return Redirect(base.AccessDeniedPath);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> Details()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Details;
            EmployeeDetailsViewModel employeeDetailsViewModel = GetWorkersDetailsViewModel();

            return View(employeeDetailsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> _Details()
        {
            EmployeeDetailsViewModel employeeDetailsViewModel = GetWorkersDetailsViewModel();

            return PartialView(employeeDetailsViewModel);
        }
        private EmployeeDetailsViewModel GetWorkersDetailsViewModel()
        {
            IIncludableQueryable<Employee, JobCategoryType> employees = _context.Employees.Where(u => u.EmployeeStatusID != 4).Where(u => !u.IsSuspended)
                .Include(e => e.EmployeeStatus).Include(e => e.SalariedEmployee).Include(e => e.JobSubcategoryType).ThenInclude(js => js.JobCategoryType);
            //var ed = employeeDetails.OrderBy(ed => ed.Employee.UserNum);
            EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel
            {
                Employees = employees.Select(u => new UserWithCentarixIDViewModel
                {
                    Employee = u,
                    CentarixID = AppUtility.GetEmployeeCentarixID(_context.CentarixIDs.Where(ci => ci.EmployeeID == u.Id).OrderBy(ci => ci.TimeStamp))
                }).OrderBy(ed => ed.Employee.UserNum),
                SalariedEmployeeCount = employees.Where(e => e.EmployeeStatusID == 1).Count(),
                FreelancerCount = employees.Where(e => e.EmployeeStatusID == 2).Count(),
                AdvisorCount = employees.Where(e => e.EmployeeStatusID == 3).Count(),
            };
            return employeeDetailsViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> Hours(YearlyMonthlyEnum yearlyMonthlyEnum = YearlyMonthlyEnum.Monthly, int month = 0, int year = 0)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Hours;

            WorkersHoursViewModel viewModel = await hoursPagePopulate(yearlyMonthlyEnum, month, year);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> _Hours(YearlyMonthlyEnum yearlyMonthlyEnum = YearlyMonthlyEnum.Monthly, int month = 0, int year = 0, int amountInYear = 0)
        {
            WorkersHoursViewModel viewModel = await hoursPagePopulate(yearlyMonthlyEnum, month, year, amountInYear);
            return PartialView(viewModel);
        }
        private async Task<WorkersHoursViewModel> hoursPagePopulate(YearlyMonthlyEnum yearlyMonthlyEnum = YearlyMonthlyEnum.Monthly, int month = 0, int year = 0, int amountInYear = 0)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }

            var employees = _context.Employees.Where(u => !u.IsSuspended && u.EmployeeStatusID != 4).Include(e => e.EmployeeStatus).Include(e => e.JobSubcategoryType).ThenInclude(js => js.JobCategoryType)
             .Include(e => e.SalariedEmployee).Select(e => new
             {
                 Employee = e,
                 EmployeeHours = e.EmployeeHours.Where(eh => (yearlyMonthlyEnum == YearlyMonthlyEnum.Monthly && (eh.Date.Year == year && eh.Date.Month == month && eh.Date.Date < DateTime.Now.Date))
                 || ((yearlyMonthlyEnum == YearlyMonthlyEnum.Yearly && (eh.Date.Year == year && eh.Date.Date < DateTime.Now.Date)))),
                 EmployeeHoursAwaitingApproval = e.EmployeeHours.Where(eh => (yearlyMonthlyEnum == YearlyMonthlyEnum.Monthly && (eh.Date.Year == year && eh.Date.Month == month && eh.Date.Date < DateTime.Now.Date))
                 || ((yearlyMonthlyEnum == YearlyMonthlyEnum.Yearly && (eh.Date.Year == year && eh.Date.Date < DateTime.Now.Date)))).Select(eh => eh.EmployeeHoursAwaitingApproval).Where(eha => eha != null && !eha.IsDenied)

             });



            List<WorkerHourViewModel> workerHoursViewModel = new List<WorkerHourViewModel>();
            var companyDaysOff = _context.CompanyDayOffs.ToList();
            double totalWorkingDaysInMonthOrYear;
            double totalWorkingDaysInYear = amountInYear == 0 ? SharedController.GetTotalWorkingDaysThisYear(new DateTime(year, 1, 1), companyDaysOff) : amountInYear;
            if (yearlyMonthlyEnum == YearlyMonthlyEnum.Monthly)
            {
                totalWorkingDaysInMonthOrYear = SharedController.GetTotalWorkingDaysThisMonth(new DateTime(year, month, 1), companyDaysOff);
            }
            else
            {
                totalWorkingDaysInMonthOrYear = totalWorkingDaysInYear;
            }
            List<Task> listOfTasks = new List<Task>();
            await employees.ForEachAsync(employee =>
            {
                int wholeVacationDays = 0;
                double totalVacationDays = 0;
                double workingDays = 0;
                int wholeSickDays = 0;
                double totalSickDays = 0;
                double sickHours;
                double vacationSickCount = 0;
                double partialOffDayPercent = 0;
                int missingDays = 0;
                int specialDays = 0;
                int unpaidLeave = 0;
                double vacationHours;
                TimeSpan hours = new TimeSpan();
                IEnumerable<EmployeeHours> employeeHoursOfMonthOrYear;
                if (yearlyMonthlyEnum == YearlyMonthlyEnum.Monthly)
                {
                    workingDays = employee.EmployeeHours.Where(eh => (eh.OffDayTypeID == null) || (eh.IsBonus && eh.OffDayTypeID != null)).Where(eh => (eh.Exit1 != null || eh.TotalHours != null)).Count();
                }
                else
                {
                    workingDays = employee.EmployeeHours.Where(eh => (eh.OffDayTypeID == null) || (eh.IsBonus && eh.OffDayTypeID != null)).Where(eh => (eh.Exit1 != null || eh.TotalHours != null)).Count();
                }
                if (employee.Employee.EmployeeStatusID == 1)
                {

                    missingDays = employee.EmployeeHours.Where(eh => (eh.Entry1 == null && eh.OffDayTypeID == null && eh.TotalHours == null && eh.CompanyDayOffID == null)
                    || (eh.Entry1 != null && eh.Exit1 == null)).Count() - employee.EmployeeHoursAwaitingApproval.Count();

                    wholeSickDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 1/*&& eh.IsBonus == false*/).Count();
                    sickHours = employee.EmployeeHours.Where(eh => eh.PartialOffDayTypeID == 1/*&& eh.IsBonus == false*/).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                    totalSickDays = Math.Round(wholeSickDays + (sickHours / employee.Employee.SalariedEmployee?.HoursPerDay ?? 1), 2);

                    wholeVacationDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 2/* && eh.IsBonus == false*/).Count();
                    vacationHours = employee.EmployeeHours.Where(eh => eh.PartialOffDayTypeID == 2 /*&& eh.IsBonus == false*/).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                    totalVacationDays = Math.Round(wholeVacationDays + (vacationHours / employee.Employee.SalariedEmployee?.HoursPerDay ?? 1), 2);

                    partialOffDayPercent = Math.Round((sickHours + vacationHours) / employee.Employee.SalariedEmployee?.HoursPerDay ?? 1, 2);

                    hours = new TimeSpan(employee.EmployeeHours.Select(eh => new { TimeSpan = eh.TotalHours?.Ticks ?? 0 }).Sum(a => a.TimeSpan));
                    specialDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 4).Count();
                    unpaidLeave = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 5).Count();
                    vacationSickCount = totalSickDays + totalVacationDays + specialDays + unpaidLeave;

                    workingDays = workingDays - partialOffDayPercent;
                }
                WorkerHourViewModel worker = new WorkerHourViewModel
                {
                    Employee = employee.Employee,
                    SickDays = totalSickDays,
                    UnpaidLeave = unpaidLeave,
                    VacationDays = totalVacationDays,
                    WorkingDays = workingDays,
                    Hours = hours,
                    VacationSickCount = vacationSickCount,
                    MissingDays = missingDays
                };
                workerHoursViewModel.Add(worker);
            });
            WorkersHoursViewModel viewModel = new WorkersHoursViewModel
            {
                Year = year,
                Month = month,
                YearlyMonthlyEnum = yearlyMonthlyEnum,
                Employees = workerHoursViewModel,
                Months = Enumerable.Range(1, 12).ToList(),
                Years = Enumerable.Range(AppUtility.YearStartedTimeKeeper, DateTime.Today.Year - (AppUtility.YearStartedTimeKeeper - 1)).ToList(),
                TotalWorkingDaysInMonthOrYear = (int)totalWorkingDaysInMonthOrYear
            };
            return viewModel;
        }


        [Authorize(Roles = "Users")]
        public async Task<IActionResult> UserHours(string userId, int month, int year)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Hours;

            Employee user = _context.Employees.Include(e => e.SalariedEmployee).Where(e => e.Id == userId).FirstOrDefault();

            SummaryHoursViewModel viewModel = base.SummaryHoursFunction(month, year, user);
            viewModel.PageType = PageTypeEnum.UsersWorkers;
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> Salary()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Salary;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> AwaitingApproval()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.AwaitingApproval;

            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = GetAwaitingApprovalModel();
            return View(awaitingApproval);
        }
        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> _AwaitingApproval(string? ErrorMessage = null)
        {
            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = GetAwaitingApprovalModel();
            if (awaitingApproval.Count > 0)
            {
                awaitingApproval[0].ErrorMessage = ErrorMessage;
            }

            return PartialView(awaitingApproval);
        }
        private List<EmployeeHoursAwaitingApprovalViewModel> GetAwaitingApprovalModel()
        {
            var employeeHoursAwaitingApproval = _context.EmployeeHoursAwaitingApprovals.Include(ehwa => ehwa.Employee).Include(ehwa => ehwa.EmployeeHours).Include(ehwa => ehwa.EmployeeHoursStatusEntry1).Include(ehwa => ehwa.EmployeeHoursStatusEntry2)
                .Include(ehwa => ehwa.PartialOffDayType).ToList()
                .Where(ehwa => !ehwa.IsDenied);
            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = new List<EmployeeHoursAwaitingApprovalViewModel>();
            foreach (EmployeeHoursAwaitingApproval ehaa in employeeHoursAwaitingApproval)
            {
                bool entry1 = false;
                bool entry2 = false;
                bool exit1 = false;
                bool exit2 = false;
                bool totalHours = false;
                bool partialHours = false;
                if (ehaa.EmployeeHours?.Entry1?.ZeroSeconds().TimeOfDay != ehaa.Entry1?.TimeOfDay)
                {
                    entry1 = true;
                }
                if (ehaa.EmployeeHours?.Entry2?.ZeroSeconds().TimeOfDay != ehaa.Entry2?.TimeOfDay)
                {
                    entry2 = true;
                }
                if (ehaa.EmployeeHours?.Exit1?.ZeroSeconds().TimeOfDay != ehaa.Exit1?.TimeOfDay)
                {
                    exit1 = true;
                }
                if (ehaa.EmployeeHours?.Exit2?.ZeroSeconds().TimeOfDay != ehaa.Exit2?.TimeOfDay)
                {
                    exit2 = true;
                }
                if (ehaa.EmployeeHours?.TotalHours != ehaa.TotalHours)
                {
                    totalHours = true;
                }
                if (ehaa.EmployeeHours?.PartialOffDayHours != ehaa.PartialOffDayHours)
                {
                    partialHours = true;
                }
                EmployeeHoursAwaitingApprovalViewModel viewModel = new EmployeeHoursAwaitingApprovalViewModel
                {
                    Entry1 = entry1,
                    Entry2 = entry2,
                    Exit1 = exit1,
                    Exit2 = exit2,
                    TotalHours = totalHours,
                    PartialHours = partialHours,
                    EmployeeHoursAwaitingApproval = ehaa
                };
                awaitingApproval.Add(viewModel);
            }

            return awaitingApproval;
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> ApproveHours(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    EmployeeHours employeeHours = new EmployeeHours();
                    EmployeeHoursAwaitingApproval employeeHoursBeingApproved = await _context.EmployeeHoursAwaitingApprovals.Where(ehaa => ehaa.EmployeeHoursAwaitingApprovalID == id).FirstOrDefaultAsync();
                    employeeHours = await _context.EmployeeHours.Where(eh => eh.EmployeeHoursID == employeeHoursBeingApproved.EmployeeHoursID).FirstOrDefaultAsync();
                    var user = await _context.Employees.Include(e => e.SalariedEmployee).Where(e => e.Id == employeeHoursBeingApproved.EmployeeID).FirstOrDefaultAsync();
                    if (employeeHoursBeingApproved.OffDayTypeID != null)
                    {
                        //add back to the bonus days
                        if (employeeHoursBeingApproved.IsBonus)
                        {
                            if (employeeHoursBeingApproved.OffDayTypeID == 2)
                            {
                                user.BonusVacationDays += 1;
                            }
                            else
                            {
                                user.BonusSickDays += 1;
                            }

                        }

                        if (employeeHoursBeingApproved.OffDayTypeID == 4)
                        {
                            user.SpecialDays += 1;
                        }

                        employeeHoursBeingApproved.OffDayTypeID = null;
                        _context.Update(user);
                    }
                    if (employeeHours?.PartialOffDayTypeID != null)
                    {
                        ReturnPartialBonusDay(user, employeeHoursBeingApproved.PartialOffDayTypeID ?? 2, employeeHours);
                    }
                    if (employeeHoursBeingApproved.PartialOffDayTypeID != null)
                    {
                        var partialHours = employeeHours.PartialOffDayHours?.TotalHours ?? 0;
                        var days = Math.Round(partialHours / user.SalariedEmployee.HoursPerDay, 2);
                        var vacationLeftCount = base.GetUsersOffDaysLeft(user, employeeHoursBeingApproved.PartialOffDayTypeID ?? 2, employeeHoursBeingApproved.Date.Year);
                        /*if (vacationLeftCount < days)
                        {
                            TakePartialBonusDay(user, employeeHoursBeingApproved.PartialOffDayTypeID ?? 2, employeeHoursBeingApproved, days);
                        }*/
                    }
                    if (employeeHours == null)
                    {
                        employeeHours = new EmployeeHours
                        {
                            Entry1 = employeeHoursBeingApproved.Entry1,
                            Entry2 = employeeHoursBeingApproved.Entry2,
                            Exit1 = employeeHoursBeingApproved.Exit1,
                            Exit2 = employeeHoursBeingApproved.Exit2,
                            TotalHours = employeeHoursBeingApproved.TotalHours,
                            EmployeeHoursStatusEntry1ID = employeeHoursBeingApproved.EmployeeHoursStatusEntry1ID,
                            EmployeeHoursStatusEntry2ID = employeeHoursBeingApproved.EmployeeHoursStatusEntry2ID,
                            EmployeeID = employeeHoursBeingApproved.EmployeeID,
                            Date = employeeHoursBeingApproved.Date,
                            EmployeeHoursID = employeeHoursBeingApproved.EmployeeHoursID,
                            PartialOffDayTypeID = employeeHoursBeingApproved.PartialOffDayTypeID,
                            PartialOffDayHours = employeeHoursBeingApproved.PartialOffDayHours,
                            OffDayTypeID = employeeHoursBeingApproved.OffDayTypeID,
                            IsBonus = employeeHoursBeingApproved.IsBonus
                        };
                    }
                    else
                    {
                        employeeHours.Entry1 = employeeHoursBeingApproved.Entry1;
                        employeeHours.Entry2 = employeeHoursBeingApproved.Entry2;
                        employeeHours.Exit1 = employeeHoursBeingApproved.Exit1;
                        employeeHours.Exit2 = employeeHoursBeingApproved.Exit2;
                        employeeHours.TotalHours = employeeHoursBeingApproved.TotalHours;
                        employeeHours.EmployeeHoursStatusEntry1ID = employeeHoursBeingApproved.EmployeeHoursStatusEntry1ID;
                        employeeHours.EmployeeHoursStatusEntry2ID = employeeHoursBeingApproved.EmployeeHoursStatusEntry2ID;
                        employeeHours.EmployeeID = employeeHoursBeingApproved.EmployeeID;
                        employeeHours.Date = employeeHoursBeingApproved.Date;
                        employeeHours.EmployeeHoursID = employeeHoursBeingApproved.EmployeeHoursID;
                        employeeHours.PartialOffDayTypeID = employeeHoursBeingApproved.PartialOffDayTypeID;
                        employeeHours.PartialOffDayHours = employeeHoursBeingApproved.PartialOffDayHours;
                        employeeHours.OffDayTypeID = employeeHoursBeingApproved.OffDayTypeID;
                        employeeHours.IsBonus = employeeHoursBeingApproved.IsBonus;
                    }

                    try
                    {
                        _context.Update(employeeHours);
                        await _context.SaveChangesAsync();
                        _context.Remove(employeeHoursBeingApproved);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    //throw new Exception();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return RedirectToAction("_AwaitingApproval", new { ErrorMessage = AppUtility.GetExceptionMessage(ex) });

                }
            }
            return RedirectToAction("_AwaitingApproval");
        }


        private void TakePartialBonusDay(Employee user, int offDayTypeID, EmployeeHoursAwaitingApproval employeeHour, double days)
        {
            if (offDayTypeID == 2)
            {
                if (user.BonusVacationDays >= days)
                {
                    employeeHour.IsBonus = true;
                    user.BonusVacationDays -= days;
                    _context.Update(user);
                }
            }
            else
            {
                if (user.BonusSickDays >= days)
                {
                    employeeHour.IsBonus = true;
                    user.BonusSickDays -= days;
                    _context.Update(user);
                }
            }
        }
        private void ReturnPartialBonusDay(Employee user, int offDayTypeID, EmployeeHours employeeHour)
        {
            var partialHours = employeeHour.PartialOffDayHours?.TotalHours ?? 0;
            var days = Math.Round(partialHours / user.SalariedEmployee.HoursPerDay, 2);
            if (offDayTypeID == 2)
            {
                if (user.BonusVacationDays >= days)
                {
                    employeeHour.IsBonus = true;
                    user.BonusVacationDays += days;
                    _context.Update(user);
                }
            }
            else
            {
                if (user.BonusSickDays >= days)
                {
                    employeeHour.IsBonus = true;
                    user.BonusSickDays += days;
                    _context.Update(user);
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> DenyApprovalRequestModal(int ehaaID)
        {
            var ehaa = _context.EmployeeHoursAwaitingApprovals
                .Include(ehaa => ehaa.EmployeeHours)
                .Where(ehaa => ehaa.EmployeeHoursAwaitingApprovalID == ehaaID).FirstOrDefault();
            return PartialView(ehaa);
        }

        [HttpPost]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> DenyApprovalRequestModal(EmployeeHoursAwaitingApproval employeeHoursAwaitingApproval)
        {
            EmployeeHoursAwaitingApproval employeeHoursBeingApproved = await _context.EmployeeHoursAwaitingApprovals
                .Where(ehaa => ehaa.EmployeeHoursAwaitingApprovalID == employeeHoursAwaitingApproval.EmployeeHoursAwaitingApprovalID)
                .FirstOrDefaultAsync();

            employeeHoursBeingApproved.IsDenied = true;

            try
            {
                _context.Update(employeeHoursBeingApproved);
                await _context.SaveChangesAsync();

                TimekeeperNotification notification = new TimekeeperNotification()
                {
                    EmployeeHoursID = employeeHoursBeingApproved.EmployeeHoursID,
                    IsRead = false,
                    ApplicationUserID = employeeHoursBeingApproved.EmployeeID,
                    Description = "update hours request denied for " + AppUtility.FormatDate(employeeHoursBeingApproved.Date),
                    NotificationStatusID = 5,
                    TimeStamp = DateTime.Now,
                    Controller = "Timekeeper",
                    Action = "SummaryHours"
                };
                _context.Add(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return RedirectToAction("_AwaitingApproval", new { ErrorMessage = AppUtility.GetExceptionMessage(ex) });
            }

            return RedirectToAction("_AwaitingApproval");
        }


    }

}