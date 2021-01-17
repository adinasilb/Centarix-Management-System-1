

$(".load-order-details").off('click').on("click", function (e) {
    console.log("in order details");
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
       var selectedPriceSort = [];
    $("#priceSortContent .priceSort:checked").each(function (e) {
        selectedPriceSort.push($(this).attr("enum"));
    })
    var section = $("#masterSectionType").val()
    //takes the item value and calls the Products controller with the ModalView view to render the modal inside
    var $itemurl = "/Requests/ReOrderFloatModalView/?id=" + $(this).attr("value") + "&NewRequestFromProduct=true" + "&SectionType=" + section+ $.fn.getRequestIndexString()
    $.fn.CallPageRequest($itemurl, "reorder");
    return false;
});
 
$(".load-product-details").off('click').on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var $itemurl = "";
    if ($('#masterSectionType').val()=="Operations") {
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
    var $itemurl = "/Requests/EditModalView/?id=" + $(this).attr("value") + "&isEditable=false";
    $.fn.CallPageRequest($itemurl, "details");
    return false;
});
$(".load-receive-and-location").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show(); 
    //takes the item value and calls the Products controller with the ModalView view to render the modal inside
    var $itemurl = "/Requests/ReceivedModal?RequestID=" + $(this).attr("value")+   $.fn.getRequestIndexString()
    $.fn.CallPageRequest($itemurl, "received");
    return false;
});
$(".order-approved-operation").off('click').on("click", function (e) {
    console.log("approving");
    e.preventDefault();
    $("#loading").show();
    ajaxPartialIndexTable($(".request-status-id").val(), "/Operations/Order/?id=" + $(this).attr("value"), "._IndexTableWithCounts",  "GET");
    return false;
});
$(".approve-order").off('click').on("click", function (e) {
    console.log("approving");
    e.preventDefault();
    $("#loading").show();
    ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/Approve/?id=" + $(this).attr("value"), "._IndexTableWithCounts",  "GET");
    return false;
});
$(".create-calibration").off('click').on("click", function (e) {
    e.preventDefault();

    $.ajax({
    async: true,
    url: "/Calibrations/CreateCalibration?requestid="+$(this).attr("value"),
    type: "GET",
    cache: false,
    success: function (data) {
        $('.render-body').html(data)
           $('#myForm a:first').tab('show');
    }
    });
    return false;
});

$(".page-item a").off('click').on("click", function (e) {
    console.log("next page");
    e.preventDefault();
    $("#loading").show();
    var pageNumber = parseInt($(this).html());
    $('.page-number').val(pageNumber);
    ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/_IndexTableData/", "._IndexTableData", "GET");
    return false;
});


function ajaxPartialIndexTable(status, url, viewClass, type, formdata) {
    console.log("in ajax partial index call"+url);
    var selectedPriceSort = [];
    $("#priceSortContent .priceSort:checked").each(function (e) {
        selectedPriceSort.push($(this).attr("enum"));
    })
  var contentType = true;
    var processType = true;
    if(formdata == undefined)
    {
        console.log("formdata is undefined");
        formdata={            
            PageNumber: $('.page-number').val(),
            RequestStatusID: status,
            PageType: $('#masterPageType').val(),
            SectionType: $('#masterSectionType').val(),
            SidebarType: $('#masterSidebarType').val(),
            SelectedPriceSort: selectedPriceSort,
            SelectedCurrency: $('#tempCurrency').val(),
            SidebarFilterID: $('.sideBarFilterID').val()
        };
       console.log(formdata);
    }
    else{
        contentType = false;
            processType = false;
        }

    $.ajax({
    contentType: contentType,
    processData: processType,
    async: true,
    url: url,
    data: formdata,
    traditional: true,
    type: type,
    cache: false,
    success: function (data) {
            $(".modal").modal('hide');
        $(viewClass).html(data);
        $("#loading").hide();
        return true;
    },
    error : function () {
        $("#loading").hide();
        return true;
    }
    });

    return false;
}

