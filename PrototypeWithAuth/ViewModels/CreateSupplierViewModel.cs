using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class CreateSupplierViewModel
    {
        public Vendor Vendor { get; set; }
        public List<VendorContact> VendorContacts { get; set; }
    }
}
