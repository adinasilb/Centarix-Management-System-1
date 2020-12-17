

$(".load-order-details").on("click", function (e) {
    console.log("in order details");
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var section = $(this).attr('section')
    //takes the item value and calls the Products controller with the ModalView view to render the modal inside
    var $itemurl = "/Requests/ReOrderFloatModalView/?id=" + $(this).val() + "&NewRequestFromProduct=true" + "&SectionType=" + section;
    $.fn.CallPageRequest($itemurl, "reorder");
    return false;
});
$(".load-product-details").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var $itemurl = "";
    if ($(this).hasClass('operations')) {
        $itemurl = "/Operations/EditModalView/?id=" + $(this).val();
    }
    else {
        console.log("Requests/EditModalView/?id")
        //takes the item value and calls the Products controller with the ModalView view to render the modal inside
        $itemurl = "/Requests/EditModalView/?id=" + $(this).val() + "&SectionType=" + $("#SectionType").val();
    }
    $.fn.CallPageRequest($itemurl, "details");
    return false;
});
$(".load-product-details-summary").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    //takes the item value and calls the Products controller with the ModalView view to render the modal inside
    var $itemurl = "/Requests/DetailsSummaryModalView/?id=" + $(this).val();
    $.fn.CallPageRequest($itemurl, "details");
    return false;
});
$(".load-receive-and-location").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var isOperations = $(this).hasClass('operations');
    //takes the item value and calls the Products controller with the ModalView view to render the modal inside
    var $itemurl = "/Requests/ReceivedModal?RequestID=" + $(this).val() + "&IsOperations=" + isOperations;
    $.fn.CallPageRequest($itemurl, "received");
    return false;
});
