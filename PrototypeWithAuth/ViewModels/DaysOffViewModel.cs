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
        public double TotalSickDays { get; set; }
        //public double SickDaysLeft 
        //{
        //    private set {; }  get { return TotalSickDays - SickDaysTaken; }
        //}
        //public double VacationDaysLeft 
        //{ 
        //    private set {; } get { return TotalVacationDays - VacationDaysTaken; } 
        //}
    }
}
