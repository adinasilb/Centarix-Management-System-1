export const SET_INDEX_TABLE_VIEWMODEL = 'SET_INDEX_TABLE_VIEWMODEL'
export const ADD_MODAL = 'ADD_MODAL'
export const REMOVE_MODAL = 'REMOVE_MODAL'
export const REMOVE_MODALS = 'REMOVE_MODALS'

export const setIndexTableViewModel = (viewmodel ) => {
    return ({

        type: SET_INDEX_TABLE_VIEWMODEL,
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