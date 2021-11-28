$('.search-by-name, .search-by-company-ID').on('change', function (e) {
	e.stopImmediatePropagation();
	var searchNameText = $(".search-by-name").val().toLowerCase();
	var searchCompanyIDText = $(".search-by-company-ID").val().toLowerCase();

	//console.log(url);
	$.ajax({
		async: true,
		url: "/Vendors/SearchByVendorNameAndCompanyID?vendorName=" + searchNameText + "&companyID=" + searchCompanyIDText,
		type: 'GET',
		cache: false,
		success: function (newData) {
			$("._IndexForPayment").html(newData);
		}
	});

});