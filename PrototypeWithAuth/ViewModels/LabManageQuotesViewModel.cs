﻿using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class LabManageQuotesViewModel
    {
        public ILookup<Vendor, Reorder> RequestsByVendor { get; set; }
    }
}
