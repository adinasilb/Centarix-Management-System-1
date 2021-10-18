/*
var isRequests = function () {
	return $("#masterSectionType").val() == "Requests";
}*/
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
		"locationTypeSelected": {
			locationRequired: true /*isRequests*/
		},
		"subLocationSelected": {
			locationRequired: true
		},
		"locationVisualSelected": {
			locationRequired: true
		}

	}
/*	messages: {
		"subLocationSelected": "Please choose a location before submitting",
		"locationVisualSelected": "Please choose a location before submitting",
		"locationTypeSelected": "Please choose a location before submitting"
}*/
});
$('#myForm').data("validator").settings.ignore = ':not(select:hidden, .location-error:hidden,input:visible, textarea:visible), [disabled]';