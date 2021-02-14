using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PrototypeWithAuth.AppData.AppUtility;

namespace PrototypeWithAuth.ViewModels
{
    public class DocumentsTabViewModel
    {
        public Request Request { get; set; }
        public Dictionary<AppUtility.RequestFolderNamesEnum, List<string>> DocumentInfo { get; set; }
    }
    

}
