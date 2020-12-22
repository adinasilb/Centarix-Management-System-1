

$.validator.addMethod("eitherHoursOrTime", function (value, element) {
return ($("#EmployeeHour_Exit1").val() != "" && $("#EmployeeHour_Entry1").val() != "") || $("#EmployeeHour_TotalHours").val() != "";
}, 'Either total hours or Entry1 and Entry 2 must be filled in');


$('.UpdateHoursForm, .reportHoursForm').validate({
	rules: {
		"EmployeeHour.Entry1": {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true
		},
		"EmployeeHour.Exit1": {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true
		},
		"EmployeeHour.Entry2": {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry2: true
		},
		"EmployeeHour.Exit2": {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry2: true
		},
		"EmployeeHour.TotalHours": {
			eitherHoursOrTime: true,
		},

	}
});
