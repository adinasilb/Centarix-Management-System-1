 //$('body').on('click', '.priceFilterDiv', function (e) { e.preventDefault(); }).click();


$('body').off('click', "#nis, #usd").on('click', "#nis, #usd", function (e) {
    $('.open-price-popover').popover('hide');
    var popoverId = "priceSortContent1";
    if ($(".modal").hasClass('editModal')) {
        popoverId = "priceSortContent2";
    }
    $('#' + popoverId + ' input[name=SelectedCurrency]').attr("checked", false)
    $('#' + popoverId + ' input[name=SelectedCurrency]').prop("checked", false)
    $('#' + popoverId + ' .'+$(this).attr("id")).attr("checked", true);
    $('#' + popoverId + ' .'+$(this).attr("id")).prop("checked", true);
    console.log(this);
    $('#tempCurrency').val($(this).val())
    console.log($('#masterSidebarType').val())
    //console.log($(".modal").hasClass('editModal'));
    if ($(".modal").hasClass('editModal')) {
        var requestID = $('#history').find('a.open-history-item-modal').attr("value");
        //console.log('reqeustid ' + requestID);
        var selectedPriceSort = [];
        $("#priceSortContent2 .priceSort:checked").each(function (e) {
            selectedPriceSort.push($(this).attr("enum"));
        })
        $.ajax({
            async: false,
            url: '/Requests/_HistoryTab',
            data: { id: requestID, selectedPriceSort: selectedPriceSort, selectedCurrency: $('#tempCurrency').val() },
            type: 'Post',
            cache: false,
            success: function (data) {
                $('#history').html(data);
            },
            error: function (jqxhr) {
                $('.modal .error-message').html(jqxhr.responseText);
            }
        });
    }
    else if ($("#masterSidebarType").val() == "Favorites" || $("#masterSidebarType").val() == "SharedRequests" || $("#masterSidebarType").val() == "MyLists")
    {
        $.fn.ajaxPartialIndexTable($('.request-status-id').val(), "/Requests/_IndexTableData", "._IndexTableData", "POST");
    }
    else if ($('#masterPageType').val() == "RequestCart" || $('#masterPageType').val() == "LabManagementQuotes" || $('#masterPageType').val() == "AccountingPayments" || $('#masterPageType').val() == "AccountingNotifications") {
    
        $.fn.ajaxPartialIndexTable($('.request-status-id').val(), "/Requests/_IndexTableDataByVendor", "._IndexTableDataByVendor", "POST");
    }
    else {
        /*if ($('#masterPageType').val() == "AccountingGeneral") {
            var year = $("#Years").val();
            var month = $("#Months").val();

        }*/
        $.fn.ajaxPartialIndexTable($('.request-status-id').val(), "/Requests/_IndexTableData", "._IndexTableData", "POST"/*, undefined, "", month, year*/);
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
                return $('#priceSortContent1').html();
			}
		});
    $('.open-price-popover').popover('toggle');

    $(".popover").off("click").on("click", ".priceFilterDiv", function (e) {
        var id = "#priceSortContent1 " + "#" + $(this).children(".priceSort").prop("id")
        $(id).attr("checked", !$(id).prop("checked"));
        //  alert("In call index with new filter")
        if ($('#masterSidebarType').val() == "Cart" || $('#masterPageType').val() == "LabManagementQuotes" || $('#masterPageType').val() == "AccountingPayments" || $('#masterPageType').val() == "AccountingNotifications") {
            $.fn.ajaxPartialIndexTable($('.request-status-id').val(), "/Requests/_IndexTableDataByVendor", "._IndexTableDataByVendor", "GET");
        }
        else {
            /*if ($('#masterPageType').val() == "AccountingGeneral") {
                var year = $("#Years").val();
                var month = $("#Months").val();

            }*/
            $.fn.ajaxPartialIndexTable($('.request-status-id').val(), "/Requests/_IndexTableData", "._IndexTableData", "POST"/*, undefined, "", month, year*/);
        }
        return false;
    })
    
	});