using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Diagnostics;
using RestSharp;
using Microsoft.AspNetCore.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
       //private readonly UserManager<TUser> _identityUserManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor
          )
        {
            _userManager = userManager;
           // _identityUserManager = identityuserManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
        public bool TwoFactorSessionExpired { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }

            [Display(Name = "Remember Two Factor Authentication")]
            public bool RememberTwoFactor { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null, string errorMessage=null, string successMessage = null)
        {
            ErrorMessage = errorMessage;
            SuccessMessage = successMessage;
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var user = await _userManager.FindByEmailAsync(Input.Email);
                //var user = await _userManager.FindByEmailAsync(Input.Email);
                // var passwordValidator = new PasswordValidator<ApplicationUser>();
                //var identityUserManager = new UserManager<IdentityUser>(user);
                var validPassword = await _signInManager.UserManager.CheckPasswordAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    if (user.NeedsToResetPassword)
                    {
                        //await _signInManager.SignOutAsync();
                        returnUrl = Url.Action("ResetPassword", "Home");
                    }
                    //else
                    //{
                    //    //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl });

                    //    return LocalRedirect(returnUrl);
                    //    //returnUrl = Url.Action("Index", "Home");
                    //}

                    //Redirect to the 2FA page!!!
                    return LocalRedirect(returnUrl);
                    //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl });


                }
                else if (result.RequiresTwoFactor)
                {
                    var myIpAddress = "";
                    IPAddress ipAddress;
                    var ipAddressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                    foreach (var address in ipAddressList)
                    {
                        if (myIpAddress == "")
                        {
                            if (IPAddress.TryParse(address.ToString(), out ipAddress))
                            {
                                switch (ipAddress.AddressFamily)
                                {
                                    case AddressFamily.InterNetwork:
                                        myIpAddress = address.ToString();
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    var ipEnd = myIpAddress.Split(".").LastOrDefault();
                    var ipEndNum = 501;
                    Int32.TryParse(ipEnd, out ipEndNum);

                    if (user.RememberTwoFactor == true)
                    {
                        string cookie = null;
                        var isCookie = true;
                        var cookieNum = 1;
                        var cookieName = "";
                        while (isCookie)
                        {
                            cookieName = "TwoFactorCookie" + cookieNum;
                            if (_httpContextAccessor.HttpContext.Request.Cookies[cookieName] != null)
                            {
                                if (_httpContextAccessor.HttpContext.Request.Cookies[cookieName] == user.Id)
                                {
                                    isCookie = false;
                                    cookie = _httpContextAccessor.HttpContext.Request.Cookies[cookieName];
                                }
                            }
                            else
                            {
                                isCookie = false;
                            }
                            cookieNum++;
                        }

                        if (cookie != null)
                        {
                            string physicalAddress = NetworkInterface
                                .GetAllNetworkInterfaces()
                                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                .Select(nic => nic.GetPhysicalAddress().ToString())
                                .FirstOrDefault();
                            var pa = _context.PhysicalAddresses.Where(p => p.Address == physicalAddress && p.EmployeeID == user.Id).FirstOrDefault();
                            if (pa != null)
                            {
                                if (myIpAddress.StartsWith("172.27.71.") && (ipEndNum >= 0 && ipEndNum <= 500))
                                {
                                    user.TwoFactorEnabled = false;
                                    _context.Update(user);
                                    _context.SaveChanges();
                                    result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                                    user.TwoFactorEnabled = true;
                                    _context.Update(user);
                                    _context.SaveChanges();
                                    if (result.Succeeded)
                                    {
                                        return LocalRedirect(returnUrl);
                                    }
                                    else
                                    {
                                        ModelState.AddModelError(string.Empty, "Something went wrong in login. Please try again.");
                                        return Page();
                                    }
                                }
                            }
                        }
                        else
                        {
                            TwoFactorSessionExpired = true;
                        }
                    }
                    else if (Input.RememberTwoFactor)
                    {
                        if (myIpAddress.StartsWith("172.27.71.") && (ipEndNum >= 0 && ipEndNum <= 500))
                        {
                            user.RememberTwoFactor = true;
                            _context.Update(user);
                            var cookieNum = 1;
                            while (_httpContextAccessor.HttpContext.Request.Cookies["TwoFactorCookie" + cookieNum] != null)
                            {
                                cookieNum++;
                            }

                            var cookieOptions = new CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(30)
                            };
                            _httpContextAccessor.HttpContext.Response.Cookies.Append("TwoFactorCookie"+cookieNum, user.Id, cookieOptions);

                            string address = NetworkInterface
                                    .GetAllNetworkInterfaces()
                                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                    .Select(nic => nic.GetPhysicalAddress().ToString())
                                    .FirstOrDefault();
                            var pa = _context.PhysicalAddresses.Where(p => p.Address == address && p.EmployeeID == user.Id).FirstOrDefault();
                            if (pa == null)
                            {
                                var physicalAddress = new Models.PhysicalAddress()
                                {
                                    Employee = _context.Employees.Where(e => e.Id == user.Id).FirstOrDefault(),
                                    EmployeeID = user.Id,
                                    Address = address,
                                    DateCreated = DateTime.Now
                                };
                                _context.PhysicalAddresses.Add(physicalAddress);
                            }
                            _context.SaveChanges();
                        }
                    }
                       

                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, TwoFactorSessionExpired });
                }
                //else if (!validPassword)
                //{
                //    _logger.LogInformation("User locked out.");
                //    //Add error
                //    return Page();
                //}
                else if (result.IsLockedOut && validPassword)
                {
                    _logger.LogInformation("User locked out.");
                    if (user.NeedsToResetPassword && !user.IsSuspended)
                    {
                        //await _signInManager.SignOutAsync();
                        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                        return RedirectToPage("./ResetPassword", new { code = code, userId = user.Id });
                    }
                    else if (user.IsSuspended)
                    {
                        _logger.LogInformation("User is suspended");
                    }
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    _logger.LogInformation("User login failed.");
                    if (user == null)
                    {
                        ErrorMessage = "Invalid Username";
                    }
                    else if (!validPassword)
                    {
                        ErrorMessage = "Invalid Password";
                    }
                    else
                    {
                        ErrorMessage = "Invalid Login Attempt";
                    }
                    //var user = await _userManager.FindByEmailAsync(Input.Email);
                    //if (user.NeedsToResetPassword)
                    //{
                    //    //await _signInManager.SignOutAsync();
                    //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //    return RedirectToPage("./ResetPassword", new { code = code });
                    //}
                    return Page();
                }
            }
            ErrorMessage = "";

            // If we got this far, something failed, redisplay form
            return Page();
        }

        //[HttpGet]
        //public async Task<IActionResult> ResetPasswordWith2FA()
        //{

        //}

        //[HttpGet]
        //public async Task<IActionResult> ResetPassword()
        //{
        //    var user = _userManager.GetUserAsync(User);
        //    return Page(user);
        //}



    }
}
