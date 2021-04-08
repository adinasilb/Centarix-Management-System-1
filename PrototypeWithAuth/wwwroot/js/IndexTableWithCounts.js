$('.countStatus').off('click').click( function(e){
    $('#pricePopover').popover('dispose');
    e.preventDefault()
    $(".active").removeClass("active")
    $(this).addClass("active")
    //reset page number
    $('.page-number').val(1);
    clearFilter();
    ajaxPartialIndexTable($(this).attr("value"), "/Requests/_IndexTable", "._IndexTable", "GET")
});


function clearFilter() {
    $.ajax({
        async: true,
        url: "/Requests/_InventoryFilterResults?selectedFilters=null&numFilters=0",
        type: 'GET',
        cache: false,
        success: function(newData) {
            $('#inventoryFilterContent').html(newData);
        }
    });
}
//function ajaxCallToPartialTable(status) {
//    var selectedPriceSort = [];
//    $("#priceSortContent .priceSort:checked").each(function (e) {
//        selectedPriceSort.push($(this).attr("enum"));
//    })
//    $.ajax({
//        async: true,
//        url: "/Requests/_IndexTable",
//        data: {
//            PageNumber: $('#PageNumber').val(),
//            RequestStatusID: status,           
//            PageType: $('#masterPageType').val(),
//            SectionType:  $('#masterSectionType').val(), 
//            SidebarType:  $('#masterSidebarType').val(),
//            SelectedPriceSort: selectedPriceSort,
//            SelectedCurrency: $('#tempCurrency').val(),
//            SidebarFilterID :  $('.sideBarFilterID').val()
//        },
//        traditional: true,
//        type: 'GET',
//        cache: false,
//        success: function (data) {
//            $("._IndexTable").html(data);
//            return true;
//        }
//  });
//    }

