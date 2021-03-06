using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RequestListRequest : ModelBase
    {
        [Key]
        public int ListID { get; set; }
        public RequestList List { get; set; }
        public int RequestID { get; set; }
        public Request Request { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
