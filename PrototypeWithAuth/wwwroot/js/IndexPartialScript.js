 $('body').on('click', '.priceFilterDiv', function (e) { e.preventDefault(); }).click();

function callIndexWithNewFilter(val, id) {
    $(id).attr("checked", !$(id).prop("checked"));
    alert("In call index with new filter")
    ajaxCallToPartialTableData();
    return false;
}
function ajaxCallToPartialTableData() {
    var selectedPriceSort = [];
    $("#priceSortContent .priceSort:checked").each(function (e) {
        selectedPriceSort.push($(this).attr("enum"));
    })
    $.ajax({
        async: true,
        url: "/Requests/_IndexTableData",
        data: {
            PageNumber: $('#PageNumber').val(),
            RequestStatusID: $('.request-status-id').val(),           
            PageType: $('#masterPageType').val(),
            SectionType:  $('#masterSectionType').val(), 
            SidebarType:  $('#masterSidebarType').val(),
            SelectedPriceSort: selectedPriceSort,
            SelectedCurrency: $('#tempCurrency').val(),
            SidebarFilterID :  $('.sideBarFilterID').val()
        },
        traditional: true,
        type: 'GET',
        cache: false,
        success: function (data) {
            $("._IndexTableData").html(data);
            return true;
        }
    });
}
$('body').off('click', "#nis, #usd").on('click', "#nis, #usd", function (e) {
    $('input[name=currency]').attr("checked", false)
    $('input[name=currency]').prop("checked", false)
    $(this).attr("checked", true);
    $(this).prop("checked", true);
    console.log(this);
    $('#tempCurrency').val($(this).val())
    ajaxCallToPartialTableData();    
    return false;

});
$("#pricePopover").click(function () {
	//	$('[data-toggle="popover"]').popover('dispose');
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