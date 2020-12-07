$(function () {
	$("#select-years").off("change").on("change", function () {
		$.fn.GetStatisticsVendorChartPartial();
	});

	$("#CategoryTypesSelected").off("change").on("change", function () {
		$.fn.GetStatisticsVendorChartPartial()
	});

	$("#Months").off("change").on("change", function () {
		$.fn.GetStatisticsVendorChartPartial()
	});

	$.fn.GetStatisticsVendorChartPartial = function () {
		var years = [];
		years = $("#select-years").val();
		var months = [];
		months = $("#Months").val();
		var catTypes = [];
		catTypes = $("#CategoryTypesSelected").val();

		var url = "/Expenses/_VendorsTable";

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			traditional: true,
			cache: false,
			data: { CategoryTypes: catTypes, Months: months, Years: years },
			success: function (data) {
				$(".statistics-vendor-chart").empty();
				$(".statistics-vendor-chart").html(data);
			}
		});
	};
});