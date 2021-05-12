using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class Report
    {
        [Key]
        public int ReportID { get; set; }
        public string ReportTitle { get; set; }
        public string ReportDescription { get; set; }
        public int ReportTypeID { get; set; }
        public ReportType ReportType { get; set; }
        public DateTime DateCreated { get; set; }
        public int ReportNumber { get; set; }
        public int ReportCategoryID { get; set; }
        public ResourceCategory ReportCategory { get; set; }
        public IEnumerable<ReportSection> ReportSections { get; set; }
    }
}
