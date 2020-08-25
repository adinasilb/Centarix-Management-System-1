using Castle.Components.DictionaryAdapter;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace PrototypeWithAuth.Models
{
    public abstract class Notification<T> where T: NotificationStatus
    {
        [Key]
        public int NotificationID { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; }
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser {get; set;}
        public string Controller { get; set; }
        public string Action { get; set; }
        public int NotificationStatusID { get; set; }
        public T NotificationStatus { get; set; }
    }
}
