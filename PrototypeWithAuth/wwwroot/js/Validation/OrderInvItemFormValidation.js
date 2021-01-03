﻿$.validator.addMethod('mindate', function (v, el, minDate) {
	if (this.optional(el)) {
		return true;
	}
	var selectedDate = new Date($(el).val());

	minDate = new Date(minDate.setHours(0));
	minDate = new Date(minDate.setMinutes(0));
	minDate = new Date(minDate.setSeconds(0));
	minDate = new Date(minDate.setMilliseconds(0));

	return selectedDate >= minDate;
}, 'Please select a valid date');


$.validator.addMethod("UniqueVendorAndCatalogNumber", function () {
	var vendorID = $("#vendorList").val();
	var catalogNumber = $("#Request_CatalogNumber").val();
	alert("VendorID: " + vendorID + " && Catalog Num: " + catalogNumber);
	$.ajax({
		url: '/Requests/CheckUniqueVendorAndCatalogNumber',
		type: 'POST',
		data: { "VendorID": vendorID, "CatalogNumber", catalogNumber },
		dataType: 'text',
		success: function (result) {
			alert("result: " + result);
			return result;
		},
		error: function (jqXHR, satus, error) {
			console.log(status, error);
		}
	});
	return false;
}, 'That product has already been created');

$('.ordersItemForm').validate({
	rules: {
		"Request.Product.ProductName": "required",
		"Request.CatalogNumber": {
			required: true,
			UniqueVendorAndCatalogNumber: true
		},
		"Request.Product.ProductSubcategory.ParentCategoryID": "selectRequired",
		"Request.Product.ProductSubcategoryID": "selectRequired",
		"Request.SubProject.ProjectID": "selectRequired",
		"Request.SubProjectID": "selectRequired",
		"Request.Product.VendorID": {
			"selectRequired" : true,
			UniqueVendorAndCatalogNumber: true
		},
		"Request.ParentQuote.QuoteNumber": {
			required: true
		},
		"Request.ParentQuote.QuoteDate": {
			required: true,
			mindate: new Date('1900-12-17T03:24:00')
		},
		"Request.ExpectedSupplyDays": {
			min: 0,
			integer: true
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
});