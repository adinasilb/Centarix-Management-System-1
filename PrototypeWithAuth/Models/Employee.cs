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
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public int Age
        {
            get
            {
                int _age = 0;
                if (DOB != null)
                {
                    _age = DateTime.Now.Year - DOB.Year;
                    if (DateTime.Now.DayOfYear > DOB.DayOfYear)
                    {
                        _age--;
                    }
                }
                return _age;
            }
            private set {; }
        }
        public double GrossSalary { get; set; }
        public double NetSalary { get { return GrossSalary - IncomeTax; } private set {; } }
        public double EmployerTax { get; set; }
        public double TotalCost { get { return GrossSalary + EmployerTax; } private set {; } }
        public double IncomeTax { get; set; }
        [Display(Name ="Tax Credits")]
        public int TaxCredits { get; set; }
        public int VacationDays { get; set; }
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }
        public int? DegreeID { get; set; }
        [ForeignKey("DegreeID")]
        public Degree Degree { get; set; }

        [Display(Name = "Relationship Status")]    
        public int? MaritalStatusID { get; set; }
        [ForeignKey("MaritalStatusID")]
        public MaritalStatus MaritalStatus { get; set; }
        //public string RelationshipStatus {get; set;}
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber2 { get; set; }
        //public string Citizenship { get; set; }
        public int? CitizenshipID { get; set; }
        [ForeignKey("CitizenshipID")]
        public Citizenship Citizenship { get; set; }
        public IEnumerable<EmployeeHours> EmployeeHours { get; set; }
        public int EmployeeStatusID { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }
        public int? JobCategoryTypeID { get; set;}
        [ForeignKey("JobCategoryTypeID")]
        public JobCategoryType JobCategoryType { get; set; }
        public Freelancer Freelancer { get; set; }
        public SalariedEmployee SalariedEmployee { get; set; }
        public Advisor Advisor { get; set; }
        //todo add job category
    }
}
