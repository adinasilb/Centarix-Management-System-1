$.validator.addMethod("IsTodayOrEarlier", function (value, element) {
	return Date.parse($("#Invoice_InvoiceDate").val()) <= new Date();
}, 'Invoice Must Be Today or Earlier');


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
			//number: true,
			//min: 1,

		},
		"Invoice.InvoiceDate": {
			required: true,
			IsTodayOrEarlier: true
        }
	}
});
