using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class StatisticsSubCategoryViewModel : ViewModelBase
    {
        public Dictionary<ProductSubcategory, List<Request>> ProductSubcategories { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
