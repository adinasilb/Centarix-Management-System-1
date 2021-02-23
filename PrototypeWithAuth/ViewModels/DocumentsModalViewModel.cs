﻿using Microsoft.AspNetCore.Http;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class DocumentsModalViewModel : ViewModelBase
    {
        public AppUtility.RequestFolderNamesEnum RequestFolderName { get; set; }
        public List<Request> Requests { get; set; }
        public List<string> FileStrings { get; set; }
        public List<FileInfo> Files { get; set; }
        public List<IFormFile> FilesToSave { get; set; }
        public bool IsEdittable { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public AppUtility.RequestModalType ModalType { get; set; }
    }
}
