
$('.editQuoteDetails').validate({
	 normalizer: function( value ) {
    return $.trim( value );
  },
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
		"QuoteFileUpload": { required: true, extension: "jpg|jpeg|png|pdf" }
	},

});
