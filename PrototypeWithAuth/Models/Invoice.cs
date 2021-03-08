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

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "field must be a number")]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDate_submit { get { return InvoiceDate; } set { InvoiceDate = value; } }
        public List<Request> Requests { get; set; }
    }
}
