
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


export function jsonToFormData(inJSON, inTestJSON, inFormData, parentKey) {
    // http://stackoverflow.com/a/22783314/260665
    // Raj: Converts any nested JSON to formData.
    console.log(inJSON)
    var form_data = inFormData || new FormData();
    var testJSON = inTestJSON || {};
    for (var key in inJSON) {
        // 1. If it is a recursion, then key has to be constructed like "parent.child" where parent JSON contains a child JSON
        // 2. Perform append data only if the value for key is not a JSON, recurse otherwise!
        var constructedKey = key;
        if (parentKey) {
            constructedKey = parentKey + "." + key;
        }

        var value = inJSON[key];
        if (value && value.constructor === {}.constructor) {
            // This is a JSON, we now need to recurse!
            jsonToFormData(value, testJSON, form_data, constructedKey);
        }
        else if (Array.isArray(value)) {
            for (var i = 0; i < value.length; i++) {
                if (value[i] && value[i].constructor === {}.constructor) {
                    // This is a JSON, we now need to recurse!
                    jsonToFormData(value[i], testJSON, form_data, constructedKey + "[" + i + "]");
                }
                else {
                    form_data.append(constructedKey+"["+i+"]", value[i]);
                }
            }
        }
        else {
            form_data.append(constructedKey, inJSON[key]);
            testJSON[constructedKey] = inJSON[key];
        }
    }
    return form_data;
}