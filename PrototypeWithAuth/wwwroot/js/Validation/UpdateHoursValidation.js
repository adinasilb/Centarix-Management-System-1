
$.validator.addMethod("eitherHoursOrTimeAndTimeRangeMakesSenseEntry1", function (value, element) {
	if($('#EmployeeHour_Exit1').val()=="" || $('#EmployeeHour_Entry1').val() =="")
	{
		return true;
	}
	var date = "1970-01-01 ";
	var exit1moment = moment(date + $('#EmployeeHour_Exit1').val());
	var entry1moment = moment(date + $('#EmployeeHour_Entry1').val());
	return (entry1moment.isBefore(exit1moment));
}, 'Entry Must Be Less Than Exit');

$.validator.addMethod("TimeRangeMakesSenseEntry2", function (value, element) {
	var date = "1970-01-01 ";
	var exit2moment = moment(date + $('#EmployeeHour_Exit2').val());
	var entry2moment = moment(date + $('#EmployeeHour_Entry2').val());
	return (entry2moment.isBefore(exit2moment));
}, 'Entry Must Be Less Than Exit');

$.validator.addMethod("IfEntry2IsInView", function (value, element) {
	return $('#EmployeeHour_Exit2').val()!="" || $('#EmployeeHour_Entry2').val() !="";
}, 'Remove or fill out Entry and Exit 2');

$.validator.addMethod("Entry2NotGreaterThanExitTwo", function (value, element) {
	var date = "1970-01-01 ";
	var exit1moment = moment(date + $('#EmployeeHour_Exit1').val());
	var entry2moment = moment(date + $('#EmployeeHour_Entry2').val());
	return exit1moment.isBefore(entry2moment);
}, 'Entry 2 must be more than Exit 1');

$.validator.addMethod("validTime", function (value, element) {
	var t = value.split(':');
	if (t[0].length == 1) {
		value = "0" + value;
	}
	if (t[2] != null) {
		$("#"+element.id).val(t[0] + ":" + t[1]);
    }
	var result = value.length == 0 || (/^\d\d:\d\d$/.test(value) &&
		t[0] >= 0 && t[0] < 24 &&
		t[1] >= 0 && t[1] < 60);
	return result;
}, "Invalid time");

$.validator.addMethod("EntryExit1FilledInBeforeEntryExit2", function (value, element) {
	var entry1 = $('#EmployeeHour_Entry1').val() != "";
	console.log("Entry 1" + entry1);
	var exit1 = $('#EmployeeHour_Exit1').val() != "";
	var entry2 = $('#EmployeeHour_Entry2').val() != "";
	var result = true;
	if ((exit1 && !entry1) || (entry2 && !exit1) || (entry2 && !entry1)) {
		result = false;
	}
	return result
}, "Entry 1 must be filled out before Entry 2");

$('.UpdateHoursForm').validate({
	rules: {
		 normalizer: function( value ) {
    return $.trim( value );
  },
		"EmployeeHour.Entry1": {
			validTime: true,
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true,
			EntryExit1FilledInBeforeEntryExit2: true
		},
		"EmployeeHour.Exit1": {
			validTime: true,
			eitherHoursOrTimeAndTimeRangeMakesSenseEntry1: true,
			EntryExit1FilledInBeforeEntryExit2 : true
		},
		"EmployeeHour.Entry2": {
			validTime: true,
			IfEntry2IsInView: true,
			Entry2NotGreaterThanExitTwo: true,	
			TimeRangeMakesSenseEntry2: true,
			EntryExit1FilledInBeforeEntryExit2 : true
		},
		"EmployeeHour.Exit2": {
			validTime: true,
			IfEntry2IsInView: true,
			TimeRangeMakesSenseEntry2: true,
			EntryExit1FilledInBeforeEntryExit2 : true
		},
		"EmployeeHour.TotalHours": {
			validTime: true,
			required: true
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


	}, 
	messages : {
		"EmployeeHour.TotalHours": {
			required: "total hours must be greater than 0"
		},
		}

});
