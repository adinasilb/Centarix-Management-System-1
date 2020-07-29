using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ConfirmQuoteEmailViewModel
    {
        public List<Quote> Requests { get; set; }
        public int VendorID { get; set; }
    }
}
