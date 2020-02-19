using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class RequestStatus
    {
        [Key]
        public int RequestStatusID { get; set; }
        public string RequestStatusDescription { get; set; }
        public IEnumerable<Request> Requests { get; set; }
    }
}
