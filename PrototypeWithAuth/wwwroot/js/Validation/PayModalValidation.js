$('.payModalForm').validate({
	rules: {
        "paymentType": {
            required: true
        },
        "Payment.PaymentReferenceDate": {
            required: true,
            maxDate: new Date() //find out range
        },
        "Payment.CompanyAccountId": {
            required: true
        },
        "Payment.Reference": {
            required: true
        }
	}
});