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
        }
	},
	messages: {
		/*		"subLocationSelected": "Please choose a location before submitting",
				"locationVisualSelected": "Please choose a location before submitting",
				"locationTypeSelected": "Please choose a location before submitting",*/
		"Invoice.InvoiceNumber": {
			remote: "This invoice number already exists for the chosen vendor"
		}
	}
});
