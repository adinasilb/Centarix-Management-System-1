import * as ActionKeys from './actions.jsx'
import undoable from 'redux-undo'

const tempRequestJsonReducer = (state = [], action) => {

    switch (action.type) {
        case ActionKeys.SET_TEMP_REQUEST_JSON:
            console.log("set temp request Json")
            return {
                ...state,
                tempRequestJson: action.payload
            };
            break;
        default:
            return state;
    }
};

const undoableReducer = undoable(tempRequestJsonReducer)

export default undoableReducer;