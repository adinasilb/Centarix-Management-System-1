$('.payModalForm').validate({
	rules: {
        "paymentType": {
            required: true
        },
        "Payment.PaymentReferenceDate": {
            required: true,
            maxDate: new Date()
        },
        "Payment.CompanyAccountId": {
            selectRequired: true
        },
        "Payment.Reference": {
            selectRequired: true
        }
	}
});