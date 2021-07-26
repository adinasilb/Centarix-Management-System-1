$.validator.addMethod("IsPubMedOrHasTitle", function (value, element) {
    return $("#Resource_Title").val() != "" || $("#Resource_PubMedID").val() != "";
}, 'Must Have Title');

$.validator.addMethod("HasResourceTypes", function (value, element) {
    var valid = false;
    $(".ResourceTagsHidden").each(function (e) {
        console.log("Tag Name : " + $(this).attr("name"));
        console.log("Value: " + $(this).attr("value"));
        if ($(this).attr("value") == "true") {
            console.log("In if statement of true");
            valid = true;
        }
    });
    return valid;
}, 'Must Select At Least One Resource Tag');

$('.add-resource-form').validate({
    normalizer: function (value) {
        return $.trim(value);
    },
    rules: {
        "Resource.Title": {
            IsPubMedOrHasTitle: true
        },
        "Resource.ResourceTypeID": {
            HasResourceTypes: true
        }
    }
});