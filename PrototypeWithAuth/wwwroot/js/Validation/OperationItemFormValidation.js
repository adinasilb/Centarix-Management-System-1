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