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
        [HttpPost]
        public IActionResult _PieChart(SummaryPieChartsViewModel summaryPieChartsViewModel)
        {
            bool isDollars = false;
            var requests = _context.Requests.Where(r=>r.RequestStatusID==3 && r.PaymentStatusID==6);
            IEnumerable<Request> requestList = null;
            if (summaryPieChartsViewModel.SelectedYears == null)
            {
                requests.Where(r => r.CreationDate.Year == DateTime.Now.Year);
            }
            else
            {
                requests.Where(r => summaryPieChartsViewModel.SelectedYears.Contains( r.CreationDate.Year));
            }
            if (summaryPieChartsViewModel.SelectedMonths != null)
            {
                requests = requests.Where(r => summaryPieChartsViewModel.SelectedMonths.Contains(r.CreationDate.Month));                
            }
            if (summaryPieChartsViewModel.Currency!=null && summaryPieChartsViewModel.Currency.Equals(AppUtility.CurrencyEnum.USD.ToString()))
            {
                isDollars = true;
            }
            Random rnd = new Random();
            PieChartViewModel pieChartViewModel = new PieChartViewModel();
            if (summaryPieChartsViewModel.SelectedParentCategories != null)
            {
                if (summaryPieChartsViewModel.SelectedProductSubcategories != null)
                {
                    var subCategories = _context.ProductSubcategories.Where(ps => summaryPieChartsViewModel.SelectedProductSubcategories.Contains(ps.ProductSubcategoryID));
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
                    var parentCategories = _context.ParentCategories.Where(pc => summaryPieChartsViewModel.SelectedParentCategories.Contains(pc.ParentCategoryID));
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
            else if (summaryPieChartsViewModel.SelectedProjects != null)
            {
                if (summaryPieChartsViewModel.SelectedSubProjects != null)
                {
                    var subProjects = _context.SubProjects.Where(sp => summaryPieChartsViewModel.SelectedSubProjects.Contains(sp.SubProjectID));
                    foreach (var sp in subProjects )
                    {
                        requestList = requests.Where(r => r.SubProjectID== sp.SubProjectID);
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
                    var projects = _context.Projects.Where(s => summaryPieChartsViewModel.SelectedProjects.Contains(s.ProjectID));
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
            else if (summaryPieChartsViewModel.SelectedEmployees != null)
            {
                var employees = _context.Employees.Where(e => summaryPieChartsViewModel.SelectedEmployees.Contains(e.Id));
              
                foreach(var e in employees)
                {
                    requestList = requests.Where(r => r.ApplicationUserCreatorID == e.Id);
                    double cost = 0;
                    if (isDollars)
                    {
                        cost = requestList.Sum(r => r.Cost/ r.ExchangeRate==0?AppUtility.ExchangeRateIfNull:r.ExchangeRate);
                    }
                    else
                    {
                        cost = requestList.Sum(r => r.Cost);
                    }
                   
                    pieChartViewModel.SectionName += "\"" + e.FirstName + " " + e.LastName + "\",";
                    pieChartViewModel.SectionColor += "\"#" + Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)).Name + "\",";
                    pieChartViewModel.SectionValue += "\""  +cost+ "\",";
                }
            }
            else if(summaryPieChartsViewModel.SelectedCategoryTypes!=null)
            {
                var categories = _context.CategoryTypes.Where(ct => summaryPieChartsViewModel.SelectedCategoryTypes.Contains(ct.CategoryTypeID));
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
   
            return PartialView(pieChartViewModel);
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