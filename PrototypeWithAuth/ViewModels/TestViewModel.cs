﻿using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class TestViewModel
    {
        public ExperimentEntry ExperimentEntry { get; set; }
        public int ExperimentID { get; set; }
        public List<SelectListItem> ExperimentEntries { get; set; }
        public Guid Guid { get; set; }
        public List<Test> Tests { get; set; }
        public List<TestValue> TestValues { get; set; } //THIS IS ONLY FOR THE FIRST ONE!!!!
        public List<FieldViewModel> FieldViewModels { get; set; }
        public List<BoolIntViewModel> FilesPrevFilled { get; set; }
    }
}
