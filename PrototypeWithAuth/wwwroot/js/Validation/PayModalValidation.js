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
        }
        
	}
});