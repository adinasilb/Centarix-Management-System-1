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
        public DateTime StartedWorking_submit { get { return StartedWorking; } set { StartedWorking = value; } }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public DateTime DOB_submit { get { return DOB; } set { DOB = value; } }
        public int Age
        {
            get
            {
                int _age = 0;
                if (DOB != null)
                {
                    _age = DateTime.Now.Year - DOB.Year;
                    if (DateTime.Now.DayOfYear < DOB.DayOfYear)
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
        public double VacationDays { get; set; }
        public double SickDays 
        {
            get { return 18; }
            private set {; }
        }
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
        public int? JobSubcategoryTypeID { get; set;}
        [ForeignKey("JobSubcategoryTypeID")]
        public JobSubcategoryType JobSubcategoryType { get; set; }
        public Freelancer Freelancer { get; set; }
        public SalariedEmployee SalariedEmployee { get; set; }
        public Advisor Advisor { get; set; }

        public bool IsUser { get; set; }

        public IEnumerable<CentarixID> CentarixIDs { get; set; }
        [Display(Name = "Roll Over Sick Days")]
        public double RollOverSickDays { get; set; }
        [Display(Name = "Roll Over Vacation Days")]
        public double RollOverVacationDays { get; set; }
        [Display(Name = "Bonus Sick Days")]
        public double BonusSickDays { get { return 0; } set {; } }
        [Display(Name = "Bonus Vacation Days")]
        public double BonusVacationDays { get { return 0; } set {; } }
        [Display(Name = "Special Days")]
        public double SpecialDays { get; set; }
        private double _VacationDaysPerMonth;
        public double VacationDaysPerMonth
        {
            get
            {
                return Math.Round(VacationDays / 12.0, 2, MidpointRounding.AwayFromZero);
            }
            private set {; }
        }
        private double _SickDaysPerMonth;
        public double SickDaysPerMonth
        {
            get
            {
                return Math.Round(SickDays / 12.0, 2, MidpointRounding.AwayFromZero);
            }
            private set {; }
        }
    }
}
