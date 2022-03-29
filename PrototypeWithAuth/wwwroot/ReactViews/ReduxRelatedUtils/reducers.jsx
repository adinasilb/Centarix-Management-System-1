import { combineReducers } from 'redux'
import { setPriceSortEnums, SET_RELOAD_INDEX } from './actions.jsx'
import indexTableReducer, { categoryPopoverReducer,  tabValueReducer, selectedFiltersReducer, setPageNumberReducer, setReloadIndexReducer, setPriceSortEnum, setSelectedCurrency } from './index-table-reducer.jsx'
import modalsReducer from './modals-reducer.jsx'
import tempRequestReducer from './temp-request-reducer.jsx'

export default combineReducers({
    viewModel: indexTableReducer,
    modals: modalsReducer,
    navigationInfo: (state = {}, action) => {
        return state;
    },
    categoryPopoverViewModel: categoryPopoverReducer,
    selectedCurrency: setSelectedCurrency,
    priceSortEnums: setPriceSortEnum,
    tabValue: tabValueReducer,
    selectedFilters: selectedFiltersReducer,
    tempRequestList: tempRequestReducer,
    pageNumber: setPageNumberReducer,
    reloadIndex: setReloadIndexReducer
})
