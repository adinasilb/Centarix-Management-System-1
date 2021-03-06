using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class EmployeeInfoNotificationStatusData
    {
        public static List<EmployeeInfoNotificationStatus> Get()
        {
            List<EmployeeInfoNotificationStatus> list = new List<EmployeeInfoNotificationStatus>();
            list.Add(new EmployeeInfoNotificationStatus
            {
                NotificationStatusID = 6,
                Icon = "icon-cake_black_24dp",
                Color = "--black-87",
                Description = "Happy Birthday"
            });
            return list;
        }
    }
}
