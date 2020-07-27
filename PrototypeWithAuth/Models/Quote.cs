using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Quote : Request
    {
      
        public int ParentQuoteID { get; set; }
        public ParentQuote ParentQuote { get; set; }
        public int QuoteStatusID { get; set; }
        public QuoteStatus QuoteStatus { get; set; }
    }
}
