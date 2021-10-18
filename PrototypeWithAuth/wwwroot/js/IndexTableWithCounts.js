﻿$('.countStatus').off('click').click(function (e) {
    $('.open-price-popover').popover('dispose');
    e.preventDefault()
    $(".active").removeClass("active")
    $(this).addClass("active")
    //reset page number
    $('.page-number').val(1);
    var requestStatusId = $(this).attr("value");
    var isProprietary = requestStatusId == 7 ? true : false;
    var sectionType = $('#masterSectionType').val();
    //alert(sectionType);
    if ($(this).hasClass('reload-filter')) {
        $.fn.ClearFilter(sectionType, isProprietary);
        $('.search-by-name').val("");
    }
    //alert('after clear filter');
    var pageType = $('#masterPageType').val();
    //var viewClass = pageType != 'RequestSummary' ? '_IndexTableWithCounts' : '_IndexTableWithProprietaryTabs';
    var viewClass = '_IndexTable'; //don't use counts now anyways so just reload data so search button doesn't get messed up
    console.log("viewclass: " + viewClass);
    $.fn.ajaxPartialIndexTable(requestStatusId, "/Requests/" + viewClass, "." + viewClass, 'GET')
});

/*$('.view-archived-requests').off('click').on('click', function () {
    $(".active").removeClass("active");
    $.fn.ajaxPartialIndexTable
});*/
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

