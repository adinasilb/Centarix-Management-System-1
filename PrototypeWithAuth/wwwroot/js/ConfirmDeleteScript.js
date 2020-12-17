$(".load-confirm-delete").on("click", function (e) {
    console.log("in confirm delete");
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var $itemurl = "/Requests/DeleteModal/?id=" + $(this).val() + "&SectionType=" + $(this).attr('name');;
    $.fn.CallPageRequest($itemurl, "delete");
    return false;
});