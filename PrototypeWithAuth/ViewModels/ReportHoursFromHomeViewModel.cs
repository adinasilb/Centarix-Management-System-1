using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ReportHoursFromHomeViewModel : ViewModelBase
    {
        public EmployeeHoursAwaitingApproval EmployeeHour { get; set; }
        public String PageType { get; set; }
    }
}
