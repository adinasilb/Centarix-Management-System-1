using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryOfDaysOffViewModel
    {
        public int Year { get; set; }
        public int TotalVacationDays { get; set; }
        public int VacationDaysTaken { get; set; }
        public int SickDaysTaken { get; set; }
        public int VacationDaysLeft { private set {; } get { return TotalVacationDays - VacationDaysTaken; } }
    }
}
