$(function () {
	$.fn.reloadHoursPage = function (year, month, yearlyMonthlyEnum) {
		alert("in reload hours page fx");
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
			cache: false,
			success: function (data) {
				$(".hours-partial").html(data);
				//$('.mdb-select-workers').materialSelect();

				$.fn.EnableMaterialSelectWorkers("#months", "select-options-months");
				$.fn.EnableMaterialSelectWorkers("#years", "select-options-years");

				if (yeardiff) {
					$('#TotalWorkingDaysInYear').val($('#newYearAmount').val())
					$('#TotalWorkingDaysInYear').attr('year', $('#currentYear').val())
				}
				return false;
			}
		});
	};
	$('.yearlyMonthlySwitch').off('click').click(function (e) {
		e.preventDefault();
		var year = $(this).attr('year');
		var yearlyMonthlyEnum = $(el).attr('yearlyMonthlyEnum');
		var month = $(this).attr('month');

		$.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
		return false;
	});

	$('.workersHoursMonths').off('change').change(function () {
		var year = $('.workerHoursAttr').attr('year');
		var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
		var month = $(this).val();
		$.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
		return false;
	});

	$('body').off("change", ".workersHoursYears").on("change", ".workersHoursYears", function () {
	    alert("workers hours years changed from body");
	    var year = $(this).val();
	    console.log("on change: " + $(this).val())
	    var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
		var month = $('.workerHoursAttr').attr('month');

		$.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
		return false;
	});

	//$(".workersHoursYears input").off("click").on("click", function () {
	//	alert("workers hours years input changed");
	//	var year = $(this).val();
	//	console.log("on change: " + $(this).val())
	//	var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
	//	var month = $('.workerHoursAttr').attr('month');

	//	$.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
	//});

	//$('.workersHoursYears').off('change').change(function () {
	//    alert("workers hours years changed");
	//    var year = $(this).val();
	//    console.log("on change: "+$(this).val())
	//    var yearlyMonthlyEnum = $('.workerHoursAttr').attr('yearlyMonthlyEnum');
	//    var month = $('.workerHoursAttr').attr('month');

	//    $.fn.reloadHoursPage(year, month, yearlyMonthlyEnum);
	//});

	$.fn.EnableMaterialSelectWorkers = function (selectID, dataActivates) {
		var selectedIndex = $('#' + dataActivates).find(".active").index();

		selectedIndex = selectedIndex - 1;

		$(selectID).destroyMaterialSelect();
		$(selectID).prop("disabled", false);
		$(selectID).prop('selectedIndex', selectedIndex);
		$(selectID).removeAttr("disabled")
		$('[data-activates="' + dataActivates + '"]').prop('disabled', false);
		$(selectID).materialSelect();
		$('.open-document-modal').attr("data-val", true);
	}

	jQuery.fn.extend({
		destroyMaterialSelect: function () {
			return this.each(function () {
				let wrapper = $(this).parent();
				let core = wrapper.find('select');
				wrapper.after(core.removeClass('initialized').prop('outerHTML'));
				wrapper.remove();
			});
		}
	});

})