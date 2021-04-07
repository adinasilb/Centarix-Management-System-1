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
    var selectedVendor = $(".popover .category-type-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedOwner = $(".popover .vendor-col .selected button").map(function () { return $(this).attr("value"); }).get();
	var selectedLocation = $(".popover .owner-col .selected button").map(function () { return $(this).attr("value"); }).get();
	var selectedLocation = $(".popover .location-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedCategory = $(".popover .category-col .selected button").map(function () { return $(this).attr("value"); }).get();
    var selectedSubCategory = $(".popover .subcategory-col .selected button").map(function () { return $(this).attr("value"); }).get();
	return {SelectedCategories: selectedCategory, SelectedSubCategories : selectedSubCategory, SelectedLocations : selectedLocation, SelectedVendors: selectedVendor, SelectedOwner : selectedOwner}
}



$(".category-search").on('change input',function(){
	var searchText=$(this).val();
	if(searchText=="")
	{
		$('.popover .category-col .not-selected button').removeClass("d-none");
	}
	else
	{
		$('.popover .category-col .not-selected button').each(function(i, e){
				if($(e).attr("labelName").toString().toLowerCase().indexOf(searchText) <0)
				{
					$(e).addClass("d-none")
				}
				else
				{
					$(e).removeClass("d-none")
				}
		});
	}
	
});
$(".subCategory-search").on('change input',function(){
		var searchText=$(this).val();
	if(searchText=="")
	{
		$('.popover .subcategory-col .not-selected button').removeClass("d-none");
	}
	else
	{
		$('.popover .subcategory-col .not-selected button').each(function(i, e){
				if($(e).attr("labelName").toString().toLowerCase().indexOf(searchText) <0)
				{
					$(e).addClass("d-none")
				}
				else
				{
					$(e).removeClass("d-none")
				}
		});
	}

});
$(".vendor-search").on('change input',function(){
	var searchText=$(this).val();
	if(searchText=="")
	{
		$('.popover .vendor-col .not-selected button').removeClass("d-none");
	}
	else
	{
		$('.popover .vendor-col .not-selected button').each(function(i, e){
				if($(e).attr("labelName").toString().toLowerCase().indexOf(searchText) <0)
				{
					$(e).addClass("d-none")
				}
				else
				{
					$(e).removeClass("d-none")
				}
		});
	}

});
$(".owner-search").on('change input',function(){
		var searchText=$(this).val();
	if(searchText=="")
	{
		$('.popover .owner-col .not-selected button').removeClass("d-none");
	}
	else
	{
		$('.popover .owner-col .not-selected button').each(function(i, e){
				if($(e).attr("labelName").toString().toLowerCase().indexOf(searchText) <0)
				{
					$(e).addClass("d-none")
				}
				else
				{
					$(e).removeClass("d-none")
				}
		});
	}

});
$(".location-search").on('change input',function(){
		var searchText=$(this).val();
	if(searchText=="")
	{
		$('.popover .location-col .not-selected button').removeClass("d-none");
	}
	else
	{
		$('.popover .location-col .not-selected button').each(function(i, e){
				if($(e).attr("labelName").toString().toLowerCase().indexOf(searchText) <0)
				{
					$(e).addClass("d-none")
				}
				else
				{
					$(e).removeClass("d-none")
				}
		});
	}

});



