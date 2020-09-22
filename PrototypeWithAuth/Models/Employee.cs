using Org.BouncyCastle.Asn1.Mozilla;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Employee : ApplicationUser
    {
        [Display(Name = "Started Working")]
        [DataType(DataType.Date)]
        public DateTime StartedWorking { get; set; }
        public double GrossSalary { get; set; }
        public double NetSalary { get { return GrossSalary - IncomeTax; } private set {; } }
        public double EmployerTax { get; set; }
        public double TotalCost { get { return GrossSalary + EmployerTax; } private set {; } }
        public double IncomeTax { get; set; }
        public int VacationDays { get; set; }
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
   
        public int DegreeID { get; set; }
        [ForeignKey("DegreeID")]
        public Degree Degree { get; set; }
        //public string Degree { get; set; }
        [Display(Name = "ID Number")]
        public int IDNumber { get; set; }
        [Display(Name = "Relationship Status")]    
        public string RelationshipStatus {get; set;}
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber2 { get; set; }
        public string Citizenship { get; set; }
        public IEnumerable<EmployeeHours> EmployeeHours { get; set; }
        public int EmployeeStatusID { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }
        public int JobCategoryTypeID { get; set;}
        [ForeignKey("JobCategoryTypeID")]
        public JobCategoryType JobCategoryType { get; set; }
        public Freelancer Freelancer { get; set; }
        public SalariedEmployee SalariedEmployee { get; set; }
        //todo add job category
    }
}
