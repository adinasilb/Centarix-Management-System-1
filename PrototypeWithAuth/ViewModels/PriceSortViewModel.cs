using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class PriceSortViewModel : ViewModelBase
    {
        public AppUtility.PriceSortEnum PriceSortEnum { get; set; }
        public bool Selected { get; set; }
    }
}
