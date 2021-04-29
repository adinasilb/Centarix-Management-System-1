
var isRequests = function () {
	return $("#masterSectionType").val() == "Requests";
}

$('.receivedModalForm').validate({
	 normalizer: function( value ) {
    return $.trim( value );
  },
	rules: {
		"Request.ArrivalDate": {
			required: true,
			maxDate: new Date()
		},
		"Request.ApplicationUserReceiverID": {
			required: true,
		},
		"locationSelected": {
			required: isRequests
		}

	},
	messages:{
		"locationSelected": "Please choose a location before submitting"
}
});
$('#myForm').data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible, #locationSelected), [disabled]';