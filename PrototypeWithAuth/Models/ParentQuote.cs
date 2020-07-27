using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ParentQuote
    {
        [Key]
        public int ParentQuoteID { get; set; }
        public IEnumerable<Quote> Quotes { get; set; }
        public string ApplicationUserID { get; set; } //this is the owner of the request - do we have every received request have its own reciever?

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.Date)]
        public DateTime QuoteDate { get; set; }
        public int? QuoteNumber { get; set; }
    }
}
