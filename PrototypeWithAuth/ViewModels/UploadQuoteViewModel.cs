﻿using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UploadQuoteViewModel
    {
        public Request Request { get; set; }
        public List<string> QuoteFileStrings { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
    }
}
