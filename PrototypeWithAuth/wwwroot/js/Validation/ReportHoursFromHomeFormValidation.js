$.validator.addMethod("eitherHoursOrTimeAndTimeRangeMakesSenseEntry1", function (value, element) {
	return ($("#Exit1").val() != "" && $("#Entry1").val() != "") && ($('#Exit1').val() > $('#Entry1').val()) || $("#TotalHours").val() != "";
}, 'Either total hours or Entry1 and Entry 2 must be filled in. Entry Must Be Less Than Exit');
$.validator.addMethod("eitherHoursOrTimeAndTimeRangeMakesSenseEntry2", function (value, element) {
	return ((($("#Exit1").val() != "" && $("#Entry1").val() != "") && ($('#Exit2').val() > $('#Entry2').val())) || $("#TotalHours").val() != "");
}, 'Either total hours or Entry1 and Entry 2 must be filled in. Entry Must Be Less Than Exit');


$('.reportHoursForm').validate({
	rules: {
		Entry1: {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true
		},
		Exit1: {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true
		},
		Entry2: {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry2: true
		},
		Exit2: {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry2: true
		},
		TotalHours: {
			eitherHoursOrTime: true,
		},

	}
});