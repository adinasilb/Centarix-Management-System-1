$('.addInvoiceForm').validate({
	rules: {
		"InvoiceImage": { required: true, extension: "jpg|jpeg|png|pdf" },
		"Invoice.InvoiceNumber": {
			required: true,
			number: true,
			min: 1
		},
	}
});
