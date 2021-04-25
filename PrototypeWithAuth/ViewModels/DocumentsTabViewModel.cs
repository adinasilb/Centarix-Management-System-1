using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PrototypeWithAuth.AppData.AppUtility;

namespace PrototypeWithAuth.ViewModels
{
    public class DocumentsTabViewModel : ViewModelBase
    {
        public Request Request { get; set; }
        public Dictionary<AppUtility.FolderNamesEnum, List<string>> DocumentInfo { get; set; }
    }
    

}
