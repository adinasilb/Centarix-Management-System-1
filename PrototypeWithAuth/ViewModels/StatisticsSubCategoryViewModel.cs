using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class StatisticsSubCategoryViewModel
    {
        public Dictionary<ProductSubcategory, List<Request>> ProductSubcategories { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
