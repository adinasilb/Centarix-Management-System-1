import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { useEffect, useRef, } from 'react';
import { useDispatch, connect } from 'react-redux';


function IndexTableReduxStore(props) {
    const dispatch = useDispatch();
    const didMount = useRef(false);
    console.log("AjaxPartialIndexTable")
    useEffect(() => {
        alert("global useEffect before if")
        if (didMount.current) {
            var formdata = new FormData();
            console.log("AjaxPartialIndexTable")
            formdata.append("indexTableJsonViewModelString", JSON.stringify(props))
            alert("global useEffect")
            fetch("/Requests/GetIndexTableJson", {
                method: "POST",
                body: formdata
            }).then(response => {
                console.log(response);
                return response.json()
            })
                .then(result => {
                    console.dir(result)
                    if (result != undefined) {
                        dispatch(Actions.setIndexTableViewModel(JSON.parse(result)));
                    }
                    // if (modals != undefined) { dispatch(Actions.removeModals(modals)); }

                    document.getElementById("loading").style.display = "none";

                }).catch(jqxhr => {
                    document.querySelectorAll('.error-message').forEach(e => e.classList.add("d-none"));
                    if (document.querySelector('.error-message') == null) {
                        console.log(jqxhr)
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
    }, [props])
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
        pageNumber: state.pageNumber
    };
};

export default connect(
    mapStateToProps
)(IndexTableReduxStore)

