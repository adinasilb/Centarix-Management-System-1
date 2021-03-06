using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ResetPasswordViewModel : ViewModelBase
    {
        public ApplicationUser User { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        // [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }
        public string AuthenticatorUri { get; set; }
        public TwoFactorAuthenticationViewModel TwoFactorAuthenticationViewModel { get; set; }
    }
}
