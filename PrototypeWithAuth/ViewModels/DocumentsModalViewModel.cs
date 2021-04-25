using Microsoft.AspNetCore.Http;
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
        public AppUtility.FolderNamesEnum FolderName { get; set; }
        public int ObjectID { get; set; }
        public AppUtility.ParentFolderName ParentFolderName { get; set; }
        public List<string> FileStrings { get; set; }
        public List<FileInfo> Files { get; set; }
        public List<IFormFile> FilesToSave { get; set; }
        public bool IsEdittable { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public bool ShowSwitch { get; set; }
    }
}
