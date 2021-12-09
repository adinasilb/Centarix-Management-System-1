using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RequestList : ModelBase
    {
        [Key]
        public int ListID { get; set; }
        public string Title { get; set; }
        public string ApplicationUserOwnerID { get; set; }
        public Employee ApplicationUserOwner { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDefault { get; set; }
        public IEnumerable<RequestListRequest> RequestListRequests { get; set; }
    }
}
