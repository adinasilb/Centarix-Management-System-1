
$('.editQuoteDetails').validate({
	 normalizer: function( value ) {
    return $.trim( value );
  },
	rules: {
		"ParentQuote.QuoteNumber": {
			required: true,
			//number: true,
			//min: 1
		},
		"ParentQuote.ExpirationDate": {
			required: true,
			mindate: new Date()
        },
		//"Request.ExpectedSupplyDays": {
		//	required: true,
		//	min: 0,
		//	integer: true
		//},
		"QuoteFileUpload": { required: true, extension: "jpg|jpeg|png|pdf|doc|docx|xls|xlsx|ppt|pptx" }
	},

});
$(".expected-supply-days").each(function () {
	$(this).rules("add", {
		required: true,
		min: 0,
		integer: true
	});
});

