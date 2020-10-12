

$(function () {
	function isInteger(n) {
		n = parseFloat(n);
		console.log(n)
		return n === +n && n === (n | 0);
	}
	$.validator.addMethod("selectRequired", function (value, element) {
		console.log("value" + value);
		return  $(element).val() != "";
	}, 'Field is required');
	$.validator.addMethod("integer", function (value, element) {
		console.log("value" + value);
		return isInteger(value) || value=='';
	}, 'Field must be an integer');

	$('.mdb-select').change(function () {
		$(this).valid();
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
		$.validator.setDefaults({
			ignore: []
		});
		
		var valid = $("#myForm").valid();
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
	$('.addItemForm').validate({
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

			
});

