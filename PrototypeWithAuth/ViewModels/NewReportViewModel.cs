using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class NewReportViewModel
    {
        public DateTime ReportDate { get; set; }
        public int ReportCategoryID { get; set; }
        public ReportType ReportType { get; set; }
    }
}
