$(function () {
	$("#select-years").off("change").on("change", function () {
		$.fn.GetStatisticsWorkerChartPartial();
	});

	$("#CategoryTypesSelected").off("change").on("change", function () {
		$.fn.GetStatisticsWorkerChartPartial()
	});

	$("#Months").off("change").on("change", function () {
		$.fn.GetStatisticsWorkerChartPartial()
	});

	$.fn.GetStatisticsWorkerChartPartial = function () {
		var years = [];
		years = $("#select-years").val();
		var months = [];
		months = $("#Months").val();
		var catTypes = [];
		catTypes = $("#CategoryTypesSelected").val();

		var url = "/Expenses/_StatisticsWorkerChart";

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			traditional: true,
			cache: false,
			data: { CategoryTypeIDs: catTypes, Months: months, Years: years },
			success: function (data) {
				$(".statistics-worker-chart").empty();
				$(".statistics-worker-chart").html(data);
			}
		});
	};
});