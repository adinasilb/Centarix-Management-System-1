using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class DocumentFolder
    {
        public AppUtility.RequestFolderNamesEnum FolderName { get; set; }
        public List<string> FileStrings { get; set; }
        public string Icon { get; set; }
    }
}
