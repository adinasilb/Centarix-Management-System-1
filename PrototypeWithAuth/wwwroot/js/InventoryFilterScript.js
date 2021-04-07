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



