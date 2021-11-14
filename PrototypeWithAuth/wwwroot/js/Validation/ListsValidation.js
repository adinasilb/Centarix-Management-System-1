$('.moveListItemForm').validate({
    normalizer: function (value) {
        return $.trim(value);
    },
    rules: {
        "NewListID":{
            selectRequired: true
       },
    }
});

$('.newListForm').validate({
    normalizer: function (value) {
        return $.trim(value);
    },
    rules: {
        "ListTitle": {
            required: true,
            minlength: 1,
            maxlength: 20
        },
    }
});

$('.listSettingsForm').validate({
    normalizer: function (value) {
        return $.trim(value);
    },
    rules: {
        "SelectedList.Title": {
            required:true,
            minlength: 1,
            maxlength: 20
        },
    }
});

