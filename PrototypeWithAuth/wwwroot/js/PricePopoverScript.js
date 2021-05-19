 //$('body').on('click', '.priceFilterDiv', function (e) { e.preventDefault(); }).click();


$('body').off('click', "#nis, #usd").on('click', "#nis, #usd", function (e) {
    $('.open-price-popover').popover('hide');
    $('input[name=SelectedCurrency]').attr("checked", false)
    $('input[name=SelectedCurrency]').prop("checked", false)
    $("."+$(this).attr("id")).attr("checked", true);
    $("."+$(this).attr("id")).prop("checked", true);
    console.log(this);
    $('#tempCurrency').val($(this).val())
    console.log($('#masterPageType').val())
    if ($('#masterPageType').val() == "RequestCart" || $('#masterPageType').val() == "LabManagementQuotes" || $('#masterPageType').val() == "AccountingPayments" || $('#masterPageType').val() == "AccountingNotifications") {
    
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
$(".open-price-popover").off('click').click(function () {
    console.log('price popover from index');
    $('[data-toggle="popover"]').popover('dispose');
        $(this).addClass("activePopover");
		$('[data-toggle="popover"]').each(function() {
            if(!$(this).hasClass("activePopover"))
            {
                 $(this).popover('dispose');
            }
        });
        $('.open-price-popover').popover({
			sanitize: false,
			placement: 'bottom',
			html: true,
            content: function () {
                return $('#priceSortContent').html();
			}
		});
    $('.open-price-popover').popover('toggle');

    $(".popover").off("click").on("click", ".priceFilterDiv", function (e) {
        var id = "#" + $(this).children(".priceSort").prop("id")
        $(id).attr("checked", !$(id).prop("checked"));
        //  alert("In call index with new filter")
        if ($('#masterPageType').val() == "RequestCart" || $('#masterPageType').val() == "LabManagementQuotes" || $('#masterPageType').val() == "AccountingPayments" || $('#masterPageType').val() == "AccountingNotifications") {
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
    })
    
	});