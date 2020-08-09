using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class DeleteDocumentsViewModel
    {
        public string FileName { get; set; }
        public int RequestID { get; set; }
        public AppUtility.RequestFolderNamesEnum FolderName { get; set; }
    }
}
