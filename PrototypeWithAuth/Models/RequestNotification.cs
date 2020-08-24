using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RequestNotification : Notification
    {
        int RequestID { get; set; }
        int RequestName { get; set; }
        int RequestStatusID { get; set; }
        RequestStatus RequestStatus { get; set; }
    }
}
