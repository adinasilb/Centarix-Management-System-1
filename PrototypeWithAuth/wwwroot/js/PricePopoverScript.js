 $('body').on('click', '.priceFilterDiv', function (e) { e.preventDefault(); }).click();

function callIndexWithNewFilter(val, id) {
    $(id).attr("checked", !$(id).prop("checked"));
  //  alert("In call index with new filter")
    if ($('#masterPageType').val() == "RequestCart" || $('#masterPageType').val() == "LabManagementQuotes" || $('#masterPageType').val() == "AccountingPayments" || $('#masterPageType').val() == "AccountingNotifications") {
        //var selectedPriceSort = [];
        //$("#priceSortContent .priceSort:checked").each(function (e) {
        //    selectedPriceSort.push($(this).attr("enum"));
        //})
        //var formdata = {
        //    SelectedPriceSort: selectedPriceSort,
        //    SelectedCurrency: $('#tempCurrency').val(),
        //};
        //console.log($('#masterSidebarType').val())
        //var url = "";
        //switch ($('#masterSidebarType').val()) {
        //    case "Cart":
        //        url = "/Requests/_Cart"
        //        break;
        //    case "Orders":
        //        url = "/Requests/_LabManageOrders"
        //        break;
        //    case "Quotes":
        //        url = "/Requests/_LabManageQuotes"
        //        break;
        //    default:
        //        break;
        //}
        //if ($('#masterSectionType').val() == "Accounting") {
        //    formdata.AccountingEnum = $("#sidebarEnum").val();
        //    formdata.PageType = $('#masterPageType').val();
        //    console.log("page type" + $('#masterPageType').val())
        //    switch ($('#masterPageType').val()) {
        //        case "AccountingPayments":
        //            url = "/Requests/_AccountingPayments"
        //            break;
        //        case "AccountingNotifications":
        //            url = "/Requests/_AccountingNotifications"
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //console.log(url)
        //$.ajax({
        //    contentType: true,
        //    processData: true,
        //    async: true,
        //    url: url,
        //    data: formdata,
        //    traditional: true,
        //    type: 'GET',
        //    cache: false,
        //    success: function (data) {
        //        $(".partial-orders").html(data)
        //    }
        //})
        ajaxPartialIndexTable($('.request-status-id').val(), "/Requests/_IndexTableDataByVendor", "._IndexTableDataByVendor", "GET");
    }
    else {
        if ($('#masterPageType').val() == "AccountingGeneral") {
            var year = $("#Years").val();
            var month = $("#Months").val();

        }
        ajaxPartialIndexTable($('.request-status-id').val(), "/Requests/_IndexTableData", "._IndexTableData", "GET", undefined, "", month, year);
    }
    return false;
}
$('body').off('click', "#nis, #usd").on('click', "#nis, #usd", function (e) {
    $('#pricePopover').popover('hide');
    $('input[name=SelectedCurrency]').attr("checked", false)
    $('input[name=SelectedCurrency]').prop("checked", false)
    $("."+$(this).attr("id")).attr("checked", true);
    $("."+$(this).attr("id")).prop("checked", true);
    console.log(this);
    $('#tempCurrency').val($(this).val())
    console.log($('#masterPageType').val())
    if ($('#masterPageType').val() == "RequestCart" || $('#masterPageType').val() == "LabManagementQuotes" || $('#masterPageType').val() == "AccountingPayments" || $('#masterPageType').val() == "AccountingNotifications") {
        //var selectedPriceSort = [];
        //$("#priceSortContent .priceSort:checked").each(function (e) {
        //    selectedPriceSort.push($(this).attr("enum"));
        //})
        //console.log(selectedPriceSort)
        //var formdata = {
        //    SelectedPriceSort: selectedPriceSort,
        //    SelectedCurrency: $('#tempCurrency').val(),
        //}; 
        //var url = "";
        //switch ($('#masterSidebarType').val()) {
        //    case "Cart":
        //        url = "/Requests/_Cart"
        //       break;
        //    case "Orders":
        //        url = "/Requests/_LabManageOrders"
        //        break;
        //    case "Quotes":
        //        url = "/Requests/_LabManageQuotes"
        //        break;
        //    default:
        //        break;
        //}
        //if ($('#masterSectionType').val() == "Accounting") {
        //    formdata.AccountingEnum = $("#sidebarEnum").val();
        //    formdata.PageType = $('#masterPageType').val();
           
        //    switch ($('#masterPageType').val()) {
        //        case "AccountingPayments":
        //            url = "/Requests/_AccountingPayments"
        //            break;
        //        case "AccountingNotifications":
        //            url = "/Requests/_AccountingNotifications"
        //            break;
        //        default:
        //            break;
        //    }
        //}
            ajaxPartialIndexTable($('.request-status-id').val(), "/Requests/_IndexTableDataByVendor", "._IndexTableDataByVendor", "GET");
    }
    else {
        if ($('#masterPageType').val() == "AccountingGeneral") {
            var year = $("#Years").val();
            var month = $("#Months").val();

        }
        ajaxPartialIndexTable($('.request-status-id').val(), "/Requests/_IndexTableData", "._IndexTableData", "GET", undefined, "", month, year);
    }
    return false;

});
$("#pricePopover").off('click').click(function () {
    $('[data-toggle="popover"]').popover('dispose');
        $(this).addClass("activePopover");
		$('[data-toggle="popover"]').each(function() {
            if(!$(this).hasClass("activePopover"))
            {
                 $(this).popover('dispose');
            }
        });
		$('#pricePopover').popover({
			sanitize: false,
			placement: 'bottom',
			html: true,
            content: function () {
                return $('#priceSortContent').html();
			}
		});
		$('#pricePopover').popover('toggle');

	});