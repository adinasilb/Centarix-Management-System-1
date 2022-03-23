import * as Actions from '../ReduxRelatedUtils/actions.jsx';
import { useEffect, useRef, } from 'react';
import { useDispatch, connect } from 'react-redux';


function AjaxPartialIndexTable(props) {
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
        navigationInfo: state.navigationInfo
    };
};

export default connect(
    mapStateToProps
)(AjaxPartialIndexTable)


export var ajaxPartialIndexTable = (dispatch, url, type, formdata, modals) => {

    return false;
}

export var combineTwoFormDatas = function (formdata1, formdata2) {
    if (formdata1 == undefined) {
        var formdata1 = new FormData();
    }
    if (formdata2 != undefined) {
        formdata2.entries().forEach(pair => {
            formdata1.append(pair[0], pair[1]);
            console.log('key ' + pair[0]);
            console.log('value ' + pair[1])

        });
    }
    console.dir(formdata1);
    return formdata1;
}




export var addObjectToFormdata = (formdata, object) => {
    console.log(object)
    if (formdata == undefined) {
        var formdata = new FormData();
    }
    Object.keys(object).forEach(key => {

        if (Array.isArray(object[key])) {
            for (const val of object[key].values()) {
                formdata.append(key, val);
                console.log('key' + key);
                console.log('val' + val)
            }
        }
        else {
            formdata.append(key, object[key]);
            console.log('key ' + key);
            console.log('value ' + object[key])
        }
    });
    console.dir("in add object to formdata function: "+formdata);
    return formdata;
}


export var getRequestIndexString = () => {
    var selectedPriceSort = "";
    document.querySelectorAll("#priceSortContent1 .priceSort:checked").forEach(e => {
        selectedPriceSort += "&SelectedPriceSort=" + e.getAttribute("enum");
    })
    var requestStatusId = document.getElementsByClassName("request-status-id")[0]?.value;
    var queryString = "PageNumber=" + document.getElementsByClassName('page-number')[0]?.value + "&RequestStatusID=" + requestStatusId + "&PageType=" + document.getElementById('masterPageType')?.value + "&SectionType=" + document.getElementById('masterSectionType')?.value + "&SidebarType=" + document.getElementById('masterSidebarType')?.value + "&SelectedCurrency=" + document.getElementById('tempCurrency')?.value + "&SidebarFilterID=" + document.getElementsByClassName('sideBarFilterID')[0]?.value + "&CategorySelected=" + (document.querySelector('#categorySortContent .select-category:checked')?.length > 0) + "&SubCategorySelected=" + (document.querySelector('#categorySortContent .select-subcategory:checked')?.length > 0) + "&ListID=" + document.getElementById("ListID")?.value + "&TabName=" + document.getElementById("TabName")?.value;
    queryString += selectedPriceSort;
    return queryString;
}