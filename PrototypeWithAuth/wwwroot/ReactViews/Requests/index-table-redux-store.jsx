import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { useEffect, useRef, } from 'react';
import { useDispatch, connect } from 'react-redux';
import { jsonToFormData } from '../Utility/root-function.jsx'

function IndexTableReduxStore(props) {
    const dispatch = useDispatch();
    const didMount = useRef(false);

    useEffect(() => {
        if (props.reloadIndex) { useEffectFunc(didMount, props, dispatch, true); }
    }, [props.reloadIndex])

    useEffect(() => {
        useEffectFunc(didMount, props, dispatch, false);
    }, [props.pageNumber])

    useEffect(() => {
        useEffectFunc(didMount, props, dispatch, true);
    }, [props.inventoryFilterViewModel, props.navigationInfo, props.categoryPopoverViewModel, props.pricePopoverViewModel, props.tabInfo])
    return <div attr={props.inventoryFilterViewModel.numFilters}></div>;
}

const mapStateToProps = state => {
    console.log("mstp global")
    return {
        inventoryFilterViewModel: state.inventoryFilterViewModel,
        tabInfo: state.tabInfo,
        categoryPopoverViewModel: state.categoryPopoverViewModel,
        pricePopoverViewModel: state.pricePopoverViewModel,
        navigationInfo: state.navigationInfo,
        pageNumber: state.pageNumber,
        reloadIndex: state.reloadIndex
    };
};

export default connect(
    mapStateToProps
)(IndexTableReduxStore)


function useEffectFunc(didMount, props, dispatch, resetPageNumber) {
    if (didMount.current) {
        var testJSON = {};
        var formdata = jsonToFormData(props, testJSON);
        if (resetPageNumber) {
            formdata.set("pageNumber", 1);
        }       
        fetch("/Requests/GetIndexTableJson", {
            method: "POST",
            body: formdata
        }).then(response => {
            console.log(response);
            return response.json();
        })
            .then(result => {
                console.dir(result);
                if (result != undefined) {
                    dispatch(Actions.setIndexTableViewModel(JSON.parse(result)));
                }
                // if (modals != undefined) { dispatch(Actions.removeModals(modals)); }
                document.getElementById("loading").style.display = "none";

            }).catch(jqxhr => {
                document.querySelectorAll('.error-message').forEach(e => e.classList.add("d-none"));
                if (document.querySelector('.error-message') == null) {
                    console.log(jqxhr);
                } else {
                    document.querySelector('.error-message').innerHTML = jqxhr;
                    document.querySelector('.error-message').classList.remove("d-none");
                }
                document.getElementById("loading").style.display = "none";
                //dispatch(Actions.removeModals(modals));
            });
    } else {
        didMount.current = true;
    }
}


