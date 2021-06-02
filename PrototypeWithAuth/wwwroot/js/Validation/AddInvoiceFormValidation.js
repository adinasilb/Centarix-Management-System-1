$('.addInvoiceForm').validate({
	 normalizer: function( value ) {
	  return $.trim( value );
	},
	rules: {
		"InvoiceImage": {
			fileRequired: true
		},
		"Invoice.InvoiceNumber": {
			required: true,
			number: true,
			min: 1,

		},
		"Invoice.InvoiceDate": {
			required: true,
			maxDate: new Date()
        }
	}
});
