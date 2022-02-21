

$('._AccountingSearchAndFilter .search-by-name, ._AccountingSearchAndFilter .search-by-vendor, ._AccountingSearchAndFilter .search-by-order-number').on('change', function (e) {
	var searchText = $('.search-by-name').val();
	var vendorID = $('select.search-by-vendor').val();
	var orderNumber =$('.search-by-order-number').val();
	var url = '_IndexTableDataByVendor';
	if ($('.' + url).length == 0) { //if it's showing the nothing is here page. not just using indextable when don't have to, so don't lose price and category filters
		url = '_IndexTableByVendor';
	}
	console.log("in search function");
	$.ajax({
		processData: false,
		contentType: false,
		async: true,
		url: "/Requests/"+url+"?" + $.fn.getRequestIndexString() + "&SelectedVendor="+vendorID+"&CentarixOrderNumber="+orderNumber+"&ProductName="+searchText,
		type: 'GET',
		cache: false,
		success: function (newData) {
			$("." + url).html(newData);
			//$('.search-requests').val(searchText);
			//$('.search-by-name').focus();
		}
	});

});
