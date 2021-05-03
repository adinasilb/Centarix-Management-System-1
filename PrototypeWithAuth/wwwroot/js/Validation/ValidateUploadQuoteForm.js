﻿$('.uploadQuoteForm').validate({
	rules: {
		 normalizer: function( value ) {
    return $.trim( value );
  },
		"ParentQuote.QuoteNumber": {
			required: true
		},
		"ParentQuote.QuoteDate": {
			required: true,
			maxDate: new Date()
		},
		QuotesInput :{
			fileRequired : true			
		},
		"ParentRequest.SupplierOrderNumber": {
			required: true
		},
		"ParentRequest.OrderDate": {
			required: true,
			maxDate: new Date()
		},
		//OrdersInput :{
		//	fileRequired : true			
		//}
	}
});

$.validator.addMethod("fileRequired", function (value, element) {
	console.log("in file required")
	return $(element).hasClass("contains-file");
}, 'Must upload a file before submitting');
