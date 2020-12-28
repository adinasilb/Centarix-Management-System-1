

$(function () {
	$('#myForm input').keydown(function (e) {
		if (e.keyCode == 13) {
			e.preventDefault();
			return false;
		}
	});
	$.each($("#myForm").validate().settings.rules, function (name, value) {
		
		if (value["required"] ) {
			$('input[name ="' + name + '"]').prev('label').first().append('*');
			
		}	
		if (value["selectRequired"]) {
			var id = $('[name = "' + name + '"]').attr('id')
			$('input[data-activates ="select-options-' + id + '"] ').parent('div').prev().first().append('*');
		}
	}
	);


	$('#myForm').data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible), [disabled]';
	$('#myForm').data("validator").settings.errorPlacement = function (error, element) {
		if (element.hasClass('select-dropdown')) {
			error.insertAfter(element);
		} else {
			error.insertAfter(element);
		}
		if (element.hasClass('employee-status')) {
			$("#validation-EmployeeStatus").removeClass("hidden");
		}
	}
	$(".cost-validation").each(function () {
		$(this).rules("add", {
			required: true,
			number: true,
			min: 1
		});
	});
	$(".supply-days-validation").each(function () {
		$(this).rules("add", {
			min: 0,
			integer: true
		});
	});

	$.validator.addMethod("nonAlphaNumeric", function (value) {
		return /^[a-zA-Z0-9]+$/.test(value) == false;
	}, "Password must contain a non alphanumeric character ");
	$.validator.addMethod("uppercase", function (value, element) {
		if (this.optional(element)) {
			return true;
		}
		return /[A-Z]/.test(value);
	}, "Must contain uppercase");
	$.validator.addMethod("lowercase", function (value, element) {
		if (this.optional(element)) {
			return true;
		}
		return /[a-z]/.test(value);
	}, "Must contain lowercase");
	$.validator.addMethod("containsNumber", function (value, element) {
		if (this.optional(element)) {
			return true;
		}
		return /[0-9]/.test(value);
	}, "Password must contain at least one number ");
	$.validator.addMethod("selectRequired", function (value, element) {
		return value != "" && value!=null;
	}, 'Field is required');
	//$.validator.addMethod("atleastOneHoursField", function (value, element) {
	//	console.log($("#NewEmployeeWorkScope").val())
	//	console.log($("#NewEmployee_SalariedEmployee_HoursPerDay").val())
	//	return ($("#NewEmployee_SalariedEmployee_WorkScope").val() != "") || ($("#NewEmployee_SalariedEmployee_HoursPerDay").val() != '0') && $("#NewEmployee_EmployeeStatusID").val() == "1";
	//}, 'Either Job Scope or Hours Per day is required');
	$.validator.addMethod("atLeastOneTerm", function (value, element) {
		console.log($("#Terms").val())
		return (($("#Paid").val() != "false" && $("#Paid").val() != "") || ($("#Terms").val() != "" && $("#Terms").val() != null) || ($("#Installments").val()!=''&&parseInt($("#Installments").val()) > 0)) ;
	}, 'Must choose one type of payment');

	$.validator.addMethod("integer", function (value, element) {
		return isInteger(value) || value == '';
	}, 'Field must be an integer');

	$('#myForm .mdb-select').change(function () {
		if ($(this).rules()) {
			$(this).valid();
		}
	});

	function isInteger(n) {
		n = parseFloat(n)
		return n === +n && n === (n | 0);
	}

	$('.modal').on('change', '.mdb-select', function () {
		if ($(this).rules()) {
			$(this).valid();
		}

	});
	$('#myForm input').focusout(function (e) {
		console.log("validating input...");
		$("#myForm").data("validator").settings.ignore = "";
		$('.error').addClass("beforeCallValid");
		if ($('#myForm').valid()) {
			$('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
		} else {
			$(".error:not(.beforeCallValid)").addClass("afterCallValid")
			$(".error:not(.beforeCallValid)").removeClass("error")
			$("label.afterCallValid").remove()
			$(".error").removeClass('beforeCallValid')
			$(".afterCallValid").removeClass('error')
			$(".afterCallValid").removeClass('afterCallValid')
			if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
				$('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
			}
		}
		$("#myForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible), [disabled]';
	});

	$('.next-tab').click(function () {
		if ($(this).hasClass('request-price')) {
			$('#Request_UnitTypeID').rules("remove", "selectRequired");
		}

		//change previous tabs to accessible --> only adding prev-tab in case we need to somehow get it after
		$(this).parent().prev().find(".next-tab").addClass("prev-tab");

		if (!$(this).hasClass("prev-tab")) {
			var valid = $("#myForm").valid();

			console.log("valid tab" + valid)
			if (!valid) {
				$('.next-tab').prop("disabled", true);
			}
			else {
				$('.next-tab').prop("disabled", false);

			}
			//work around for now - because select hidden are ignored
			if ($(this).hasClass('request-price')) {
				$('#Request_UnitTypeID').rules("add", "selectRequired");
			}
		}


	});
	$('#myForm').submit(function (e) {
		//alert("validate form");
		$(this).data("validator").settings.ignore = "";
		var valid = $(this).valid();
		console.log("valid form: " + valid)
		if (!valid) {
			e.preventDefault();
			if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
				$('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
			}

		}
		else {
			$('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
		}
		$(this).data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
	});
	$('.modal #myForm').submit(function (e) {
		//alert("validate form");
		$(this).data("validator").settings.ignore = "";
		var valid = $(this).valid();
		console.log("valid form: " + valid)
		if (!valid) {
			e.preventDefault();
			if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
				$('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
			}

		}
		else {
			$('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
		}
		$(this).data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
	});




});
