using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SettingsForm
    {
        public CategoryBase Category { get; set; }
        public int ItemCount { get; set; }
        public int RequestCount { get; set; }
        public string CategoryDescription { get; set; }
        public string SubcategoryDescription { get; set; }
        public List<bool> Test { get; set; }
        public IEnumerable<CustomDataType> CustomFieldData { get; set; }
        public List<UnitParentType> UnitParentTypes { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
