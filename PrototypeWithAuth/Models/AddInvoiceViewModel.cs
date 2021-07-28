using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class AddInvoiceViewModel : ViewModelBase
    {
        public List<Request> Requests { get; set; }
        public Invoice Invoice { get; set; }
        [Display(Name = "Invoice Image")]
        public IFormFile InvoiceImage { get; set; }
        public string InvoiceImageSaved { get; set; }
        public Guid Guid { get; set; }
    }
}
