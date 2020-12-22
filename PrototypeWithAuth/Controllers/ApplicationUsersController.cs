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
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrototypeWithAuth.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: /<controller>/
        [HttpGet]

        [Authorize(Roles = "Users")]
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, int categoryType = 1)
        {
            TempData["CategoryType"] = categoryType == 1 ? AppUtility.CategoryTypeEnum.Lab : AppUtility.CategoryTypeEnum.Operations;
            if (categoryType == 1)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
                TempData["SidebarTitle"] = AppUtility.OrdersAndInventorySidebarEnum.Owner;
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
            }
            if (categoryType == 2)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.OperationsSidebarEnum.Owner;
                if (PageType == AppUtility.RequestPageTypeEnum.Request || PageType.ToString() == AppUtility.OperationsPageTypeEnum.RequestOperations.ToString())
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.OperationsPageTypeEnum.RequestOperations;
                }
                else if (PageType == AppUtility.RequestPageTypeEnum.Inventory || PageType.ToString() == AppUtility.OperationsPageTypeEnum.InventoryOperations.ToString())
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.OperationsPageTypeEnum.InventoryOperations;
                }
            }
            return View(await _context.Employees.Where(u => !u.IsSuspended).ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> Details()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.UserPageTypeEnum.Workers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.UserSideBarEnum.WorkersDetails;
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
            IIncludableQueryable<Employee, JobCategoryType> employees = _context.Users.OfType<Employee>().Where(u => u.EmployeeStatusID != 4).Where(u => !u.IsSuspended)
                .Include(e => e.EmployeeStatus).Include(e => e.SalariedEmployee).Include(e => e.JobCategoryType);
            EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel
            {
                Employees = employees.ToList(),
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
            WorkersHoursViewModel viewModel = hoursPagePopulate(yearlyMonthlyEnum, month, year);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> _Hours(YearlyMonthlyEnum yearlyMonthlyEnum = YearlyMonthlyEnum.Monthly, int month = 0, int year = 0, int amountInYear = 0)
        {
            WorkersHoursViewModel viewModel = hoursPagePopulate(yearlyMonthlyEnum, month, year, amountInYear);
            return PartialView(viewModel);
        }
        private WorkersHoursViewModel hoursPagePopulate(YearlyMonthlyEnum yearlyMonthlyEnum = YearlyMonthlyEnum.Monthly, int month = 0, int year = 0, int amountInYear = 0)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.UserPageTypeEnum.Workers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.UserSideBarEnum.WorkersHours;
            IIncludableQueryable<Employee, SalariedEmployee> employees = _context.Users.OfType<Employee>().Where(u => !u.IsSuspended)
              .Include(e => e.EmployeeStatus).Include(e => e.JobCategoryType).Include(e => e.EmployeeHours).Include(e => e.SalariedEmployee);
            List<WorkerHourViewModel> workerHoursViewModel = new List<WorkerHourViewModel>();
            foreach (Employee employee in employees)
            {
                int vacationDays = 0;
                int workDays = 0;
                int sickDays = 0;
                int vacationSickCount = 0;
                TimeSpan hours = new TimeSpan();
                switch (yearlyMonthlyEnum)
                {
                    case YearlyMonthlyEnum.Monthly:
                        sickDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 1 && eh.Date.Year == year && eh.Date.Month == month).Count();
                        vacationDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 2 && eh.Date.Year == year && eh.Date.Month == month).Count();
                        workDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == null && eh.Date.Year == year && eh.Date.Month == month).Count();
                        hours = new TimeSpan(employee.EmployeeHours.Where(eh => eh.Date.Year == year && eh.Date.Month == month).Select(eh => new { TimeSpan = eh.TotalHours?.Ticks ?? 0 }).Sum(a => a.TimeSpan));
                        vacationSickCount = employee.EmployeeHours.Where(eh => eh.Date.Month == month && eh.Date.Year == year && (eh.OffDayTypeID == 2 || eh.OffDayTypeID == 1) && eh.Date <= DateTime.Now.Date)?.Count()??0;
                        break;
                    case YearlyMonthlyEnum.Yearly:
                        sickDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 1 && eh.Date.Year == year).Count();
                        vacationDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 2 && eh.Date.Year == year).Count();
                        workDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == null && eh.Date.Year == year).Count();
                        hours = new TimeSpan(employee.EmployeeHours.Where(eh => eh.Date.Year == year).Select(eh => new { TimeSpan = eh.TotalHours?.Ticks ?? 0 }).Sum(a => a.TimeSpan));
                        vacationSickCount = employee.EmployeeHours.Where(eh => eh.Date.Year == year && (eh.OffDayTypeID == 2 || eh.OffDayTypeID == 1) && eh.Date <= DateTime.Now.Date)?.Count()??0;
                        break;
                }

                WorkerHourViewModel worker = new WorkerHourViewModel
                {
                    Employee = employee,
                    SickDays = sickDays,
                    VacationDays = vacationDays,
                    WorkingDays = workDays,
                    Hours = hours,
                    VacationSickCount = vacationSickCount
                };
                workerHoursViewModel.Add(worker);
            }
            WorkersHoursViewModel viewModel = new WorkersHoursViewModel
            {
                Year = year,
                Month = month,
                YearlyMonthlyEnum = yearlyMonthlyEnum,
                Employees = workerHoursViewModel,
                Months = Enumerable.Range(1, 12).ToList(),
                Years = Enumerable.Range(2010, DateTime.Today.Year - 2009).ToList(),
                TotalWorkingDaysInMonth = AppUtility.GetTotalWorkingDaysThisMonth(new DateTime(DateTime.Now.Year, month, 1), _context.CompanyDayOffs, 0),
                TotalWorkingDaysInYear = amountInYear == 0 ? AppUtility.GetTotalWorkingDaysThisYear(new DateTime(year, 1, 1), _context.CompanyDayOffs, 0) : amountInYear
            };
            return viewModel;
        }
        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> Salary()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.UserPageTypeEnum.Workers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.UserSideBarEnum.WorkersSalary;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> AwaitingApproval()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.UserPageTypeEnum.Workers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.UserSideBarEnum.WorkersAwaitingApproval;

            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = GetAwaitingApprovalModel();
            return View(awaitingApproval);
        }
        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> _AwaitingApproval()
        {
            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = GetAwaitingApprovalModel();
            return PartialView(awaitingApproval);
        }
        private List<EmployeeHoursAwaitingApprovalViewModel> GetAwaitingApprovalModel()
        {
            var employeeHoursAwaitingApproval = _context.EmployeeHoursAwaitingApprovals.Include(ehwa => ehwa.Employee).Include(ehwa => ehwa.EmployeeHours).Include(ehwa => ehwa.EmployeeHoursStatus).ToList();
            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = new List<EmployeeHoursAwaitingApprovalViewModel>();
            foreach (EmployeeHoursAwaitingApproval ehaa in employeeHoursAwaitingApproval)
            {
                bool entry1 = false;
                bool entry2 = false;
                bool exit1 = false;
                bool exit2 = false;
                bool totalHours = false;
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
                EmployeeHoursAwaitingApprovalViewModel viewModel = new EmployeeHoursAwaitingApprovalViewModel
                {
                    Entry1 = entry1,
                    Entry2 = entry2,
                    Exit1 = exit1,
                    Exit2 = exit2,
                    TotalHours = totalHours,
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
            EmployeeHours employeeHours = new EmployeeHours();
            EmployeeHoursAwaitingApproval employeeHoursBeingApproved = await _context.EmployeeHoursAwaitingApprovals.Where(ehaa => ehaa.EmployeeHoursAwaitingApprovalID == id).FirstOrDefaultAsync();
            EmployeeHours oldEmployeeHours = await _context.EmployeeHours.Where(eh => eh.EmployeeHoursID == employeeHoursBeingApproved.EmployeeHoursID).FirstOrDefaultAsync();
            if (oldEmployeeHours != null)
            {
                if (oldEmployeeHours.EmployeeHoursStatusEntry1ID != 1)
                {
                    oldEmployeeHours.EmployeeHoursStatusEntry1ID = employeeHoursBeingApproved.EmployeeHoursStatusID;
                }
                oldEmployeeHours.Entry1 = employeeHoursBeingApproved.Entry1;
                oldEmployeeHours.Entry2 = employeeHoursBeingApproved.Entry2;
                oldEmployeeHours.Exit1 = employeeHoursBeingApproved.Exit1;
                oldEmployeeHours.Exit2 = employeeHoursBeingApproved.Exit2;
                oldEmployeeHours.TotalHours = employeeHoursBeingApproved.TotalHours;
                oldEmployeeHours.OffDayTypeID = null;
                employeeHours = oldEmployeeHours;
            }
            else
            {
                employeeHours = new EmployeeHours
                {
                    Entry1 = employeeHoursBeingApproved.Entry1,
                    Entry2 = employeeHoursBeingApproved.Entry2,
                    Exit1 = employeeHoursBeingApproved.Exit1,
                    Exit2 = employeeHoursBeingApproved.Exit2,
                    TotalHours = employeeHoursBeingApproved.TotalHours,
                    EmployeeHoursStatusEntry1ID = employeeHoursBeingApproved.EmployeeHoursStatusID,
                    EmployeeID = employeeHoursBeingApproved.EmployeeID,
                    Date = employeeHoursBeingApproved.Date
                };
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
                TempData["ErrorMessage"] = ex.Message;
                TempData["InnerMessage"] = ex.InnerException;
                return View("~/Views/Shared/RequestError.cshtml");
            }
          
            return RedirectToAction("_AwaitingApproval");
        }
    }

}
