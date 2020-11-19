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
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.CodeAnalysis;

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
            var count = 0;
            bool isDollars = false;
            var colors = AppUtility.GetChartColors();
            var requests = _context.Requests.Where(r => r.RequestStatusID == 3 && r.PaymentStatusID == 6);
            IEnumerable<Models.Request> requestList = null;
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
            ChartViewModel pieChartViewModel = new ChartViewModel();
            if (summaryChartsViewModel.SelectedParentCategories != null)
            {
                if (summaryChartsViewModel.SelectedProductSubcategories != null)
                {
                    count = 0;
                 
                    var subCategories = _context.ProductSubcategories.Where(ps => summaryChartsViewModel.SelectedProductSubcategories.Contains(ps.ProductSubcategoryID));
                    foreach (var ps in subCategories)
                    {
                        if (count > 18)
                        {
                            count = 0;
                        }
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
                        pieChartViewModel.SectionColor += "\"" + colors[count] + "\",";
                        pieChartViewModel.SectionValue += "\"" + cost + "\",";
                        count++;
                    }
                }
                else
                {
                    count = 0;
                    var parentCategories = _context.ParentCategories.Where(pc => summaryChartsViewModel.SelectedParentCategories.Contains(pc.ParentCategoryID));
                    foreach (var pc in parentCategories)
                    {
                        if (count > 18)
                        {
                            count = 0;
                        }
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
                        pieChartViewModel.SectionColor += "\"" + colors[count] + "\",";
                        pieChartViewModel.SectionValue += "\"" + cost + "\",";
                        count++;
                    }
                }
            }
            else if (summaryChartsViewModel.SelectedProjects != null)
            {
                if (summaryChartsViewModel.SelectedSubProjects != null)
                {
                    count = 0;
                    var subProjects = _context.SubProjects.Where(sp => summaryChartsViewModel.SelectedSubProjects.Contains(sp.SubProjectID));
                    foreach (var sp in subProjects)
                    {
                        if (count > 18)
                        {
                            count = 0;
                        }
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
                        pieChartViewModel.SectionColor += "\"" + colors[count] + "\",";
                        pieChartViewModel.SectionValue += "\"" + cost + "\",";
                        count++;
                    }
                }
                else
                {
                    count = 0;
                    var projects = _context.Projects.Where(s => summaryChartsViewModel.SelectedProjects.Contains(s.ProjectID));
                    foreach (var s in projects)
                    {
                        if (count > 18)
                        {
                            count = 0;
                        }
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
                        pieChartViewModel.SectionColor += "\"" + colors[count] + "\",";
                        pieChartViewModel.SectionValue += "\"" + cost + "\",";
                        count++;
                    }
                }
            }
            else if (summaryChartsViewModel.SelectedEmployees != null)
            {
                var employees = _context.Employees.Where(e => summaryChartsViewModel.SelectedEmployees.Contains(e.Id));
                count = 0;
                foreach (var e in employees)
                {
                    if (count > 18)
                    {
                        count = 0;
                    }
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
                    pieChartViewModel.SectionColor += "\"" + colors[count] + "\",";
                    pieChartViewModel.SectionValue += "\"" + cost + "\",";
                    count++;
                }
            }
            else if (summaryChartsViewModel.SelectedCategoryTypes != null)
            {
                count = 0;
                var categories = _context.CategoryTypes.Where(ct => summaryChartsViewModel.SelectedCategoryTypes.Contains(ct.CategoryTypeID));
                foreach (var c in categories)
                {
                    if (count > 18)
                    {
                        count = 0;
                    }
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
                    pieChartViewModel.SectionColor += "\"" + colors[count] + "\",";
                    pieChartViewModel.SectionValue += "\"" + cost + "\",";
                    count++;
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

            SummaryTablesViewModel summaryTablesViewModel = GetSummaryTablesViewModel(AppUtility.CurrencyEnum.USD, DateTime.Now.Year);
            return View(summaryTablesViewModel);
        }

        [HttpGet]
        public IActionResult _SummaryTables(AppUtility.CurrencyEnum currencyEnum, int year)
        {
            SummaryTablesViewModel summaryTablesViewModel = GetSummaryTablesViewModel(currencyEnum, year);
            return PartialView(summaryTablesViewModel);
        }

        public SummaryTablesViewModel GetSummaryTablesViewModel(AppUtility.CurrencyEnum currencyEnum, int year)
        {
            List<SummaryTableItem> summaryTableItems = new List<SummaryTableItem>();
            for (int i = 1; i <= 12; i++)
            {
                SummaryTableItem sti = new SummaryTableItem()
                {
                    Month = new DateTime(year, i, 1),
                    Salary = string.Format("{0:n0}", Convert.ToInt32("0")),
                };
                var requestsFromMonth = _context.Requests.Where(r => r.CreationDate.Year == year && r.CreationDate.Month == i)
                    .Where(r => r.RequestStatusID == 3) //items that were paid for and received
                    .Include(r => r.Product).ThenInclude(p => p.ProductSubcategory).ThenInclude(ps => ps.ParentCategory);
                double lab = 0;
                double operation = 0;
                double instrument = 0;
                double reagents = 0;
                double plastics = 0;
                double reusables = 0;
                double total = 0;
                switch (currencyEnum)
                {
                    case AppUtility.CurrencyEnum.NIS:
                        lab = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1).Sum(r => r.Cost);
                        operation = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2).Sum(r => r.Cost);
                        instrument = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5).Sum(r => r.Cost);
                        reagents = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 2).Sum(r => r.Cost);
                        plastics = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 1).Sum(r => r.Cost);
                        reusables = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 4).Sum(r => r.Cost);
                        total = requestsFromMonth.Sum(r => r.Cost);
                        break;
                    case AppUtility.CurrencyEnum.USD:
                        lab = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 1).Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                        operation = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategory.CategoryTypeID == 2).Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                        instrument = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 5).Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                        reagents = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 2).Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                        plastics = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 1).Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                        reusables = requestsFromMonth.Where(r => r.Product.ProductSubcategory.ParentCategoryID == 4).Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                        total = requestsFromMonth.Sum(r => r.Cost / r.ExchangeRate);
                        break;
                }
                sti.Lab = string.Format("{0:n0}", Convert.ToInt32(lab));
                sti.Operation = string.Format("{0:n0}", Convert.ToInt32(operation));
                sti.Instrument = string.Format("{0:n0}", Convert.ToInt32(instrument));
                sti.Reagents = string.Format("{0:n0}", Convert.ToInt32(reagents));
                sti.Plastics = string.Format("{0:n0}", Convert.ToInt32(plastics));
                sti.Reusable = string.Format("{0:n0}", Convert.ToInt32(reusables));
                sti.Total = string.Format("{0:n0}", Convert.ToInt32(total));
                summaryTableItems.Add(sti);
            }
            SummaryTablesViewModel summaryTablesViewModel = new SummaryTablesViewModel()
            {
                Currency = currencyEnum,
                CurrentYear = year,
                SummaryTableItems = summaryTableItems
            };
            return summaryTablesViewModel;
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

            var projects = _context.Projects.Include(p => p.SubProjects).ThenInclude(sp => sp.Requests).ThenInclude(r => r.Product).ToList();

            return View(projects);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, CEO, Expenses")]
        public IActionResult _StatisticsSubProjects(int ProjectID)
        {
            _StatisticsProjectViewModel statisticsProjectViewModel = new _StatisticsProjectViewModel()
            {
                SubProjects = _context.SubProjects.Where(sp => sp.ProjectID == ProjectID)
                .Include(sp => sp.Requests).ThenInclude(r => r.Product).ToList(),
                ProjectName = _context.Projects.Where(p => p.ProjectID == ProjectID).FirstOrDefault().ProjectDescription
            };

            return PartialView(statisticsProjectViewModel);
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