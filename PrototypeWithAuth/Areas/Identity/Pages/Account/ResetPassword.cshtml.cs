﻿using System;
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

        public async Task<IActionResult> OnGet(string code = null, string userID = null)
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
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                    AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey)
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                var verificationCode = Input.TwoFactorAuthenticationViewModel.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

                var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                    user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

                if (!is2faTokenValid)
                {
                    var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                    Input.ErrorMessage = "Invalid Authentication Code";
                    Input.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
                    //return View(resetPasswordViewModel);
                }
                user.NeedsToResetPassword = false;
                user.LockoutEnabled = false;
                user.LockoutEnd = DateTime.Now;
                _context.Update(user);
                await _context.SaveChangesAsync();

                var signInResult = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: false);
                if (signInResult.Succeeded || signInResult.RequiresTwoFactor)
                {
                    return RedirectToAction("Index");
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("PrototypeWithAuth"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}
