$('.filter-col').mouseover(function () {
    //console.log('hurray');
    $('.filter-col').focus();
});

$('#invFilterPopover').on('shown.bs.popover', function () {
    $('body').addClass('popover-open');
})
$('body').off('click').on('click', '.btn-filter', function () {
	//alert("in filter function")
	//var archived = $('.archive-check').val();
	//console.log('archived ' + archived);
	var data = $.fn.BindSelectedFilters('.popover');
	var id = $(this).val();
	var col = $(this).parent().parent();
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
	//console.log('archived ' + archived);
	$.fn.ReloadFilterDiv(numFilters, data);
});
$.fn.ReloadFilterDiv = function (numFilters, data) {
	console.log('in reload filter function');
	var sectionType = $('#masterSectionType').val();
	var isProprietary = $(".request-status-id").attr("value") == 7 ? true : false;
	console.log('status ' + $(".request-status-id").attr("value"));
	//console.log(data);
	$.ajax({
		//processData: false,
		//contentType: false,
		data: data,
		traditional: true,
		async: true,
		url: "/Requests/_InventoryFilterResults?numFilters=" + numFilters + "&sectionType=" + sectionType + "&isProprietary=" + isProprietary,
		type: 'POST',
		cache: false,
		success: function (newData) {
			$('#inventoryFilterContent').html(newData);
			$('#inventoryFilterContentDiv .popover-body').html($('#inventoryFilterContent').html());
		}
	});
}
$('.search-requests').on('change', function () {
	var searchText = $(this).val().toLowerCase();
	console.log(searchText);
	/*if (searchText.length < 3 && searchText != "") {
		return;
    }*/
	var url;
	switch ($('#masterPageType').val()) {
		case 'RequestSummary':
			url = "_IndexTableWithProprietaryTabs";
			break;
		case 'RequestRequest':
		case 'OperationsRequest':
			url = "_IndexTableWithCounts"
			break;
		case 'OperationsInventory':
			url = "_IndexTable";
			break;
	}
	//console.log(url);
	$.ajax({
		//processData: false,
		//contentType: false,
		//data: data,
		traditional: true,
		async: true,
		url: "/Requests/"+url+"?" + $.fn.getRequestIndexString() + "&searchText=" + searchText,
		type: 'GET',
		cache: false,
		success: function (data) {
			$("." + url).html(data);
			$('.search-requests').val(searchText);
			$('.search-requests').focus();
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
	var sectionType = $('#masterSectionType').val();
	$.fn.ClearFilter(sectionType, isProprietary);
});
$(".popover").off("click").on("click", ".archive-button", function (e) {
	console.log('check archive!')
	$(".archive-check").each(function () {
		console.log(this);
		var checked = $(this).prop("checked");
		$(this).attr("checked", !checked);
		$(this).val(!checked);
		/*var numFilters = $('.numFilters').attr("value");
		var data = $.fn.BindSelectedFilters('.popover');
		$.fn.ReloadFilterDiv(numFilters, data);*/
	})
});
$('body').on('click', "#applyFilter", function (e) {
	e.stopImmediatePropagation();
	console.log('clicked!')
	var data = $.fn.BindSelectedFilters('.popover');
	var searchText = $('.popover .search-requests-in-filter').val();
	var numFilters = $('.numFilters').attr("value");
	console.log('search text ' + searchText);
	//reset page number
	$('.page-number').val(1);

	var url;
	switch ($('#masterPageType').val()) {
		case 'RequestSummary':
			url = "_IndexTableWithProprietaryTabs";
			break;
		case 'RequestRequest':
		case 'OperationsRequest':
			url = "_IndexTableWithCounts"
			break;
		case 'OperationsInventory':
			url = "_IndexTable";
			break;
	}
	//console.log(data);
	$.ajax({
		//    processData: false,
		//    contentType: false,
		data: data,
		async: true,
		traditional: true,
		url: "/Requests/" + url + "?" + $.fn.getRequestIndexString() + "&searchText=" + searchText + "&numFilters=" + numFilters,
		type: 'POST',
		cache: false,
		success: function (data) {
			$('.' + url).html(data);
			$('[data-toggle="popover"]').popover('dispose');
			$('body').removeClass('popover-open');
			$('#invFilterPopover').removeClass('order-inv-background-color custom-button-font');
			$('#invFilterPopover').addClass('custom-order-inv');
		}
	});
});

