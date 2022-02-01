using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class EmployeeInfoNotification : Notification<EmployeeInfoNotificationStatus>, ModelBase
    {
        public string EmployeeID { get; set; }
        public Employee Employee { get; set; }
    }
}
