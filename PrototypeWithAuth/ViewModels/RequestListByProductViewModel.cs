using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestListByProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Request> Requests { get; set; }
    }
}
