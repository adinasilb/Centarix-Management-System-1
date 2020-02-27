using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestItemViewModel
    {
        public IEnumerable<Request> Requests { get; set; }
        public Product Product { get; set; }
        public Vendor Vendor { get; set; }
    }
}
