$('.filter-col').mouseover(function () {
    //console.log('hurray');
    $('.filter-col').focus();
});

$('#protocolFilterPopover').on('shown.bs.popover', function () {
    $('body').addClass('popover-open');
})

       $('body').off('click').on('click', '.btn-filter', function () {
            $.fn.ButtonFilterClick($(this).val(), $(this).parent().parent(), $(this).parent())
        });

    $('body').on('click', "#applyFilter", function (e) {
            e.stopImmediatePropagation();
            $.fn.ApplyFilterClick();
        });


$('.search-by-name').on('change', function (e) {
	e.stopImmediatePropagation();
	var searchText = $(this).val().toLowerCase();
	console.log(searchText);
	console.log('searchtext length' + searchText.length)
	/*if (searchText.length < 3 && searchText != "") {
		return;3
    }*/
	//clear search in filter so doesn't mess things up
	$('.search-by-name-in-filter').attr('value', "");
	//reset page number
	$('.page-number').val(1);
	var data = $.fn.BindSelectedFilters('');
	var url;
	/*switch ($('#masterPageType').val()) {
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
	}*/
	var url = '_IndexTableData';
	if ($('.' + url).length == 0) { //if it's showing the nothing is here page. not just using indextable when don't have to, so don't lose price and category filters
		url = '_IndexTable';
	}
	//console.log(url);
	$.ajax({
		//processData: false,
		//contentType: false,
		data: data,
		traditional: true,
		async: true,
		url: "/Protocols/"+url+"?" + $.fn.getRequestIndexString(),
		type: 'POST',
		cache: false,
		success: function (newData) {
			$("." + url).html(newData);
			//$('.search-requests').val(searchText);
			$('.search-by-name').focus();
		}
	});

});
$(".type-search").on('change input',function(){
	var searchText = $(this).val();
	var colName = '.type-col';
	$.fn.SearchColumns(searchText, colName);
	
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
$(".owner-search").on('change input',function(){
	var searchText=$(this).val();
	var colName = '.owner-col';
	$.fn.SearchColumns(searchText, colName);

});


$("body").on("click", "#protocolFilterContentDiv .popover-close", function (e) {
	//alert('x button')
	$('[data-toggle="popover"]').popover('dispose');
	$('body').removeClass('popover-open');
	$('#protocolFilterPopover').removeClass('protocols-background-color custom-button-font');
	$('#protocolFilterPopover').addClass('custom-protocols');
}); 

$('body').on('click', '.clear-filters', function () {
	var isProprietary = $(".request-status-id").attr("value") == 7 ? true : false;
	var sectionType = $('#masterSectionType').val();
	$.fn.ClearFilter(sectionType, isProprietary);
});



