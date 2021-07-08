using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class DocumentFolder
    {
        public AppUtility.FolderNamesEnum FolderName { get; set; }
        public List<string> FileStrings { get; set; }
        public string Icon { get; set; }
        public AppUtility.ParentFolderName ParentFolderName { get; set; }
        public string ObjectID { get; set; }
    }
}
