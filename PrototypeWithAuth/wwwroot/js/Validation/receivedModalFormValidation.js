$('.receivedModalForm').validate({
	rules: {
		"Request.ArrivalDate": {
			required: true,
		},
		"Request.ApplicationUserReceiverID": {
			required: true,
		},
	}
});