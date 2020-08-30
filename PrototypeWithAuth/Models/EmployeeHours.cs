using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class EmployeeHours
    {
        [Key]
        public int EmployeeHoursID { get; set; }
        public string EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
        public DateTime Entry1 { get; set; }
        public DateTime? Entry2 { get; set; }
        public DateTime? Exit1 { get; set; }
        public DateTime? Exit2 { get; set; }
        public int? OffDayTypeID { get; set; }
        public OffDayType OffDayType { get; set; }
        public int? EmployeeHoursStatusID { get; set; }
        public EmployeeHoursStatus EmployeeHoursStatus { get; set; } 
    }
}
