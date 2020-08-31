using Org.BouncyCastle.Asn1.Mozilla;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Employee : ApplicationUser
    {
        public DateTime StartedWorking { get; set; }
        public int WorkScope { get; set; }
        public double GrossSalary { get; set; }
        public double? Food { get; set; }
        public double? Transportation { get; set; }
        public double? BituachLeumiEmployer { get; set; }
        public double? EducationFundEmployer { get; set; }
        public double? PensionEmployer { get; set; }
        public double IncomeTax { get; set; }
        public double HoursPerWeek { get; set; }
        public int VacationDays { get; set; }
        public IEnumerable<EmployeeHours> EmployeeHours {get; set;}
    }
}
