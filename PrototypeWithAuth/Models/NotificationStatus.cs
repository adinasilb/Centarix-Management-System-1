
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class NotificationStatus
    {
        [Key]
        public int NotificationStatusID { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
       
    }
}
