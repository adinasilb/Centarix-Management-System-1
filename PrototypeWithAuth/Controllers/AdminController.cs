﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MailKit.Net.Smtp;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using MimeKit;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace PrototypeWithAuth.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private SignInManager<ApplicationUser> _signManager;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment hostingEnvironment, UrlEncoder urlEncoder)
        {
            _context = context;
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
            _urlEncoder = urlEncoder;
            //CreateSingleRole();
        }


        [HttpGet]
        [Authorize(Roles = "Admin , Users")]
        public IActionResult Index()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.UserPageTypeEnum.User;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.UserSideBarEnum.UsersList;

            ViewBag.ErrorMessage = ViewBag.ErrorMessage;

            List<Employee> users = new List<Employee>();
            users = _context.Employees
                //.Where(u => !u.IsSuspended) //instead of using lockout use bool so needstoreset password will be shown
                //.Where(u => u.NeedsToResetPassword? !u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null)
                //.OrderBy(u => u.UserNum)
                .ToList();
            var UsersList = _context.Users.ToList();
            bool IsCEO = false;
            if (User.IsInRole("CEO"))
            {
                users = _context.Employees.OrderBy(u => u.UserNum).ToList(); //The CEO can see all users even the ones that are suspended 
                IsCEO = true;
            }

            UserIndexViewModel userIndexViewModel = new UserIndexViewModel()
            {
                ApplicationUsers = users,
                IsCEO = IsCEO
            };
            return View(userIndexViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult RegisterUser()

        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.UserPageTypeEnum.User;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.UserSideBarEnum.UsersAdd;
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
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.UserPageTypeEnum.User;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.UserSideBarEnum.UsersAdd;

            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();

            registerUserViewModel.NewEmployee = new Employee()
            {
                StartedWorking = DateTime.Today
            };
            registerUserViewModel.JobCategoryTypes = _context.JobCategoryTypes.Select(jc => jc).ToList();
            registerUserViewModel.EmployeeStatuses = _context.EmployeeStatuses.Select(es => es).ToList();
            registerUserViewModel.MaritalStatuses = _context.MaritalStatuses.Select(ms => ms).ToList();
            registerUserViewModel.Degrees = _context.Degrees.Select(d => d).ToList();
            registerUserViewModel.Citizenships = _context.Citizenships.Select(c => c).ToList();

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
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Reports, Name="General", Selected=false }
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


        private async Task CreateSingleRole()
        {
            var user = _context.Users.Where(u => u.Email == "adina@centarix.com").FirstOrDefault();
            await _userManager.AddToRoleAsync(user, "CEO");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel registerUserViewModel)
        {
            var userid = 0;
            var usernum = 1;
            if (_context.Users.Any())
            {
                usernum = _context.Users.OrderByDescending(u => u.UserNum).FirstOrDefault().UserNum + 1;
            }
            var UserType = registerUserViewModel.NewEmployee.EmployeeStatusID;

            var user = new Employee()
            {
                /*User*/
                UserName = registerUserViewModel.Email,
                Email = registerUserViewModel.Email,
                FirstName = registerUserViewModel.FirstName,
                LastName = registerUserViewModel.LastName,
                SecureAppPass = registerUserViewModel.SecureAppPass,
                CentarixID = registerUserViewModel.CentarixID,
                PhoneNumber = registerUserViewModel.PhoneNumber,
                PhoneNumber2 = registerUserViewModel.PhoneNumber2,
                UserNum = usernum,
                LabMonthlyLimit = registerUserViewModel.LabMonthlyLimit,
                LabUnitLimit = registerUserViewModel.LabUnitLimit,
                LabOrderLimit = registerUserViewModel.LabOrderLimit,
                OperationMonthlyLimit = registerUserViewModel.OperationMonthlyLimit,
                OperationUnitLimit = registerUserViewModel.OperationUnitLimit,
                OperationOrderLimit = registerUserViewModel.OperaitonOrderLimit,
                DateCreated = DateTime.Now,
                EmployeeStatusID = registerUserViewModel.NewEmployee.EmployeeStatusID,
                IsUser = true,
                NeedsToResetPassword = true,
                TwoFactorEnabled = true
            };
            if (UserType == 4)
            {

            }
            else
            {
                /*User*/
                /*Employee*/
                user.IsUser = true;
                user.StartedWorking = registerUserViewModel.NewEmployee.StartedWorking;
                user.DOB = registerUserViewModel.NewEmployee.DOB;
                user.GrossSalary = registerUserViewModel.NewEmployee.GrossSalary;
                user.EmployerTax = registerUserViewModel.NewEmployee.EmployerTax;
                user.IncomeTax = registerUserViewModel.NewEmployee.IncomeTax;
                user.TaxCredits = registerUserViewModel.NewEmployee.TaxCredits;
                user.VacationDays = registerUserViewModel.NewEmployee.VacationDays;
                user.JobTitle = registerUserViewModel.NewEmployee.JobTitle;
                user.DegreeID = registerUserViewModel.NewEmployee.DegreeID;
                user.IDNumber = registerUserViewModel.NewEmployee.IDNumber;
                user.MaritalStatusID = registerUserViewModel.NewEmployee.MaritalStatusID;
                /*phonenumber2 is not working --> talk to Debbie*/
                user.CitizenshipID = registerUserViewModel.NewEmployee.CitizenshipID;
                user.JobCategoryTypeID = registerUserViewModel.NewEmployee.JobCategoryTypeID;
                /*Salaried Employee*/
            }
            var IsUser = true;
            if (registerUserViewModel.Password == "" || registerUserViewModel.Password == null)
            {
                IsUser = false;
                var newPassword = GeneratePassword(true, true, true, true, false, 10);
                registerUserViewModel.Password = newPassword;
            }
            var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);
            //var role = _context.Roles.Where(r => r.Name == "Admin").FirstOrDefault().Id;
            if (result.Succeeded)
            {
                if (!IsUser)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = new DateTime(2999, 01, 01);
                    user.IsUser = false;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                if (user.NeedsToResetPassword)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = new DateTime(2999, 01, 01);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                switch (UserType)
                {
                    case 1: /*Salaried Employee*/
                        var salariedEmployee = new SalariedEmployee()
                        {
                            EmployeeId = user.Id,
                            HoursPerDay = registerUserViewModel.NewEmployee.SalariedEmployee.HoursPerDay
                        };
                        _context.Add(salariedEmployee);
                        break;
                    case 2: /*Freelancer*/
                        var freelancer = new Freelancer()
                        {
                            EmployeeId = user.Id
                        };
                        _context.Add(freelancer);
                        break;
                    case 3: /*Advisor*/
                        break;
                }
                await _context.SaveChangesAsync();


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
                        await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Reports.ToString());
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

                if (registerUserViewModel.UserImageSaved == "true")
                {
                    //delete old photo
                    string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                    DirectoryInfo dir1 = new DirectoryInfo(uploadFolder1);
                    FileInfo[] files1 = dir1.GetFiles(user.UserNum + ".*");
                    if (files1.Length > 0)
                    {
                        foreach (FileInfo file in files1)
                        {
                            System.IO.File.Delete(file.FullName);
                        }
                    }

                    //add new photo
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                    DirectoryInfo dir = new DirectoryInfo(uploadFolder);
                    FileInfo[] files = dir.GetFiles("TempUserImage" + ".*");
                    if (files.Length > 0)
                    {
                        //File exists
                        foreach (FileInfo file in files)
                        {
                            //System.IO.File.Move(file., user.UserNum.ToString());
                            file.MoveTo(Path.Combine(uploadFolder, user.UserNum.ToString() + file.Extension));
                        }
                    }

                    //should we move the delete here and test for the extension just in case it breaks over there

                }


                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                string confirmationLink = Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { area = "Identity", userId = userId, code = code },
            protocol: Request.Scheme);


                var message = new MimeMessage();

                //instantiate the body builder
                var builder = new BodyBuilder();





                //add a "From" Email
                message.From.Add(new MailboxAddress("Elixir", "elixir@centarix.com"));

                // add a "To" Email
                message.To.Add(new MailboxAddress(user.FirstName, user.Email));

                //subject
                message.Subject = "Confirm centarix sign-up Link";

                //body
                builder.TextBody = confirmationLink;

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {

                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("elixir@centarix.com", "cdbmhjidnzoghqvt");
                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception ex)
                    {
                    }

                    client.Disconnect(true);
                }
                //}
                //else
                //{
                //    foreach (var error in result.Errors)
                //    {
                //        ModelState.AddModelError("", error.Description);
                //    }
                //}

            }
            else
            {
                ViewBag.ErrorMessage = "User Failed to add. Please try again.";
                foreach (var e in result.Errors)
                {
                    ViewBag.ErrorMessage += "/n " + e;
                }
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> EditUser(string id)
        {
            return await editUserFunction(id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public async Task<IActionResult> EditUserPartial(string id, int? Tab)
        {
            return await editUserFunction(id, Tab);
        }

        [HttpPost]
        [Authorize(Roles = "Admin , Users")]
        public async Task<IActionResult> EditUser(RegisterUserViewModel registerUserViewModel)
        {
            try
            {
                Employee userEditted = userEditted = await _context.Employees.Where(u => u.Id == registerUserViewModel.ApplicationUserID).FirstOrDefaultAsync();
                var selectedStatusID = registerUserViewModel.NewEmployee.EmployeeStatusID;
                Employee employeeEditted = await _context.Employees.Where(e => e.Id == registerUserViewModel.ApplicationUserID).FirstOrDefaultAsync();
                var oldSelectedStatus = employeeEditted.EmployeeStatusID;
                if (selectedStatusID == 4)
                {
                    //never was an employee only was a user and wants to update info                 
                    userEditted.UserName = registerUserViewModel.Email;
                    userEditted.CentarixID = registerUserViewModel.CentarixID;
                    userEditted.FirstName = registerUserViewModel.FirstName;
                    userEditted.LastName = registerUserViewModel.LastName;
                    userEditted.Email = registerUserViewModel.Email;
                    userEditted.NormalizedEmail = registerUserViewModel.Email.ToUpper();
                    userEditted.PhoneNumber = registerUserViewModel.PhoneNumber;
                    //are users allowed to update their password
                    if (registerUserViewModel.SecureAppPass != null)
                    {
                        userEditted.SecureAppPass = registerUserViewModel.SecureAppPass;
                    }
                    userEditted.LabMonthlyLimit = registerUserViewModel.LabMonthlyLimit;
                    userEditted.LabUnitLimit = registerUserViewModel.LabUnitLimit;
                    userEditted.LabOrderLimit = registerUserViewModel.LabOrderLimit;
                    userEditted.OperationMonthlyLimit = registerUserViewModel.OperationMonthlyLimit;
                    userEditted.OperationUnitLimit = registerUserViewModel.OperationUnitLimit;
                    userEditted.OperationOrderLimit = registerUserViewModel.OperaitonOrderLimit;
                    userEditted.EmployeeStatusID = registerUserViewModel.NewEmployee.EmployeeStatusID;
                    _context.Update(userEditted);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    // still wants to remain an employee
                    employeeEditted.UserName = registerUserViewModel.Email;
                    employeeEditted.CentarixID = registerUserViewModel.CentarixID;
                    employeeEditted.FirstName = registerUserViewModel.FirstName;
                    employeeEditted.LastName = registerUserViewModel.LastName;
                    employeeEditted.Email = registerUserViewModel.Email;
                    employeeEditted.NormalizedEmail = registerUserViewModel.Email.ToUpper();
                    employeeEditted.PhoneNumber = registerUserViewModel.PhoneNumber;
                    //are users allowed to update their password
                    if (registerUserViewModel.SecureAppPass != null)
                    {
                        userEditted.SecureAppPass = registerUserViewModel.SecureAppPass;
                    }
                    employeeEditted.LabMonthlyLimit = registerUserViewModel.LabMonthlyLimit;
                    employeeEditted.LabUnitLimit = registerUserViewModel.LabUnitLimit;
                    employeeEditted.LabOrderLimit = registerUserViewModel.LabOrderLimit;
                    employeeEditted.OperationMonthlyLimit = registerUserViewModel.OperationMonthlyLimit;
                    employeeEditted.OperationUnitLimit = registerUserViewModel.OperationUnitLimit;
                    employeeEditted.OperationOrderLimit = registerUserViewModel.OperaitonOrderLimit;
                    employeeEditted.StartedWorking = registerUserViewModel.NewEmployee.StartedWorking;
                    employeeEditted.DOB = registerUserViewModel.NewEmployee.DOB;
                    employeeEditted.GrossSalary = registerUserViewModel.NewEmployee.GrossSalary;
                    employeeEditted.EmployerTax = registerUserViewModel.NewEmployee.EmployerTax;
                    employeeEditted.IncomeTax = registerUserViewModel.NewEmployee.IncomeTax;
                    employeeEditted.TaxCredits = registerUserViewModel.NewEmployee.TaxCredits;
                    employeeEditted.VacationDays = registerUserViewModel.NewEmployee.VacationDays;
                    employeeEditted.JobTitle = registerUserViewModel.NewEmployee.JobTitle;
                    employeeEditted.DegreeID = registerUserViewModel.NewEmployee.DegreeID;
                    employeeEditted.IDNumber = registerUserViewModel.NewEmployee.IDNumber;
                    employeeEditted.MaritalStatusID = registerUserViewModel.NewEmployee.MaritalStatusID;
                    employeeEditted.CitizenshipID = registerUserViewModel.NewEmployee.CitizenshipID;
                    employeeEditted.EmployeeStatusID = selectedStatusID;
                    employeeEditted.JobCategoryTypeID = registerUserViewModel.NewEmployee.JobCategoryTypeID;

                    _context.Update(employeeEditted);


                    switch (selectedStatusID)
                    {
                        case 1: /*Salaried Employee*/
                            var salariedEmployee = _context.SalariedEmployees.Where(x => x.EmployeeId == employeeEditted.Id).FirstOrDefault();
                            if (salariedEmployee == null)
                            {
                                salariedEmployee = new SalariedEmployee();
                            }
                            salariedEmployee.EmployeeId = employeeEditted.Id;
                            salariedEmployee.HoursPerDay = registerUserViewModel.NewEmployee.SalariedEmployee.HoursPerDay;
                            employeeEditted.SalariedEmployee = salariedEmployee;
                            break;
                        case 2: /*Freelancer*/
                            var freelancer = _context.Freelancers.Where(x => x.EmployeeId == employeeEditted.Id).FirstOrDefault();
                            if (freelancer == null)
                            {
                                freelancer = new Freelancer();
                            }
                            freelancer.EmployeeId = employeeEditted.Id;
                            employeeEditted.Freelancer = freelancer;
                            break;
                        case 3: /*Advisor*/
                            break;
                    }
                    await _context.SaveChangesAsync();
                }

                if (!String.IsNullOrEmpty(registerUserViewModel.Password))
                {
                    string resetToken = await _userManager.GeneratePasswordResetTokenAsync(employeeEditted);
                    IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(employeeEditted, resetToken, registerUserViewModel.Password);
                    if (passwordChangeResult.Succeeded)
                    {
                        employeeEditted.NeedsToResetPassword = true;
                        await _userManager.ResetAuthenticatorKeyAsync(employeeEditted); 
                        await _userManager.UpdateSecurityStampAsync(employeeEditted);
                        employeeEditted.LockoutEnabled = true;
                        employeeEditted.LockoutEnd = new DateTime(2999, 01, 01);
                        _context.Update(employeeEditted);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //TODO: alert the user that it didn't succeed!!!
                    }
                }

                //if password isn't blank - reset the password):
                //if (registerUserViewModel.Password != null)
                //{
                //    ApplicationUser cUser = await _userManager.FindByIdAsync(registerUserViewModel.ApplicationUserID);
                //    string hashpassword = _userManager.PasswordHasher.HashPassword(cUser, registerUserViewModel.Password);
                //    cUser.PasswordHash = hashpassword;
                //    await _userManager.UpdateAsync(cUser);
                //}


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

                if (rolesList.Contains(AppUtility.MenuItems.Reports.ToString()) && !registerUserViewModel.ExpenseesRoles[0].Selected)
                {
                    await _userManager.RemoveFromRoleAsync(userEditted, AppUtility.MenuItems.Reports.ToString());
                }
                else if (!rolesList.Contains(AppUtility.MenuItems.Reports.ToString()) && registerUserViewModel.ExpenseesRoles[0].Selected)
                {
                    await _userManager.AddToRoleAsync(userEditted, AppUtility.MenuItems.Reports.ToString());
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



                if (registerUserViewModel.UserImageSaved == "true")
                {
                    //delete old photo
                    string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                    DirectoryInfo dir1 = new DirectoryInfo(uploadFolder1);
                    FileInfo[] files1 = dir1.GetFiles(registerUserViewModel.UserNum + ".*");
                    if (files1.Length > 0)
                    {
                        foreach (FileInfo file in files1)
                        {
                            System.IO.File.Delete(file.FullName);
                        }
                    }

                    //add new photo
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                    DirectoryInfo dir = new DirectoryInfo(uploadFolder);
                    FileInfo[] files = dir.GetFiles("TempUserImage" + ".*");
                    if (files.Length > 0)
                    {
                        //File exists
                        foreach (FileInfo file in files)
                        {
                            //System.IO.File.Move(file., user.UserNum.ToString());
                            file.MoveTo(Path.Combine(uploadFolder, registerUserViewModel.UserNum.ToString() + file.Extension));
                        }
                    }

                    //should we move the delete here and test for the extension just in case it breaks over there

                }

                //var folderName = "UserImages";
                //string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
                //var directory = Directory.CreateDirectory(uploadFolder);
                //if (registerUserViewModel.UserImage != null) //test for more than one???
                //{
                //    //create file
                //    var indexOfDot = registerUserViewModel.UserImage.FileName.IndexOf(".");
                //    var extension = registerUserViewModel.UserImage.FileName.Substring(indexOfDot, registerUserViewModel.UserImage.FileName.Length - indexOfDot);
                //    string uniqueFileName = userEditted.UserNum.ToString() + extension;
                //    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                //    var stream = new FileStream(filePath, FileMode.Create);
                //    registerUserViewModel.UserImage.CopyTo(stream);

                //    var pathToSave = Path.Combine(folderName, uniqueFileName);
                //    userEditted.UserImage = "/" + pathToSave;
                //    _context.Update(employeeEditted);
                //    await _context.SaveChangesAsync();
                //    stream.Close();
                //}

            }
            catch (DbUpdateException ex)
            {

            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult DeleteUserModal(string Id)
        {
            var user = _context.Users.Where(u => u.Id == Id).FirstOrDefault();
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
        public async Task<IActionResult> UserImageModal()
        {
            return PartialView();
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin, Users")]
        //public async Task<IActionResult> SaveUserImage()
        //{
        //    return View(); //See what this should be doing...
        //}

        [HttpGet]
        [Authorize(Roles = "CEO")]
        public IActionResult SuspendUserModal(string Id)
        {
            var user = _context.Users.Where(u => u.Id == Id).FirstOrDefault();
            return PartialView(user);
        }

        [HttpPost]
        [Authorize(Roles = "CEO")]
        public async Task<IActionResult> SuspendUserModal(ApplicationUser applicationUser)
        {
            applicationUser = _context.Users.Where(u => u.Id == applicationUser.Id).FirstOrDefault();
            if (applicationUser.LockoutEnabled == true && (applicationUser.LockoutEnd > DateTime.Now))
            {
                applicationUser.IsSuspended = false;
                applicationUser.LockoutEnabled = false;
                applicationUser.LockoutEnd = DateTime.Now;
            }
            else
            {
                applicationUser.IsSuspended = true;
                applicationUser.LockoutEnabled = true;
                applicationUser.LockoutEnd = new DateTime(2999, 01, 01);
            }
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


        public async Task<IActionResult> editUserFunction(string id, int? Tab = 0)
        {
            var userSelected = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (userSelected != null)
            {
                var registerUserViewModel = new RegisterUserViewModel
                {
                    ApplicationUserID = userSelected.Id,
                    UserNum = userSelected.UserNum,
                    FirstName = userSelected.FirstName,
                    LastName = userSelected.LastName,
                    Email = userSelected.Email,
                    PhoneNumber = userSelected.PhoneNumber,
                    CentarixID = userSelected.CentarixID,
                    UserImageSaved = userSelected.UserImage,
                    //TODO: do we want to show the secure app pass??
                    LabMonthlyLimit = userSelected.LabMonthlyLimit,
                    LabUnitLimit = userSelected.LabUnitLimit,
                    LabOrderLimit = userSelected.LabOrderLimit,
                    OperationMonthlyLimit = userSelected.OperationMonthlyLimit,
                    OperationUnitLimit = userSelected.OperationUnitLimit,
                    OperaitonOrderLimit = userSelected.OperationOrderLimit,
                    Tab = Tab ?? 1
                };

                string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                DirectoryInfo dir1 = new DirectoryInfo(uploadFolder1);
                FileInfo[] files1 = dir1.GetFiles(registerUserViewModel.UserNum + ".*");
                if (files1.Length > 0)
                {
                    foreach (FileInfo file in files1)
                    {
                        registerUserViewModel.UserImage = file.FullName;
                    }
                }


                registerUserViewModel.NewEmployee = new Employee(); //this may be able to be taken out but it might cause errors with users if taken out. so check first

                registerUserViewModel.NewEmployee = _context.Employees.Where(u => u.Id == id).Where(u => !u.LockoutEnabled || u.LockoutEnd <= DateTime.Now || u.LockoutEnd == null).Include(s => s.SalariedEmployee).FirstOrDefault();
                registerUserViewModel.EmployeeStatuses = _context.EmployeeStatuses.Select(es => es).ToList();
                registerUserViewModel.JobCategoryTypes = _context.JobCategoryTypes.Select(jt => jt).ToList();
                registerUserViewModel.MaritalStatuses = _context.MaritalStatuses.Select(ms => ms).ToList();
                registerUserViewModel.Degrees = _context.Degrees.Select(d => d).ToList();
                registerUserViewModel.Citizenships = _context.Citizenships.Select(c => c).ToList();
                if (registerUserViewModel.NewEmployee == null)
                {
                    registerUserViewModel.NewEmployee = new Employee();
                    registerUserViewModel.NewEmployee.EmployeeStatusID = 4;
                }


                //round job scope
                string WorkScope = registerUserViewModel.NewEmployee?.SalariedEmployee?.WorkScope.ToString("0.00") ?? "0";
                registerUserViewModel.NewEmployeeWorkScope = Decimal.Parse(WorkScope);



                //var userToShow = _context.Users.Where(u => u.Id == id).FirstOrDefault();
                var rolesList = await _userManager.GetRolesAsync(userSelected).ConfigureAwait(false);

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
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Reports, Name="General", Selected=false }
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
                    else if (role == AppUtility.MenuItems.Reports.ToString()) //this was giving me an error in a switch case
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

                if (registerUserViewModel.NewEmployee.UserImage == null)
                {
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                    string filePath = Path.Combine(uploadFolder, "profile_circle_big.png");
                    registerUserViewModel.NewEmployee.UserImage = filePath;
                }

                //FileStreamResult fs;
                //using (var img = System.Drawing.Image.FromStream(file.OpenReadStream()))
                //{
                //    Stream ms = new MemoryStream(img.Resize(100, 100).ToByteArray());

                //    return new FileStreamResult(ms, "image/jpg");
                //}

                //                using(var stream = File.OpenRead(registerUserViewModel.NewEmployee.UserImage))
                //{
                //                    var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                //                    {
                //                        Headers = new HeaderDictionary(),
                //                        ContentType = "application/pdf"
                //                    };
                //                }



                return PartialView(registerUserViewModel);

            }
            else
            {
                return RedirectToAction("Index", "ApplicationUsers");
            }
        }

        public JsonResult GetGeneratedPassword()
        {
            var password = "";
            while (!PasswordIsValid(password))
            {
                password = GeneratePassword();
            }

            return Json(password);
        }
        private static string GeneratePassword(bool includeLowercase = true, bool includeUppercase = true, bool includeNumeric = true, bool includeSpecial = true, bool includeSpaces = false, int lengthOfPassword = 12)
        {
            const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
            const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
            const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMERIC_CHARACTERS = "0123456789";
            const string SPECIAL_CHARACTERS = @"!#$%&*@\";
            const string SPACE_CHARACTER = " ";
            const int PASSWORD_LENGTH_MIN = 8;
            const int PASSWORD_LENGTH_MAX = 128;

            if (lengthOfPassword < PASSWORD_LENGTH_MIN || lengthOfPassword > PASSWORD_LENGTH_MAX)
            {
                return "Password length must be between 8 and 128.";
            }

            string characterSet = "";

            if (includeLowercase)
            {
                characterSet += LOWERCASE_CHARACTERS;
            }

            if (includeUppercase)
            {
                characterSet += UPPERCASE_CHARACTERS;
            }

            if (includeNumeric)
            {
                characterSet += NUMERIC_CHARACTERS;
            }

            if (includeSpecial)
            {
                characterSet += SPECIAL_CHARACTERS;
            }

            if (includeSpaces)
            {
                characterSet += SPACE_CHARACTER;
            }

            char[] password = new char[lengthOfPassword];
            int characterSetLength = characterSet.Length;

            System.Random random = new System.Random();
            for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
            {
                password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];

                bool moreThanTwoIdenticalInARow =
                    characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
                    && password[characterPosition] == password[characterPosition - 1]
                    && password[characterPosition - 1] == password[characterPosition - 2];

                if (moreThanTwoIdenticalInARow)
                {
                    characterPosition--;
                }
            }

            return string.Join(null, password);
        }


        public string SaveTempUserImage(UserImageViewModel userImageViewModel)
        {
            var SavedUserImagePath = "";

            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
            Directory.CreateDirectory(uploadFolder);
            if (userImageViewModel.FileToSave != null) //test for more than one???
            {
                var FileName = "TempUserImage";

                DirectoryInfo dir = new DirectoryInfo(uploadFolder);
                FileInfo[] files = dir.GetFiles(FileName + ".*");
                if (files.Length > 0)
                {
                    //File exists
                    foreach (FileInfo file in files)
                    {
                        System.IO.File.Delete(file.FullName);
                    }
                }

                var indexOfDot = userImageViewModel.FileToSave.FileName.IndexOf(".");
                var extension = userImageViewModel.FileToSave.FileName.Substring(indexOfDot, userImageViewModel.FileToSave.FileName.Length - indexOfDot);
                string uniqueFileName = FileName + extension;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var FileStream = new FileStream(filePath, FileMode.Create))
                {
                    try
                    {
                        userImageViewModel.FileToSave.CopyTo(FileStream);
                        SavedUserImagePath = AppUtility.GetLastFiles(filePath, 2);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            return SavedUserImagePath;

        }


       
        private static bool PasswordIsValid(string password, bool includeLowercase = true, bool includeUppercase = true, bool includeNumeric = true, bool includeSpecial = true, bool includeSpaces = false)
        {
            const string REGEX_LOWERCASE = @"[a-z]";
            const string REGEX_UPPERCASE = @"[A-Z]";
            const string REGEX_NUMERIC = @"[\d]";
            const string REGEX_SPECIAL = @"([!#$%&*@\\])+";
            const string REGEX_SPACE = @"([ ])+";

            bool lowerCaseIsValid = !includeLowercase || (includeLowercase && Regex.IsMatch(password, REGEX_LOWERCASE));
            bool upperCaseIsValid = !includeUppercase || (includeUppercase && Regex.IsMatch(password, REGEX_UPPERCASE));
            bool numericIsValid = !includeNumeric || (includeNumeric && Regex.IsMatch(password, REGEX_NUMERIC));
            bool symbolsAreValid = !includeSpecial || (includeSpecial && Regex.IsMatch(password, REGEX_SPECIAL));
            bool spacesAreValid = !includeSpaces || (includeSpaces && Regex.IsMatch(password, REGEX_SPACE));

            return lowerCaseIsValid && upperCaseIsValid && numericIsValid && symbolsAreValid && spacesAreValid;
        }

    }
}