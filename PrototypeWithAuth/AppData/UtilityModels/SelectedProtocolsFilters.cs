using System.Collections.Generic;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class SelectedProtocolsFilters:SelectedFilters
    {

    
        private List<int> _selectedTypesIDs;
        public List<int> SelectedTypesIDs
        {
            get
            {
                if (_selectedTypesIDs == null)
                {
                    return new List<int>();
                }
                else
                {
                    return _selectedTypesIDs;
                }
            }
            set
            {
                _selectedTypesIDs = value;
            }
        }
    }
}
