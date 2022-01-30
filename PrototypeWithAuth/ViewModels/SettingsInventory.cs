﻿using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SettingsInventory
    {
        public CategoryListViewModel Categories { get; set; }
        public CategoryListViewModel Subcategories { get; set; }
        public SettingsForm SettingsForm { get; set; }
    }
}
