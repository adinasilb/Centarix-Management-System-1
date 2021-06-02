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
        public string ReportDescription { get; set; }

    }
}
