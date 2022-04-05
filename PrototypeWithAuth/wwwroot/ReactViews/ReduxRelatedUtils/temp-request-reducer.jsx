import * as ActionKeys from './actions.jsx'
import undoable from 'redux-undo'

const tempRequestReducer = (state = [], action) => {

    switch (action.type) {
        case ActionKeys.SET_TEMP_REQUEST_LIST:
            return action.payload;
            break;
        default:
            return state;
    }
};

const undoableReducer = undoable(tempRequestReducer)

export default undoableReducer;