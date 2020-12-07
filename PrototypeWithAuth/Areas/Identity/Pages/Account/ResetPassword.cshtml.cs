using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PrototypeWithAuth.Data;
using System.Diagnostics;

namespace PrototypeWithAuth.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly ApplicationDbContext _context;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public ResetPasswordModel(UserManager<ApplicationUser> userManager, UrlEncoder urlEncoder, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _urlEncoder = urlEncoder;
            _context = context;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        //public IFormatProvider AuthenticatorUriFormat { get; private set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
            public string ErrorMessage { get; set; }
            public string AuthenticatorUri { get; set; }
            public PrototypeWithAuth.ViewModels.TwoFactorAuthenticationViewModel TwoFactorAuthenticationViewModel { get; set; }
        }

        public async Task<IActionResult> OnGet(string code = null, string userID = null, string errorMessage = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                //var user = await _userManager.GetUserAsync(User);
                var user = await _userManager.FindByIdAsync(userID);
                var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                if (string.IsNullOrEmpty(unformattedKey))
                {
                    await _userManager.ResetAuthenticatorKeyAsync(user);
                    unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                }

                var email = user.Email;
                Input = new InputModel
                {
                    Email = user.Email,
                    Code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)),
                    AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey),
                    ErrorMessage = errorMessage
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string errorMessage = "";
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            };


            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Input.Code)), Input.Password);
            if (result.Succeeded)
            {
                var verificationCode = Input.TwoFactorAuthenticationViewModel.TwoFACode.Replace(" ", string.Empty).Replace("-", string.Empty);

                var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                    user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

                if (!is2faTokenValid)
                {
                    //var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                    Input.ErrorMessage = "Password reset was successful.\nInvalid Authentication Code.";
                    //Input.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
                    ////return View(resetPasswordViewModel);
                    //var pcode = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //pcode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(pcode));
                    return RedirectToPage("./Login", new { errorMessage = Input.ErrorMessage });

                }

                if (!user.IsSuspended) //don't want to unlock them out if they are suspended
                {
                    user.LockoutEnd = DateTime.Now;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                var signInResult = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (signInResult.RequiresTwoFactor) //took out the is locked out query- b/c they may be suspended
                {
                    return RedirectToPage("./Login", new { SuccessMessage = "Password Reset & 2 Factor Authentication Setup Successful.\nLogin to Continue" });

                    //var authenticatorCode = Input.TwoFactorAuthenticationViewModel.TwoFACode.Replace(" ", string.Empty).Replace("-", string.Empty);

                    //var result2fa = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, false, false);

                    //if (result2fa.Succeeded)
                    //{
                    //    //user.LockoutEnabled = false;
                    //    user.NeedsToResetPassword = false;
                    //    _context.Update(user);
                    //    await _context.SaveChangesAsync();
                    //    return RedirectToAction("Index", "Home");
                    //}
                    ////TODO: Add errors for 2fa
                    //else
                    //{
                    //    user.LockoutEnabled = true;
                    //    user.LockoutEnd = new DateTime(2999,01 , 01);
                    //    _context.Update(user);
                    //    await _context.SaveChangesAsync();
                    //    errorMessage += "Invalid Authentication Code";
                    //}

                }
                //TODO: Add errors for signing
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                Input.ErrorMessage += "\n" + error.Description;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            Input = new InputModel
            {
                Email = user.Email,
                Code = code,
                AuthenticatorUri = Input.AuthenticatorUri,
                ErrorMessage = errorMessage
            };
            //return Page();
            return RedirectToPage("./ResetPassword", new { code = code, userId = user.Id, errorMessage = Input.ErrorMessage });
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("Elixir"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}
