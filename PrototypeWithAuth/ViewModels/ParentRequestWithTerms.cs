﻿using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ParentRequestWithTerms
    {
        public ParentRequest ParentRequest { get; set; }
        public DateTime DateToBePaid { get; set; }
    }
}
