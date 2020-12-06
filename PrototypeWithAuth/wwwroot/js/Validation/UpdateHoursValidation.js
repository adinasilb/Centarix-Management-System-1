
$.validator.addMethod("timeRangeMakeSenseEntry1", function (value, element) {
	return $('#EmployeeHour_Exit1').val() > $('#EmployeeHour_Entry1').val();
}, 'Entry must be less than Exit');


$.validator.addMethod("timeRangeMakeSenseEntry2", function (value, element) {
	return $('#EmployeeHour_Exit2').val() > $('#EmployeeHour_Entry2').val();
}, 'Entry must be less than Exit');


$('.UpdateHoursForm').validate({
	rules: {
		"EmployeeHour.Entry1": {
			eitherHoursOrTime: true,
			timeRangeMakeSenseEntry1: true
		},
		"EmployeeHour.Exit1": {
			eitherHoursOrTime: true,
			timeRangeMakeSenseEntry1: true
		},
		"EmployeeHour.Entry2": {
			eitherHoursOrTime: true,
			timeRangeMakeSenseEntry2: true
		},
		"EmployeeHour.Exit2": {
			eitherHoursOrTime: true,
			timeRangeMakeSenseEntry2: true
		},
		"EmployeeHour.TotalHours": {
			eitherHoursOrTime: true,
		},

	}
});