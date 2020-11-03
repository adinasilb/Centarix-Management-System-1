using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    //currently
    public class ParentRequest
    {
        [Key]
        public int ParentRequestID { get; set; }
        public IEnumerable<Request> Requests { get; set; }
        public string ApplicationUserID { get; set; } //this is the person who placed the order

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        
        [Display(Name = "Order Number")]
        public int? OrderNumber { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "field must be a number")]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }
        public IEnumerable<Payment> Payments { get; set; }
        public bool IsDeleted { get; set; } //this is set to true if all the requests under this parentrequest are deleted
        public bool WithoutOrder { get; set; }
        public double Discount { get; set; }
        public double Taxes { get; set; }
        public double Credit { get; set; }
        public double Shipping { get; set; }
    }
}
//list of request, owner, order date, (order number for an entire order or is it just for specific request) same for invoice number