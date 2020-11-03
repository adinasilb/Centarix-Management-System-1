

$(function () {
	$.validator.setDefaults({
		ignore: ':not(select:hidden, input:visible, textarea:visible)',
		errorPlacement: function (error, element) {
			if (element.hasClass('select-dropdown')) {
				error.insertAfter(element);
			} else {
				error.insertAfter(element);
			}
			if (element.hasClass('employee-status')) {
				$("#validation-EmployeeStatus").removeClass("hidden");
			}
		}
	});
	$('.addInvoiceForm').validate({
		rules: {
			"InvoiceImage": { required: true, extension: "jpg|jpeg|png|pdf" },
			"Invoice.InvoiceNumber": {
				required: true,
				number: true,
				min: 1
			},
		}
	});
	$('.addLocationForm').validate({
		rules: {
			"LocationInstance.LocationInstanceName": "required",
			"LocationInstance.LocationTypeID": {
				required: true,
				min: 1,
			},
			"LocationInstances[0].Height": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},
			"LocationInstances[0].Width": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},
			"LocationInstances[1].Height": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},
			"LocationInstances[2].Height": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},
			"LocationInstances[2].LocationTypeID": "selectRequired"
		}
	});
	$('.reorderForm').validate({
		rules: {

			"Request.Unit": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},
			"Request.Cost": {
				required: true,
				number: true,
				min: 1
			},

			"Request.UnitTypeID": "selectRequired"
		},

	});
	$('.editQuoteDetails').validate({
		rules: {
			"Request.ParentQuote.QuoteNumber": {
				required: true,
				number: true,
				min: 1
			},
			"Request.ExpectedSupplyDays": {
				min: 0,
				integer: true
			},
			"QuoteFileUpload": "required"
		},

	});
	$(".cost-validation").each(function () {
		$(this).rules("add", {
			required: true,
			number: true,
			min: 1
		});
	});
	$(".supply-days-validation").each(function () {
		$(this).rules("add", {
			min: 0,
			integer: true
		});
	});

	$('.ordersItemForm').validate({
		rules: {
			"Request.Product.ProductName": "required",
			"Request.CatalogNumber": {
				required: true,
				number: true,
				min: 1
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
	$('.reportHoursForm').validate({
		rules: {
			Entry1: {
				eitherHoursOrTime: true,
			},
			Exit1: {
				eitherHoursOrTime: true,
			},
			Entry2: {
				eitherHoursOrTime: true,
			},
			Exit2: {
				eitherHoursOrTime: true,
			},
			TotalHours: {
				eitherHoursOrTime: true,
			},
		}
	});
	$('.receivedModalForm').validate({
		rules: {
			"Request.ArrivalDate": {
				required: true,
			},
			"Request.ApplicationUserReceiverID": {
				required: true,
			},
		}
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
				required: function () {
					return $("#currency").val() == "dollar" || $("#currency").val() == null;
				},
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
				min: 1
			},
			"Vendor.VendorCountry": "required",
			"Vendor.VendorCity": "required",
			"Vendor.VendorStreet": "required",
			"Vendor.VendorTelephone": {
				required: true,
				minlength: 9
			},
			"Vendor.VendorCellPhone": {
				minlength: 9
			},
			"Vendor.VendorFax": {
				minlength: 9
			},
			"Vendor.OrdersEmail": {
				email: true,
				required: true
			},
			"Vendor.InfoEmail": {
				email: true
			},
			"Vendor.VendorBank": "required",
			"Vendor.VendorBankBranch": "required",
			"Vendor.VendorAccountNum": {
				required: true,
				number: true,
				min: 1,
				integer: true
			},


		},
	});
	$.validator.addMethod("nonAlphaNumeric", function (value) {
		return /^[a-zA-Z0-9]+$/.test(value) == false;
	}, "Password must contain a non alphanumeric character ");
	$.validator.addMethod("uppercase", function (value, element) {
		if (this.optional(element)) {
			return true;
		}
		return /[A-Z]/.test(value);
	}, "Must contain uppercase");
	$.validator.addMethod("lowercase", function (value, element) {
		if (this.optional(element)) {
			return true;
		}
		return /[a-z]/.test(value);
	}, "Must contain lowercase");
	$.validator.addMethod("containsNumber", function (value, element) {
		if (this.optional(element)) {
			return true;
		}
		return /[0-9]/.test(value);
	}, "Password must contain at least one number ");
	$.validator.addMethod("selectRequired", function (value, element) {
		console.log("in select required")
		return value != "";
	}, 'Field is required');
	$.validator.addMethod("atleastOneHoursField", function (value, element) {
		console.log("salary: " + ($("#NewEmployee_SalariedEmployee_WorkScope").val() != "") || ($("#NewEmployee_SalariedEmployee_HoursPerDay").val() != ""));
		return ($("#NewEmployee_SalariedEmployee_WorkScope").val() != "") || ($("#NewEmployee_SalariedEmployee_HoursPerDay").val() != "") || $("#NewEmployee_EmployeeStatusID").val() == "4";
	}, 'Either Job Scope or Hours Per day is required');
	$.validator.addMethod("eitherHoursOrTime", function (value, element) {
		return ($("#Exit1").val() != "" && $("#Entry1").val() != "") || $("#TotalHours").val() != "";
	}, 'Either total hours or Entry1 and Entry 2 must be filled in');
	$.validator.addMethod("integer", function (value, element) {
		return isInteger(value) || value == '';
	}, 'Field must be an integer');

	$('.mdb-select').change(function () {
		if ($(this).rules()) {
			$(this).valid();
		}

	});
	var isEmployee = function () {
		console.log("employeestatus: " + ($("#NewEmployee_EmployeeStatusID").val() != '4'));
		return $("#NewEmployee_EmployeeStatusID").val() != "4";
	}
	var isUserAndIsNotEdit = function () {
		return $("#NewEmployee_EmployeeStatusID").val() == "4" && $('#myForm').hasClass('editUser') == false;
	}

	var isUser = function () {
		return $("#NewEmployee_EmployeeStatusID").val() == "4";
	}
	$('.usersForm').validate({
		rules: {
			"FirstName": "required",
			"LastName": "required",
			"CentarixID": {
				required: true,
				//number: true,
				minlength: 1,
				//integer: true
			},
			"Email": {
				email: true,
				required: true
			},
			"PhoneNumber": {
				required: true,
				minlength: 9
			},
			"NewEmployee.JobTitle": {
				required: isEmployee,
			},
			"NewEmployee.DOB": {
				required: isEmployee,
				date: true
			},
			"NewEmployee.JobCategoryTypeID": {
				selectRequired: isEmployee,
			},
			"NewEmployee.DegreeID": {
				selectRequired: isEmployee,
			},
			"NewEmployee.MaritalStatusID": {
				selectRequired: isEmployee,
			},
			"NewEmployee.CitizenshipID": {
				selectRequired: isEmployee,
			},
			"NewEmployee.IDNumber": {
				required: isEmployee,
				number: true,
				min: 1,
				integer: true
			},
			"PhoneNumber2": {
				minlength: 9
			},
			"NewEmployee.StartedWorking": {
				required: isEmployee,
			},
			"NewEmployee.TaxCredits": {
				number: true,
				integer: true
			},
			"NewEmployee.SalariedEmployee.WorkScope": {
				atleastOneHoursField: true,
			},
			"NewEmployee.SalariedEmployee.HoursPerDay": {
				atleastOneHoursField: true,
				number: true
			},
			"NewEmployee.VacationDays": {
				required: isEmployee,
				number: true,
				integer: true
			},
			"Password": {
				required: isUserAndIsNotEdit,
				nonAlphaNumeric: true,
				uppercase: true,
				lowercase: true,
				containsNumber: true,
				minlength: 8,
				maxlength: 20
			},
			"SecureAppPass": {
				required: isUserAndIsNotEdit || function () {
					return $('#Password').val() != '';
				}
				//todo: are we allowing edit of secure appp password
				// validate format
			},
			"NewEmployee.EmployeeStatusID": {
				required: true,
				min: 1
			},
			//UserImage: { extension: "jpg|jpeg|png" },
			LabMonthlyLimit: {
				integer: true
			},
			LabUnitLimit: {
				integer: true
			},
			LabOrderLimit: {
				integer: true
			},
			OperationMonthlyLimit: {
				integer: true
			},
			OperationUnitLimit: {
				integer: true
			},
			OperaitonOrderLimit: {
				integer: true
			},
		},
		messages: {
			"NewEmployee.EmployeeStatusID": {
				required: "",
				min: "",
			},
		}
	});

	$('.resetPasswordForm').validate({
		rules: {
			"Password": {
				required: true,
				nonAlphaNumeric: true,
				uppercase: true,
				lowercase: true,
				containsNumber: true,
				minlength: 8,
				maxlength: 20
			},
			"ConfirmPassword": {
				required: true,
				equalTo: "#Password"
			},
			"TwoFactorAuthenticationViewModel.Code": {
				required: true,
				integer: true,
				number: true
			}
		}
	});

	function isInteger(n) {
		n = parseFloat(n)
		return n === +n && n === (n | 0);
	}

	$('.modal').on('change', '.mdb-select', function () {
		console.log("mdb change .mdb-select")
		if ($(this).rules()) {
			$(this).valid();
		}

	});
	$('#myForm input').focusout(function (e) {
		$("#myForm").data("validator").settings.ignore = "";
		$('.error').addClass("beforeCallValid")
		if ($('#myForm').valid()) {

			$('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
		} else {
			$(".error:not(.beforeCallValid)").addClass("afterCallValid")
			$(".error:not(.beforeCallValid)").removeClass("error")
			$("label.afterCallValid").remove()
			$(".error").removeClass('beforeCallValid')
			$(".afterCallValid").removeClass('error')
			$(".afterCallValid").removeClass('afterCallValid')
			if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
				$('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
			}
		}
		$("#myForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
	});

	$('.next-tab').click(function () {
		if ($(this).hasClass('request-price')) {
			$('#Request_UnitTypeID').rules("remove", "selectRequired");
		}

		//change previous tabs to accessible --> only adding prev-tab in case we need to somehow get it after
		$(this).parent().prev().find(".next-tab").addClass("prev-tab");

		if (!$(this).hasClass("prev-tab")) {
			var valid = $("#myForm").valid();

			console.log("valid tab" + valid)
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
		}


	});
	$('#myForm').submit(function (e) {
		//alert("validate form");
		$(this).data("validator").settings.ignore = "";
		var valid = $(this).valid();
		console.log("valid form: " + valid)
		if (!valid) {
			e.preventDefault();
			if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
				$('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
			}

		}
		else {
			$('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
		}
		$(this).data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
	});
	$('.modal #myForm').submit(function (e) {
		//alert("validate form");
		$(this).data("validator").settings.ignore = "";
		var valid = $(this).valid();
		console.log("valid form: " + valid)
		if (!valid) {
			e.preventDefault();
			if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
				$('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
			}

		}
		else {
			$('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
		}
		$(this).data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
	});




});
