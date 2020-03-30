﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//global Exchange Rate variable (usd --> nis)

function showmodal() {
	$("#modal").modal('show');
};

//change product subcategory dropdown according to the parent categroy selection when a parent category is selected
$("#parentlist").change(function () {
	var parentCategoryId = $("#parentlist").val();
	var url = "/Requests/GetSubCategoryList";

	$.getJSON(url, { ParentCategoryId: parentCategoryId }, function (data) {
		var item = "";
		$("#sublist").empty();
		$.each(data, function (i, subCategory) {
			item += '<option value="' + subCategory.productSubcategoryID + '">' + subCategory.productSubcategoryDescription + '</option>'
		});
		$("#sublist").html(item);
	});
});

$("#vendorList").change(function () {
	//get the new vendor id selected
	var vendorid = $("#vendorList").val();
	$.fn.ChangeVendorBusinessId(vendorid);
});

$.fn.ChangeVendorBusinessId = function (vendorid) {
	var newBusinessID = "";

	//will throw an error if its a null value so tests it here
	if (vendorid > 0) {
		//load the url of the Json Get from the controller
		var url = "/Vendors/GetVendorBusinessID";
		$.getJSON(url, { VendorID: vendorid }, function (data) {
			//get the business id from json
			newBusinessID = data.vendorBuisnessID;
			//cannot only use the load outside. apparently it needs this one in order to work
			$(".vendorBusinessId").html(newBusinessID);
		})
	}
	//if nothing was selected want to load a blank
	$(".vendorBusinessId").html(newBusinessID);
	//put the business id into the form
}


//view documents on modal view
$(".view-docs").click(function (clickEvent) {
	console.log("order docs clicked!");
	clickEvent.preventDefault();
	clickEvent.stopPropagation();
	var title = $(this).val();
	$(".images-header").html(title + " Documents Uploaded:");
});

//PRICE PAGE ON MODAL VIEW//
//on opening of the price tag see if subunits and subsubunits should be enabled
$("#priceTab").click(function () {
	if ($("#unit-amount").val() && $("#unit-type").val()) {
		$.fn.CalculateUnitAmounts();
		$.fn.OpenCloseSubUnits(); //figure out how to get selected
		$.fn.ChangeSubUnitDropdown();
	}
	if ($("#subunit-amount").val() && $("#subunit-type").val()) {
		$.fn.CalculateSubUnitAmounts();
		$.fn.OpenCloseSubSubUnits(); //figure out how to get selected
		$.fn.ChangeSubSubUnitDropdown();
	}
	if ($("#subsubunit-amount").val() && $("#subsubunit-type").val()) {
		$.fn.CalculateSubSubUnitAmounts();
	}
	if ($("#vatInShekel").val()) {
		$.fn.CalculateTotalPlusVat();
	}
});

$("#currency").change(function () {
	$.fn.EnableSumDollarsOrShekel();
});

$("#Request_ExchangeRate").change(function () {
	$.fn.ChangeSumDollarsAndSumShekel();
	$.fn.CalculateUnitAmounts();
	$.fn.CalculateSubUnitAmounts(); //do we need an if statement here?
	$.fn.CalculateSubSubUnitAmounts(); //do we need an if statement here?
	$.fn.CalculateTotalPlusVat();
});

$("#unit-amount").change(function () {
	$.fn.CalculateUnitAmounts();
	$.fn.CalculateSubUnitAmounts(); //do we need an if statement here?
	$.fn.CalculateSubSubUnitAmounts(); //do we need an if statement here?
});

$("#unit-type").change(function () {
	var selected = $(':selected', this);
	console.log("selected " + selected);
	var optgroup = selected.closest('optgroup').attr('label');
	$.fn.OpenCloseSubUnits();
	$.fn.ChangeSubUnitDropdown(optgroup);
});

$("#subunit-amount").change(function () {
	$.fn.CalculateSubUnitAmounts();
	$.fn.CalculateSubSubUnitAmounts();
});

$("#subunit-type").change(function () {
	var selected = $(':selected', this);
	var optgroup = selected.closest('optgroup').attr('label');
	$.fn.OpenCloseSubSubUnits();
	$.fn.ChangeSubSubUnitDropdown(optgroup);
});

$("#subsubunit-amount").change(function () {
	$.fn.CalculateSubSubUnitAmounts();
});

