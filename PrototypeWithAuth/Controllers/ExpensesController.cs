using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PrototypeWithAuth.Controllers
{
    public class ExpensesController : Controller
    {
        public IActionResult SummaryPieCharts()
        {
            return View();
        }
    }
}