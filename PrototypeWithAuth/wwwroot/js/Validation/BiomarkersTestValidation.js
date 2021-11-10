$(function () {
    $.validator.addClassRules("double", {
        number: true /*,
        other rules */
    });
    $(".bio-form").validate()
})