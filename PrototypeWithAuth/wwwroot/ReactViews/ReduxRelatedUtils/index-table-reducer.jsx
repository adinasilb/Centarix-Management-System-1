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
export const tabInfoReducer = (state = {}, action) => {
    switch (action.type) {
        case ActionKeys.SET_TAB_INFO:
            return { ...state, tabValue: action.payload };
            break;
        default:
            return state;
    }
};
export const inventoryFilterReducer = (state = {}, action) => {
    console.log("inventoryFilterReducer")
    console.log(action.type)
    switch (action.type) {
        case ActionKeys.SET_INVENTORY_FILTER_VIEWMODEL:
            return action.payload;
            break;
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

export default indexTableReducer;