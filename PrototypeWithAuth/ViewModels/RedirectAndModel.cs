using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RedirectAndModel
    {
        public RedirectToActionResult RedirectToActionResult { get; set; }
        public TermsViewModel TermsViewModel { get; set; }
    }
}
