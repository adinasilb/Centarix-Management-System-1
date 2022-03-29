
export var combineTwoFormDatas = function (formdata1, formdata2) {
    if (formdata1 == undefined) {
        var formdata1 = new FormData();
    }
    if (formdata2 != undefined) {
        for (var pair of formdata2.entries()) {
            formdata1.append(pair[0], pair[1]);
        }
    }
    return formdata1;
}


export function jsonToFormData(inJSON, inTestJSON, inFormData, parentKey) {

    //console.log(inJSON)
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
                    if (value[i] == null) {
                        value[i] ="";
                    }
                    form_data.append(constructedKey+"["+i+"]", value[i]);
                }
            }
        }
        else {
            if (inJSON[key] == null) {
                inJSON[key] = "";
            }
            form_data.append(constructedKey, inJSON[key]);
            testJSON[constructedKey] = inJSON[key];
        }
    }
    return form_data;
}