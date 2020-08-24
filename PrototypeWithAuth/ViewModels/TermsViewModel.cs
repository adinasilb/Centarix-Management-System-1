using System;
using System.Collections.Generic;
using PrototypeWithAuth.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrototypeWithAuth.ViewModels
{
    public class TermsViewModel
    {
        public ParentRequest ParentRequest { get; set; }
        public bool Paid { get; set; }
        public string Discount { get; set; }
        public string Terms { get; set; }
        public List<SelectListItem> TermsList { get; set; }
        //public int Taxes { get; set; } //does this go here?
        //public int Shipping { get; set; }
        //public int Credit { get; set; }
        public int Installments { get; set; }
        public List<Payment> NewPayments { get; set; }
    }
}
