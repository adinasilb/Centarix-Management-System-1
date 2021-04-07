$('.filter-col').mouseover(function () {
    //console.log('hurray');
    $('.filter-col').focus();
});

$('#invFilterPopover').on('shown.bs.popover', function () {
    $('body').addClass('popover-open');
})
$('body').on('click', '.btn-filter', function () {
	var data = $.fn.BindSelectedFilters();
	var id = $(this).val();
	var col = $(this).parent().parent()
	console.log(data);

	if (col.hasClass('vendor-col')) {
		data.SelectedVendorsIDs.push(id);
	} else if (col.hasClass('owner-col')) {
		data.SelectedOwnersIDs.push(id);
	} else if (col.hasClass('location-col')) {
		data.SelectedLocationsIDs.push(id);
	} else if (col.hasClass('category-col')) {
		data.SelectedCategoriesIDs.push(id);
	} else if (col.hasClass('subcategory-col')) {
		data.SelectedSubCategoriesIDs.push(id);
	}
    
    $.ajax({
			//processData: false,
			//contentType: false,
			data: data,
			traditional:true,
			async: true,
			url: "/Requests/_InventoryFilterResults",
			type: 'GET',
			cache: false,
			success: function (newData) {
				$('#inventoryFilterContent').html(newData);
				$('#inventoryFilterContentDiv .popover-body').html($('#inventoryFilterContent').html());
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
    var selectedVendor = $(".popover .vendor-col .selected button").map(function () { return $(this).attr("value"); }).get();
	var selectedOwner = $(".popover .owner-col .selected button").map(function () { return $(this).attr("value"); }).get();
	var selectedLocation = $(".popover .location-col .selected button").map(function () { return $(this).attr("value"); }).get();
	var selectedCategory = $(".popover .category-col .selected button").map(function () { return $(this).attr("value"); }).get();
	var selectedSubCategory = $(".popover .subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
	return {SelectedCategoriesIDs: selectedCategory, SelectedSubCategoriesIDs : selectedSubCategory, SelectedLocationsIDs : selectedLocation, SelectedVendorsIDs: selectedVendor, SelectedOwnersIDs : selectedOwner}
}