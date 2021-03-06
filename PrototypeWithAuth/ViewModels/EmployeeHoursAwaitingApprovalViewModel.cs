using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class EmployeeHoursAwaitingApprovalViewModel : ViewModelBase
    {
        public  bool Entry1 { get; set; }
        public bool Entry2 { get; set; }
        public bool Exit1 { get; set; }
        public bool Exit2 { get; set; }
        public bool TotalHours { get; set; }
        public bool PartialHours { get; set; }
        public EmployeeHoursAwaitingApproval EmployeeHoursAwaitingApproval { get; set; }
    }
}
