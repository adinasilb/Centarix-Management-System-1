﻿import { combineReducers } from 'redux'
import indexTableReducer, { categoryPopoverReducer, pricePopoverReducer, tabInfoReducer, inventoryFilterReducer } from './index-table-reducer.jsx'
import modalsReducer from './modals-reducer.jsx'
import tempRequestJsonReducer from './temp-request-json-reducer.jsx'

export default combineReducers({
    viewModel: indexTableReducer,
    modals: modalsReducer,
    navigationInfo: (state = {}, action) => {
        return state;
    },
    tempRequestJson: tempRequestJsonReducer,
    categoryPopoverViewModel: categoryPopoverReducer,
    pricePopoverViewModel: pricePopoverReducer,
    tabInfo: tabInfoReducer,
    inventoryFilterViewModel: inventoryFilterReducer
})
