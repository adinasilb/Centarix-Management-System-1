$(".add-entry-form").validate({
    normalizer: function (value) {
        return $.trim(value);
    },
    rules: {
        'Date': "required",
        'VisitNumber': "required",
        'SiteID': "required"
    }
});