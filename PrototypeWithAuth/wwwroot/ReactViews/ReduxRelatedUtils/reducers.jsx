import { combineReducers } from 'redux'
import indexTableReducer from './index-table-reducer.jsx'
import modalsReducer from './modals-reducer.jsx'
import tempRequestReducer from './temp-request-reducer.jsx'

export default combineReducers({
    viewModel: indexTableReducer,
    modals: modalsReducer,
    tempRequestList: tempRequestReducer
})
