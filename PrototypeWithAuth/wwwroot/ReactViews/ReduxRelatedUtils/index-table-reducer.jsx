import * as ActionKeys from './actions.jsx'

const indexTableReducer = (state = {}, action) => {
    switch (action.type) {
        case ActionKeys.SET_INDEX_TABLE_VIEWMODEL:
            return action.payload;
            break;
        default:
            return state ;
    }
};

export const categoryPopoverReducer = (state = {}, action) => {
    switch (action.type) {
        case ActionKeys.SET_CATEGORY_POPOVER_VIEWMODEL:
            return action.payload;
            break;
        default:
            return state;
    }
};

export const pricePopoverReducer = (state = {}, action) => {
    switch (action.type) {
        case ActionKeys.SET_PRICE_POPOVER_VIEWMODEL:
            return action.payload;
            break;
        default:
            return state;
    }
};
export const tabValueReducer = (state = {}, action) => {
    switch (action.type) {
        case ActionKeys.SET_TAB_VALUE:
            return action.payload;
            break;
        default:
            return state;
    }
};
export const selectedFiltersReducer = (state = {}, action) => {
    console.log(action.type)
    switch (action.type) {
        case ActionKeys.SET_SELECTED_FILTERS_VIEWMODEL:
            return action.payload;
            break;
        case ActionKeys.SET_SEARCH_TEXT:
            return { ...state, SearchText: action.payload }
        default:
            return state;
    }
};

export const setPageNumberReducer = (state = {}, action) => {
    switch (action.type) {
        case ActionKeys.SET_PAGE_NUMBER:
            return action.payload;
            break;
        default:
            return state;
    }
};

export const setReloadIndexReducer = (state = {}, action) => {
    switch (action.type) {
        case ActionKeys.SET_RELOAD_INDEX:
            return action.payload;
            break;
        default:
            return state;
    }
};


export default indexTableReducer;