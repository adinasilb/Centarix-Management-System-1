

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
			"Vendor.VendorTelephone": {
				required : true,
				minlength : 9
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

	var isEmployee = function () {
		return $("#NewEmployee_EmployeeStatusID").val() != "4";
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
				number: true,
				min: 1,
				integer: true
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
				required: isEmployee
			},
			"NewEmployee.DOB": {
				required: isEmployee,
				date: true
			},
			"NewEmployee.JobCategoryTypeID": {
				selectRequired: isEmployee
			},
			"NewEmployee.DegreeID": {
				selectRequired: isEmployee
			},
			"NewEmployee.MaritalStatusID": {
				selectRequired: isEmployee
			},
			"NewEmployee.CitizenshipID": {
				selectRequired: isEmployee
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
				integer : true
			},
			"NewEmployee.SalariedEmployee.WorkScope": {
				atleastOneHoursField: isEmployee,
			},
			"NewEmployee.SalariedEmployee.HoursPerDay": {
				atleastOneHoursField: isEmployee,
			},
			"NewEmployee.VacationDays": {
				required: isEmployee,
				number: true,
				integer : true
			},
			"Password": {
				required: isUser,				
				nonAlphaNumeric: true,
				uppercase: true,
				lowercase: true,
				containsNumber: true,
				minlength: 8,
				maxlength : 20
			},
			"ConfirmPassword": {
				required: isUser,
				equalTo: "#Password"
			},
			"SecureAppPass": {
				required: isUser
				// validate format
			},
			"NewEmployee.EmployeeStatusID": {
				required: true,
				min : 1
			}
		},
		messages: {
			"NewEmployee.EmployeeStatusID": {
				required: "",
				min: "",
			},
		}
	});


	function isInteger(n) {
		n = parseFloat(n)
		return n === +n && n === (n | 0);
	}
	$.validator.addMethod("nonAlphaNumeric", function (value) {
		return /^[a-zA-Z0-9]+$/.test(value)==false;
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
		return  value != "";
	}, 'Field is required');
	$.validator.addMethod("atleastOneHoursField", function (value, element) {
		return $("#NewEmployee_SalariedEmployee_WorkScope").val() != "" || $("#NewEmployee_SalariedEmployee_HoursPerDay").val() != "";
	}, 'Either Job Scope or Hours Per day is required');
	$.validator.addMethod("integer", function (value, element) {
		return isInteger(value) || value=='';
	}, 'Field must be an integer');

	$('.mdb-select').change(function () {
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


