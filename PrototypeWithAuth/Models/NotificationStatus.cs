
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class NotificationStatus
    {
        public int id { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
    }
}
