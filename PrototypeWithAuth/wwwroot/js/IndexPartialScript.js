 $('body').on('click', '.priceFilterDiv', function (e) { e.preventDefault(); }).click();
function callIndexWithNewFilter(val, id) {
    $(id).attr("checked", !$(id).prop("checked"));

    ajaxCallToPartial();
    return false;
}
function ajaxCallToPartial() {
    var section = $('#section').val();
    var selectedPriceSort = [];
    $(".popover-body .priceSort:checked").each(function (e) {
        selectedPriceSort.push($(this).attr("enum"));
    })
 
    $.ajax({
        async: true,
        url: "/"+section+"/_IndexTable",
        data: {
            page: $('#Page').val(),
            RequestStatusID: $('#RequestStatusID').val(),
            subcategoryID: $('#SubCategoryID').val(),
            vendorID: $('#VendorID').val(),
            applicationUserID: $('#ApplicationUserID').val(),
            parentLocationInstanceID: $('#RequestParentLocationInstanceID').val(),
            PageType: $('#PageType').val(),
            selectedPriceSort: selectedPriceSort,
            selectedCurrency: $('#tempCurrency').val()
        },
        traditional: true,
        type: 'GET',
        cache: false,
        success: function (data) {
            $(".index-partial").html(data);

            return true;
        }
    });
}
$('body').off('click', "#nis, #usd").on('click', "#nis, #usd", function (e) {
    $('input[name=currecy]').attr("checked", false)
    $('input[name=currecy]').prop("checked", false)
    $(this).attr("checked", true);
    $('#tempCurrency').val($(this).val())
    ajaxCallToPartial();    
    return false;

});