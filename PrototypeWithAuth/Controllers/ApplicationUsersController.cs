﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrototypeWithAuth.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request)
        {
            TempData["PageType"] = PageType;
            TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.Owner;
            return View(await _context.Users.ToListAsync());
        }
    }
}
