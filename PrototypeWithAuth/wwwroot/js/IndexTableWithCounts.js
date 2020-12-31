$('body').on('click', '.countStatus', function(e){
    $('#pricePopover').popover('dispose');
    e.preventDefault()
    alert("in countstatus click")
    $(".active").removeClass("active")
    $(this).addClass("active")
    ajaxCallToPartialTable($(this).attr("value"))
});

function ajaxCallToPartialTable(status) {
    var selectedPriceSort = [];
    $("#priceSortContent .priceSort:checked").each(function (e) {
        selectedPriceSort.push($(this).attr("enum"));
    })
    $.ajax({
        async: true,
        url: "/Requests/_IndexTable",
        data: {
            PageNumber: $('#PageNumber').val(),
            RequestStatusID: status,           
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
            $("._IndexTable").html(data);
            return true;
        }
  });
    }

