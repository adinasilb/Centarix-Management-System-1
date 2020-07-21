// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//global Exchange Rate variable (usd --> nis)


function showmodal() {
	$("#modal").modal('show');
};

//modal adjust scrollability/height
//$("#myModal").modal('handleUpdate');

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

//change product subcategory dropdown according to the parent categroy selection when a parent category is selected
$(".Project").change(function () {

	var projectId = $(this).val();
	var url = "/Requests/GetSubProjectList";

	$.getJSON(url, { ProjectID: projectId }, function (data) {
		var item = "<option value=''>Select Subcategory</option>";
		$("#SubProject").empty();
		$.each(data, function (i, subproject) {
			item += '<option value="' + subproject.subProjectID + '">' + subproject.subProjectDescription + '</option>'
		});
		$("#SubProject").html(item);
	});

});


//search forms- Redo js in newer versions
$("#search-form #Project").change(function () {
});

$("#search-form #SubProject").change(function () {
	if ($(this).val() != 'Select a sub project') {
		$("#search-form #Project").attr("disabled", true);
	}
	else {
		$("#search-form #Project").attr("disabled", false);
	}
});

$("#search-form #sublist").change(function () {
	if ($(this).val() == 'Please select a subcategory') {
		$("#search-form #parentlist").attr("disabled", false);
	}
	else if ($(this).val() == '') {
		$("#search-form #parentlist").attr("disabled", false);
	}
	else {
		$("#search-form #parentlist").attr("disabled", true);
	}
});

$("#search-form #vendorList").change(function () {
	$("#search-form #vendorBusinessIDList").val($(this).val());
});

$("#search-form #vendorBusinessIDList").change(function () {
	$("#search-form #vendorList").val($(this).val());
});


var today = new Date();
var dd = today.getDate();
var mm = today.getMonth(); //January is 0 but do not add because everytime you create a new line it adds 1
var yyyy = today.getFullYear();

if (dd < 10) { dd = '0' + dd }

var prevmm = mm;
var prevyyyy = yyyy;
//insert the payment lines
$("#Request_ParentRequest_Installments").change(function () {
	var installments = $(this).val();
	var countPrevInstallments = $(".payment-line").length;
	var difference = installments - countPrevInstallments;


	if (difference > 0) {
		var newIncrementNumber = countPrevInstallments;
		for (x = difference; x > 0; x--) {

			var newmm = 0;
			var newyyyy = 0;
			if (prevmm < 12) {
				newmm = parseInt(prevmm);
				newmm = newmm + 1;
				newyyyy = prevyyyy;
			}
			else {
				newyyyy = parseInt(prevyyyy) + 1;
				newmm = 1;
			}

			if (newmm < 10) { newmm = '0' + newmm }

			var paymentDate = newyyyy + '-' + newmm + '-' + dd;

			prevyyyy = newyyyy;
			prevmm = newmm;
			$.fn.AddNewPaymentLine(newIncrementNumber, paymentDate);
			newIncrementNumber++;
		};

		$.fn.AdjustPaymentDates();
	}
	else if (difference < 0) { //installments were removed
		for (x = difference; x < 0; x++) {
			$(".payments-table tr:last").remove();
		}
	}
});


$.fn.AddNewPaymentLine = function (increment, date) {
	var htmlTR = "";
	htmlTR += "<tr class='payment-line'>";
	htmlTR += "<td>";
	htmlTR += '<input class="form-control-plaintext border-bottom payment-date" type="date" data-val="true" data-val-required="The PaymentDate field is required." id="NewPayments_' + increment + '__PaymentDate" name="NewPayments[' + increment + '].PaymentDate" value="' + date + '" />';
	htmlTR += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].PaymentDate" data-valmsg-replace="true"></span>';
	htmlTR += '</td>';
	htmlTR += '<td>';
	htmlTR += '<select class="form-control-plaintext border-bottom paymentType" id="NewPayments_' + increment + '__CompanyAccount_PaymentType" name="NewPayments[' + increment + '].CompanyAccount.PaymentType"><option value="">Select A Payment Type </option>';
	htmlTR += '<option value="1">Credit Card</option>';
	htmlTR += '<option value="2">Bank Account</option>';
	htmlTR += '</select>';
	htmlTR += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].CompanyAccount.PaymentType" data-valmsg-replace="true"></span>';
	htmlTR += '</td>';
	htmlTR += '<td>';
	var newPaymentsId = "NewPayments_" + increment + "__CompanyAccountID";
	var newPaymentsName = "NewPayments[" + increment + "].CompanyAccountID";
	htmlTR += '<select class="form-control-plaintext border-bottom companyAccountNum" id="' + newPaymentsId + '" name="' + newPaymentsName + '"></select>';
	htmlTR += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].CompanyAccount.CompanyAccountID" data-valmsg-replace="true"></span>';
	htmlTR += '</td>';
	htmlTR += '<td>';
	htmlTR += '<input class="form-control-plaintext border-bottom" type="number" data-val="true" data-val-required="The PaymentID field is required." id="NewPayments_' + increment + '__PaymentID" name="NewPayments[' + increment + '].PaymentID" value="" />';
	htmlTR += '<span class="text-danger field-validation-valid" data-valmsg-for="NewPayments[' + increment + '].PaymentID" data-valmsg-replace="true"></span>';
	htmlTR += '</td>';
	htmlTR += '</tr >';
	//$("body").append(htmlTR);
	$(".payments-table tr:last").after(htmlTR);


};

//since the paymentType field is dynamically created, the function needs to be bound the payments-table b/c js binds server-side
$(".payments-table").on("change", ".paymentType", function (e) {
	var paymentTypeID = $(this).val();
	var url = "/CompanyAccounts/GetAccountsByPaymentType";
	var id = "" + $(this).attr('id');
	var number = id.substr(12, 1);
	var newid = "NewPayments_" + number + "__CompanyAccountID";
	$.getJSON(url, { paymentTypeID: paymentTypeID }, function (data) {
		var item = "";
		$("#" + newid).empty();
		$.each(data, function (i, companyAccount) {
			item += '<option value="' + companyAccount.companyAccountID + '">' + companyAccount.companyAccountNum + '</option>'
		});
		$("#" + newid).html(item);
	});
});


