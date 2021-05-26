$('.uploadQuoteForm').validate({
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
		"ExpectedSupplyDays": {
			required: true,
			min: 0,
			integer: true
        }
		//OrdersInput :{
		//	fileRequired : true			
		//}
	}
});