$("#sumShekel").change(function () {
	$.fn.ChangeSumDollarsAndSumShekel(); //make sure this is right here
	$.fn.CalculateUnitAmounts(); //if statement?
	$.fn.CalculateSubUnitAmounts(); //ifstatement?
	$.fn.CalculateSubSubUnitAmounts(); //ifstatement?
	$.fn.CalculateTotalPlusVat();
});

$("#sumDollars").change(function () {
	$.fn.ChangeSumDollarsAndSumShekel(); //make sure this is right here
	$.fn.CalculateUnitAmounts(); //if statement?
	$.fn.CalculateSubUnitAmounts(); //ifstatement?
	$.fn.CalculateSubSubUnitAmounts(); //ifstatement?
	$.fn.CalculateTotalPlusVat();
});

$("#vatInShekel").change(function () {
	$.fn.CalculateTotalPlusVat();
});

//Calculate unit amounts
$.fn.CalculateUnitAmounts = function () {
	console.log("Calculating unit amounts");
	var $unitAmounts = $("#unit-amount").val();
	var $sumDollars = $("#sumDollars").val();
	var $exchangeRate = $("#Request_ExchangeRate").val();
	$unitSumDollars = $sumDollars / $unitAmounts;
	$('input[name="unit-price-dollars"]').val($unitSumDollars);
	$unitSumShekel = $unitSumDollars * $exchangeRate;
	$('input[name="unit-price-shekel"').val($unitSumShekel);
	console.log("Done");
};
//calculate sub unit amountss
$.fn.CalculateSubUnitAmounts = function () {
	console.log("Calculating sub unit amounts");
	var $subUnitAmounts = $("#subunit-amount").val();
	var $unitPriceDollars = $("#unit-price-dollars").val();
	var $exchangeRate = $("#Request_ExchangeRate").val();
	$subUnitSumDollars = $unitPriceDollars / $subUnitAmounts;
	$('input[name="subunit-price-dollars"]').val($subUnitSumDollars);
	$subUnitSumShekel = $subUnitSumDollars * $exchangeRate;
	$('input[name="subunit-price-shekel"').val($unitSumShekel);
	console.log("Done");
};
//calculate sub sub unit amounts
$.fn.CalculateSubSubUnitAmounts = function () {
	console.log("Calculating sub sub unit amouts");
	var $subSubUnitAmounts = $("#subsubunit-amount").val();
	console.log("subsubunitamounts: " + $subSubUnitAmounts);
	var $subUnitPriceDollars = $("#subunit-price-dollars").val();
	console.log("subunitpricedollars: " + $subUnitPriceDollars);
	var $exchangeRate = $("#Request_ExchangeRate").val();
	$subSubUnitSumDollars = $subUnitPriceDollars / $subSubUnitAmounts;
	console.log("subsubunitsumdollars: " + $subSubUnitSumDollars);
	$('input[name="subsubunit-price-dollars"]').val($subSubUnitSumDollars);
	$subSubUnitSumShekel = $subSubUnitSumDollars * $exchangeRate;
	console.log("subsubunitsumshekel: " + $subSubUnitSumShekel);
	$('input[name="subsubunit-price-shekel"').val($subSubUnitSumShekel);
	console.log("Done");
};
//Open subunits
$.fn.OpenCloseSubUnits = function () {
	console.log("open sub units");
	var selected = $(':selected', $("#unit-type"));
	if (selected > "") {
		$(".RequestSubunitCard .form-control").attr('disabled', false);
	}
	else { //else is in case they deleted it
		$(".RequestSubunitCard .form-control").prop('disabled', true);
		$(".RequestSubsubunitCard .form-control").prop('disabled', true);
	}
	console.log("Done");
};
//open sub sub units 
$.fn.OpenCloseSubSubUnits = function () {
	console.log("open sub sub units");
	var selected = $(':selected', $("#subunit-type"));
	if (selected > "") {
		$(".RequestSubsubunitCard .form-control").attr('disabled', false);
	}
	else {
		$(".RequestSubsubunitCard .form-control").attr('disabled', true);
	}
	console.log("Done");
};
//change subunit dropdown
$.fn.ChangeSubUnitDropdown = function (optgroup) {
	console.log("change sub unit dropdown");
	//the following is based on the fact that the unit types and parents are seeded with primary key values
	switch (optgroup) {
		case "Units":
			$("#subunit-type optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
			$("#subunit-type optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
			break;
		case "Weight/Volume":
			$("#subunit-type optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
			$("#subunit-type optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
			break;
		case "Test":
			$("#subunit-type optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
			$("#subunit-type optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
			break;
	}
	console.log("Done");
};
//change sub sub unit dropdown
$.fn.ChangeSubSubUnitDropdown = function (optgroup) {
	console.log("change sub sub unit dropdown");
	switch (optgroup) {
		case "Units":
			$("#subsubunit-type optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
			$("#subsubunit-type optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
			break;
		case "Weight/Volume":
			$("#subsubunit-type optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
			$("#subsubunit-type optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
			break;
		case "Test":
			$("#subsubunit-type optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
			$("#subsubunit-type optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
			break;
	}
	console.log("Done");
};
//calculate total + vat
$.fn.CalculateTotalPlusVat = function () {
	console.log("calculate total plus vat");
	var $exchangeRate = $("#Request_ExchangeRate").val();
	var vatInShekel = $("#vatInShekel").val();
	var sumShekel = $("#sumShekel").val();
	var $sumPlusVatShekel = parseInt(sumShekel) + parseInt(vatInShekel);
	$('input[name="SumPlusVat-Shekel"]').val($sumPlusVatShekel);
	//IS THIS THE BEST WAY OF DOING IT OR SHOULD I CONVERT THE INPUT I JUST DID??
	var vatInDollars = vatInShekel / $exchangeRate;
	var $sumPlusVatDollars = parseInt($("#sumDollars").val()) + parseInt(vatInDollars);
	$('input[name="SumPlusVat-Dollar"]').val($sumPlusVatDollars);
	console.log("Done");
};
//enable or disable sumdollars and sumshekel
$.fn.EnableSumDollarsOrShekel = function () {
	console.log("enable or disable sumdollars and sumshekel");
	var currencyType = $("#currency").val();
	switch (currencyType) {
		case "dollar":
			$("#sumShekel").prop("disabled", true);
			$("#sumDollars").prop("disabled", false);
			break;
		case "shekel":
			$("#sumShekel").prop("disabled", false);
			$("#sumDollars").prop("disabled", true);
			break;
	}
	console.log("Done");
}
//change sumDollars and sumSHekel
$.fn.ChangeSumDollarsAndSumShekel = function () {
	console.log("changing sumdollars and sumshekel");
	var $exchangeRate = $("#Request_ExchangeRate").val();
	if ($("#sumDollars").prop("disabled")) {
		console.log("sum dollars disabled");
		var $sumShekel = $("#sumShekel").val();
		var $sumDollars = $sumShekel / $exchangeRate;
		$('input[name="sumDollars"]').val($sumDollars);
	}
	else if ($("#sumShekel").prop("disabled")) {
		console.log("sum shekel disabled");
		var $sumDollars = $("#sumDollars").val();
		var $sumShekel = $sumDollars * $exchangeRate;
		$('input[name="sumShekel"]').val($sumShekel);
	}
	else {
		console.log("oops none are disabled");
	}
	console.log("done!");
}


////change expected supply days automatically
//$("#Request_ExpectedSupplyDays").change(function () {
//    console.log("Request_expectedsupplydays changed!")
//    var invoiceDate = $("#Request_ParentRequest_InvoiceDate").val();
//    console.log("Invoice Date: " + invoiceDate);
//    var invoiceDateinDateFormat = new Date(invoiceDate);
//    console.log("Invoice Date in Date Format: " + invoiceDateinDateFormat);
//    var expectedSupplyDays = $(this).val();
//    console.log("Expected Supply Days: " + expectedSupplyDays);
//    var expectedSupplyDate = invoiceDateinDateFormat.setMonth(3);
//    console.log("Expected Supply Date: " + expectedSupplyDate);
//});
////change warranty month automatically
//$("#Request_Warranty").change(function () {
//    console.log("Request_Warranty changed!")
//});

//function addMonths(date, months) {
//    var d = date.getDate();
//    date.setMonth(date.getMonth() + +months);
//    if (date.getDate() != d) {
//        date.setDate(0);
//    }
//};