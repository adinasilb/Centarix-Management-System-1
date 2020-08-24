using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RequestNotification : Notification
    {
        public int RequestID { get; set; }
        public string RequestName { get; set; }
        public int RequestStatusID { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}
