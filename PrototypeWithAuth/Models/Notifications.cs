using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Notifications
    {
        public int NotificationsID { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; }
    }
}
