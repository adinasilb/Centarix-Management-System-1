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
using PrototypeWithAuth.Areas.Identity.Pages.Account;
using Microsoft.CodeAnalysis.CSharp;
using Org.BouncyCastle.Asn1.Cms;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace PrototypeWithAuth.Controllers
{
    public class AdminController : SharedController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly SignInManager<ApplicationUser> _signInManager;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, SignInManager<ApplicationUser> signInManager, UrlEncoder urlEncoder, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager, ICompositeViewEngine viewEngine)
            : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
            _roleManager = roleManager;
            _urlEncoder = urlEncoder;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            //CreateSingleRole();
        }


        [HttpGet]
        [Authorize(Roles = "Users")]
        public IActionResult Index(string ErrorMessage = null)
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
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            UserIndexViewModel userIndexViewModel = GetUserIndexViewModel();
            return PartialView(userIndexViewModel);
        }

        [Authorize(Roles = "Users")]
        private UserIndexViewModel GetUserIndexViewModel()
        {
            var users = _employeesProc.Read().OrderBy(u => u.UserNum)
                .Select(u => new UserWithCentarixIDViewModel
                {
                    Employee = u,
                    CentarixID = AppUtility.GetEmployeeCentarixID(_centarixIDsProc
                        .Read(new List<System.Linq.Expressions.Expression<Func<CentarixID, bool>>> { ci => ci.EmployeeID == u.Id }, null)
                        .OrderBy(ci => ci.TimeStamp))
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

            registerUserViewModel.OrderRoles = new List<UserRoleViewModel>();
            var counter = 0;
            foreach (var role in AppUtility.RequestRoleEnums())
            {
                registerUserViewModel.OrderRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, StringWithName = role, Selected = false });
                counter++;
            }
            registerUserViewModel.ProtocolRoles = new List<UserRoleViewModel>();
            registerUserViewModel.ProtocolRoles.Add(new UserRoleViewModel()
            {
                MenuItemsID = counter,
                StringWithName = new StringWithName()
                {
                    StringName = AppUtility.MenuItems.Protocols.ToString(),
                    StringDefinition = AppUtility.MenuItems.Protocols.ToString()
                },
                Selected = false
            }
            );
            counter++;
            registerUserViewModel.OperationRoles = new List<UserRoleViewModel>();
            foreach (var role in AppUtility.OperationRoleEnums())
            {
                registerUserViewModel.OperationRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, StringWithName = role, Selected = false });
                counter++;
            }
            registerUserViewModel.BiomarkerRoles = new List<UserRoleViewModel>();
            registerUserViewModel.BiomarkerRoles.Add(new UserRoleViewModel()
            {
                MenuItemsID = counter,
                StringWithName = new StringWithName()
                {
                    StringName = AppUtility.MenuItems.Biomarkers.ToString(),
                    StringDefinition = AppUtility.MenuItems.Biomarkers.ToString()
                },
                Selected = false
            }
            );
            counter++;
            registerUserViewModel.TimekeeperRoles = new List<UserRoleViewModel>();
            registerUserViewModel.TimekeeperRoles.Add(new UserRoleViewModel()
            {
                MenuItemsID = counter,
                StringWithName = new StringWithName()
                {
                    StringName = AppUtility.MenuItems.TimeKeeper.ToString(),
                    StringDefinition = AppUtility.MenuItems.TimeKeeper.ToString()
                },
                Selected = false
            }
            );
            counter++;
            registerUserViewModel.LabManagementRoles = new List<UserRoleViewModel>();
            registerUserViewModel.LabManagementRoles.Add(new UserRoleViewModel()
            {
                MenuItemsID = counter,
                StringWithName = new StringWithName()
                {
                    StringName = AppUtility.MenuItems.LabManagement.ToString(),
                    StringDefinition = AppUtility.MenuItems.LabManagement.ToString()
                },
                Selected = false
            }
            );
            counter++;
            registerUserViewModel.AccountingRoles = new List<UserRoleViewModel>();
            registerUserViewModel.AccountingRoles.Add(new UserRoleViewModel()
            {
                MenuItemsID = counter,
                StringWithName = new StringWithName()
                {
                    StringName = AppUtility.MenuItems.Accounting.ToString(),
                    StringDefinition = AppUtility.MenuItems.Accounting.ToString()
                },
                Selected = false
            }
            );
            counter++;
            registerUserViewModel.ExpenseesRoles = new List<UserRoleViewModel>();
            registerUserViewModel.ExpenseesRoles.Add(new UserRoleViewModel()
            {
                MenuItemsID = counter,
                StringWithName = new StringWithName()
                {
                    StringName = AppUtility.MenuItems.Reports.ToString(),
                    StringDefinition = AppUtility.MenuItems.Reports.ToString()
                },
                Selected = false
            }
            );
            counter++;
            registerUserViewModel.IncomeRoles = new List<UserRoleViewModel>();
            registerUserViewModel.IncomeRoles.Add(new UserRoleViewModel()
            {
                MenuItemsID = counter,
                StringWithName = new StringWithName()
                {
                    StringName = AppUtility.MenuItems.Income.ToString(),
                    StringDefinition = AppUtility.MenuItems.Income.ToString()
                },
                Selected = false
            }
            );
            counter++;
            registerUserViewModel.UserRoles = new List<UserRoleViewModel>();
            registerUserViewModel.UserRoles.Add(new UserRoleViewModel()
            {
                MenuItemsID = counter,
                StringWithName = new StringWithName()
                {
                    StringName = AppUtility.MenuItems.Users.ToString(),
                    StringDefinition = AppUtility.MenuItems.Users.ToString()
                },
                Selected = false
            }
            );
            //registerUserViewModel.OrderRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Requests, Name="General", Selected=false }
            //};
            //registerUserViewModel.ProtocolRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Protocols, Name="General", Selected=false }
            //};
            //registerUserViewModel.OperationRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Operations, Name="General", Selected=false }
            //};
            //registerUserViewModel.BiomarkerRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Biomarkers, Name="General", Selected=false }
            //};
            //registerUserViewModel.TimekeeperRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.TimeKeeper, Name="General", Selected=false }
            //};
            //registerUserViewModel.LabManagementRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.LabManagement, Name="General", Selected=false }
            //};
            //registerUserViewModel.AccountingRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Accounting, Name="General", Selected=false }
            //};
            //registerUserViewModel.ExpenseesRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Reports, Name="General", Selected=false }
            //};
            //registerUserViewModel.IncomeRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Income, Name="General", Selected=false }
            //};
            //registerUserViewModel.UserRoles = new List<UserRoleViewModel>()
            //{
            //    new UserRoleViewModel(){ MenuItemsID= AppUtility.MenuItems.Users, Name="General", Selected=false }
            //};

            return View(registerUserViewModel);
        }



        [HttpPost]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel registerUserViewModel)
        {
            var errorMessage = "";
            var success = await _employeesProc.CreateUser(registerUserViewModel, _hostingEnvironment, Url, Request);
            if (!success.Bool)
            {
                registerUserViewModel.ErrorMessage = success.String;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersUser;
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
                registerUserViewModel.JobCategoryTypes = _jobCategoryTypesProc.Read().ToList();
                registerUserViewModel.EmployeeStatuses = _employeeStatusesProc.Read().ToList();
                registerUserViewModel.MaritalStatuses = _maritalStatusesProc.Read().ToList();
                registerUserViewModel.Degrees = _degreesProc.Read().ToList();
                registerUserViewModel.Citizenships = _citizenshipsProc.Read().ToList();
                return View("CreateUser", registerUserViewModel);
            }
            if (success.String != null)
            {
                errorMessage = success.String;
            }
            return RedirectToAction("Index", new { errorMessage });
        }

        [Authorize(Roles = "Users")]
        private async void SendConfimationEmail(ApplicationUser user)
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
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return await editUserFunction(id);
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
                        //employeeEditted.BonusSickDays = registerUserViewModel.NewEmployee.BonusSickDays;
                        //employeeEditted.BonusVacationDays = registerUserViewModel.NewEmployee.BonusVacationDays;
                        employeeEditted.SpecialDays = registerUserViewModel.NewEmployee.SpecialDays;
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


                    List<string> rolesList = new List<string>(await _userManager.GetRolesAsync(employeeEditted).ConfigureAwait(false));

                    foreach (var role in registerUserViewModel.OrderRoles)
                    {
                        await CheckRoleAsync(rolesList, employeeEditted, role.StringWithName.StringDefinition, role.Selected);
                    }
                    foreach (var role in registerUserViewModel.OperationRoles)
                    {
                        await CheckRoleAsync(rolesList, employeeEditted, role.StringWithName.StringDefinition, role.Selected);
                    }
                    foreach (var role in registerUserViewModel.ProtocolRoles)
                    {
                        await CheckRoleAsync(rolesList, employeeEditted, role.StringWithName.StringDefinition, role.Selected);
                    }
                    await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Biomarkers.ToString(), registerUserViewModel.BiomarkerRoles[0].Selected);
                    await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.TimeKeeper.ToString(), registerUserViewModel.TimekeeperRoles[0].Selected);
                    await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.LabManagement.ToString(), registerUserViewModel.LabManagementRoles[0].Selected);
                    await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Accounting.ToString(), registerUserViewModel.AccountingRoles[0].Selected);
                    await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Reports.ToString(), registerUserViewModel.ExpenseesRoles[0].Selected);
                    await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Income.ToString(), registerUserViewModel.IncomeRoles[0].Selected);
                    await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Users.ToString(), registerUserViewModel.UserRoles[0].Selected);

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
                    //throw new Exception();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    Response.StatusCode = 500;
                    registerUserViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    FillViewDropdowns(registerUserViewModel);
                    return PartialView("EditUser", registerUserViewModel);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Response.StatusCode = 500;
                    registerUserViewModel.ErrorMessage = AppUtility.GetExceptionMessage(ex);
                    FillViewDropdowns(registerUserViewModel);
                    return PartialView("EditUser", registerUserViewModel);
                }
            }

            //return RedirectToAction("Index");
            return new EmptyResult();
        }

        [Authorize(Roles = "Users")]
        public async Task CheckRoleAsync(IList<string> roleslist, Employee employee, string roleName, bool selected)
        {
            if (!roleslist.Contains(roleName) && selected)
            {
                var rolesResult = await _userManager.AddToRoleAsync(employee, roleName);
            }
            else if ((roleslist.Contains(roleName)) && !selected)
            {
                var rolesResult = await _userManager.RemoveFromRoleAsync(employee, roleName);
            }

        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> EditUserPartial(string id, int? Tab)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            return await editUserFunction(id, Tab);
        }
        private void FillViewDropdowns(RegisterUserViewModel registerUserViewModel)
        {
            registerUserViewModel.JobCategoryTypes = _jobCategoryTypesProc.Read().ToList();
            registerUserViewModel.EmployeeStatuses = _employeeStatusesProc.Read().ToList();
            registerUserViewModel.MaritalStatuses = _maritalStatusesProc.Read().ToList();
            registerUserViewModel.Degrees = _degreesProc.Read().ToList();
            registerUserViewModel.Citizenships = _citizenshipsProc.Read().ToList();
            registerUserViewModel.NewEmployee = _employeesProc.Read(new List<System.Linq.Expressions.Expression<Func<Employee, bool>>>
                { u => u.Id == registerUserViewModel.ApplicationUserID && !u.IsSuspended }, new List<ComplexIncludes<Employee, ModelBase>>
                {
                    new ComplexIncludes<Employee, ModelBase>{Include = s => s.SalariedEmployee},
                    new ComplexIncludes<Employee, ModelBase>{Include = e => e.JobSubcategoryType}
                }).FirstOrDefault(); 
            if (registerUserViewModel.NewEmployee != null)
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
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
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
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
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
                return RedirectToAction("Index", new { ErrorMessage = AppUtility.GetExceptionMessage(ex) });
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
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TwoFactorSessionModal(bool rememberTwoFactor = true)
        {
            try
            {
                var user = _signInManager.GetTwoFactorAuthenticationUserAsync();
                var appUser = await _employeesProc.Read(new List<System.Linq.Expressions.Expression<Func<Employee, bool>>>
                    { e => e.Email == user.Result.Email }).FirstOrDefaultAsync();
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
                    var success = _employeesProc.UpdateAsync(appUser);
                }
            }
            catch (Exception ex)
            {
                return View("DefaultView");
            }

            return PartialView();
        }

        public async Task<IActionResult> editUserFunction(string id, int? Tab = 0)
        {
            Employee userSelected = _employeesProc.Read(new List<System.Linq.Expressions.Expression<Func<Employee, bool>>>
                { u => u.Id == id }).FirstOrDefault();
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




                IList<string> rolesList = await _userManager.GetRolesAsync(userSelected).ConfigureAwait(false);

                var counter = 0;
                registerUserViewModel.OrderRoles = new List<UserRoleViewModel>();
                var nextselected = false;
                foreach (var role in AppUtility.RequestRoleEnums())
                {
                    nextselected = rolesList.Contains(role.StringDefinition) ? true : false;
                    registerUserViewModel.OrderRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, StringWithName = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.ProtocolRoles = new List<UserRoleViewModel>();
                nextselected = rolesList.Contains(AppUtility.MenuItems.Protocols.ToString()) ? true : false;
                foreach (var role in AppUtility.ProtocolRoleEnums())
                {
                    nextselected = rolesList.Contains(role.StringDefinition) ? true : false;
                    registerUserViewModel.ProtocolRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, StringWithName = role, Selected = nextselected });
                    counter++;
                }
                counter++;
                registerUserViewModel.OperationRoles = new List<UserRoleViewModel>();
                foreach (var role in AppUtility.OperationRoleEnums())
                {
                    nextselected = rolesList.Contains(role.StringDefinition) ? true : false;
                    registerUserViewModel.OperationRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, StringWithName = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.BiomarkerRoles = new List<UserRoleViewModel>();
                nextselected = rolesList.Contains(AppUtility.MenuItems.Biomarkers.ToString()) ? true : false;
                registerUserViewModel.BiomarkerRoles.Add(new UserRoleViewModel()
                {
                    MenuItemsID = counter,
                    StringWithName = new StringWithName()
                    {
                        StringName = AppUtility.MenuItems.Biomarkers.ToString(),
                        StringDefinition = AppUtility.MenuItems.Biomarkers.ToString()
                    },
                    Selected = nextselected
                }
                );
                counter++;
                registerUserViewModel.TimekeeperRoles = new List<UserRoleViewModel>();
                nextselected = rolesList.Contains(AppUtility.MenuItems.TimeKeeper.ToString()) ? true : false;
                registerUserViewModel.TimekeeperRoles.Add(new UserRoleViewModel()
                {
                    MenuItemsID = counter,
                    StringWithName = new StringWithName()
                    {
                        StringName = AppUtility.MenuItems.TimeKeeper.ToString(),
                        StringDefinition = AppUtility.MenuItems.TimeKeeper.ToString()
                    },
                    Selected = nextselected
                }
                );
                counter++;
                registerUserViewModel.LabManagementRoles = new List<UserRoleViewModel>();
                nextselected = rolesList.Contains(AppUtility.MenuItems.LabManagement.ToString()) ? true : false;
                registerUserViewModel.LabManagementRoles.Add(new UserRoleViewModel()
                {
                    MenuItemsID = counter,
                    StringWithName = new StringWithName()
                    {
                        StringName = AppUtility.MenuItems.LabManagement.ToString(),
                        StringDefinition = AppUtility.MenuItems.LabManagement.ToString()
                    },
                    Selected = nextselected
                }
                );
                counter++;
                registerUserViewModel.AccountingRoles = new List<UserRoleViewModel>();
                nextselected = rolesList.Contains(AppUtility.MenuItems.Accounting.ToString()) ? true : false;
                registerUserViewModel.AccountingRoles.Add(new UserRoleViewModel()
                {
                    MenuItemsID = counter,
                    StringWithName = new StringWithName()
                    {
                        StringName = AppUtility.MenuItems.Accounting.ToString(),
                        StringDefinition = AppUtility.MenuItems.Accounting.ToString()
                    },
                    Selected = nextselected
                }
                );
                counter++;
                registerUserViewModel.ExpenseesRoles = new List<UserRoleViewModel>();
                nextselected = rolesList.Contains(AppUtility.MenuItems.Reports.ToString()) ? true : false;
                registerUserViewModel.ExpenseesRoles.Add(new UserRoleViewModel()
                {
                    MenuItemsID = counter,
                    StringWithName = new StringWithName()
                    {
                        StringName = AppUtility.MenuItems.Reports.ToString(),
                        StringDefinition = AppUtility.MenuItems.Reports.ToString()
                    },
                    Selected = nextselected
                }
                );
                counter++;
                registerUserViewModel.IncomeRoles = new List<UserRoleViewModel>();
                nextselected = rolesList.Contains(AppUtility.MenuItems.Income.ToString()) ? true : false;
                registerUserViewModel.IncomeRoles.Add(new UserRoleViewModel()
                {
                    MenuItemsID = counter,
                    StringWithName = new StringWithName()
                    {
                        StringName = AppUtility.MenuItems.Income.ToString(),
                        StringDefinition = AppUtility.MenuItems.Income.ToString()
                    },
                    Selected = nextselected
                }
                );
                counter++;
                registerUserViewModel.UserRoles = new List<UserRoleViewModel>();
                nextselected = rolesList.Contains(AppUtility.MenuItems.Users.ToString()) ? true : false;
                registerUserViewModel.UserRoles.Add(new UserRoleViewModel()
                {
                    MenuItemsID = counter,
                    StringWithName = new StringWithName()
                    {
                        StringName = AppUtility.MenuItems.Users.ToString(),
                        StringDefinition = AppUtility.MenuItems.Users.ToString()
                    },
                    Selected = nextselected
                }
                );



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
            string password = _employeesProc.GeneratePassword();

            return Json(password);
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
                        //throw new Exception();
                    }
                    catch (Exception e)
                    {
                        Response.StatusCode = 500;
                        //Response.WriteAsync(AppUtility.GetExceptionMessage(e)); both do same thing - which is better?
                        return AppUtility.GetExceptionMessage(e);
                    }
                }
            }

            return SavedUserImagePath;

        }





        public JsonResult GetJobSubcategoryTypeList(int JobCategoryTypeID)
        {
            var subcategories = _jobCategoryTypesProc.Read(new List<System.Linq.Expressions.Expression<Func<JobCategoryType, bool>>>
                { js => js.JobCategoryTypeID == JobCategoryTypeID }).ToList();
            return Json(subcategories);
        }

        public bool CheckUserEmailExist(bool isEdit, string email, string currentEmail)
        {
            return !(_employeesProc.Read(new List<System.Linq.Expressions.Expression<Func<Employee, bool>>> { u => u.Email.ToLower().Equals(email.ToLower()) })).Any()
                || (isEdit && currentEmail.ToLower().Equals(email.ToLower()));
        }

    }
}