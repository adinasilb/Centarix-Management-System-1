

$('.reportHoursForm').validate({
	rules: {
		Entry1: {
			eitherHoursOrTime: true,
		},
		Exit1: {
			eitherHoursOrTime: true,
		},
		Entry2: {
			eitherHoursOrTime: true,
		},
		Exit2: {
			eitherHoursOrTime: true,
		},
		TotalHours: {
			eitherHoursOrTime: true,
		},
	}
});