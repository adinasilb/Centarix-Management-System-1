$.validator.addMethod("IsTodayOrEarlier", function (value, element) {
    return Date.parse($("#InvoiceInfoViewModel.Invoice.InvoiceDate").val()) <= new Date();
}, 'Invoice Must Be Today or Earlier');

$('.payModalForm').validate({
     normalizer: function( value ) {
    return $.trim( value );
  },
    rules: {
        "Payment.PaymentTypeID": {
            selectRequired: true
        },
        "Payment.CreditCardID": {
            selectRequired: true
        },
        "Payment.PaymentReferenceDate": {
            required: true
            //maxDate: new Date()
        },
        "Payment.CompanyAccountID": {
            selectRequired: true
        },
        "Payment.Reference": {
            required: true
        },
        "Payment.CheckNumber": {
            required: true,
            number: true
        },
        "InvoiceInfoViewModel.Invoice.InvoiceNumber": {
            required: true,
            remote: {
                url: '/Requests/CheckUniqueVendorAndInvoiceNumber',
                type: 'POST',
                data: {
                    "VendorID": function () { return $("#Vendor_VendorID").val() },
                    "InvoiceNumber": function () { return $("#InvoiceInfoViewModel_Invoice_InvoiceNumber").val() }
                },
            },
        },
        "InvoiceInfoViewModel.Invoice.InvoiceDate": {
            required: true,
            IsTodayOrEarlier: true
        },
        "Requests[0].Payments[0].Sum": {
            required: true,
            number: true,
            min: 1,
            max: function () {
                console.log($('.amtLeftToPay').val())
                return parseFloat($('.amtLeftToPay').val());
                }
        }
    },
    messages: {
        "Requests[0].Payments[0].Sum": {
            max: "Payment must be less than the amount left to pay"
        },
		"InvoiceInfoViewModel.Invoice.InvoiceNumber": {
            remote: "This invoice number already exists for the chosen vendor"
        }
        
	}
});