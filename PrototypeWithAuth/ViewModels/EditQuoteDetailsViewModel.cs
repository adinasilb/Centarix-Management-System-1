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
        public IFormFile QuoteFileUpload { get; set; }
        public ParentQuote ParentQuote { get; set; }

    }
}
