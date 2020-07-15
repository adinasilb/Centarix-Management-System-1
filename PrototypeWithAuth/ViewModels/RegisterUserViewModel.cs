﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrototypeWithAuth.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        //[DataType(DataType.Text)]
        //[Display(Name = "First Name")]
        //public string FirstName { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        //[DataType(DataType.Text)]
        //[Display(Name = "Last Name")]
        //public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password), Compare(nameof(Password))]
        //[Display(Name = "Confirm password")]
        //// [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
        //public IEnumerable<IdentityRole> Roles { get; set; }

        //[Required]
        //public string Role { get; set; }

        [Required]
        [Display(Name = "Google Secure App Password")]
        public string SecureAppPass { get; set; }


        //Permissions
        public SelectList OrdersList { get; set; }
        public string[] SelectedOrders { get; set; }
        public SelectList ProtocolsList { get; set; }
        public string[] SelectedProtocols { get; set; }
        public SelectList LabManagementList { get; set; }
        public string[] SelectedLabManagement { get; set; }
        public SelectList AccountingList { get; set; }
        public string[] SelectedAccounting { get; set; }
        public SelectList OperationsList { get; set; }
        public string[] SelectedOperations { get; set; }
        public SelectList ExpensesList { get; set; }
        public string[] SelectedExpenses { get; set; }
        public SelectList BiomarkersList { get; set; }
        public string[] SelectedBiomarkers { get; set; }
        public SelectList IncomeList { get; set; }
        public string[] SelectedIncome { get; set; }
        public SelectList TimekeeperList { get; set; }
        public string[] SelectedTimekeeper { get; set; }
        public SelectList UsersList { get; set; }
        public string[] SelectedUsers { get; set; }


        //Budget

        public double LabMonthlyLimit { get; set; }
        public double LabUnitLimit { get; set; }
        public double LabOrderLimit { get; set; }
        public double OperationMonthlyLimit { get; set; }
        public double OperationUnitLimit { get; set; }
        public double OperaitonOrderLimit { get; set; }
    }
}
