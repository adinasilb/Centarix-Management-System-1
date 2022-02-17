import * as ActionKeys from './actions.jsx'

const reducer = (state, action) => {

    switch (action.type) {
        case ActionKeys.SET_INDEX_TABLE_VIEWMODEL:
       
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
            action.payload.map(item => {
                newState = newState.filter(m => m != item)
            })
            console.log("remove modals" + newState)
            return {
                ...state,
                modals: newState
            };
            break;
        default:
            return state;
    }
};

export default reducer;