using System;
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
using Abp.Extensions;
using System.Linq.Dynamic.Core;
using OpenCvSharp;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Controllers
{
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private SignInManager<ApplicationUser> _signManager;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UrlEncoder _urlEncoder;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment hostingEnvironment, UrlEncoder urlEncoder, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
            _urlEncoder = urlEncoder;
            _httpContextAccessor = httpContextAccessor;
            //CreateSingleRole();
        }


        [HttpGet]
        [Authorize(Roles = "Users")]
        public IActionResult Index(String? ErrorMessage =null)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersUser;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.List;

          
            UserIndexViewModel userIndexViewModel = GetUserIndexViewModel();
            userIndexViewModel.ErrorMessage = ErrorMessage;
            return View(userIndexViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public IActionResult _Index()
        {         
            UserIndexViewModel userIndexViewModel = GetUserIndexViewModel();
            return PartialView(userIndexViewModel);
        }
        private UserIndexViewModel GetUserIndexViewModel()
        {

            var users = _context.Employees.OrderBy(u => u.UserNum)
                .Select(u => new UserWithCentarixIDViewModel
                {
                    Employee = u,
                    CentarixID = AppUtility.GetEmployeeCentarixID(_context.CentarixIDs.Where(ci => ci.EmployeeID == u.Id).OrderBy(ci => ci.TimeStamp))
                });

            UserIndexViewModel userIndexViewModel = new UserIndexViewModel()
            {
                ApplicationUsers = users,
                IsCEO = false
            };
            return userIndexViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public IActionResult CreateUser()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersUser;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;

            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel();

            registerUserViewModel.NewEmployee = new Employee()
            {
                StartedWorking = DateTime.Today
            };
            FillViewDropdowns(registerUserViewModel);
            registerUserViewModel.OrderRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Requests, Name="General", Selected=false }
            };
            registerUserViewModel.ProtocolRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Protocols, Name="General", Selected=false }
            };
            registerUserViewModel.OperationRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Operations, Name="General", Selected=false }
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



        [HttpPost]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel registerUserViewModel)
        {
            var errorMessage = "";
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    
                    int userid = 0;
                    int usernum = 1;
                    if (_context.Users.Any())
                    {
                        usernum = _context.Users.OrderByDescending(u => u.UserNum).FirstOrDefault().UserNum + 1;
                    }
                    int UserType = registerUserViewModel.NewEmployee.EmployeeStatusID;

                    Employee user = new Employee()
                    {
                        /*User*/
                        UserName = registerUserViewModel.Email,
                        Email = registerUserViewModel.Email,
                        FirstName = registerUserViewModel.FirstName,
                        LastName = registerUserViewModel.LastName,
                        SecureAppPass = registerUserViewModel.SecureAppPass,
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
                        JobSubcategoryTypeID = registerUserViewModel.NewEmployee.JobSubcategoryTypeID,
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
                        if (user.StartedWorking > AppUtility.DateSoftwareLaunched)
                        {
                            user.LastLogin = user.StartedWorking;
                        }
                        else
                        {
                            user.LastLogin = AppUtility.DateSoftwareLaunched;
                        }
                        user.DOB = registerUserViewModel.NewEmployee.DOB;
                        user.GrossSalary = registerUserViewModel.NewEmployee.GrossSalary;
                        user.EmployerTax = registerUserViewModel.NewEmployee.EmployerTax;
                        user.IncomeTax = registerUserViewModel.NewEmployee.IncomeTax;
                        user.TaxCredits = registerUserViewModel.NewEmployee.TaxCredits;
                        user.VacationDays = registerUserViewModel.NewEmployee.VacationDays;
                        //user.JobSubcategoryTypeID = registerUserViewModel.NewEmployee.JobSubategoryTypeID;
                        user.DegreeID = registerUserViewModel.NewEmployee.DegreeID;
                        user.IDNumber = registerUserViewModel.NewEmployee.IDNumber;
                        user.MaritalStatusID = registerUserViewModel.NewEmployee.MaritalStatusID;
                        user.CitizenshipID = registerUserViewModel.NewEmployee.CitizenshipID;
                        user.JobSubcategoryTypeID = registerUserViewModel.NewEmployee.JobSubcategoryTypeID;
                        /*Salaried Employee*/
                    }

                    bool IsUser = true;
                    if (registerUserViewModel.Password == "" || registerUserViewModel.Password == null)
                    {
                        IsUser = false;
                        string newPassword = GeneratePassword(true, true, true, true, false, 10);
                        registerUserViewModel.Password = newPassword;
                    }
                    IdentityResult result = await _userManager.CreateAsync(user, registerUserViewModel.Password);
                    await _userManager.ResetAuthenticatorKeyAsync(user);
                    await _userManager.UpdateSecurityStampAsync(user);
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

                        //add in CentarixID
                        var employeeStatus = _context.EmployeeStatuses.Where(es => es.EmployeeStatusID == UserType).FirstOrDefault();
                        var currentNum = employeeStatus.LastCentarixID + 1;
                        var abbrev = employeeStatus.Abbreviation;
                        if (abbrev[1] == ' ')
                        {
                            abbrev = abbrev.Substring(0, 1);
                        }
                        var cID = abbrev + currentNum.ToString();
                        CentarixID centarixID = new CentarixID()
                        {
                            EmployeeID = user.Id,
                            CentarixIDNumber = cID,
                            IsCurrent = true,
                            TimeStamp = DateTime.Now,
                            Employee = _context.Employees.Where(e => e.Id == user.Id).FirstOrDefault()
                        };

                        _context.Add(centarixID);
                        await _context.SaveChangesAsync();

                        //update last ID
                        employeeStatus.LastCentarixID = currentNum;
                        employeeStatus.LastCentarixIDTimeStamp = DateTime.Now;
                        _context.Update(employeeStatus);
                        await _context.SaveChangesAsync();

                        switch (UserType)
                        {
                            case 1: /*Salaried Employee*/
                                SalariedEmployee salariedEmployee = new SalariedEmployee()
                                {
                                    EmployeeId = user.Id,
                                    HoursPerDay = registerUserViewModel.NewEmployee.SalariedEmployee.HoursPerDay
                                };
                                _context.Add(salariedEmployee);
                                break;
                            case 2: /*Freelancer*/
                                Freelancer freelancer = new Freelancer()
                                {
                                    EmployeeId = user.Id
                                };
                                _context.Add(freelancer);
                                break;
                            case 3: /*Advisor*/
                                Advisor advisor = new Advisor()
                                {
                                    EmployeeID = user.Id
                                };
                                _context.Add(advisor);
                                break;
                        }
                        await _context.SaveChangesAsync();

                        try
                        {

                            foreach (UserRoleViewModel orderRole in registerUserViewModel.OrderRoles)
                            {
                                if (orderRole.Name == "General" && orderRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Requests.ToString());
                                }
                            }
                            foreach (UserRoleViewModel protcolRole in registerUserViewModel.ProtocolRoles)
                            {
                                if (protcolRole.Name == "General" && protcolRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Protocols.ToString());
                                }
                            }
                            foreach (UserRoleViewModel operationRole in registerUserViewModel.OperationRoles)
                            {
                                if (operationRole.Name == "General" && operationRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Operations.ToString());
                                }
                            }
                            foreach (UserRoleViewModel biomarkerRole in registerUserViewModel.BiomarkerRoles)
                            {
                                if (biomarkerRole.Name == "General" && biomarkerRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Biomarkers.ToString());
                                }
                            }
                            foreach (UserRoleViewModel timekeeperRole in registerUserViewModel.TimekeeperRoles)
                            {
                                if (timekeeperRole.Name == "General" && timekeeperRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.TimeKeeper.ToString());
                                }
                            }
                            foreach (UserRoleViewModel labmanagementRole in registerUserViewModel.LabManagementRoles)
                            {
                                if (labmanagementRole.Name == "General" && labmanagementRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.LabManagement.ToString());
                                }
                            }
                            foreach (UserRoleViewModel accountingRole in registerUserViewModel.AccountingRoles)
                            {
                                if (accountingRole.Name == "General" && accountingRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Accounting.ToString());
                                }
                            }
                            foreach (UserRoleViewModel expensesRole in registerUserViewModel.ExpenseesRoles)
                            {
                                if (expensesRole.Name == "General" && expensesRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Reports.ToString());
                                }
                            }
                            foreach (UserRoleViewModel incomeRole in registerUserViewModel.IncomeRoles)
                            {
                                if (incomeRole.Name == "General" && incomeRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Income.ToString());
                                }
                            }
                            foreach (UserRoleViewModel usersRole in registerUserViewModel.UserRoles)
                            {
                                if (usersRole.Name == "General" && usersRole.Selected == true)
                                {
                                    await _userManager.AddToRoleAsync(user, AppUtility.MenuItems.Users.ToString());
                                }
                            
                            }
                           
                        }
                        catch (Exception ex)
                        {
                           errorMessage = "User saved successful but something went wrong while tring to add the roles. Please retry adding roles to the newly create user";
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
                                    user.UserImage = file.FullName;
                                }
                                _context.Update(user);
                                await _context.SaveChangesAsync();
                            }
                            //should we move the delete here and test for the extension just in case it breaks over there
                        }

                        if (IsUser)
                        {
                            SendConfimationEmail(user);
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
                        foreach (IdentityError e in result.Errors)
                        {
                            registerUserViewModel.ErrorMessage +=  "User Failed to add. Please try again. " + e.Code.ToString() + " " + e.Description.ToString();
                        }
                        //refill Model to view errors
                        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersUser;
                        TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
                        TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
                        registerUserViewModel.JobCategoryTypes = _context.JobCategoryTypes.Select(jc => jc).ToList();
                        registerUserViewModel.EmployeeStatuses = _context.EmployeeStatuses.Select(es => es).ToList();
                        registerUserViewModel.MaritalStatuses = _context.MaritalStatuses.Select(ms => ms).ToList();
                        registerUserViewModel.Degrees = _context.Degrees.Select(d => d).ToList();
                        registerUserViewModel.Citizenships = _context.Citizenships.Select(c => c).ToList();
                        return View("CreateUser", registerUserViewModel);
                    }
                    //throw new Exception();
                   await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    registerUserViewModel.ErrorMessage += ex.Message;
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersUser;
                    TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
                    TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
                    registerUserViewModel.JobCategoryTypes = _context.JobCategoryTypes.Select(jc => jc).ToList();
                    registerUserViewModel.EmployeeStatuses = _context.EmployeeStatuses.Select(es => es).ToList();
                    registerUserViewModel.MaritalStatuses = _context.MaritalStatuses.Select(ms => ms).ToList();
                    registerUserViewModel.Degrees = _context.Degrees.Select(d => d).ToList();
                    registerUserViewModel.Citizenships = _context.Citizenships.Select(c => c).ToList();
                    return View("CreateUser", registerUserViewModel);
                }
            }
          
            return RedirectToAction("Index", new { errorMessage });
        }

        public async void SendConfimationEmail(ApplicationUser user)
        {
            string userId = await _userManager.GetUserIdAsync(user);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string confirmationLink = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);


            MimeMessage message = new MimeMessage();

            //instantiate the body builder
            BodyBuilder builder = new BodyBuilder();

            //add a "From" Email
            message.From.Add(new MailboxAddress("Elixir", "elixir@centarix.com"));

            // add a "To" Email
            message.To.Add(new MailboxAddress(user.FirstName, user.Email));

            //subject
            message.Subject = "Confirm centarix sign-up Link";

            //body
            builder.TextBody = confirmationLink;

            message.Body = builder.ToMessageBody();

            using (SmtpClient client = new SmtpClient())
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
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> EditUser(string id)
        {
            return await editUserFunction(id);
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> EditUserPartial(string id, int? Tab)
        {
            return await editUserFunction(id, Tab);
        }

        [HttpPost]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> EditUser(RegisterUserViewModel registerUserViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int selectedStatusID = registerUserViewModel.NewEmployee.EmployeeStatusID;
                    Employee employeeEditted = await _context.Employees.Where(e => e.Id == registerUserViewModel.ApplicationUserID).FirstOrDefaultAsync();
                    int oldSelectedStatus = employeeEditted.EmployeeStatusID;
                    bool changedEmployeeStatus = false;
                    if (selectedStatusID != oldSelectedStatus)
                    {
                        changedEmployeeStatus = true;
                    }
                    if (selectedStatusID == 4)
                    {
                        //never was an employee only was a user and wants to update info                 
                        employeeEditted.UserName = registerUserViewModel.Email;
                        employeeEditted.FirstName = registerUserViewModel.FirstName;
                        employeeEditted.LastName = registerUserViewModel.LastName;
                        employeeEditted.Email = registerUserViewModel.Email;
                        employeeEditted.NormalizedEmail = registerUserViewModel.Email.ToUpper();
                        employeeEditted.PhoneNumber = registerUserViewModel.PhoneNumber;
                        //are users allowed to update their password
                        if (registerUserViewModel.SecureAppPass != null)
                        {
                            employeeEditted.SecureAppPass = registerUserViewModel.SecureAppPass;
                        }
                        employeeEditted.LabMonthlyLimit = registerUserViewModel.LabMonthlyLimit;
                        employeeEditted.LabUnitLimit = registerUserViewModel.LabUnitLimit;
                        employeeEditted.LabOrderLimit = registerUserViewModel.LabOrderLimit;
                        employeeEditted.OperationMonthlyLimit = registerUserViewModel.OperationMonthlyLimit;
                        employeeEditted.OperationUnitLimit = registerUserViewModel.OperationUnitLimit;
                        employeeEditted.OperationOrderLimit = registerUserViewModel.OperaitonOrderLimit;
                        employeeEditted.EmployeeStatusID = registerUserViewModel.NewEmployee.EmployeeStatusID;
                        _context.Update(employeeEditted);
                        await _context.SaveChangesAsync();

                        if (changedEmployeeStatus)
                        {
                            await AddNewCentarixID(employeeEditted.Id, 4);
                        }
                    }
                    else
                    {
                        // still wants to remain an employee
                        employeeEditted.UserName = registerUserViewModel.Email;
                        employeeEditted.FirstName = registerUserViewModel.FirstName;
                        employeeEditted.LastName = registerUserViewModel.LastName;
                        employeeEditted.Email = registerUserViewModel.Email;
                        employeeEditted.NormalizedEmail = registerUserViewModel.Email.ToUpper();
                        employeeEditted.PhoneNumber = registerUserViewModel.PhoneNumber;
                        //are users allowed to update their password
                        if (registerUserViewModel.SecureAppPass != null)
                        {
                            employeeEditted.SecureAppPass = registerUserViewModel.SecureAppPass;
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
                        employeeEditted.JobSubcategoryTypeID = registerUserViewModel.NewEmployee.JobSubcategoryTypeID;
                        employeeEditted.DegreeID = registerUserViewModel.NewEmployee.DegreeID;
                        employeeEditted.IDNumber = registerUserViewModel.NewEmployee.IDNumber;
                        employeeEditted.MaritalStatusID = registerUserViewModel.NewEmployee.MaritalStatusID;
                        employeeEditted.CitizenshipID = registerUserViewModel.NewEmployee.CitizenshipID;
                        employeeEditted.EmployeeStatusID = selectedStatusID;
                        employeeEditted.RollOverSickDays = registerUserViewModel.NewEmployee.RollOverSickDays;
                        employeeEditted.RollOverVacationDays = registerUserViewModel.NewEmployee.RollOverVacationDays;
                        employeeEditted.BonusSickDays = registerUserViewModel.NewEmployee.BonusSickDays;
                        employeeEditted.BonusVacationDays = registerUserViewModel.NewEmployee.BonusVacationDays;
                        //employeeEditted.JobSubategoryTypeID = registerUserViewModel.NewEmployee.JobSubcategoryTypeID;

                        _context.Update(employeeEditted);


                        switch (selectedStatusID)
                        {
                            case 1: /*Salaried Employee*/
                                var salariedEmployee = _context.SalariedEmployees.Where(x => x.EmployeeId == employeeEditted.Id).FirstOrDefault();
                                if (salariedEmployee == null)
                                {
                                    salariedEmployee = new SalariedEmployee();
                                }
                                if (changedEmployeeStatus)
                                {
                                    await AddNewCentarixID(employeeEditted.Id, 1);
                                }
                                salariedEmployee.EmployeeId = employeeEditted.Id;
                                salariedEmployee.HoursPerDay = registerUserViewModel.NewEmployee.SalariedEmployee.HoursPerDay;
                                employeeEditted.SalariedEmployee = salariedEmployee;
                                break;
                            case 2: /*Freelancer*/
                                Freelancer freelancer = _context.Freelancers.Where(x => x.EmployeeId == employeeEditted.Id).FirstOrDefault();
                                if (freelancer == null)
                                {
                                    freelancer = new Freelancer();
                                }
                                if (changedEmployeeStatus)
                                {
                                    await AddNewCentarixID(employeeEditted.Id, 2);
                                }
                                freelancer.EmployeeId = employeeEditted.Id;
                                employeeEditted.Freelancer = freelancer;
                                break;
                            case 3: /*Advisor*/
                                Advisor advisor = _context.Advisors.Where(a => a.EmployeeID == employeeEditted.Id).FirstOrDefault();
                                if (advisor == null)
                                {
                                    advisor = new Advisor();
                                }
                                if (changedEmployeeStatus)
                                {
                                    await AddNewCentarixID(employeeEditted.Id, 3);
                                }
                                advisor.EmployeeID = employeeEditted.Id;
                                employeeEditted.Advisor = advisor;
                                break;
                        }
                        await _context.SaveChangesAsync();
                    }
                    //add new centarixID

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

                            if (!registerUserViewModel.NewEmployee.IsUser)
                            {
                                employeeEditted.IsUser = true;
                                _context.Update(employeeEditted);
                                await _context.SaveChangesAsync();

                                SendConfimationEmail(employeeEditted);
                            }
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


                    IList<string> rolesList = await _userManager.GetRolesAsync(employeeEditted).ConfigureAwait(false);

                    if (rolesList.Contains(AppUtility.MenuItems.Requests.ToString()) && !registerUserViewModel.OrderRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.Requests.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.Requests.ToString()) && registerUserViewModel.OrderRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.Requests.ToString());
                    }

                    if (rolesList.Contains(AppUtility.MenuItems.Protocols.ToString()) && !registerUserViewModel.ProtocolRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.Protocols.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.Protocols.ToString()) && registerUserViewModel.ProtocolRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.Protocols.ToString());
                    }

                    if (rolesList.Contains(AppUtility.MenuItems.Operations.ToString()) && !registerUserViewModel.OperationRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.Operations.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.Operations.ToString()) && registerUserViewModel.OperationRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.Operations.ToString());
                    }

                    if (rolesList.Contains(AppUtility.MenuItems.Biomarkers.ToString()) && !registerUserViewModel.BiomarkerRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.Biomarkers.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.Biomarkers.ToString()) && registerUserViewModel.BiomarkerRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.Biomarkers.ToString());
                    }

                    if (rolesList.Contains(AppUtility.MenuItems.TimeKeeper.ToString()) && !registerUserViewModel.TimekeeperRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.TimeKeeper.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.TimeKeeper.ToString()) && registerUserViewModel.TimekeeperRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.TimeKeeper.ToString());
                    }

                    if (rolesList.Contains(AppUtility.MenuItems.LabManagement.ToString()) && !registerUserViewModel.LabManagementRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.LabManagement.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.LabManagement.ToString()) && registerUserViewModel.LabManagementRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.LabManagement.ToString());
                    }

                    if (rolesList.Contains(AppUtility.MenuItems.Accounting.ToString()) && !registerUserViewModel.AccountingRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.Accounting.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.Accounting.ToString()) && registerUserViewModel.AccountingRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.Accounting.ToString());
                    }

                    if (rolesList.Contains(AppUtility.MenuItems.Reports.ToString()) && !registerUserViewModel.ExpenseesRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.Reports.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.Reports.ToString()) && registerUserViewModel.ExpenseesRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.Reports.ToString());
                    }

                    if (rolesList.Contains(AppUtility.MenuItems.Income.ToString()) && !registerUserViewModel.IncomeRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.Income.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.Income.ToString()) && registerUserViewModel.IncomeRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.Income.ToString());
                    }

                    if (rolesList.Contains(AppUtility.MenuItems.Users.ToString()) && !registerUserViewModel.UserRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.RemoveFromRoleAsync(employeeEditted, AppUtility.MenuItems.Users.ToString());
                    }
                    else if (!rolesList.Contains(AppUtility.MenuItems.Users.ToString()) && registerUserViewModel.UserRoles[0].Selected)
                    {
                        var rolesResult = await _userManager.AddToRoleAsync(employeeEditted, AppUtility.MenuItems.Users.ToString());
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
                                employeeEditted.UserImage = file.FullName;
                            }
                            _context.Update(employeeEditted);
                            await _context.SaveChangesAsync();
                        }

                        //should we move the delete here and test for the extension just in case it breaks over there

                    }
        
                    await transaction.CommitAsync();
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    Response.StatusCode = 500;
                    registerUserViewModel.ErrorMessage = ex.Message;
                    FillViewDropdowns(registerUserViewModel);
                    return PartialView("EditUser", registerUserViewModel);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Response.StatusCode = 500;
                    registerUserViewModel.ErrorMessage = ex.Message;
                    FillViewDropdowns(registerUserViewModel);
                    return PartialView("EditUser", registerUserViewModel);
                }
            }

            //return RedirectToAction("Index");
            return new EmptyResult();
        }
        private void FillViewDropdowns(RegisterUserViewModel registerUserViewModel)
        {
            registerUserViewModel.JobCategoryTypes = _context.JobCategoryTypes.Select(jc => jc).ToList();
            registerUserViewModel.EmployeeStatuses = _context.EmployeeStatuses.Select(es => es).ToList();
            registerUserViewModel.MaritalStatuses = _context.MaritalStatuses.Select(ms => ms).ToList();
            registerUserViewModel.Degrees = _context.Degrees.Select(d => d).ToList();
            registerUserViewModel.Citizenships = _context.Citizenships.Select(c => c).ToList();
            registerUserViewModel.NewEmployee = _context.Employees.Where(u => u.Id == registerUserViewModel.ApplicationUserID).Where(u => !u.IsSuspended)
                  .Include(s => s.SalariedEmployee).Include(e => e.JobSubcategoryType).FirstOrDefault();
            registerUserViewModel.EmployeeStatuses = _context.EmployeeStatuses.Select(es => es).ToList();
            registerUserViewModel.JobCategoryTypes = _context.JobCategoryTypes.Select(jt => jt).ToList();
            if(registerUserViewModel.NewEmployee !=null)
            {
                //get CentarixID
                registerUserViewModel.CentarixID = AppUtility.GetEmployeeCentarixID(_context.CentarixIDs.Where(ci => ci.EmployeeID == registerUserViewModel.ApplicationUserID).OrderBy(ci => ci.TimeStamp));

                if (registerUserViewModel.NewEmployee.JobSubcategoryTypeID != null)
                {
                    registerUserViewModel.JobSubcategoryTypes = _context.JobSubcategoryTypes.Where(js => js.JobCategoryTypeID == registerUserViewModel.NewEmployee.JobSubcategoryType.JobCategoryTypeID).ToList();
                }
                else
                {
                    registerUserViewModel.JobSubcategoryTypes = new List<JobSubcategoryType>();
                }
            }
          
        }
        public string GetProbableNextCentarixID(int StatusID)
        {
            var EmployeeStatus = _context.EmployeeStatuses.Where(es => es.EmployeeStatusID == StatusID).FirstOrDefault();
            var abbrev = EmployeeStatus.Abbreviation;
            if (abbrev[1] == ' ')
            {
                abbrev = abbrev.Substring(0, 1);
            }
            var newID = abbrev + (EmployeeStatus.LastCentarixID + 1).ToString();

            return newID;
        }

        public async Task<bool> AddNewCentarixID(string UserID, int StatusID)
        {
            var oldCentarixID = _context.CentarixIDs.Where(ci => ci.EmployeeID == UserID)
                .Where(ci => ci.IsCurrent).FirstOrDefault();
            oldCentarixID.IsCurrent = false;
            _context.Update(oldCentarixID);
            await _context.SaveChangesAsync();

            var lastStatus = _context.EmployeeStatuses.Where(es => es.EmployeeStatusID == StatusID).FirstOrDefault();
            var newNum = lastStatus.LastCentarixID + 1;
            var abbrev = lastStatus.Abbreviation;
            if (abbrev[1] == ' ')
            {
                abbrev = abbrev.Substring(0, 1);
            }
            var newID = abbrev + newNum.ToString();

            var newCentarixID = new CentarixID()
            {
                EmployeeID = UserID,
                CentarixIDNumber = newID,
                IsCurrent = true,
                TimeStamp = DateTime.Now,
                Employee = _context.Employees.Where(e => e.Id == UserID).FirstOrDefault()
            };
            _context.Add(newCentarixID);
            await _context.SaveChangesAsync();

            lastStatus.LastCentarixID = newNum;
            lastStatus.LastCentarixIDTimeStamp = DateTime.Now;
            _context.Update(lastStatus);
            await _context.SaveChangesAsync();

            return true; //just to allow asyncs
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> UserImageModal()
        {
            return PartialView();
        }

        //[HttpPost]
        //[Authorize(Roles = "Users")]
        //public async Task<IActionResult> SaveUserImage()
        //{
        //    return View(); //See what this should be doing...
        //}

        [HttpGet]
        [Authorize(Roles = "Users")]
        public IActionResult SuspendUserModal(string Id)
        {
            ApplicationUser user = _context.Users.Where(u => u.Id == Id).FirstOrDefault();
            return PartialView(user);
        }

        [HttpPost]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> SuspendUserModal(ApplicationUser applicationUser)
        {
            try
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
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index",new { ErrorMessage= ex.Message });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public IActionResult GetHomeView()
        {
            //Adina's code: should not go to IndexAdmin otherwise if not Admin will say denied Index will take them to IndexAdmin if they're an admin
            return RedirectToAction("Index", "Home");
            //Faige's original code below:
            //return View("~/Views/Home/IndexAdmin.cshtml");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TwoFactorSessionModal()
        {
            var user = await _signManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> TwoFactorSessionModal(bool rememberTwoFactor = true)
        {            
            var user = _signManager.GetTwoFactorAuthenticationUserAsync();
            var appUser = await _context.Employees.Where(e => e.Email == user.Result.Email).FirstOrDefaultAsync();
            if (rememberTwoFactor)
            {
                var cookieNum = 1;
                while (_httpContextAccessor.HttpContext.Request.Cookies["TwoFactorCookie" + cookieNum] != null)
                {
                    cookieNum++;
                }

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30)
                };
                _httpContextAccessor.HttpContext.Response.Cookies.Append("TwoFactorCookie" + cookieNum, appUser.Id, cookieOptions);
            }
            else
            {
                appUser.RememberTwoFactor = false;
                _context.Update(appUser);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IActionResult> editUserFunction(string id, int? Tab = 0)
        {
            Employee userSelected = _context.Employees.Where(u => u.Id == id).FirstOrDefault();
            if (userSelected != null)
            {
                RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel
                {
                    ApplicationUserID = userSelected.Id,
                    UserNum = userSelected.UserNum,
                    FirstName = userSelected.FirstName,
                    LastName = userSelected.LastName,
                    Email = userSelected.Email,
                    PhoneNumber = userSelected.PhoneNumber,
                    //CentarixID = userSelected.CentarixID,
                    UserImageSaved = userSelected.UserImage,
                    //TODO: do we want to show the secure app pass??
                    LabMonthlyLimit = userSelected.LabMonthlyLimit,
                    LabUnitLimit = userSelected.LabUnitLimit,
                    LabOrderLimit = userSelected.LabOrderLimit,
                    OperationMonthlyLimit = userSelected.OperationMonthlyLimit,
                    OperationUnitLimit = userSelected.OperationUnitLimit,
                    OperaitonOrderLimit = userSelected.OperationOrderLimit,
                    Tab = Tab ?? 1,
                    ConfirmedEmail = userSelected.EmailConfirmed
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
                FillViewDropdowns(registerUserViewModel);
                   
                    if (registerUserViewModel.NewEmployee == null)
                    {
                        registerUserViewModel.NewEmployee = new Employee();
                        registerUserViewModel.NewEmployee.EmployeeStatusID = 4;
                    }


                    //round job scope
                    string WorkScope = registerUserViewModel.NewEmployee?.SalariedEmployee?.WorkScope.ToString("0.00") ?? "0";
                    registerUserViewModel.NewEmployeeWorkScope = Decimal.Parse(WorkScope);

       


                //var userToShow = _context.Users.Where(u => u.Id == id).FirstOrDefault();
                IList<string> rolesList = await _userManager.GetRolesAsync(userSelected).ConfigureAwait(false);

                registerUserViewModel.OrderRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Requests, Name="General", Selected=false }
            };
                registerUserViewModel.ProtocolRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Protocols, Name="General", Selected=false }
            };
                registerUserViewModel.OperationRoles = new List<UserRoleViewModel>()
            {
                new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Operations, Name="General", Selected=false }
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
                    if (role == AppUtility.MenuItems.Requests.ToString()) //this was giving me an error in a switch case
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
                    else if (role == AppUtility.MenuItems.Operations.ToString()) //this was giving me an error in a switch case
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
            string password = GeneratePassword();

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

            while (!PasswordIsValid(string.Join(null, password)))
            {
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
            }


            return string.Join(null, password);
        }


        public string SaveTempUserImage(UserImageViewModel userImageViewModel)
        {
            string SavedUserImagePath = "";

            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
            Directory.CreateDirectory(uploadFolder);
            if (userImageViewModel.FileToSave != null) //test for more than one???
            {
                string FileName = "TempUserImage";

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

                int indexOfDot = userImageViewModel.FileToSave.FileName.IndexOf(".");
                string extension = userImageViewModel.FileToSave.FileName.Substring(indexOfDot, userImageViewModel.FileToSave.FileName.Length - indexOfDot);
                string uniqueFileName = FileName + extension;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (FileStream FileStream = new FileStream(filePath, FileMode.Create))
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

        public JsonResult GetJobSubcategoryTypeList(int JobCategoryTypeID)
        {
            var subcategories = _context.JobSubcategoryTypes.Where(js => js.JobCategoryTypeID == JobCategoryTypeID).ToList();
            return Json(subcategories);
        }

        public bool CheckUserEmailExist(bool isEdit, string email, string currentEmail)
        {
            return !_context.Users.Where(u => u.Email.ToLower().Equals(email.ToLower())).Any()||(isEdit && currentEmail.ToLower().Equals(email.ToLower()));
        }

    }
}