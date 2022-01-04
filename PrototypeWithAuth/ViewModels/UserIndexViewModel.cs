﻿using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserIndexViewModel : ViewModelBase
    {
        public List<UserWithCentarixIDViewModel> ApplicationUsers { get; set; }
        public bool IsCEO { get; set; }
    }
}
