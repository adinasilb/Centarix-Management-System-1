using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ReportType
    {
        [Key]
        public int ReportTypeID { get; set; }
        public string ReportTypeDescription { get; set; }
    }
}
