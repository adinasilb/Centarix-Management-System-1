﻿using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class DocumentsModalViewModel
    {
        public AppUtility.RequestFolderNamesEnum RequestFolderName { get; set; }
        public Request Request { get; set; }
        public List<string> FileStrings { get; set; }
    }
}
