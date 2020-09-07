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
        public double WorkScope { get { return 100-(8.5 / HoursPerDay); } private set {; } }
        public double GrossSalary { get; set; }
        public double NetSalary { get { return GrossSalary - IncomeTax; } private set {; } }
        public double EmployerTax { get; set; }
        public double TotalCost { get { return GrossSalary +EmployerTax; } private set {; } }
        public double IncomeTax { get; set; }
        public double HoursPerDay { get; set; }
        public int VacationDays { get; set; }
        public string JobTitle { get; set; }
        public IEnumerable<EmployeeHours> EmployeeHours {get; set;}
        public int EmployeeStatusID { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }

        //todo add job category
    }
}
