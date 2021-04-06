using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class SelectedFilters
    {
        public List<int> SelectedTypesIDs { get; set; }
        public List<int> SelectedVendorsIDs { get; set; }
        public List<string> SelectedOwnersIDs { get; set; }
        public List<int> SelectedLocationsIDs { get; set; }
        public List<int> SelectedCategoriesIDs { get; set; }
        public List<int> SelectedSubcategoriesIDs { get; set; }
    }
}
