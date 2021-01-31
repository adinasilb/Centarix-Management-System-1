using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ConfirmQuoteEmailViewModel
    {
        public List<Request> Requests { get; set; }
        public int VendorId { get; set; }
        public int RequestID { get; set; }
        public bool IsResend { get; set; }
    }
}
