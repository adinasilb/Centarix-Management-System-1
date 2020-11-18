using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Chart.Mvc.Core.SimpleChart;
using System.Drawing;
using PrototypeWithAuth.Models;
using Microsoft.EntityFrameworkCore;

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

            SummaryChartsViewModel summaryChartsViewModel = GetSummaryChartsViewModel();

            return View(summaryChartsViewModel);
        }
        [HttpPost]
        public IActionResult _PieChart(SummaryChartsViewModel summaryChartsViewModel)
        {
            ChartViewModel pieChartViewModel = GetChartData(summaryChartsViewModel);

            return PartialView(pieChartViewModel);
        }
        [HttpPost]
        public IActionResult _GraphChart(SummaryChartsViewModel summaryChartsViewModel)
        {
            ChartViewModel chartViewModel = GetChartData(summaryChartsViewModel);

            return PartialView(chartViewModel);
        }
        private ChartViewModel GetChartData(SummaryChartsViewModel summaryChartsViewModel)
        {
            bool isDollars = false;
            var requests = _context.Requests.Where(r => r.RequestStatusID == 3 && r.PaymentStatusID == 6);
            IEnumerable<Request> requestList = null;
            if (summaryChartsViewModel.SelectedYears == null)
            {
                requests.Where(r => r.CreationDate.Year == DateTime.Now.Year);
            }
            else
            {
                requests.Where(r => summaryChartsViewModel.SelectedYears.Contains(r.CreationDate.Year));
            }
            if (summaryChartsViewModel.SelectedMonths != null)
            {
                requests = requests.Where(r => summaryChartsViewModel.SelectedMonths.Contains(r.CreationDate.Month));
            }
            if (summaryChartsViewModel.Currency != null && summaryChartsViewModel.Currency.Equals(AppUtility.CurrencyEnum.USD.ToString()))
            {
                isDollars = true;
            }
            Random rnd = new Random();
            ChartViewModel pieChartViewModel = new ChartViewModel();
            if (summaryChartsViewModel.SelectedParentCategories != null)
            {
                if (summaryChartsViewModel.SelectedProductSubcategories != null)
                {
                    var subCategories = _context.ProductSubcategories.Where(ps => summaryChartsViewModel.SelectedProductSubcategories.Contains(ps.ProductSubcategoryID));
                    foreach (var ps in subCategories)
                    {
                        requestList = requests.Where(r => r.Product.ProductSubcategoryID == ps.ProductSubcategoryID).Include(r => r.Product).ThenInclude(r => r.ProductSubcategory);
                        double cost = 0;
                        if (isDollars)
                        {
                            cost = requestList.Sum(r => r.Cost / r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate);
                        }
                        else
                        {
                            cost = requestList.Sum(r => r.Cost);
                        }

                        pieChartViewModel.SectionName += "\"" + ps.ProductSubcategoryDescription + "\",";
                        pieChartViewModel.SectionColor += "\"#" + Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)).Name + "\",";
                        pieChartViewModel.SectionValue += "\"" + cost + "\",";
                    }
                }
                else
                {
                    var parentCategories = _context.ParentCategories.Where(pc => summaryChartsViewModel.SelectedParentCategories.Contains(pc.ParentCategoryID));
                    foreach (var pc in parentCategories)
                    {
                        requestList = requests.Where(r => r.Product.ProductSubcategory.ParentCategoryID == pc.ParentCategoryID).Include(r => r.Product).ThenInclude(r => r.ProductSubcategory).ThenInclude(ps => ps.ParentCategory);
                        double cost = 0;
                        if (isDollars)
                        {
                            cost = requestList.Sum(r => r.Cost / r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate);
                        }
                        else
                        {
                            cost = requestList.Sum(r => r.Cost);
                        }

                        pieChartViewModel.SectionName += "\"" + pc.ParentCategoryDescription + "\",";
                        pieChartViewModel.SectionColor += "\"#" + Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)).Name + "\",";
                        pieChartViewModel.SectionValue += "\"" + cost + "\",";
                    }
                }
            }
            else if (summaryChartsViewModel.SelectedProjects != null)
            {
                if (summaryChartsViewModel.SelectedSubProjects != null)
                {
                    var subProjects = _context.SubProjects.Where(sp => summaryChartsViewModel.SelectedSubProjects.Contains(sp.SubProjectID));
                    foreach (var sp in subProjects)
                    {
                        requestList = requests.Where(r => r.SubProjectID == sp.SubProjectID);
                        double cost = 0;
                        if (isDollars)
                        {
                            cost = requestList.Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                        }
                        else
                        {
                            cost = requestList.Sum(r => r.Cost);
                        }

                        pieChartViewModel.SectionName += "\"" + sp.SubProjectDescription + "\",";
                        pieChartViewModel.SectionColor += "\"#" + Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)).Name + "\",";
                        pieChartViewModel.SectionValue += "\"" + cost + "\",";
                    }
                }
                else
                {
                    var projects = _context.Projects.Where(s => summaryChartsViewModel.SelectedProjects.Contains(s.ProjectID));
                    foreach (var s in projects)
                    {
                        requestList = requests.Where(r => r.SubProject.ProjectID == s.ProjectID);
                        double cost = 0;
                        if (isDollars)
                        {
                            cost = requestList.Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                        }
                        else
                        {
                            cost = requestList.Sum(r => r.Cost);
                        }

                        pieChartViewModel.SectionName += "\"" + s.ProjectDescription + "\",";
                        pieChartViewModel.SectionColor += "\"#" + Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)).Name + "\",";
                        pieChartViewModel.SectionValue += "\"" + cost + "\",";
                    }
                }
            }
            else if (summaryChartsViewModel.SelectedEmployees != null)
            {
                var employees = _context.Employees.Where(e => summaryChartsViewModel.SelectedEmployees.Contains(e.Id));

                foreach (var e in employees)
                {
                    requestList = requests.Where(r => r.ApplicationUserCreatorID == e.Id);
                    double cost = 0;
                    if (isDollars)
                    {
                        cost = requestList.Sum(r => r.Cost / r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate);
                    }
                    else
                    {
                        cost = requestList.Sum(r => r.Cost);
                    }

                    pieChartViewModel.SectionName += "\"" + e.FirstName + " " + e.LastName + "\",";
                    pieChartViewModel.SectionColor += "\"#" + Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)).Name + "\",";
                    pieChartViewModel.SectionValue += "\"" + cost + "\",";
                }
            }
            else if (summaryChartsViewModel.SelectedCategoryTypes != null)
            {
                var categories = _context.CategoryTypes.Where(ct => summaryChartsViewModel.SelectedCategoryTypes.Contains(ct.CategoryTypeID));
                foreach (var c in categories)
                {
                    requestList = requests.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == c.CategoryTypeID).Include(r => r.Product).ThenInclude(r => r.ProductSubcategory).ThenInclude(ps => ps.ParentCategory).ThenInclude(pc => pc.CategoryType);
                    double cost = 0;
                    if (isDollars)
                    {
                        cost = requestList.Sum(r => r.Cost / r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate);
                    }
                    else
                    {
                        cost = requestList.Sum(r => r.Cost);
                    }

                    pieChartViewModel.SectionName += "\"" + c.CategoryTypeDescription + "\",";
                    pieChartViewModel.SectionColor += "\"#" + Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)).Name + "\",";
                    pieChartViewModel.SectionValue += "\"" + cost + "\",";
                }
            }

            return pieChartViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult SummaryTables()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.SummaryTables.ToString();

            SummaryTablesViewModel summaryTablesViewModel = new SummaryTablesViewModel()
            {
                CurrentYear = DateTime.Today.Year,
                SummaryTableItems = new List<SummaryTableItem>()
                {
                    new SummaryTableItem(){
                        Month = DateTime.Today,
                        Salary = 1400,
                        Lab = 4000,
                        Operation = 90,
                        Reagents = 115000,
                        Plastics = 90080,
                        Reusable = 2500
                    }
                }

            };

            return View(summaryTablesViewModel);
        }

        [HttpGet]
        public IActionResult _SummaryTables(string currency, int year)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult SummaryGraphs()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.SummaryGraphs.ToString();
            SummaryChartsViewModel summaryChartsViewModel = GetSummaryChartsViewModel();

            return View(summaryChartsViewModel);
        }

        private SummaryChartsViewModel GetSummaryChartsViewModel()
        {
            return new SummaryChartsViewModel()
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
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult WorkersDetails()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesWorkers.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.WorkersDetails.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult WorkersHours()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesWorkers.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.WorkersHours.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult WorkersSalary()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesWorkers.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.WorkersSalary.ToString();
            return View();
        }
    }
}