$(function () {
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

	$("#GeneratePassword").on("click", function () {
		var url = "/Admin/GetGeneratedPassword";
		var password = "";
		$.getJSON(url, function (data) {
			password = data;
			$("#Password").val(password);
			$("#ConfirmPassword").val(password);
			console.log("password: " + password);
		});
	});

	$("#ViewPassword").on("click", function () {
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
		console.log("newTimespan: " + newTimespan);
		var hours = parseInt(newTimespan.substr(0, 2));
		console.log("hours: " + hours);
		var minutes = newTimespan.substr(3, 2);
		console.log("minutes: " + minutes);
		var minutesFloat = parseFloat(minutes) / 60;
		console.log("minutesFloat: " + minutesFloat);
		var hoursPercentage = hours + minutesFloat;
		console.log("hoursPercentage: " + hoursPercentage);

		$("#NewEmployee_SalariedEmployee_HoursPerDay").val(hoursPercentage);

		var percentageWorked = 100 * (hoursPercentage / 8.4);
		console.log("percentage worked: " + percentageWorked);
		$("#NewEmployeeWorkScope").val(percentageWorked);
	});

});