using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class DeleteReportDocumentViewModel :ViewModelBase
    {
        public FunctionReport FunctionReport { get; set; }
        public int ReportID { get; set; }
        public List<DocumentFolder> DocumentsInfo { get; set; }
    }
}
