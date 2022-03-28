export const SET_INDEX_TABLE_VIEWMODEL = 'SET_INDEX_TABLE_VIEWMODEL'
export const SET_CATEGORY_POPOVER_VIEWMODEL = 'SET_CATEGORY_POPOVER_VIEWMODEL'
export const SET_PRICE_POPOVER_VIEWMODEL = 'SET_PRICE_POPOVER_VIEWMODEL'
export const SET_TAB_VALUE = 'SET_TAB_VALUE'
export const SET_HEADER_INFO = 'SET_HEADER_INFO'
export const SET_SELECTED_FILTERS_VIEWMODEL = 'SET_SELECTED_FILTERS_VIEWMODEL'
export const ADD_MODAL = 'ADD_MODAL'
export const REMOVE_MODAL = 'REMOVE_MODAL'
export const REMOVE_MODALS = 'REMOVE_MODALS'
export const SET_PAGE_NUMBER = 'SET_PAGE_NUMBER'
export const SET_TEMP_REQUEST_LIST = 'SET_TEMP_REQUEST_LIST'
export const SET_RELOAD_INDEX = 'SET_RELOAD_INDEX'
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
export const setTabValue = (tabValue) => {
    return ({

        type: SET_TAB_VALUE,
        payload: tabValue
    })
};
export const setSelectedFiltersViewModel = (viewmodel) => {
    return ({

        type: SET_SELECTED_FILTERS_VIEWMODEL,
        payload: viewmodel
    })
};

export const setPageNumber = (viewmodel) => {
    return ({

        type: SET_PAGE_NUMBER,
        payload: viewmodel
    })
};

export const setReloadIndex = (viewmodel) => {
    return ({

        type: SET_RELOAD_INDEX,
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