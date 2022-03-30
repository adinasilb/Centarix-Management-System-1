﻿import { set } from "date-fns/esm/fp"

export const SET_INDEX_TABLE_VIEWMODEL = 'SET_INDEX_TABLE_VIEWMODEL'
export const SET_CURRENCY_ENUM = 'SET_CURRENCY_ENUM'
export const SET_PRICE_SORT_ENUM = 'SET_PRICE_SORT_ENUM'
export const SET_TAB_VALUE = 'SET_TAB_VALUE'
export const SET_HEADER_INFO = 'SET_HEADER_INFO'
export const SET_SELECTED_FILTERS_VIEWMODEL = 'SET_SELECTED_FILTERS_VIEWMODEL'
export const ADD_MODAL = 'ADD_MODAL'
export const REMOVE_MODAL = 'REMOVE_MODAL'
export const REMOVE_MODALS = 'REMOVE_MODALS'
export const SET_PAGE_NUMBER = 'SET_PAGE_NUMBER'
export const SET_TEMP_REQUEST_LIST = 'SET_TEMP_REQUEST_LIST'
export const SET_RELOAD_INDEX = 'SET_RELOAD_INDEX'
export const SET_SEARCH_TEXT = 'SET_SEARCH_TEXT'
export const SET_CATEGORY_SELECTED = 'SET_CATEGORY_SELECTED'
export const SET_SUBCATEGORY_SELECTED = 'SET_SUBCATEGORY_SELECTED '
export const SET_SOURCE_SELECTED = 'SET_SOURCE_SELECTED'
export const setIndexTableViewModel = (viewmodel ) => {
    return ({

        type: SET_INDEX_TABLE_VIEWMODEL,
        payload: viewmodel 
    })
};
 
export const setPriceSortEnums = (viewmodel) => {
    return ({

        type: SET_PRICE_SORT_ENUM,
        payload: viewmodel
    })
};
export const setSelectedCurrency = (viewmodel) => {
    return ({

        type: SET_CURRENCY_ENUM,
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

export const setSearchText = (searchText) => {
    return ({

        type: SET_SEARCH_TEXT,
        payload: searchText
    })
};

export const setSubCategorySelected = (searchText) => {
    return ({

        type: SET_SUBCATEGORY_SELECTED,
        payload: searchText
    })
};
export const setCategorySelected = (searchText) => {
    return ({

        type: SET_CATEGORY_SELECTED,
        payload: searchText
    })
};
export const setSourceSelected = (searchText) => {
    return ({

        type: SET_SOURCE_SELECTED,
        payload: searchText
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