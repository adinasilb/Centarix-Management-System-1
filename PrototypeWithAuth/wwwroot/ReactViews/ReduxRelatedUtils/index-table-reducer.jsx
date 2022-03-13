import * as ActionKeys from './actions.jsx'

const indexTableReducer = (state = {}, action) => {

    switch (action.type) {
        case ActionKeys.SET_INDEX_TABLE_VIEWMODEL:
            console.log("in reducer")
            return {
                ...state,
                viewModel: action.payload
            };
            break;
        default:
            return state;
    }
};

export default indexTableReducer;