using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class VendorContactWithDeleteViewModel : ViewModelBase
    {
        public VendorContact VendorContact { get; set; }
        public bool Delete { get; set; }
    }
}
