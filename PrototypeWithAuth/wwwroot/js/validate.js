$(function () {

	$.validator.addMethod("selectRequired", function (value, element) {
		console.log("value" + value);
		return  $(element).val() != "";
	}, 'Field is required');

	$('.mdb-select').change(function () {
		console.log("mdb focus");
		$(this).valid();
	});
	$('.next-tab').click(function () {
		var valid = $("#myForm").valid();
		console.log("valid form: " + valid)
		if (!valid) {
			$('.next-tab').prop("disabled", true);
		}
		else {
			$('.next-tab').prop("disabled", false);
		}
		
	});
	$('#myForm').submit(function (e) {
		$.validator.setDefaults({
			ignore: []
		});
		var valid = $("#myForm").valid();

		console.log("valid form: " + valid)
		if (!valid) {
			e.preventDefault();
		}
		$.validator.setDefaults({
			ignore:':not(select:hidden, input:visible, textarea:visible)'
		});
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
	var addItemTab1 = {
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
			min: 1
		},
		"Request.ExpectedSupplyDays": {
			min: 0
		},
		"Request.Warranty": {
			min: 0
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
		},
		"Request.UnitTypeID": "selectRequired",
	};
	var addItemTab2 = {

		"Request.ExchangeRate": {
			required: true,
			number: true,
			min: 1
		},
		"Request.Unit": {
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
		},
		"Request.UnitTypeID": "selectRequired",

	};
	$('.addItemForm').validate({ // initialize the Plugin

		rules: addItemTab1,

			messages: {
      
		}
	
	
    //submitHandler: function (form) {
    //    alert('valid form submitted');
    //    return false;
    //}
	});



});


