using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ConfirmQuoteOrderEmailViewModel
    {
        public List<Quote> Requests { get; set; }
        public int VendorID { get; set; }
        public int RequestID { get; set; }
        public bool IsResend { get; set; }
    }
}
