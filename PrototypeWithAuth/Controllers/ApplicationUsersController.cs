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
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, AppUtility.CategoryTypeEnum categoryType =AppUtility.CategoryTypeEnum.Lab)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
            TempData["CategoryType"] = categoryType;
            TempData["SidebarTitle"] = AppUtility.OrdersAndInventorySidebarEnum.Owner;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.OrdersAndInventory;
            return View(await _context.Users.Where(u=>!u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null).ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> Details()
        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Workers;
            TempData["Sidebar"] = AppUtility.UserSideBarEnum.Details;
            var employees = _context.Users.OfType<Employee>().Where(u => !u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null)
                .Include(e=>e.EmployeeStatus).Include(e=>e.SalariedEmployee).Include(e=>e.JobCategoryType);
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
        public async Task<IActionResult> Hours()
        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Workers;
            TempData["Sidebar"] = AppUtility.UserSideBarEnum.Hours;
            return View();
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

            var employeeHoursAwaitingApproval = _context.EmployeeHoursAwaitingApprovals.Include(ehwa=>ehwa.Employee).Include(ehwa=>ehwa.EmployeeHours).Include(ehwa=>ehwa.EmployeeHoursStatus).ToList();
            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = new List<EmployeeHoursAwaitingApprovalViewModel>();
            foreach(var ehaa in employeeHoursAwaitingApproval)
            {
                bool entry1 = false;
                bool entry2 = false;
                bool exit1 = false;
                bool exit2 = false;
                bool totalHours = false;
                if(ehaa.EmployeeHours?.Entry1?.ZeroSeconds().TimeOfDay != ehaa.Entry1?.TimeOfDay)
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
    }

}
