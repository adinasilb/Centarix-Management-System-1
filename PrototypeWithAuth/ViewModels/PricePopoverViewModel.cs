using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class PricePopoverViewModel
    {
        public List<PriceSortViewModel> PriceSortEnums { get; set; }
        public List<String> PriceSortEnumsList { get; set; }

        private AppUtility.CurrencyEnum _selectedCurrency;
        public AppUtility.CurrencyEnum SelectedCurrency { 
            get {
                if(_selectedCurrency == AppUtility.CurrencyEnum.None)
                {
                    return AppUtility.CurrencyEnum.NIS;
                }
                else
                {
                    return _selectedCurrency;
                }
            }
            set
            {
                _selectedCurrency = value;
            }
        }
        public int PopoverSource { get; set; } //1 is in indextable, 2 is in history tab
    }
}
