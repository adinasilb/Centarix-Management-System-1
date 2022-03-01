using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class NotificationFilterViewModel
    {
        public List<Vendor> Vendors { get; set; }
        public int? SelectedVendor { get; set; }
        public string NameOrCentarixOrderNumber { get; set; }
        //public string ProductName { get; set; }
    }
}
