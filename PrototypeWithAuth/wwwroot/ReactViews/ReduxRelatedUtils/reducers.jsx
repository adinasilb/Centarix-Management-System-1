import { combineReducers } from 'redux'
import { SET_RELOAD_INDEX } from './actions.jsx'
import indexTableReducer, { categoryPopoverReducer, pricePopoverReducer, tabValueReducer, selectedFiltersReducer, setPageNumberReducer, setReloadIndexReducer } from './index-table-reducer.jsx'
import modalsReducer from './modals-reducer.jsx'
import tempRequestReducer from './temp-request-reducer.jsx'

export default combineReducers({
    viewModel: indexTableReducer,
    modals: modalsReducer,
    navigationInfo: (state = {}, action) => {
        return state;
    },
    categoryPopoverViewModel: categoryPopoverReducer,
    pricePopoverViewModel: pricePopoverReducer,
    tabValue: tabValueReducer,
    selectedFilters: selectedFiltersReducer,
    tempRequestList: tempRequestReducer,
    pageNumber: setPageNumberReducer,
    reloadIndex: setReloadIndexReducer
})
