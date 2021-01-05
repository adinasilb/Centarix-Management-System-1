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
        public IEnumerable<EmployeeHours> SickDaysTaken { get; set; }
        public double VacationDaysLeft{ get; set; }
        public int SelectedYear { get; set; }
    }
}
