
$('.termsModalForm').validate({
	rules: {
		Paid: "atLeastOneTerm",
		Terms: "atLeastOneTerm",
		Installments: "atLeastOneTerm",

	}

});