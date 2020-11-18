$(function () {
	$(".change-currency").on("change", function () {
		console.log("change currency");
		$.fn.CallSummaryTablesPartial();
	});
	$(".change-year").on("change", function () {
		console.log("change currency");
		$.fn.CallSummaryTablesPartial();
	});

	$.fn.CallSummaryTablesPartial = function () {
		var year = $("#select-years").val();
		var currency = $("#select-currency").val();
		console.log("year: " + year + " , currency: " + currency);

		var url = "/Expenses/_SummaryTables?currencyEnum=" + currency + "&year=" + year;

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: false,
			success: function (data) {
				$("#summaryTable").empty();
				$("#summaryTable").html(data);
			}
		});
	};
});