using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.Controllers
{
    public class ExpensesController : Controller
    {
        public IActionResult SummaryPieCharts()
        {
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Expenses.ToString();
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.ExpensesPageTypeEnum.ExpensesSummary.ToString();
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.ExpensesSidebarEnum.SummaryPieCharts.ToString();
            return View();
        }
    }
}