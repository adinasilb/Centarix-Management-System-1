﻿$.validator.addMethod("timeRangeMakeSenseEntry1", function (value, element) {
	return $('#Exit1').val() > $('#Entry1').val();
}, 'Entry must be less than Exit');


$.validator.addMethod("timeRangeMakeSenseEntry2", function (value, element) {
	return $('#Exit2').val() > $('#Entry2').val();
}, 'Entry must be less than Exit');


$('.reportHoursForm').validate({
	rules: {
		Entry1: {
			eitherHoursOrTime: true,
			timeRangeMakeSenseEntry1: true
		},
		Exit1: {
			eitherHoursOrTime: true,
			timeRangeMakeSenseEntry1: true
		},
		Entry2: {
			eitherHoursOrTime: true,
			timeRangeMakeSenseEntry2: true
		},
		Exit2: {
			eitherHoursOrTime: true,
			timeRangeMakeSenseEntry2: true
		},
		TotalHours: {
			eitherHoursOrTime: true,
		},

	}
});