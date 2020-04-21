using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PrototypeWithAuth.Controllers
{
    public class LocationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddLocation()
        {
            return PartialView();
        }

        public IActionResult AddLocationType()
        {
            return PartialView();
        }
    }
}