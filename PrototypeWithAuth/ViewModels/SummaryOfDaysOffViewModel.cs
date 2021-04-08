using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryOfDaysOffViewModel : ViewModelBase
    {
        public IEnumerable<PrototypeWithAuth.ViewModels.DaysOffViewModel> DaysOffs { get; set; }
        public double SickDaysLeft { get; set; }
        private double _VacationDaysLeft;
        public double VacationDaysLeft
        {
            get { return Math.Round(_VacationDaysLeft, 2); }
            set { _VacationDaysLeft = value; }
        }
        public double TotalVacationDaysPerYear { get; set; }
        public double TotalSickDaysPerYear { get; set; }
        public double BonusSickDays { get; set; }
        public double BonusVacationDays { get; set; }
        public Employee Employee {get ;set;}
    }
}
