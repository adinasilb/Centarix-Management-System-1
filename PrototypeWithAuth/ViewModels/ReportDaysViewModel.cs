using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ReportDaysViewModel
    {
        public int Vacationdays { get; internal set; }
        public int VacationDays { get; set; }
        public IEnumerable<EmployeeHours> VacationDaysTaken { get; set; }
        public IEnumerable<EmployeeHours> SickDaysTaken { get; set; }
    }
}
