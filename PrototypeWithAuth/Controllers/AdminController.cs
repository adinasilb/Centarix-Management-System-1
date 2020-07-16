using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult Index()
        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Index;
            var users = _context.Users
                .ToList();
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult RegisterUser()

        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Add;
            var roles = _roleManager.Roles; // get the roles from db and have displayed sent to view model
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel
            {
                //Roles = roles

            };
            return View(registerUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Users")]
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
                    //FirstName = registerUserViewModel.FirstName,
                    //LastName = registerUserViewModel.LastName,
                    SecureAppPass = registerUserViewModel.SecureAppPass,
                    UserNum = usernum
                };

                var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);
                if (result.Succeeded)
                {
                    //await _userManager.AddToRoleAsync(user, registerUserViewModel.Role);
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

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult CreateUser()
        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Add;
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();
            registerUserViewModel.OrderList = new Dictionary<string, bool>(); ;
            registerUserViewModel.OrderList.Add("General", false);
            return View(registerUserViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel registerUserViewModel)
        {
            var usernum = 1;
            if (_context.Users.Any())
            {
                usernum = _context.Users.OrderByDescending(u => u.UserNum).FirstOrDefault().UserNum + 1;
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerUserViewModel.UserName,
                    Email = registerUserViewModel.Email,
                    //FirstName = registerUserViewModel.FirstName,
                    //LastName = registerUserViewModel.LastName,
                    SecureAppPass = registerUserViewModel.SecureAppPass,
                    UserNum = usernum,
                    LabMonthlyLimit = registerUserViewModel.LabMonthlyLimit,
                    LabUnitLimit = registerUserViewModel.LabUnitLimit,
                    LabOrderLimit = registerUserViewModel.LabOrderLimit,
                    OperationMonthlyLimit = registerUserViewModel.OperationMonthlyLimit,
                    OperationUnitLimit = registerUserViewModel.OperationUnitLimit,
                    OperaitonOrderLimit = registerUserViewModel.OperaitonOrderLimit
                };

                var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);
                //var role = _context.Roles.Where(r => r.Name == "Admin").FirstOrDefault().Id;
                if (result.Succeeded)
                {
                    //add user roles
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> EditUser(string id)
        {
            var registerUserViewModel = _context.Users.Where(u => u.Id == id)
                .Select(u => new RegisterUserViewModel
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    //do we want to show the secure app pass??
                    LabMonthlyLimit = u.LabMonthlyLimit,
                    LabUnitLimit = u.LabUnitLimit,
                    LabOrderLimit = u.LabOrderLimit,
                    OperationMonthlyLimit = u.OperationMonthlyLimit,
                    OperationUnitLimit = u.OperationUnitLimit,
                    OperaitonOrderLimit = u.OperaitonOrderLimit
                }).FirstOrDefault();

            var userToShow = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            var rolesList = await _userManager.GetRolesAsync(userToShow).ConfigureAwait(false);
            foreach (var role in rolesList)
            {
                if (role == AppUtility.MenuItems.OrdersAndInventory.ToString()) //this was giving me an error in a switch case
                {

                }
                else if (role == AppUtility.MenuItems.Protocols.ToString()) //this was giving me an error in a switch case
                {

                }
                else if (role == AppUtility.MenuItems.LabManagement.ToString()) //this was giving me an error in a switch case
                {

                }
                else if (role == AppUtility.MenuItems.Accounting.ToString()) //this was giving me an error in a switch case
                {

                }
                else if (role == AppUtility.MenuItems.Operation.ToString()) //this was giving me an error in a switch case
                {

                }
                else if (role == AppUtility.MenuItems.Expenses.ToString()) //this was giving me an error in a switch case
                {

                }
                else if (role == AppUtility.MenuItems.Biomarkers.ToString()) //this was giving me an error in a switch case
                {

                }
                else if (role == AppUtility.MenuItems.Income.ToString()) //this was giving me an error in a switch case
                {

                }
                else if (role == AppUtility.MenuItems.TimeKeeper.ToString()) //this was giving me an error in a switch case
                {

                }
                else if (role == AppUtility.MenuItems.Users.ToString()) //this was giving me an error in a switch case
                {

                }
            }

            return View(registerUserViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult GetHomeView()
        {
            //Adina's code: should not go to IndexAdmin otherwise if not Admin will say denied Index will take them to IndexAdmin if they're an admin
            return RedirectToAction("Index", "Home");
            //Faige's original code below:
            //return View("~/Views/Home/IndexAdmin.cshtml");
        }
    }
}