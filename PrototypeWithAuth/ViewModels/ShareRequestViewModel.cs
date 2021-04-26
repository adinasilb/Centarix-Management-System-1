using Org.BouncyCastle.Asn1.Ocsp;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrototypeWithAuth.ViewModels
{
    public class ShareRequestViewModel
    {
        public PrototypeWithAuth.Models.Request Request { get; set; }
        public List<SelectListItem> ApplicationUsers { get; set; }
        public String ApplicationUserID { get; set; }
    }
}
