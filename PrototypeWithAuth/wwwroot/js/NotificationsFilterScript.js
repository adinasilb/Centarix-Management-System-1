

$('.search-by-name-or-order-num, .search-by-vendor').on('change', function (e) {
	var searchText = $('.search-by-name-or-order-num').val();
	var vendorID = $('select.search-by-vendor').val();
	var url = '_IndexTableDataByVendor';
	if ($('.' + url).length == 0) { //if it's showing the nothing is here page. not just using indextable when don't have to, so don't lose price and category filters
		url = '_IndexTableByVendor';
	}
	console.log("in search function");
	$.ajax({
		processData: false,
		contentType: false,
		async: true,
		url: "/Requests/"+url+"?" + $.fn.getRequestIndexString() + "&SelectedVendor="+vendorID+"&NameOrCentarixOrderNumber="+searchText,
		type: 'GET',
		cache: false,
		success: function (newData) {
			$("." + url).html(newData);
			//$('.search-requests').val(searchText);
			//$('.search-by-name').focus();
		}
	});

});
