
$('.receivedModalForm').validate({
	rules: {
		"Request.ArrivalDate": {
			required: true,
		},
		"Request.ApplicationUserReceiverID": {
			required: true,
		},
		"locationSelected": {
			required: true
		}

	},
	messages:{
		"locationSelected": "Please choose a location before submitting"
}
});
$('#myForm').data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible, #locationSelected), [disabled]';