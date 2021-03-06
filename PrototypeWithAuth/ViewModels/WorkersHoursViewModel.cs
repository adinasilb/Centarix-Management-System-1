using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PrototypeWithAuth.AppData.AppUtility;

namespace PrototypeWithAuth.ViewModels
{
    public class WorkersHoursViewModel : ViewModelBase
    {
        public YearlyMonthlyEnum YearlyMonthlyEnum { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<int> Years { get; set; }
        public List<int> Months { get; set; }
        public List<WorkerHourViewModel> Employees { get; set; }
        public int TotalWorkingDaysInMonthOrYear { get; set; }
        //public int TotalWorkingDaysInYear { get; set; }
    }
}
