

$(function () {
	$('.ordersItemForm').validate({
		rules: {
			"Request.Product.ProductName": "required",
			"Request.CatalogNumber": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},
			"Request.Product.ProductSubcategory.ParentCategoryID": "selectRequired",
			"Request.Product.ProductSubcategoryID": "selectRequired",
			"Request.SubProject.ProjectID": "selectRequired",
			"Request.SubProjectID": "selectRequired",
			"Request.Product.VendorID": "selectRequired",
			"Request.ParentQuote.QuoteNumber": {
				required: true,
				number: true,
				min: 1,
				integer: true
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
				required: true,
				number: true,
				min: 1
			},
			"Request.Unit": {
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
			"Request.UnitTypeID": "selectRequired"
		},
	});
	$('.operationsAddItemForm').validate({
		rules: {
			"Request.Product.ProductName": "required",
			"Request.CatalogNumber": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},
			"Request.Product.ProductSubcategory.ParentCategoryID": "selectRequired",
			"Request.Product.ProductSubcategoryID": "selectRequired",
			"Request.Product.VendorID": "selectRequired",
			"Request.ParentQuote.QuoteNumber": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},
			"Request.ExpectedSupplyDays": {
				min: 0,
				integer: true
			},
			"Request.Warranty": {
				min: 0,
				integer: true
			},
			"Request.ExchangeRate": {
				required: true,
				number: true,
				min: 1
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
			}
		},
	});
	$('.createVendorForm').validate({
		rules: {
			"Vendor.VendorEnName": "required",
			"Vendor.VendorHeName": "required",
			"VendorCategoryTypes": "selectRequired",
			"Vendor.VendorBuisnessID": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},
			"Vendor.VendorCountry": "required",
			"Vendor.VendorCity": "required",
			"Vendor.VendorStreet": "required",
			"Vendor_VendorTelephone": "required",
			"Vendor.OrdersEmail": "required",
			"Vendor.VendorBank": "required",
			"Vendor.VendorBankBranch": "required",
			"Vendor.VendorAccountNum": "required",
			
		},
	});
	function isInteger(n) {
		n = parseFloat(n)
		return n === +n && n === (n | 0);
	}
	$.validator.addMethod("selectRequired", function (value, element) {
		return  value != "";
	}, 'Field is required');
	$.validator.addMethod("integer", function (value, element) {
		return isInteger(value) || value=='';
	}, 'Field must be an integer');

	$('.mdb-select').change(function () {
		if ($(this).rules()) {
			$(this).valid();
		}
	
	});
	$('.next-tab').click(function () {
		if ($(this).hasClass('request-price')) {
			$('#Request_UnitTypeID').rules("remove", "selectRequired"); 
		}

		var valid = $("#myForm").valid();
	
		if (!valid) {
			$('.next-tab').prop("disabled", true);
		}
		else {
			$('.next-tab').prop("disabled", false);	
			
		}
		//work around for now - because select hidden are ignored
		if ($(this).hasClass('request-price')) {
			$('#Request_UnitTypeID').rules("add", "selectRequired"); 
		}

	});
	$('#myForm').submit(function (e)  {
		$(this).data("validator").settings.ignore = "";
		var valid = $(this).valid();
		console.log("valid form: " + valid)
		if (!valid) {
			e.preventDefault();
			$('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
		}
		else {
			$('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
		}
		$(this).data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
	});
	$.validator.setDefaults({
		ignore: ':not(select:hidden, input:visible, textarea:visible)',
		errorPlacement: function (error, element) {
			if (element.hasClass('select-dropdown')) {
				error.insertAfter(element);
			} else {
				error.insertAfter(element);
			}
		}
	});
	

			
});

//$.fn.validateUserDetailsTab = function () {
//	//all the true and falses are opposite because fo the ariainvalid is true if invalid
//	valid = $("#FirstName").attr('aria-invalid');
//	if (valid == "true" || $("#FirstName").val() == "") {
//		console.log("valid: " + valid);
//		return;
//	}
//	valid = $("#LastName").attr('aria-invalid');
//	if (valid == "true" || $("#LastName").val() == "") {
//		return;
//	}
//	valid = $("#Email").attr('aria-invalid');
//	if (valid == "true" || $("#Email").val() == "") {
//		console.log("valid: " + valid);
//		return;
//	}
//	valid = $("#SecureAppPass").attr('aria-invalid');
//	if (valid == "true" /*|| $("#SecureAppPass").val() == ""*/) {
//		return;
//	}
//	valid = $("#CentarixID").attr('aria-invalid');
//	if (valid == "true" || $("#CentarixID").val() == "") {
//		return;
//	}
//	valid = $("#Password").attr('aria-invalid');
//	if (valid == "true" /*|| $("#Password").val() == ""*/) {
//		return;
//	}
//	valid = $("#ConfirmPassword").attr('aria-invalid');
//	if (valid == "true" /*|| $("#ConfirmPassword").val() == ""*/) {
//		return;
//	}
//	if (!$('input[name="NewEmployee.EmployeeStatusID"]:checked').length) {
//		// none of the radio buttons were checked
//		return;
//	}
//	if (valid == "false" || valid == undefined) {
//		$("#user-permissions-tab").prop("disabled", false);
//		$("#user-budget-tab").prop("disabled", false);
//		$("#user-more-tab").prop("disabled", false);
//	}
//	return valid;
//}
//$("#SaveInvoiceModal").on("click", function (e) {
//	$("#myForm").valid();
//	var valid = $("#Invoice_InvoiceNumber").attr("aria-invalid");
//	console.log("invoice number validation + " + valid);
//	if (valid == "true" || $("#Invoice_InvoiceNumber") == "") {
//		e.preventDefault();
//		e.stopPropagation();
//		$("#invoice-number-validation").html("Please enter a valid Number");
//	}
//})


