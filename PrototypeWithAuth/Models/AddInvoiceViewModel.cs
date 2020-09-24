using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class AddInvoiceViewModel
    {
        public List<Request> Requests { get; set; }
        public Invoice Invoice { get; set; }
        public IFormFile InvoiceImage { get; set; }
        public string InvoiceImageSaved { get; set; }
    }
}
