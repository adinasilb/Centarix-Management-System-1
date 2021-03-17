using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;

namespace PrototypeWithAuth.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginWith2faModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginWith2faModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginWith2faModel> logger, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

      

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
            public bool InputRememberTwoFactor { get; set; }
            public bool TwoFactorSessionExpired { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe = false, string returnUrl = null, bool twoFactorSessionExpired = false, bool InputRememberTwoFactor = false, string errorMessage = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;
            Input = new InputModel
            {
                TwoFactorSessionExpired = twoFactorSessionExpired,
                InputRememberTwoFactor = InputRememberTwoFactor
            };
            if(errorMessage != null)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                if(Input.InputRememberTwoFactor && !Input.TwoFactorSessionExpired)
                {
                    var myIpAddress = AppUtility.GetMyIPAddress();
                    var ipEnd = myIpAddress.Split(".").LastOrDefault();
                    var ipEndNum = 501;
                    Int32.TryParse(ipEnd, out ipEndNum);

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
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("TwoFactorCookie" + cookieNum, user.Id, cookieOptions);

                        var address = AppUtility.PhysicalAddress;
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
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return Page();
            }
        }
    }
}
