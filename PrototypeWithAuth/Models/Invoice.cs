using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Range(1, 2147483647, ErrorMessage = "Field must be a number")]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }
        public List<Request> Requests { get; set; }
    }
}
