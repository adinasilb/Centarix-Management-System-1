using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ReportDaysViewModel
    {
        public double VacationDays { get; set; }
        public double SickDays { get; set; }
        public IEnumerable<EmployeeHours> VacationDaysTaken { get; set; }
        public IEnumerable<EmployeeHours> SickDaysTaken { get; set; }
        public double VacationDaysLeft 
        { 
            get { return VacationDays - VacationDaysTaken.Count(); } private set {; } 
        }
        public double SickDaysLeft 
        {
            get { return SickDays- SickDaysTaken.Count(); }  private set {; } 
        }
        public int SelectedYear { get; set; }
    }
}
