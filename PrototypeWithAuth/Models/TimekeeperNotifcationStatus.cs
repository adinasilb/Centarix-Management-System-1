using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TimekeeperNotificationStatus: NotificationStatus
    {
        public IEnumerable<TimekeeperNotification> TimekeeperNotifications { get; set; }
    }
}
