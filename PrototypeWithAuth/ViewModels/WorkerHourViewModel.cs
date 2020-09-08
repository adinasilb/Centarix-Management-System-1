using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class WorkerHourViewModel
    {
        public Employee Employee { get; set; }
        public double Hours { get; set; }
        public int WorkingDays { get; set; }
        public int VacationDays { get; set; }
        public int SickDays { get; set; }
    }
}
