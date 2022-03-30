import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { useEffect, useRef, } from 'react';
import { useDispatch, connect } from 'react-redux';
import { jsonToFormData } from '../Utility/root-function.jsx'
import { batch } from 'react-redux'

function IndexTableReduxStore(props) {
    const dispatch = useDispatch();
    const didMount = useRef(false);

    useEffect(() => {
        console.log("reloadindex/pagenumber")
        if (didMount.current) {
            if (props.reloadIndex) {
                console.log("reloadindex true")
                useEffectFunc(props, dispatch, false);
            }
        }
        else {
            didMount.current = true;
        }
    }, [props.pageNumber, props.reloadIndex])

    useEffect(() => {

        if (didMount.current) {

            useEffectFunc(props, dispatch, props.pageNumber!=1);
        }
        else {
            didMount.current = true;
        }
    }, [props.selectedFilters, props.navigationInfo, props.selectedCurrency, props.priceSortEnums, props.tabValue, props.categorySelected, props.sourceSelected, props.subcategorySelected])
    return <div attr={props.pageNumber}></div>;
}

const mapStateToProps = state => {
    return {
        selectedFilters: state.selectedFilters,
        tabValue: state.tabValue,
        selectedCurrency: state.selectedCurrency,
        priceSortEnums: state.priceSortEnums,
        navigationInfo: state.navigationInfo,
        pageNumber: state.pageNumber,
        reloadIndex: state.reloadIndex,
        categorySelected: state.categorySelected,
        subcategorySelected: state.subcategorySelected,
        sourceSelected: state.sourceSelected
    };
};

export default connect(
    mapStateToProps
)(IndexTableReduxStore)


function useEffectFunc(props, dispatch, resetPageNumber) {
    var testJSON = {};

    if (resetPageNumber) {
        batch(() => {
            console.log("resetpagenumber")
            dispatch(Actions.setPageNumber(1));
            dispatch(Actions.setReloadIndex(true));
        })
    }
    else {
        var formdata = jsonToFormData(props, testJSON);
        fetch("/Requests/GetIndexTableJson", {
            method: "POST",
            body: formdata
        }).then(response => {
            return response.json();
        })
            .then(result => {
                if (result != undefined) {
                    batch(() => {
                        dispatch(Actions.setIndexTableViewModel(JSON.parse(result)));
                    })
                }
                // if (modals != undefined) { dispatch(Actions.removeModals(modals)); }
                document.getElementById("loading").style.display = "none";
                dispatch(Actions.setReloadIndex(false));

            }).catch(jqxhr => {
                document.querySelectorAll('.error-message').forEach(e => e.classList.add("d-none"));
                if (document.querySelector('.error-message') == null) {
                    console.log(jqxhr);
                } else {
                    document.querySelector('.error-message').innerHTML = jqxhr;
                    document.querySelector('.error-message').classList.remove("d-none");
                }
                document.getElementById("loading").style.display = "none";
                dispatch(Actions.setReloadIndex(false));
                //dispatch(Actions.removeModals(modals));
            });


    }
}


