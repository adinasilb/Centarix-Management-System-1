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
		console.log("age: " + age);
		if (isNaN(age)) {
			age = '';
		}
		$("input[name='Age']").val(age);
	});



});