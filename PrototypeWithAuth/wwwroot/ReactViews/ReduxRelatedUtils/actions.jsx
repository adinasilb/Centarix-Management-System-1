export const SET_INDEX_TABLE_VIEWMODEL = 'SET_INDEX_TABLE_VIEWMODEL'
export const SET_CATEGORY_POPOVER_VIEWMODEL = 'SET_CATEGORY_POPOVER_VIEWMODEL'
export const SET_PRICE_POPOVER_VIEWMODEL = 'SET_PRICE_POPOVER_VIEWMODEL'
export const SET_TAB_INFO = 'SET_TAB_INFO'
export const SET_HEADER_INFO = 'SET_HEADER_INFO'
export const SET_INVENTORY_FILTER_VIEWMODEL = 'SET_INVENTORY_FILTER_VIEWMODEL'
export const ADD_MODAL = 'ADD_MODAL'
export const REMOVE_MODAL = 'REMOVE_MODAL'
export const REMOVE_MODALS = 'REMOVE_MODALS'
export const SET_TEMP_REQUEST_LIST = 'SET_TEMP_REQUEST_LIST'

export const setIndexTableViewModel = (viewmodel ) => {
    return ({

        type: SET_INDEX_TABLE_VIEWMODEL,
        payload: viewmodel 
    })
};
export const setCategoryPopoverViewModel = (viewmodel) => {
    return ({

        type: SET_CATEGORY_POPOVER_VIEWMODEL,
        payload: viewmodel
    })
}; 
export const setPricePopoverViewModel = (viewmodel) => {
    return ({

        type: SET_PRICE_POPOVER_VIEWMODEL,
        payload: viewmodel
    })
};
export const setTabInfo = (tabValue) => {
    return ({

        type: SET_TAB_INFO,
        payload: tabValue
    })
};
export const setInventoryFilterViewModel = (viewmodel) => {
    return ({

        type: SET_INVENTORY_FILTER_VIEWMODEL,
        payload: viewmodel
    })
};
export const setHeaderInfo = (viewmodel) => {
    return ({

        type: SET_HEADER_INFO,
        payload: viewmodel
    })
};
export const addModal = (modal) => {
    return ({

        type: ADD_MODAL,
        payload:  modal
    })
};

export const removeModal = (modal) => {
    return ({

        type: REMOVE_MODAL,
        payload: modal
    })
};

export const removeModals = (modals) => {
    console.log("in remove modals");
    return ({

        type: REMOVE_MODALS,
        payload:  modals 
    })
};

export const setTempRequestList = (tempRequestList) => {
    return ({

        type: SET_TEMP_REQUEST_LIST,
        payload: tempRequestList
    })
};