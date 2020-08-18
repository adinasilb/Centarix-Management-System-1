using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class PaymentStatus
    {
        [Key]
        public int PaymentStatusID { get; set; }
        public string PaymentStatusDescription { get; set; }
        public IEnumerable<Request> Requests { get; set; }
    }
}
