$('.filter-col').mouseover(function () {
    //console.log('hurray');
    $('.filter-col').focus();
});

$('#invFilterPopover').on('shown.bs.popover', function () {
    $('body').addClass('popover-open');
})
$('.btn-filter').click(function () {
        
    var selectedVendor = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedOwner = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedLocation = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedCategory = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedSubCategory = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    $.ajax({
        async: true,
        url: "Requests/_InventoryFilterResults",
        type: 'GET',
        //cache: true,
        success: function (data) {

        }
    });

})