using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryHoursViewModel
    {
        public List<EmployeeHours> EmployeeHours { get; set; }
        public DateTime CurrentMonth { get; set; }
        public int  SelectedYear { get; set;  }
        public double? TotalHoursInMonth { get; set; }
    }
}
