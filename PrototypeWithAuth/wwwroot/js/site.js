// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//global Exchange Rate variable (usd --> nis)

function showmodal() {
	$("#modal").modal('show');
};

//modal adjust scrollability/height
$("#myModal").modal('handleUpdate');

//change product subcategory dropdown according to the parent categroy selection when a parent category is selected
$("#parentlist").change(function () {
	var parentCategoryId = $("#parentlist").val();
	var url = "/Requests/GetSubCategoryList";

	$.getJSON(url, { ParentCategoryId: parentCategoryId }, function (data) {
		var item = "<option value=''>Select Subcategory</option>";
		$("#sublist").empty();
		$.each(data, function (i, subCategory) {
			item += '<option value="' + subCategory.productSubcategoryID + '">' + subCategory.productSubcategoryDescription + '</option>'
		});
		$("#sublist").html(item);
	});
});

//insert the payment lines
$("#Request_ParentRequest_Installments").change(function () {
	var installments = $(this).val();
	console.log("installments " + installments);
	var countPrevInstallments = $(".companyAccountNum").length;
	console.log("countPrevInstallments " + countPrevInstallments);
	var difference = installments - countPrevInstallments;
	console.log("difference " + difference);
	if (difference > 0) { //installments were added
		console.log("installments increased");
		var increment = countPrevInstallments; //don't add one because it starts with zero
		var htmlTR = "";
		htmlTR += "<tr>";
		htmlTR += "<td>";
		htmlTR += '<input class="form-control" type="date" data-val="true" data-val-required="The PaymentDate field is required." id="NewPayments_' + increment + '__PaymentDate" name="NewPayments[' + increment + '].PaymentDate" value="" />';
		htmlTR += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].PaymentDate" data-valmsg-replace="true"></span>';
		htmlTR += '</td>';
		htmlTR += '<td>';
		htmlTR += '<select class="form-control paymentType" id="NewPayments_' + increment + '__CompanyAccount_PaymentType" name="NewPayments[' + increment + '].CompanyAccount.PaymentType"><option value="">Select A Payment Type </option>';
		htmlTR += '<option value="1">Credit Card</option>';
		htmlTR += '<option value="2">Bank Account</option>';
		htmlTR += '</select>';
		htmlTR += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].CompanyAccount.PaymentType" data-valmsg-replace="true"></span>';
		htmlTR += '</td>';
		htmlTR += '<td>';
		htmlTR += '<select class="form-control companyAccountNum" id="NewPayments_' + increment + '__CompanyAccount" name="NewPayments[' + increment + '].CompanyAccount"></select>';
		htmlTR += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].CompanyAccount.CompanyAccountNum" data-valmsg-replace="true"></span>';
		htmlTR += '</td>';
		htmlTR += '<td>';
		htmlTR += '<input class="form-control" type="number" data-val="true" data-val-required="The PaymentID field is required." id="NewPayments_' + increment + '__PaymentID" name="NewPayments[' + increment + '].PaymentID" value="" />';
		htmlTR += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].PaymentID" data-valmsg-replace="true"></span>';
		htmlTR += '</td>';
		htmlTR += '</tr >';
		$("body").append(htmlTR);
		$(".payments-table tr:last").after(htmlTR);
	}
	else if (difference < 0) { //installments were removed
		console.log("installments decreased");
	}
});


////Location Add View - Change dropdownlist
//$("LocationInstance_LocationTypeID").change(function () {
//	console.log("entered js got json function");
//	var locationTypeID = $(this).val();
//	var url = "/Locations/GetChildrenTypes";
//	$.getJSON(url, { LocationTypeID: locationTypeId }, function (data) {
//		console.log("in location instance json");
//		$.each(data, function (i, locationType) {
//			htmlChildType = '<div class='
//		});
//	});
//});

