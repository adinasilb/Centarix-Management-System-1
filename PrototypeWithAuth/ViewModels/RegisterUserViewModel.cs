using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using PrototypeWithAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace PrototypeWithAuth.ViewModels
{
    public class RegisterUserViewModel
    {
        public string ApplicationUserID { get; set; } //this is used for the Edit form
        public int UserNum { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        //[DataType(DataType.Text)]
        //[Display(Name = "User Name")]
        //public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password), Compare(nameof(Password))]
        //[Display(Name = "Confirm password")]
        //// [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        [Required]
        public string CentarixID { get; set; }

        //[Required]
        //public string Role { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Google Secure App Password")]
        public string SecureAppPass { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number 2")]
        public string PhoneNumber2 { get; set; }

        //Permissions
        public List<UserRoleViewModel> OrderRoles { get; set; }
        public List<UserRoleViewModel> ProtocolRoles { get; set; }
        public List<UserRoleViewModel> OperationRoles { get; set; }
        public List<UserRoleViewModel> BiomarkerRoles { get; set; }
        public List<UserRoleViewModel> TimekeeperRoles { get; set; }
        public List<UserRoleViewModel> LabManagementRoles { get; set; }
        public List<UserRoleViewModel> AccountingRoles { get; set; }
        public List<UserRoleViewModel> ExpenseesRoles { get; set; }
        public List<UserRoleViewModel> IncomeRoles { get; set; }
        public List<UserRoleViewModel> UserRoles { get; set; }

        //Budget

        [Display(Name = "Monthly Limit")]
        public double LabMonthlyLimit { get; set; }

        [Display(Name = "Unit Limit")]
        public double LabUnitLimit { get; set; }

        [Display(Name = "Order Limit")]

        public double LabOrderLimit { get; set; }

        [Display(Name = "Monthly Limit")]
        public double OperationMonthlyLimit { get; set; }

        [Display(Name = "Unit Limit")]
        public double OperationUnitLimit { get; set; }

        [Display(Name = "Order Limit")]
        public double OperaitonOrderLimit { get; set; }

        //public IFormFile UserImage { get; set; }
        public string UserImage { get; set; }

        public string UserImageSaved { get; set; }


        //Added For Employees
        public Employee? NewEmployee { get; set; }
        public decimal NewEmployeeWorkScope { get; set; }
        public FormFile NewEmployeeImage { get; set; }
        public int NewEmployeeStatus { get; set; }
        public List<JobCategoryType> JobCategoryTypes { get; set; }
        public List<EmployeeStatus> EmployeeStatuses { get; set; }
        public List<MaritalStatus> MaritalStatuses { get; set; }
        public List<Degree> Degrees { get; set; }
        public List<Citizenship> Citizenships { get; set; }
        public int Tab { get; set; }
        //this for the 2fa
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
        public string SharedKey { get; set; }

        public string AuthenticatorUri { get; set; }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public bool ConfirmedEmail { get; set; }
    }
}
