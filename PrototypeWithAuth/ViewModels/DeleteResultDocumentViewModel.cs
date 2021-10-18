using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class DeleteResultDocumentViewModel :ViewModelBase
    {
        public FunctionResult FunctionResult { get; set; }
        public int ResultID { get; set; }
        public List<DocumentFolder> DocumentsInfo { get; set; }
    }
}
