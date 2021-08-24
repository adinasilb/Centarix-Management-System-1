using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class DeleteDocumentsViewModel : ViewModelBase
    {
        public string FullFileName { get; set; }
        public string FileName { get; set; }
        public int ObjectID { get; set; }
        public AppUtility.FolderNamesEnum FolderName { get; set; }
        public AppUtility.ParentFolderName ParentFolderName { get; set; }
        public bool IsEdittable { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
    }
}
