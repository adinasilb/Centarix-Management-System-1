using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class EmployeeInfoNotificationStatus: NotificationStatus
    {
        public IEnumerable<EmployeeInfoNotification> EmployeeInfoNotifications { get; set; }
    }
}
