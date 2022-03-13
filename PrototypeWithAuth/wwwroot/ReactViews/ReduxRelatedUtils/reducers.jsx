import * as ActionKeys from './actions.jsx'

const reducer = (state, action) => {

    switch (action.type) {
        case ActionKeys.SET_INDEX_TABLE_VIEWMODEL:
            console.log("in reducer")
            return {
                ...state,
                viewModel: action.payload
            };
            break;
        case ActionKeys.ADD_MODAL:
            return {
                ...state,
                modals: [...state.modals, action.payload]
            };
            break;
        case ActionKeys.REMOVE_MODAL:

            return {
                ...state,
                modals:state.modals.filter(m => m != action.payload)
            };
            break;
        case ActionKeys.REMOVE_MODALS:
            var newState = state.modals;
            action.payload?.map(item => {
                newState = newState.filter(m => m != item)
            })
            console.log("remove modals" + newState)
            return {
                ...state,
                modals: newState
            };
            break;
        case ActionKeys.SET_TEMP_REQUEST_JSON:
            return {
                ...state,
                tempRequestJson: [...state.tempRequestJson, action.payload]
            };
            break;
        default:
            return state;
    }
};

export default reducer;