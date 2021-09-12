
$('.editQuoteDetails').validate({
	 normalizer: function( value ) {
    return $.trim( value );
  },
	rules: {
		"QuoteNumber": {
			required: true,
			//number: true,
			//min: 1
		},
		//"Request.ExpectedSupplyDays": {
		//	required: true,
		//	min: 0,
		//	integer: true
		//},
		"QuoteFileUpload": { required: true, extension: "jpg|jpeg|png|pdf" }
	},

});
$(".expected-supply-days").each(function () {
	$(this).rules("add", {
		required: true,
		min: 0,
		integer: true
	});
});

