using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class WorkerHourViewModel : ViewModelBase
    {
        public Employee Employee { get; set; }
        public TimeSpan Hours { get; set; }
        public int WorkingDays { get; set; }
        public double VacationDays { get; set; }
        public double SickDays { get; set; }
        public double VacationSickCount { get; set; }
        public int MissingDays { get; set; }
    }
}
