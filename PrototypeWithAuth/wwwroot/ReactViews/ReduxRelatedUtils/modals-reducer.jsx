import * as ActionKeys from './actions.jsx'

const modalsReducer = (state = [], action) => {

    switch (action.type) {
        case ActionKeys.ADD_MODAL:
            console.log("add modal")
            return [
                ...state, action.payload];
            break;
        case ActionKeys.REMOVE_MODAL:
            return state.filter(m => m != action.payload);
            break;
        case ActionKeys.REMOVE_MODALS:
            var newState = state;
            action.payload.map(item => {
                newState = newState.filter(m => m != item)
            })
            console.log("remove modals" + newState)
            return 
                newState;
            break;
        default:
            return [ ...state ];
    }
};

export default modalsReducer;