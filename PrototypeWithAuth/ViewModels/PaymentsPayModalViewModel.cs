using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class PaymentsPayModalViewModel
    {
        public List<Request> Requests { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
