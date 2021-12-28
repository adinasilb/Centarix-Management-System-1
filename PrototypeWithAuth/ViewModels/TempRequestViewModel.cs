﻿using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class TempRequestViewModel
    {
        public Request Request { get; set; }
        public List<Payment> Payments { get; set; }
        public List<RequestComment> Comments { get; set; }
        public List<String> Emails { get; set; }
    }
}
