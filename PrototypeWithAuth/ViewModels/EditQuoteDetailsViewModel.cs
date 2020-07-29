using Microsoft.AspNetCore.Http;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class EditQuoteDetailsViewModel
    {
        public List<Quote> Quotes { get; set; }
        [DataType(DataType.Date)]
        public Vendor Vendor { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Quote Date")]
        public DateTime QuoteDate { get; set; }
        [Display(Name = "Quote Number")]
        public int? QuoteNumber { get; set; }
        public IFormFile QuoteFileUpload { get; set; }
        public int ParentQuoteID { get; set; }

    }
}