$.fn.AdjustPaymentDates = function () {

};


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
	var firstNum = paymentTypeId.charAt(12);
	var secondNum = paymentTypeId.charAt(13);
	var numId = firstNum;
	if (secondNum != "_") {
		numId = firstNum + secondNum;
	}
	var companyAccountId = "NewPayments_" + numId + "__CompanyAccount";

	$.getJSON(url, { paymentTypeId: paymentTypeId }, function (data) {
		var item = "";
		$("#" + companyAccountId).empty();
		$.each(data, function (i, companyAccount) {
			item += '<option value="' + companyAccount.companyAccountId + '">' + companyAccount.companyAccountNum + ' - hello</option>'
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



$("#Request_Warranty").change(function () {
	var date = $("#Request_ParentRequest_InvoiceDate").val().split("-");
	console.log("Date: " + date);
	var dd = date[2];
	var mm = parseInt(date[1]); //January is 0! ?? do we still need th get month???
	var yyyy = date[0];
	var newmm = parseInt(mm) + parseInt($(this).val());
	mm = newmm;

	if (dd < 10) { dd = '0' + dd }

	var flag = true;
	while (flag) {
		if (mm > 12) {
			mm = parseInt(mm) - 12;
			yyyy = parseInt(yyyy) + 1;
		}
		else {
			flag = false;
		}
	}
	if (mm < 10) { mm = '0' + mm }

	var warrantyDate = yyyy + '-' + mm + '-' + dd;

	$("input[name='WarrantyDate']").val(warrantyDate);
});


$("#Request_ExpectedSupplyDays").change(function () {
	//var date = $("#Request_ParentRequest_InvoiceDate").val();
	//console.log("Order date: " + date);
	//console.log("this.val: " + $(this).val());
	//var supplyDate = Date.addDays($("#Request_ExpectedSupplyDays").val())
	//console.log("supplyDate: " + supplyDate);
	//$("input[name='expected-supply-days']").val(supplyDate);

});

$.fn.leapYear = function (year) {
	return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
}

//PRICE PAGE ON MODAL VIEW//
$("#price-tab").click(function () {
	$.fn.CheckUnitsFilled();
	$.fn.CheckSubUnitsFilled();
	//I don't think that we need $.fn.CheckSubSubUnitsFilled over here b/c we don't need to enable or disable anything and the CalculateSubSubUnits should already run
	$.fn.CalculateSumPlusVat();
	$.fn.CheckCurrency();
});

$("#currency").change(function (e) {
	$.fn.CheckCurrency();
});

$("#Request_ExchangeRate").change(function (e) {
	$.fn.CalculateSumPlusVat();
	$.fn.CalculateUnitAmounts();
	$.fn.CalculateSubUnitAmounts();
	$.fn.CalculateSubSubUnitAmounts();
});

$("#Request_Cost").change(function (e) {
	$.fn.CalculateSumPlusVat();
	$.fn.CalculateUnitAmounts();
	$.fn.CalculateSubUnitAmounts();
	$.fn.CalculateSubSubUnitAmounts();

});

$("#sum-dollars").change(function (e) {
	$.fn.CalculateSumPlusVat();
	$.fn.CalculateUnitAmounts();
	$.fn.CalculateSubUnitAmounts();
	$.fn.CalculateSubSubUnitAmounts();
	$.fn.updateDebt();
});

$("#Request_Unit").change(function () {
	$.fn.CheckUnitsFilled();
});

$("#Request_UnitTypeID").change(function () {
	$.fn.CheckUnitsFilled();
});

$("#Request_SubUnit").change(function () {
	$.fn.CheckSubUnitsFilled();
});

$("#Request_SubUnitTypeID").change(function () {
	$.fn.CheckSubUnitsFilled();
});

$("#Request_SubSubUnit").change(function () {
	$.fn.CheckSubUnitsFilled();
});

$("#Request_SubSubUnitTypeID").change(function () {
	$.fn.CheckSubUnitsFilled();
});

$.fn.CheckCurrency = function () {
	var currencyType = $("#currency").val();
	switch (currencyType) {
		case "dollar":
			$("#Request_Cost").prop("readonly", true);
			$("#sum-dollars").prop("disabled", false);
			break;
		case "shekel":
			$("#Request_Cost").prop("readonly", false);
			$("#sum-dollars").prop("disabled", true);
			break;
	}
};

$.fn.CheckUnitsFilled = function () {
	console.log("in check units function");
	if ($("#edit #Request_Unit").val() > 0 && $("#edit #Request_UnitTypeID").val()) {
		$.fn.EnableSubUnits();
		$.fn.ChangeSubUnitDropdown();
	}
	else {
		$.fn.DisableSubUnits();
		$.fn.DisableSubSubUnits();
	}
	$.fn.CalculateUnitAmounts();
	$.fn.CalculateSubUnitAmounts();
	$.fn.CalculateSubSubUnitAmounts();
};

$.fn.CheckSubUnitsFilled = function () {
	console.log("in check sub units function");
	if ($("#edit #Request_SubUnit").val() > 0 && $("#edit #Request_SubUnitTypeID").val()) {
		$.fn.EnableSubSubUnits();
		$.fn.ChangeSubSubUnitDropdown();
	}
	else {
		$.fn.DisableSubSubUnits();
	}
	$.fn.CalculateSubUnitAmounts();
	$.fn.CalculateSubSubUnitAmounts();
}


$.fn.EnableSubUnits = function () {
	$("#Request_SubUnit").prop("disabled", false);
	$("#Request_SubUnitTypeID").prop("disabled", false);
};

$.fn.EnableSubSubUnits = function () {
	$("#Request_SubSubUnit").prop("disabled", false);
	$("#Request_SubSubUnitTypeID").prop("disabled", false);
};

$.fn.DisableSubUnits = function () {
	$("#Request_SubUnit").prop("disabled", true);
	$("#Request_SubUnitTypeID").prop("disabled", true);
};

$.fn.DisableSubSubUnits = function () {
	$("#Request_SubSubUnit").prop("disabled", true);
	$("#Request_SubSubUnitTypeID").prop("disabled", true);
};

$.fn.CalculateUnitAmounts = function () {
	$unitSumShekel = parseFloat($("#Request_Cost").val()) / $("#Request_Unit").val();
	$iptBox = $("input[name='unit-price-shekel']");
	$.fn.ShowResults($iptBox, $unitSumShekel);
	var $exchangeRate = $("#Request_ExchangeRate").val();
	$unitSumDollars = $unitSumShekel / $exchangeRate;
	$iptBox = $("input[name='unit-price-dollars']");
	$.fn.ShowResults($iptBox, $unitSumDollars);
};

$.fn.CalculateSubUnitAmounts = function () {
	$subUnitSumShekel = $("#unit-price-shekel").val() / $("#Request_SubUnit").val();
	$iptBox = $("input[name='subunit-price-shekel']");
	$.fn.ShowResults($iptBox, $subUnitSumShekel);
	var $exchangeRate = $("#Request_ExchangeRate").val();
	$subUnitSumDollars = $subUnitSumShekel / $exchangeRate;
	$iptBox = $("input[name='subunit-price-dollars']");
	$.fn.ShowResults($iptBox, $subUnitSumDollars);
};

$.fn.CalculateSubSubUnitAmounts = function () {
	$subSubUnitSumShekel = $("#subunit-price-shekel").val() / $("#Request_SubSubUnit").val();
	$iptBox = $("input[name='subsubunit-price-shekel']");
	$.fn.ShowResults($iptBox, $subSubUnitSumShekel);
	var $exchangeRate = $("#Request_ExchangeRate").val();
	$subSubUnitSumDollars = $subSubUnitSumShekel / $exchangeRate;
	console.log("$subSubUnitSumShekel: " + $subSubUnitSumShekel);
	console.log("$exchangeRate: " + $exchangeRate);
	console.log("$subSubUnitSumDollars: " + $subSubUnitSumDollars);
	$iptBox = $("input[name='subsubunit-price-dollars']");
	$.fn.ShowResults($iptBox, $subSubUnitSumDollars);
};

$.fn.CalculateSumPlusVat = function () {
	var $exchangeRate = $("#Request_ExchangeRate").val();
	var vatInShekel = $("#Request_VAT").val();
	console.log("$('#Request_Cost').val(): " + $("#Request_Cost").val());
	if ($("#sum-dollars").prop("disabled")) {
		$sumDollars = parseFloat($("#Request_Cost").val()) / $exchangeRate;
		$iptBox = $('input[name="sum-dollars"]');
		$.fn.ShowResults($iptBox, $sumDollars);
	}
	else if ($("#Request_Cost").prop("readonly")) {
		$sumShekel = $("#sum-dollars").val() * $exchangeRate;
		$iptBox = $("input[name='Request.Cost']");
		$.fn.ShowResults($iptBox, $sumShekel);
	}
	$sumShekel = parseFloat($("#Request_Cost").val());
	$vatOnshekel = $sumShekel * parseFloat(vatInShekel);
	$sumTotalVatShekel = $sumShekel + $vatOnshekel;
	$iptBox = $("input[name='sumPlusVat-Shekel']");
	$.fn.ShowResults($iptBox, $sumTotalVatShekel);
	$sumTotalVatDollars = $sumTotalVatShekel / $exchangeRate;
	$iptBox = $("input[name='sumPlusVat-Dollar']");
	$.fn.ShowResults($iptBox, $sumTotalVatDollars);
};

$.fn.ChangeSubUnitDropdown = function () {
	var selected = $(':selected', $("#Request_UnitTypeID"));
	console.log("u selected: " + selected);
	var optgroup = selected.closest('optgroup').attr('label');
	console.log("u optgroup: " + optgroup);
	//the following is based on the fact that the unit types and parents are seeded with primary key values
	switch (optgroup) {
		case "Units":
			$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
			$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
			break;
		case "Weight/Volume":
			$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
			$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
			break;
		case "Test":
			$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
			$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
			break;
	}
};
//change sub sub unit dropdown
$.fn.ChangeSubSubUnitDropdown = function () {
	var selected = $(':selected', $("#Request_SubUnitTypeID"));
	console.log("su selected: " + selected);
	var optgroup = selected.closest('optgroup').attr('label');
	console.log("su optgroup: " + optgroup);
	switch (optgroup) {
		case "Units":
			$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
			$("#sRequest_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
			break;
		case "Weight/Volume":
			$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
			$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
			break;
		case "Test":
			$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
			$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
			break;
	}
};

$.fn.ShowResults = function ($inputBox, $value) { //this function ensures that the value passed back won't be NaN or undefined --> it'll instead send back a blank
	var theResult = parseFloat($value);
	theResult = theResult.toFixed(2);
	theResult = isFinite(theResult) && theResult || "";
	$inputBox.val(theResult);
}


//LOCATIONS:


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

//AJAX load full partial view for modalview manage locations
$("#locationTypeDepthZero").change(function () {
	var myDiv = $(".divSublocations");
	var selectedId = $(this).children("option:selected").val();
	$.ajax({
		//IMPORTANT: ADD IN THE ID
		url: "/Requests/ReceivedModalSublocations/?LocationTypeID=" + selectedId,
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

//Received Modal => fill up the next selectLocationInstance with the right selections
$(".SLI").change(function () {


	//ONE ---> GET THE NEXT DROPDOWNLIST

	var name = $(this).attr('name');
	var number = name.charAt(name.length - 2);
	var place = parseInt(number);

	console.log("")
	var nextSelectClass = name.replace(place.toString(), (place + 1).toString());
	var nextSelect = $("select[name='" + nextSelectClass + "']");

	var locationInstanceParentId = $(this).children("option:selected").val();
	var url = "/Requests/GetSublocationInstancesList";/*/?LocationInstanceParentID=" + locationInstanceParentId;*/

	if (nextSelect.length) { //if there is another one


		var NotFinished = true;
		var placeCounter = place + 1; //adding one to skip the select that we will fill in below
		var newNextSelect = nextSelect;

		//if the same one was selected jquery automatically deals with it
		while (NotFinished) {
			//var incrementedPlaceCounter = placeCounter + 1);
			nextSelectClass = nextSelectClass.replace(placeCounter.toString(), (placeCounter + 1).toString());
			newNextSelect = $("select[name='" + nextSelectClass + "']");
			if (newNextSelect.length) {
				newNextSelect.html("");
			}
			else {
				NotFinished = false;
			}
			placeCounter++;
		}


		$.getJSON(url, { locationInstanceParentId, locationInstanceParentId }, function (result) {
			var item = "<option>Select Location Instance</option>";
			$.each(result, function (i, field) {
				item += '<option value="' + field.locationInstanceID + '">' + field.locationInstanceName + '</option>'
			});
			nextSelect.html(item);
		});

	}
	//else {
	//	console.log("in the else unfortunately... :( ");
	//}


	//TWO ---> FILL VISUAL VIEW
	var myDiv = $(".visualView");
	if (locationInstanceParentId == 0) { //if zero was selected
		console.log("selected was 0");
		//check if there is a previous select box
		var oldSelectClass = name.replace(place.toString(), (place - 1).toString());
		var oldSelect = $("select[name='" + oldSelectClass + "']");
		if (oldSelect.length) {
			console.log("oldSelectClass " + oldSelectClass + " exists and refilling with that");
			var oldSelected = $("." + oldSelect).children("option:selected").val();
			console.log("oldSelected: " + oldSelected);
			$.ajax({
				url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + oldSelected,
				type: 'GET',
				cache: false,
				context: myDiv,
				success: function (result) {
					this.html(result);
				}
			});
		}
		else {
			console.log("oldSelectClass " + oldSelectClass + " does not exist and clearing");
			myDiv.html("");
		}
	}
	else {
		console.log("regular visual");
		$.ajax({
			url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + locationInstanceParentId,
			type: 'GET',
			cache: false,
			context: myDiv,
			success: function (result) {
				this.html(result);
			}
		});
	}


});

//$.fn.CallAjax() = function ($url, $div) {

//	$.ajax({
//		//IMPORTANT: ADD IN THE ID
//		url: $url,
//		type: 'GET',
//		cache: false,
//		context: $div,
//		success: function (result) {
//			this.html(result); //do we need to change this to $div?
//		}
//	});
//};


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

$(".visual-locations-zoom").on("click", function (e) {
	console.log("called visual locations zoom with an id of: " + $(this).val());
	var myDiv = $(".location-modal-view");
	console.log("myDiv: " + myDiv);
	var parentId = $(this).val();
	//console.log("about to call ajax with a parentid of: " + parentId);
	$.ajax({
		//IMPORTANT: ADD IN THE ID
		url: "/Locations/VisualLocationsZoom/?VisualContainerId=" + parentId,
		type: 'GET',
		cache: false,
		context: myDiv,
		success: function (result) {
			this.html(result);
			//turn off data dismiss by clicking out of the box and by pressing esc
			myDiv.modal({
				backdrop: 'static',
				keyboard: false
			});
			//shows the modal
			myDiv.modal('show');
			//bootstrap dynamically adds a class of modal-backdrop which must be taken off to make it clickable
			$(".modal-backdrop").remove();

		}
	});
});

$(".clicked-inside-button").click(function () {
	$(".clicked-inside-button").parent().removeClass("td-selected-inside");
	$(this).parent().addClass("td-selected-inside");
});

$(".clicked-outer-button").click(function () {
	$(".clicked-outer-button").parent().removeClass("td-selected-outer");
	$(".clicked-outer-button").parent().parent().removeClass("td-selected-outer");
	$(this).parent().addClass("td-selected-outer");
	$(this).parent().parent().addClass("td-selected-outer");
});



function changeTerms(checkbox) {
	if (checkbox.checked) {
		$(".installments").hide();
		console.log("checked");
		$("#Request_Terms").append(`<option value="${"0"}">${"Pay Now"}</option>`);;
		$("#Request_Terms").val("0");
		$("#Request_Terms").attr("disabled", true);
	}
	else {
		console.log("unchecked");
		$("#Request_Terms").attr("disabled", false);
		$("#Request_Terms option[value='0']").remove();
	}
}

$(".documents-tab").click(function () {
	//this is for validation
	$("#myForm").valid();
	$.fn.validatePriceTab();



	console.log("documents tab clicked");
	$.fn.HideAllDocs();
	$.fn.CheckIfFileSelectsAreFull();


});

//$(".upload-file-1").click(function () {
//	console.log("file select clicked 1");
//	$(".file-select-1").click();
//});
//$(".upload-file-2").click(function () {
//	console.log("file select clicked 2");
//	$(".file-select-2").click();
//});
//$(".upload-file-3").click(function () {
//	console.log("file select clicked 3 ");
//	$(".file-select-3").click();
//});
//$(".upload-file-4").click(function () {
//	console.log("file select clicked");
//	$(".file-select-4").click();
//});
//$(".upload-file-5").click(function () {
//	console.log("file select clicked");
//	$(".file-select-5").click();
//});
//$(".upload-file-6").click(function () {
//	console.log("file select clicked");
//	$(".file-select-6").click();
//});
//$(".upload-file-7").click(function () {
//	console.log("file select clicked");
//	$(".file-select-7").click();
//});
//$(".upload-file-8").click(function () {
//	console.log("file select clicked");
//	$(".file-select-8").click();
//});

$(".file-select").on("change", function (e) {
	console.log("file was changed");
	$cardDiv = $(this).closest("div.card");
	console.log("cardDiv: " + JSON.stringify($cardDiv));
	$cardDiv.addClass("document-border");
});

$.fn.HideAllDocs = function () {
	console.log("hide all docs");
	$(".orders-view").hide();
	$(".invoices-view").hide();
	$(".shipments-view").hide();
	$(".quotes-view").hide();
	$(".info-view").hide();
	$(".pictures-view").hide();
	$(".returns-view").hide();
	$(".credits-view").hide();
};

$(".show-orders-view").click(function () {
	$(".invoices-view").hide();
	$(".shipments-view").hide();
	$(".quotes-view").hide();
	$(".info-view").hide();
	$(".pictures-view").hide();
	$(".returns-view").hide();
	$(".credits-view").hide();
	$(".orders-view").toggle();
});
$(".show-invoices-view").click(function () {
	$(".orders-view").hide();
	$(".shipments-view").hide();
	$(".quotes-view").hide();
	$(".info-view").hide();
	$(".pictures-view").hide();
	$(".returns-view").hide();
	$(".credits-view").hide();
	$(".invoices-view").toggle();
});
$(".show-shipments-view").click(function () {
	$(".orders-view").hide();
	$(".invoices-view").hide();
	$(".quotes-view").hide();
	$(".info-view").hide();
	$(".pictures-view").hide();
	$(".returns-view").hide();
	$(".credits-view").hide();
	$(".shipments-view").toggle();
});
$(".show-quotes-view").click(function () {
	$(".orders-view").hide();
	$(".invoices-view").hide();
	$(".shipments-view").hide();
	$(".info-view").hide();
	$(".pictures-view").hide();
	$(".returns-view").hide();
	$(".credits-view").hide();
	$(".quotes-view").toggle();
});
$(".show-info-view").click(function () {
	$(".orders-view").hide();
	$(".invoices-view").hide();
	$(".shipments-view").hide();
	$(".quotes-view").hide();
	$(".pictures-view").hide();
	$(".returns-view").hide();
	$(".credits-view").hide();
	$(".info-view").toggle();
});
$(".show-pictures-view").click(function () {
	$(".orders-view").hide();
	$(".invoices-view").hide();
	$(".shipments-view").hide();
	$(".quotes-view").hide();
	$(".info-view").hide();
	$(".returns-view").hide();
	$(".credits-view").hide();
	$(".pictures-view").toggle();
});
$(".show-returns-view").click(function () {
	$(".orders-view").hide();
	$(".invoices-view").hide();
	$(".shipments-view").hide();
	$(".quotes-view").hide();
	$(".info-view").hide();
	$(".pictures-view").hide();
	$(".credits-view").hide();
	$(".returns-view").toggle();
});
$(".show-credits-view").click(function () {
	$(".orders-view").hide();
	$(".invoices-view").hide();
	$(".shipments-view").hide();
	$(".quotes-view").hide();
	$(".info-view").hide();
	$(".pictures-view").hide();
	$(".returns-view").hide();
	$(".credits-view").toggle();
});


//$(".close").click(function () {
//	console.log("close");
//	$(".modal").hide();
//	$(".modal").modal('hide');
//	$('.modal').replaceWith('');
//	//$.ajax({
//	//	async: true,
//	//	url: "Locations/Index",
//	//	type: 'GET',
//	//	cache: false,
//	//});
//});


$(".load-product-edit").on("click", function (e) {
	console.log("inside of load product edit")
	e.preventDefault();
	e.stopPropagation();
	//takes the item value and calls the Products controller with the ModalView view to render the modal inside
	var $itemurl = "Requests/EditModalView/?id=" + $(this).val();
	console.log("itemurl: " + $itemurl);
	$.fn.CallPage($itemurl, "edit");
	return false;
});


$.fn.updateDebt = function () {
	console.log("in update debt");
	var sum = $("#Request_Cost").val();
	var installments = $("#Request_ParentRequest_Installments").val();
	var tdate = new Date();
	var dd = tdate.getDate(); //yields day
	var MM = tdate.getMonth(); //yields month
	var yyyy = tdate.getFullYear(); //yields year
	if (dd < 10) {
		dd = "0" + dd
	}
	if (MM < 10) {
		MM = "0" + (MM + 1)
	} else {
		MM = MM + 1;
	}
	var today = yyyy + "-" + (MM) + "-" + dd;
	console.log("today:" + today);
	//count how many installment dates already passed
	var count = 0;
	if (installments != 0) {
		$(".payments-table .payment-date").each(function (index) {
			var date = $(this).val();
			console.log("date: " + date);
			if (today >= date) {
				console.log("today > date");
				//date passed
				count = count + 1
			}
		});
	}
	if (count != 0) {
		var paymentPerMonth = sum / installments;
		console.log(" paymentPerMonth: " + paymentPerMonth);
		var amountToSubstractFromDebt = paymentPerMonth * count;
		console.log(" amountToSubstractFromDebt: " + amountToSubstractFromDebt);
		var debt = sum - amountToSubstractFromDebt
		console.log(" sum: " + sum);
		console.log(" debt: " + debt);
		$("#Debt").val(debt);
	} else {
		$("#Debt").val(sum);
	}
};

$(".payments-table").on("change", ".payment-date", function (e) {
	console.log("in change .payments-table ");
	$.fn.updateDebt();
});

$("#Request_ExchangeRate").change(function (e) {
	console.log("in change #Request_ExchangeRate ");
	$.fn.updateDebt();
});


$("#sum-dollars").change(function (e) {
	console.log("in change #sum-dollars ");
	$.fn.updateDebt();
});

$("#Request_ParentRequest_Installments").change(function () {
	console.log("in change Request_ParentRequest_Installments ");
	$.fn.updateDebt();
});

$('#submitAddLocation').click(function () {
	$('#loading').show();
});




$(".load-location-index-view").click(function (e) {
	//clear the div to restart filling with new children
	$.fn.setUpLocationIndexList($(this).val())
});

$.fn.setUpLocationIndexList = function (val) {
	$("#loading3").show();
	//fill up col 2 with the next one
	var myDiv = $(".colOne");
	var typeId = val;
	//console.log("about to call ajax with a parentid of: " + parentId);
	$.ajax({
		//IMPORTANT: ADD IN THE ID
		url: "/Locations/LocationIndex/?typeId=" + typeId,
		type: 'GET',
		cache: false,
		context: myDiv,
		success: function (result) {
			$(".VisualBoxColumn").hide();
			$(".colTwoSublocations").hide();
			$("#loading3").hide();
			myDiv.show();
			this.html(result);

		}
	});

};


$(".load-sublocation-view").click(function (e) {
	//add or remove the background class in col 1
	//$(".load-sublocation-view").parent().removeClass("td-selected");
	//$(this).parent().addClass("td-selected");
	$("#loading1").show();
	//fill up col 2 with the next one
	var myDiv = $(".colTwoSublocations");
	var parentId = $(this).val();

	var parentsParentId = $(this).closest('tr').attr('name');

	if ($("#colTwoSublocations" + parentsParentId).length == 0) {
		$('.colTwoSublocations').append('<div class="colTwoSublocationsChildren" id="colTwoSublocations' + parentsParentId + '"></div>');
	}
	//console.log("about to call ajax with a parentid of: " + parentId);
	$.ajax({
		//IMPORTANT: ADD IN THE ID
		url: "/Locations/SublocationIndex/?parentId=" + parentId,
		type: 'GET',
		cache: false,
		context: $("#colTwoSublocations" + parentsParentId),
		success: function (result) {
			myDiv.show();
			$("#loading1").hide();
			this.html(result);

		}
	});

	$.fn.setUpVisual($(this).val());


});

$.fn.setUpVisual = function (val) {
	$("#loading2").show();
	//fill up col three with the visual
	var visualDiv = $(".VisualBoxColumn");
	var visualContainerId = val;
	//console.log("about to call ajax with a visual container id of: " + visualContainerId);
	$.ajax({
		url: "/Locations/VisualLocations/?VisualContainerId=" + visualContainerId,
		type: 'GET',
		cache: false,
		context: visualDiv,
		success: function (result) {
			visualDiv.show();
			this.html(result);
			$("#loading2").hide();
		}
	});
};

$.fn.validateItemTab = function () {
	$("#price-tab").prop("disabled", true);
	$("#location-tab").prop("disabled", true);
	$("#order-tab").prop("disabled", true);

	console.log("in $.fn.validateItemTab");
	valid = $("#parentlist").attr('aria-invalid');
	if (valid == "true" || $("#parentlist").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	valid = $("#Request_Product_ProductName").attr('aria-invalid');
	console.log("valid: " + valid);
	if (valid == "true" || $("#Request_Product_ProductName").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	console.log("valid1: " + valid);
	valid = $("#sublist").attr('aria-invalid');
	if (valid == "true" || $("#sublist").val() == "") {
		return;
	}
	console.log("valid2: " + valid);
	valid = $("#Request_SubProjectID").attr('aria-invalid');
	if (valid == "true" || $("#Request_SubProjectID").val() == "") {
		return;
	}
	console.log("valid3: " + valid);
	valid = $("#Request_SubProject_ProjectID").attr('aria-invalid');
	if (valid == "true" || $("#Request_SubProject_ProjectID").val() == "") {
		return;
	}
	console.log("valid4: " + valid);
	valid = $("#vendorList").attr('aria-invalid');
	if (valid == "true" || $("#vendorList").val() == "") {
		return;
	}
	console.log("valid5: " + valid);
	valid = $("#Request_ParentRequest_InvoiceNumber").attr('aria-invalid');
	if (valid == "true" || $("#Request_ParentRequest_InvoiceNumber").val() == "") {
		return;
	}
	console.log("valid5: " + valid);
	valid = $("#Request_Warranty").attr('aria-invalid');
	if (valid == "true") {
		return;
	}
	console.log("valid6: " + valid);
	valid = $("#Request_ParentRequest_InvoiceDate").attr('aria-invalid');
	if (valid == "true" || $("#Request_ParentRequest_InvoiceDate").val() == "") {
		return;
	}
	var validDate = true;
	var dateVal = $(".create-modal-invoice-date").val();
	console.log("date val: " + dateVal)
	if (dateVal != undefined) {

		validDate = $.fn.validateDateisGreaterThanOrEqualToToday(dateVal)
	}
	valid = "" + !validDate;
	console.log("valid date: " + valid);
	if (valid == "true" || $("#Request_ParentRequest_InvoiceDate").val() == "") {
		return;
	}
	console.log("valid7: " + valid);
	valid = $("#Request_ExpectedSupplyDays").attr('aria-invalid');
	if (valid == "true" || $("#Request_ExpectedSupplyDays").val() == "") {
		return;
	}
	valid = $("#Request_CatalogNumber").attr('aria-invalid');
	if (valid == "true" || $("#Request_CatalogNumber").val() == "") {
		return;
	}
	console.log("valid8: " + valid);
	if (valid == "false" || valid == undefined) {
		$("#price-tab").prop("disabled", false);
		$("#location-tab").prop("disabled", false);
		$("#saveEditModal").removeClass("disabled");
		$("#saveEditModal").prop("disabled", false);
		$("#order-tab").prop("disabled", false);
	}
	return valid;
}

$("#Request_ParentRequest_InvoiceDate").change(function () {

});
$('#myModal').change(
	function () {
		$.validator.unobtrusive.parse("#myForm");
	});


$("#price-tab").click(function () {
	console.log("in onclick price tab");
	$("#myForm").valid();
	$.fn.validateItemTab();
});

$("#saveEditModal").click(function () {
	console.log("in onclick save modal");
	$("#saveEditModal").addClass("disabled");
	$("#saveEditModal").prop("disabled", true);
	$("#myForm").valid();
	var valid = $.fn.validateItemTab();
	if (valid == "false" || valid == undefined) {
		valid = $.fn.validatePriceTab();
		if (valid == "false" || valid == undefined) {
			$("#saveEditModal").removeClass("disabled");
			$("#saveEditModal").prop("disabled", false);
			return true;
		}
	};
	$("#saveEditModal").removeClass("disabled");
	$("#saveEditModal").prop("disabled", false);
	return false;

});

$.fn.validateDateisGreaterThanOrEqualToToday = function (date) {
	var tdate = new Date();
	var dd = tdate.getDate(); //yields day
	var MM = tdate.getMonth(); //yields month
	var yyyy = tdate.getFullYear(); //yields year
	if (dd < 10) {
		dd = "0" + dd
	}
	if (MM < 10) {
		MM = "0" + (MM + 1);
	} else {
		MM = MM + 1;
	}
	var today = yyyy + "-" + (MM) + "-" + dd;
	console.log("today:" + today);
	//count how many installment dates already passed
	if (date < today) {
		return false;
	}
	return true;
};

$("#price-tab").click(function () {
	console.log("in onclick price tab");
	$("#myForm").valid();
	$.fn.validateItemTab();

});

$("#archive-tab").click(function () {
	console.log("in onclick archive-tab");
	$("#myForm").valid();
	$.fn.validatePriceTab();

});

$("#history-tab").click(function () {
	console.log("in onclick history-tab");
	$("#myForm").valid();
	$.fn.validateItemTab();
	$.fn.validatePriceTab();

});

$("#order-tab").click(function () {
	console.log("in onclick order-tab");
	$("#myForm").valid();
	var valid = $.fn.validateItemTab();
	if (valid == "false" || valid == undefined) {
		$.fn.validatePriceTab();
	};

});

$("#comments-tab").click(function () {
	console.log("in onclick comments-tab");
	$("#myForm").valid();
	$.fn.validatePriceTab();
});

$("#location-tab").click(function () {
	console.log("in onclick location-tab");
	$("#myForm").valid();
	$.fn.validateItemTab();
});


$("#Request_Terms").change(function () {
	console.log("in change Request_Terms");
	if ($(this).val() == -1) {
		$(".installments").hide();
	} else {
		$(".installments").show();
	}

});

$.fn.validatePriceTab = function () {
	//all the true and falses are opposite because fo the ariainvalid is true if invalid
	$("#item-tab").prop("disabled", true);
	$("#documents-tab").prop("disabled", true);
	$("#comments-tab").prop("disabled", true);
	$("#archive-tab").prop("disabled", true);
	$("#history-tab").prop("disabled", true);
	$("#order-tab").prop("disabled", true);
	valid = $("#Request_Product_ProductName").attr('aria-invalid');
	console.log("valid: " + valid);
	if (valid == "true" || $("#Request_Product_ProductName").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	valid = $("#Request_Unit").attr('aria-invalid');
	console.log("valid: " + valid);
	if (valid == "true" || $("#Request_Unit").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	console.log("valid1: " + valid);
	valid = $("#Request_UnitTypeID").attr('aria-invalid');
	if (valid == "true" || $("#Request_UnitTypeID").val() == "") {
		return;
	}
	console.log("valid2: " + valid);
	valid = $("#Request_ExchangeRate").attr('aria-invalid');
	if (valid == "true" || $("#Request_ExchangeRate").val() == "") {
		return;
	}
	console.log("valid3: " + valid);
	valid = $("#Request_Cost").attr('aria-invalid');
	if (valid == "true" || $("#Request_Cost").val() == "") {
		return;
	}
	console.log("final valid: " + valid);
	if ($("#Request_Terms").val() != -1) {
		console.log(".paymentType: " + $(".paymentType").val());
		if ($(".paymentType").val() != "") {
			valid = "false";
		} else {
			valid = "true";
		}
	}

	if (valid == "false" || valid == undefined) {
		$("#item-tab").prop("disabled", false);
		$("#documents-tab").prop("disabled", false);
		$("#comments-tab").prop("disabled", false);
		$("#archive-tab").prop("disabled", false);
		$("#history-tab").prop("disabled", false);
		$("#order-tab").prop("disabled", false);
	}
	return valid;
}
$("#profile-img").click(function () {
	$("#logout-form").toggle();
});


$(".create-user .permissions-tab").on("click", function () {
	console.log("permissions tab opened");
	$.fn.HideAllPermissionsDivs();
});

$(".edit-user .permissions-tab").on("click", function () {
	console.log("permissions tab opened");
	$.fn.HideAllPermissionsDivs();
});

$.fn.HideAllPermissionsDivs = function () {
	console.log("hide all permissions function entered");
	$(".orders-list").hide();
	$(".protocols-list").hide();
	$(".operations-list").hide();
	$(".biomarkers-list").hide();
	$(".timekeeper-list").hide();
	$(".lab-list").hide();
	$(".accounting-list").hide();
	$(".expenses-list").hide();
	$(".income-list").hide();
	$(".users-list").hide();
};

$(".back-to-main-permissions").on("click", function (e) {
	console.log("back to main permissions clicked");
	e.preventDefault();
	e.stopPropagation();
	$.fn.HideAllPermissionsDivs();
	$(".main-permissions").show();
});

$(".open-orders-list").on("click", function (e) {
	console.log("open orders lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".orders-list").show();
});

$(".open-protocols-list").on("click", function (e) {
	console.log("open protocols lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".protocols-list").show();
});

$(".open-operations-list").on("click", function (e) {
	console.log("open operations lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".operations-list").show();
});

$(".open-biomarkers-list").on("click", function (e) {
	console.log("open biomarkers lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".biomarkers-list").show();
});

$(".open-timekeepers-list").on("click", function (e) {
	console.log("open timekeeper lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".timekeeper-list").show();
});

$(".open-lab-list").on("click", function (e) {
	console.log("open lab lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".lab-list").show();
});

$(".open-accounting-list").on("click", function (e) {
	console.log("open accounting lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".accounting-list").show();
});

$(".open-expenses-list").on("click", function (e) {
	console.log("open expenses lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".expenses-list").show();
});

$(".open-income-list").on("click", function (e) {
	console.log("open income lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".income-list").show();
});

$(".open-users-list").on("click", function (e) {
	console.log("open users lsit clicked");
	e.preventDefault();
	e.stopPropagation();
	$(".main-permissions").hide();
	$(".users-list").show();
});

$("#vendor-payments-tab").click(function () {
	console.log("$('#vendor - payments - tab')");
	$("#createModalForm").validate();   
	$("#createModalForm").valid();
	$.fn.validateVendorDetailsTab();
});
$("#vendor-categories-tab").click(function () {
	console.log("$('#vendor - payments - tab')");
	$("#createModalForm").valid();
	$.fn.validateVendorDetailsTab();
});
$("#vendor-comment-tab").click(function () {
	$("#createModalForm").valid();
	$.fn.validateVendorPayment();
});
$("#vendor-contact-tab").click(function () {
	$("#createModalForm").valid();
	$.fn.validateVendorPayment();
});
$("#addSupplier").click(function () {
	$("#createModalForm").valid();
});

$.fn.validateVendorDetailsTab = function () {
	//all the true and falses are opposite because fo the ariainvalid is true if invalid
	valid = $("#Vendor_VendorEnName").attr('aria-invalid');
	console.log("valid: " + valid);
	if (valid == "true" || $("#Vendor_VendorEnName").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	valid = $("#Vendor_VendorHeName").attr('aria-invalid');
	console.log("valid: " + valid);
	if (valid == "true" || $("#Vendor_VendorHeName").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	console.log("valid1: " + valid);
	valid = $("#Vendor_VendorBuisnessID").attr('aria-invalid');
	if (valid == "true" || $("#Vendor_VendorBuisnessID").val() == "") {
		return;
	}
	console.log("valid2: " + valid);
	valid = $("#Vendor_VendorCountry").attr('aria-invalid');
	if (valid == "true" || $("#Vendor_VendorCountry").val() == "") {
		return;
	}
	console.log("valid3: " + valid);
	valid = $("#Vendor_VendorCity").attr('aria-invalid');
	if (valid == "true" || $("#Vendor_VendorCity").val() == "") {
		return;
	}
	console.log("valid1: " + valid);
	valid = $("#Vendor_VendorStreet").attr('aria-invalid');
	if (valid == "true" || $("#Vendor_VendorStreet").val() == "") {
		return;
	}
	console.log("valid2: " + valid);
	valid = $("#Vendor_VendorTelephone").attr('aria-invalid');
	if (valid == "true" || $("#Vendor_VendorTelephone").val() == "") {
		return;
	}
	console.log("valid3: " + valid);
	valid = $("#Vendor_VendorFax").attr('aria-invalid');
	if (valid == "true") {
		return;
	}
	console.log("valid1: " + valid);
	valid = $("#Vendor_OrdersEmail").attr('aria-invalid');
	if (valid == "true" || $("#Vendor_OrdersEmail").val() == "") {
		return;
	}
	console.log("valid2: " + valid);
	valid = $("#Vendor_InfoEmail").attr('aria-invalid');
	if (valid == "true") {
		return;
	}
	console.log("valid3: " + valid);
	valid = $("#Vendor_VendorCellPhone").attr('aria-invalid');
	if (valid == "true") {
		return;
	}
	if (valid == "false" || valid == undefined) {
		$("#vendor-payments-tab").prop("disabled", false);
		$("#vendor-categories-tab").prop("disabled", false);
	}
	return valid;
}
$.fn.validateVendorPayment = function () {
	//all the true and falses are opposite because fo the ariainvalid is true if invalid
	valid = $("#Vendor_VendorBank").attr('aria-invalid');
	console.log("valid: " + valid);
	if (valid == "true" || $("#Vendor_VendorBank").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	valid = $("#Vendor_VendorBankBranch").attr('aria-invalid');
	console.log("valid: " + valid);
	if (valid == "true" || $("#Vendor_VendorBankBranch").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	console.log("valid1: " + valid);
	valid = $("#Vendor_VendorAccountNum").attr('aria-invalid');
	if (valid == "true" || $("#Vendor_VendorAccountNum").val() == "") {
		return;
	}
	
	if (valid == "false" || valid == undefined) {
		$("#vendor-comment-tab").prop("disabled", false);
		$("#vendor-contact-tab").prop("disabled", false);
	}
	return valid;
}


$(".vendor-contact").change(function () {
	if ($("#createModalForm").valid()) {
		$("#addSupplier").prop("disabled", false);
		$("#addSupplier").removeClass("disabled-submit");
	}
});

$("#vendor-contact-tab").click(function () {
	console.log("in onclickcontact");
	$(".contact-info:hidden:first").find(".contact-active").val(true);
	$(".contact-info:hidden:first").show();
});

$("#addSuplierContact").click(function () {
	console.log("in onclick addSuplierContact");
	$(".contact-info:hidden:first").find(".contact-active").val(true);
	$(".contact-info:hidden:first").show();
});


$("#addSupplierComment").click(function () {

	$('[data-toggle="popover"]').popover({
		sanitize: false,
		placement: 'bottom',
		html: true,
		content: function () {
			return $('#popover-content').html();
		}
	});
	$('[data-toggle="popover"]').popover('toggle');

});

$("#addSupplier").click(function () {
	console.log($("#createModalForm").valid());
	if (!$("#createModalForm").valid()) {
		$("#addSupplier").prop("disabled", true);
	}
});

$("#addUser").click(function () {
	console.log($("#createModalForm").valid());
	if (!$("#createModalForm").valid()) {
		$("#addUser").prop("disabled", true);
	}
});




$.fn.addComment = function (type) {
	console.log("$('#Comment').click");
	$(".comment-info:hidden:first").find(".comment-active").val(true);
	$(".comment-info:hidden:first").find(".comment-type").val(type);
	$(".comment-info:hidden:first").show();
}

$("#user-permissions-tab").click(function () {
	$("#createModalForm").valid();
	$.fn.validateUserDetailsTab();
});
$("#user-budget-tab").click(function () {
	$("#createModalForm").valid();
});
$("#user-more-tab").click(function () {
	console.log('"#user-more-tab").click');
	$("#createModalForm").valid();
	$("#addUser").prop("disabled", false);
	$("#addUser").removeClass("disabled-submit");
});

$.fn.validateUserDetailsTab = function () {
	//all the true and falses are opposite because fo the ariainvalid is true if invalid
	valid = $("#FirstName").attr('aria-invalid');
	if (valid == "true" || $("#FirstName").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	valid = $("#LastName").attr('aria-invalid');
	if (valid == "true" || $("#LastName").val() == "") {
		return;
	}
	valid = $("#Email").attr('aria-invalid');
	if (valid == "true" || $("#Email").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	valid = $("#SecureAppPass").attr('aria-invalid');
	if (valid == "true" || $("#SecureAppPass").val() == "") {
		return;
	}
	valid = $("#CentarixID").attr('aria-invalid');
	if (valid == "true" || $("#CentarixID").val() == "") {
		return;
	}
	valid = $("#Password").attr('aria-invalid');
	if (valid == "true" || $("#Password").val() == "") {
		return;
	}
	valid = $("#ConfirmPassword").attr('aria-invalid');
	if (valid == "true" || $("#ConfirmPassword").val() == "") {
		return;
	}
	if (valid == "false" || valid == undefined) {
		$("#user-permissions-tab").prop("disabled", false);
		$("#user-budget-tab").prop("disabled", false);
		$("#user-more-tab").prop("disabled", false);
	}
	return valid;
}