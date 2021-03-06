using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class Report : ModelBase
    {
        [Key]
        public int ReportID { get; set; }
        public string ReportTitle { get; set; }
        public string ReportText { get; set; }
        public int ReportTypeID { get; set; }
        public ReportType ReportType { get; set; }
        public DateTime DateCreated { get; set; }
        public string ReportNumber { get; set; }
        public int ReportCategoryID { get; set; }
        public ResourceCategory ReportCategory { get; set; }
        public int WeekNumber { get; set; }
        public string TemporaryReportText { get; set; }
    }
}
