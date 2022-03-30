using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels.ReactUtilityModels
{
    public class IndexTableJsonViewModel
    {
        public SelectedRequestFilters SelectedFilters { get; set; }
        public NavigationInfo NavigationInfo { get; set; }
        public string TabValue { get; set; }
        public int PageNumber { get; set; }
        public AppUtility.CurrencyEnum SelectedCurrency { get; set; }
        public List<AppUtility.PriceSortEnum> PriceSortEnums { get; set; }
        public bool CategorySelected { get; set; }
        public bool SubcategorySelected { get; set; }
        public bool SourceSelected { get; set; }
    }
}