//$("#reorderRequest").click(function () {
//	console.log($("#reorderForm").valid());
//	if (!$("#reorderForm").valid()) {
//		$("#reorderRequest").prop("disabled", true);

//	}
//	$("#reorderRequest").prop("disabled", false);

//});


//$.fn.validatePersonalTab = function () {
//	console.log("validatePersonalTab");
//	$(".salary-tab").prop("disabled", true);
//	$(".documents-tab").prop("disabled", true);


//	var statIdVal = parseInt($("#NewEmployee_EmployeeStatusID").val());
//	console.log("vpt statIdVal: " + statIdVal);
//	if (parseInt(statIdVal) == 1 || statIdVal == 2 || statIdVal == 3) {
//		console.log("inside if statement");
//		//if (statIdVal != 4) {
//		//	console.log("statIdVal: " + statIdVal + " entered if statement");
//		if (/*checkedItem1 != true && checkedItem2 != true && checkedItem3 != true && checkedItem4 != true*/$('#NewEmployee_EmployeeStatusID').val() == '' || $('#NewEmployee_EmployeeStatusID').val() == 0) {
//			$("#validation-EmployeeStatus").removeClass("hidden");
//			return;
//		}
//		else {
//			$("#validation-EmployeeStatus").addClass("hidden");
//		}
//		valid = $("#NewEmployee_IDNumber").attr('aria-invalid');
//		if (valid == "true" || $("#NewEmployee_IDNumber").val() == "") {
//			console.log("valid: " + valid);
//			return;
//		}
//		valid = $("#NewEmployee_CentarixID").attr('aria-invalid');
//		if (valid == "true" || $("#NewEmployee_CentarixID").val() == "") {
//			console.log("valid: " + valid);
//			return;
//		}
//		valid = $("#NewEmployee_JobTitle").attr('aria-invalid');
//		if (valid == "true" || $("#NewEmployee_JobTitle").val() == "") {
//			return;
//		}
//		valid = $("#NewEmployee_Email").attr('aria-invalid');
//		if (valid == "true" || $("#NewEmployee_Email").val() == "") {
//			return;
//		}
//		valid = $("#NewEmployee_PhoneNumber").attr('aria-invalid');
//		if (valid == "true" || $("#NewEmployee_PhoneNumber").val() == "") {
//			return;
//		}
//		valid = $("#NewEmployee_PhoneNumber").attr('aria-invalid');
//		if (valid == "true") {
//			return;
//		}
//		valid = $("#NewEmployee_StartedWorking").attr('aria-invalid');
//		if (valid == "true" || $("#NewEmployee_StartedWorking").val() == "") {
//			return;

//		}
//		if (valid == "false" || valid == undefined) {
//			$(".salary-tab").prop("disabled", false);
//			$(".security-tab").prop("disabled", false);
//		}
//	}
//	else if (statIdVal == 4) {
//		$(".salary-tab").prop("disabled", false);
//	}
//	return valid;
//}

//$(".security-tab").on("click", function () {
//	if (parseInt($("#NewEmployee_EmployeeStatusID").val()) == 4) {
//		$(".security-tab").prop("disabled", false);
//	}
//	else {
//		$.fn.validateSalaryTab();
//	}
//});
//$.fn.validateSalaryTab = function () {
//	$(".security-tab").prop("disabled", true);
//	console.log("in $.fn.validateItemTab");
//	valid = $("#NewEmployee_SalariedEmployee_HoursPerDay").attr('aria-invalid');
//	if (valid == "true" || $("#NewEmployee_SalariedEmployee_HoursPerDay").val() == "") {
//		return;
//	}
//	valid = $("#NewEmployee_VacationDays").attr('aria-invalid');
//	if (valid == "true" || $("#NewEmployee_VacationDays").val() == "") {
//		return;
//	}
//	if (valid == "false" || valid == undefined) {
//		$(".security-tab").prop("disabled", false);
//	}
//	return valid;
//};
