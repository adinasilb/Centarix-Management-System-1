import { SET_INDEX_TABLE_VIEWMODEL } from './actions.jsx'

const reducer = (state, action) => {
    console.log("in reducer");

    switch (action.type) {
        case SET_INDEX_TABLE_VIEWMODEL:
            console.log("in reducer right key")
            return {
                ...state,
                viewModel: action.viewmodel
            };
      
        default:
            return state;
    }
};

export default reducer;