using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddReportFunctionViewModel : AddFunctionViewModel
    {
        public int ReportID { get; set; }
        public string ReportTempText { get; set; }
        public string FileName { get; set; }
        public FunctionReport FunctionReport { get; set; }
    }
}
