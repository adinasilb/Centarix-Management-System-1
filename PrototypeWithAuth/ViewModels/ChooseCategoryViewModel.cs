using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ChooseCategoryViewModel
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public int SelectedCategory { get; set; }
        public bool isRequestQuote { get; set; }
    }
}
