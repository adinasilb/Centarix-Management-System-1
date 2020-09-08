using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class SalariedEmployee
    {
        [Key]
        public int SalariedEmployeeID { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public double WorkScope { get { return 100*(HoursPerDay/8.5); } private set {; } }
        public double HoursPerDay { get; set; }
    }
}
