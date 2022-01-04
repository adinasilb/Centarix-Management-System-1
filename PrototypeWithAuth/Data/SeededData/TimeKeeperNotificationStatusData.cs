using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class TimeKeeperNotificationStatusData
    {
        public static List<TimekeeperNotificationStatus> Get()
        {
            List<TimekeeperNotificationStatus> list = new List<TimekeeperNotificationStatus>();
            list.Add(new TimekeeperNotificationStatus
            {
                NotificationStatusID = 5,
                Icon = "icon-notification_timekeeper-24px",
                Color = "--timekeeper-color",
                Description = "UpdateHours"
            });
            return list;
        }
    }
}
