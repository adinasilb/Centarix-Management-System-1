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

        public string ApplicationUserID { get; set; } //this is the owner of the request - do we have every received request have its own reciever?

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        public int? OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }
        public bool WithOrder { get; set; } //do we need this here?
        public double Discount { get; set; }
        public bool Payed { get; set; }

        public uint Installments { get; set; } //number of installments

        public IEnumerable<Payment> Payments { get; set; }
    }
}
//list of request, owner, order date, (order number for an entire order or is it just for specific request) same for invoice number