export const SET_INDEX_TABLE_VIEWMODEL = 'SET_INDEX_TABLE_VIEWMODEL'


export const setIndexTableViewModel = ( viewmodel ) => {
    console.dir(viewmodel);
    return ({

        type: SET_INDEX_TABLE_VIEWMODEL,
        viewmodel
    })
};

