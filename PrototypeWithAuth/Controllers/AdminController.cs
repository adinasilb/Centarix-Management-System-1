using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private SignInManager<ApplicationUser> _signManager;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
      
        [HttpGet]
        public IActionResult RegisterUser()
            
        {
            var roles = _roleManager.Roles; // get the roles from db and have displayed sent to view model
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel
            {
                Roles = roles

            };
            return View(registerUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel registerUserViewModel)
        {

            var usernum = 1;
            if (_context.Users.Any())
            {
                usernum = _context.Users.LastOrDefault().UserNum + 1;
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerUserViewModel.Email,
                    Email = registerUserViewModel.Email,
                    FirstName = registerUserViewModel.FirstName,
                    LastName = registerUserViewModel.LastName,
                    SecureAppPass = registerUserViewModel.SecureAppPass,
                    UserNum = usernum
                };
            
                var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, registerUserViewModel.Role);
                    await _signManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "ApplicationUsers");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registerUserViewModel);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult GetHomeView()
        {
            //Adina's code: should not go to IndexAdmin otherwise if not Admin will say denied Index will take them to IndexAdmin if they're an admin
            return RedirectToAction("Index", "Home");
            //Faige's original code below:
            //return View("~/Views/Home/IndexAdmin.cshtml");
        }
    }
}