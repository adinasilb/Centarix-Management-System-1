using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UpdateHoursViewModel : ViewModelBase
    {
        public EmployeeHours EmployeeHour { get; set; }
        public String PageType { get; set; }
        public bool IsForgotToReport { get; set; }
        public int AutoFillEntry1Type { get; set; }
        public IEnumerable<OffDayType> PartialOffDayTypes {get; set;}
    }
}
