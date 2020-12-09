$(function () {
	$.fn.reloadHoursPage = function (year, month, yearlyMonthlyEnum) {
		//alert("in reload hours page");
		var yeardiff = false;
		if ($('#TotalWorkingDaysInYear').attr('year') == year) {
			var amountInYear = $('#TotalWorkingDaysInYear').val();

		}
		else {
			yeardiff = true;
		}

		var url = "/ApplicationUsers/_Hours" + '?yearlyMonthlyEnum=' + yearlyMonthlyEnum + "&year=" + year + "&month=" + month + "&amountInYear=" + amountInYear
		console.log("url: " + url);

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: true,
			success: function (data) {
				$(".hours-partial").html(data);
				$('.mdb-select-workers').materialSelect();

				//$.fn.EnableMaterialSelectWorkers("#months", "select-options-months");
				//$.fn.EnableMaterialSelectWorkers("#years", "select-options-years");

				if (yeardiff) {
					$('#TotalWorkingDaysInYear').val($('#newYearAmount').val())
					$('#TotalWorkingDaysInYear').attr('year', $('#currentYear').val())
				}
				return false;
			}
		});
	};
	$('.yearlyMonthlySwitch').off('click').on("click", function (e) {
		//$('body').off("change", ".workersHoursMonths").on("change", ".workersHoursMonths", function () {
		//alert("in yearly monthly switch");
		e.preventDefault();
		var year = $(this).attr('year');
		var yearlyMonthlyEnum = $(this).attr('yearlyMonthlyEnum');
		var month = $(this).attr('month');

		$.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
	});

	$('body').off("change", ".workersHoursMonths").on("change", ".workersHoursMonths", function () {
		var year = $('.workerHoursAttr').attr('year');
		var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
		var month = $(this).val();
		$.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
		return false;
	});

	$('body').off("change", ".workersHoursYears").on("change", ".workersHoursYears", function () {
		var year = $(this).val();
		console.log("on change: " + $(this).val())
		var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
		var month = $('.workerHoursAttr').attr('month');

		$.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
		return false;
	});


})