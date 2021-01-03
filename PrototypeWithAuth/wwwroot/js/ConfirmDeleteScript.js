$(".load-confirm-delete").on("click", function (e) {
    console.log("in confirm delete");
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var $itemurl = "/Requests/DeleteModal/?id=" + $(this).attr("value") + "&SectionType=" + $("#masterSectionType").val();
    $.fn.CallPageRequest($itemurl, "delete");
    return false;
});