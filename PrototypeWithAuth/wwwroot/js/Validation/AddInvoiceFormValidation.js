$.validator.addMethod("IsTodayOrEarlier", function (value, element) {
	return Date.parse($("#Invoice_InvoiceDate").val()) <= new Date();
}, 'Invoice Must Be Today or Earlier');


$('.addInvoiceForm').validate({
	 normalizer: function( value ) {
	  return $.trim( value );
	},
	rules: {
		"InvoiceImage": {
			fileRequired: true,
			/*extension: "jpg|jpeg|png|pdf|ppt|pptx" */
		},
		"Invoice.InvoiceNumber": {
			required: true,
			remote: {
				url: '/Requests/CheckUniqueVendorAndInvoiceNumber',
				type: 'POST',
				data: {
					"VendorID": function () { return $("#Vendor_VendorID").val() },
					"InvoiceNumber": function () { return $("#Invoice_InvoiceNumber").val() }
				},
			},

		},
		"Invoice.InvoiceDate": {
			required: true,
			IsTodayOrEarlier: true
		},
		"Sum": {
			number: true,
			required: true,
			min: 1
		},
		"VAT": {
			number: true,
			required: true,
			min: 0
		},
		"Shipping": {
			number: true,
			min: 0
		},
		"VATShipping": {
			number: true,
			min: 0
        },
		"Invoice.InvoiceDiscount": {
			number: true,
			min: 0
        }
	},
	messages: {
		"Invoice.InvoiceNumber": {
			remote: "This invoice number already exists for the chosen vendor"
		}
	}
});
