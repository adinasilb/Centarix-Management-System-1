using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddReportFunctionViewModel : AddFunctionViewModel<FunctionReport>
    {
        public int ReportID { get; set; }
        public string ClosingTags { get; set; }

    }
}
