﻿using System.Collections.Generic;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class SelectedFilters
    {
        public int NumFilters { get; set; }

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
     
        private List<int> _selectedCategoriesIDs;
        public List<int> SelectedCategoriesIDs {
            get
            {
                if (_selectedCategoriesIDs == null)
                {
                    return new List<int>();
                }
                else
                {
                    return _selectedCategoriesIDs;
                }
            }
            set
            {
                _selectedCategoriesIDs = value;
            }
        }
        private List<int> _selectedSubcategoriesIDs;
        public List<int> SelectedSubcategoriesIDs {
            get
            {
                if (_selectedSubcategoriesIDs == null)
                {
                    return new List<int>();
                }
                else
                {
                    return _selectedSubcategoriesIDs;
                }
            }
            set
            {
                _selectedSubcategoriesIDs = value;
            }
        }
    }
}
