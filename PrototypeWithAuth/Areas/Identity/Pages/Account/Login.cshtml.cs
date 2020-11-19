using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using System.Web;

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

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context
          )
        {
            _userManager = userManager;
           // _identityUserManager = identityuserManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
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
                var passwordValidator = new PasswordValidator<ApplicationUser>();
                //var identityUserManager = new UserManager<IdentityUser>(user);
                var validPassword = await passwordValidator.ValidateAsync(_userManager, user, Input.Password);
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
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl });
                }
                else if (!validPassword.Succeeded)
                {
                    _logger.LogInformation("User locked out.");
                    //Add error
                    return Page();
                }
                else if (result.IsLockedOut)
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
                    //var user = await _userManager.FindByEmailAsync(Input.Email);
                    //if (user.NeedsToResetPassword)
                    //{
                    //    //await _signInManager.SignOutAsync();
                    //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //    return RedirectToPage("./ResetPassword", new { code = code });
                    //}
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

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
