using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class StatisticsItemViewModel : ViewModelBase
    {
        public List<Request> Requests { get; set; }
        public bool FrequentlyBought { get; set; }
        public bool HighestPrice { get; set; }
        public List<CategoryType> CategoryTypes { get; set; }
        public List<CategoryType> CategoryTypesSelected { get; set; }
        public List<int> Months { get; set; }
        public List<int> Years { get; set; }
    }
}
