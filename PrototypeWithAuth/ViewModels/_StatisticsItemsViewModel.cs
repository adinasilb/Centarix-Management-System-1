using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class _StatisticsItemsViewModel
    {
        public RequestItemViewModel RequestItemViewModel { get; set; }
        public List<CategoryType> CategoryTypes { get; set; }
        public List<CategoryType> CategoryTypesSelected { get; set; }
        public List<int> Months { get; set; }
        public List<int> Years { get; set; }
    }
}
