
$(function () {
	$("body").off('click').on("click", ".upload-image", function (e) {
		console.log("in upload image");
		$.fn.CallModal2("/Admin/UserImageModal");
	});


	$("#NewEmployee_DOB").on("change", function () {
		var age = 0;
		console.log("age: " + age);
		var DOB = new Date($(this).val());
		//var Today = new Date().toJSON().slice(0, 10).replace(/-/g, '-');
		//console.log("dob: " + DOB);
		//console.log("today: " + Today);
		var today = new Date();
		age = Math.floor((today - DOB) / (365.25 * 24 * 60 * 60 * 1000));
		if (isNaN(age)) {
			age = '';
		}
		$("input[name='Age']").val(age);
	});

	$("#GeneratePassword").off('click').on("click", function () {
		var url = "/Admin/GetGeneratedPassword";
		var password = "";
		$.getJSON(url, function (data) {
			password = data;
			$("#Password").val(password);
			$("#ConfirmPassword").val(password);
		});
	});

	$("#ViewPassword").off('click').on("click", function () {
		console.log("view password")
		var passwordField = $("#Password");
		var type = passwordField.attr("type");
		switch (type) {
			case "password":
				passwordField.attr("type", "text");
				$(this).removeClass("users-filter");
				break;
			case "text":
				passwordField.attr("type", "password");
				$(this).addClass("users-filter");
				break;
			default:
				passwordField.attr("type", "password");
				break;
		}
	});

	$("#TimeSpan-HoursPerDay").on("change", function () {
		var newTimespan = $(this).val();
		var hours = parseInt(newTimespan.substr(0, 2));
		var minutes = newTimespan.substr(3, 2);
		console.log("minutes: " + minutes);
		var minutesFloat = parseFloat(minutes) / 60;
		console.log("minutesFloat: " + minutesFloat);
		var hoursPercentage = hours + minutesFloat;
		console.log("hoursPercentage: " + hoursPercentage);

		SetHiddenHoursPerDay(hoursPercentage);

		var percentageWorked = 100 * (hoursPercentage / 8.4).toFixed(4);
		console.log("percentage worked: " + percentageWorked);
		$("#NewEmployeeWorkScope").val(percentageWorked);
	});

	$("#NewEmployeeWorkScope").on("change", function () {
		var workScope = $(this).val();
		console.log("workScope: " + workScope);

		var hoursPerDay = (workScope / 100) * 8.4;
		console.log("hoursPerDay: " + hoursPerDay);

		SetHiddenHoursPerDay(hoursPerDay);

		var hours = Math.floor(hoursPerDay);
		if (hours < 10) { hours = '0' + hours }
		var mins = Math.floor(60 * (hoursPerDay - hours));

		//var timespanHours = (`${hours}:${mins}`);
		var timespanHours = hours + ":" + mins;
		console.log("timespanHours: " + timespanHours);

		$("#TimeSpan-HoursPerDay").val(timespanHours);
	});

	function SetHiddenHoursPerDay(hoursPercentage) {
		$("#NewEmployee_SalariedEmployee_HoursPerDay").val(hoursPercentage);
	};

});