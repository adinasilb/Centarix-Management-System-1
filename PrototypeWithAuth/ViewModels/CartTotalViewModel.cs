﻿using System;
using System.Collections.Generic;
using PrototypeWithAuth.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.ViewModels
{
    public class CartTotalViewModel
    {
        public Vendor Vendor { get; set; }
        public double VendorCartTotal { get; set; }
        public Request Request { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
    }
}
