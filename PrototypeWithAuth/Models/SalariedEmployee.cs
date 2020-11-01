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
        [Display(Name = "Job Scope")]
        public double WorkScope { get { return 100*(HoursPerDay/8.4); } private set {; } } //8.4 is 8 hours and 24 minutes
        [Display(Name = "Hours Per Day")]
        public double HoursPerDay { get; set; }
    }
}
