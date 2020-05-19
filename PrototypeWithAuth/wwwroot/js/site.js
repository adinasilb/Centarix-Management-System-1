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
$("#price-tab").on("click", function () {
	console.log("$('#priceTab').click(function () {");
	$.fn.CheckEnableSubUnits();
	$.fn.CheckEnableSubSubUnits();
});

$("#currency").change(function () {
	console.log("currency was changed");
	$.fn.EnableSumDollarsOrShekel();
});

$("#Request_ExchangeRate").change(function () {
	$.fn.ChangeSumDollarsAndSumShekel();
	$.fn.CalculateUnitAmounts();
});
//enable the units and subunits tabs
$("#unit-amount").change(function () {
	$.fn.CheckEnableSubUnits();
	console.log("unit amounts 1");
	$.fn.CalculateUnitAmounts();
	$.fn.CalculateSubUnitAmounts(); //do we need an if statement here?
	$.fn.CalculateSubSubUnitAmounts(); //do we need an if statement here?

});
$("#unit-type").change(function () {
	console.log("unit type changed");
	$.fn.CheckEnableSubUnits();
});

$("#subunit-amount").change(function () {
	$.fn.CheckEnableSubSubUnits();
});
$("#subunit-type").change(function () {
	$.fn.CheckEnableSubSubUnits();
});

$.fn.CheckEnableSubUnits = function () {
	console.log("check enable subunits");
	if ($("#unit-amount").val() > 0 && $("#unit-type").val()) {
		$.fn.EnableSubUnits();
	}
	else {
		$.fn.DisableSubUnits();
		$.fn.DisableSubSubUnits();
	}
};

$.fn.CheckEnableSubSubUnits = function () {
	console.log("check enable subsubunits");
	if ($("#subunit-amount").val() > 0 && $("#subunit-type").val()) {
		$.fn.EnableSubSubUnits();
	}
	else {
		$.fn.DisableSubSubUnits();
	}
};

$.fn.EnableSubUnits = function () {
	console.log(" enable subunits");
	$(".RequestSubunitCard #subunit-amount").prop("disabled", false);
	$(".RequestSubunitCard #subunit-type").prop("disabled", false);
};

$.fn.EnableSubSubUnits = function () {
	console.log("check enable subsubunits");
	$(".RequestSubsubunitCard #subsubunit-amount").prop("disabled", false);
	$(".RequestSubsubunitCard #subsubunit-type").prop("disabled", false);
};

$.fn.DisableSubUnits = function () {
	console.log(" disable subunits");
	$(".RequestSubunitCard #subunit-amount").prop("disabled", true);
	$(".RequestSubunitCard #subunit-type").prop("disabled", true);
};

$.fn.DisableSubSubUnits = function () {
	console.log(" disable subsubunits");
	$(".RequestSubsubunitCard #subsubunit-amount").prop("disabled", true);
	$(".RequestSubsubunitCard #subsubunit-type").prop("disabled", true);
};


//enable or disable sumdollars and sumshekel
$.fn.EnableSumDollarsOrShekel = function () {
	console.log("in enable sum dollars or shekel fx");
	var currencyType = $("#currency").val();
	console.log("currencyType: " + currencyType);
	switch (currencyType) {
		case "dollar":
			console.log("in case dollar");
			$("#sumShekel").prop("disabled", true);
			$("#Request_Cost").prop("disabled", false);
			break;
		case "shekel":
			console.log("in case shekel");
			$("#sumShekel").prop("disabled", false);
			$("#Request_Cost").prop("disabled", true);
			break;
	}
}

//change sumDollars and sumSHekel
$.fn.ChangeSumDollarsAndSumShekel = function () {
	var $exchangeRate = $("#Request_ExchangeRate").val();
	if ($("#Request_Cost").prop("disabled")) {
		var $sumShekel = $("#sumShekel").val();
		var $sumDollars = $sumShekel / $exchangeRate;
		$inputBox = $('input[name="Request_Cost"]');
		$.fn.ShowResults($inputBox, $sumDollars);
	}
	else if ($("#sumShekel").prop("disabled")) {
		var $sumDollars = $("#Request_Cost").val();
		var $sumShekel = $sumDollars * $exchangeRate;
		$sumShekelsInputBox = $('input[name="sumShekel"]');
		console.log("$sumShekel: " + $sumShekel);
		$inputBox = $('input[name="sumShekel"]');
		$.fn.ShowResults($inputBox, $sumShekel);
		//$('input[name="sumShekel"]').val($sumShekel);
	}
	else {
		console.log("oops none are disabled");
	}
}

