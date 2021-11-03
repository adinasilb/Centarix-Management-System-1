using System.Collections.Generic;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class SelectedRequestFilters:SelectedFilters
    {
        public bool Archived { get; set; }
        public string CatalogNumber { get; set; }
        //public List<int> SelectedTypesIDs { get; set; }
        private List<int> _selectedVendorsIDs;
        public List<int> SelectedVendorsIDs 
        {
            get
            {
                if (_selectedVendorsIDs == null)
                {
                    return new List<int>();
                } 
                else
                {
                    return _selectedVendorsIDs;
                }
            }
            set 
            {
                _selectedVendorsIDs = value;
            } 
        }
    
        private List<int> _selectedLocationsIDs;
        public List<int> SelectedLocationsIDs {
            get
            {
                if (_selectedLocationsIDs == null)
                {
                    return new List<int>();
                }
                else
                {
                    return _selectedLocationsIDs;
                }
            }
            set
            {
                _selectedLocationsIDs = value;
            }
        }

        private string _searchText;
        public string SearchText
        {
            get
            {
                if (_searchText == null)
                {
                    return "";
                }
                else
                {
                    return _searchText;
                }
            }
            set { _searchText = value; }
        }

    }
}