//payments on the price modal -cascading dropdown choice with json
$(".paymentType").change(function () {
	var paymentTypeId = $(this).val();
	var url = "/Requests/GetCompanyAccountList";
	var paymentTypeId = $(this).attr("id");
	console.log("payment type id: " + paymentTypeId);
	var firstNum = paymentTypeId.charAt(12);
	var secondNum = paymentTypeId.charAt(13);
	var numId = firstNum;
	if (secondNum != "_") {
		numId = firstNum + secondNum;
	}
	var companyAccountId = "NewPayments_" + numId + "__CompanyAccount";

	$.getJSON(url, { paymentTypeId: paymentTypeId }, function (data) {
		console.log("in json");
		var item = "";
		$("#" + companyAccountId).empty();
		$.each(data, function (i, companyAccount) {
			console.log(companyAccount.companyAccountId)
			item += '<option value="' + companyAccount.companyAccountId + '">' + companyAccount.companyAccountNum + ' - hello</option>'
			console.log(item);
		});
		$("#" + companyAccountId).html(item);
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
	console.log("price tab was clicked");
	if ($("#unit-amount").val() && $("#unit-type").val()) {
		console.log("unit amounts and unit types are there");
		$.fn.CalculateUnitAmounts();
		$.fn.OpenCloseSubUnits(); //figure out how to get selected
		$.fn.ChangeSubUnitDropdown();
	}
	if ($("#subunit-amount").val() && $("#subunit-type").val()) {
		console.log("subunit amounts and subunit types are there");
		$.fn.CalculateSubUnitAmounts();
		$.fn.OpenCloseSubSubUnits(); //figure out how to get selected
		$.fn.ChangeSubSubUnitDropdown();
	}
	if ($("#subsubunit-amount").val() && $("#subsubunit-type").val()) {
		console.log("subsubunit amounts and subunit types are there");
		$.fn.CalculateSubSubUnitAmounts();
	}
	if ($("#Request_VAT").val() > "") {
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

$("#Request_Cost").change(function () {
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
	var $unitAmounts = $("#unit-amount").val();
	var $sumDollars = $("#Request_Cost").val();
	var $exchangeRate = $("#Request_ExchangeRate").val();
	$unitSumDollars = $sumDollars / $unitAmounts;
	$inputBox = $('input[name="unit-price-dollars"]');
	console.log("Using function show results: $unitSumDollars = " + $unitSumDollars);
	$.fn.ShowResults($inputBox, $unitSumDollars);
	console.log("back in calculate amounts");
	//$('input[name="unit-price-dollars"]').val($unitSumDollars);
	$unitSumShekel = $unitSumDollars * $exchangeRate;
	$inputBox2 = $('input[name="unit-price-shekel"]');
	console.log("Using function show results: $unitSumShekel = " + $unitSumShekel);
	$.fn.ShowResults($inputBox2, $unitSumShekel);
	//$('input[name="unit-price-shekel"').val($unitSumShekel);
};
//calculate sub unit amountss
$.fn.CalculateSubUnitAmounts = function () {
	var $subUnitAmounts = $("#subunit-amount").val();
	var $unitPriceDollars = $("#unit-price-dollars").val();
	var $exchangeRate = $("#Request_ExchangeRate").val();
	$subUnitSumDollars = $unitPriceDollars / $subUnitAmounts;
	$('input[name="subunit-price-dollars"]').val($subUnitSumDollars);
	$subUnitSumShekel = $subUnitSumDollars * $exchangeRate;
	$('input[name="subunit-price-shekel"').val($unitSumShekel);
};
//calculate sub sub unit amounts
$.fn.CalculateSubSubUnitAmounts = function () {
	var $subSubUnitAmounts = $("#subsubunit-amount").val();
	var $subUnitPriceDollars = $("#subunit-price-dollars").val();
	var $exchangeRate = $("#Request_ExchangeRate").val();
	$subSubUnitSumDollars = $subUnitPriceDollars / $subSubUnitAmounts;
	$('input[name="subsubunit-price-dollars"]').val($subSubUnitSumDollars);
	$subSubUnitSumShekel = $subSubUnitSumDollars * $exchangeRate;
	$('input[name="subsubunit-price-shekel"').val($subSubUnitSumShekel);
};
//Open subunits
$.fn.OpenCloseSubUnits = function () {
	var selected = $(':selected', $("#unit-type"));
	if (selected > "") {
		$(".RequestSubunitCard .form-control").attr('disabled', false);
	}
	else { //else is in case they deleted it
		$(".RequestSubunitCard .form-control").prop('disabled', true);
		$(".RequestSubsubunitCard .form-control").prop('disabled', true);
	}
};
//open sub sub units 
$.fn.OpenCloseSubSubUnits = function () {
	var selected = $(':selected', $("#subunit-type"));
	if (selected > "") {
		$(".RequestSubsubunitCard .form-control").attr('disabled', false);
	}
	else {
		$(".RequestSubsubunitCard .form-control").attr('disabled', true);
	}
};
//change subunit dropdown
$.fn.ChangeSubUnitDropdown = function (optgroup) {
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
};
//change sub sub unit dropdown
$.fn.ChangeSubSubUnitDropdown = function (optgroup) {
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
};
//calculate total + vat
$.fn.CalculateTotalPlusVat = function () {
	var $exchangeRate = $("#Request_ExchangeRate").val();
	var vatInShekel = $("#vatInShekel").val();
	var sumShekel = $("#sumShekel").val();
	var $sumPlusVatShekel = parseInt(sumShekel) + parseInt(vatInShekel);
	$('input[name="SumPlusVat-Shekel"]').val($sumPlusVatShekel);
	//IS THIS THE BEST WAY OF DOING IT OR SHOULD I CONVERT THE INPUT I JUST DID??
	var vatInDollars = vatInShekel / $exchangeRate;
	var $sumPlusVatDollars = parseInt($("#Request_Cost").val()) + parseInt(vatInDollars);
	$('input[name="SumPlusVat-Dollar"]').val($sumPlusVatDollars);
};
//enable or disable sumdollars and sumshekel
$.fn.EnableSumDollarsOrShekel = function () {
	var currencyType = $("#currency").val();
	switch (currencyType) {
		case "dollar":
			$("#sumShekel").prop("disabled", true);
			$("#Request_Cost").prop("disabled", false);
			break;
		case "shekel":
			$("#sumShekel").prop("disabled", false);
			$("#Request_Cost").prop("disabled", true);
			break;
	}
}
//change sumDollars and sumSHekel
$.fn.ChangeSumDollarsAndSumShekel = function () {
	var $exchangeRate = $("#Request_ExchangeRate").val();
	if ($("#Request_Cost").prop("disabled")) {
		console.log("sum dollars disabled");
		var $sumShekel = $("#sumShekel").val();
		var $sumDollars = $sumShekel / $exchangeRate;
		$('input[name="sumDollars"]').val($sumDollars);
	}
	else if ($("#sumShekel").prop("disabled")) {
		console.log("sum shekel disabled and going into the function");
		var $sumDollars = $("#Request_Cost").val();
		var $sumShekel = $sumDollars * $exchangeRate;
		$sumShekelsInputBox = $('input[name="sumShekel"]');
		console.log("$sumShekel: " + $sumShekel);
		$('input[name="sumShekel"]').val($sumShekel);
		//$.fn.ShowResults($sumShekelsInputBox, $sumShekel);
		//$('input[name="sumShekel"]').val($sumShekel);
	}
	else {
		console.log("oops none are disabled");
	}
}

$.fn.ShowResults = function ($inputBox, $value) {
	var theResult = isFinite($value) && $value || "";
	$inputBox.val(theResult);
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




LOCATIONS:


var $sublocationCounter = 1;
$.fn.AddSublocation = function () {
	if ($sublocationCounter == 0 && !$(".nameSublocation").val()) {
		$(".nameError").html("Please input a name first");
		return;
	}
	else if (false) {
		//check if last sublocation is blank and make sure any other spans are blank
	}
	//check that the one on top is filled out
	else {
		$("span").html("");
	}
	console.log("Location site.js");

	var newSublocationID = 'Sublocations_' + $sublocationCounter + '_';
	console.log("newSublocationID: " + newSublocationID);
	var newSublocationName = 'Sublocations[' + $sublocationCounter + ']';
	console.log("newSublocationName: " + newSublocationName);
	var newSublocationClass = 'sublocationName' + $sublocationCounter;
	console.log("newSublocationClass: " + newSublocationClass);
	var sublocationHtml = '<div class="col-md-4">';
	sublocationHtml += '<label class="control-label">Sublocation ' + $sublocationCounter + ':</label>';
	sublocationHtml += '<input type="text" class="form-control" id="' + newSublocationID + '" name="' + newSublocationName + '" class="' + newSublocationClass + '" />';
	//sublocationHtml += '<input type="text" class="form-control" ' + newSublocationClass + '  />';
	var spanClass = 'spanSublocation' + $sublocationCounter;
	sublocationHtml += '<span class="text-danger ' + spanClass + '></span>"';
	sublocationHtml += '</div>';
	$(".addSublocation").append(sublocationHtml);
	$(".addSublocation").show();
	$sublocationCounter++;
}



//AJAX partial view submit for addLocations --> Sublocations
$("#LocationInstance_LocationTypeID").change(function () {
	var myDiv = $(".divSublocations");
	var selectedId = $(this).children("option:selected").val();
	console.log("selectedId: " + selectedId);
	$.ajax({
		//IMPORTANT: ADD IN THE ID
		url: "/Locations/SubLocation/?ParentLocationTypeID=" + selectedId,
		type: 'GET',
		cache: false,
		context: myDiv,
		success: function (result) {
			this.html(result);
		}
	});
	//$(document).ajaxStart(function () {
	//	$("#loading").show();
	//});

	//$(document).ajaxComplete(function () {
	//	$("#loading").hide();
	//});
});
	//.on("change", function (e) {
	//e.preventDefault();
	////show loading gif
	////$("#loading").show();
	//$.ajax({
	//	url: this.action,
	//	type: this.method,
	//	data: $(this).serialize(), //getting a model and serializing it
	//	success: function (data) {
	//		$("#divSublocations").html(data); //passing the model into dvResults
	//		//$("#loading").hide(); //hides the loading gif
	//	}
	//})
//})