export const SET_INDEX_TABLE_VIEWMODEL = 'SET_INDEX_TABLE_VIEWMODEL'
export const OPEN_CUSTOM_FIELD = 'OPEN_CUSTOM_FIELD'

export const setIndexTableViewModel = (viewmodel ) => {
    console.dir(viewmodel);
    return ({

        type: SET_INDEX_TABLE_VIEWMODEL,
         viewmodel 
    })
};

export const openCustomField = (viewmodel) => {
    console.log("in action.jsx opencustomfield");
    return ({
        type: OPEN_CUSTOM_FIELD,
        viewmodel
    })
}

