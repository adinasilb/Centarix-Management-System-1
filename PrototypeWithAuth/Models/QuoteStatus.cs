﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class QuoteStatus
    {
        [Key]
        public int QuoteStatusID { get; set; }
        public string QuoteStatusDescription { get; set; }
        public IEnumerable<ParentQuote> ParentQuotes { get; set; }
    }
}
