﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RequestLink : ReportSection
    {
        public int RequestID { get; set; }
        public Request Request { get; set; }
    }
}