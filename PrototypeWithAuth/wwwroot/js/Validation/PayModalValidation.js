$('.payModalForm').validate({
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
        }
	}
});