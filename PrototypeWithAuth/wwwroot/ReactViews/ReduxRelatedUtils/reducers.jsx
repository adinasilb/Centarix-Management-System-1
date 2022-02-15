import { SET_INDEX_TABLE_VIEWMODEL } from './actions.jsx'
import { OPEN_CUSTOM_FIELD } from './actions.jsx'

const reducer = (state, action) => {
    console.log("in reducer");

    switch (action.type) {
        case SET_INDEX_TABLE_VIEWMODEL:
            console.log("in reducer right key")
            return {
                ...state,
                viewModel: action.viewmodel
            };
        case OPEN_CUSTOM_FIELD:
            console.log("in reducer open custom field");
            return {
                ...state,
                viewModel: action.viewModel
            }
        default:
            return state;
    }
};

export default reducer;