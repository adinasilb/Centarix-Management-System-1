using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TimekeeperNotification : Notification<TimekeeperNotificationStatus>, ModelBase
    {

        public int EmployeeHoursID { get; set; }
        public EmployeeHours EmployeeHours { get; set; }
    } 
}
