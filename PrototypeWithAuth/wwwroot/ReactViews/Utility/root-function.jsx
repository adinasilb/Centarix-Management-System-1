import { MDBInput } from 'mdbreact';
import * as Actions from '../ReduxRelatedUtils/actions.jsx';

export var ajaxPartialIndexTable =(dispatch, url, type, formdata, modals) =>{
    console.log("in ajax partial index call " + url);
    //alert('before bind filter')
    if (document.getElementById('searchHiddenForsForm') !=null) {
        var moreFormData = new FormData(document.getElementById('searchHiddenForsForm')[0])
        console.dir(moreFormData);
        formdata =combineTwoFormDatas( moreFormData, formdata);
        console.dir(formdata);
    }
    if (document.getElementById("inventoryFilterContent") != null) {
        var selectedFilters = bindSelectedFilters("");
        console.log('in if')
    }
    var monthsString = "";
    var yearsString = "";
    var listString = "";
    var months = document.getElementById("Months");
    var years = document.getElementById("Years");
    var listID = document.getElementById("ListID");
    if (months != null) {
        months.value.forEach(month => monthsString += "&months=" + month)
    }
    if (years != null) {
        years.value.forEach(year => yearsString += "&years=" + year)
    }
    if (listID != null) {
        listString += "&listID=" + listID.value
    }

    console.log("in else");
        
    if (!url.includes("?")) {
        url += "?"
    } else {
        url += "&";
    }
    url += getRequestIndexString();
    url += monthsString;
    url += yearsString;
    url += listString;
        
    

    if (selectedFilters != undefined/*should also somehow check if anything is chosen...*/) {
        formdata = addObjectToFormdata(formdata, selectedFilters);
        console.dir(formdata);
    }

    if (formdata != undefined) {
        type = "POST"
    }
    fetch(url, {
        method: type,
        body: formdata
    }).then(response => response.json())
       .then(result => {
            console.dir(result)
            if (result != undefined) {
                dispatch(Actions.setIndexTableViewModel(JSON.parse(result)));
            }
            dispatch(Actions.removeModals(modals));

        document.querySelectorAll(".tooltip").forEach(c => c.remove());
        document.getElementById("loading").style.display = "none";
        //workaround for price radio button not coming in when switching from nothing is here tab
        var id = "nis";
        //alert($(id).prop('checked'))
        if (document.getElementById('tempCurrency').value === "USD") {
            id = "usd";
        }
        if (document.getElementById(id).getAttribute("checked") !== "checked") {
            console.log('checking button')
            document.getElementById(id).setAttribute("checked", "checked");
        }

        }).catch(jqxhr => {
            document.querySelectorAll('.error-message').forEach(e => e.classList.add("d-none"));
            document.querySelector('.error-message').innerHTML = jqxhr;
            document.querySelector('.error-message').classList.remove("d-none");
            document.getElementById("loading").style.display = "none";
            dispatch(Actions.removeModals(modals));
    });    
    return false;
}


export var bindSelectedFilters = (className) => {
    console.log('in bind selected filters');
    var selectedVendor =[... (document.querySelectorAll(className + " .vendor-col .selected button")??[])]?.map(e=> e.getAttribute("value"));
    var selectedOwner = [...(document.querySelectorAll(className + " .owner-col .selected button")??[])]?.map(e => e.getAttribute("value"));
    var selectedCategory = [...(document.querySelectorAll(className + " .category-col .selected button") ?? [])];
    var selectedLocation = [...(document.querySelectorAll(className + " .location-col .selected button")??[])]?.map(e =>  e.getAttribute("value"))
    var selectedType = [...(document.querySelector(className + " .type-col .selected button")??[])]?.map(e =>  e.getAttribute("value"))
    var selectedLocation = [...(document.querySelectorAll(className + " .location-col .selected button")??[])]?.map(e =>  e.getAttribute("value"))
    var selectedSubCategory = [...(document.querySelectorAll(className + " .subcategory-col .selected button")??[])]?.map(e =>  e.getAttribute("value"))
    console.log(selectedVendor);
    var catalogNumber = document.querySelector(className + ' .search-by-catalog-number')?.value
    if (catalogNumber == undefined) {
        catalogNumber = "";
    }
    console.log(catalogNumber)
    var archived = document.querySelector('.archive-check')?.value;
    if (archived == undefined) {
        archived = false;
    }
    var catalogNumber = document.querySelector(className + ' .search-by-catalog-number')?.value
    if (catalogNumber == undefined) {
        catalogNumber = "";
    }
    console.log(catalogNumber)
    var searchText = document.querySelector('.search-by-name')?.value;
    console.log(searchText);
    //console.log('searchtext length before if' + searchText.length)
    if (searchText == undefined || !searchText.length) {
        searchText = document.querySelector('.popover .search-by-name-in-filter')?.value;
        console.log(searchText)
        if (searchText == undefined || !searchText.length) {
            searchText = document.querySelector('.search-by-name-in-filter')?.value;
            if (searchText == undefined || !searchText.length) {
                searchText = "";
            }
        }
    }
    return {
        SelectedCategoriesIDs: selectedCategory, SelectedSubCategoriesIDs: selectedSubCategory, SelectedLocationsIDs: selectedLocation,
        SelectedVendorsIDs: selectedVendor, SelectedOwnersIDs: selectedOwner, Archived: archived, CatalogNumber: catalogNumber,
        SelectedTypesIDs: selectedType, NumFilters: document.querySelector("#NumFilters").value, "SearchText": searchText
    }
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
     document.querySelectorAll("#priceSortContent1 .priceSort:checked").forEach(e=> {
        selectedPriceSort += "&SelectedPriceSort=" + e.getAttribute("enum");
    })
    var requestStatusId = document.getElementsByClassName("request-status-id")[0]?.value;
     var queryString = "PageNumber=" + document.getElementsByClassName('page-number')[0]?.value + "&RequestStatusID=" + requestStatusId + "&PageType=" + document.getElementById('masterPageType')?.value + "&SectionType=" + document.getElementById('masterSectionType')?.value + "&SidebarType=" + document.getElementById('masterSidebarType')?.value + "&SelectedCurrency=" + document.getElementById('tempCurrency')?.value + "&SidebarFilterID=" + document.getElementsByClassName('sideBarFilterID')[0]?.value + "&CategorySelected=" + (document.querySelector('#categorySortContent .select-category:checked')?.length > 0) + "&SubCategorySelected=" + (document.querySelector('#categorySortContent .select-subcategory:checked').length > 0) + "&ListID=" + document.getElementById("ListID")?.value+"&TabName=" + document.getElementById("TabName")?.value;
    queryString += selectedPriceSort;
    return queryString;
}
