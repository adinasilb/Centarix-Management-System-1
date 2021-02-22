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
        public string FileName { get; set; }
        public int RequestID { get; set; }
        public AppUtility.RequestFolderNamesEnum FolderName { get; set; }
        public bool IsEdittable { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
    }
}
