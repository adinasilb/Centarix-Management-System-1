using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class PriceSortViewModel
    {
        public AppUtility.PriceSortEnum PriceSortEnum { get; set; }
        public bool Selected { get; set; }
    }
}
