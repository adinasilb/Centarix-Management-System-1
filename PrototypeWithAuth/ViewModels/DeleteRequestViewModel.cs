﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class DeleteRequestViewModel
    {
        public Request Request { get; set; } 

        public RequestIndexObject RequestIndexObject { get; set; }
    }
}
