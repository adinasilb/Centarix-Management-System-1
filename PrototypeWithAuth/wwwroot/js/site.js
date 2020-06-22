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
$("#Project").change(function () {
	console.log("in project change");
	var projectId = $(this).val();
	var url = "/Requests/GetSubProjectList";

	$.getJSON(url, { ProjectID: projectId }, function (data) {
		var item = "<option value=''>Select Subcategory</option>";
		$("#SubProject").empty();
		$.each(data, function (i, subproject) {
			item += '<option value="' + subproject.subProjectID + '">' + subproject.subProjectDescription + '</option>'
		});
		console.log(item);
		$("#SubProject").html(item);
	});
});


//search forms- Redo js in newer versions
$("#search-form #Project").change(function () {
	console.log("changed project");
});

$("#search-form #SubProject").change(function () {
	console.log("changed subproject");
});

$("#search-form #vendorList").change(function () {
	$("#search-form #vendorBusinessIDList").val($(this).val());
});

$("#search-form #vendorBusinessIDList").change(function () {
	$("#search-form #vendorList").val($(this).val());
});


var today = new Date();
var dd = today.getDate();
var mm = today.getMonth() + 1; //January is 0!
var yyyy = today.getFullYear();

if (dd < 10) { dd = '0' + dd }

var prevmm = mm;
var prevyyyy = yyyy;
//insert the payment lines
$("#Request_ParentRequest_Installments").change(function () {
	var installments = $(this).val();
	console.log("installments " + installments);
	var countPrevInstallments = $(".payment-line").length;
	console.log("countPrevInstallments " + countPrevInstallments);
	var difference = installments - countPrevInstallments;
	console.log("difference " + difference);


	console.log("dd: " + dd);


	if (difference > 0) { 
		console.log("installments increased");
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
			console.log("payment date: " + paymentDate);
			$.fn.AddNewPaymentLine(newIncrementNumber, paymentDate);
			newIncrementNumber++;
		};

		$.fn.AdjustPaymentDates();
	}
	else if (difference < 0) { //installments were removed
		console.log("installments decreased");
		for (x = difference; x < 0; x++) {
			$(".payments-table tr:last").remove();
		}
	}
});

$.fn.AddNewPaymentLine = function (increment, date) {
	var htmlTR = "";
	htmlTR += "<tr class='payment-line'>";
	htmlTR += "<td>";
	htmlTR += '<input class="form-control-plaintext border-bottom" type="date" data-val="true" data-val-required="The PaymentDate field is required." id="NewPayments_' + increment + '__PaymentDate" name="NewPayments[' + increment + '].PaymentDate" value="' + date + '" />';
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

$.fn.ShowResults = function ($inputBox, $value) { //this function ensures that the value passed back won't be NaN or undefined --> it'll instead send back a blank
	var theResult = parseFloat($value);
	theResult = theResult.toFixed(2);
	theResult = isFinite(theResult) && theResult || "";
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

$(".clicked-button").click(function () {
	$(".clicked-button").parent().removeClass("td-selected");
	$(this).parent().addClass("td-selected");
})

$(".load-sublocation-view").click(function () {
	//add or remove the background class in col 1
	//$(".load-sublocation-view").parent().removeClass("td-selected");
	//$(this).parent().addClass("td-selected");

	//fill up col 2 with the next one
	var myDiv = $(".colTwoSublocations");
	var parentId = $(this).val();
	//console.log("about to call ajax with a parentid of: " + parentId);
	$.ajax({
		//IMPORTANT: ADD IN THE ID
		url: "/Locations/SublocationIndex/?parentId=" + parentId,
		type: 'GET',
		cache: false,
		context: myDiv,
		success: function (result) {
			this.html(result);
		}
	});

	//fill up col three with the visual
	var visualDiv = $(".VisualBoxColumn");
	var visualContainerId = $(this).val();
	//console.log("about to call ajax with a visual container id of: " + visualContainerId);
	$.ajax({
		url: "/Locations/VisualLocations/?VisualContainerId=" + visualContainerId,
		type: 'GET',
		cache: false,
		context: visualDiv,
		success: function (result) {
			this.html(result);
		}
	});
});





function changeTerms(checkbox) {
	if (checkbox.checked) {
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
	$.fn.HideAllDocs();
	$(".orders-view").toggle();
});
$(".show-invoices-view").click(function () {
	$.fn.HideAllDocs();
	$(".invoices-view").toggle();
});
$(".show-shipments-view").click(function () {
	$.fn.HideAllDocs();
	$(".shipments-view").toggle();
});
$(".show-quotes-view").click(function () {
	$.fn.HideAllDocs();
	$(".quotes-view").toggle();
});
$(".show-info-view").click(function () {
	$.fn.HideAllDocs();
	$(".info-view").toggle();
});
$(".show-pictures-view").click(function () {
	$.fn.HideAllDocs();
	$(".pictures-view").toggle();
});
$(".show-returns-view").click(function () {
	$.fn.HideAllDocs();
	$(".returns-view").toggle();
});
$(".show-credits-view").click(function () {
	$.fn.HideAllDocs();
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