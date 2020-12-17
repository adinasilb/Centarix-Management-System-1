var isEmployee = function () {
	return $("#NewEmployee_EmployeeStatusID").val() != "4";
}
var isEmployeeOnly = function () {
	return $("#NewEmployee_EmployeeStatusID").val() == "1";
}
var isUserAndIsNotEdit = function () {
	return $("#NewEmployee_EmployeeStatusID").val() == "4" && $('#myForm').hasClass('editUser') == false;
}

var isUser = function () {
	return $("#NewEmployee_EmployeeStatusID").val() == "4";
}

$('.usersForm').validate({
	rules: {
		"FirstName": "required",
		"LastName": "required",
		"CentarixID": {
			required: true,
			//number: true,
			minlength: 1,
			//integer: true
		},
		"Email": {
			email: true,
			required: true
		},
		"PhoneNumber": {
			required: true,
			minlength: 9
		},
		"NewEmployee.JobTitle": {
			required: isEmployee,
		},
		"NewEmployee.DOB": {
			required: isEmployee,
			date: true
		},
		"NewEmployee.JobCategoryTypeID": {
			selectRequired: isEmployee,
		},
		"NewEmployee.DegreeID": {
			selectRequired: isEmployee,
		},
		"NewEmployee.MaritalStatusID": {
			selectRequired: isEmployee,
		},
		"NewEmployee.CitizenshipID": {
			selectRequired: isEmployee,
		},
		"NewEmployee.IDNumber": {
			required: isEmployee,
			number: true,
			min: 1,
			integer: true
		},
		"PhoneNumber2": {
			minlength: 9
		},
		"NewEmployee.StartedWorking": {
			required: isEmployee,
		},
		"NewEmployee.TaxCredits": {
			number: true,
			integer: true
		},
		"NewEmployeeWorkScope": {
			required: isEmployeeOnly,
		},
		"NewEmployee.SalariedEmployee.HoursPerDay": {
			required: isEmployeeOnly,
			number: true
		},
		"NewEmployee.VacationDays": {
			required: isEmployeeOnly,
			number: true,
			integer: true
		},
		"Password": {
			required: isUserAndIsNotEdit,
			nonAlphaNumeric: true,
			uppercase: true,
			lowercase: true,
			containsNumber: true,
			minlength: 8,
			maxlength: 20
		},
		"SecureAppPass": {
			required: isUserAndIsNotEdit || function () {
				return $('#Password').val() != '';
			}
			//todo: are we allowing edit of secure appp password
			// validate format
		},
		"NewEmployee.EmployeeStatusID": {
			required: true,
			min: 1
		},
		//UserImage: { extension: "jpg|jpeg|png" },
		LabMonthlyLimit: {
			integer: true
		},
		LabUnitLimit: {
			integer: true
		},
		LabOrderLimit: {
			integer: true
		},
		OperationMonthlyLimit: {
			integer: true
		},
		OperationUnitLimit: {
			integer: true
		},
		OperaitonOrderLimit: {
			integer: true
		},
	},
	messages: {
		"NewEmployee.EmployeeStatusID": {
			required: "",
			min: "",
		},
	}
});
