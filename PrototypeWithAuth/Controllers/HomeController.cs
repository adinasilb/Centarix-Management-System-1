using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using SQLitePCL;

namespace PrototypeWithAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Adina added in 3 lines
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("IndexAdmin");
            }
            return View();
        }

        //Adina added in
        [Authorize (Roles ="Admin")]
        public IActionResult IndexAdmin()
        {
            IEnumerable<Menu> menu = _context.Menus.Select(x => x);
            return View(menu);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
