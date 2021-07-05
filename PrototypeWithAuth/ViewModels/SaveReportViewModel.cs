using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SaveReportViewModel
    {
        public int ReportID { get; set; }
        public bool SaveReport { get; set; }
        public string ReportTitle { get; set; }
    }
}
