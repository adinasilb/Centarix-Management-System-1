using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System.Text.Encodings.Web;
using System.Linq.Dynamic.Core;
using PrototypeWithAuth.AppData.UtilityModels;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Linq.Expressions;

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
            var listCentarixIDs = _centarixIDsProc.Read();
            List<UserWithCentarixIDViewModel> users = _employeesProc.Read().OrderBy(u => u.UserNum)
                .Select(u => new UserWithCentarixIDViewModel
                {
                    Employee = u,
                    CentarixID = AppUtility.GetEmployeeCentarixID(
                        listCentarixIDs.Where(ci => ci.EmployeeID == u.Id)
                        .OrderBy(ci => ci.TimeStamp))
                }).ToList();
            var users2 = System.Linq.Enumerable.ToList(users);
            UserIndexViewModel userIndexViewModel = new UserIndexViewModel()
            {
                ApplicationUsers = users2,
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

            RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel() { SidebarEnum = AppUtility.SidebarEnum.Add };

            registerUserViewModel.Employee = new Employee()
            {
                StartedWorking = DateTime.Today
            };
            FillViewDropdowns(registerUserViewModel);

            registerUserViewModel.OrderRoles = new List<UserRoleViewModel>();
            var counter = 0;
            foreach (var role in AppUtility.RequestRoleEnums())
            {
                registerUserViewModel.OrderRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }
            registerUserViewModel.ProtocolRoles = new List<UserRoleViewModel>();
            counter = 0;
            foreach (var role in AppUtility.ProtocolRoleEnums())
            {
                registerUserViewModel.ProtocolRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }
            registerUserViewModel.OperationRoles = new List<UserRoleViewModel>();
            counter = 0;
            foreach (var role in AppUtility.OperationRoleEnums())
            {
                registerUserViewModel.OperationRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }
            registerUserViewModel.BiomarkerRoles = new List<UserRoleViewModel>();
            foreach (var role in AppUtility.BiomarkerRoleEnums())
            {
                registerUserViewModel.BiomarkerRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }
            counter = 0;
            registerUserViewModel.TimekeeperRoles = new List<UserRoleViewModel>();
            foreach (var role in AppUtility.TimekeeperRoleEnums())
            {
                registerUserViewModel.TimekeeperRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }
            registerUserViewModel.LabManagementRoles = new List<UserRoleViewModel>();
            counter = 0;
            foreach (var role in AppUtility.LabManagementRoleEnums())
            {
                registerUserViewModel.LabManagementRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }
            registerUserViewModel.AccountingRoles = new List<UserRoleViewModel>();
            counter = 0;
            foreach (var role in AppUtility.AccountingRoleEnums())
            {
                registerUserViewModel.AccountingRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }
            registerUserViewModel.ExpenseesRoles = new List<UserRoleViewModel>();
            counter = 0;
            foreach (var role in AppUtility.ReportsRoleEnums())
            {
                registerUserViewModel.ExpenseesRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }
            registerUserViewModel.IncomeRoles = new List<UserRoleViewModel>();
            counter = 0;
            foreach (var role in AppUtility.IncomeRoleEnums())
            {
                registerUserViewModel.IncomeRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }
            registerUserViewModel.UserRoles = new List<UserRoleViewModel>();
            counter = 0;
            foreach (var role in AppUtility.UsersRoleEnums())
            {
                registerUserViewModel.UserRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = false });
                counter++;
            }

            return View(registerUserViewModel);
        }



        [HttpPost]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel registerUserViewModel)
        {
            var success = await _employeesProc.CreateUser(registerUserViewModel, _hostingEnvironment, Url, Request, _userManager);
            if (!success.Bool)
            {
                FillViewDropdowns(registerUserViewModel);
                registerUserViewModel.ErrorMessage = success.String;
                TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersUser;
                TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
                TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Add;
                registerUserViewModel.Employee.IsUser = false;
                return View("CreateUser", registerUserViewModel);
            }
            return RedirectToAction("Index", new { ErrorMessage = success.String });
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

            var success = await _employeesProc.UpdateUser(registerUserViewModel, _hostingEnvironment, Url, Request, _userManager);
            if (success.Bool)
            {
                return Content(success.String);
            }
            else
            {
                Response.StatusCode = 500;
                registerUserViewModel.ErrorMessage = success.String;
                GetEmployeeFields(registerUserViewModel);
                FillViewDropdowns(registerUserViewModel);
                return PartialView("EditUser", registerUserViewModel);
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

        private void GetEmployeeFields(RegisterUserViewModel registerUserViewModel)
        {
            registerUserViewModel.Employee = _employeesProc.Read(new List<System.Linq.Expressions.Expression<Func<Employee, bool>>>
                { u => u.Id == registerUserViewModel.Employee.Id && !u.IsSuspended }, new List<ComplexIncludes<Employee, ModelBase>>
                {
                    new ComplexIncludes<Employee, ModelBase>{Include = s => s.SalariedEmployee},
                    new ComplexIncludes<Employee, ModelBase>{Include = e => e.JobSubcategoryType}
                }).FirstOrDefault();
        }
        private void FillViewDropdowns(RegisterUserViewModel registerUserViewModel)
        {
            registerUserViewModel.JobCategoryTypes = _jobCategoryTypesProc.Read().ToList();
            registerUserViewModel.EmployeeStatuses = _employeeStatusesProc.Read().ToList();
            registerUserViewModel.MaritalStatuses = _maritalStatusesProc.Read().ToList();
            registerUserViewModel.Degrees = _degreesProc.Read().ToList();
            registerUserViewModel.Citizenships = _citizenshipsProc.Read().ToList();

            if (registerUserViewModel.Employee is Employee)
            {
                //get CentarixID
                registerUserViewModel.CentarixID = AppUtility.GetEmployeeCentarixID(_centarixIDsProc.Read(new List<Expression<Func<CentarixID, bool>>>
                    { ci => ci.EmployeeID == registerUserViewModel.Employee.Id }).OrderBy(ci => ci.TimeStamp));

                if (registerUserViewModel.Employee.JobSubcategoryTypeID != null)
                {
                    registerUserViewModel.JobSubcategoryTypes =
                        _jobSubcategoryTypesProc.Read(new List<Expression<Func<JobSubcategoryType, bool>>> {
                        js => js.JobCategoryTypeID == registerUserViewModel.Employee.JobSubcategoryType.JobCategoryTypeID }).ToList();
                }
                else
                {
                    registerUserViewModel.JobSubcategoryTypes = new List<JobSubcategoryType>();
                }
            }
        }
        public string GetProbableNextCentarixID(int StatusID)
        {
            var EmployeeStatus = _employeeStatusesProc.Read(new List<Expression<Func<EmployeeStatus, bool>>> { es => es.EmployeeStatusID == StatusID }).FirstOrDefault();
            var abbrev = EmployeeStatus.Abbreviation;
            if (abbrev[1] == ' ')
            {
                abbrev = abbrev.Substring(0, 1);
            }
            var newID = abbrev + (EmployeeStatus.LastCentarixID + 1).ToString();

            return newID;
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
            ApplicationUser user = _employeesProc.ReadOne(new List<Expression<Func<Employee, bool>>> { u => u.Id == Id }).FirstOrDefault();
            return PartialView(user);
        }

        [HttpPost]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> SuspendUserModal(ApplicationUser applicationUser)
        {
            applicationUser = _employeesProc.Read(new List<Expression<Func<Employee, bool>>> { u => u.Id == applicationUser.Id }).FirstOrDefault();
            var success = await _employeesProc.SuspendUser(applicationUser);
            return RedirectToAction("Index", new { ErrorMessage = success.String });
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public IActionResult GetHomeView()
        {
            return RedirectToAction("Index", "Home");
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

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> TwoFactorSessionModal(bool rememberTwoFactor = true)
        //{
        //    try
        //    {
        //        var user = _signInManager.GetTwoFactorAuthenticationUserAsync();
        //        var appUser = await _employeesProc.Read(new List<System.Linq.Expressions.Expression<Func<Employee, bool>>>
        //            { e => e.Email == user.Result.Email }).FirstOrDefaultAsync();
        //        if (rememberTwoFactor)
        //        {
        //            var cookieNum = 1;
        //            while (_httpContextAccessor.HttpContext.Request.Cookies["TwoFactorCookie" + cookieNum] != null)
        //            {
        //                cookieNum++;
        //            }

        //            var cookieOptions = new CookieOptions
        //            {
        //                Expires = DateTime.Now.AddDays(30)
        //            };
        //            _httpContextAccessor.HttpContext.Response.Cookies.Append("TwoFactorCookie" + cookieNum, appUser.Id, cookieOptions);
        //        }
        //        else
        //        {
        //            var success = _employeesProc.UpdateAsync(appUser);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("DefaultView");
        //    }

        //    return PartialView();
        //}


        public async Task<IActionResult> editUserFunction(string id, int? Tab = 0)
        {
            Employee userSelected = _employeesProc.Read(new List<System.Linq.Expressions.Expression<Func<Employee, bool>>>
                { u => u.Id == id }).FirstOrDefault();
            if (userSelected != null)
            {
                RegisterUserViewModel registerUserViewModel = new RegisterUserViewModel
                {
                    Employee = userSelected,
                    //CentarixID = userSelected.CentarixID,
                    UserImageSaved = userSelected.UserImage,
                    //TODO: do we want to show the secure app pass??
                    Tab = Tab ?? 1,
                    ConfirmedEmail = userSelected.EmailConfirmed,
                    SidebarEnum = AppUtility.SidebarEnum.List
                };


                string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                DirectoryInfo dir1 = new DirectoryInfo(uploadFolder1);
                FileInfo[] files1 = dir1.GetFiles(registerUserViewModel.Employee.UserNum + ".*");
                if (files1.Length > 0)
                {
                    foreach (FileInfo file in files1)
                    {
                        registerUserViewModel.UserImage = file.FullName;
                    }
                }
                GetEmployeeFields(registerUserViewModel);
                FillViewDropdowns(registerUserViewModel);

                //if (registerUserViewModel.NewEmployee == null)
                //{
                //    registerUserViewModel.NewEmployee = new Employee();
                //    registerUserViewModel.NewEmployee.EmployeeStatusID = 4;
                //}


                //round job scope
                string WorkScope = registerUserViewModel.Employee?.SalariedEmployee?.WorkScope.ToString("0.00") ?? "0";
                registerUserViewModel.EmployeeWorkScope = Decimal.Parse(WorkScope);




                IList<string> rolesList = await _userManager.GetRolesAsync(userSelected).ConfigureAwait(false);

                var counter = 0;
                registerUserViewModel.OrderRoles = new List<UserRoleViewModel>();
                var nextselected = false;
                foreach (var role in AppUtility.RequestRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.OrderRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.ProtocolRoles = new List<UserRoleViewModel>();
                foreach (var role in AppUtility.ProtocolRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.ProtocolRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.OperationRoles = new List<UserRoleViewModel>();
                foreach (var role in AppUtility.OperationRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.OperationRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.BiomarkerRoles = new List<UserRoleViewModel>();
                foreach(var role in AppUtility.BiomarkerRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.BiomarkerRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.TimekeeperRoles = new List<UserRoleViewModel>();
                foreach (var role in AppUtility.TimekeeperRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.TimekeeperRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.LabManagementRoles = new List<UserRoleViewModel>();
                foreach (var role in AppUtility.LabManagementRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.LabManagementRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.AccountingRoles = new List<UserRoleViewModel>();
                foreach (var role in AppUtility.AccountingRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.AccountingRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.ExpenseesRoles = new List<UserRoleViewModel>();
                foreach (var role in AppUtility.ReportsRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.ExpenseesRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.IncomeRoles = new List<UserRoleViewModel>();
                foreach (var role in AppUtility.IncomeRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.IncomeRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }
                registerUserViewModel.UserRoles = new List<UserRoleViewModel>();
                foreach (var role in AppUtility.UsersRoleEnums())
                {
                    nextselected = rolesList.Contains(role.RoleDefinition) ? true : false;
                    registerUserViewModel.UserRoles.Add(new UserRoleViewModel() { MenuItemsID = counter, Role = role, Selected = nextselected });
                    counter++;
                }



                if (registerUserViewModel.Employee.UserImage == null)
                {
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                    string filePath = Path.Combine(uploadFolder, "profile_circle_big.png");
                    registerUserViewModel.Employee.UserImage = filePath;
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
                return PartialView("InvalidLinkPageRightModal");
            }
        }

        public JsonResult GetGeneratedPassword()
        {
            string password = _employeesProc.GeneratePassword();

            return Json(password);
        }


        public string SaveTempUserImage(UserImageViewModel userImageViewModel)
        {
            // 1. application/pdf 2. application/msword 3. image/jpeg 4. image/png
            string SavedUserImagePath = "";

            var supportedContentTypes = new List<String> { "image/jpeg", "image/png" };
            if (supportedContentTypes.Contains(userImageViewModel.FileToSave.ContentType))
            {
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
            }

            return SavedUserImagePath;

        }





        public JsonResult GetJobSubcategoryTypeList(int JobCategoryTypeID)
        {
            var subcategories = _jobSubcategoryTypesProc.Read(new List<Expression<Func<JobSubcategoryType, bool>>> { js => js.JobCategoryTypeID == JobCategoryTypeID }).ToList();
            return Json(subcategories);
        }

        public bool CheckUserEmailExist(bool isEdit, string email, string currentEmail)
        {
            var emailcheck1 = email == null ? true : !(_employeesProc.Read(new List<Expression<Func<Employee, bool>>> { u => u.Email.ToLower() == email.ToLower() }).Any());
            var emailcheck2 = email == null ? true : (isEdit && currentEmail.ToLower().Equals(email.ToLower()));
            var check = emailcheck1 || emailcheck2;
            return check;
        }

    }
}