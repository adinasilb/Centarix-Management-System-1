using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryOfDaysOffViewModel
    {
        public IEnumerable<PrototypeWithAuth.ViewModels.DaysOffViewModel> DaysOffs { get; set; }
        public double SickDaysLeft { get; set; }
        public double VacationDaysLeft { get; set; }
        public double TotalVacationDaysPerYear { get; set; }
        public double TotalSickDaysPerYear { get; set; }
    }
}
