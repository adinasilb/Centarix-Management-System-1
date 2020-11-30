
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
		"Request.SubUnit": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"Request.SubSubUnit": {
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
		"Request.UnitTypeID": "selectRequired",
		"Request.SubUnitTypeID": "selectRequired",
		"Request.SubSubUnitTypeID": "selectRequired"
	},
			});