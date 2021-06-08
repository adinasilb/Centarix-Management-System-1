using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ReportSection
    {
        [Key]
        public int ReportSectionID { get; set; }
        public string ReportSectionContent { get; set; }
        public int ReportID { get; set; }
        public Report Report { get; set; }
        public int SectionNumber { get; set; }
    }
}
