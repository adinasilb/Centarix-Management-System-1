

$(function () {
	$('#myForm input').keydown(function (e) {
		if (e.keyCode == 13) {
			e.preventDefault();
			return false;
		}
	});

	$.fn.AppendAsteriskToRequired = function() {
		 $('input').each(function () {
			var str = $(this).prev('label').first().text();
			var char = str.slice(-1);
			if (char == "*") {
				$(this).prev('label').first().html(str.slice(0, -1));
			}
			var str2= $(this).parent('div').prev('label').first().text();
			var char2 = str2.slice(-1);
			if (char2 == "*") {
				$(this).parent('div').prev('label').first().html(str2.slice(0, -1));
			}
		 });
		$.each($("#myForm").validate().settings.rules, function (name, value) {
			var rule = value["required"];
			if($.isFunction(rule))
			{
				rule=rule();
			}
			if (rule) {
				var str = $('input[name ="' + name + '"]').prev('label').first().text();
				var char = str.slice(-1);
				if (char == "*") {
					$('input[name ="' + name + '"]').prev('label').first().html(str.slice(0,-1));
                }
				$('input[name ="' + name + '"]').prev('label').first().append('*');

			}
			var rule2 = value["selectRequired"];
			if($.isFunction(rule2))
			{
				rule2=rule2();
			}
			if (rule2 == true) {
				var id = $('[name = "' + name + '"]').attr('id')
				var str = $('input[data-activates ="select-options-' + id + '"]').parent('div').prev('label').first().text();
				var char = str.slice(-1);
				if (char == "*") {
					$('input[data-activates ="select-options-' + id + '"]').parent('div').prev('label').first().html(str.slice(0, -1));
				}
				$('input[data-activates ="select-options-' + id + '"]').parent('div').prev('label').first().append('*');
			}
		}
		);
    }
	
	$.fn.AppendAsteriskToRequired();

	$('#myForm').data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible), [disabled], #-error';
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

	$.validator.addMethod('mindate', function (v, el, minDate) {
	if (this.optional(el)) {
		return true;
	}
		
	var val = $(el).val();
	console.log('val: ' + val);
	//val = val.split("/").reverse().join("-");
	var selectedDate = moment(val, "D MMM YYYY").toDate();
	console.log("selected date"+selectedDate)
	minDate = new Date(minDate.setHours(0));
	minDate = new Date(minDate.setMinutes(0));
	minDate = new Date(minDate.setSeconds(0));
	minDate = new Date(minDate.setMilliseconds(0));
	return selectedDate >= minDate;
}, 'Please select a valid date');

	$.validator.addMethod('maxDate', function (v, el, minDate) {
	if (this.optional(el)) {
		return true;
	}
	var val = $(el).val();
	//val = val.split("/").reverse().join("/");
	console.log('val: ' + val);
	var selectedDate = moment(val,"D MMM YYYY").toDate();
	console.log("selected date"+selectedDate)
	selectedDate = new Date(selectedDate.setHours(0));
	selectedDate = new Date(selectedDate.setMinutes(0));
	selectedDate = new Date(selectedDate.setSeconds(0));
	selectedDate = new Date(selectedDate.setMilliseconds(0));
	minDate = new Date(minDate.setHours(0));
	minDate = new Date(minDate.setMinutes(0));
	minDate = new Date(minDate.setSeconds(0));
	minDate = new Date(minDate.setMilliseconds(0));
			console.log("min date" + minDate)
	return selectedDate <= minDate;
}, 'Please select a valid date');
	$.validator.addMethod('greaterThan', function (value, el, param) {
		return value > param;
	}, 'Please enter a value greater than {0}');
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

	$.validator.addMethod("notEqualTo",
    function (value, element, param) {
        var notEqual = true;
        value = $.trim(value);
        for (i = 0; i < param.length; i++) {

            var checkElement = $(param[i]);
            var success = !$.validator.methods.equalTo.call(this, value, element, checkElement);
            // console.log('success', success);
            if(!success)
                notEqual = success;
        }

        return this.optional(element) || notEqual;
    },
    "Please enter a diferent value."
);
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


	$('#myForm input').change(function (e) {
		console.log("validating input...");
		$("#myForm").data("validator").settings.ignore = "";
		$('.error').addClass("beforeCallValid");
		if ($('#myForm').valid()) {
			$('.activeSubmit').removeClass('disabled-submit')
		} else {
			$(".error:not(.beforeCallValid)").addClass("afterCallValid")
			$(".error:not(.beforeCallValid)").removeClass("error")
			$("label.afterCallValid").remove()
			$(".error").removeClass('beforeCallValid')
			$(".afterCallValid").removeClass('error')
			$(".afterCallValid").removeClass('afterCallValid')
			if (!$('.activeSubmit').hasClass('disabled-submit')) {
				$('.activeSubmit').addClass('disabled-submit')
			}
		}
		$("#myForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible), [disabled]';
	});

	$('.next-tab').off("click").click(function () {
		var clickedElement= $(this);
		
		if(!$(this).hasClass("current-tab"))
		{
			if ($(this).hasClass('request-price') ) {
				$('#unitTypeID').rules("remove", "selectRequired");
			}

			if ($(this).hasClass('order-tab-link') ) {
				$('.activeSubmit').removeClass('disabled-submit')
			}

			//change previous tabs to accessible --> only adding prev-tab in case we need to somehow get it after

			if (!$(this).hasClass("prev-tab")) {
				var valid = $("#myForm").valid();

				console.log("valid tab" + valid)
				if (!valid) {
					$(this).prop("disabled", true);
				}
				else {
						var currentTab = $(".current-tab")
						$(currentTab).removeClass("current-tab")
						$(this).prop("disabled", false);
						$(this).addClass("current-tab");
						$(".next-tab").removeClass("prev-tab");
						$('.next-tab').each(function(index, element){
			

							if($(clickedElement).parent("li").index() > $(element).parent("li").index())
							{
								//alert("true")
								$(element).addClass("prev-tab");
							}

						});
					}
			
			}
			else
			{
			    var currentTab = $(".current-tab")
				$(currentTab).removeClass("current-tab")
				$(this).prop("disabled", false);
				$(this).addClass("current-tab");
				$(".next-tab").removeClass("prev-tab");
				$('.next-tab').each(function(index, element){
	

					if($(clickedElement).index() > $(element).parent("li").index())
					{
						//alert("true")
						$(element).addClass("prev-tab");
					}

				});

			}
			//work around for now - because select hidden are ignored
				if ($(this).hasClass('request-price')) {
					$('#unitTypeID').rules("add", "selectRequired");
				}
		}
		

	});

	$.fn.isBefore= function(sel){
		 return 
	};
	$('#myForm, .modal #myForm').submit(function (e) {
		//alert("validate form");
		$(this).data("validator").settings.ignore = "";
		var valid = $(this).valid();
		console.log("validate.js says valid form: " + valid)
		if (!valid) {
			e.preventDefault();
			if (!$('.activeSubmit').hasClass('disabled-submit')) {
				$('.activeSubmit').addClass('disabled-submit')
			}

		}
		else {
			$('.activeSubmit ').removeClass('disabled-submit')
		}
		$(this).data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';
	});

	$.validator.addMethod("fileRequired", function (value, element) {
		console.log("in file required")
		return $(element).hasClass("contains-file");
	}, 'Must upload a file before submitting');

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

	$.validator.addMethod("validTimeWithSeconds", function (value, element) {
	var t = value.split(':');
	if (t[0].length == 1) {
		value = "0" + value;
	}
	if(t[0]==undefined)
	{
		t[0]="00";
	}
	if(t[1]==undefined)
	{
		t[1]="00";
	}
	if(t[2]==undefined)
	{
		t[2]="00";
	}
	$("#"+element.id).val(t[0] + ":" + t[1]+":" + t[2]);

	var result = value.length == 0 || (/^\d\d:\d\d:\d\d$/.test(value) &&
		t[0] >= 0 && t[0] < 24 &&
		t[1] >= 0 && t[1] < 60 &&
		t[2] >= 0 && t[2] < 60  
		);
	return result;
	}, "Invalid time");
});
