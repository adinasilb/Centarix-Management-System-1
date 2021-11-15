using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class RequestNotificationStatusData
    {
        public static List<RequestNotificationStatus> Get()
        {
            List<RequestNotificationStatus> list = new List<RequestNotificationStatus>();
            list.Add(new RequestNotificationStatus
            {
                NotificationStatusID = 1,
                Icon = "icon-priority_high-24px",
                Color = "--notifications-orderlate-color",
                Description = "OrderLate"
            });
            list.Add(new RequestNotificationStatus
            {
                NotificationStatusID = 2,
                Icon = "icon-centarix-icons-03",
                Color = "--notifications-ordered-color",
                Description = "ItemOrdered"
            });
            list.Add(new RequestNotificationStatus
            {
                NotificationStatusID = 3,
                Icon = "icon-done-24px",
                Color = "--notifications-approved-color",
                Description = "ItemApproved"
            });
            list.Add(new RequestNotificationStatus
            {
                NotificationStatusID = 4,
                Icon = "icon-local_mall-24px",
                Color = "--notifications-received-color",
                Description = "ItemReceived"
            });
            return list;
        }
    }
}
