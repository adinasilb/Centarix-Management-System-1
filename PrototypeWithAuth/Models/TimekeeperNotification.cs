using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TimekeeperNotification : Notification<TimekeeperNotificationStatus>
    {

        public int EmployeeHoursID { get; set; }
    } 
}
