using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialRowViewModel
    {
        public IEnumerable<RequestIndexPartialColumnViewModel> Columns { get; set; }
        public Vendor Vendor { get; set; }
        public decimal TotalCost { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal ExchangeRate { get; set; }
        public string ButtonClasses {get ;set;}
        public string ButtonText { get; set; }

    }
}
