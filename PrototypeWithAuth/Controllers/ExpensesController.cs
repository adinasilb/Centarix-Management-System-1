﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Project = PrototypeWithAuth.Models.Project;
using Request = PrototypeWithAuth.Models.Request;
using System.Threading.Tasks;


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
        [Authorize(Roles = "Expenses")]
        public IActionResult SummaryPieCharts()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.SummaryPieCharts.ToString();

            SummaryChartsViewModel summaryChartsViewModel = GetSummaryChartsViewModel();

            return View(summaryChartsViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Expenses")]
        public IActionResult _PieChart(SummaryChartsViewModel summaryChartsViewModel)
        {
            ChartViewModel pieChartViewModel = GetChartData(summaryChartsViewModel);

            return PartialView(pieChartViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Expenses")]
        public IActionResult _GraphChart(SummaryChartsViewModel summaryChartsViewModel)
        {
            ChartViewModel chartViewModel = GetChartData(summaryChartsViewModel);

            return PartialView(chartViewModel);
        }
        [Authorize(Roles = "Expenses")]
        private ChartViewModel GetChartData(SummaryChartsViewModel summaryChartsViewModel)
        {
            var count = 0;
            bool isDollars = false;
            var colors = AppUtility.GetChartColors();
            var requests = _context.Requests.Where(r => r.RequestStatusID == 3 && r.PaymentStatusID == 6);
            string currency = "₪";
            var greyColor = AppUtility.GetChartUnderZeroColor();
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
                currency = "$";
            }
            ChartViewModel pieChartViewModel = new ChartViewModel();
            pieChartViewModel.SectionColor = new List<String>();
            pieChartViewModel.SectionName = new List<String>();
            pieChartViewModel.SectionValue = new List<double>();
            pieChartViewModel.Currency = currency;
            if (summaryChartsViewModel.SelectedEmployees != null)
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
                        cost = requestList.Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                    }
                    else
                    {
                        cost = requestList.Sum(r => r.Cost);
                    }

                    pieChartViewModel.SectionName.Add(e.FirstName + " " + e.LastName);
                    if (cost > 0)
                    {
                        pieChartViewModel.SectionColor.Add(colors[count]);
                        count++;
                    }
                    else
                    {
                        pieChartViewModel.SectionColor.Add(greyColor);
                    }
                    pieChartViewModel.SectionValue.Add(cost);
                }
            }
            else if (summaryChartsViewModel.SelectedVendors != null)
            {
                var vendors = _context.Vendors.Where(e => summaryChartsViewModel.SelectedVendors.Contains(e.VendorID));
                count = 0;
                foreach (var v in vendors)
                {
                    if (count > 18)
                    {
                        count = 0;
                    }
                    requestList = requests.Where(r => r.Product.Vendor.VendorID == v.VendorID);
                    double cost = 0;
                    if (isDollars)
                    {
                        cost = requestList.Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                    }
                    else
                    {
                        cost = requestList.Sum(r => r.Cost);
                    }

                    pieChartViewModel.SectionName.Add(v.VendorEnName);
                    if (cost > 0)
                    {
                        pieChartViewModel.SectionColor.Add(colors[count]);
                        count++;
                    }
                    else
                    {
                        pieChartViewModel.SectionColor.Add(greyColor);
                    }
                    pieChartViewModel.SectionValue.Add(cost);
                }
            }
            else if (summaryChartsViewModel.SelectedProductSubcategories != null)
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
                        cost = requestList.Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                    }
                    else
                    {
                        cost = requestList.Sum(r => r.Cost);
                    }

                    pieChartViewModel.SectionName.Add(ps.ProductSubcategoryDescription);
                    if (cost > 0)
                    {
                        pieChartViewModel.SectionColor.Add(colors[count]);
                        count++;
                    }
                    else
                    {
                        pieChartViewModel.SectionColor.Add(greyColor);
                    }
                    pieChartViewModel.SectionValue.Add(cost);
                }
            }
            else if (summaryChartsViewModel.SelectedParentCategories != null)
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
                        cost = requestList.Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                    }
                    else
                    {
                        cost = requestList.Sum(r => r.Cost);
                    }

                    pieChartViewModel.SectionName.Add(pc.ParentCategoryDescription);
                    if (cost > 0)
                    {
                        pieChartViewModel.SectionColor.Add(colors[count]);
                        count++;
                    }
                    else
                    {
                        pieChartViewModel.SectionColor.Add(greyColor);
                    }
                    pieChartViewModel.SectionValue.Add(cost);
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
                        cost = requestList.Sum(r => r.Cost / (r.ExchangeRate == 0 ? AppUtility.ExchangeRateIfNull : r.ExchangeRate));
                    }
                    else
                    {
                        cost = requestList.Sum(r => r.Cost);
                    }

                    pieChartViewModel.SectionName.Add(c.CategoryTypeDescription);
                    if (cost > 0)
                    {
                        pieChartViewModel.SectionColor.Add(colors[count]);
                        count++;
                    }
                    else
                    {
                        pieChartViewModel.SectionColor.Add(greyColor);
                    }
                    pieChartViewModel.SectionValue.Add(cost);
                }
            }
            else if (summaryChartsViewModel.SelectedSubProjects != null)
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

                    pieChartViewModel.SectionName.Add(sp.SubProjectDescription);
                    if (cost > 0)
                    {
                        pieChartViewModel.SectionColor.Add(colors[count]);
                        count++;
                    }
                    else
                    {
                        pieChartViewModel.SectionColor.Add(greyColor);
                    }
                    pieChartViewModel.SectionValue.Add(cost);
                }
            }
            else if (summaryChartsViewModel.SelectedProjects != null)
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

                    pieChartViewModel.SectionName.Add(s.ProjectDescription);
                    if (cost > 0)
                    {
                        pieChartViewModel.SectionColor.Add(colors[count]);
                        count++;
                    }
                    else
                    {
                        pieChartViewModel.SectionColor.Add(greyColor);
                    }
                    pieChartViewModel.SectionValue.Add(cost);


                }
            }

            return pieChartViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult SummaryTables()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.SummaryTables.ToString();

            SummaryTablesViewModel summaryTablesViewModel = GetSummaryTablesViewModel(AppUtility.CurrencyEnum.USD, DateTime.Now.Year);
            return View(summaryTablesViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult _SummaryTables(AppUtility.CurrencyEnum currencyEnum, int year)
        {
            SummaryTablesViewModel summaryTablesViewModel = GetSummaryTablesViewModel(currencyEnum, year);
            return PartialView(summaryTablesViewModel);
        }

        [Authorize(Roles = "Expenses")]
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
        [Authorize(Roles = "Expenses")]
        public IActionResult SummaryGraphs()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.SummaryGraphs.ToString();
            SummaryChartsViewModel summaryChartsViewModel = GetSummaryChartsViewModel();

            return View(summaryChartsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult _DDLForCharts()
        {
            SummaryChartsViewModel summaryChartsViewModel = GetSummaryChartsViewModel();

            return PartialView(summaryChartsViewModel);
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
                              ).ToList(),
                Vendors = _context.Vendors.ToList()
            };
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult StatisticsProject()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsProject.ToString();


            var AllProjects = _context.Projects.Include(p => p.SubProjects).ThenInclude(sp => sp.Requests).ThenInclude(r => r.Product)
                .Include(p => p.SubProjects).ThenInclude(sp => sp.Requests).ThenInclude(r => r.Invoice)
                .ToList();

            return View(GetStatisticsProjectViewModel(AllProjects, new List<int>() { DateTime.Now.Month }, new List<int>() { DateTime.Now.Year }));
        }

        [Authorize(Roles = "Expenses")]
        public IActionResult _StatisticsProjects(List<int> Months, List<int> Years)
        {
            var AllProjects = _context.Projects.Include(p => p.SubProjects).ThenInclude(sp => sp.Requests).ThenInclude(r => r.Product)
                .Include(p => p.SubProjects).ThenInclude(sp => sp.Requests).ThenInclude(r => r.Invoice)
                .ToList();

            return PartialView(GetStatisticsProjectViewModel(AllProjects, Months, Years));
        }

        [Authorize(Roles = "Expenses")]
        public static StatisticsProjectViewModel GetStatisticsProjectViewModel(List<Project> AllProjects, List<int> Months, List<int> Years)
        {

            //var MatchingRequests = _context.Requests.Where(r => r.Invoice.InvoiceDate.Month == 9).ToList();

            //List<ProjectStatistics> projectStatistics = new List<ProjectStatistics>();
            Dictionary<Project, List<Request>> Projects = new Dictionary<Project, List<Request>>();
            //This is to ensure that all projects  are passed into the front end even if there aren't any orders for it
            foreach (var project in AllProjects)
            {
                var MonthlyRequestsInProject = project.SubProjects.SelectMany(
                    sp => sp.Requests
                    .Where(r => r.RequestStatusID == 3 && r.PaymentStatusID == 6)
                    .Where(r => Months.Contains(Convert.ToInt32(r.Invoice?.InvoiceDate.Month)))
                    .Where(r => Years.Contains(Convert.ToInt32(r.Invoice?.InvoiceDate.Year)))
                    ).ToList();
                Projects.Add(project, MonthlyRequestsInProject);
            }

            StatisticsProjectViewModel statisticsProjectViewModel = new StatisticsProjectViewModel()
            {
                Projects = Projects,
                Months = Months,
                Years = Years
            };

            return statisticsProjectViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult _StatisticsSubProjects(int ProjectID, List<int> Months, List<int> Years)
        {
            Dictionary<SubProject, List<Request>> subProjectRequests = new Dictionary<SubProject, List<Request>>();

            var subprojects = _context.SubProjects.Where(sp => sp.ProjectID == ProjectID).ToList();
            foreach (var subproject in subprojects)
            {
                var requests = _context.Requests
                    .Include(r => r.Invoice)
                    .Where(r => r.RequestStatusID == 3 && r.PaymentStatusID == 6)
                    .Where(r => r.SubProjectID == subproject.SubProjectID)
                    .Where(r => r.InvoiceID != null)
                    .Where(r => Months.Contains(r.Invoice.InvoiceDate.Month))
                    .Where(r => Years.Contains(r.Invoice.InvoiceDate.Year)).ToList();
                subProjectRequests.Add(subproject, requests);
            }

            _StatisticsSubProjectViewModel statisticsSubProjectViewModel = new _StatisticsSubProjectViewModel()
            {
                SubProjects = subProjectRequests,
                ProjectName = _context.Projects.Where(p => p.ProjectID == ProjectID).FirstOrDefault().ProjectDescription
            };

            return PartialView(statisticsSubProjectViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult StatisticsItem()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsItem.ToString();

            var categoryTypesSelected = _context.CategoryTypes.Select(ct => ct.CategoryTypeID).ToList();

            return View(GetStatisticsItemViewModel(true, false, categoryTypesSelected, new List<int>() { DateTime.Now.Month }, new List<int>() { DateTime.Now.Year }));
        }

        [Authorize(Roles = "Expenses")]
        public StatisticsItemViewModel GetStatisticsItemViewModel(bool FrequentlyBought, bool HighestPrice, List<int> CategoryTypesSelected, List<int> Months, List<int> Year)
        {
            var categoryTypes = _context.CategoryTypes.ToList();

            StatisticsItemViewModel statisticsItemViewModel = new StatisticsItemViewModel()
            {
                Requests = _context.Requests.ToList(),
                FrequentlyBought = FrequentlyBought,
                HighestPrice = HighestPrice,
                CategoryTypesSelected = _context.CategoryTypes.Where(ct => CategoryTypesSelected.Contains(ct.CategoryTypeID)).ToList(),
                CategoryTypes = categoryTypes,
                Months = Months,
                Year = Year
            };

            return statisticsItemViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public async Task<IActionResult> StatisticsWorker()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsWorker.ToString();

            var employees = await _context.Employees.ToListAsync();
            var categoryTypes = await _context.CategoryTypes.ToListAsync();
            var months = new List<int> { DateTime.Today.Month };
            var years = new List<int> { DateTime.Today.Year };

            return View(GetStatisticsWorkerViewModel(employees, categoryTypes, months, years));
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public async Task<IActionResult> _StatisticsWorkerChart(List<int> CategoryTypeIDs, List<int> Months, List<int> Years)
        {
            var employees = await _context.Employees.ToListAsync();
            var categoryTypes = await _context.CategoryTypes.Where(ct => CategoryTypeIDs.Contains(ct.CategoryTypeID)).ToListAsync();

            return PartialView(GetStatisticsWorkerViewModel(employees, categoryTypes, Months, Years));
        }


        [Authorize(Roles = "Expenses")]
        public StatisticsWorkerViewModel GetStatisticsWorkerViewModel(List<Employee> Employees, List<CategoryType> CategoryTypes, List<int> Months, List<int> Years)
        {
            Dictionary<Employee, List<Request>> EmployeeRequests = new Dictionary<Employee, List<Request>>();

            foreach (var employee in Employees)
            {
                var requests = _context.Requests.Where(r => r.RequestStatusID == 3 && r.PaymentStatusID == 6)
                    .Where(r => r.ApplicationUserCreator.Id == employee.Id)
                    .Where(r => CategoryTypes.Contains(r.Product.ProductSubcategory.ParentCategory.CategoryType))
                    .Where(r => r.Invoice != null)
                    .Where(r => Months.Contains(r.Invoice.InvoiceDate.Month)).Where(r => Years.Contains(r.Invoice.InvoiceDate.Year))
                    .ToList();
                EmployeeRequests.Add(employee, requests);
            }

            StatisticsWorkerViewModel statisticsWorkerViewModel = new StatisticsWorkerViewModel()
            {
                Employees = EmployeeRequests,
                CategoryTypesSelected = CategoryTypes,
                CategoryTypes = _context.CategoryTypes.ToList(),
                Months = Months,
                Years = Years
            };

            return statisticsWorkerViewModel;
        }


        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult StatisticsCategory()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsCategory.ToString();

            var parentCategories = _context.ParentCategories.ToList();
            var catTypes = _context.CategoryTypes.Select(ct => ct.CategoryTypeID).ToList();

            return View(GetStatisticsCategoryViewModel(parentCategories, catTypes, new List<int>() { DateTime.Now.Month }, new List<int>() { DateTime.Today.Year }));
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult _CategoryTypes(List<int> categoryTypes, List<int> months, List<int> years)
        {
            var parentCategories = _context.ParentCategories.ToList();

            return PartialView(GetStatisticsCategoryViewModel(parentCategories, categoryTypes, months, years));
        }

        [Authorize(Roles = "Expenses")]
        public StatisticsCategoryViewModel GetStatisticsCategoryViewModel(List<ParentCategory> parentCategories, List<int> categoryTypes, List<int> months, List<int> years)
        {
            Dictionary<ParentCategory, List<Request>> ParentCategories = new Dictionary<ParentCategory, List<Request>>();
            foreach (var pc in parentCategories)
            {
                var pcRequests = _context.Requests.Where(r => r.Product.ProductSubcategory.ParentCategoryID == pc.ParentCategoryID)
                    .Where(r => r.RequestStatusID == 3 && r.PaymentStatusID == 6)
                    .Where(r => categoryTypes.Contains(r.Product.ProductSubcategory.ParentCategory.CategoryTypeID))
                    .Where(r => r.Invoice != null)
                    .Where(r => months.Contains(r.Invoice.InvoiceDate.Month)).Where(r => years.Contains(r.Invoice.InvoiceDate.Year))
                    .ToList();
                ParentCategories.Add(pc, pcRequests);
            }
            StatisticsCategoryViewModel statisticsCategoryViewModel = new StatisticsCategoryViewModel()
            {
                ParentCategories = ParentCategories,
                Months = months,
                CategoryTypes = _context.CategoryTypes.ToList(),
                CategoryTypeSelected = _context.CategoryTypes.Where(ct => categoryTypes.Contains(ct.CategoryTypeID)).ToList(),
                Years = years
            };
            return statisticsCategoryViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult _SubCategoryTypes(int ParentCategoryId, List<int> categoryTypes, List<int> months, List<int> years)
        {
            var subCategories = _context.ProductSubcategories.Where(sc => sc.ParentCategoryID == ParentCategoryId).ToList();
            Dictionary<ProductSubcategory, List<Request>> productSubs = new Dictionary<ProductSubcategory, List<Request>>();
            foreach (var sc in subCategories)
            {
                var scRequests = _context.Requests.Where(r => r.Product.ProductSubcategoryID == sc.ProductSubcategoryID)
                    .Where(r => r.RequestStatusID == 3 && r.PaymentStatusID == 6)
                    .Where(r => categoryTypes.Contains(r.Product.ProductSubcategory.ParentCategory.CategoryTypeID))
                    .Where(r => r.Invoice != null)
                    .Where(r => months.Contains(r.Invoice.InvoiceDate.Month)).Where(r => years.Contains(r.Invoice.InvoiceDate.Year))
                    .ToList();
                productSubs.Add(sc, scRequests);
            }
            StatisticsSubCategoryViewModel statisticsSubCategoryViewModel = new StatisticsSubCategoryViewModel()
            {
                ProductSubcategories = productSubs,
                ParentCategoryName = _context.ParentCategories.Where(pc => pc.ParentCategoryID == ParentCategoryId).FirstOrDefault().ParentCategoryDescription
            };

            return PartialView(statisticsSubCategoryViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult StatisticsVendor()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesStatistics.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.StatisticsVendor.ToString();

            var catTypes = _context.CategoryTypes.ToList();

            return View(GetStatisticsVendorViewModel(catTypes, new List<int> { DateTime.Now.Month }, new List<int> { DateTime.Now.Year }));
        }

        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult _VendorsTable(List<int> CategoryTypes, List<int> Months, List<int> Years)
        {
            var catTypes = _context.CategoryTypes.Where(ct => CategoryTypes.Contains(ct.CategoryTypeID)).ToList();
            return PartialView(GetStatisticsVendorViewModel(catTypes, Months, Years));
        }

        [Authorize(Roles = "Expenses")]
        public StatisticsVendorViewModel GetStatisticsVendorViewModel(List<CategoryType> CategoryTypes, List<int> Months, List<int> Years)
        {
            Dictionary<Vendor, List<Request>> VendorRequests = new Dictionary<Vendor, List<Request>>();

            foreach (var vendor in _context.Vendors.ToList())
            {
                var requests = _context.Requests
                    .Include(r => r.Product).ThenInclude(p => p.Vendor)
                .Where(r => r.RequestStatusID == 3 && r.PaymentStatusID == 6)
                .Where(r => r.Product.Vendor.VendorID == vendor.VendorID)
                .Where(r => CategoryTypes.Contains(r.Product.ProductSubcategory.ParentCategory.CategoryType))
                .Where(r => r.Invoice != null)
                .Where(r => Months.Contains(r.Invoice.InvoiceDate.Month)).Where(r => Years.Contains(r.Invoice.InvoiceDate.Year))
                   .ToList();
                VendorRequests.Add(vendor, requests);
            }

            StatisticsVendorViewModel svvm = new StatisticsVendorViewModel()
            {
                Vendors = VendorRequests,
                CategoryTypesSelected = CategoryTypes,
                CategoryTypes = _context.CategoryTypes.ToList(),
                Months = Months,
                Years = Years
            };
            return svvm;
        }


        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public async Task<IActionResult> CostsProject()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesCost.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.CostsProject.ToString();
            CostsProjectViewModel costsProjectViewModel = new CostsProjectViewModel();
            costsProjectViewModel.VendorList =   _context.Vendors;
            costsProjectViewModel.ParentCategoryList = _context.ParentCategories;
            costsProjectViewModel.SubCategoryList = _context.ProductSubcategories;
            return View(costsProjectViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult CostsAdvancedSearch()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesCost.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.CostsAdvancedSearch.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult CostsAdvancedList()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesCost.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.CostsAdvancedLists.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult WorkersDetails()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesWorkers.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.WorkersDetails.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult WorkersHours()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesWorkers.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.WorkersHours.ToString();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Expenses")]
        public IActionResult WorkersSalary()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Reports.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesWorkers.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.WorkersSalary.ToString();
            return View();
        }
    }
}