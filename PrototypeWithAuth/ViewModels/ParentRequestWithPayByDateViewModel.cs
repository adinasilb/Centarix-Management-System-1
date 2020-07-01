using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ParentRequestWithPayByDateViewModel
    {
        public ParentRequest ParentRequest { get; set; }
        public DateTime? PayByDate { get; set; }
    }
}
