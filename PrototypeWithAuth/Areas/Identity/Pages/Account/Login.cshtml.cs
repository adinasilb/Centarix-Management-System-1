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

namespace PrototypeWithAuth.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ApplicationDbContext _context;

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
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
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                { 
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private void fillInTimekeeperMissingDays(ApplicationUser user)
        {
                DateTime nextDay = user.LastLogin.AddDays(1);
                while (nextDay.Date <= DateTime.Today)
                {
                    if (nextDay.DayOfWeek != DayOfWeek.Friday && nextDay.DayOfWeek != DayOfWeek.Saturday)
                    {
                        var existentHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Date == nextDay.Date).FirstOrDefault();
                        if (existentHours == null)
                        {
                            EmployeeHours employeeHours = new EmployeeHours
                            {
                                EmployeeID = user.Id,
                                Date = nextDay.Date
                            };
                            _context.Update(employeeHours);
                        }
                    }
                nextDay = nextDay.AddDays(1);
                 
             }
            _context.SaveChanges();
        }

        private void fillInOrderLate(ApplicationUser user)
        { 
            if (user.LastLogin.Date != DateTime.Now.Date)
            {
                var lateOrders = _context.Requests.Where(r => r.ApplicationUserCreatorID == user.Id).Where(r => r.RequestStatusID == 2)
                    .Where(r => r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0).Date >= user.LastLogin.Date && r.ParentRequest.OrderDate.AddDays(r.ExpectedSupplyDays ?? 0).Date < DateTime.Today)
                    .Include(r => r.Product).ThenInclude(p => p.Vendor).Include(r => r.ParentRequest);
                foreach (var request in lateOrders)
                {
                    RequestNotification requestNotification = new RequestNotification();
                    requestNotification.RequestID = request.RequestID;
                    requestNotification.IsRead = false;
                    requestNotification.RequestName = request.Product.ProductName;
                    requestNotification.ApplicationUserID = request.ApplicationUserCreatorID;
                    requestNotification.Description = "should have arrived " + request.ParentRequest.OrderDate.AddDays(request.ExpectedSupplyDays ?? 0).ToString("dd/MM/yyyy");
                    requestNotification.NotificationStatusID = 1;
                    requestNotification.TimeStamp = DateTime.Now;
                    requestNotification.Controller = "Requests";
                    requestNotification.Action = "NotificationsView";
                    requestNotification.OrderDate = request.ParentRequest.OrderDate;
                    requestNotification.Vendor = request.Product.Vendor.VendorEnName;
                    _context.Update(requestNotification);
                }
                _context.SaveChanges();

            }
        }
    }
}
