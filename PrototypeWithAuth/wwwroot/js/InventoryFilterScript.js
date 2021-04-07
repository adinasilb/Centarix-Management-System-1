$('.filter-col').mouseover(function () {
    //console.log('hurray');
    $('.filter-col').focus();
});

$('#invFilterPopover').on('shown.bs.popover', function () {
    $('body').addClass('popover-open');
})
$('.btn-filter').click(function () {     
	
   var data = $.fn.BindSelectedFilters();
   $.ajax({
			processData: false,
			contentType: false,
			data: data,
			async: true,
			url: "/Requests/_InventoryFilterResults",
			type: 'GET',
			cache: false,
			success: function (data) {

            }
    });

});

$("#applyFilter").click(function(){
   var data = $.fn.BindSelectedFilters();
    $.ajax({
		processData: false,
		contentType: false,
		data: data,
		async: true,
		url: "/Requests/_IndexTableData?"+$.fn.getRequestIndexString(),
		type: 'GET',
		cache: false,
		success: function (data) {
			$("._IndexTableData").html(data);
			  $('[data-toggle="popover"]').popover('dispose');
            $('body').removeClass('popover-open');
        }
    });


});

$.fn.BindSelectedFilters = function(){
    var selectedVendor = $(".popover .subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedOwner = $(".popover .subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedLocation = $(".popover .subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedCategory = $(".popover .subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedSubCategory = $(".popover .subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
	return {SelectedCategories: selectedCategory, SelectedSubCategories : selectedSubCategory, SelectedLocations : selectedLocation, SelectedVendors: selectedVendor, SelectedOwner : selectedOwner}
}