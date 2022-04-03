using Microsoft.AspNetCore.Http;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class InvoiceInfoViewModel
    {
        public bool CurrentPayment { get; set; }
        [Display(Name = "Invoice Image")]
        public IFormFile InvoiceImage { get; set; }
        public Invoice Invoice { get; set; }
    }
}
