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
        }
    });


});

$.fn.BindSelectedFilters = function(){
    var selectedVendor = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedOwner = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedLocation = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedCategory = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedSubCategory = $(".subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
	return {SelectedCategories: selectedCategory, SelectedSubCategories : selectedSubCategory, SelectedLocations : selectedLocation, SelectedVendors: selectedVendor, SelectedOwner : selectedOwner}
}