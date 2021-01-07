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
        public IEnumerable<EmployeeHours> VacationDaysTaken { get; set; }
        public double  VacationDaysTakenCount { get; set; }
        public IEnumerable<EmployeeHours> SickDaysTaken { get; set; }
        private double _SickDays;
        public double SickDays
        {
            get
            {
                if (_SickDays > 90)
                {
                    return _SickDays = 90;
                }
                else
                {
                    return _SickDays;
                }
            }
            set
            {
                if (_SickDays > 90)
                {
                    _SickDays = 90;
                }
                else
                {
                    _SickDays = value;
                }
            }
        }
        public double VacationDaysLeft 
        { 
            get { return VacationDays - VacationDaysTakenCount; } private set {; } 
        }
        public double SickDaysLeft 
        {
            get { return SickDays- SickDaysTaken.Count(); }  private set {; } 
        }
        public int SelectedYear { get; set; }
    }
}
