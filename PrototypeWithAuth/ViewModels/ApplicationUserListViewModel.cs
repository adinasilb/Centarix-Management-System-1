﻿using System;
using System.Collections.Generic;
using PrototypeWithAuth.Models;
using PrototypeWithAuth;
using PrototypeWithAuth.ViewModels;
using PrototypeWithAuth.Areas;
using PrototypeWithAuth.Areas.Identity;
using PrototypeWithAuth.Areas.Identity.Pages;
using PrototypeWithAuth.Areas.Identity.Pages.Account;
using PrototypeWithAuth.Areas.Identity.Pages.Account.Manage;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrototypeWithAuth.ViewModels
{
    public class ApplicationUserListViewModel
    {
        public IEnumerable<ApplicationIdentity> ApplicationIdentity { get; set; }
    }
}