//Calculate unit amounts
$.fn.CalculateUnitAmounts = function () {
	var $unitAmounts = $("#unit-amount").val();
	console.log("$unitAmounts: " + $unitAmounts);
	var $sumDollars = $("#Request_Cost").val();
	console.log("Request_Cost: " + $sumDollars);
	var $exchangeRate = $("#Request_ExchangeRate").val();
	console.log("$exchangeRate: " + $exchangeRate);
	$unitSumDollars = $sumDollars / $unitAmounts;
	console.log("Request_Cost: " + $unitSumDollars);
	$inputBox = $('input[name="unit-price-dollars"]');
	$.fn.ShowResults($inputBox, $unitSumDollars);
	//$('input[name="unit-price-dollars"]').val($unitSumDollars);
	$unitSumShekel = $unitSumDollars * $exchangeRate;
	console.log("$unitSumShekel: " + $unitSumShekel);
	$inputBox2 = $('input[name="unit-price-shekel"]');
	$.fn.ShowResults($inputBox2, $unitSumShekel);
	//$('input[name="unit-price-shekel"').val($unitSumShekel);
};
//calculate sub unit amountss
$.fn.CalculateSubUnitAmounts = function () {
	var $subUnitAmounts = $("#subunit-amount").val();
	console.log("$subUnitAmounts: " + $subUnitAmounts);
	var $unitPriceDollars = $("#unit-price-dollars").val();
	console.log("$unitPriceDollars: " + $unitPriceDollars);
	var $exchangeRate = $("#Request_ExchangeRate").val();
	console.log("$exchangeRate: " + $exchangeRate);
	$subUnitSumDollars = $unitPriceDollars / $subUnitAmounts;
	console.log("$subUnitSumDollars: " + $subUnitSumDollars);
	$ipt = $('input[name="subunit-price-dollars"]');
	$.fn.ShowResults($ipt, $subUnitSumDollars);
	$subUnitSumShekel = $subUnitSumDollars * $exchangeRate;
	console.log("$subUnitSumShekel: " + $subUnitSumShekel);
	$ipt = $('input[name="subunit-price-shekel"');
	$.fn.ShowResults($ipt, $subUnitSumShekel);
};
//calculate sub sub unit amounts
$.fn.CalculateSubSubUnitAmounts = function () {
	var $subSubUnitAmounts = $("#subsubunit-amount").val();
	console.log("$subSubUnitAmounts: " + $subSubUnitAmounts);
	var $subUnitPriceDollars = $("#subunit-price-dollars").val();
	console.log("$subUnitPriceDollars: " + $subUnitPriceDollars);
	var $exchangeRate = $("#Request_ExchangeRate").val();
	console.log("$exchangeRate: " + $exchangeRate);
	$subSubUnitSumDollars = $subUnitPriceDollars / $subSubUnitAmounts;
	console.log("$subSubUnitSumDollars: " + $subSubUnitSumDollars);
	$ipt = $('input[name="subsubunit-price-dollars"]');
	$.fn.ShowResults($ipt, $subSubUnitSumDollars);
	$subSubUnitSumShekel = $subSubUnitSumDollars * $exchangeRate;
	console.log("$subSubUnitSumShekel: " + $subSubUnitSumShekel);
	$ipt = $('input[name="subsubunit-price-shekel"');
	$.fn.ShowResults($ipt, $subSubUnitSumShekel);
};


$.fn.ShowResults = function ($inputBox, $value) {
	var theResult = isFinite($value) && $value || "";
	$inputBox.val(theResult/*.toFixed(2)*/);//should set it to 2 decimal places
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
	sublocationHtml += '<input type="number" min="1" class="form-control" id="' + newSublocationID + '" name="locationRowColumn" class="' + newSublocationClass + '" />';
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

$(".load-sublocation-view").click(function () {
	//add or remove the background class in col 1
	$(".load-sublocation-view").removeClass("bg-secondary");
	$(this).addClass("bg-secondary");

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