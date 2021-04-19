using System.Collections.Generic;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class SelectedProtocolsFilters
    {

        private List<string> _selectedOwnersIDs;
        public List<string> SelectedOwnersIDs {
            get
            {
                if (_selectedOwnersIDs == null)
                {
                    return new List<string>();
                }
                else
                {
                    return _selectedOwnersIDs;
                }
            }
            set 
            {
                _selectedOwnersIDs = value;
            }
        }
        List<int> _selectedProtocolsCategoriesIDs;
        public List<int> SelectedCategoriesIDs {
            get
            {
                if (_selectedProtocolsCategoriesIDs == null)
                {
                    return new List<int>();
                }
                else
                {
                    return _selectedProtocolsCategoriesIDs;
                }
            }
            set
            {
                _selectedProtocolsCategoriesIDs = value;
            }
        }
        private List<int> _selectedProtocolsSubcategoriesIDs;
        public List<int> SelectedProtocolsSubcategoriesIDs {
            get
            {
                if (_selectedProtocolsSubcategoriesIDs == null)
                {
                    return new List<int>();
                }
                else
                {
                    return _selectedProtocolsSubcategoriesIDs;
                }
            }
            set
            {
                _selectedProtocolsSubcategoriesIDs = value;
            }
        }
    }
}
