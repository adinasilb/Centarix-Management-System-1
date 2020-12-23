
$.validator.addMethod("eitherHoursOrTimeAndTimeRangeMakesSenseEntry1", function (value, element) {
	return ($("#EmployeeHour_Exit1").val() != "" && $("#EmployeeHour_Entry1").val() != "") && ($('#EmployeeHour_Exit1').val() > $('#EmployeeHour_Entry1').val()) || $("#EmployeeHour_TotalHours").val() != "";
}, 'Either total hours or Entry1 and Entry 2 must be filled in. Entry Must Be Less Than Exit');

$.validator.addMethod("TimeRangeMakesSenseEntry2", function (value, element) {
	return $('#EmployeeHour_Exit2').val() > $('#EmployeeHour_Entry2').val();
}, 'Exit must be more than Entry');

$.validator.addMethod("eitherHoursOrTime", function (value, element) {
return ($("#EmployeeHour_Exit1").val() != "" && $("#EmployeeHour_Entry1").val() != "") || $("#EmployeeHour_TotalHours").val() != "";
}, 'Either total hours or Entry1 and Entry 2 must be filled in');

$.validator.addMethod("IfEntry2IsInView", function (value, element) {
	return $('#EmployeeHour_Exit2').val()!="" || $('#EmployeeHour_Entry2').val() !="";
}, 'Remove or fill out Entry and Exit 2');

$.validator.addMethod("Entry2NotGreaterThanExitTwo", function (value, element) {
	return $('#EmployeeHour_Entry2').val() > $('#EmployeeHour_Exit1').val();
}, 'Entry 2 must be more than Exit 2');


$('.UpdateHoursForm, .reportHoursForm').validate({
	rules: {
		"EmployeeHour.Entry1": {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true
		},
		"EmployeeHour.Exit1": {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true
		},
		"EmployeeHour.Entry2": {
			IfEntry2IsInView: true,
			Entry2NotGreaterThanExitTwo: true,	
			TimeRangeMakesSenseEntry2: true
		
		},
		"EmployeeHour.Exit2": {
			IfEntry2IsInView: true,
			TimeRangeMakesSenseEntry2: true
		
		},
		"EmployeeHour.TotalHours": {
			eitherHoursOrTime: true,
		},
		"EmployeeHour.EmployeeHoursStatusEntry1ID": {
			required: true,
		},
		"EmployeeHour.EmployeeHoursStatusEntry2ID": {
			required: true,
		}


	}
});
