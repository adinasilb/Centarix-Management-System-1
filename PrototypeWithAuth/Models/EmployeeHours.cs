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
        [DataType(DataType.Time)]
        public DateTime Entry1 { get; set; }
        [DataType(DataType.Time)]
        public DateTime? Entry2 { get; set; }
        [DataType(DataType.Time)]
        public DateTime? Exit1 { get; set; }
        [DataType(DataType.Time)]
        public DateTime? Exit2 { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int? OffDayTypeID { get; set; }
        public OffDayType OffDayType { get; set; }
        public int? EmployeeHoursStatusID { get; set; }
        public TimeSpan TotalHours => ((Exit1 - Entry1) ?? TimeSpan.Zero) + ((Exit2 - Entry2)??TimeSpan.Zero);           
        public EmployeeHoursStatus EmployeeHoursStatus { get; set; } 
        
    }
}
