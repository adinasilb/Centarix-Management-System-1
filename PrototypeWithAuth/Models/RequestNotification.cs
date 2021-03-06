using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RequestNotification : Notification<RequestNotificationStatus>, ModelBase
    {
        public int RequestID { get; set; }
        public Request Request { get; set; }
        public string RequestName { get; set; }
        public string Vendor { get; set; }
    }
}
