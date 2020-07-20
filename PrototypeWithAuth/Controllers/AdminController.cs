﻿using System;
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
                .Where(u => u.IsDeleted == false)
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
            registerUserViewModel.OrderRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.OrdersAndInventory, Name="General", Selected=false }
            };
            registerUserViewModel.ProtocolRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Protocols, Name="General", Selected=false }
            };
            registerUserViewModel.OperationRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Operation, Name="General", Selected=false }
            };
            registerUserViewModel.BiomarkerRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Biomarkers, Name="General", Selected=false }
            };
            registerUserViewModel.TimekeeperRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.TimeKeeper, Name="General", Selected=false }
            };
            registerUserViewModel.LabManagementRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.LabManagement, Name="General", Selected=false }
            };
            registerUserViewModel.AccountingRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Accounting, Name="General", Selected=false }
            };
            registerUserViewModel.ExpenseesRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Expenses, Name="General", Selected=false }
            };
            registerUserViewModel.IncomeRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Income, Name="General", Selected=false }
            };
            registerUserViewModel.UserRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Users, Name="General", Selected=false }
            };

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
                    FirstName = registerUserViewModel.FirstName,
                    LastName = registerUserViewModel.LastName,
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
                    foreach (var orderRole in registerUserViewModel.OrderRoles)
                    {
                        if (orderRole.Name == "General" && orderRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.OrdersAndInventory.ToString());
                        }
                    }
                    foreach (var protcolRole in registerUserViewModel.ProtocolRoles)
                    {
                        if (protcolRole.Name == "General" && protcolRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Protocols.ToString());
                        }
                    }
                    foreach (var operationRole in registerUserViewModel.OperationRoles)
                    {
                        if (operationRole.Name == "General" && operationRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Operation.ToString());
                        }
                    }
                    foreach (var biomarkerRole in registerUserViewModel.BiomarkerRoles)
                    {
                        if (biomarkerRole.Name == "General" && biomarkerRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Biomarkers.ToString());
                        }
                    }
                    foreach (var timekeeperRole in registerUserViewModel.TimekeeperRoles)
                    {
                        if (timekeeperRole.Name == "General" && timekeeperRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.TimeKeeper.ToString());
                        }
                    }
                    foreach (var labmanagementRole in registerUserViewModel.LabManagementRoles)
                    {
                        if (labmanagementRole.Name == "General" && labmanagementRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.LabManagement.ToString());
                        }
                    }
                    foreach (var accountingRole in registerUserViewModel.AccountingRoles)
                    {
                        if (accountingRole.Name == "General" && accountingRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Accounting.ToString());
                        }
                    }
                    foreach (var expensesRole in registerUserViewModel.ExpenseesRoles)
                    {
                        if (expensesRole.Name == "General" && expensesRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Expenses.ToString());
                        }
                    }
                    foreach (var incomeRole in registerUserViewModel.IncomeRoles)
                    {
                        if (incomeRole.Name == "General" && incomeRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Income.ToString());
                        }
                    }
                    foreach (var usersRole in registerUserViewModel.UserRoles)
                    {
                        if (usersRole.Name == "General" && usersRole.Selected == true)
                        {
                            await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Users.ToString());
                        }
                    }
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
            var registerUserViewModel = _context.Users.Where(u => u.Id == id).Where(u => u.IsDeleted == false)
                .Select(u => new RegisterUserViewModel
                {
                    ApplicationUserID = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
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

            registerUserViewModel.OrderRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.OrdersAndInventory, Name="General", Selected=false }
            };
            registerUserViewModel.ProtocolRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Protocols, Name="General", Selected=false }
            };
            registerUserViewModel.OperationRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Operation, Name="General", Selected=false }
            };
            registerUserViewModel.BiomarkerRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Biomarkers, Name="General", Selected=false }
            };
            registerUserViewModel.TimekeeperRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.TimeKeeper, Name="General", Selected=false }
            };
            registerUserViewModel.LabManagementRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.LabManagement, Name="General", Selected=false }
            };
            registerUserViewModel.AccountingRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Accounting, Name="General", Selected=false }
            };
            registerUserViewModel.ExpenseesRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Expenses, Name="General", Selected=false }
            };
            registerUserViewModel.IncomeRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Income, Name="General", Selected=false }
            };
            registerUserViewModel.UserRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Users, Name="General", Selected=false }
            };
            foreach (var role in rolesList)
            {
                if (role == AppUtility.MenuItems.OrdersAndInventory.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.OrderRoles[0].Selected = true;
                }
                else if (role == AppUtility.MenuItems.Protocols.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.ProtocolRoles[0].Selected = true;
                }
                else if (role == AppUtility.MenuItems.LabManagement.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.LabManagementRoles[0].Selected = true;
                }
                else if (role == AppUtility.MenuItems.Accounting.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.AccountingRoles[0].Selected = true;
                }
                else if (role == AppUtility.MenuItems.Operation.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.OperationRoles[0].Selected = true;
                }
                else if (role == AppUtility.MenuItems.Expenses.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.ExpenseesRoles[0].Selected = true;
                }
                else if (role == AppUtility.MenuItems.Biomarkers.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.BiomarkerRoles[0].Selected = true;
                }
                else if (role == AppUtility.MenuItems.Income.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.IncomeRoles[0].Selected = true;
                }
                else if (role == AppUtility.MenuItems.TimeKeeper.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.TimekeeperRoles[0].Selected = true;
                }
                else if (role == AppUtility.MenuItems.Users.ToString()) //this was giving me an error in a switch case
                {
                    registerUserViewModel.UserRoles[0].Selected = true;
                }
            }

            return View(registerUserViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin , Users")]
        public async Task<IActionResult> EditUser(RegisterUserViewModel registerUserViewModel)
        {
            var userEditted = _context.Users.Where(u => u.Id == registerUserViewModel.ApplicationUserID).FirstOrDefault();
            userEditted.UserName = registerUserViewModel.UserName;
            userEditted.FirstName = registerUserViewModel.FirstName;
            userEditted.LastName = registerUserViewModel.LastName;
            userEditted.Email = registerUserViewModel.Email;
            userEditted.SecureAppPass = registerUserViewModel.SecureAppPass;
            userEditted.LabMonthlyLimit = registerUserViewModel.LabMonthlyLimit;
            userEditted.LabUnitLimit = registerUserViewModel.LabUnitLimit;
            userEditted.LabOrderLimit = registerUserViewModel.LabOrderLimit;
            userEditted.OperationMonthlyLimit = registerUserViewModel.OperationMonthlyLimit;
            userEditted.OperationUnitLimit = registerUserViewModel.OperationUnitLimit;
            userEditted.OperaitonOrderLimit = registerUserViewModel.OperaitonOrderLimit;
            _context.Update(userEditted);
            _context.SaveChanges();

            var rolesList = await _userManager.GetRolesAsync(userEditted).ConfigureAwait(false); 

            if (rolesList.Contains(AppUtility.MenuItems.OrdersAndInventory.ToString()) && !registerUserViewModel.OrderRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.OrdersAndInventory.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.OrdersAndInventory.ToString()) && registerUserViewModel.OrderRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.OrdersAndInventory.ToString());
            }

            if (rolesList.Contains(AppUtility.MenuItems.Protocols.ToString()) && !registerUserViewModel.ProtocolRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.Protocols.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.Protocols.ToString()) && registerUserViewModel.ProtocolRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.Protocols.ToString());
            }

            if (rolesList.Contains(AppUtility.MenuItems.Operation.ToString()) && !registerUserViewModel.OperationRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.Operation.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.Operation.ToString()) && registerUserViewModel.OperationRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.Operation.ToString());
            }

            if (rolesList.Contains(AppUtility.MenuItems.Biomarkers.ToString()) && !registerUserViewModel.BiomarkerRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.Biomarkers.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.Biomarkers.ToString()) && registerUserViewModel.BiomarkerRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.Biomarkers.ToString());
            }

            if (rolesList.Contains(AppUtility.MenuItems.TimeKeeper.ToString()) && !registerUserViewModel.TimekeeperRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.TimeKeeper.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.TimeKeeper.ToString()) && registerUserViewModel.TimekeeperRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.TimeKeeper.ToString());
            }

            if (rolesList.Contains(AppUtility.MenuItems.LabManagement.ToString()) && !registerUserViewModel.LabManagementRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.LabManagement.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.LabManagement.ToString()) && registerUserViewModel.LabManagementRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.LabManagement.ToString());
            }

            if (rolesList.Contains(AppUtility.MenuItems.Accounting.ToString()) && !registerUserViewModel.AccountingRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.Accounting.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.Accounting.ToString()) && registerUserViewModel.AccountingRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.Accounting.ToString());
            }

            if (rolesList.Contains(AppUtility.MenuItems.Expenses.ToString()) && !registerUserViewModel.ExpenseesRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.Expenses.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.Expenses.ToString()) && registerUserViewModel.ExpenseesRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.Expenses.ToString());
            }

            if (rolesList.Contains(AppUtility.MenuItems.Income.ToString()) && !registerUserViewModel.IncomeRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.Income.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.Income.ToString()) && registerUserViewModel.IncomeRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.Income.ToString());
            }

            if (rolesList.Contains(AppUtility.MenuItems.Users.ToString()) && !registerUserViewModel.UserRoles[0].Selected)
            {
                await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.Users.ToString());
            }
            else if (!rolesList.Contains(AppUtility.MenuItems.Users.ToString()) && registerUserViewModel.UserRoles[0].Selected)
            {
                await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.Users.ToString());
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles ="Admin, Users")]
        public IActionResult DeleteUserModal(string Id)
        {
            var user = _context.Users.Where(u => u.Id == Id).Where(u => u.IsDeleted == false).FirstOrDefault();
            return PartialView(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> DeleteUserModal(ApplicationUser applicationUser)
        {
            applicationUser = _context.Users.Where(u => u.Id == applicationUser.Id).FirstOrDefault();
            applicationUser.IsDeleted = true;
            _context.Update(applicationUser);
            _context.SaveChanges();
            return RedirectToAction("Index");
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