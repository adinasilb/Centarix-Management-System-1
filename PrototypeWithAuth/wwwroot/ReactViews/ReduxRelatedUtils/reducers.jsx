import { combineReducers } from 'redux'
import indexTableReducer from './index-table-reducer.jsx'
import modalsReducer from './modals-reducer.jsx'
import tempRequestJsonReducer from './temp-request-json-reducer.jsx'

export default combineReducers({
    viewModel: indexTableReducer,
    modals: modalsReducer,
    tempRequestJson: tempRequestJsonReducer
})
