$.validator.addMethod('mindate', function (v, el, minDate) {
	if (this.optional(el)) {
		return true;
	}
	var selectedDate = new Date($(el).attr("data-val"));
	console.log("selected date"+selectedDate)
	minDate = new Date(minDate.setHours(0));
	minDate = new Date(minDate.setMinutes(0));
	minDate = new Date(minDate.setSeconds(0));
	minDate = new Date(minDate.setMilliseconds(0));
	return selectedDate >= minDate;
}, 'Please select a valid date');


$.validator.addMethod("UniqueVendorAndCatalogNumber", function () {
	var vendorID = $("#vendorList").val();
	var catalogNumber = $("#Request_CatalogNumber").val();
	var productID = null;
	var catalogResult = false;
	
	$.ajax({
		async: false,
		url: '/Requests/CheckUniqueVendorAndCatalogNumber',
		type: 'POST',
		data: { "VendorID": vendorID, "CatalogNumber": catalogNumber, "ProductID": productID },
		dataType: 'text',
		success: function (result) {
			catalogResult = result;
		},
		error: function (jqXHR, status, error) {
			console.log(status, error);
		}
	});
	return catalogResult;
}, 'That product has already been created');

$('.ordersItemForm').validate({
	rules: {
		"Request.Product.ProductName": "required",
		"Request.CatalogNumber": {
			required: true,
				remote:{
		url: '/Requests/CheckUniqueVendorAndCatalogNumber',
		type: 'POST',
		data: { "VendorID":function(){ return $("#vendorList").val()}, "CatalogNumber": function(){return $("#Request_CatalogNumber").val() } , "ProductID": function(){if ($(".turn-edit-on-off").length > 0) {
		return $(".turn-edit-on-off").attr("productID");
	}else{return null}}},
			},
		},
		"Request.Product.ProductSubcategory.ParentCategoryID": "selectRequired",
		"Request.Product.ProductSubcategoryID": "selectRequired",
		"Request.SubProject.ProjectID": "selectRequired",
		"Request.SubProjectID": "selectRequired",
		"Request.Product.VendorID": {
			"selectRequired" : true,				
		},
		"Request.ParentQuote.QuoteNumber": {
			required: true
		},
		"Request.ParentQuote.QuoteDate": {
			required: true,
			//mindate: new Date('1900-12-17T03:24:00')
		},
		"Request.ExpectedSupplyDays": {
			min: 0,
			integer: true,
			required: true
		},
		"Request.Warranty": {
			min: 0,
			integer: true
		},
		"Request.URL": {
			//url: true
		},
		"Request.ExchangeRate": {
			required: function () {
				return $("#currency").val() == "dollar" || $("#currency").val() == null;
			},
			number: true,
			min: 1
		},

		"Request.Unit": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"Request.SubUnit": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"Request.SubSubUnit": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"Request.Cost": {
			required: false,
			number: true,
			min: 1
		},
		"sum-dollars": {
			required: true,
			number: true,
			min: 1
		},
		"Request.UnitTypeID": "selectRequired",
		"Request.SubUnitTypeID": "selectRequired",
		"Request.SubSubUnitTypeID": "selectRequired"
	},
	messages: {
        "Request.CatalogNumber": {
            remote: "this product has already been created"
        },
		}
});

$('#vendorList').change(function(){
	$('#Request_CatalogNumber').valid();
});
