
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
