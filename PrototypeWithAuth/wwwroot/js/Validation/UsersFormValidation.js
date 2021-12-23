$(function () {
	var isEmployee = function () {
		//console.log('is employee ' + ($("#NewEmployee_EmployeeStatusID").val() != "4"))
		return $("#Employee_EmployeeStatusID").val() != "4";
	}
	var isEmployeeOnly = function () {
		//console.log('is employee only' + $("#NewEmployee_EmployeeStatusID").val() == "1")
		return $("#Employee_EmployeeStatusID").val() == "1";
	}
	var isUserAndIsNotEdit = function () {
		//console.log('is user and not in edit' + $("#NewEmployee_EmployeeStatusID").val() == "4" && $('#myForm').hasClass('editUser') == false)
		return $("#Employee_EmployeeStatusID").val() == "4" && $('#myForm').hasClass('editUser') == false;
	}
	var isEdit = function () {
		//console.log('is edit' + $('#myForm').hasClass('editUser'))
		return $('#myForm').hasClass('editUser');
	}
	var isUser = function () {
		//console.log('is user' + $("#NewEmployee_EmployeeStatusID").val() == "4")
		return $("#Employee_EmployeeStatusID").val() == "4";
	}
	var isTrueIfNotEmpty = function (variable) {
		alert("variable: " + variable.val());
		if (variable.val() == "") {
			alert("empty string!");
			return false;
		}
		return true;
    }
	$.validator.addMethod("validTime", function (value, element) {
		var t = value.split(':');
		if (t[0].length == 1) {
			value = "0" + value;
		}
		if (t[2] != null) {
			$("#" + element.id).val(t[0] + ":" + t[1]);
		}
		var result = value.length == 0 || (/^\d\d:\d\d$/.test(value) &&
			t[0] >= 0 && t[0] < 24 &&
			t[1] >= 0 && t[1] < 60);
		return result;
	}, "Invalid time");

	$.validator.addMethod("validHours", function (value, element) {
		var result = parseFloat(value) > 0 && parseFloat(value) < 24;
		return result;
	}, "Invalid Hours");

	$('.usersForm').validate({
		normalizer: function (value) {
			return $.trim(value);
		},
		rules: {
			"Employee.FirstName": "required",
			"Employee.LastName": "required",
			//"CentarixID": {
			//	required: true,
			//	//number: true,
			//	minlength: 1,
			//	//integer: true
			//},
			"Employee.Email": {
				email: true,
				required: true,
				remote: {
					url: '/Admin/CheckUserEmailExist?isEdit=' + isEdit() + "&currentEmail=" + $(".turn-edit-on-off").attr("currentEmail"),
					type: 'POST',
					data: { "email": function () { return $("#Employee_Email").val() } },
				}
			},
			"Employee.PhoneNumber": {
				required: true,
				minlength: 9
			},
			"Employee.DOB": {
				required: isEmployee
			},
			"Employee.JobSubcategoryType.JobCategoryTypeID": {
				selectRequired: isEmployee,
			},
			"Employee.JobSubcategoryTypeID": {
				selectRequired: isEmployee,
			},
			"Employee.DegreeID": {
				selectRequired: isEmployee,
			},
			"Employee.MaritalStatusID": {
				selectRequired: isEmployee,
			},
			"Employee.CitizenshipID": {
				selectRequired: isEmployee,
			},
			"Employee.IDNumber": {
				required: isEmployee,
				number: isTrueIfNotEmpty($("#Employee_IDNumber")),
				//min: 1,
				//integer: true
			},
		//	"Employee.PhoneNumber2": {
		//		minlength: 9
		//	},
		//	"Employee.StartedWorking": {
		//		required: isEmployee,
		//	},
		//	"Employee.TaxCredits": {
		//		number: true,
		//		integer: true
		//	},
		//	"EmployeeWorkScope": {
		//		required: isEmployeeOnly,
		//	},
		//	"Employee.SalariedEmployee.HoursPerDay": {
		//		required: isEmployeeOnly,
		//		number: true
		//	},
		//	"TimeSpan-HoursPerDay": {
		//		required: isEmployeeOnly,
		//		validHours: isEmployeeOnly
		//	},
		//	"Employee.VacationDays": {
		//		required: isEmployeeOnly,
		//		number: true,
		//		integer: true
		//	},
		//	"Password": {
		//		required: isUserAndIsNotEdit,
		//		nonAlphaNumeric: true,
		//		uppercase: true,
		//		lowercase: true,
		//		containsNumber: true,
		//		minlength: 8,
		//		maxlength: 20
		//	},
		//	"Employee.SecureAppPass": {
		//		required: true
		//		//todo: are we allowing edit of secure appp password
		//		// validate format
		//	},
		//	"Employee.EmployeeStatusID": {
		//		required: true,
		//		min: 1
		//	},
		//	//UserImage: { extension: "jpg|jpeg|png" },
		//	LabMonthlyLimit: {
		//		integer: true
		//	},
		//	LabUnitLimit: {
		//		integer: true
		//	},
		//	LabOrderLimit: {
		//		integer: true
		//	},
		//	OperationMonthlyLimit: {
		//		integer: true
		//	},
		//	OperationUnitLimit: {
		//		integer: true
		//	},
		//	OperaitonOrderLimit: {
		//		integer: true
		//	},
		},
		messages: {
			"Employee.EmployeeStatusID": {
				required: "",
				min: "",
			},
			"Employee.Email": {
				remote: "email already exists"
			}
		}
	});
});