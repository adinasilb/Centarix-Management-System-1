using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Authorization;
using PrototypeWithAuth.Models;
using Microsoft.AspNetCore.Identity;
using Abp.Threading.Extensions;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrototypeWithAuth.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: /<controller>/
        [HttpGet]

        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> Index(AppUtility.RequestPageTypeEnum PageType = AppUtility.RequestPageTypeEnum.Request, AppUtility.CategoryTypeEnum categoryType =AppUtility.CategoryTypeEnum.Lab)
        {
            TempData["PageType"] = PageType;
            TempData["CategoryType"] = categoryType;
            TempData["SidebarTitle"] = AppUtility.RequestSidebarEnum.Owner;
            return View(await _context.Users.Where(u=>!u.LockoutEnabled && (u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null)).ToListAsync());
        }
    }
}
