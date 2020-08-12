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
        public IEnumerable<Request> Requests { get; set; }
        public string ApplicationUserID { get; set; } //this is the owner of the request - do we have every received request have its own reciever?

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Quote Date")]
        //should not really be null just waiting till figure out how else to do the parentquote in create modal
        public DateTime QuoteDate { get; set; }
        [Display(Name = "Quote Number")]
        public bool IsDeleted { get; set; } //will be set to true if all requests under parent are delted
        public int QuoteNumber { get; set; }
        public int QuoteStatusID { get; set; }
        [ForeignKey("QuoteStatusID")]
        public QuoteStatus QuoteStatus { get; set; }
    }
}
