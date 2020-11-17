using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrototypeWithAuth.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExpensesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult SummaryPieCharts()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.SummaryPieCharts.ToString();

            SummaryPieChartsViewModel summaryPieChartsViewModel = new SummaryPieChartsViewModel()
            {
                Years = _context.Requests.Select(r => r.CreationDate.Year).Distinct().ToList(),
                CategoryTypes = _context.CategoryTypes.ToList(),
                ParentCategories = _context.ParentCategories.ToList(),
                ProductSubcategories = _context.ProductSubcategories.ToList(),
                Projects = _context.Projects.ToList(),
                SubProjects = _context.SubProjects.ToList(),
                Employees = _context.Employees
                    .Select(
                        e => new SelectListItem
                        {
                            Text = e.FirstName + " " + e.LastName,
                            Value = e.Id
                        }
                    ).ToList()
            };

            return View(summaryPieChartsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult SummaryTables()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.SummaryTables.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult SummaryGraphs()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.SummaryGraphs.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult StatisticsProject()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsProject.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult StatisticsItem()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsItem.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult StatisticsWorker()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsWorker.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult StatisticsCategory()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsCategory.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult StatisticsVendor()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsVendor.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult CostsProject()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesCost.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.CostsProject.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult CostsAdvancedSearch()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesCost.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.CostsAdvancedSearch.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult CostsAdvancedList()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesCost.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.CostsAdvancedLists.ToString();
            return View();
        }
    }
}