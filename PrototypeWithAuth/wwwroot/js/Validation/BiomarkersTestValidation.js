$(function () {
    $.validator.addClassRules("double", {
        number: true /*,
        other rules */
    });
    $("form").validate()
})