
$.validator.addMethod("eitherHoursOrTimeAndTimeRangeMakesSenseEntry1", function (value, element) {
	return ($("#EmployeeHour_Exit1").val() != "" && $("#EmployeeHour_Entry1").val() != "") && ($('#EmployeeHour_Exit1').val() > $('#EmployeeHour_Entry1').val()) || $("#EmployeeHour_TotalHours").val() != "";
}, 'Either total hours or Entry1 and Exit1 must be filled in. Entry Must Be Less Than Exit');
$.validator.addMethod("TimeRangeMakesSenseEntry2", function (value, element) {
	return (($("#EmployeeHour_Exit1").val() != "" && $("#EmployeeHour_Entry1").val() != "") && ($('#EmployeeHour_Exit2').val() > $('#EmployeeHour_Entry2').val()));
}, 'Entry Must Be Less Than Exit');

$.validator.addMethod("eitherHoursOrTime", function (value, element) {
return ($("#EmployeeHour_Exit1").val() != "" && $("#EmployeeHour_Entry1").val() != "") || $("#EmployeeHour_TotalHours").val() != "";
}, 'Either total hours or Entry must be filled in');

$.validator.addMethod("IfEntry2IsInView", function (value, element) {
	return $('#EmployeeHour_Exit2').val()!="" || $('#EmployeeHour_Entry2').val() !="";
}, 'Remove or fill out Entry and Exit 2');

$.validator.addMethod("Entry2NotGreaterThanExitTwo", function (value, element) {
	return $('#EmployeeHour_Entry2').val() > $('#EmployeeHour_Exit1').val();
}, 'Entry 2 must be more than Exit 2');

$.validator.addMethod("validTime", function (value, element) {
	var t = value.split(':');
	if (t[0].length == 1) {
		value = "0" + value;
	}
	if (t[2] != null) {
		$("#"+element.id).val(t[0] + ":" + t[1]);
		console.log($("#" +element.id).val());
    }
	//console.log($("value" + value.length));
	var result = value.length == 0 || (/^\d\d:\d\d$/.test(value) &&
		t[0] >= 0 && t[0] < 24 &&
		t[1] >= 0 && t[1] < 60);
	return result;
}, "Invalid time");

$('.UpdateHoursForm').validate({
	rules: {
		"EmployeeHour.Entry1": {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true,
			validTime: true
		},
		"EmployeeHour.Exit1": {
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true,
			validTime: true
		},
		"EmployeeHour.Entry2": {
			IfEntry2IsInView: true,
			Entry2NotGreaterThanExitTwo: true,	
			TimeRangeMakesSenseEntry2: true,
			validTime: true
		},
		"EmployeeHour.Exit2": {
			IfEntry2IsInView: true,
			TimeRangeMakesSenseEntry2: true,
			validTime: true
		
		},
		"EmployeeHour.TotalHours": {
			eitherHoursOrTime: true,
			validTime: true
		},
		"EmployeeHour.EmployeeHoursStatusEntry1ID": {
			required: true,
		},
		"EmployeeHour.EmployeeHoursStatusEntry2ID": {
			required: true,
		},
		"EmployeeHour.PartialOffDayHours": {
			validTime: true
        }


	}
});
