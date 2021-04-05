using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Controllers
{
    public class ProtocolsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
