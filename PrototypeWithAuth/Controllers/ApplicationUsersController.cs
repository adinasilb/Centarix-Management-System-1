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

        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, AppUtility.CategoryTypeEnum categoryType = AppUtility.CategoryTypeEnum.Lab)
        {
            TempData["CategoryType"] = categoryType;
            if (categoryType == AppUtility.CategoryTypeEnum.Lab)
            {
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
                TempData["SidebarTitle"] = AppUtility.OrdersAndInventorySidebarEnum.Owner;
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.OrdersAndInventory;
            }
            if (categoryType == AppUtility.CategoryTypeEnum.Operations)
            {
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operation;
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
            return View(await _context.Users.Where(u=>!u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null).ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> Details()
        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Workers;
            TempData["Sidebar"] = AppUtility.UserSideBarEnum.Details;
            var employees = _context.Users.OfType<Employee>().Where(u => !u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null)
                .Include(e => e.EmployeeStatus).Include(e => e.SalariedEmployee).Include(e => e.JobCategoryType);
            EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel
            {
                Employees = employees.ToList(),
                SalariedEmployeeCount = employees.Where(e => e.EmployeeStatusID == 1).Count(),
                FreelancerCount = employees.Where(e => e.EmployeeStatusID == 2).Count(),
                AdvisorCount = employees.Where(e => e.EmployeeStatusID == 3).Count(),
            };

            return View(employeeDetailsViewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> Hours(YearlyMonthlyEnum yearlyMonthlyEnum = YearlyMonthlyEnum.Monthly, int month = 0, int year = 0)
        {
            if(year == 0)
            {
                year = DateTime.Now.Year;
            }
            if(month == 0)
            {
                month = DateTime.Now.Month;
            }
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Workers;
            TempData["Sidebar"] = AppUtility.UserSideBarEnum.Hours;
            var employees = _context.Users.OfType<Employee>().Where(u => !u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null)
              .Include(e => e.EmployeeStatus).Include(e => e.JobCategoryType).Include(e=>e.EmployeeHours);
            List<WorkerHourViewModel> workerHoursViewModel = new List<WorkerHourViewModel>();
            foreach(var employee in employees)
            {
                int vacationDays = 0;
                int workDays = 0;
                int sickDays = 0;
                TimeSpan hours = new TimeSpan();
                switch (yearlyMonthlyEnum)
                {
                    case YearlyMonthlyEnum.Monthly:
                        sickDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 1 && eh.Date.Year == year && eh.Date.Month == month).Count();
                        vacationDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 2 && eh.Date.Year == year && eh.Date.Month == month).Count();
                        workDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == null && eh.Date.Year == year && eh.Date.Month == month).Count();
                        hours = new TimeSpan(employee.EmployeeHours.Where(eh =>  eh.Date.Year == year && eh.Date.Month == month).Select(eh=>new { TimeSpan = eh.TotalHours?.Ticks ?? 0 }).Sum(a => a.TimeSpan));
                        break;
                    case YearlyMonthlyEnum.Yearly:
                        sickDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 1 && eh.Date.Year == year).Count();
                        vacationDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 2 && eh.Date.Year == year ).Count();
                        workDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == null && eh.Date.Year == year).Count();
                        hours = new TimeSpan(employee.EmployeeHours.Where(eh => eh.Date.Year == year).Select(eh => new { TimeSpan = eh.TotalHours?.Ticks ?? 0 }).Sum(a => a.TimeSpan));
                        break;
                }
                var worker = new WorkerHourViewModel
                {
                    Employee = employee,
                    SickDays =sickDays, 
                    VacationDays =vacationDays,
                    WorkingDays = workDays,
                    Hours = hours
                };
                workerHoursViewModel.Add(worker);
            }
            var viewModel = new WorkersHoursViewModel
            {
                Year = year,
                Month= month,
                YearlyMonthlyEnum = yearlyMonthlyEnum,
                Employees =workerHoursViewModel
            };
            return View(viewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> Salary()
        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Workers;
            TempData["Sidebar"] = AppUtility.UserSideBarEnum.Salary;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> AwaitingApproval()
        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Workers;
            TempData["Sidebar"] = AppUtility.UserSideBarEnum.AwaitingApproval;

            var employeeHoursAwaitingApproval = _context.EmployeeHoursAwaitingApprovals.Include(ehwa => ehwa.Employee).Include(ehwa => ehwa.EmployeeHours).Include(ehwa => ehwa.EmployeeHoursStatus).ToList();
            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = new List<EmployeeHoursAwaitingApprovalViewModel>();
            foreach (var ehaa in employeeHoursAwaitingApproval)
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
            return View(awaitingApproval);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> AddWorker()
        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Workers;
            TempData["SideBar"] = AppUtility.UserSideBarEnum.AddWorker;
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> ApproveHours(int id)
        {
            EmployeeHours employeeHours = new EmployeeHours();
            var employeeHoursBeingApproved = await _context.EmployeeHoursAwaitingApprovals.Where(ehaa => ehaa.EmployeeHoursAwaitingApprovalID == id).FirstOrDefaultAsync();
            var oldEmployeeHours = await _context.EmployeeHours.Where(eh => eh.EmployeeHoursID == employeeHoursBeingApproved.EmployeeHoursID).FirstOrDefaultAsync();
            if (oldEmployeeHours != null)
            {
                if (oldEmployeeHours.EmployeeHoursStatusID != 1)
                {
                    oldEmployeeHours.EmployeeHoursStatusID = employeeHoursBeingApproved.EmployeeHoursStatusID;
                }
                oldEmployeeHours.Entry1 = employeeHoursBeingApproved.Entry1;
                oldEmployeeHours.Entry2 = employeeHoursBeingApproved.Entry2;
                oldEmployeeHours.Exit1 = employeeHoursBeingApproved.Exit1;
                oldEmployeeHours.Exit2 = employeeHoursBeingApproved.Exit2;
                oldEmployeeHours.TotalHours = employeeHoursBeingApproved.TotalHours;
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
                    EmployeeHoursStatusID = employeeHoursBeingApproved.EmployeeHoursStatusID,
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
          
            return Redirect("AwaitingApproval");
        }
    }

}
