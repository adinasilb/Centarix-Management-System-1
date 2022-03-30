import { combineReducers } from 'redux'
import { setCategorySelected, setPriceSortEnums, setSubCategorySelected, SET_RELOAD_INDEX } from './actions.jsx'
import indexTableReducer, {  tabValueReducer, selectedFiltersReducer, setPageNumberReducer, setReloadIndexReducer, setPriceSortEnum, setSelectedCurrency, setSelectedCategory, setSelectedSubCategory, setSourceSelected } from './index-table-reducer.jsx'
import modalsReducer from './modals-reducer.jsx'
import tempRequestReducer from './temp-request-reducer.jsx'

export default combineReducers({
    viewModel: indexTableReducer,
    modals: modalsReducer,
    navigationInfo: (state = {}, action) => {
        return state;
    },
    selectedCurrency: setSelectedCurrency,
    priceSortEnums: setPriceSortEnum,
    tabValue: tabValueReducer,
    selectedFilters: selectedFiltersReducer,
    tempRequestList: tempRequestReducer,
    pageNumber: setPageNumberReducer,
    reloadIndex: setReloadIndexReducer,
    sourceSelected: setSourceSelected,
    subcategorySelected: setSelectedSubCategory,
    categorySelected: setSelectedCategory
})
