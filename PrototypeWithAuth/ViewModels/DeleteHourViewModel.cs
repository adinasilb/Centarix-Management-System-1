using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class DeleteHourViewModel : ViewModelBase
    {
        
        public AppUtility.MenuItems SectionType { get; set; }
        public EmployeeHours EmployeeHour { get; set; }
        public EmployeeHoursAwaitingApproval Ehaa { get; set; }
    }
}
