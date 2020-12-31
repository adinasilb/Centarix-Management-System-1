

$(".load-order-details").on("click", function (e) {
    console.log("in order details");
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var section = $("#masterSectionType").val()
    //takes the item value and calls the Products controller with the ModalView view to render the modal inside
    var $itemurl = "/Requests/ReOrderFloatModalView/?id=" + $(this).attr("value") + "&NewRequestFromProduct=true" + "&SectionType=" + section;
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
        $itemurl = "/Requests/EditModalView/?id=" + $(this).val() + "&SectionType=" +  $("#masterSectionType").val();
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
    var isOperations = false;
    if($("#masterSectionType").val()=="Operations")
    {
        isOperations=true;
    }   
    //takes the item value and calls the Products controller with the ModalView view to render the modal inside
    var $itemurl = "/Requests/ReceivedModal?RequestID=" + $(this).attr("value") + "&IsOperations=" + isOperations;
    $.fn.CallPageRequest($itemurl, "received");
    return false;
});
$(".order-approved-operation").on("click", function (e) {
    console.log("approving");
    e.preventDefault();
    $("#loading").show();
    ajaxPartialIndexTable($(".request-status-id").val(), "/Operations/Order/", "._IndexTableWithCounts");
    return false;
});
$(".approve-order").on("click", function (e) {
    console.log("approving");
    e.preventDefault();
    $("#loading").show();
    ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/Approve/" + $(this).attr("value"), "._IndexTableWithCounts");
    return false;
});
function ajaxPartialIndexTable(status, url, viewClass) {
    console.log("in ajax partial index call");
    var selectedPriceSort = [];
    $("#priceSortContent .priceSort:checked").each(function (e) {
        selectedPriceSort.push($(this).attr("enum"));
    })
    $.ajax({
        async: true,
        url: url,
        data: {
            id: $(this).attr("value"),
            PageNumber: $('#PageNumber').val(),
            RequestStatusID: status,
            PageType: $('#masterPageType').val(),
            SectionType: $('#masterSectionType').val(),
            SidebarType: $('#masterSidebarType').val(),
            SelectedPriceSort: selectedPriceSort,
            SelectedCurrency: $('#tempCurrency').val(),
            SidebarFilterID: $('.sideBarFilterID').val()
        },
        traditional: true,
        type: 'GET',
        cache: false,
        success: function (data) {
            $(viewClass).html(data);
            $("#loading").hide();
            return true;
        }
    });
}