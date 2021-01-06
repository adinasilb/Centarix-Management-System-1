using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class DaysOffViewModel
    {
        public int Year { get; set; }
        public double TotalVacationDays { get; set; }
        public double VacationDaysTaken { get; set; }
        public double SickDaysTaken { get; set; }
        private double _TotalSickDays;
        public double TotalSickDays
        {
            get 
            {
                if (_TotalSickDays > 90)
                {
                    return _TotalSickDays = 90;
                }
                else
                {
                    return _TotalSickDays;
                }
            }
            set
            {
                if (_TotalSickDays > 90)
                {
                    _TotalSickDays = 90;
                }
                else
                {
                    _TotalSickDays=value;
                }
            }
        }
        public double SickDaysLeft
        {
            private set {; }
            get { return TotalSickDays - SickDaysTaken; }
        }
        public double VacationDaysLeft
        {
            private set {; }
            get { return TotalVacationDays - VacationDaysTaken; }
        }
    }
}
