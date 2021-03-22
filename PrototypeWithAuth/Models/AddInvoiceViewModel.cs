using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Models
{
    public class AddInvoiceViewModel : ViewModelBase
    {
        public List<Request> Requests { get; set; }
        public Invoice Invoice { get; set; }
        public IFormFile InvoiceImage { get; set; }
        public string InvoiceImageSaved { get; set; }
    }
}
