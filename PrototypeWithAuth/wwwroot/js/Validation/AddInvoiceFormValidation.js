$('.addInvoiceForm').validate({
	 normalizer: function( value ) {
	  return $.trim( value );
	},
	rules: {
		"InvoiceImage": {
			required: true,
			extension: "jpg|jpeg|png|pdf"
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
