$('.filter-col').mouseover(function () {
    //console.log('hurray');
    $('.filter-col').focus();
});

$('#invFilterPopover').on('shown.bs.popover', function () {
    $('body').addClass('popover-open');
})
$('body').off('click').on('click', '.btn-filter', function () {
	//alert("in filter function")
	var data = $.fn.BindSelectedFilters();
	var id = $(this).val();
	var col = $(this).parent().parent();
	var isProprietary = $(".request-status-id").attr("value") == 7 ? true : false;
	console.log('status ' + $(".request-status-id").attr("value"));
	var arr;
	if (col.hasClass('vendor-col')) {
			arr = data.SelectedVendorsIDs;
		} else if (col.hasClass('owner-col')) {
			arr = data.SelectedOwnersIDs;
		} else if (col.hasClass('location-col')) {
			arr = data.SelectedLocationsIDs;
		} else if (col.hasClass('category-col')) {
			arr = data.SelectedCategoriesIDs;
		} else if (col.hasClass('subcategory-col')) {
			arr = data.SelectedSubCategoriesIDs;
	}
	var numFilters;
	if ($(this).parent().hasClass('not-selected')) {
		arr.push(id);
		numFilters = Number($('.numFilters').attr("value")) + 1;
	} else {
		arr.splice($.inArray(id, arr), 1);
		numFilters = Number($('.numFilters').attr("value")) - 1;
	}
	//console.log(data);
    $.ajax({
		//processData: false,
		//contentType: false,
		data: data,
		traditional:true,
		async: true,
		url: "/Requests/_InventoryFilterResults?numFilters=" + numFilters + "&isProprietary="+ isProprietary,
		type: 'POST',
		cache: false,
		success: function (newData) {
			$('#inventoryFilterContent').html(newData);
			$('#inventoryFilterContentDiv .popover-body').html($('#inventoryFilterContent').html());
        }
     });

});
$('.body').on('change', '.search-requests', function () {

	var searchText = $(this).val().toLowerCase();
	$.ajax({
		//processData: false,
		//contentType: false,
		//data: data,
		traditional: true,
		async: true,
		url: "/Requests/_IndexTableData?" + $.fn.getRequestIndexString() + "&searchText=" + searchText,
		type: 'GET',
		cache: false,
		success: function (data) {
			$("._IndexTableData").html(data);
		}
	});

});

$(".category-search").on('change input',function(){
	var searchText = $(this).val();
	var colName = '.category-col';
	$.fn.SearchColumns(searchText, colName);
	
});
$(".subCategory-search").on('change input',function(){
	var searchText = $(this).val();
	var colName = '.subcategory-col';
	$.fn.SearchColumns(searchText, colName);

});
$(".vendor-search").on('change input',function(){
	var searchText = $(this).val();
	var colName = '.vendor-col';
	$.fn.SearchColumns(searchText, colName);

});
$(".owner-search").on('change input',function(){
	var searchText=$(this).val();
	var colName = '.owner-col';
	$.fn.SearchColumns(searchText, colName);

});
$(".location-search").on('change input',function(){
	var searchText = $(this).val();
	var colName = '.location-col';
	$.fn.SearchColumns(searchText, colName);

});
$.fn.SearchColumns = function (searchText, colName) {
	if (searchText == "") {
		$('.popover ' + colName + ' .not-selected button').removeClass("d-none");
	}
	else {
		$('.popover ' + colName + ' .not-selected button').each(function (i, e) {
			if ($(e).attr("labelName").toString().toLowerCase().indexOf(searchText) < 0) {
				$(e).addClass("d-none")
			}
			else {
				$(e).removeClass("d-none")
			}
		});
	}

}

$("body").on("click", "#inventoryFilterContentDiv .popover-close", function (e) {
	//alert('x button')
	$('[data-toggle="popover"]').popover('dispose');
	$('body').removeClass('popover-open');
	$('#invFilterPopover').removeClass('order-inv-background-color custom-button-font');
	$('#invFilterPopover').addClass('custom-order-inv');
}); 

$('body').on('click', '.clear-filters', function () {
	var isProprietary = $(".request-status-id").attr("value") == 7 ? true : false;
	$.fn.ClearFilter(isProprietary);
});


