
$(function () {


	$("#NewEmployee_DOB").off("change").on("change", function () {
		var val = $(this).val();
		val = val.split("/").reverse().join("-");
		var DOB = moment(val);
		var age = 0;
		//var Today = new Date().toJSON().slice(0, 10).replace(/-/g, '-');
		console.log("dob: " + DOB);
		var today = new Date();
		console.log("today: " + today);
		age = Math.floor((today - DOB) / (365.25 * 24 * 60 * 60 * 1000));
		//alert(age);
		if (isNaN(age)) {
			age = '';
		}
		$("input[name='Age']").val(age);
		$("input[name='NewEmployee.DOB_submit']").val(val)
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
		var hoursPercentage = parseFloat(hours + minutesFloat);
		console.log("hoursPercentage: " + hoursPercentage);

		SetHiddenHoursPerDay(hoursPercentage);

		var percentageWorked = parseFloat(100 * (hoursPercentage / 8.4)).toFixed(2);
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
		console.log("mins: "+mins)
		if (mins <10) { mins = '0' + mins }
	
		//var timespanHours = (`${hours}:${mins}`);
		var timespanHours = hours + ":" + mins;
		console.log("timespanHours: " + timespanHours);

		$("#TimeSpan-HoursPerDay").val(timespanHours);
	});

	function SetHiddenHoursPerDay(hoursPercentage) {
		$("#NewEmployee_SalariedEmployee_HoursPerDay").val(hoursPercentage);
	};

	$('.employee-status-radio').off("click").on("click", function () {

		var val = $(this).val();
		$('#NewEmployee_EmployeeStatusID').val(val)
		$("#validation-EmployeeStatus").addClass("hidden");
		if (val == "4") {
			$('.only-employee').removeClass("error");
			$('.only-employee').addClass("hidden");
			$('.only-employee').addClass("m-0");
		}
		else {
			$('.only-employee').removeClass("hidden");
			$('.only-employee').removeClass("m-0");
        }

		$.fn.AppendAsteriskToRequired();

		var centarixIDInput = $('#CentarixID');
		if (val == $("#OriginalStatusID").val()) {
			centarixIDInput.val($("#OriginalStatusID").attr("centarixID"));
		}
		else {
			$.ajax({
				async: true,
				url: "/Admin/GetProbableNextCentarixID?StatusID=" + val,
				type: 'GET',
				cache: true,
				success: function (data) {
					console.log("original status id: " + $("#OriginalStatusID").attr("CentarixID"));
					console.log("data " + data);
					var orginalID =$("#OriginalStatusID").attr("CentarixID")
					if(orginalID==undefined)
					{
						orginalID='';
					}
					var showCentarixID = orginalID + data;
					console.log("showCentarixID: " + showCentarixID);
					centarixIDInput.val(showCentarixID);
				}
			});
		}


	});
	$("body, .modal").off("change", ".job-category").on("change", ".job-category", function () {
		console.log("in on change before fx");
		//$.fn.changeProject($(this).val());
		console.log("job-category was changed");
		var categoryID = $(this).val();
		var url = "/Admin/GetJobSubcategoryTypeList";

		var jobSubcategory = $("#job-subcategory");

		$.getJSON(url, { JobCategoryTypeID: categoryID }, function (data) {
			var item1 = "<option value=''>Select Sub Category Type</option>";
			jobSubcategory.empty();
			jobSubcategory.append(item1);
			$.each(data, function (i, subCategory) {
				item = '<option value="' + subCategory.jobSubcategoryTypeID + '">' + subCategory.description + '</option>'
				jobSubcategory.append(item);
			});
			//jobSubcategory.destroyMaterialSelect();
			//jobSubcategory.prop("disabled", false);
			//jobSubcategory.removeAttr("disabled");
			//$('[data-activates="select-options-job-subcategory"]').prop('disabled', false);
			jobSubcategory.materialSelect();
			return false;
		});
		return false;
	});
});