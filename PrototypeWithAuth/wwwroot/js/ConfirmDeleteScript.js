$("body").on("click", ".load-confirm-delete", function (e) {
    console.log("in confirm delete");
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var $itemurl = "/Requests/DeleteModal/?id=" + $(this).attr("value") + "&" + $.fn.getRequestIndexString();
    $.fn.CallPageRequest($itemurl, "delete");
    return false;
});