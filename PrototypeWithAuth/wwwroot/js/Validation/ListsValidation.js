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
            minlength: 0,
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
            minlength: 0,
            maxlength: 20
        },
    }
});

