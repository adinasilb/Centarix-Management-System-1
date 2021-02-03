$('.uploadQuoteForm').validate({
	rules: {
		
		"Request.ParentQuote.QuoteNumber": {
			required: true
		},
		"Request.ParentQuote.QuoteDate": {
			//required: true,
			//date: true
		},
		}
});