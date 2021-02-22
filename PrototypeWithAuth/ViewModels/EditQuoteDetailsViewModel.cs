using Microsoft.AspNetCore.Http;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class EditQuoteDetailsViewModel : ViewModelBase
    {
        public List<Request> Requests { get; set; }
        [DataType(DataType.Date)]
        public Vendor Vendor { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Quote Date")]
        public DateTime QuoteDate { get; set; }
        [Display(Name = "Quote Number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "field must be a number")]
        public string QuoteNumber { get; set; }
        public IFormFile QuoteFileUpload { get; set; }
        public int? ParentQuoteID { get; set; }

    }
}
