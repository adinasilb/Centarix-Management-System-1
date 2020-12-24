using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class EmployeeStatus 
    {
        [Key]
        public int EmployeeStatusID { get; set; }
        public string Description { get; set; }
        public IEnumerable<Employee> Employees {get; set;}

        [Column(TypeName ="char(2)")]
        public string Abbreviation { get; set; }
        public int LastCentarixID { get; set; }
        public DateTime LastCentarixIDTimeStamp { get; set; }
    }
}
