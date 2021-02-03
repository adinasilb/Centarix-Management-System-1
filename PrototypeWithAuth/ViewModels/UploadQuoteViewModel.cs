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
        public ParentQuote ParentQuote { get; set; }
        public List<string> FileStrings { get; set; }
        public AppUtility.OrderTypeEnum OrderTypeEnum { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }

    }
}
