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
    public class ShareRequestListViewModel
    {
        public PrototypeWithAuth.Models.ShareRequestList ShareRequestList { get; set; }
        public bool IsRemoved { get; set; }
    }
}
