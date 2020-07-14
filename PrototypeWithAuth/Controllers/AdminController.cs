using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
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
        public IActionResult CreateUserModal()
        {
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();
            return PartialView(registerUserViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> CreateUserModal(RegisterUserViewModel registerUserViewModel)
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
                    UserNum = usernum
                };

                var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);
                //var role = _context.Roles.Where(r => r.Name == "Admin").FirstOrDefault().Id;
                if (result.Succeeded)
                {
                    //await _userManager.AddToRoleAsync(user, "Admin");
                    if (registerUserViewModel.SelectedOrders != null)
                    {
                        if (registerUserViewModel.SelectedOrders[0] == AppUtility.MenuItems.OrdersAndInventory.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.OrdersAndInventory.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedProtocols != null)
                    {
                        if (registerUserViewModel.SelectedProtocols[0] == AppUtility.MenuItems.Protocols.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Protocols.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedLabManagement != null)
                    {
                        if (registerUserViewModel.SelectedLabManagement[0] == AppUtility.MenuItems.LabManagement.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.LabManagement.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedAccounting != null)
                    {
                        if (registerUserViewModel.SelectedAccounting[0] == AppUtility.MenuItems.Accounting.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Accounting.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedOperations != null)
                    {
                        if (registerUserViewModel.SelectedOperations[0] == AppUtility.MenuItems.Operation.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Operation.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedExpenses != null)
                    {
                        if (registerUserViewModel.SelectedExpenses[0] == AppUtility.MenuItems.Expenses.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Expenses.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedBiomarkers != null)
                    {
                        if (registerUserViewModel.SelectedBiomarkers[0] == AppUtility.MenuItems.Biomarkers.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Biomarkers.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedIncome != null)
                    {
                        if (registerUserViewModel.SelectedIncome[0] == AppUtility.MenuItems.Income.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Income.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedTimekeeper != null)
                    {
                        if (registerUserViewModel.SelectedTimekeeper[0] == AppUtility.MenuItems.TimeKeeper.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.TimeKeeper.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedUsers != null)
                    {
                        if (registerUserViewModel.SelectedUsers[0] == AppUtility.MenuItems.Users.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Users.ToString());
                        }
                    }
                    //await _signManager.SignInAsync(user, false);  --> this would sign in the user automatically. we don't want that.
                    //return RedirectToAction("Index", "ApplicationUsers");
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
        public IActionResult CreateUser()
        {
            TempData["PageType"] = AppUtility.UserPageTypeEnum.Add;
            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();
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
                    //await _userManager.AddToRoleAsync(user, "Admin");
                    if (registerUserViewModel.SelectedOrders != null)
                    {
                        if (registerUserViewModel.SelectedOrders[0] == AppUtility.MenuItems.OrdersAndInventory.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.OrdersAndInventory.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedProtocols != null)
                    {
                        if (registerUserViewModel.SelectedProtocols[0] == AppUtility.MenuItems.Protocols.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Protocols.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedLabManagement != null)
                    {
                        if (registerUserViewModel.SelectedLabManagement[0] == AppUtility.MenuItems.LabManagement.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.LabManagement.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedAccounting != null)
                    {
                        if (registerUserViewModel.SelectedAccounting[0] == AppUtility.MenuItems.Accounting.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Accounting.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedOperations != null)
                    {
                        if (registerUserViewModel.SelectedOperations[0] == AppUtility.MenuItems.Operation.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Operation.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedExpenses != null)
                    {
                        if (registerUserViewModel.SelectedExpenses[0] == AppUtility.MenuItems.Expenses.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Expenses.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedBiomarkers != null)
                    {
                        if (registerUserViewModel.SelectedBiomarkers[0] == AppUtility.MenuItems.Biomarkers.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Biomarkers.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedIncome != null)
                    {
                        if (registerUserViewModel.SelectedIncome[0] == AppUtility.MenuItems.Income.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Income.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedTimekeeper != null)
                    {
                        if (registerUserViewModel.SelectedTimekeeper[0] == AppUtility.MenuItems.TimeKeeper.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.TimeKeeper.ToString());
                        }
                    }
                    if (registerUserViewModel.SelectedUsers != null)
                    {
                        if (registerUserViewModel.SelectedUsers[0] == AppUtility.MenuItems.Users.ToString())
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Users.ToString());
                        }
                    }
                    //await _signManager.SignInAsync(user, false);  --> this would sign in the user automatically. we don't want that.
                    //return RedirectToAction("Index", "ApplicationUsers");
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
                    registerUserViewModel.SelectedOrders[0] = AppUtility.MenuItems.OrdersAndInventory.ToString();
                }
                else if (role == AppUtility.MenuItems.Protocols.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.SelectedProtocols[0] = AppUtility.MenuItems.Protocols.ToString();
                }
                else if (role == AppUtility.MenuItems.LabManagement.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.SelectedLabManagement[0] = AppUtility.MenuItems.LabManagement.ToString();
                }
                else if (role == AppUtility.MenuItems.Accounting.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.SelectedAccounting[0] = AppUtility.MenuItems.Accounting.ToString();
                }
                else if (role == AppUtility.MenuItems.Operation.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.SelectedOperations[0] = AppUtility.MenuItems.Operation.ToString();
                }
                else if (role == AppUtility.MenuItems.Expenses.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.SelectedExpenses[0] = AppUtility.MenuItems.Expenses.ToString();
                }
                else if (role == AppUtility.MenuItems.Biomarkers.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.SelectedBiomarkers[0] = AppUtility.MenuItems.Biomarkers.ToString();
                }
                else if (role == AppUtility.MenuItems.Income.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.SelectedIncome[0] = AppUtility.MenuItems.Income.ToString();
                }
                else if (role == AppUtility.MenuItems.TimeKeeper.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.SelectedTimekeeper[0] = AppUtility.MenuItems.TimeKeeper.ToString();
                }
                else if (role == AppUtility.MenuItems.Users.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.SelectedUsers[0] = AppUtility.MenuItems.Users.ToString();
                }
            }

            return View(registerUserViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult EditUserModal(string id)
        {
            UserItemViewModel userItemViewModel = new UserItemViewModel();
            userItemViewModel.ApplicationUser = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            return PartialView(userItemViewModel);
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